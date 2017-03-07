using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using CoffeePotDevice.Services;
using CoffeePotDevice.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.ViewManagement;

namespace CoffeePotDevice.ViewModels
{
  public class MainPageViewModel : ViewModelBase
  {

    private Models.FullScreenStates fullScreenState;

    public Models.FullScreenStates FullScreenState
    {
      get { return fullScreenState; }
      set { Set(ref fullScreenState, value); }
    }


    private SettingsService _settings;

    public string DeviceId
    {
      get { return _settings.CoffePotDeviceId; }
    }


    IoTHubDeviceService iotHubDeviceService;

    private ObservableCollection<String> messagesReceived;

    public ObservableCollection<String> MessagesReceived
    {
      get { return messagesReceived; }
      set
      {
        Set(ref messagesReceived, value);
      }
    }

    private ObservableCollection<String> messagesSent;

    public ObservableCollection<String> MessagesSent
    {
      get { return messagesSent; }
      set
      {
        Set(ref messagesSent, value);
      }
    }

    public void Rebind()
    {
      RaisePropertyChanged(nameof(DeviceId));
    }


    public MainPageViewModel()
    {
      _settings = SettingsService.Instance;
      _settings.PropertyChanged += _settings_PropertyChanged;

      MessagesReceived = new ObservableCollection<String>();
      MessagesSent = new ObservableCollection<String>();
    }

    private void _settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      RaisePropertyChanged();
    }

    DelegateCommand toggleFullScreenCommand;
    public DelegateCommand ToggleFullScreenCommand =>
      toggleFullScreenCommand ?? (
        toggleFullScreenCommand = new DelegateCommand(() => 
        {
          ApplicationView currentView = ApplicationView.GetForCurrentView();
          if (currentView.IsFullScreenMode)
          {
            currentView.ExitFullScreenMode();
          }
          else
          {
            currentView.TryEnterFullScreenMode();
          }
          UpdateFullScreenStateProperty();
        }, 
          () => true)
      );

    #region Play Sounds

    DelegateCommand<String> playSoundCommand;

    public DelegateCommand<String> PlaySoundCommand
       => playSoundCommand ?? (playSoundCommand = new DelegateCommand<String>(async (path) => await PlaySoundAsync(path), (path) => true));

    private async Task PlaySoundAsync(String Path)
    {
      await SoundService.PlayAudioFileAsync(Path);
    }

    #endregion Play Sounds

    #region Navigation

    DelegateCommand<String> navigateCommand;

    public DelegateCommand<String> NavigateCommand
       => navigateCommand ?? (navigateCommand = new DelegateCommand<String>((target) => NavigateToTarget(target), (target) => true));

    private void NavigateToTarget(String Target)
    {
      try
      {
        Type targetType = Type.GetType(Target);
        NavigationService.Navigate(targetType, 0);
      }
      catch
      {
        throw;
      }
    }

    private void UpdateFullScreenStateProperty()
    {
      FullScreenState = ApplicationView.GetForCurrentView().IsFullScreenMode ? FullScreenStates.Expanded : FullScreenStates.Collapsed;

    }

    public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
    {
      await InitIoTHubDeviceService();
      UpdateFullScreenStateProperty();
      Rebind();
    }

    #endregion Navigation

    #region IoT Hub Device Functionality

    private async Task InitIoTHubDeviceService()
    {
      string deviceConnectionString = _settings.CoffePotDeviceConnectionString;
      if (!String.IsNullOrWhiteSpace(deviceConnectionString))
      {
        if(iotHubDeviceService == null)
        {
          iotHubDeviceService = new Services.IoTHubDeviceService(deviceConnectionString);

          iotHubDeviceService.MessageReceived += IotHubDeviceService_MessageReceived;
          iotHubDeviceService.BadMessageReceived += IotHubDeviceService_BadMessageReceived;
          iotHubDeviceService.UnknownCommandReceived += IotHubDeviceService_UnknownCommandReceived;
          iotHubDeviceService.PingCommandReceived += IotHubDeviceService_PingCommandReceived;
          iotHubDeviceService.BrewCommandReceived += IotHubDeviceService_BrewCommandReceived;
        }
        else
        {
          iotHubDeviceService.DeviceConnectionString = deviceConnectionString;
        }

        if (!iotHubDeviceService.IsListening)
        {
          iotHubDeviceService.StartListening();
        }
        await Task.CompletedTask;
      }
    }

    private async void SendDeviceMessage(string Team, string correlationId, string MessageText)
    {
      DeviceToCloudMessage deviceMessage = new DeviceToCloudMessage() { Team = Team, MessageText = MessageText };
      Debug.WriteLine(MessageText);
      MessagesSent.Insert(0, MessageText);
      await iotHubDeviceService.SendDeviceToCloudMessagesAsync(deviceMessage, correlationId);
    }

    private async void IotHubDeviceService_MessageReceived(object sender, Models.MessageEventArgs e)
    {
      MessagesReceived.Insert(0, e.MessageString);
      //Debug.WriteLine(String.Format("Received message: {0}", e.MessageString));
    }

    private async void IotHubDeviceService_BadMessageReceived(object sender, Models.MessageEventArgs e)
    {
      await SoundService.PlayAudioFileAsync("allyourbase.wav");
      string msg = String.Format("Malformed message received: {0}", e.MessageString);
      //Debug.WriteLine(msg);
      SendDeviceMessage("unknown", e.Message.MessageId, msg);
    }

    private async void IotHubDeviceService_UnknownCommandReceived(object sender, Models.CommandEventArgs e)
    {
      await SoundService.PlayAudioFileAsync("laserwoowoo.wav");
      string msg = String.Format("Unknown command received from Team {0}: {1}", e.CommandMessage.Team, e.CommandMessage.Command);
      SendDeviceMessage(e.CommandMessage.Team, e.Message.MessageId, msg);
    }

    private async void IotHubDeviceService_PingCommandReceived(object sender, Models.PingCommandEventArgs e)
    {
      await SoundService.PlayAudioFileAsync("ping.wav");
      string msg = String.Format("Ping response for {0}: {1}", e.CommandMessage.Team, e.Payload);
      SendDeviceMessage(e.CommandMessage.Team, e.Message.MessageId, msg);
    }

    private async void IotHubDeviceService_BrewCommandReceived(object sender, Models.BrewCommandEventArgs e)
    {
      await SoundService.PlayAudioFileAsync("brew.wav");
      string msg = String.Format("Brewing Coffee for {0}: {1}",e.CommandMessage.Team, e.Payload);
      SendDeviceMessage(e.CommandMessage.Team, e.Message.MessageId, msg);
    }


    #endregion IoT Hub Device Functionality
  }
}

