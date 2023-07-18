using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Toolkits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class SettingsViewModel: ObservableObject
    {
        private LocalSettingsToolkit _localSettingsToolkit;
        public SettingsViewModel(LocalSettingsToolkit localSettingsToolkit)
        {
            _localSettingsToolkit = localSettingsToolkit;
        }
        [ObservableProperty]
        private WindowBackdrop selectedBackdrop = WindowBackdrop.Acrylic;
    }
}
