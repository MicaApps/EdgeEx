using Bluebird.Core;
using Bluebird.Shared;
using Microsoft.Web.WebView2.Core;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages.SettingPages;

public sealed partial class About : Page
{
    public About()
    {
        this.InitializeComponent();
        BluebirdVersion.Text = "Bluebird " + AppVersion.GetAppVersion() + " (" + SystemHelper.GetSystemArchitecture() + ")";
        WebView2Version.Text = "WebView2 " + CoreWebView2Environment.GetAvailableBrowserVersionString();
    }
}
