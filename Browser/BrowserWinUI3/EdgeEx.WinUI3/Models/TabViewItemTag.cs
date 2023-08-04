using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Models
{
    public class TabViewItemTag
    {
        public bool CanGoBack { get; set; }
        public bool CanGoForward { get; set; }
        public bool CanRefresh { get; set; }
        public Uri NavigateUri { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public bool IsBox { get; set; } = false;
        public TabViewItemTag() { }
        public Visibility Not(bool isBox)
        {
            return isBox ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
