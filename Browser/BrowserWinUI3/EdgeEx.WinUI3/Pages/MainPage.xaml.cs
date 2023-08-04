using ABI.System;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Helpers;
using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Models;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
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
        private LocalSettingsToolkit localSettingsToolkit;
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
            localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
            caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.AddressUriNavigatedEvent += Caller_UriNavigatedEvent;
            caller.UriNavigationCompletedEvent += Caller_UriNavigationCompletedEvent;
            caller.UriNavigatedStartingEvent += Caller_UriNavigatedStartingEvent;
            caller.FrameStatusEvent += Caller_FrameStatusEvent;
            caller.LoadingEvent += Caller_LoadingEvent;
            caller.LoadingProgressEvent += Caller_LoadingProgressEvent;
        }

        private void Caller_LoadingProgressEvent(object sender, LoadingProgressEventArg e)
        {
            LoadingProgressBar.Value = e.Progress;
        }

        private void Caller_LoadingEvent(object sender, LoadingEventArg e)
        {
            LoadingControl.IsLoading = e.IsLoading;
            LoadingTitle.Text = e.Title;
        }

        private void Caller_UriNavigatedStartingEvent(object sender, UriNavigatedMessageEventArg e)
        {
            if (e.PersistenceId == PersistenceId && ViewModel.TabItems.FirstOrDefault(x => x.Name == e.TabItemName) is EdgeExTabItem item)
            {
                string uri = e.NavigatedUri.ToString();
                AddressBar.Text = e.NavigatedUri.ToString();
                item.Tag.NavigateUri = e.NavigatedUri;
                item.Tag.Header = uri;
            }
        }
        private void Caller_UriNavigationCompletedEvent(object sender, UriNavigatedMessageEventArg e)
        {
            if (e.PersistenceId == PersistenceId && ViewModel.TabItems.FirstOrDefault(x => x.Name == e.TabItemName) is EdgeExTabItem item)
            { 
                AddressBar.Text = e.NavigatedUri.ToString();
                item.Tag.NavigateUri = e.NavigatedUri;
                if(e.Title != null)
                {
                    item.Tag.Header = e.Title;
                }
                if (e.Icon != null && e.Icon != item.IconSource)
                {
                    item.IconSource = e.Icon;
                }
                ViewModel.CheckFavorite(e.NavigatedUri);
            }
        }
        private void Caller_FrameStatusEvent(object sender, FrameStatusEventArg e)
        {
            if (e.PersistenceId == PersistenceId && ViewModel.TabItems.FirstOrDefault(x => x.Name == e.TabItemName) is EdgeExTabItem item)
            {
                TabViewItemTag tag = item.Tag as TabViewItemTag;
                GoBackButton.IsEnabled = tag.CanGoBack = e.CanGoBack;
                GoForwardButton.IsEnabled = tag.CanGoForward = e.CanGoForward;
                RefreshButton.IsEnabled = tag.CanRefresh = e.CanRefresh;
            }
        }

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is Uri uri)
            {
                UriNavigate(uri, NavigateTabMode.NewTab);
            }else if(e.Parameter is EdgeExTabItem item)
            {
                ViewModel.TabItems.Add(item);
                Tabs.SelectedIndex = 0;
            }
            
        }
        private void Caller_UriNavigatedEvent(object sender, UriNavigatedEventArg e)
        {
            UriNavigate(e.NavigatedUri,e.Mode);
        }

        
        /// <summary>
        /// Add a new Tab to the TabView
        /// </summary>
        private void NewTab(string title,Uri uri,IconSource icon,Type page, NavigateTabMode mode)
        {
            AddressBar.Text = uri.ToString();
            string name = Guid.NewGuid().ToString("N");
            if (mode == NavigateTabMode.NewTab)
            {
                Frame frame = new Frame()
                {
                    Margin = new Thickness(0, titleBarHeight, 0, 0),
                };
                frame.Navigate(page, new NavigatePageArg(name, uri));
                EdgeExTabItem item = new EdgeExTabItem
                {
                    Name = name,
                    IconSource = icon,
                    Tag = new TabViewItemTag { NavigateUri = uri, Header = title, Name=name },
                };
                item.Content = frame;
                ViewModel.TabItems.Add(item);
                ViewModel.SelectedItem = item;
            }
            else if (mode == NavigateTabMode.Current)
            {
                if(Tabs.SelectedItem is EdgeExTabItem item)
                {
                    item.Tag = new TabViewItemTag { NavigateUri=uri};
                    (item.Content as Frame).Navigate(page, new NavigatePageArg(item.Name, uri));
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
                    case "bookmarks":
                        NewTab(resourceToolkit.GetString(ResourceKey.Bookmarks), uri, new FontIconSource() { Glyph = "\uE728" }, typeof(BookMarkPage), mode);
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
                TabViewItemTag tag = item.Tag as TabViewItemTag;
                AddressBar.Text = tag.NavigateUri.ToString();
                GoBackButton.IsEnabled = tag.CanGoBack;
                GoForwardButton.IsEnabled = tag.CanGoForward;
                RefreshButton.IsEnabled = tag.CanRefresh;
                ViewModel.CheckFavorite(tag.NavigateUri);
            }
            
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is EdgeExTabItem item)
            {
                caller.FrameOperate(sender, PersistenceId, item.Name, FrameOperation.GoBack);
            }
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is EdgeExTabItem item)
            {
                caller.FrameOperate(sender, PersistenceId, item.Name, FrameOperation.GoForward);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if(Tabs.SelectedItem is EdgeExTabItem item)
            {
                caller.FrameOperate(sender, PersistenceId, item.Name, FrameOperation.Refresh);
            }
        }

        private void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            ViewModel.TabItems.Remove((EdgeExTabItem)args.Item);
            if(ViewModel.TabItems.Count == 0)
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

        private void Tabs_TabDragStarting(TabView sender, TabViewTabDragStartingEventArgs args)
        {
            if (localSettingsToolkit.GetBoolean(LocalSettingName.IsTabDragTo))
            {
                DataPackage data = args.Data;
                data.RequestedOperation = DataPackageOperation.Move;
                data.SetText($"{PersistenceId}|{sender.TabItems.IndexOf(args.Tab)}");
            }
        }
        private void Tabs_TabDroppedOutside(TabView sender, TabViewTabDroppedOutsideEventArgs args)
        {
            if (localSettingsToolkit.GetBoolean(LocalSettingName.IsTabDragOut))
            {

                ThemeHelper.InitializeSetting();
                MainWindow window = new MainWindow();
                Frame frame = new Frame();
                window.PersistenceId = Guid.NewGuid().ToString("N");
                frame.Navigate(typeof(MainPage), args.Tab);
                window.Content = frame;
                WindowHelper.TrackWindow(window);
                window.Activate();
                ThemeHelper.Initialize();
                Tabs.TabItems.Remove(args.Tab);
                if (Tabs.TabItems.Count == 0)
                {
                    WindowHelper.GetWindowForElement(this)?.Close();
                }
            }
        }
        
        private void Tabs_TabDragCompleted(TabView sender, TabViewTabDragCompletedEventArgs args)
        {
           
        }

        private void Tabs_DragOver(object sender, DragEventArgs e)
        {
            if (localSettingsToolkit.GetBoolean(LocalSettingName.IsTabDragTo))
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.Caption = resourceToolkit.GetString(ResourceKey.TabDragTo);
                e.DragUIOverride.IsCaptionVisible = true;
                e.DragUIOverride.IsContentVisible = true;
                e.DragUIOverride.IsGlyphVisible = true;
            }
            
        }

        private void Tabs_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
           
        }

        private async void Tabs_Drop(object sender, DragEventArgs e)
        {
            if (localSettingsToolkit.GetBoolean(LocalSettingName.IsTabDragTo))
            {
                string a = await e.DataView.GetTextAsync();
                string[] args = a.Split('|');
                int index = Convert.ToInt32(args[1]);
                if (args[0] != PersistenceId && index < Tabs.TabItems.Count && index >= 0)
                {

                }
            }
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedItem is EdgeExTabItem item)
            {
                FavoriteName.Tag = item.Name;
                if (!ViewModel.IsFavorite)
                {
                    ViewModel.IsFavorite = !ViewModel.IsFavorite;
                    string title =  item.Tag.Header;
                    caller.Favorite(sender, PersistenceId,  item.Name, ViewModel.IsFavorite, item.Tag.NavigateUri.ToString(), title, "root");
                    FavoriteName.Text = title;
                }
                
            }
        }

        private void FavoriteDeteleButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.TabItems.FirstOrDefault(x=>x.Name == (string)FavoriteName.Tag ) is EdgeExTabItem item)
            {
                caller.Favorite(sender, PersistenceId, item.Name,
                    false, item.Tag.NavigateUri.ToString(), FavoriteName.Text, "default");
                ViewModel.IsFavorite = false;
                FavoriteFlyout.Hide();
            }
        }

        private void FavoriteConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TabItems.FirstOrDefault(x => x.Name == (string)FavoriteName.Tag) is EdgeExTabItem item)
            {
                caller.Favorite(sender, PersistenceId, item.Name,
                    true, item.Tag.NavigateUri.ToString(), FavoriteName.Text, "default");
                ViewModel.IsFavorite = true;
                FavoriteFlyout.Hide();
            }
        }
        
        private void TextBox_Tapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (ViewModel.CheckAddressTab())
            {
                var s = sender as Grid;
                var tag = s.Tag as TabViewItemTag;
                if (ViewModel.SelectedItem is EdgeExTabItem item && item.Name == tag.Name)
                {
                    s.Children[0].Visibility = Visibility.Collapsed;
                    s.Children[1].Visibility = Visibility.Visible;
                    s.Children[1].Focus(FocusState.Programmatic);

                }
            }
            
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CheckAddressTab())
            {
                var ss = sender as TextBox;
                var s = ss.Parent as Grid;
                s.Children[0].Visibility = Visibility.Visible;
                s.Children[1].Visibility = Visibility.Collapsed;
            }
        }
    }
}
