using CoffeePotDevice.Models;
using CoffeePotDevice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace CoffeePotDevice.ViewModels
{
  class SettingsPageViewModel: ViewModelBase
  {
    #region Navigation Commands

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

    #endregion Navigation Commands

    #region Pivot View Model References

    public SettingsPivotViewModel SettingsPivotViewModel { get; } = new SettingsPivotViewModel();

    public DevicesPivotViewModel DevicesPivotViewModel { get; } = new DevicesPivotViewModel();

    #endregion Pivot View Model References

  }

}
