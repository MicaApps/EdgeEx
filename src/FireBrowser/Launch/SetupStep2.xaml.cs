using FireBrowser.Pages.SettingsPages;
using Newtonsoft.Json;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static FireBrowser.App;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Launch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetupStep2 : Page
    {
        public SetupStep2()
        {
            this.InitializeComponent();
            FireBrowserInterop.SettingsHelper.SetSetting("LightMode", "0");
        }

        private void LgMode_Toggled(object sender, RoutedEventArgs e)
        {
            FireBrowserInterop.SettingsHelper.SetSetting("LightMode", LgMode.IsOn ? "1" : "0");
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            MsLogin ms = new MsLogin();
            ms.ShowAsync();
        }

        private async void Install_Click(object sender, RoutedEventArgs e)
        {
            bool isFirstLaunch = true;

            var settingsFile = await ApplicationData.Current.LocalFolder.GetFileAsync("Params.json");
            string settingsJson = await FileIO.ReadTextAsync(settingsFile);
            AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(settingsJson);

            if (settings != null && settings.IsFirstLaunch)
            {
                // The app has been launched before, but this is the first launch after an update
                isFirstLaunch = true;
                settings.IsFirstLaunch = false; // Set the IsFirstLaunch property to false
                string updatedSettingsJson = JsonConvert.SerializeObject(settings);
                await FileIO.WriteTextAsync(settingsFile, updatedSettingsJson); // Save the updated settings
            }

            if (tbv.Text.Equals("#000000"))
            {
                FireBrowserInterop.SettingsHelper.SetSetting("ColorTool", "#000000");
            }
            if (tbc.Text.Equals("#000000"))
            {
                FireBrowserInterop.SettingsHelper.SetSetting("ColorTV", "#000000");
            }

            FireBrowserInterop.SystemHelper.RestartApp();
        }

        private void tbv_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = tbv.Text;
            FireBrowserInterop.SettingsHelper.SetSetting("ColorTool", value);
        }

        private void tbc_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = tbc.Text;
            FireBrowserInterop.SettingsHelper.SetSetting("ColorTV", value);
        }
    }
}
