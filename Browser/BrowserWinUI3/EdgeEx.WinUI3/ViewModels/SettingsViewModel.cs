using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.UI.Composition.SystemBackdrops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class SettingsViewModel: ObservableObject
    {
        private LocalSettingsToolkit _localSettingsToolkit;
        private ResourceToolkit _resourceToolkit;
        public SettingsViewModel(LocalSettingsToolkit localSettingsToolkit, ResourceToolkit resourceToolkit)
        {
            _localSettingsToolkit = localSettingsToolkit;
            _resourceToolkit = resourceToolkit;
        }
        public void InitDesktopAcrylicController(DesktopAcrylicController controller = null)
        {
            AcrylicTintColor        = controller.TintColor;
            AcrylicTintOpacity      = controller.TintOpacity;
            AcrylicFallbackColor    = controller.FallbackColor;
        }
        public void InitMicaController(MicaController controller = null)
        {
            Kind                = controller.Kind;
        }
        [ObservableProperty]
        private Color acrylicTintColor;
        [ObservableProperty]
        private Color acrylicFallbackColor;
        [ObservableProperty]
        private float acrylicTintOpacity;
        [ObservableProperty]
        private MicaKind kind;
    }
}
