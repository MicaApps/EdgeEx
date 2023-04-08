using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Input;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Webbrowser_winui3.Views;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.WebUI;

namespace Webbrowser_winui3.ViewModels;

public class MainViewModel : ObservableRecipient
{
    public static MainPage _MainPage;
    public static ObservableCollection<EngineModel> _EngineSource = new() { };
    public static SettingViewModel _SettingViewModel = new SettingViewModel();
    public static Dictionary<string, WebView2Page> _ListWebViewPage = new();
    public static Dictionary<string, WebModel> _ListWebModel = new();
    public static List<Grid> _RequestedThemeList = new();
    public static bool _IsOpenInNew = true;
    public static string _SearchEngine = "http://www.bing.com/search?q=";
    static int _TagCount = 0;
    public static ICommand MainPageInitCommand = new RelayCommand<MainPage>(async (param) =>
    {
        _MainPage = param;
        _SettingViewModel.ThemeInitCommand.Execute(null);
        UpdateTitleBar();
        await ApplicationData.Current.LocalFolder.CreateFolderAsync("HomeIcons", CreationCollisionOption.OpenIfExists);
        SqliteService.CreatDBByName("Data.db", "Home", "Name text,Url text,IconName text");
        SqliteService.CreatDBByName("Data.db", "Favorite", "Name text,Url text,Date text");
        SqliteService.CreatDBByName("Data.db", "History", "Name text,Url text,Date text");
        SqliteService.CreatDBByName("Data.db", "SearchEngine", "Name text,Url text");
        var IsBingBack = ApplicationData.Current.LocalSettings.Values.ContainsKey("IsBingBackground") ? ApplicationData.Current.LocalSettings.Values["IsBingBackground"].ToString() == "True" ? true : false : false; 
        if(IsBingBack) _MainPage.g_bing.Background = new ImageBrush() { ImageSource = await GetBingImage() };
        OpenHomeCommand.Execute(null);
    });

