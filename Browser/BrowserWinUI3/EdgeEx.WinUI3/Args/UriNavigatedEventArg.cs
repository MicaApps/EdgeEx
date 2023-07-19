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
        public UriNavigatedEventArg(Uri uri)
        {
            this.NavigatedUri = uri;
        }
        public UriNavigatedEventArg() { }
    }
}
