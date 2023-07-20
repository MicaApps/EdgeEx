using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    public class NavigatePageArg
    {
        public string TabItemName { get; set; }
        public Uri NavigateUri { get; set; }
        public NavigatePageArg(string tabItemName, Uri navigateUri)
        {
            TabItemName = tabItemName;
            NavigateUri = navigateUri;
        }
    }
}
