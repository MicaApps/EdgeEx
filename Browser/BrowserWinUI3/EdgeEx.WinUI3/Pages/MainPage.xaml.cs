
using CommunityToolkit.WinUI;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
        private MainViewModel ViewModel { get; }
        public MainPage()
        {
            this.InitializeComponent();
            WindowHelper.MainWindow.SetTitleBar(AppTitleBar);
            ViewModel = App.Current.Services.GetService<MainViewModel>();
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

        /// <summary>
        /// Add a new Tab to the TabView
        /// </summary>
        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            UriNavigate(new Uri("EdgeEx://NewTab/"));
        }
          
        private void NewTab(string title,Uri uri,IconSource icon,Type page, NavigateTabMode mode)
        {
            Frame frame = new Frame()
            {
                Margin = new Thickness(0, 48, 0, 0),
            };
            frame.Navigate(page, uri);
            if (mode == NavigateTabMode.NewTab)
            {
                
                string name = Guid.NewGuid().ToString("N");
                TabViewItem item = new TabViewItem
                {
                    Name = name,
                    IconSource = icon,
                    Header = title,
                    Tag = uri,
                    Content = frame,
                };
                int index = Tabs.SelectedIndex + 1;
                if(index == Tabs.TabItems.Count || Tabs.TabItems.Count == 0)
                {
                    Tabs.TabItems.Add(item);
                    Tabs.SelectedItem = item;
                }
                else
                {
                    Tabs.TabItems.Insert(index, item);
                    Tabs.SelectedItem = item;
                }
            }
            else if (mode == NavigateTabMode.Current)
            {
                (Tabs.SelectedItem as TabViewItem).Content = frame;
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
            if (Tabs.SelectedItem is TabViewItem item)
            {
                Frame frame = item.Content as Frame;
                ViewModel.CanGoBack = frame.CanGoBack;
                ViewModel.CanGoForward = frame.CanGoForward;
                Uri uri = item.Tag as Uri;
                AddressBar.Text = uri.ToString();
            }
            
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is TabViewItem item&& item.Content is Frame frame)
            {
                frame.GoBack();
                ViewModel.CanGoBack = frame.CanGoBack;
                ViewModel.CanGoForward = frame.CanGoForward;
            }
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is TabViewItem item && item.Content is Frame frame)
            { 
                frame.GoForward();
                ViewModel.CanGoBack = frame.CanGoBack;
                ViewModel.CanGoForward = frame.CanGoForward;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is TabViewItem item && item.Tag is Uri uri)
            {
                UriNavigate(uri, NavigateTabMode.Current);
            }
        }

        private void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            Tabs.TabItems.Remove(args.Tab);
        }
    }
}
