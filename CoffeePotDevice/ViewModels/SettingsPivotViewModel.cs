using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace CoffeePotDevice.ViewModels
{
  public class SettingsPivotViewModel : ViewModelBase
  {
    Services.SettingsService _settings;

    public SettingsPivotViewModel()
    {
      if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
      {
        //design time
      }
      else
      {
        _settings = Services.SettingsService.Instance;
      }
    }

    public string IoTHubManagementConnectionString
    {
      get { return _settings.IoTHubManagementConnectionString; }
      set
      {
        _settings.IoTHubManagementConnectionString = value;
        base.RaisePropertyChanged();
      }
    }

    public string CoffePotDeviceId
    {
      get { return _settings.CoffePotDeviceId; }
      set
      {
        _settings.CoffePotDeviceId = value;
        base.RaisePropertyChanged();
      }
    }

    public string CoffePotDeviceConnectionString
    {
      get { return _settings.CoffePotDeviceConnectionString; }
      set
      {
        _settings.CoffePotDeviceConnectionString = value;
        base.RaisePropertyChanged();
      }
    }

    public void Rebind()
    {
      RaisePropertyChanged(nameof(IoTHubManagementConnectionString));
      RaisePropertyChanged(nameof(CoffePotDeviceId));
      RaisePropertyChanged(nameof(CoffePotDeviceConnectionString));
    }
  }

}
