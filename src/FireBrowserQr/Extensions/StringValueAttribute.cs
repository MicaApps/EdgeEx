using System;

namespace FireBrowserQr.Extensions
{
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the alue in an enum
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        /// <summary>
        /// Init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }

    public static class CustomExtensions
    {
        /// <summary>
        /// Will get the string value for a given enum's value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
#if NETSTANDARD1_3
            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
#else
            var fieldInfo = value.GetType().GetField(value.ToString());
#endif
            var attr = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            return attr.Length > 0 ? attr[0].StringValue : null;
        }
    }
}
