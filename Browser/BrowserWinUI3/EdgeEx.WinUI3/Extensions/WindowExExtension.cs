using EdgeEx.WinUI3.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace EdgeEx.WinUI3.Extensions;
public static class WindowExExtension
{
    /// <summary>
    /// get SystemBackdropsHelper by window
    /// </summary>
    public static SystemBackdropsHelper GetSystemBackdropsHelper(this WindowEx window)
    {
        foreach (var item in WindowHelper.SystemBackdropsHelpers)
        {
            if (item.Window == window)
            {
                return item;
            }
        }
        return null;
    }
}