using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowserDialogs.DialogTypes.UpdateChangelog
{
    public sealed partial class UpdateChangelog : ContentDialog
    {
        public UpdateChangelog()
        {
            this.InitializeComponent();
            PackageVersion version = Package.Current.Id.Version;
            Ver.Text = "Update Version: " + $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}" + " - FireBrowser Alpha";
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://discord.gg/kYStRKBHwy");
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
