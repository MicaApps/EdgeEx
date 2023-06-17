using Newtonsoft.Json;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            SysInfoBox.Text = "SysInfo: " + FireBrowserInterop.SystemHelper.GetSystemArchitecture();
            check();
        }

        public async void check()
        {
            // Get the local folder object
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            // Get the file object
            StorageFile file = await localFolder.GetFileAsync("Params.json");

            // Read the contents of the file
            string fileContent = await FileIO.ReadTextAsync(file);

            // Parse the JSON string to a dynamic object
            dynamic jsonObject = JsonConvert.DeserializeObject(fileContent);

            // Access the "IsConnected" parameter and check if it's true or false
            bool isConnected = jsonObject.IsConnected;

            if (isConnected == true)
            {
                Status.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                TextStat.Text = "Connected";
            }
            else
            {
                Status.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                TextStat.Text = "Connect";
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StorageFile fileToDelete = await ApplicationData.Current.LocalFolder.GetFileAsync("Params.json");
            await fileToDelete.DeleteAsync();
            FireBrowserInterop.SystemHelper.RestartApp();
        }

        private async void MsAccount_Click(object sender, RoutedEventArgs e)
        {
            MsLogin login = new MsLogin();
            login.ShowAsync();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TestDialog ts = new TestDialog();
            ts.ShowAsync();
        }
    }
}
