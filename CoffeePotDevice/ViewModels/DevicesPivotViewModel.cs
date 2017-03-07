using CoffeePotDevice.Models;
using CoffeePotDevice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;

namespace CoffeePotDevice.ViewModels
{
  public class DevicesPivotViewModel : ViewModelBase
  {

    private DeviceEntity editDevice;

    public DeviceEntity EditDevice
    {
      get { return editDevice; }
      set { Set(ref editDevice, value); }
    }


    private DeviceEntity selectedDevice;

    public DeviceEntity SelectedDevice
    {
      get { return selectedDevice; }
      set
      {
        Set(ref selectedDevice, value);
        OpenUpdateDeviceDialogCommand.RaiseCanExecuteChanged();
        DeleteDeviceFromRegistryCommand.RaiseCanExecuteChanged();
        EntangleDeviceCommand.RaiseCanExecuteChanged();
      }
    }

    private string deviceDialogTitle;

    public string DeviceDialogTitle
    {
      get { return deviceDialogTitle; }
      set { Set(ref deviceDialogTitle, value); }
    }

    private bool isCreateDialog;

    public bool IsCreateDialog
    {
      get { return isCreateDialog; }
      set { Set(ref isCreateDialog, value); }
    }


    Services.SettingsService _settings;

    public DevicesPivotViewModel()
    {
      if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
      {
        //design time
      }
      else
      {
        _settings = Services.SettingsService.Instance;
        _settings.SettingsChanged += _settings_SettingsChanged;
      }
    }

    private void _settings_SettingsChanged(object sender, Template10.Common.EventArgs<string> e)
    {
      if (e.Value == "IoTHubManagementConnectionString")
      {
        GetDevicesCommand.RaiseCanExecuteChanged();
        OpenCreateDeviceDialogCommand.RaiseCanExecuteChanged();
      }
    }

    public List<DeviceEntity> devices;
    public List<DeviceEntity> Devices
    {
      get { return devices; }
      set
      {
        Set(ref devices, value);
      }
    }

    DelegateCommand<String> getDevicesCommand;

    public DelegateCommand<String> GetDevicesCommand
       => getDevicesCommand ?? (
        getDevicesCommand = new DelegateCommand<String>(
          async (param) => {
            if (String.IsNullOrEmpty(_settings.IoTHubManagementConnectionString))
            {
              MessageDialog dlg = new MessageDialog("You must provide an \"IoT Hub Management Connection String\" on the \"Settings\" tab first.");
              dlg.ShowAsync();
              return;
            }
            await GetDevices();
          }, 
          (param) => true));

    private async Task GetDevices()
    {
      string conString = _settings.IoTHubManagementConnectionString;

      if (!String.IsNullOrWhiteSpace(conString))
      {
        if (Devices != null)
        {
          Devices.Clear();
          Devices = null;
        }
        Devices = await IoTHubRegistryService.GetDevices(conString, "");
      }
    }

    private bool deviceDialogIsOpen;

    public bool IsDeviceDialogOpen
    {
      get { return deviceDialogIsOpen; }
      set
      {
        Set(ref deviceDialogIsOpen, value);
      }
    }

    private DelegateCommand generateDeviceIdCommand;
    public DelegateCommand GenerateDeviceIdCommand =>
      generateDeviceIdCommand ?? (generateDeviceIdCommand = new DelegateCommand(() => {
        EditDevice.Id = IoTHubRegistryService.GenerateDeviceId("coffeepot-");
      }, () => true));

    private DelegateCommand<String> generateDeviceKey;
    public DelegateCommand<String> GenerateDeviceKey =>
      generateDeviceKey ?? (generateDeviceKey = new DelegateCommand<String>((target) =>
      {
        if (target.ToLower().Trim().Contains("primary"))
        {
          EditDevice.PrimaryKey = IoTHubRegistryService.GenerateSymmetricKey();
        }
        else
        {
          EditDevice.SecondaryKey = IoTHubRegistryService.GenerateSymmetricKey();
        }
      }
      , (target) => true));


    DelegateCommand<String> openCreateDeviceDialogCommand;

    public DelegateCommand<String> OpenCreateDeviceDialogCommand
       => openCreateDeviceDialogCommand ?? (
        openCreateDeviceDialogCommand = new DelegateCommand<String>((param) => {
          if (String.IsNullOrEmpty(_settings.IoTHubManagementConnectionString))
          {
            MessageDialog dlg = new MessageDialog("You must provide an \"IoT Hub Management Connection String\" on the \"Settings\" tab first.");
            dlg.ShowAsync();
            return;
          }
          IsCreateDialog = true;
          DeviceDialogTitle = "Create New Device";
          EditDevice = IoTHubRegistryService.CreateNewDeviceEntity(true, param);
          IsDeviceDialogOpen = true;
        }, (param) => true));


