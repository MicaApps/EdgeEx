﻿using CommunityToolkit.WinUI.Helpers;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Serilog;
using WinRT;
using WinUIEx;

namespace EdgeEx.WinUI3.Helpers;
/// <summary>
/// WindowSystemBackdrops Controller
/// <br/>Part Copy From WinUI Gallery
/// </summary>
public class SystemBackdropsHelper
{
    private LocalSettingsToolkit localSettingsToolkit;

    private WindowEx window;
    public WindowEx Window { get => window; }
    private WindowBackdrop currentBackdrop;
    public WindowBackdrop CurrentBackdrop {
        get => currentBackdrop;
        set{
            localSettingsToolkit.Set(LocalSettingName.Backdrop, value.ToString());
            currentBackdrop = value;
        }
    }
    private MicaController m_micaController;
    public MicaController WindowMicaController { get => m_micaController; }
    private DesktopAcrylicController m_acrylicController;
    public DesktopAcrylicController WindowAcrylicController { get => m_acrylicController; }
    private SystemBackdropConfiguration m_configurationSource;
    public SystemBackdropsHelper(WindowEx window)
    {
        this.window = window;
        localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
        
        if (localSettingsToolkit.GetString(LocalSettingName.Backdrop) is string backdrop && backdrop == WindowBackdrop.Mica.ToString())
        {
            CurrentBackdrop = WindowBackdrop.Mica;
        }
        else
        {
            // Default set Acrylic
            CurrentBackdrop = WindowBackdrop.Acrylic;
        }
        if (IsDefault)
        {
            // Default set
            SetBackdrop(CurrentBackdrop, null);
        }
        else
        {
            if(CurrentBackdrop== WindowBackdrop.Acrylic)
            {
                SetBackdrop(CurrentBackdrop, new DesktopAcrylicController()
                {
                    TintColor = localSettingsToolkit.GetString(LocalSettingName.AcrylicTintColor).ToColor(),
                    TintOpacity = localSettingsToolkit.GetFloat(LocalSettingName.AcrylicTintOpacity),
                    FallbackColor = localSettingsToolkit.GetString(LocalSettingName.AcrylicFallbackColor).ToColor(),
                });
            }
            else
            {
                Log.Information(localSettingsToolkit.GetString(LocalSettingName.MicaKind));
                SetBackdrop(CurrentBackdrop, new MicaController()
                {
                   Kind = EnumHelper.GetEnum<MicaKind>(localSettingsToolkit.GetString(LocalSettingName.MicaKind)),
                });
            }
        }
        
    }
    public bool IsDefault
    {
        get => !(localSettingsToolkit.GetString(LocalSettingName.AcrylicTintColor) is string);
    }
    public static WindowBackdrop IsMica { 
        get{
            
            LocalSettingsToolkit localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
            if (localSettingsToolkit.GetString(LocalSettingName.Backdrop) is string backdrop && backdrop == WindowBackdrop.Mica.ToString())
            {
                return WindowBackdrop.Mica;
            }
            else
            {
                localSettingsToolkit.Set(LocalSettingName.Backdrop, WindowBackdrop.Acrylic.ToString());
                return WindowBackdrop.Acrylic;
            }
        } 
    }
    
    /// <summary>
    /// Reset to default color. If the requested type is supported, we'll update to that.
    /// Note: This completely removes any previous controller to reset to the default state.
    /// </summary>
    /// <param name="type"></param>
    public void SetBackdrop(WindowBackdrop type, ISystemBackdropController controller = null)
    {
        if (m_micaController != null)
        {
            m_micaController.Dispose();
            m_micaController = null;
        }
        if (m_acrylicController != null)
        {
            m_acrylicController.Dispose();
            m_acrylicController = null;
        }
        this.window.Activated -= Window_Activated;
        this.window.Closed -= Window_Closed;
        ((FrameworkElement) this.window.Content).ActualThemeChanged -= Window_ThemeChanged;
        m_configurationSource = null;
        if(controller != null)
        {
            if(controller is DesktopAcrylicController)
            {
                m_acrylicController = (DesktopAcrylicController)controller;
            }
            else if(controller is MicaController)
            {
                m_micaController = (MicaController)controller;
            }
        }
        if (type == WindowBackdrop.Mica)
        {
            if (TrySetMicaBackdrop())
            {
                CurrentBackdrop = type;
            }
            else
            {
                // Mica isn't supported. Try Acrylic.
                type = WindowBackdrop.Acrylic;
            }
        }
        if (type == WindowBackdrop.Acrylic)
        {
            if (TrySetAcrylicBackdrop())
            {
                CurrentBackdrop = type;
            }
            else
            {
                // Acrylic isn't supported, so take the next option, which is DefaultColor, which is already set.
            }
        }
    }
    private bool TrySetMicaBackdrop()
    {
        if (MicaController.IsSupported())
        {
            // Hooking up the policy object
            m_configurationSource = new SystemBackdropConfiguration();
            this.window.Activated += Window_Activated;
            this.window.Closed += Window_Closed;
            ((FrameworkElement) this.window.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            m_micaController ??= new MicaController();
            Log.Information("Set Mica WindowBackdrop:Kind={Kind}", m_micaController.Kind);
            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_micaController.AddSystemBackdropTarget(this.window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }
    private bool TrySetAcrylicBackdrop()
    {
        if (DesktopAcrylicController.IsSupported())
        {
            // Hooking up the policy object
            m_configurationSource = new SystemBackdropConfiguration();
            this.window.Activated += Window_Activated;
            this.window.Closed += Window_Closed;
            ((FrameworkElement) this.window.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();
            AcrylicBrush defaultAcrylicBrush = Application.Current.Resources["DefaultEdgeExAcrylicBrush"] as AcrylicBrush;
            if (m_acrylicController == null)
            {
                m_acrylicController = new DesktopAcrylicController
                {
                    TintColor = defaultAcrylicBrush.TintColor,
                    TintOpacity = (float)defaultAcrylicBrush.TintOpacity,
                    FallbackColor = defaultAcrylicBrush.FallbackColor,
                };
            }
            Log.Information("Set Acrylic WindowBackdrop:TintColor={TintColor},TintOpacity={TintOpacity},FallbackColor={FallbackColor}",
                m_acrylicController.TintColor, 
                m_acrylicController.TintOpacity,
                m_acrylicController.FallbackColor);
            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_acrylicController.AddSystemBackdropTarget(this.window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_acrylicController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Acrylic is not supported on this system
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use this closed window.
        if (m_micaController != null)
        {
            m_micaController.Dispose();
            m_micaController = null;
        }
        if (m_acrylicController != null)
        {
            m_acrylicController.Dispose();
            m_acrylicController = null;
        }
        this.window.Activated -= Window_Activated;
        m_configurationSource = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (m_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        m_configurationSource.Theme = (((FrameworkElement)this.window.Content).ActualTheme) switch
        {
            ElementTheme.Dark => SystemBackdropTheme.Dark,
            ElementTheme.Light => SystemBackdropTheme.Light,
            ElementTheme.Default => SystemBackdropTheme.Default,
            _ => throw new System.NotImplementedException(),
        };
    } 

}