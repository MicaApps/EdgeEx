using CommunityToolkit.Labs.WinUI;
using CommunityToolkit.WinUI.Helpers;
using EdgeEx.WinUI3.Args;
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
using Microsoft.UI.Xaml.Media.Animation;
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
using WinRT;
using WinUIEx;

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
        private SystemBackdropsHelper BackdropsHelper { get; set; }
        private string PersistenceId { get; set; }
        private string TabItemName { get; set; }
        private Uri NavigateUri { get; set; }
        private ICallerToolkit caller;
        public SettingsPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService<SettingsViewModel>();
            Version.Text = string.Format("v{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
        }
        private void InitPersistenceId()
        {
            WindowEx window = WindowHelper.GetWindowForElement(this);
            PersistenceId = window.PersistenceId;
        }

        /// <summary>
        /// Control tab page size(From Event)
        /// </summary>
        private void Caller_SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            Top.Height = e.NewSize.Height;
            Top.Width = e.NewSize.Width;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigatePageArg args = e.Parameter as NavigatePageArg;
            TabItemName = args.TabItemName;
            NavigateUri = args.NavigateUri;
            caller = App.Current.Services.GetService<ICallerToolkit>();
            caller.SizeChangedEvent += Caller_SizeChangedEvent;
            caller.WindowBackdropChangedEvent += Caller_WindowBackdropChangedEvent;
            caller.FrameOperationEvent += Caller_FrameOperationEvent; 
        }

        private void Caller_FrameOperationEvent(object sender, FrameOperationEventArg e)
        {
            if (TabItemName == e.TabItemName)
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
            caller.SizeChangedEvent -= Caller_SizeChangedEvent;
            caller.WindowBackdropChangedEvent -= Caller_WindowBackdropChangedEvent;
        }
        private void Caller_WindowBackdropChangedEvent(object sender, Args.WindowBackdropChangedEventArg e)
        {
            BackdropsHelper ??= WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            // Change Color / Opacity
            if (e.NewMode != e.OldMode)
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
        }
        private void Top_Loaded(object sender, RoutedEventArgs e)
        {
            InitPersistenceId();
            // Initialize Tab size
            Rect rect = WindowHelper.GetWindowForElement(this).Bounds;
            int titleBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExTitleBarHeight"]);
            int commandBarHeight = Convert.ToInt32(Application.Current.Resources["EdgeExCommandBarHeight"]);
            Top.Height = rect.Height - titleBarHeight - commandBarHeight;
            Top.Width = rect.Width;
            caller.FrameStatus(this, PersistenceId, Frame.CanGoBack, Frame.CanGoForward, false);
            ResourceToolkit resourceToolkit = App.Current.Services.GetService<ResourceToolkit>();
            caller.SendUriNavigatedMessage(this, PersistenceId, TabItemName,
                        NavigateUri, $"\"{resourceToolkit.GetString(ResourceKey.Settings)}\"", new FontIconSource() { Glyph = "\uE713" });

        }
        /// <summary>
        /// Open Logs Folder
        /// </summary>
        private async void LogButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        }
        /// <summary>
        /// Authors Width Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsExpander_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AuthorScrollViewer.Width = (sender as SettingsExpander).ActualWidth - 115;
        }
        /// <summary>
        /// Launch Uri in Browser
        /// </summary>
        private async void Uri_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri((sender as SettingsCard).Tag.ToString());
            await Launcher.LaunchUriAsync(uri);
        }
        /// <summary>
        /// Go To Lab Settings Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabSettingCard_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LabSettingsPage),new NavigatePageArg(TabItemName,null),new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ElementTheme theme = EnumHelper.GetEnum<ElementTheme>(((sender as ComboBox).SelectedItem as FrameworkElement).Tag.ToString());
            if(ThemeHelper.RootTheme != theme)
            {
                ThemeHelper.RootTheme = theme;
                Log.Debug("Change Theme To {theme}", theme.ToString());
                BackdropsHelper ??= WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
                BackdropsHelper.Reload();
                ViewModel.InitDesktopAcrylicController(BackdropsHelper.WindowAcrylicController);
                ViewModel.InitMicaController(BackdropsHelper.WindowMicaController);
            }
        }

        private void BackdropComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WindowBackdrop backdrop = EnumHelper.GetEnum<WindowBackdrop>(((sender as ComboBox).SelectedItem as FrameworkElement).Tag.ToString());
            ViewModel.WindowBackdrop = backdrop;
            Log.Debug("Change WindowBackdrop To {backdrop}", backdrop.ToString());
        }

        private void BackdropComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            BackdropsHelper = WindowHelper.GetWindowForXamlRoot(XamlRoot).GetSystemBackdropsHelper();
            ViewModel.InitDesktopAcrylicController(BackdropsHelper.WindowAcrylicController);
            ViewModel.InitMicaController(BackdropsHelper.WindowMicaController);
            ViewModel.WindowBackdrop = BackdropsHelper.CurrentBackdrop;
            (sender as ComboBox).SelectedIndex = BackdropsHelper.CurrentBackdrop == WindowBackdrop.Acrylic ? 0 : 1;
        }

        private void ThemeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).SelectedIndex = ThemeHelper.IsDarkTheme ? 1 : 0;
        }
    }
}
