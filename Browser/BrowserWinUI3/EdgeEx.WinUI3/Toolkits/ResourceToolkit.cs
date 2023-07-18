using EdgeEx.WinUI3.Enums;
using Windows.ApplicationModel.Resources;

namespace EdgeEx.WinUI3.Toolkits
{
    /// <summary>
    /// ResourceHelper
    /// </summary>
    public class ResourceToolkit
    {
        private static readonly ResourceLoader loader = new ResourceLoader();
        public string GetString(string resourceName)
        {
            return loader.GetString(resourceName);
        }
        /// <summary>
        /// Get the language from the resw file
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public string GetString(ResourceKey resourceKey)
        {
            return loader.GetString(resourceKey.ToString());
        }
    }
}