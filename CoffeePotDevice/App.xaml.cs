using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Data;

namespace CoffeePotDevice
{
  // sample: https://github.com/Windows-XAML/Template10/blob/master/Templates%20(Project)/Minimal/App.xaml.cs

  [Bindable]
  sealed partial class App : Template10.Common.BootStrapper
  {
    public App()
    {
      InitializeComponent();
      RequestedTheme = Windows.UI.Xaml.ApplicationTheme.Dark;
    }

    public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
    {
      // TODO: add your long-running task here
      await NavigationService.NavigateAsync(typeof(Views.MainPage));
    }
  }
}

