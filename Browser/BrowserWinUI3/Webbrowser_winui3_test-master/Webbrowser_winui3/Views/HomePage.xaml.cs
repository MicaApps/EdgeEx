using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Webbrowser_winui3.ViewModels;
using Windows.Devices.Enumeration;
using System.Xml;
using System.Net;
using Microsoft.UI.Xaml.Media;
using System.Text;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Text.RegularExpressions;
using Sgml;
using Microsoft.UI.Xaml;
using ColorCode.Compilation.Languages;

namespace Webbrowser_winui3.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class HomePage : Page
{
    ListViewModel listDetailsViewModel = new ListViewModel();
    private string _TabTag = "";
    public HomePage()
    {
        _TabTag = MainViewModel._TagCount.ToString();
        InitializeComponent();
        this.Loaded += (s, e) =>
        {
            HomePageViewModel.InitCommand.Execute(gv);
            foreach(var lines in PageLinks.Children)
            {
                foreach (var links in ((StackPanel)lines).Children)
                {
                    Grid item = links as Grid;

                    string site = (item.Tag as string) ?? "blanks";

                    if (site == "blanks")
                        continue;
                    
                    WebClient wc = new WebClient();

                    byte[] downloaddata = wc.DownloadData(site);
                    string codestr = Encoding.GetEncoding("UTF-8").GetString(downloaddata);

                    var metas = MatchesMataTag().Matches(MatchHeadTag().Match(codestr).Value).ToArray();

                    string iconpath = "";

                    foreach (var meta in metas)
                    {
                        string metastring = meta.Value;

                        metastring = !metastring.EndsWith("/>") ? metastring[..^1] + " />" : metastring;

                        string after = "";

                        foreach(string unit in metastring.Split(" "))
                        {
                            var split = unit.Split("=");
                            if(split.Length == 1)
                            {
                                if (unit.StartsWith("<") || unit.EndsWith(">"))
                                {
                                    if (unit.EndsWith(">") && unit.Split("/")[1] == ">")
                                    {
                                        after += "/>";
                                        continue;
                                    }
                                        
                                    after += unit + " ";
                                }
                                else if(unit.StartsWith("\"") || unit.EndsWith("\""))
                                {
                                    after += unit + " ";
                                }
                                    
                            }
                            else
                            {
                                after += unit + " ";
                            }
                        }

                        after = after.TrimEnd(' ');

                        XmlDocument xd = new();
                        xd.LoadXml(after);

                        var element = xd.DocumentElement;

                        if (element.Attributes["rel"].Value.Split(" ").Contains("icon"))
                        {
                            iconpath = element.Attributes["href"].Value;
                            break;
                        }
                    }

                    if(iconpath != "")
                        item.Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri((iconpath.StartsWith("//")) ? "https:" + iconpath : iconpath.StartsWith("/")? "https://" + new Uri(site).Host + iconpath : iconpath))
                        };
                    else
                    {
                        item.Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("https://" + new Uri(site).Host + "/favicon.ico"))
                        };
                    }
                    item.PointerPressed += (sender, e) =>
                    {
                        /*int index = 0;
                        bool ok = false;
                        MainViewModel._MainPage.tabView.TabItems.ToList().ForEach(e =>
                        {
                            if(!ok)
                            {
                                var item = e as TabViewItem;
                                if (item.Tag == this._TabTag)
                                {
                                    ok = true;
                                    return;
                                }
                                index++;
                            }
                        });

                        var newpage = new WebView2Page();
                        newpage.SetUrl(site);
                        MainViewModel._MainPage.g_frames.Children[index] = newpage;*/
                        MainViewModel.OpenWebPageCommand.Execute(site);
                    };
                    
                }
            }
        };
    }

    [GeneratedRegex("<link [^>]+>")]
    private static partial Regex MatchesMataTag();
    [GeneratedRegex("<head>(.+)</head>")]
    private static partial Regex MatchHeadTag();

    private static string HtmlToStdXml(string html)
    {
        


        SgmlReader sr = new SgmlReader();

        sr.DocType = "HTML";
        sr.InputStream = new StringReader(html);

        StringWriter XMLText = new();
        XmlTextWriter xmlTextWriter = new XmlTextWriter(XMLText);

        while (!sr.EOF)
        {
            xmlTextWriter.WriteNode(sr, true);
        }

        xmlTextWriter.Close();


        string result = XMLText.ToString();
        XMLText.Close();
        sr.Close();
        return result;
    }

    private void GobnClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(tb_url.Text);
    }


    private void Grid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        var properties = e.GetCurrentPoint(root).Properties;
        if (properties.IsLeftButtonPressed)
        {
            HomePageViewModel.gv_ItemClickCommand.Execute(sender as Grid);
        }
    }

    private void TextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            MainViewModel.OpenWebPageCommand.Execute(tb_homeurl.Text);
        }
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(tb_homeurl.Text);
    }

    private void Grid_Holding(object sender, Microsoft.UI.Xaml.Input.HoldingRoutedEventArgs e)
    {
        HomePageViewModel.RightMenuCommand.Execute(sender as Grid);
    }

    private void Grid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        HomePageViewModel.RightMenuCommand.Execute(sender as Grid);
    }

    private void TextBox_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }

    private void TextBox_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }

    private void TextBox_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }
    private void TextBox_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }

    
}
