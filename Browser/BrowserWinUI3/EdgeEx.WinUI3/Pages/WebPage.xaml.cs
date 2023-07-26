using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WinUIEx;
using EdgeEx.WinUI3.Args;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using EdgeEx.WinUI3.Enums;
using Windows.UI.WebUI;
using Microsoft.Web.WebView2.Core;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.ViewModels;
using SqlSugar;
using EdgeEx.WinUI3.Models;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebPage : Page
    {
        private ICallerToolkit caller;
        private LocalSettingsToolkit settings;
        private ISqlSugarClient db;
        private string PersistenceId { get; set; }
        private string TabItemName { get; set; }
        private WebViewModel ViewModel { get; set; }
        public WebPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService<WebViewModel>();
            caller = App.Current.Services.GetService<ICallerToolkit>();
            settings = App.Current.Services.GetService<LocalSettingsToolkit>();
            db = App.Current.Services.GetService<ISqlSugarClient>();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            caller.SizeChangedEvent += Caller_SizeChangedEvent;
            caller.FrameOperationEvent += Caller_FrameOperationEvent;
            caller.FavoriteEvent += Caller_FavoriteEvent;
            NavigatePageArg args = e.Parameter as NavigatePageArg;
            TopWebView.Source = args.NavigateUri;
            TabItemName = args.TabItemName;
            await TopWebView.EnsureCoreWebView2Async();
            TopWebView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            caller.SizeChangedEvent -= Caller_SizeChangedEvent;
            caller.FrameOperationEvent -= Caller_FrameOperationEvent;
            caller.FavoriteEvent -= Caller_FavoriteEvent;
            TopWebView.CoreWebView2.NewWindowRequested -= CoreWebView2_NewWindowRequested;

        }
        private async void Caller_FavoriteEvent(object sender, FavoriteEventArg e)
        {
            if(e.PersistenceId == PersistenceId && e.TabItemName==TabItemName)
            {
                if(e.IsFavorite)
                {
                    
                    string screenshot = await CaptureScreenshotAsync();
                    ViewModel.AddBookMark(screenshot,e.Title,e.FolderId);
                }
                else
                {
                    ViewModel.DeleteBookMark();
                }
            }
            
        }

        private async void CoreWebView2_NewWindowRequested(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs args)
        {
            Deferral deferral = args.GetDeferral();
            await TopWebView.EnsureCoreWebView2Async();
            args.NewWindow = TopWebView.CoreWebView2;
            deferral.Complete();
            // TODO: New Tab
            /*Uri uri = new Uri(args.Uri);
            if (uri.Host == TopWebView.Source.Host)
            {
                
            }
            else
            {
                caller.NavigateUri(new Uri(args.Uri));
                Deferral deferral = args.GetDeferral();
                deferral.Complete();
            }*/
        }

        /// <summary>
        /// Capture WebView Screenshot
        /// </summary>
        private async Task<string> CaptureScreenshotAsync(string fileName=null, StorageFolder folder=null)
        {
            if(folder == null)
            {
                string thumbPath = settings.GetString(LocalSettingName.AppDataThumbsPath);
                if (!Directory.Exists(thumbPath))
                {
                    Directory.CreateDirectory(thumbPath);
                }
                folder = await StorageFolder.GetFolderFromPathAsync(thumbPath);
            }
            fileName ??= $"{Guid.NewGuid()}.png";
            MemoryStream memoryStream = new MemoryStream();
            IRandomAccessStream randomAccessStream = memoryStream.AsRandomAccessStream();
            SoftwareBitmap softwareBitmap;
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await DispatcherQueue.EnqueueAsync(
                async () =>
                {
                    await TopWebView.EnsureCoreWebView2Async();
                    await TopWebView.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, randomAccessStream);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
                    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                        encoder.SetSoftwareBitmap(softwareBitmap);
                        encoder.IsThumbnailGenerated = false;
                        await encoder.FlushAsync();
                    }
                }
                );
            return file.Path;
        }
        private void Caller_FrameOperationEvent(object sender, FrameOperationEventArg e)
        {
            if (TabItemName == e.TabItemName)
            {
                switch (e.Operation)
                {
                    case FrameOperation.Refresh:
                        TopWebView.Reload();
                        break;
                    case FrameOperation.GoBack:
                        if (TopWebView.CanGoBack)
                            TopWebView.GoBack();
                        break;
                    case FrameOperation.GoForward:
                        if (TopWebView.CanGoForward)
                            TopWebView.GoForward();
                        break;
                }
            }
        }

        private void Caller_SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            TopWebView.Height = e.NewSize.Height;
            TopWebView.Width = e.NewSize.Width;
            LoadingBar.Width = e.NewSize.Width;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 
            Rect rect = WindowHelper.GetWindowForElement(this).Bounds;
            int titleBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExTitleBarHeight"]);
            int commandBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExCommandBarHeight"]);
            TopWebView.Height = rect.Height - titleBarHeight - commandBarHeight;
            TopWebView.Width = rect.Width;
            LoadingBar.Width = rect.Width;
            InitPersistenceId();
        }
        private void InitPersistenceId()
        {
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
        }
        private async void TopWebView_NavigationCompletedAsync(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            LoadingBar.Visibility = Visibility.Collapsed;
            TopWebView.Visibility = Visibility.Visible;
            if (args.IsSuccess && sender.Source.ToString() != "about:blank")
            {
                string title = (await sender.CoreWebView2.ExecuteScriptAsync("document.title")).ToString();
                string iconUri = $"https://{sender.Source.Host}/favicon.ico";
                ViewModel.CallUriNavigationCompleted(sender, PersistenceId,
                    TabItemName, title, iconUri, sender.Source);
                if (ViewModel.CanUpdateScreenshot())
                {
                    ViewModel.UpdateScreenshot(await CaptureScreenshotAsync());
                }
            }
        }

        private void TopWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (sender.Source.ToString() == "about:blank") return;
            LoadingBar.Visibility = Visibility.Visible;
            ViewModel.CallUriNavigatedStarting(sender, PersistenceId, TabItemName, sender.Source);
        }
    }
}
