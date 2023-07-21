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
        private string PersistenceId { get; set; }
        private string TabItemName { get; set; }
        public WebPage()
        {
            this.InitializeComponent();
            caller = App.Current.Services.GetService<ICallerToolkit>();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            caller.SizeChangedEvent += Caller_SizeChangedEvent;
            caller.FrameOperationEvent += Caller_FrameOperationEvent;
            NavigatePageArg args = e.Parameter as NavigatePageArg;
            TopWebView.Source = args.NavigateUri;
            TabItemName = args.TabItemName;
            await TopWebView.EnsureCoreWebView2Async();
            TopWebView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            caller.SizeChangedEvent -= Caller_SizeChangedEvent;
            caller.FrameOperationEvent -= Caller_FrameOperationEvent;
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
        }

        
         

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 
            Rect rect = WindowHelper.GetWindowForElement(this).Bounds;
            int titleBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExTitleBarHeight"]);
            int commandBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExCommandBarHeight"]);
            TopWebView.Height = rect.Height - titleBarHeight - commandBarHeight;
            TopWebView.Width = rect.Width;
            InitPersistenceId();
        }
        private void InitPersistenceId()
        {
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
        }
        private async void TopWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            if (args.IsSuccess && sender.Source.ToString() != "about:blank")
            {
                TopWebView.Visibility = Visibility.Visible; 
                ImageIconSource icon = new ImageIconSource()
                {
                    ImageSource = new BitmapImage(new Uri($"https://{sender.Source.Host}/favicon.ico")),
                };
                caller.SendUriNavigatedMessage(sender, PersistenceId, TabItemName,sender.Source,
                    (await sender.CoreWebView2.ExecuteScriptAsync("document.title")).ToString(), icon );
            }
            caller.FrameStatus(sender, PersistenceId, sender.CanGoBack, sender.CanGoForward, true);
        }

        private void TopWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if (sender.Source.ToString() == "about:blank") return;
            caller.SendUriNavigatedMessage(sender, PersistenceId,TabItemName, sender.Source, sender.Source.ToString(),null);
            caller.FrameStatus(sender, PersistenceId, sender.CanGoBack, sender.CanGoForward, true);
        }
    }
}
