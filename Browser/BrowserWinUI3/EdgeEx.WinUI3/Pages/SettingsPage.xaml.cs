using CommunityToolkit.Labs.WinUI;
using CommunityToolkit.WinUI.Helpers;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Extensions;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.ApplicationModel;
using Windows.Globalization.NumberFormatting;
using Windows.Storage;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel { get; }
        private SystemBackdropsHelper helper;
        public SettingsPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService<SettingsViewModel>();
            Version.Text = string.Format("v{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
        }

        private void WindowBackdropComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(helper == null)
                helper = WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            ReloadBackdrop();
            bool isAcrylic = helper.CurrentBackdrop == WindowBackdrop.Acrylic;
            TintColorCard.Visibility = isAcrylic.ToVisibility();
            TintOpacityCard.Visibility = isAcrylic.ToVisibility();
            FallbackColorCard.Visibility = isAcrylic.ToVisibility();
            MicaKindCard.Visibility = (!isAcrylic).ToVisibility();
        }
        private void SetToLocalSettings()
        {
            LocalSettingsToolkit localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
            if (helper.CurrentBackdrop == WindowBackdrop.Acrylic)
            {
                localSettingsToolkit.Set(LocalSettingName.AcrylicTintColor, ViewModel.AcrylicTintColor.ToHex());
                localSettingsToolkit.Set(LocalSettingName.AcrylicFallbackColor, ViewModel.AcrylicFallbackColor.ToHex());
                localSettingsToolkit.Set(LocalSettingName.AcrylicTintOpacity, ViewModel.AcrylicTintOpacity);
            }
            else
            {
                localSettingsToolkit.Set(LocalSettingName.MicaKind,ViewModel.Kind.ToString());
            }
        }
        private void WindowBackdropComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            helper = WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            ViewModel.InitMicaController(helper.WindowMicaController ?? new MicaController());
            ViewModel.InitDesktopAcrylicController(helper.WindowAcrylicController ?? new DesktopAcrylicController());
            WindowBackdropComboBox.SelectedIndex = helper.CurrentBackdrop==WindowBackdrop.Acrylic ? 0 : 1;
        }
        private async void LogButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        }

        private void SettingsExpander_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AuthorScrollViewer.Width = (sender as SettingsExpander).ActualWidth - 115;
        }
        private async void Uri_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri((sender as SettingsCard).Tag.ToString());
            await Launcher.LaunchUriAsync(uri);
        }
        private void FormattedNumberBox_Loaded(object sender, RoutedEventArgs e)
        {
            IncrementNumberRounder rounder = new IncrementNumberRounder();
            rounder.Increment = 0.01;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 2;
            formatter.NumberRounder = rounder;
            AcrylicFormattedNumberBox.NumberFormatter = formatter;
        }
        private void ReloadBackdrop(bool force = false)
        {
            WindowBackdrop backdrop = EnumHelper.GetEnum<WindowBackdrop>((WindowBackdropComboBox.SelectedItem as FrameworkElement).Tag.ToString());
            if (backdrop == WindowBackdrop.Acrylic)
            {
                helper.SetBackdrop(WindowBackdrop.Acrylic, new DesktopAcrylicController
                {
                    TintColor = ViewModel.AcrylicTintColor,
                    FallbackColor = ViewModel.AcrylicFallbackColor,
                    TintOpacity = ViewModel.AcrylicTintOpacity,
                }, force);
            }
            else
            {
                helper.SetBackdrop(WindowBackdrop.Mica, new MicaController
                {
                    Kind = ViewModel.Kind,
                }, force);
            }
        }
        private void RefreshBackdropButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadBackdrop(true);
            SetToLocalSettings();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            AcrylicBrush defaultAcrylicBrush = Application.Current.Resources["DefaultEdgeExAcrylicBrush"] as AcrylicBrush;
            WindowBackdrop backdrop = EnumHelper.GetEnum<WindowBackdrop>((WindowBackdropComboBox.SelectedItem as FrameworkElement).Tag.ToString());
            if (backdrop == WindowBackdrop.Acrylic)
            {
                helper.SetBackdrop(WindowBackdrop.Acrylic, new DesktopAcrylicController
                {
                    TintColor = defaultAcrylicBrush.TintColor,
                    TintOpacity = (float)defaultAcrylicBrush.TintOpacity,
                    FallbackColor = defaultAcrylicBrush.FallbackColor,
                }, true);
                ViewModel.AcrylicTintColor = defaultAcrylicBrush.TintColor;
                ViewModel.AcrylicTintOpacity = (float)defaultAcrylicBrush.TintOpacity;
                ViewModel.AcrylicFallbackColor = defaultAcrylicBrush.FallbackColor;
            }
            else
            {
                helper.SetBackdrop(WindowBackdrop.Mica, null,true);
                ViewModel.Kind = MicaKind.Base;
            }
            SetToLocalSettings();
        }
    }
}
