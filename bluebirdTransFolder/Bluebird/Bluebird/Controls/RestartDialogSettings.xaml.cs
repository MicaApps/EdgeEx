using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Bluebird.Controls;

public sealed partial class RestartDialogSettings : UserControl
{
    public RestartDialogSettings()
    {
        this.InitializeComponent();
    }

    private void RestartBtn_Click(object sender, RoutedEventArgs e)
    {
        Bluebird.Shared.SystemHelper.RestartApp();
    }
}