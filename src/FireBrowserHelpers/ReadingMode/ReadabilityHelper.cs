using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FireBrowserHelpers.ReadingMode
{
    public class ReadabilityHelper
    {
        public static async Task<string> GetReadabilityScriptAsync()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FireBrowserHelpers/ReadingMode/Jscript/readability.js"));
            string jscript = await FileIO.ReadTextAsync(file);
            return jscript;
        }
    }
}
