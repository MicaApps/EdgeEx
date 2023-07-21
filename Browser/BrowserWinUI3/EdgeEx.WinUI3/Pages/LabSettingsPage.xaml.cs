using CommunityToolkit.WinUI.Helpers;
using EdgeEx.WinUI3.Args;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Extensions;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using System; 
using Windows.Foundation;
using Windows.Globalization.NumberFormatting;
using Windows.UI;
using WinUIEx;

namespace EdgeEx.WinUI3.Pages
{
    /// <summary>
    /// Lab Settings Page
    /// </summary>
    public sealed partial class LabSettingsPage : Page
    {
        private SettingsViewModel ViewModel { get; }
        private SystemBackdropsHelper BackdropsHelper { get; set; }
        private ICallerToolkit caller;
        private string PersistenceId { get; set; }
        private string TabItemName { get; set; }
        private Uri NavigateUri { get; set; }
        public LabSettingsPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService<SettingsViewModel>();
        }
        private void InitPersistenceId()
        {
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
        }
        private void WindowBackdropComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceToolkit resourceToolkit = App.Current.Services.GetService<ResourceToolkit>();   
            BackdropsHelper = WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            ViewModel.InitDesktopAcrylicController(BackdropsHelper.WindowAcrylicController);
            ViewModel.InitMicaController(BackdropsHelper.WindowMicaController);
            ViewModel.WindowBackdrop = BackdropsHelper.CurrentBackdrop;
            WindowBackdropComboBox.SelectedIndex = BackdropsHelper.CurrentBackdrop == WindowBackdrop.Acrylic ? 0 : 1;
            InitPersistenceId();
            caller.FrameStatus(this, PersistenceId, Frame.CanGoBack, Frame.CanGoForward, false);
            caller.SendUriNavigatedMessage(this, PersistenceId, TabItemName,
                    NavigateUri, $"\"{resourceToolkit.GetString(ResourceKey.Lab)}\"",  new FontIconSource() { Glyph = "\uF158" });
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigatePageArg args = e.Parameter as NavigatePageArg;
            TabItemName = args.TabItemName;
            NavigateUri = new Uri("EdgeEx://Settings/Lab");
            caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.WindowBackdropChangedEvent += Caller_WindowBackdropChangedEvent;
            caller.SizeChangedEvent += Caller_SizeChangedEvent;
            caller.FrameOperationEvent += Caller_FrameOperationEvent;
        }

        private void Caller_FrameOperationEvent(object sender, FrameOperationEventArg e)
        {
            if(TabItemName == e.TabItemName)
            {
                switch (e.Operation)
                {
                    case FrameOperation.Refresh:
                         
                        break;
                    case FrameOperation.GoBack:
                        if (Frame.CanGoBack)
                            Frame.GoBack();
                        break;
                    case FrameOperation.GoForward:
                        if (Frame.CanGoForward)
                            Frame.GoForward();
                        break;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            caller.WindowBackdropChangedEvent -= Caller_WindowBackdropChangedEvent;
            caller.SizeChangedEvent -= Caller_SizeChangedEvent;
            caller.FrameOperationEvent -= Caller_FrameOperationEvent;
        }
        /// <summary>
        /// Control tab page size(From Event)
        /// </summary>
        private void Caller_SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            Top.Height = e.NewSize.Height;
            Top.Width = e.NewSize.Width;
        }
        /// <summary>
        /// Respond to WindowBackdrop changes(From Event)
        /// </summary>
        private void Caller_WindowBackdropChangedEvent(object sender, Args.WindowBackdropChangedEventArg e)
        {
            BackdropsHelper ??= WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            // Change Color / Opacity
            if (e.NewMode == e.OldMode)
            {
                if (e.NewMode == WindowBackdrop.Acrylic)
                {
                    if (BackdropsHelper.WindowAcrylicController == null) return;
                    if (BackdropsHelper.WindowAcrylicController.TintOpacity != e.TintOpacity)
                    {
                        BackdropsHelper.WindowAcrylicController.TintOpacity = e.TintOpacity;
                        BackdropsHelper.WindowAcrylicController.FallbackColor = Color.FromArgb(1, e.FallbackColor.R, e.FallbackColor.G, e.FallbackColor.B);
                        BackdropsHelper.WindowAcrylicController.FallbackColor = Color.FromArgb(2, e.FallbackColor.R, e.FallbackColor.G, e.FallbackColor.B);
                    }
                    BackdropsHelper.WindowAcrylicController.TintColor = e.TintColor;
                    BackdropsHelper.WindowAcrylicController.FallbackColor = e.FallbackColor;
                    Log.Debug("Set Acrylic WindowBackdrop:TintColor={TintColor},TintOpacity={TintOpacity},FallbackColor={FallbackColor}",
                                    BackdropsHelper.WindowAcrylicController.TintColor,
                                    BackdropsHelper.WindowAcrylicController.TintOpacity,
                                    BackdropsHelper.WindowAcrylicController.FallbackColor);
                }
                else
                {
                    if (BackdropsHelper.WindowMicaController == null) return;
                    BackdropsHelper.WindowMicaController.Kind = e.Kind;
                    Log.Debug("Set Mica WindowBackdrop:Kind={Kind}", BackdropsHelper.WindowMicaController.Kind);
                }
            }
            // Change WindowBackdrop
            else
            {
                if (e.NewMode == WindowBackdrop.Acrylic)
                {
                    BackdropsHelper.SetBackdrop(WindowBackdrop.Acrylic, new DesktopAcrylicController
                    {
                        TintColor = ViewModel.AcrylicTintColor,
                        FallbackColor = ViewModel.AcrylicFallbackColor,
                        TintOpacity = ViewModel.AcrylicTintOpacity,
                    });

                }
                else
                {
                    BackdropsHelper.SetBackdrop(WindowBackdrop.Mica, new MicaController
                    {
                        Kind = ViewModel.Kind,
                    });
                }
            }
            SetToLocalSettings();
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
            // Initialize Tab size
            Rect rect = WindowHelper.GetWindowForElement(this).Bounds;
            int titleBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExTitleBarHeight"]);
            int commandBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExCommandBarHeight"]);
            Top.Height = rect.Height - titleBarHeight - commandBarHeight;
            Top.Width = rect.Width;
        }
        /// <summary>
        /// Reset WindowBackdrop
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.WindowBackdrop == WindowBackdrop.Acrylic)
            {
                BackdropsHelper.SetBackdrop(WindowBackdrop.Acrylic, ViewModel.InitDesktopAcrylicController());
            }
            else
            {
                BackdropsHelper.SetBackdrop(WindowBackdrop.Mica, ViewModel.InitMicaController());
            }
            SetToLocalSettings();
        }
        /// <summary>
        /// WindowBackdrop Changed with Selection Changed
        /// </summary>
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
        /// <summary>
        /// Save WindowBackdrop Changes
        /// </summary>
        private void SetToLocalSettings()
        {
            LocalSettingsToolkit localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
            if (BackdropsHelper.CurrentBackdrop == WindowBackdrop.Acrylic)
            {
                if (ThemeHelper.IsDarkTheme)
                {
                    localSettingsToolkit.Set(LocalSettingName.DarkAcrylicTintColor, ViewModel.AcrylicTintColor.ToHex());
                    localSettingsToolkit.Set(LocalSettingName.DarkAcrylicFallbackColor, ViewModel.AcrylicFallbackColor.ToHex());
                    localSettingsToolkit.Set(LocalSettingName.DarkAcrylicTintOpacity, ViewModel.AcrylicTintOpacity);
                }
                else
                {
                    localSettingsToolkit.Set(LocalSettingName.LightAcrylicTintColor, ViewModel.AcrylicTintColor.ToHex());
                    localSettingsToolkit.Set(LocalSettingName.LightAcrylicFallbackColor, ViewModel.AcrylicFallbackColor.ToHex());
                    localSettingsToolkit.Set(LocalSettingName.LightAcrylicTintOpacity, ViewModel.AcrylicTintOpacity);
                }
            }
            else
            {
                localSettingsToolkit.Set(LocalSettingName.MicaKind, ViewModel.Kind.ToString());
            }
        }
        
    }
}
