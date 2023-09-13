using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Models
{
    public class EdgeExTabItem
    {
        public string Name { get; set; }
        public IconSource IconSource { get; set; }
        public object Content { get; set; }
        public TabViewItemTag Tag { get; set; }
        public EdgeExTabItem() { }
    }
}
