using CommunityToolkit.Labs.WinUI;
using FireBrowser.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static FireBrowser.MainPage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        Passer param;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            param = e.Parameter as Passer;
        }
        public About()
        {
            this.InitializeComponent();
        }

        private void AboutCardClicked(object sender, RoutedEventArgs e)
        {
            string url = "https://example.com";
            switch ((sender as SettingsCard).Tag)
            {
                case "Discord":
                    url = "https://discord.gg/kYStRKBHwy";
                    break;
                case "GitHub":
                    url = "https://github.com/FirebrowserDevs/FireBrowser-Uwp";
                    break;
                case "License":
                    url = "https://github.com/FirebrowserDevs/FireBrowser-Uwp/blob/main/LICENSE";
                    break;
            }
            MainPage mp = new();
            UseContent.MainPageContent.NavigateToUrl(url);
        }
    }
}
