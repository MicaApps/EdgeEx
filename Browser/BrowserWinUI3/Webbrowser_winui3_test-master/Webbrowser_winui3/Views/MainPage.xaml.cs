using Microsoft.UI.Xaml.Controls;

using Webbrowser_winui3.ViewModels;

namespace Webbrowser_winui3.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        this.Loaded += (s, e) =>
        {
            MainViewModel.MainPageInitCommand.Execute(this);
        };
    }

    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        MainViewModel.TabCloseRequestedCommand.Execute(args.Tab);
    }

    private void tabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        MainViewModel.TabSelectChangeCommand.Execute(tabView.SelectedItem as TabViewItem);
    }

    private void tabView_AddTabButtonClick(TabView sender, object args)
    {
        MainViewModel.OpenHomeCommand.Execute(null);
    }

    private void Page_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        MainViewModel.Page_SizeChangedCommand.Execute(e.NewSize);
    }
}
