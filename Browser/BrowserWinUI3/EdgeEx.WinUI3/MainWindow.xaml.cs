using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Serilog;
using System.IO;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets/icon.ico"));
            ResourceToolkit resourceToolkit = App.Current.Services.GetService<ResourceToolkit>();
            Title = resourceToolkit.GetString(ResourceKey.AppTitle);
            ExtendsContentIntoTitleBar = true;
        }

    }
}
