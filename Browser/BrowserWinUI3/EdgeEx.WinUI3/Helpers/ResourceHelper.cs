using EdgeEx.WinUI3.Enums;
using Windows.ApplicationModel.Resources;

namespace EdgeEx.WinUI3.Helpers
{
    /// <summary>
    /// ResourceHelper
    /// </summary>
    internal class ResourceHelper
    {
        private static readonly ResourceLoader loader = new ResourceLoader();
        public static string GetString(string resourceName)
        {
            return loader.GetString(resourceName);
        }
        /// <summary>
        /// Get the language from the resw file
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetString(ResourceKey resourceKey)
        {
            return loader.GetString(resourceKey.ToString());
        }
    }
}