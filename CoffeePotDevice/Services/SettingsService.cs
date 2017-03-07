using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;

namespace CoffeePotDevice.Services
{

  class SettingsService : BindableBase
  {

    public event EventHandler<EventArgs<String>> SettingsChanged;

    public static SettingsService Instance { get; } = new SettingsService();

    Template10.Services.SettingsService.SettingsHelper _helper;

    private SettingsService()
    {
      _helper = new Template10.Services.SettingsService.SettingsHelper();
    }

    public string IoTHubManagementConnectionString
    {
      get { return _helper.Read<string>(nameof(IoTHubManagementConnectionString), ""); }
      set
      {
        _helper.Write(nameof(IoTHubManagementConnectionString), value);
        NotifySettingsChanged();
      }
    }

    public string CoffePotDeviceId
    {
      get { return _helper.Read<string>(nameof(CoffePotDeviceId), ""); }
      set
      {
        _helper.Write(nameof(CoffePotDeviceId), value);
        RaisePropertyChanged();
        NotifySettingsChanged();
      }
    }

    public string CoffePotDeviceConnectionString
    {
      get { return _helper.Read<string>(nameof(CoffePotDeviceConnectionString), ""); }
      set
      {
        _helper.Write(nameof(CoffePotDeviceConnectionString), value);
        RaisePropertyChanged();
        NotifySettingsChanged();
      }
    }

    private void NotifySettingsChanged([CallerMemberName] string Setting = "")
    {
      if(SettingsChanged != null)
      {
        SettingsChanged(null,new EventArgs<String>(Setting));
      }
    }



  }
}
