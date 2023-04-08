using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Webbrowser_winui3.ViewModels;
using Webbrowser_winui3.Views;

namespace Webbrowser_winui3;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    public static WindowEx _MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent(); 
        Environment.SetEnvironmentVariable("WEBVIEW2_USE_VISUAL_HOSTING_FOR_OWNED_WINDOWS", "1");
    }
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        _MainWindow.Activate();
    }
}
