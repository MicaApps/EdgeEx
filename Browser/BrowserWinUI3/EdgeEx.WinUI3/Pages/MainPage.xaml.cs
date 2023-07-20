
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Uri = System.Uri;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            WindowHelper.MainWindow.SetTitleBar(AppTitleBar);
            ICallerToolkit caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.UriNavigatedEvent += Caller_UriNavigatedEvent;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UriNavigate(e.Parameter as Uri);
        }
        private void Caller_UriNavigatedEvent(object sender, Args.UriNavigatedEventArg e)
        {
            UriNavigate(e.NavigatedUri);
        }

        // Add a new Tab to the TabView
        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            UriNavigate(new Uri("EdgeEx://NewTab/"));
        }

        // Remove the requested tab from the TabView
        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        { 
            //sender.TabItems.Remove(args.Item);
        }

        
        private void NewTab(string title,Uri uri,IconSource icon,Type page, NavigateTabMode mode)
        {
            if (mode == NavigateTabMode.NewTab)
            {
                Frame frame = new Frame()
                {
                    Margin = new Thickness(0, 48, 0, 0),
                };
                 
                var item = new TabViewItem
                {
                    IconSource = icon,
                    Header = title,
                    Tag = uri,
                    Content = frame,
                };
                int index = Tabs.SelectedIndex + 1;
                if(index == Tabs.TabItems.Count || Tabs.TabItems.Count==0)
                {
                    Tabs.TabItems.Add(item);
                }
                else
                {
                    Tabs.TabItems.Insert(index, item);
                }
                Tabs.SelectedItem = item; 
            }
        }
        private void UriNavigate(Uri uri, NavigateTabMode mode = NavigateTabMode.NewTab)
        {
            if(uri.Scheme == "edgeex")
            {
                switch (uri.Host)
                {
                    case "history":
                        NewTab("history", uri, new FontIconSource() { Glyph = "\uE81C" }, typeof(HistroyPage), mode);
                        break;
                    case "settings":
                        NewTab("settings", uri, new FontIconSource() { Glyph = "\uE713" }, typeof(SettingsPage), mode);
                        break;
                    case "newtab":
                        NewTab("newtab", uri, new FontIconSource() { Glyph = "\uE8A5" }, typeof(HomePage), mode);
                        break;
                    default: 
                        
                        break;
                }
            }
            else
            {

            }
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("EdgeEx://History/"));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("EdgeEx://Settings/"));
        }

        private void BookMarksButthon_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("EdgeEx://BookMarks/"));
        }
        private void DownloadButthon_Click(object sender, RoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = true;
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedItem == null) return;
            Uri uri = (Tabs.SelectedItem as FrameworkElement).Tag as Uri;
            AddressBar.Text = uri.ToString();
        }
    }
}
