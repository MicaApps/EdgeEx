using FireBrowserCore.Models;
using FireBrowserDialogs.DialogTypes.AreYouSureDialog;
using FireBrowserFavorites;
using FireBrowserInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using static FireBrowserFavorites.Globals;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages.TimeLine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Favorites : Page
    {
        public Favorites()
        {
            this.InitializeComponent();
            LoadFavorites();
        }

        public List<Globals.JsonItems> JsonItemsList { get; private set; }

        private async void LoadFavorites()
        {
            JsonItemsList = await Json.GetListFromJsonAsync("Favorites.json");
            if (JsonItemsList != null)
                FavoritesListView.ItemsSource = JsonItemsList;
            else
                FavoritesListView.ItemsSource = null;
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            // Get all ListView items with the submitted search query
            var SearchResults = from s in JsonItemsList where s.Title.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase) select s;
            // Set SearchResults as ItemSource for HistoryListView
            FavoritesListView.ItemsSource = SearchResults;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SureDialog customDialog = new SureDialog(); // Create an instance of your custom content dialog class
            customDialog.State = DialogState.Favorites; // Set the state of the dialog
            ContentDialogResult result = await customDialog.ShowAsync(); // Show the dialog and wait for the user to respond
            if (result == ContentDialogResult.Primary)
            {
                await FileHelper.DeleteFile("Favorites.json");
                FavoritesListView.ItemsSource = null;// User clicked "OK" button
            }
            else
            {
                // User clicked "Cancel" button or closed the dialog
            }
        }

        string ctmtext;
        string ctmurl;
        private async void FavContextItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as AppBarButton).Tag)
            {
                case "OpenLnkInNewWindow":
                    await Launcher.LaunchUriAsync(new Uri($"{ctmurl}"));
                    break;
                case "Copy":
                    SystemHelper.WriteStringToClipboard(ctmurl);
                    break;
                case "CopyText":
                    SystemHelper.WriteStringToClipboard(ctmtext);
                    break;
                // link context menu
                case "ShareLink":
                    SystemHelper.ShowShareUIURL(ctmtext, ctmurl);
                    break;
            }
            FavoritesContextMenu.Hide();
        }

        private void FavoritesListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            var options = new FlyoutShowOptions()
            {
                Position = e.GetPosition(listView),
            };
            FavoritesContextMenu.ShowAt(listView, options);
            var item = ((FrameworkElement)e.OriginalSource).DataContext as JsonItems;
            ctmtext = item.Title;
            ctmurl = item.Url;
        }
    }
}
