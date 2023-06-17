using CommunityToolkit.Mvvm.ComponentModel;
using FireBrowserCore.Models;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace FireBrowser.Controls
{
    public sealed partial class FireBrowserTabView : TabView
    {
        public FireBrowserTabView()
        {
            this.InitializeComponent();
            ViewModel = new FireBrowserTabViewViewModel()
            {
                Style = (Style)Application.Current.Resources["DefaultTabViewStyle"]
            };
        }

        public FireBrowserTabViewViewModel ViewModel { get; set; }
        public partial class FireBrowserTabViewViewModel : ObservableObject
        {
            [ObservableProperty]
            private Style style;
        }

        public Settings.UILayout Mode
        {
            get => (Settings.UILayout)GetValue(ModeProperty);
            set
            {
                switch (value)
                {
                    case Settings.UILayout.Modern:
                        ViewModel.Style = (Style)Application.Current.Resources["DefaultTabViewStyle"];
                        break;
                    case Settings.UILayout.Vertical:
                        ViewModel.Style = (Style)Application.Current.Resources["VerticalTabViewStyle"];
                        break;
                    default:
                        ViewModel.Style = (Style)Application.Current.Resources["DefaultTabViewStyle"];
                        break;
                }
                SetValue(ModeProperty, value);
            }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(Settings.UILayout), typeof(FireBrowserTabView), null);
    }
}
