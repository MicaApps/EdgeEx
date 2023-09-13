using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Extensions
{
    public static class CommonExtension
    {
        /// <summary>
        /// bool to Visibility
        /// </summary>
        public static Visibility ToVisibility(this bool f)
        {
            return f? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// TimeStamp to DateTime
        /// </summary>
        /// <param name="timeStamp">timestamp</param>
        /// <param name="inMilli">is millisecond</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timeStamp, bool inMilli = false)
        {
            DateTimeOffset dateTimeOffset = inMilli ? DateTimeOffset.FromUnixTimeMilliseconds(timeStamp) : DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            return dateTimeOffset.LocalDateTime;
        }
    }
}
