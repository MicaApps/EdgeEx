using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Helpers;
using Windows.ApplicationModel;
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
        public SettingsPage()
        {
            this.InitializeComponent();
            ViewModel = App.Current.Services.GetService<SettingsViewModel>();
            Version.Text = string.Format("v{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
        }

        private void WindowBackdropComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WindowHelper.SetWindowBackdrop(WindowHelper.GetWindowForXamlRoot(XamlRoot), ViewModel.SelectedBackdrop);
        }

        private void WindowBackdropComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            WindowBackdropComboBox.SelectedIndex = WindowHelper.IsMica() ? 0 : 1;
        }
        private async void LogButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        }
    }
}
