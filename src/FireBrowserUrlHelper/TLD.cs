using System;
using Windows.Storage;

namespace FireBrowserUrlHelper
{
    public class TLD
    {
        public static string KnownDomains { get; set; }
        public static async void LoadKnownDomains()
        {
            StorageFile assets = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FireBrowserUrlHelper/List/public_domains.txt"));
            KnownDomains = await FileIO.ReadTextAsync(assets);
        }

        public static string GetTLDfromURL(string url)
        {
            int pos = url.LastIndexOf(".") + 1;
            string tld = url.Substring(pos, url.Length - pos);
            return tld;
        }
    }
}
