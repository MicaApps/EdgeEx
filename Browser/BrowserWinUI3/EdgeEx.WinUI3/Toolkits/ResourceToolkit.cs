using EdgeEx.WinUI3.Enums;
using Windows.ApplicationModel.Resources;

namespace EdgeEx.WinUI3.Toolkits
{
    /// <summary>
    /// Toolkit that i18n
    /// </summary>
    public class ResourceToolkit
    {
        private static readonly ResourceLoader loader = new ResourceLoader();
        /// <summary>
        /// Get the language from the resw file by string
        /// </summary>
        public string GetString(string resourceName)
        {
            return loader.GetString(resourceName);
        }
        /// <summary>
        /// Get the language from the resw file by <see cref="ResourceKey"/>
        /// </summary>
        public string GetString(ResourceKey resourceKey)
        {
            return loader.GetString(resourceKey.ToString());
        }
    }
}