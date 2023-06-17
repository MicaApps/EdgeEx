using FireBrowser.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FireBrowser.Core
{
    public class UseContent
    {
        public static WebContent WebContent
        {
            get { return (Window.Current.Content as Frame)?.Content as WebContent; }
        }

        public static MainPage MainPageContent
        {
            get { return (Window.Current.Content as Frame)?.Content as MainPage; }
        }

        public static SettingsPage SettingsContent
        {
            get { return (Window.Current.Content as Frame)?.Content as SettingsPage; }
        }
    }
}
