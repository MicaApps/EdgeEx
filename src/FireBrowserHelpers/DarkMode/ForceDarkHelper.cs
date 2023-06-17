using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FireBrowserHelpers.DarkMode
{
    public class ForceDarkHelper
    {
        public static async Task<string> GetForceDark()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FireBrowserHelpers/DarkMode/Jscript/forcedark.js"));
            string jscript = await FileIO.ReadTextAsync(file);
            return jscript;
        }
    }
}
