using CoffeePotDevice.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CoffeePotDevice.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class SettingsPage : Page
  {
    public SettingsPage()
    {
      this.InitializeComponent();
      NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
    }

    private void SettingsPagePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      PivotItem item = e.AddedItems[0] as PivotItem;
      if (item != null)
      {
        SettingsPivotViewModel vm = item.DataContext as SettingsPivotViewModel;
        if (vm != null)
        {
          vm.Rebind();
        }
      }
      
    }
  }
}
