using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.ViewModels;

namespace Webbrowser_winui3.Views;

public sealed partial class ListDetailsPage : Page
{

    ListViewModel listViewModel = new ListViewModel();
    public ListDetailsPage()
    {
        InitializeComponent();
        ListViewModel.Set_ListViewControlSource_Command.Execute(new object[] { ListDetailsViewControl ,tb_url});
    }

    private void ListDetailsViewControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListViewModel.ListDetailsViewControl_SelectionChanged.Execute(e.AddedItems);
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ListViewModel.TextBox_TextChanged_Command.Execute(sender as TextBox);
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ListViewModel.OnNavigatedTo_Command.Execute(e.Parameter);
        base.OnNavigatedTo(e);
    }

    private void GobnClick(object sender, RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(tb_url.Text);
    }
}
