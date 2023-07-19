using EdgeEx.WinUI3.Enums;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.UI;
using WinUIEx;

namespace EdgeEx.WinUI3.Args
{
    public class WindowBackdropChangedEventArg
    {
        public WindowBackdrop OldMode { get; set; }
        public WindowBackdrop NewMode { get; set; }
        public Color TintColor { get; set; }
        public Color FallbackColor { get; set; }
        public float TintOpacity { get; set; }
        public MicaKind Kind { get; set; }
        public WindowBackdropChangedEventArg() { }
        public WindowBackdropChangedEventArg(WindowBackdrop oldMode, WindowBackdrop newMode, Color tintColor, Color fallbackColor, float tintOpacity, MicaKind kind)
        {
            OldMode = oldMode;
            NewMode = newMode;
            TintColor = tintColor;
            FallbackColor = fallbackColor;
            TintOpacity = tintOpacity;
            Kind = kind;
        }
    }
}
