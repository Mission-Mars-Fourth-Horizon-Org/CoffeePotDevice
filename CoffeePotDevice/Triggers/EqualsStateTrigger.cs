using CoffeePotDevice.Models;
using Windows.UI.Xaml;


namespace CoffeePotDevice.Triggers
{
  public class EqualsStateTrigger : StateTriggerBase
  {
    public FullScreenStates Left
    {
      get { return (FullScreenStates)GetValue(LeftProperty); }
      set { SetValue(LeftProperty, value); }
    }
    public static readonly DependencyProperty LeftProperty =
        DependencyProperty.Register(nameof(Left), typeof(FullScreenStates), typeof(EqualsStateTrigger),
            new PropertyMetadata(FullScreenStates.Collapsed, (s, e) => (s as EqualsStateTrigger).Update()));

    public FullScreenStates Right
    {
      get { return (FullScreenStates)GetValue(RightProperty); }
      set { SetValue(RightProperty, value); }
    }
    public static readonly DependencyProperty RightProperty =
        DependencyProperty.Register(nameof(Right), typeof(FullScreenStates), typeof(EqualsStateTrigger),
            new PropertyMetadata(FullScreenStates.Collapsed, (s, e) => (s as EqualsStateTrigger).Update()));

    void Update()
    {
      var equals = Equals(Left, Right);
      SetActive(equals);
    }
  }
}
