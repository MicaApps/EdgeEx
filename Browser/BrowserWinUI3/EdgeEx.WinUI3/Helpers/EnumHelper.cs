using System;
using System.Reflection;

namespace EdgeEx.WinUI3.Helpers;
/// <summary>
/// Enum Helper
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// Get Enum by string
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
    {
        if (!typeof(TEnum).GetTypeInfo().IsEnum)
        {
            throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
        }
        return (TEnum)Enum.Parse(typeof(TEnum), text);
    }
}