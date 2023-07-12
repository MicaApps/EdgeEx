using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
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
            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/icon.ico"));
            Title = ResourceHelper.GetString(ResourceKey.AppTitle);
            ExtendsContentIntoTitleBar = true;
            if(LocalSettingsHelper.GetString(LocalSettingName.Backdrop) is string backdrop)
            {
                if(backdrop == "Mica")
                {
                    this.SystemBackdrop = new MicaBackdrop();
                }
                else
                {
                    this.SystemBackdrop = new DesktopAcrylicBackdrop();
                }
            }
            else
            {
                this.SystemBackdrop = new MicaBackdrop();
                LocalSettingsHelper.Set(LocalSettingName.Backdrop, "Mica");
            }
        }
    }
}
