using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using Microsoft.UI.Composition.SystemBackdrops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using WinUIEx;

namespace EdgeEx.WinUI3.Interfaces
{
    public interface ICallerToolkit
    {
        /// <summary>
        /// Window Backdrop Changed Event
        /// </summary>
        event EventHandler<WindowBackdropChangedEventArg> WindowBackdropChangedEvent;
        /// <summary>
        /// Change Window Backdrop  
        /// </summary>
        void ChangeWindowBackdrop(WindowBackdrop oldMode, WindowBackdrop newMode, Color tintColor, Color fallbackColor, float tintOpacity, MicaKind kind);
    }
}
