using System;
using System.Linq;

namespace FireBrowserUrlHelper
{
    public class UrlHelper
    {
        public static string GetInputType(string input)
        {
            string type = "searchquery";
            string tld = TLD.GetTLDfromURL(input);

            if (Uri.TryCreate(input, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                type = "url";
            }
            else if (input.Contains(".") && TLD.KnownDomains.Any(tld.Contains))
            {
                type = "urlNOProtocol";
            }

            return type;
        }
    }
}
