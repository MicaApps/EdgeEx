using FireBrowserDataBase;
using FireBrowserDialogs.DialogTypes.AreYouSureDialog;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages.TimeLine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HistoryTime : Page
    {
        public HistoryTime()
        {
            this.InitializeComponent();
            FetchBrowserHistory();
        }

        private void FetchBrowserHistory()
        {
            Batteries.Init();

            try
            {
                // Create a connection to the SQLite database
                var connectionStringBuilder = new SqliteConnectionStringBuilder();
                connectionStringBuilder.DataSource = Path.Combine(ApplicationData.Current.LocalFolder.Path, "History.db");

                using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Define the SQL query to fetch the browser history
                    string sql = "SELECT url, title, visit_count, last_visit_time FROM urlsDb ORDER BY last_visit_time DESC";

                    // Create a command object with the SQL query and connection
                    using (SqliteCommand command = new SqliteCommand(sql, connection))
                    {
                        // Execute the SQL query and get the results
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            // Create a list to store the browser history items
                            List<HistoryItem> historyItems = new List<HistoryItem>();

                            // Iterate through the query results and create a BrowserHistoryItem for each row
                            while (reader.Read())
                            {
                                HistoryItem historyItem = new HistoryItem
                                {
                                    Url = reader.GetString(0),
                                    Title = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    VisitCount = reader.GetInt32(2),
                                    LastVisitTime = DateTimeOffset.FromFileTime(reader.GetInt64(3)).DateTime
                                };

                                var item = historyItem;
                                item.ImageSource = new BitmapImage(new Uri("https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=" + item.Url + "&size=32"));
                                historyItems.Add(historyItem);
                            }


                            // Bind the browser history items to the ListView
                            BigTemp.ItemsSource = historyItems;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that might be thrown during the execution of the code
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchBrowserHistory();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            SureDialog customDialog = new SureDialog(); // Create an instance of your custom content dialog class
            customDialog.State = DialogState.History; // Set the state of the dialog 
            ContentDialogResult result = await customDialog.ShowAsync(); // Show the dialog and wait for the user to respond
            if (result == ContentDialogResult.Primary)
            {
                DbClear.ClearDb();
                BigTemp.ItemsSource = null;
            }
            else
            {
                // User clicked "Cancel" button or closed the dialog
            }
        }

        private void Ts_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
