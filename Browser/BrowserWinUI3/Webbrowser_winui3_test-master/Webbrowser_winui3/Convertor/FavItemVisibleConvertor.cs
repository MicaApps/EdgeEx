using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webbrowser_winui3.Convertor
{
   public  class FavItemVisibleConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var data=value as IEnumerable<object>;
            if (data.Any())
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
