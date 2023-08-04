
using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Models;
using EdgeEx.WinUI3.Toolkits;
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
        private LocalSettingsToolkit _localSettingsToolkit;
        [ObservableProperty]
        private bool canGoBack;
        [ObservableProperty]
        private bool canGoForward;
        [ObservableProperty]
        private bool isApp;
        [ObservableProperty]
        private bool isFavorite;
        [ObservableProperty]
        private EdgeExTabItem selectedItem;
        [ObservableProperty]
        private int selectedIndex;
        public ObservableCollection<EdgeExTabItem> TabItems { get; set; } = new ObservableCollection<EdgeExTabItem>();
        public MainViewModel(LocalSettingsToolkit localSettingsToolkit, ISqlSugarClient sqlSugarClient)
        {
            db = sqlSugarClient;
            _localSettingsToolkit = localSettingsToolkit;
        }
        public bool CheckAddressTab()
        {
            return _localSettingsToolkit.GetBoolean(Enums.LocalSettingName.AddressTabMode);
        }
        public void CheckFavorite(Uri uri)
        {
            string url = uri.ToString();
            IsFavorite = db.Queryable<Bookmark>().First(x => x.Uri == url) is Bookmark;
        }
    }
}
