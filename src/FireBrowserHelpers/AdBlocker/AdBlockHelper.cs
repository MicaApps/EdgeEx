using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FireBrowserHelpers.AdBlocker
{
    public class AdBlockHelper
    {
        public static async Task<string> GetAdblockReadAsync()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FireBrowserHelpers/AdBlocker/Jscript/firebrowser-removead.js"));
            string jscript = await FileIO.ReadTextAsync(file);
            return jscript;
        }
    }
}
