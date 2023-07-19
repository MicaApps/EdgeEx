using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Interfaces;
using Microsoft.UI.Composition.SystemBackdrops;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using WinUIEx;

namespace EdgeEx.WinUI3.Toolkits
{
    public class CallerToolkit : ICallerToolkit
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<WindowBackdropChangedEventArg> WindowBackdropChangedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ChangeWindowBackdrop(WindowBackdrop oldMode, WindowBackdrop newMode, Color tintColor, Color fallbackColor, float tintOpacity, MicaKind kind)
        {
            Log.Information(tintOpacity.ToString());
            WindowBackdropChangedEvent?.Invoke(this, new WindowBackdropChangedEventArg(oldMode, newMode, tintColor, fallbackColor, tintOpacity, kind));
        }
    }
}
