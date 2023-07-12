using EdgeEx.WinUI3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EdgeEx.WinUI3.Helpers
{
    
    class LocalSettingsHelper
    {
        /// <summary>
        /// Does the setting name exist in the local app settings
        /// </summary>
        /// <param name="settingName">setting name</param>
        public static bool Contains(LocalSettingName settingName)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(settingName.ToString());
        }
        #region  Set
        private static void Set(LocalSettingName settingName, object value) {
            ApplicationData.Current.LocalSettings.Values[settingName.ToString()] = value;
        }
        public static void Set(LocalSettingName settingName, string value)
        {
            Set(settingName, (object)value);
        }
        #endregion

        #region  Get
        public static object Get(LocalSettingName settingName)
        {
            if (Contains(settingName))
            {
                return ApplicationData.Current.LocalSettings.Values[settingName.ToString()];
            }
            return null;
        }
        public static string GetString(LocalSettingName settingName)
        {
            return (string)Get(settingName);
        }
        #endregion
    }
}