    public static ICommand TabSelectChangeCommand = new RelayCommand<TabViewItem>((param) =>
        {
            if (param != null)
            {
                GetFrame(param.Tag.ToString());
            }
        });
    public static ICommand Page_SizeChangedCommand = new RelayCommand<Size>((param) =>
        {
            UpdateTitleBar();
        });
    public static ICommand TabCloseRequestedCommand = new RelayCommand<TabViewItem>((param) =>
        {
            if (_ListWebViewPage.ContainsKey(param.Tag.ToString()))
            {
                var wvp = _ListWebViewPage[param.Tag.ToString()];
                wvp.Mute(true);
                _ListWebViewPage.Remove(param.Tag.ToString());
            }
            _MainPage.tabView.TabItems.Remove(param);
            _MainPage.g_frames.Children.Remove(_MainPage.g_frames.Children.First(o => (o as Frame).Tag.ToString() == param.Tag.ToString()));
        });
    public static ICommand CopyUrlCommand = new RelayCommand<string>((param) =>
    {
        SetClipBoard(param);
        ShowMessageBox(ReswSource.GetString("AlreadyCopy"), param);
    });
    public static ICommand ButtonAddToHome_ClickCommand = new RelayCommand<string>(async (param) =>
    {
        WebModel webModel;
        try
        {
            webModel = ListDetailsViewModel._HistorySource0.First(o => o.Url == param);
        }
        catch
        {
            webModel = ListDetailsViewModel._FavoriteSource0.First(o => o.Url == param);
        }
        ContentDialog cd = new ContentDialog();
        cd.XamlRoot = _MainPage.XamlRoot;
        cd.Title = ReswSource.GetString("AddToHome");
        cd.CloseButtonText = ReswSource.GetString("Cancle");
        cd.Content = $"{webModel.Title}\n{webModel.Url}";
        cd.PrimaryButtonText = ReswSource.GetString("OK");
        cd.PrimaryButtonClick += (ss, ee) =>
        {
            SqliteService.EditDBByCommand($@"INSERT INTO Home (Name,Url,IconName)
                                                VALUES ('{webModel.Title}','{webModel.Url}','{webModel.Title}');");
            HomePageViewModel._HomeSource.Insert(0, webModel);
        };
        await cd.ShowAsync();

    });
    public static ICommand OpenWebPageCommand = new RelayCommand<string>((param) =>
    {
        switch (param)
        {
            case "app://favorite/":
                ListDetailsViewModel._IsHisOrFav = false;
                ListDetailsViewModel.FavInit_Command.Execute(null);
                OpenFavCommand.Execute(null);
                break;
            case "app://history/":
                ListDetailsViewModel._IsHisOrFav = true;
                ListDetailsViewModel.HistoryInit_Command.Execute(null);
                OpenHistoryCommand.Execute(null);
                break;
            case "app://settings/":
                OpenSettingCommand.Execute(null);
                break;
            case "app://home/":
                OpenHomeCommand.Execute(null);
                break;
            default:
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("SearchEngine"))
                {
                    _SearchEngine = ApplicationData.Current.LocalSettings.Values["SearchEngine"].ToString();
                }
                if (_SearchEngine == "") _SearchEngine = "http://www.bing.com/search?q=";
                OpenNewPage(typeof(WebView2Page), "", new object[] { new WebModel() { Url = GetRealUrl(param)}});
                break;
        }
    });
    public static ICommand OpenHomeCommand = new RelayCommand<object>((param) =>
    {
        OpenNewPage(typeof(HomePage), ReswSource.GetString("Home"), null);
    });
    public static ICommand OpenSettingCommand = new RelayCommand<object>((param) =>
    {
        OpenNewPage(typeof(SettingsPage), ReswSource.GetString("Setting"), null);
    });
    public static ICommand OpenHistoryCommand = new RelayCommand<object>((param) =>
    {
        OpenNewPage(typeof(ListDetailsPage), ReswSource.GetString("History"), new object[] { "History" });
    });
    public static ICommand OpenFavCommand = new RelayCommand<object>((param) =>
    {
        OpenNewPage(typeof(ListDetailsPage), ReswSource.GetString("Favorite"), new object[] { "Favorite" });
    });
    static void OpenNewPage(Type type, string header, object[] objects)
    {
        var tag = (_TagCount += 1) + "";
        Frame frame = new() { Tag = tag };
        var tabviewitem = new TabViewItem()
        {
            Tag = tag,
            Header = header,
        };
        if(type== typeof(WebView2Page))
        {
            (objects[0] as WebModel).tabItem = tabviewitem;
        }
        _MainPage.tabView.TabItems.Add(tabviewitem);
        frame.Navigate(type, objects);
        _MainPage.g_frames.Children.Add(frame);
        GetFrame(tag);
        UpdateTitleBar();
    }
    static Frame GetFrame(string tag)
    {
        Frame result = null;
        foreach (var f in _MainPage.g_frames.Children)
        {
            if (f.GetType() == typeof(Frame))
            {
                var frame = f as Frame;
                if (frame.Tag.ToString() == tag)
                {
                    frame.Visibility = Visibility.Visible;
                    _MainPage.tabView.SelectedItem = _MainPage.tabView.TabItems.First(o => (o as TabViewItem).Tag.ToString() == tag);
                    result = frame;
                }
                else
                {
                    frame.Visibility = Visibility.Collapsed;
                }
            }
        }
        return result;
    }
    static void UpdateTitleBar()
    {
        if (_MainPage != null)
        {
            try
            {
                _MainPage.tabView.MaxWidth = _MainPage.ActualWidth - 150;
            }
            catch { }
            try
            {
                App._MainWindow.SetTitleBar(_MainPage.g_titlebar);
            }
            catch { }
        }
    }
    async public static void ShowMessageBox(string title, string content)
    {
        ContentDialog cd = new ContentDialog();
        cd.XamlRoot = _MainPage.XamlRoot;
        cd.Title = title;
        cd.CloseButtonText = "OK";
        cd.Content = new TextBlock() { Text = content };
        await cd.ShowAsync();
    }
    public static void InitSearchEngine()
    {
        _EngineSource.Clear();
        var query = SqliteService.ReadTableData("SearchEngine", "Name,Url", "", "");
        while (query.Read())
        {
            _EngineSource.Add(new EngineModel() { Title = query.GetString(0), Url = query.GetString(1) });
        }
        SqliteService.db.Close();
        if (_EngineSource.Count == 0)
        {
            _EngineSource.Add(new EngineModel() { Title = "Bing", Url = "http://www.bing.com/search?q=" });
            _EngineSource.Add(new EngineModel() { Title = "Baidu", Url = "http://www.baidu.com/s?word=" });
            SqliteService.EditDBByCommand($@"INSERT INTO SearchEngine (Name,Url)
                                            VALUES ('Bing','http://www.bing.com/search?q=');");
            SqliteService.EditDBByCommand($@"INSERT INTO SearchEngine (Name,Url)
                                            VALUES ('Baidu','http://www.baidu.com/s?word=');");
            ApplicationData.Current.LocalSettings.Values["SearchEngine"] = "http://www.bing.com/search?q=";
        }
    }
    public static string GetRealUrl(string url)
    {
        if (url.Length >= 8)
        {
            if (url.Substring(0, 7) == "http://" || url.Substring(0, 8) == "https://")
            { }
            else
            {
                if (url.Contains("."))
                    url = "http://" + url;
                else
                {
                    url = _SearchEngine + HttpUtility.UrlEncode(url);
                }
            }
        }
        else
        {
            if (url.Contains("."))
                url = "http://" + url;
            else
            {
                url = _SearchEngine + HttpUtility.UrlEncode(url);
            }
        }
        return url;
    }
    public static async Task<BitmapImage> GetStorageFileImage(StorageFile sf)
    {
        BitmapImage bmp = new BitmapImage();
        using (IRandomAccessStream outputStream = await sf.OpenAsync(FileAccessMode.ReadWrite))
        {
            bmp.SetSource(outputStream);
        }
        return bmp;
    }
    public static async Task<BitmapImage> GetUrlImage(string name,string url)
    {
        try
        {
            var fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("HomeIcons", CreationCollisionOption.OpenIfExists);
            Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();
            IBuffer buffer = await http.GetBufferAsync(new Uri(url));
            BitmapImage img = new BitmapImage();
            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer);
                stream.Seek(0);
                await img.SetSourceAsync(stream);
                StorageFile file = await fd.CreateFileAsync(name);
                await FileIO.WriteBytesAsync(file, await ConvertIRandomAccessStreamByte(stream));
                return img;
            }
        }
        catch { return null; }
    }
    private static async Task<byte[]> ConvertIRandomAccessStreamByte(IRandomAccessStream stream)

