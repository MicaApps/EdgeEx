
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Webbrowser_winui3.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Webbrowser_winui3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebView2Page : Page
    {
        string _WaitUrl = "", _WaitHtml = "";
        bool _IsWvLoaded = false;
        ListDetailsViewModel listDetailsViewModel = new ListDetailsViewModel();
        public WebView2Page()
        {
            this.InitializeComponent();
            this.Loaded += async (s, e) =>
            {
                await AppInit();
                _IsWvLoaded = true;
                if (_WaitUrl != "")
                {
                    SetUrl(_WaitUrl);
                }
                else if (_WaitHtml != "")
                {
                    SetHtml(_WaitHtml);
                }
            };
        }
        bool _IsNewWindowRequested = false;
        async Task AppInit()
        {
            await CoreWebView2Environment.CreateAsync();
            await wv.EnsureCoreWebView2Async();
            wv.DefaultBackgroundColor = Colors.Transparent;
            wv.NavigationCompleted += async (ss, ee) =>
            {
                progressBar.IsIndeterminate = false;
                webModel.Url = GetWvurl();
                webModel.Title = GetWvTitle();
                tb_url.Text = webModel.Url;
                SqliteService.EditDBByCommand($@"INSERT INTO History (Name,Url,Date)
                                                VALUES ('{webModel.Title}','{webModel.Url}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');");
                ListDetailsViewModel._HistorySource0.Insert(0, webModel);
                ListDetailsViewModel._HistorySource.Insert(0, webModel);
                webModel.tabItemIcon = await GetUrlImage(getIconUrl(webModel.Url));
                if (!_IsNewWindowRequested)
                {
                    wv.CoreWebView2.NewWindowRequested += (sss, eee) =>
                    {
                        eee.Handled = true;
                        if (MainViewModel._IsOpenInNew)
                        {
                            MainViewModel.OpenWebPageCommand.Execute(eee.Uri);
                        }
                        else
                        {
                            wv.Source = new Uri(eee.Uri);
                        }
                    };
                    _IsNewWindowRequested = true;
                }
            };
            wv.NavigationStarting += async (ss, ee) =>
            {
                progressBar.IsIndeterminate = true;
                _IsWvLoaded = false;
                webModel.Url = GetWvurl();
                webModel.Title = GetWvTitle();
                tb_url.Text = webModel.Url;
                webModel.tabItemIcon = await GetUrlImage(getIconUrl(webModel.Url));
            };
            wv.WebMessageReceived += (ss, ee) =>
            {
                var webmsg = ee.TryGetWebMessageAsString();

            };
            await Task.CompletedTask;
        }
        public void SetUrlOrSearch(string t)
        {
            if (_IsWvLoaded)
            {
                wv.Source = new Uri(t);
            }
            else
            {
                _WaitUrl = t;
            }
        }
        public void SetUrl(string url)
        {
            if (_IsWvLoaded)
            {
                wv.Source = new Uri(url);
            }
            else
            {
                _WaitUrl = url;
            }
        }
        public void SetHtml(string html)
        {
            if (_IsWvLoaded)
            {
                wv.NavigateToString(html);
            }
            else
            {
                _WaitHtml = html;
            }
        }
        public string getIconUrl(string url)
        {
            Uri uri = new Uri(url);
            return "http://" + uri.Host + "/favicon.ico";
        }
        WebModel webModel;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var line = e.Parameter;
            if (line != null)
            {
                if (line.GetType() == typeof(object[]))
                {
                    webModel = (line as object[])[0] as WebModel;
                    _WaitUrl = webModel.Url;
                    MainViewModel._ListWebViewPage.Add(webModel.tabItem.Tag.ToString(), this);
                }
            }
            base.OnNavigatedTo(e);
        }
        public void WebPageClose()
        {
            wv.NavigateToString("");
        }
        public void GoBack()
        {
            if (wv.CanGoBack)
            {
                wv.GoBack();
            }
        }
        public void GoForward()
        {
            if (wv.CanGoForward)
            {
                wv.GoForward();
            }
        }
        public void GoRefresh()
        {
            wv.CoreWebView2.Reload();
        }
       
        string GetWvTitle()
        {
            return wv.CoreWebView2.DocumentTitle;
        }
        string GetWvurl()
        {
            return wv.Source.AbsoluteUri;
        }
        public async Task<BitmapImage> GetUrlImage(string url)
        {
            try
            {
                Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();

                IBuffer buffer = await http.GetBufferAsync(new Uri(url));

                BitmapImage img = new BitmapImage();

                using (IRandomAccessStream stream = new InMemoryRandomAccessStream())

                {
                    await stream.WriteAsync(buffer);

                    stream.Seek(0);

                    await img.SetSourceAsync(stream);

                    return img;
                }
            }
            catch { return null; }
        }
        /// <summary>
        /// ×¢Èëjs
        /// </summary>
        /// <param name="mutefunctionString"></param>
        public async void jsmsg(string mutefunctionString)
        {
            //string mutefunctionString = @"document.onkeydown=function(e){
            //        var keyNum=e.keyCode;
            //        chrome.webview.postMessage(keyNum+'');
            //        }";
            var a = await wv.ExecuteScriptAsync(mutefunctionString);
        }


        private async void AddFavButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog();
            cd.XamlRoot = ContentArea.XamlRoot;
            cd.Title = ReswSource.GetString("AddToFav");
            cd.CloseButtonText = ReswSource.GetString("Cancle");
            cd.Content = new TextBlock() { Text = $"{webModel.Title}\n{webModel.Url}" };
            cd.PrimaryButtonText = ReswSource.GetString("OK");
            cd.PrimaryButtonClick += (ss, ee) =>
            {
                SqliteService.EditDBByCommand($@"INSERT INTO Favorite (Name,Url,Date)
                                                VALUES ('{webModel.Title}','{webModel.Url}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');");
                ListDetailsViewModel._FavoriteSource0.Insert(0, webModel);
                ListDetailsViewModel._FavoriteSource.Insert(0, webModel);
            };
            await cd.ShowAsync();
        }

        private void DownloadbnClick(object sender, RoutedEventArgs e)
        {
            wv.CoreWebView2.OpenDefaultDownloadDialog();
        }

        private void GobnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel.OpenWebPageCommand.Execute(tb_url.Text);
        }

        private void backclick(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void forwardclick(object sender, RoutedEventArgs e)
        {
            GoForward();
        }

        private void refreshclick(object sender, RoutedEventArgs e)
        {
            GoRefresh();
        }

        /// <summary>
        /// ÍøÒ³¾²Òô
        /// </summary>
        public async void Mute(bool isMute)
        {
            if (isMute)
            {
                string mutefunctionString = @"
    var videos = document.querySelectorAll('video'),
    audios = document.querySelectorAll('audio');
    [].forEach.call(videos, function(video) { video.muted = true; });
    [].forEach.call(audios, function(audio) { audio.muted = true; }); ";

                var a = await wv.ExecuteScriptAsync(mutefunctionString);
                wv.NavigateToString("");
            }
            else
            {
                string mutefunctionString = @"
    var videos = document.querySelectorAll('video'),
    audios = document.querySelectorAll('audio');
    [].forEach.call(videos, function(video) { video.muted = false; });
    [].forEach.call(audios, function(audio) { audio.muted = false; }); ";

                await wv.ExecuteScriptAsync(mutefunctionString);
            }
        }
    }
}