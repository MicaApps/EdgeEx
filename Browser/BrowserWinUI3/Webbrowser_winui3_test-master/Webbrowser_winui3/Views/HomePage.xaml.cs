using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Webbrowser_winui3.ViewModels;
using Windows.Devices.Enumeration;

namespace Webbrowser_winui3.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class HomePage : Page
{
    ListDetailsViewModel listDetailsViewModel = new ListDetailsViewModel();
    public HomePage()
    {
        InitializeComponent();
        this.Loaded += (s, e) =>
        {
            HomePageViewModel.InitCommand.Execute(gv);
        };
    }
    private void GobnClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(tb_url.Text);
    }


    private void Grid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        var properties = e.GetCurrentPoint(root).Properties;
        if (properties.IsLeftButtonPressed)
        {
            HomePageViewModel.gv_ItemClickCommand.Execute(sender as Grid);
        }
    }

    private void TextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            MainViewModel.OpenWebPageCommand.Execute(tb_homeurl.Text);
        }
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(tb_homeurl.Text);
    }

    private void Grid_Holding(object sender, Microsoft.UI.Xaml.Input.HoldingRoutedEventArgs e)
    {
        HomePageViewModel.RightMenuCommand.Execute(sender as Grid);
    }

    private void Grid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        HomePageViewModel.RightMenuCommand.Execute(sender as Grid);
    }

    private void TextBox_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }

    private void TextBox_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }

    private void TextBox_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }
    private void TextBox_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        textBox.Background = textBox.Background;
    }
}
