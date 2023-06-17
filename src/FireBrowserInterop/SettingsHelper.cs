using Windows.Storage;

namespace FireBrowserInterop
{
    public static class SettingsHelper
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public static string GetSetting(string setting) => localSettings.Values.TryGetValue(setting, out var settingValue) ? settingValue as string : null;
        public static void SetSetting(string setting, string settingValue)
        {
            localSettings.Values[setting] = settingValue;
        }
    }
}
