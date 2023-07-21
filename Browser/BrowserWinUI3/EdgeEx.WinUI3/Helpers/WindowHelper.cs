using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Serilog;
using System.Collections.Generic;
using System.Xml.Linq;
using Windows.UI;
using WinRT;
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
            _systemBackdropsHelpers.Add(new SystemBackdropsHelper(window));
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
        static public WindowEx GetWindowForPersistenceId(string persistenceId)
        {
            foreach (WindowEx window in _activeWindows)
            {
                if (persistenceId == window.PersistenceId)
                {
                    return window;
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
        public static List<SystemBackdropsHelper> SystemBackdropsHelpers { get { return _systemBackdropsHelpers; } }

        private static List<WindowEx> _activeWindows = new List<WindowEx>();
        private static List<SystemBackdropsHelper> _systemBackdropsHelpers = new List<SystemBackdropsHelper>();
    }
}
