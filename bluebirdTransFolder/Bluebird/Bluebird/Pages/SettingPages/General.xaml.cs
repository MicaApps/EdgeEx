using Bluebird.Core;
using System.Threading.Tasks;
using System;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using System.IO;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Bluebird.Shared;
using Windows.Storage;

namespace Bluebird.Pages.SettingPages;

public sealed partial class General : Page
{
    public General()
    {
        this.InitializeComponent();
        id();
        ReadFileContent();
    }

    private async void ReadFileContent()
    {
        // Get the file
        StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Profile.txt", CreationCollisionOption.OpenIfExists);

        // Read the file's contents
        string fileContent = await FileIO.ReadTextAsync(file);

        // Split the file's contents into lines
        string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }
    private async void id()
    {
        var startup = await StartupTask.GetAsync("BlueBirdStartUp");
        UpdateToggleState(startup.State);
    }

    private void SearchengineSelection_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        string selection = SettingsHelper.GetSetting("EngineFriendlyName");
        if (selection != null)
        {
            SearchengineSelection.PlaceholderText = selection;
        }
        else
        {
            SearchengineSelection.PlaceholderText = "Qwant Lite";
        }
    }

    private void SearchengineSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        if (selection == "Ask") SetEngine("Ask", "https://www.ask.com/web?q=");
        if (selection == "Baidu") SetEngine("Baidu", "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=");
        if (selection == "Bing") SetEngine("Bing", "https://www.bing.com?q=");
        if (selection == "DuckDuckGo") SetEngine("DuckDuckGo","https://www.duckduckgo.com?q=");
        if (selection == "Ecosia") SetEngine("Ecosia", "https://www.ecosia.org/search?q=");
        if (selection == "Google") SetEngine("Google", "https://www.google.com/search?q=");
        if (selection == "Startpage") SetEngine("Startpage", "https://www.startpage.com/search?q=");
        if (selection == "Qwant") SetEngine("Qwant", "https://www.qwant.com/?q=");
        if (selection == "Qwant Lite") SetEngine("Qwant Lite", "https://lite.qwant.com/?q=");
        if (selection == "Yahoo!") SetEngine("Yahoo!", "https://search.yahoo.com/search?p=");
    }

    private void SetEngine(string EngineFriendlyName, string SearchUrl)
    {
        SettingsHelper.SetSetting("EngineFriendlyName", EngineFriendlyName);
        SettingsHelper.SetSetting("SearchUrl", SearchUrl);
    }

    private void UpdateToggleState(StartupTaskState state)
    {
        AutoStart.IsEnabled = true;
        switch (state)
        {
            case StartupTaskState.Enabled:
                AutoStart.IsChecked = true;
                break;
            case StartupTaskState.Disabled:
            case StartupTaskState.DisabledByUser:
                AutoStart.IsChecked = false;
                break;
            default:
                AutoStart.IsEnabled = false;
                break;
        }

    }
    private async Task ToggleLaunchOnStartup(bool enable)
    {
        var startup = await StartupTask.GetAsync("BlueBirdStartUp");
        switch (startup.State)
        {
            case StartupTaskState.Enabled when !enable:
                startup.Disable();
                break;
            case StartupTaskState.Disabled when enable:
                var updatedState = await startup.RequestEnableAsync();
                UpdateToggleState(updatedState);
                break;
            case StartupTaskState.DisabledByUser when enable:
                await UI.ShowDialog("Error", "Unable to change state of startup task via the application - enable via Startup tab on Task Manager (Ctrl+Shift+Esc)");
                break;
            default:
                await UI.ShowDialog("Error", "Unable to change state of startup task");
                break;
        }
    }

    private async void AutoStart_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        await ToggleLaunchOnStartup(AutoStart.IsChecked ?? false);
    }

    #region UpdateFrame
    private void ShowFrame()
    {
        FrameUpdate.Height = 200;
        FrameUpdate.Visibility = Windows.UI.Xaml.Visibility.Visible;
        Inameb.Text = "Close Add";
    }

    private void HideFrame()
    {
        FrameUpdate.Height = 1;
        FrameUpdate.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        Inameb.Text = "Add Profile";
    }

    #endregion

    private void Addprf_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        if (FrameUpdate.Content != null)
        {
            IDisposable disposableContent = FrameUpdate.Content as IDisposable;
            if (disposableContent != null)
            {
                disposableContent.Dispose();
            }
            FrameUpdate.Content = null;
            HideFrame();
        }
        else
        {
            ShowFrame();
            FrameUpdate.Navigate(typeof(AddProfile));
        }

    }

    private DispatcherTimer _timer;
    private int _progressValue;

    private void Syncprf_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        iscon.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        Progr.Visibility = Windows.UI.Xaml.Visibility.Visible;
        Progr.IsActive = true;
        Text.Text = "Syncing Profile";
        prg.Visibility = Windows.UI.Xaml.Visibility.Visible;
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object sender, object e)
    {
        _progressValue++;
        prg.Value = _progressValue;
        if (_progressValue >= 100)
        {
            iscon.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Progr.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Progr.IsActive = false;
            Text.Text = "Sync Profile";
            prg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _timer.Stop();
            _progressValue = 0;
        }
    }
    private void SettingsBlockControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {

    }
}