    DelegateCommand<String> openUpdateDeviceDialogCommand;

    public DelegateCommand<String> OpenUpdateDeviceDialogCommand
       => openUpdateDeviceDialogCommand ?? (
        openUpdateDeviceDialogCommand = new DelegateCommand<String>((param) =>
        {
          IsCreateDialog = false;
          DeviceDialogTitle = "Update Device";
          EditDevice = SelectedDevice.Copy();
          IsDeviceDialogOpen = true;
        }, (param) => SelectedDevice != null));

    private DelegateCommand saveDeviceToRegistryCommand;
    public DelegateCommand SaveDeviceToRegistryCommand =>
      saveDeviceToRegistryCommand ?? (
        saveDeviceToRegistryCommand = new DelegateCommand(async () => {
          try
          {
            DeviceEntity outputDevice;

            if (!isCreateDialog)
            {
              string message = "Are you sure you want to update this device?  If the device was created by another user you could be negatively impacting them, and other users that depend on that device.  Only update this device if it is approprite.  So, once again, are you sure you want to update this device?";
              string title = "Device Update Confirmation";
              int result = await ConfirmationDailog(message,title);

              if (result == 1)
              {
                return;
              }
            }

            outputDevice = await IoTHubRegistryService.SaveDeviceEntityToRegistry(IsCreateDialog, _settings.IoTHubManagementConnectionString, "", EditDevice);
            await GetDevices();
            SelectedDevice = Devices.FirstOrDefault((d) => d.Id == outputDevice.Id);
            IsDeviceDialogOpen = false;
            EditDevice = null;
          }
          catch(Exception ex)
          {
            string message = $"An {ex.GetType().Name} occurred when trying to save the Device. {ex.Message}";
            MessageDialog dlg = new MessageDialog(message);
            dlg.ShowAsync();
          }
        },()=>true)
      );

    private DelegateCommand deleteDeviceFromRegistryCommand;
    public DelegateCommand DeleteDeviceFromRegistryCommand =>
      deleteDeviceFromRegistryCommand ?? (
        deleteDeviceFromRegistryCommand = new DelegateCommand(async () => {
          try
          {

            string message = "Are you sure you want to delete this device?  If the device was created by another user you could be negatively impacting them, and other users that depend on that device.  Only delete this device if it is approprite.  So, once again, are you sure you want to delete this device?";
            string title = "Device Deletion Confirmation";
            int result = await ConfirmationDailog(message,title);
            if (result == 1)
            {
              return;
            }

            await IoTHubRegistryService.DeleteDeviceEntityFromRegistry(_settings.IoTHubManagementConnectionString,SelectedDevice);
            await GetDevices();
            SelectedDevice = (Devices.Count >= 1) ? Devices[0] : null;
            EditDevice = null;

          }
          catch (Exception ex)
          {
            string message = $"An {ex.GetType().Name} occurred when trying to delete the Device. {ex.Message}";
            MessageDialog dlg = new MessageDialog(message);
            dlg.ShowAsync();
          }
        },
        ()=> SelectedDevice != null)
      );

    private DelegateCommand entangleDeviceCommand;
    public DelegateCommand EntangleDeviceCommand =>
      entangleDeviceCommand ?? (
        entangleDeviceCommand = new DelegateCommand(() => {
          _settings.CoffePotDeviceId = SelectedDevice.Id;
          _settings.CoffePotDeviceConnectionString = SelectedDevice.ConnectionString;
        }, 
        () => SelectedDevice != null)
      );

    private static async Task<int> ConfirmationDailog(string message, string title)
    {
      MessageDialog dlg = new MessageDialog(message, title);
      dlg.Commands.Add(new UICommand("Yes") { Id = 0 });
      dlg.Commands.Add(new UICommand("No") { Id = 1 });

      dlg.DefaultCommandIndex = 1;
      dlg.CancelCommandIndex = 1;

      var result = await dlg.ShowAsync();
      return (int)result.Id;
    }

    private DelegateCommand cancelDeviceEditCommand;
    public DelegateCommand CancelDeviceEditCommand =>
      cancelDeviceEditCommand ?? (
        cancelDeviceEditCommand = new DelegateCommand(() => {
          DeviceDialogTitle = "";
          IsDeviceDialogOpen = false;
          EditDevice = null;
        }, () => true)
      );


  }
}
