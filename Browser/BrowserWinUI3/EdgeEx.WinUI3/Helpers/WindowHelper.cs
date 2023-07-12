using Microsoft.UI.Xaml;
using System.Collections.Generic;
using WinUIEx;

namespace EdgeEx.WinUI3.Helpers
{
    // Copy From WinUI 3 Gallery
    public static class WindowHelper
    {
        static public WindowEx CreateWindow()
        {
            WindowEx newWindow = new WindowEx();
            TrackWindow(newWindow);
            return newWindow;
        }
        static public void TrackWindow(WindowEx window)
        {
            window.Closed += (sender, args) => {
                _activeWindows.Remove(window);
            };
            _activeWindows.Add(window);
        }
        static public void ColseWindow(WindowEx window)
        {
            window.Close();
        }
        static public WindowEx GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (WindowEx window in _activeWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
        static public WindowEx GetWindowForXamlRoot(XamlRoot xamlRoot)
        {
            if (xamlRoot != null)
            {
                foreach (WindowEx window in _activeWindows)
                {
                    if (xamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
        public static WindowEx MainWindow { get; set; }
        public static List<WindowEx> ActiveWindows { get { return _activeWindows; } }

        private static List<WindowEx> _activeWindows = new List<WindowEx>();
    }
}
