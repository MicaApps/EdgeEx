using Microsoft.UI.Xaml.Controls;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.ViewModels;

namespace Webbrowser_winui3.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    ListDetailsViewModel listDetailsViewModel = new ListDetailsViewModel();
    SettingViewModel settingViewModel=new SettingViewModel();
    public SettingsPage()
    {
        InitializeComponent();
        settingViewModel.SettingInitCommand.Execute(new object[] { cbb_engine});
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        settingViewModel.ComboBox_SelectionChangedCommand.Execute(sender);
    }
    private void GobnClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(tb_url.Text);
    }


    private void SeachEngineSeleChanged(object sender, SelectionChangedEventArgs e)
    {
        settingViewModel.ComboBox_SelectionEngineChangedCommand.Execute(sender);
    }

    private void bnClearHistoryClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ListDetailsViewModel.ClearHistoryCommand.Execute(null);
    }

    private void CheckBox_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        settingViewModel.CheckBox_ClickCommand.Execute(sender);
    }
    private void CheckBox1_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        settingViewModel.CheckBox1_ClickCommand.Execute(sender);
    }
}
