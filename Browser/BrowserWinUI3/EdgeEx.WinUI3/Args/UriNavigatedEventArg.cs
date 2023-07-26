using EdgeEx.WinUI3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    public class UriNavigatedEventArg
    {
        public Uri NavigatedUri { get; set; }
        public NavigateTabMode Mode { get; set; }
        public UriNavigatedEventArg(Uri uri, NavigateTabMode mode )
        {
            this.NavigatedUri = uri;
            this.Mode = mode;
        }
        public UriNavigatedEventArg() { }
    }
}
