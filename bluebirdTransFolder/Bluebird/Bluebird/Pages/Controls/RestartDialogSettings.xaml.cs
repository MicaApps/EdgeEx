using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bluebird.Shared;
using Windows.Storage;
using Bluebird.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Bluebird.Pages.Controls
{
    public sealed partial class RestartDialogSettings : UserControl
    {
        public RestartDialogSettings()
        {
            this.InitializeComponent();
        }

        private async void RestartBtn_Click(object sender, RoutedEventArgs e)
        {
            Bluebird.Shared.SystemHelper.RestartApp();
            SettingsHelper.SetSetting("UpdatedSettings", "false");
        }
    }
}