    {

        DataReader read = new DataReader(stream.GetInputStreamAt(0));

        await read.LoadAsync((uint)stream.Size);

        byte[] temp = new byte[stream.Size];

        read.ReadBytes(temp);

        return temp;

    }
    /// <summary>
    /// 设置剪切板文字
    /// </summary>
    /// <param name="t"></param>
    public static void SetClipBoard(string t)
    {
        DataPackage dp = new DataPackage();
        dp.SetText(t);
        Clipboard.SetContent(dp);
    }
    async public static Task<BitmapImage> GetBingImage()
    {
        string InfoUrl = "http://www.bing.com/HPImageArchive.aspx?idx=0&n=1";
        HttpResponseMessage response = await new HttpClient().GetAsync(new Uri(InfoUrl), HttpCompletionOption.ResponseHeadersRead);
        string XmlString = await response.Content.ReadAsStringAsync();
        // 定义正则表达式用来匹配标签
        Regex regImg = new Regex("<Url>(?<imgUrl>.*?)</Url>", RegexOptions.IgnoreCase);
        // 搜索匹配的字符串
        MatchCollection matches = regImg.Matches(XmlString);
        // 取得匹配项列表
        string ImageUrl = "http://www.bing.com" + matches[0].Groups["imgUrl"].Value;
        return new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
    }

}
