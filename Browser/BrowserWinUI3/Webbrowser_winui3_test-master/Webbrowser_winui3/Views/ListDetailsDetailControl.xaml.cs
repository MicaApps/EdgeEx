using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Webbrowser_winui3.Models;
using Webbrowser_winui3.ViewModels;

namespace Webbrowser_winui3.Views;

public sealed partial class ListDetailsDetailControl : UserControl
{
    public WebModel? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as WebModel;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(WebModel), typeof(ListDetailsDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));


    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }
    ListDetailsViewModel listDetailsViewModel = new ListDetailsViewModel();
    public ListDetailsDetailControl()
    {
        InitializeComponent();
        ListDetailsViewModel.Set_lv_historydetailSource_Command.Execute(lv_historydetail);
    }

    private void ButtonOpen_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.OpenWebPageCommand.Execute(((Button)sender).Tag.ToString());
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        ListDetailsViewModel.DeleteCommand.Execute(((Button)sender).Tag.ToString());
    } 
    private void ButtonCopyUrl_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.CopyUrlCommand.Execute(((Button)sender).Tag.ToString());
    }
    public void ButtonAddToHome_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.ButtonAddToHome_ClickCommand.Execute(((Button)sender).Tag.ToString());
    }
}
