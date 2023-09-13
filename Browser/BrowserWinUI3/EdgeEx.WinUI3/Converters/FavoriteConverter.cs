using Microsoft.UI.Xaml.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Converters
{
    public class FavoriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
         {
             bool isFavorite = (bool) value;
             return isFavorite ? "\uE735" : "\uE734";
         }
         public object ConvertBack(object value, Type targetType, object parameter, string language)
         {
             throw new NotImplementedException();
         }
    }
}
