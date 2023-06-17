using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace FireBrowser.Controls
{
    public sealed partial class FireBrowserTabViewItem : TabViewItem
    {
        public FireBrowserTabViewItem()
        {
            this.InitializeComponent();
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(FireBrowserTabViewItem), null);
    }
}
