using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class SettingsViewModel: ObservableObject
    {
        private LocalSettingsToolkit _localSettingsToolkit;
        private ResourceToolkit _resourceToolkit;
        private ICallerToolkit _callerToolkit;
        public SettingsViewModel(LocalSettingsToolkit localSettingsToolkit, ResourceToolkit resourceToolkit,ICallerToolkit caller)
        {
            _localSettingsToolkit = localSettingsToolkit;
            _resourceToolkit = resourceToolkit;
            _callerToolkit = caller;
            IsTabDragOut = _localSettingsToolkit.GetBoolean(LocalSettingName.IsTabDragOut);
            IsTabDragTo = _localSettingsToolkit.GetBoolean(LocalSettingName.IsTabDragTo);
            AppDataThumbsPath = _localSettingsToolkit.GetString(LocalSettingName.AppDataThumbsPath);
        }
        public DesktopAcrylicController InitDesktopAcrylicController(DesktopAcrylicController controller = null)
        {
            if (ThemeHelper.IsDarkTheme)
            {
                controller ??= new DesktopAcrylicController
                {
                    TintColor = SystemBackdropsHelper.DarkDefaultTintColor,
                    TintOpacity = SystemBackdropsHelper.DarkDefaultTintOpacity,
                    FallbackColor = SystemBackdropsHelper.DarkDefaultFallbackColor,
                };
            }
            else
            {
                controller ??= new DesktopAcrylicController
                {
                    TintColor = SystemBackdropsHelper.LightDefaultTintColor,
                    TintOpacity = SystemBackdropsHelper.LightDefaultTintOpacity,
                    FallbackColor = SystemBackdropsHelper.LightDefaultFallbackColor,
                };
            }
            AcrylicTintColor        = controller.TintColor;
            AcrylicTintOpacity      = controller.TintOpacity;
            AcrylicFallbackColor    = controller.FallbackColor;
            return controller;
        }
        public MicaController InitMicaController(MicaController controller = null)
        {
            controller ??= new MicaController();
            Kind                = controller.Kind;
            return controller;
        }
        [ObservableProperty]
        private Color acrylicTintColor;
        [ObservableProperty]
        private Color acrylicFallbackColor;
        [ObservableProperty]
        private float acrylicTintOpacity;
        [ObservableProperty]
        private MicaKind kind;
        [ObservableProperty]
        private WindowBackdrop windowBackdrop;
        [ObservableProperty]
        private bool isTabDragTo;
        [ObservableProperty]
        private bool isTabDragOut;
        [ObservableProperty]
        private string appDataThumbsPath;
        partial void OnAppDataThumbsPathChanged(string oldValue, string newValue)
        {
            if(oldValue != newValue)
            {
                _localSettingsToolkit.Set(LocalSettingName.AppDataThumbsPath, newValue);
            }
        }
        partial void OnIsTabDragOutChanged(bool oldValue, bool newValue)
        {
            _localSettingsToolkit.Set(LocalSettingName.IsTabDragOut, newValue);
        }
        partial void OnIsTabDragToChanged(bool oldValue, bool newValue)
        {
            _localSettingsToolkit.Set(LocalSettingName.IsTabDragTo, newValue);
        }
        public void ChangeWindowBackdrop(WindowBackdrop oldMode, WindowBackdrop newMode)
        {
            _callerToolkit.ChangeWindowBackdrop(oldMode, newMode, AcrylicTintColor, AcrylicFallbackColor, AcrylicTintOpacity, Kind);
        }
        partial void OnAcrylicFallbackColorChanged(Color oldValue, Color newValue)
        {
            ChangeWindowBackdrop(WindowBackdrop, WindowBackdrop);
        }
        partial void OnAcrylicTintColorChanged(Color oldValue, Color newValue)
        {
            ChangeWindowBackdrop(WindowBackdrop, WindowBackdrop);
        }
        partial void OnAcrylicTintOpacityChanged(float oldValue, float newValue)
        {
            ChangeWindowBackdrop(WindowBackdrop, WindowBackdrop);
        }
        partial void OnKindChanged(MicaKind oldValue, MicaKind newValue)
        {
            ChangeWindowBackdrop(WindowBackdrop, WindowBackdrop);
        }
        partial void OnWindowBackdropChanged(WindowBackdrop oldValue, WindowBackdrop newValue)
        {
            ChangeWindowBackdrop(oldValue, newValue);
        }
    }
}
