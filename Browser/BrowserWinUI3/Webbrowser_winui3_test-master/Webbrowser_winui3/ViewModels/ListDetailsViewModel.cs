using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;

namespace Webbrowser_winui3.ViewModels;

public class ListDetailsViewModel : ObservableRecipient
{
    public static List<WebModel> _HistorySource0 = new() { };
    public static ObservableCollection<WebModel> _HistorySource = new() { };
    public static List<WebModel> _FavoriteSource0 = new() { };
    public static ObservableCollection<WebModel> _FavoriteSource = new() { };
    public static ObservableCollection<WebModel> _ItemSource = new() { };
    public static ICommand OnNavigatedTo_Command = new RelayCommand<object[]>((line) =>
    {
        if (line != null)
        {
            switch (line[0].ToString())
            {
                case "Favorite":
                    FavInit_Command.Execute(null);
                    break;
                case "History":
                    HistoryInit_Command.Execute(null);
                    break;
            }
        }
    });
    public static ICommand TextBox_TextChanged_Command = new RelayCommand<TextBox>((param) =>
    {
        if (IsHisOrFav())
        {
            _HistorySource.Clear();
            var list = _HistorySource0.Where(o => o.Title.ToLower().Contains(param.Text.ToLower()) || o.Url.ToLower().Contains(param.Text.ToLower())).ToArray();
            foreach (var l in list)
            {
                _HistorySource.Add(l);
            }
        }
        else
        {
            _FavoriteSource.Clear();
            var list = _FavoriteSource0.Where(o => o.Title.ToLower().Contains(param.Text.ToLower()) || o.Url.ToLower().Contains(param.Text.ToLower())).ToArray();
            foreach (var l in list)
            {
                _FavoriteSource.Add(l);
            }
        }
    });
    public static ICommand Set_ListDetailsViewControlSource_Command = new RelayCommand<object[]>((param) =>
    {
        var lv = param[0] as ListDetailsView;
        var tb = param[1] as TextBox;
        if (_IsHisOrFav)
        {
            lv.ItemsSource = _HistorySource;
            tb.Text = "app://history/";
        }
        else
        {
            lv.ItemsSource = _FavoriteSource;
            tb.Text = "app://favorite/";
        }
    });
    public static ICommand Set_lv_historydetailSource_Command = new RelayCommand<ListView>((param) =>
    {
        param.ItemsSource = _ItemSource;
    });
    public static ICommand DeleteCommand = new RelayCommand<string>((param) =>
    {
        if (IsHisOrFav())
        {
            SqliteService.DeleteTableData("History", $"Url='{param}'");
            var list1 = _HistorySource.Where(o => o.Url == param).ToArray();
            var list2 = _ItemSource.Where(o => o.Url == param).ToArray();
            foreach (var o in list1)
            {
                _HistorySource0.Remove(o);
                _HistorySource.Remove(o);
            }
            foreach (var o in list2)
            {
                _ItemSource.Remove(o);
            }
        }
        else
        {
            SqliteService.DeleteTableData("Favorite", $"Url='{param}'");
            var list1 = _FavoriteSource.Where(o => o.Url == param).ToArray();
            var list2 = _ItemSource.Where(o => o.Url == param).ToArray();
            foreach (var o in list1)
            {
                _FavoriteSource0.Remove(o);
                _FavoriteSource.Remove(o);
            }
            foreach (var o in list2)
            {
                _ItemSource.Remove(o);
            }
        }
    });
    public static ICommand ClearHistoryCommand = new RelayCommand<string>(async (param) =>
    {
        ContentDialog cd = new ContentDialog();
        cd.XamlRoot =MainViewModel._MainPage.XamlRoot;
        cd.Title = ReswSource.GetString("ClearAllHistory");
        cd.CloseButtonText = ReswSource.GetString("Cancle");
        cd.Content = "";
        cd.PrimaryButtonText = ReswSource.GetString("OK");
        cd.PrimaryButtonClick += (ss, ee) =>
        {
            _HistorySource0.Clear();
            _HistorySource.Clear();
            _ItemSource.Clear();
            SqliteService.DeleteTableData("History", "");
        };
        await cd.ShowAsync();
        
    });
    public static ICommand ListDetailsViewControl_SelectionChanged = new RelayCommand<IList<object>>((param) =>
    {
        if (param != null)
        {
            _ItemSource.Clear();
            foreach (var i in param)
            {
                var m = i as WebModel;
                if (m != null)
                {
                    _ItemSource.Add(m);
                }
            }
        }
    });
    public static ICommand HistoryInit_Command = new RelayCommand<object>((param) =>
    {
        _HistorySource0.Clear();
        _HistorySource.Clear();
        var query = SqliteService.ReadTableData("History", "Name,Url,Date", "", "");
        while (query.Read())
        {
            _HistorySource0.Insert(0,new WebModel { Title = query.GetString(0), Url = query.GetString(1), Date = query.GetString(2) });
            _HistorySource.Insert(0, new WebModel { Title = query.GetString(0), Url = query.GetString(1), Date = query.GetString(2) });
        }
        SqliteService.db.Close();
    });
    public static ICommand FavInit_Command = new RelayCommand<object>((param) =>
    {
        _FavoriteSource0.Clear();
        _FavoriteSource.Clear();
        var query = SqliteService.ReadTableData("Favorite", "Name,Url,Date", "", "");
        while (query.Read())
        {
            _FavoriteSource0.Insert(0, new WebModel { Title = query.GetString(0), Url = query.GetString(1), Date = query.GetString(2) });
            _FavoriteSource.Insert(0, new WebModel { Title = query.GetString(0), Url = query.GetString(1), Date = query.GetString(2) });
        }
        SqliteService.db.Close();
    });
    public static bool _IsHisOrFav;
    public void HisButton_Click(object sender, RoutedEventArgs e)
    {
        _IsHisOrFav = true;
        HistoryInit_Command.Execute(null);
        MainViewModel.OpenHistoryCommand.Execute(null);
    }
    public void ButtonAddToHome_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.ButtonAddToHome_ClickCommand.Execute(((Button)sender).Tag.ToString());
    }
    public void FavButton_Click(object sender, RoutedEventArgs e)
    {
        _IsHisOrFav = false;
        FavInit_Command.Execute(null);
        MainViewModel.OpenFavCommand.Execute(null);
    }
    public void GoButton_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute((sender as TextBox).Text);
    }
    public void MoreButton_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.OpenSettingCommand.Execute(null);
    }
    public  void tb_url_KeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            MainViewModel.OpenWebPageCommand.Execute((sender as TextBox).Text);
        }
    } 
    public  void tb_url_GotFocus(object sender, RoutedEventArgs e)
    {
        (sender as TextBox).SelectAll();
    }
    static bool IsHisOrFav()
    {
        var tabitem = MainViewModel._MainPage.tabView.SelectedItem as TabViewItem;
        if (tabitem.Header.ToString().Equals(ReswSource.GetString("History")))
        {
            return true;
        }
        else 
        { 
            return false;
        }
    }
}
