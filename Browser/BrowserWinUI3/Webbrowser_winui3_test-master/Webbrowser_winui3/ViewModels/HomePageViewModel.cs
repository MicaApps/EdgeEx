
using ABI.System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webbrowser_winui3.Helpers;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Windows.ApplicationModel;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using static System.Net.Mime.MediaTypeNames;

namespace Webbrowser_winui3.ViewModels
{
    public class HomePageViewModel:ObservableRecipient
    {
        public static ObservableCollection<WebModel> _HomeSource = new() { };
        public static ICommand InitCommand = new RelayCommand<object>(async (param) =>
        {
            (param as GridView).ItemsSource = _HomeSource;
            _HomeSource.Clear();
            var query = SqliteService.ReadTableData("Home", "Name,Url,IconName", "", "");
            while (query.Read())
            {
                _HomeSource.Insert(0, new WebModel
                {
                    Title = query.GetString(0),
                    Url = query.GetString(1),
                    tabItemIcon =await GetIcon(query.GetString(0), query.GetString(1)),
                });
            }
            SqliteService.db.Close();

        });
        public static ICommand gv_ItemClickCommand = new RelayCommand<Grid>(async (param) =>
        {
            MainViewModel.OpenWebPageCommand.Execute(param.Tag.ToString());
        }); 
        public static ICommand RightMenuCommand = new RelayCommand<Grid>(async (param) =>
        {
            MenuFlyout mf = new MenuFlyout();
            MenuFlyoutItem mfi = new MenuFlyoutItem() { Text = ReswSource.GetString("Delete") };
            mfi.Click += (ss, ee) =>
            {
                SqliteService.DeleteTableData("Home", $"Url='{param.Tag.ToString()}'");
                var list1 = _HomeSource.Where(o => o.Url == param.Tag.ToString()).ToArray();
                foreach (var o in list1)
                {
                    _HomeSource.Remove(o);
                }
            };
            mf.Items.Add(mfi);
            mf.ShowAt(param);
        });
        async static Task<ImageSource> GetIcon(string iconname,string url)
        {
            try
            {
                var fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("HomeIcons", CreationCollisionOption.OpenIfExists);
                
                if (await fd.FileExistsAsync(iconname))
                {
                    return await MainViewModel.GetStorageFileImage(await fd.GetFileAsync(iconname));
                }
                else
                {
                    var m=await MainViewModel.GetUrlImage(iconname,@$"http://{new System.Uri(url).Host}/favicon.ico");
                    if (m != null)
                    {
                        return m;
                    }
                    else
                    {
                        return new BitmapImage(new System.Uri("ms-appx:///Images/web.png"));
                    }
                }
            }
            catch
            {
                return new BitmapImage(new System.Uri("ms-appx:///Images/web.png"));
            }
        }
    }
}
