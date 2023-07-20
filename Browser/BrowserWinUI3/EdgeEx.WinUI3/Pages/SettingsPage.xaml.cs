using CommunityToolkit.Labs.WinUI;
using CommunityToolkit.WinUI.Helpers;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Extensions;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Globalization.NumberFormatting;
using Windows.Storage;
using Windows.System;
using Windows.UI;

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
        private void Caller_SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            Top.Height = e.NewSize.Height;
            Top.Width = e.NewSize.Width;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ICallerToolkit caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.WindowBackdropChangedEvent -= Caller_WindowBackdropChangedEvent;
            caller.SizeChangedEvent -= Caller_SizeChangedEvent;
        }
        private void Caller_WindowBackdropChangedEvent(object sender, Args.WindowBackdropChangedEventArg e)
        {
            helper ??= WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            // Change Color / Opacity
            if (e.NewMode == e.OldMode)
            {
                if (e.NewMode == WindowBackdrop.Acrylic)
                {
                    if (helper.WindowAcrylicController == null) return;
                    if (helper.WindowAcrylicController.TintOpacity != e.TintOpacity)
                    {
                        helper.WindowAcrylicController.TintOpacity = e.TintOpacity;
                        helper.WindowAcrylicController.FallbackColor = Color.FromArgb(1, e.FallbackColor.R, e.FallbackColor.G, e.FallbackColor.B);
                        helper.WindowAcrylicController.FallbackColor = Color.FromArgb(2, e.FallbackColor.R, e.FallbackColor.G, e.FallbackColor.B);
                    }
                    helper.WindowAcrylicController.TintColor = e.TintColor;
                    helper.WindowAcrylicController.FallbackColor = e.FallbackColor;
                    Log.Information("Set Acrylic WindowBackdrop:TintColor={TintColor},TintOpacity={TintOpacity},FallbackColor={FallbackColor}",
                                    helper.WindowAcrylicController.TintColor,
                                    helper.WindowAcrylicController.TintOpacity,
                                    helper.WindowAcrylicController.FallbackColor);
                }
                else
                {
                    if (helper.WindowMicaController == null) return;
                    helper.WindowMicaController.Kind = e.Kind;
                    Log.Information("Set Mica WindowBackdrop:Kind={Kind}", helper.WindowMicaController.Kind);
                }
            }
            // Change WindowBackdrop
            else
            {
                if (e.NewMode == WindowBackdrop.Acrylic)
                {
                    helper.SetBackdrop(WindowBackdrop.Acrylic, new DesktopAcrylicController
                    {
                        TintColor = ViewModel.AcrylicTintColor,
                        FallbackColor = ViewModel.AcrylicFallbackColor,
                        TintOpacity = ViewModel.AcrylicTintOpacity,
                    });

                }
                else
                {
                    helper.SetBackdrop(WindowBackdrop.Mica, new MicaController
                    {
                        Kind = ViewModel.Kind,
                    });
                }
            }
            SetToLocalSettings();
        }

        private void WindowBackdropComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WindowBackdrop backdrop = EnumHelper.GetEnum<WindowBackdrop>((WindowBackdropComboBox.SelectedItem as FrameworkElement).Tag.ToString());
            ViewModel.WindowBackdrop = backdrop;
            bool isAcrylic = backdrop == WindowBackdrop.Acrylic;
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
            ViewModel.InitDesktopAcrylicController(helper.WindowAcrylicController);
            ViewModel.InitMicaController(helper.WindowMicaController);
            ViewModel.WindowBackdrop = helper.CurrentBackdrop;
            WindowBackdropComboBox.SelectedIndex = helper.CurrentBackdrop== WindowBackdrop.Acrylic ? 0 : 1;
            ICallerToolkit caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.WindowBackdropChangedEvent += Caller_WindowBackdropChangedEvent;
            caller.SizeChangedEvent += Caller_SizeChangedEvent;
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
            Rect rect = WindowHelper.GetWindowForElement(this).Bounds;
            Top.Height = rect.Height - 48 - 48;
            Top.Width = rect.Width;
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.WindowBackdrop == WindowBackdrop.Acrylic)
            { 
                helper.SetBackdrop(WindowBackdrop.Acrylic, ViewModel.InitDesktopAcrylicController());
            }
            else
            {
                helper.SetBackdrop(WindowBackdrop.Mica, ViewModel.InitMicaController());
            }
            SetToLocalSettings();
        }
    }
}
