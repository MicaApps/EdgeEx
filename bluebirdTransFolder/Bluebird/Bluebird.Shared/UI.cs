using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Shared
{
    public class UI
    {
        // Show dialog
        public static async Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Okay",
                DefaultButton = ContentDialogButton.Primary
            };

            await dialog.ShowAsync();
        }
    }
}
