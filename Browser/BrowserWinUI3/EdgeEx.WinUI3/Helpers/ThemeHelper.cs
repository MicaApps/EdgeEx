using Microsoft.UI.Xaml;
using WinUIEx;

namespace EdgeEx.WinUI3.Helpers;
/// <summary>
/// Class providing functionality around switching and restoring theme settings
/// <br/>Copy From WinUI Gallery
/// </summary>
public static class ThemeHelper
{
    private const string SelectedAppThemeKey = "SelectedAppTheme";

    private static WindowEx CurrentApplicationWindow;

    /// <summary>
    /// Gets the current actual theme of the app based on the requested theme of the
    /// root element, or if that value is Default, the requested theme of the Application.
    /// </summary>
    /*public static ElementTheme ActualTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    if (rootElement.RequestedTheme != ElementTheme.Default)
                    {
                        return rootElement.RequestedTheme;
                    }
                }
            }

            return EnumHelper.GetEnum<ElementTheme>(App.Current.RequestedTheme.ToString());
        }
    }*/

    /// <summary>
    /// Gets or sets (with LocalSettings persistence) the RequestedTheme of the root element.
    /// </summary>
    public static ElementTheme RootTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    return rootElement.RequestedTheme;
                }
            }

            return ElementTheme.Default;
        }
        set
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = value;
                }
            }
            //ConfigHelper.Set(SelectedAppThemeKey, value.ToString());
        }
    }

    public static void Initialize(WindowEx window)
    {
        CurrentApplicationWindow = window;
        //string savedTheme = ConfigHelper.GetString(SelectedAppThemeKey);
        //if (savedTheme != null)
        //{
        //    RootTheme = EnumHelper.GetEnum<ElementTheme>(savedTheme);
        //}
    }

    public static bool IsDarkTheme()
    {
        if (RootTheme == ElementTheme.Default)
        {
            return Application.Current.RequestedTheme == ApplicationTheme.Dark;
        }
        return RootTheme == ElementTheme.Dark;
    }
}
