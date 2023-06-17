using Bluebird.Core;
using System;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages.SettingPages;

public sealed partial class Data : Page
{
    public Data()
    {
        this.InitializeComponent();
    }

    private async void ClearBrowserData_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        ContentDialog contentDialog = new()
        {
            Title = "Clear data",
            Content = "Do you want to clear all your browsing data including: Favorites and History",
            PrimaryButtonText = "Clear",
            DefaultButton = ContentDialogButton.Primary,
            SecondaryButtonText = "Cancel"
        };

        var result = await contentDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            // Clear favorties
            await FileHelper.DeleteLocalFile("Favorites.json");
        }
    }
}
