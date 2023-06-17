using System.Collections.Generic;
using Windows.Storage;

namespace FireBrowserFavorites
{
    public class Globals
    {
        public class JsonItems
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }

        // Global variables
        public static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public static List<JsonItems> JsonItemsList;
    }
}
