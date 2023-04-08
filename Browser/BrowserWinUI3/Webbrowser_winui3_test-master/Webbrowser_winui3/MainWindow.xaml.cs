
using Webbrowser_winui3.ViewModels;
using Webbrowser_winui3.Views;
using Windows.Storage;
using WinUIEx;

namespace Webbrowser_winui3;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();
        bool IsAcrylicOrMica = ApplicationData.Current.LocalSettings.Values.ContainsKey("IsAcrylicOrMica") ? ApplicationData.Current.LocalSettings.Values["IsAcrylicOrMica"].ToString() == "False" ? false : true : true;
        if(IsAcrylicOrMica) this.Backdrop = new AcrylicSystemBackdrop(); else this.Backdrop = new MicaSystemBackdrop();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = AppWindow.Title;
        ExtendsContentIntoTitleBar = true;
        frame.Navigate(typeof(MainPage));
        MainViewModel._RequestedThemeList.Add(root);
    }
}
