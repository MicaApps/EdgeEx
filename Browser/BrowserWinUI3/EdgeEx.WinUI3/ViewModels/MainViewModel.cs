
using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Models;
using Microsoft.UI.Xaml.Controls;
using SqlSugar;
using System;
using System.Collections.ObjectModel;
using System.Security.Policy;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private ISqlSugarClient db;
        [ObservableProperty]
        private bool canGoBack;
        [ObservableProperty]
        private bool canGoForward;
        [ObservableProperty]
        private bool isApp;
        [ObservableProperty]
        private bool isFavorite;
        public MainViewModel(ISqlSugarClient sqlSugarClient)
        {
            db = sqlSugarClient;
        }
        public void CheckFavorite(Uri uri)
        {
            string url = uri.ToString();
            IsFavorite = db.Queryable<BookMark>().First(x => x.Uri == url) is BookMark;
        }
    }
}
