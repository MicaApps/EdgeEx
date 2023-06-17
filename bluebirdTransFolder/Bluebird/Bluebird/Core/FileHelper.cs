using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Bluebird.Core;

public class FileHelper
{
    public static async System.Threading.Tasks.Task DeleteLocalFile(string fileName)
    {
        var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
        if (file != null)
        {
            await file.DeleteAsync();
        }
    }
}