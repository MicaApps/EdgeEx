using System;
using Windows.Storage;

namespace FireBrowserCore.Models
{
    public class FileHelper
    {
        public static async System.Threading.Tasks.Task DeleteFile(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            if (file != null)
                await file.DeleteAsync();
        }
    }
}
