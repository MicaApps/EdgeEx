using System;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages.SettingsPages
{
    public sealed partial class MsLogin : ContentDialog
    {

        public MsLogin()
        {
            this.InitializeComponent();
            Login();
        }
        public async void Login()
        {
            await WebApp.EnsureCoreWebView2Async();
            WebApp.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            WebApp.Source = new Uri("https://login.live.com/");
        }
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            WebApp.Close();

            // Get a reference to the local folder
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            // Get a reference to the JSON file
            StorageFile file = await localFolder.GetFileAsync("Params.json");

            string jsonText = await FileIO.ReadTextAsync(file);

            // Parse the JSON string into a JsonObject
            JsonObject jsonObject = JsonObject.Parse(jsonText);

            jsonObject["IsConnected"] = JsonValue.CreateBooleanValue(false);

            // Save the updated JsonObject back to the JSON file
            await FileIO.WriteTextAsync(file, jsonObject.ToString());
        }

        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            WebApp.Close();

            // Load the JSON file
            // Get a reference to the local folder
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            // Get a reference to the JSON file
            StorageFile file = await localFolder.GetFileAsync("Params.json");

            string jsonText = await FileIO.ReadTextAsync(file);

            // Parse the JSON string into a JsonObject
            JsonObject jsonObject = JsonObject.Parse(jsonText);

            jsonObject["IsConnected"] = JsonValue.CreateBooleanValue(true);

            // Save the updated JsonObject back to the JSON file
            await FileIO.WriteTextAsync(file, jsonObject.ToString());
        }
    }
}
