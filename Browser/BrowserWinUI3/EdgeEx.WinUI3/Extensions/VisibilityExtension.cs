using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Extensions
{
    public static class VisibilityExtension
    {
        public static Visibility ToVisibility(this bool f)
        {
            return f? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
