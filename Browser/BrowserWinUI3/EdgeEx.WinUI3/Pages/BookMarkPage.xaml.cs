using EdgeEx.WinUI3.Adapters;
using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Models;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookMarkPage : Page
    {
        private ICallerToolkit caller;
        private string PersistenceId { get; set; }
        private string TabItemName { get; set; }
        private Uri NavigateUri { get; set; }
        private BookmarkViewModel ViewModel { get; set; }
        public BookMarkPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService<BookmarkViewModel>();
        }
        private void InitPersistenceId()
        {
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
        }
        /// <summary>
        /// Control tab page size(From Event)
        /// </summary>
        private void Caller_SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            Top.Height = e.NewSize.Height;
            Top.Width = e.NewSize.Width;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigatePageArg args = e.Parameter as NavigatePageArg;
            TabItemName = args.TabItemName;
            NavigateUri = args.NavigateUri;
            
            caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.SizeChangedEvent += Caller_SizeChangedEvent;
            caller.FrameOperationEvent += Caller_FrameOperationEvent;
        }

        private void Caller_FrameOperationEvent(object sender, FrameOperationEventArg e)
        {
            if (TabItemName == e.TabItemName)
            {
                switch (e.Operation)
                {
                    case FrameOperation.Refresh:

                        break;
                    case FrameOperation.GoBack:

                        break;
                    case FrameOperation.GoForward:

                        break;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            caller.SizeChangedEvent -= Caller_SizeChangedEvent;
            caller.FrameOperationEvent -= Caller_FrameOperationEvent;
        }

        private void Top_Loaded(object sender, RoutedEventArgs e)
        {
            InitPersistenceId();
            // Initialize Tab size
            Rect rect = WindowHelper.GetWindowForElement(this).Bounds;
            int titleBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExTitleBarHeight"]);
            int commandBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExCommandBarHeight"]);
            Top.Height = rect.Height - titleBarHeight - commandBarHeight;
            Top.Width = rect.Width;
            caller.FrameStatus(this, PersistenceId, TabItemName, Frame.CanGoBack, Frame.CanGoForward, false);

        }

        private void BookmarksTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            ViewModel.SetCurrentBookmarks(args.InvokedItem as Bookmark);
            /* caller.UriNavigationCompleted(sender, PersistenceId, TabItemName,
                new System.Uri("EdgeEx://Bookmarks/"+), null , null);*/
        }

        private void BookmarksTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.InitBookmarks();
            StyleSelecter.SelectedIndex = 1;
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tag = (StyleSelecter.SelectedItem as FrameworkElement).Tag as string;
            if(tag == "ThumbList")
            {
                BookmarkGridView.Visibility = Visibility.Collapsed;
                BookmarkListView.Visibility = Visibility.Visible;
                BookmarkListView.ItemTemplate = this.Resources["BookmarkThumbListTemplate"] as DataTemplate;
            }
            else if(tag == "SimpleList")
            {
                BookmarkGridView.Visibility = Visibility.Collapsed;
                BookmarkListView.Visibility = Visibility.Visible;
                BookmarkListView.ItemTemplate = this.Resources["BookmarkSimpleListTemplate"] as DataTemplate;
            }
            else
            {
                BookmarkGridView.Visibility = Visibility.Visible;
                BookmarkListView.Visibility = Visibility.Collapsed;
            }
        }
         
        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            BookmarkAdapter adapter = new BookmarkAdapter();
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(WindowHelper.GetWindowForElement(this));
            FileOpenPicker openPicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.FileTypeFilter.Add(".html");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                adapter.ImportAsync(file);
            }
        }
    }
}
