using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Serilog;
using WinUIEx;

namespace EdgeEx.WinUI3.Helpers;
/// <summary>
/// Class providing functionality around switching and restoring theme settings
/// <br/>Copy From WinUI Gallery
/// </summary>
public static class ThemeHelper
{
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
            foreach (WindowEx window in WindowHelper.ActiveWindows)
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
            foreach (WindowEx window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = value;
                }
            }
            App.Current.Services.GetService<LocalSettingsToolkit>().Set(LocalSettingName.SelectedAppTheme, value.ToString());
        }
    }

    public static void Initialize()
    {
        LocalSettingsToolkit toolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
        string savedTheme = toolkit.GetString(LocalSettingName.SelectedAppTheme);
        if (savedTheme != null)
        {
            Log.Debug("Current Theme {savedTheme}", savedTheme);
            RootTheme = EnumHelper.GetEnum<ElementTheme>(savedTheme);
        }
        else
        {
            toolkit.Set(LocalSettingName.SelectedAppTheme, IsDarkTheme ? "Dark" : "Light");
        }
    }
    public static void InitializeSetting()
    {
        LocalSettingsToolkit toolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
        string savedTheme = toolkit.GetString(LocalSettingName.SelectedAppTheme);
        if (savedTheme == null)
        {
            toolkit.Set(LocalSettingName.SelectedAppTheme, IsDarkTheme ? "Dark" : "Light");
        }
    }
    public static bool IsDarkTheme
    {
        get
        {
            if (RootTheme == ElementTheme.Default)
            {
                return Application.Current.RequestedTheme == ApplicationTheme.Dark;
            }
            return RootTheme == ElementTheme.Dark;
        }
    }
}
