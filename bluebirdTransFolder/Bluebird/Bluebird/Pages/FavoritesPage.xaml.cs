using Bluebird.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Bluebird.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            this.InitializeComponent();
            LoadFavorites();
        }

        private async void LoadFavorites()
        {
            JsonItemsList = await Json.GetListFromJsonAsync("Favorites.json");
            if (JsonItemsList != null) FavoritesListView.ItemsSource = JsonItemsList;
            else
            {
                FavoritesListView.ItemsSource = null;
            }
        }

        private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get listview sender
            ListView listView = sender as ListView;
            if (listView.ItemsSource != null)
            {
                // Get selected item
                JsonItems item = (JsonItems)listView.SelectedItem;
                launchurl = item.Url;
                MainPageContent.CreateWebTab();
            }
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            // Get all ListView items with the submitted search query
            var SearchResults = from s in JsonItemsList where s.Title.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase) select s;
            // Set SearchResults as ItemSource for HistoryListView
            FavoritesListView.ItemsSource = SearchResults;
        }
    }
}
