using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
        static public bool IsMica()
        {
            LocalSettingsToolkit localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
            if (localSettingsToolkit.GetString(LocalSettingName.Backdrop) is string backdrop && backdrop == WindowBackdrop.Mica.ToString())
            {
                return true;
            }
            else
            {
                localSettingsToolkit.Set(LocalSettingName.Backdrop, WindowBackdrop.Acrylic.ToString());
                return false;
            }
        }
        static public void SetWindowBackdrop(WindowEx window, WindowBackdrop backdrop)
        {
            if(backdrop == WindowBackdrop.Acrylic)
            {
                if (window.SystemBackdrop is DesktopAcrylicBackdrop) return;
                window.SystemBackdrop = new DesktopAcrylicBackdrop();
            }
            else
            {
                if (window.SystemBackdrop is MicaBackdrop) return;
                window.SystemBackdrop = new MicaBackdrop();
            }
        }
        public static WindowEx MainWindow { get; set; }
        public static List<WindowEx> ActiveWindows { get { return _activeWindows; } }

        private static List<WindowEx> _activeWindows = new List<WindowEx>();
    }
}
