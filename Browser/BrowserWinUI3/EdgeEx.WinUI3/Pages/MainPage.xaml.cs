using ABI.System;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Helpers;
using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using WinUIEx;
using Type = System.Type;
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
        private ResourceToolkit resourceToolkit;
        private ICallerToolkit caller;
        private int titleBarHeight;
        private int commandBarHeight;
        private string PersistenceId { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            titleBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExTitleBarHeight"]);
            commandBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExCommandBarHeight"]);
            ViewModel = App.Current.Services.GetService<MainViewModel>();
            resourceToolkit = App.Current.Services.GetService<ResourceToolkit>();
            caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.UriNavigatedEvent += Caller_UriNavigatedEvent;
            caller.UriNavigatedMessageEvent += Caller_UriNavigatedMessageEvent;
            caller.FrameStatusEvent += Caller_FrameStatusEvent;
        }
        private void Caller_FrameStatusEvent(object sender, FrameStatusEventArg e)
        {
            if(e.PersistenceId == PersistenceId)
            {
                GoBackButton.IsEnabled = e.CanGoBack;
                GoForwardButton.IsEnabled = e.CanGoForward;
                RefreshButton.IsEnabled = e.CanRefresh;
            }
        }

        private void Caller_UriNavigatedMessageEvent(object sender, UriNavigatedMessageEventArg e)
        {
            if (Tabs.SelectedItem is TabViewItem item&& item.Name == e.TabItemName)
            {
                AddressBar.Text = e.NavigatedUri.ToString();
                item.Tag = e.NavigatedUri;
                item.Header = e.Title[1..^1];
                if(e.Icon != null && e.Icon!= item.IconSource)
                {
                    item.IconSource = e.Icon;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UriNavigate(e.Parameter as Uri, NavigateTabMode.NewTab);
        }
        private void Caller_UriNavigatedEvent(object sender, UriNavigatedEventArg e)
        {
            UriNavigate(e.NavigatedUri);
        }

        
        /// <summary>
        /// Add a new Tab to the TabView
        /// </summary>
        private void NewTab(string title,Uri uri,IconSource icon,Type page, NavigateTabMode mode)
        {
            string name = Guid.NewGuid().ToString("N");
            if (mode == NavigateTabMode.NewTab)
            {
                Frame frame = new Frame()
                {
                    Margin = new Thickness(0, titleBarHeight, 0, 0),
                };
                frame.Navigate(page, new NavigatePageArg(name, uri));
                TabViewItem item = new TabViewItem
                {
                    Name = name,
                    IconSource = icon,
                    Header = title,
                    Tag = uri,
                };
                item.Content = frame;
                int index = Tabs.SelectedIndex + 1;
                if(index == Tabs.TabItems.Count || Tabs.TabItems.Count == 0)
                {
                    Tabs.TabItems.Add(item);
                    Tabs.SelectedItem = item;
                }
                else
                {
                    Tabs.TabItems.Insert(index, item);
                    Tabs.SelectedIndex = index;
                }
            }
            else if (mode == NavigateTabMode.Current)
            {
                if(Tabs.SelectedItem is TabViewItem item)
                {
                    item.Name = name;
                    item.Tag = uri;
                    (item.Content as Frame).Navigate(page, new NavigatePageArg(name, uri));
                }
            }
        }


        private void UriNavigate(string uri, NavigateTabMode mode = NavigateTabMode.Current)
        {
            UriNavigate(new Uri(uri), mode);

        }
        private void UriNavigate(Uri uri, NavigateTabMode mode = NavigateTabMode.Current)
        {
            if(uri.Scheme == "edgeex")
            {
                GoBackButton.IsEnabled = false;
                GoForwardButton.IsEnabled = false;
                RefreshButton.IsEnabled = false;
                switch (uri.Host)
                {
                    case "history":
                        NewTab(resourceToolkit.GetString(ResourceKey.History), uri, new FontIconSource() { Glyph = "\uE81C" }, typeof(HistroyPage), mode);
                        break;
                    case "settings":
                        NewTab(resourceToolkit.GetString(ResourceKey.Settings), uri, new FontIconSource() { Glyph = "\uE713" }, typeof(SettingsPage), mode);
                        break;
                    case "newtab":
                        NewTab(resourceToolkit.GetString(ResourceKey.NewTab), uri, new FontIconSource() { Glyph = "\uE8A5" }, typeof(HomePage), mode);
                        break;
                    default: 
                        
                        break;
                }
            }
            else
            {
                NewTab(uri.ToString(), uri, new FontIconSource() { Glyph = "\uE8A5" }, typeof(WebPage), mode);
            }
        }

        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            UriNavigate(new Uri("EdgeEx://NewTab/"), NavigateTabMode.NewTab);
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("EdgeEx://History/"), NavigateTabMode.NewTab);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        { 
            UriNavigate(new Uri("EdgeEx://Settings/"), NavigateTabMode.NewTab);
        }

        private void BookMarksButthon_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("EdgeEx://BookMarks/"), NavigateTabMode.NewTab);
        }
        private void DownloadButthon_Click(object sender, RoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = true;
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedItem is TabViewItem item)
            { 
                Uri uri = item.Tag as Uri;
                AddressBar.Text = uri.ToString();
            }
            
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is TabViewItem item)
            {
                caller.FrameOperate(sender, PersistenceId, item.Name, FrameOperation.GoBack);
            }
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is TabViewItem item)
            {
                caller.FrameOperate(sender, PersistenceId, item.Name, FrameOperation.GoForward);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if(Tabs.SelectedItem is TabViewItem item)
            {
                caller.FrameOperate(sender, PersistenceId, item.Name, FrameOperation.Refresh);
            }
        }

        private void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            Tabs.TabItems.Remove(args.Tab);
            if(Tabs.TabItems.Count == 0)
            {
                WindowHelper.GetWindowForElement(this)?.Close();
            }
        } 

        private void AddressBar_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            
        }

        private void AddressBar_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UriNavigate(AddressBar.Text, NavigateTabMode.Current);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            caller.SizeChanged(e);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            TransparentBorder.Margin = new Thickness(0, titleBarHeight + commandBarHeight, 0, 0);
            CommandBar.Margin = new Thickness(8, titleBarHeight, 8, 0);
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
            window.SetTitleBar(AppTitleBar);
        }
        private void InitPersistenceId()
        {
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
        }
    }
}
