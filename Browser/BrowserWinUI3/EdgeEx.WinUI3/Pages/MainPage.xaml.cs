using EdgeEx.WinUI3.Helpers;
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
        }
        // Add a new Tab to the TabView
        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            var newTab = new TabViewItem();
            newTab.IconSource = new SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Document";

            // The Content of a TabViewItem is often a frame which hosts a page.
            Frame frame = new Frame();
            newTab.Content = frame;
            frame.Navigate(typeof(HomePage));

            sender.TabItems.Add(newTab);
        }

        // Remove the requested tab from the TabView
        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Item);
        }

        
        private void NewTab(string title,IconSource icon,Type page)
        {
            Tabs.TabItems.Add(new TabViewItem
            {
                IconSource = icon,
                Header = title,
                Tag = title,
            });
            ContentFrame.Navigate(page);
        }
        private void UriNavigate(Uri uri)
        {
            switch(uri.Scheme.ToLower()) 
            {
                case "history":
                    NewTab("history", new FontIconSource() { Glyph = "\uE81C" }, typeof(HistroyPage));
                    break;
                case "home":
                    NewTab("home", new FontIconSource() { Glyph = "\uE80F" }, typeof(HomePage));
                    break;
                case "bookmarks":
                    NewTab("bookmarks", new FontIconSource() { Glyph = "\uE728" }, typeof(HomePage));
                    break;
                case "settings":
                    NewTab("settings", new FontIconSource() { Glyph = "\uE713" }, typeof(SettingsPage));
                    break;
            }
            
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("History://local/"));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("Settings://local/"));
        }

        private void BookMarksButthon_Click(object sender, RoutedEventArgs e)
        {
            UriNavigate(new Uri("BookMarks://local/"));
        }
        private void DownloadButthon_Click(object sender, RoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = true;
        }
    }
}
