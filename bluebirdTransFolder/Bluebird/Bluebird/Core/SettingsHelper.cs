using Windows.Storage;

namespace Bluebird.Core;

public static class SettingsHelper
{
    private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

    public static string GetSetting(string Setting)
    {
        string SettingValue = localSettings.Values[Setting] as string;
        return SettingValue;
    }

    public static void SetSetting(string Setting, string SettingValue)
    {
        localSettings.Values[Setting] = SettingValue;
    }
}