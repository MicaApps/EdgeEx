
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool canGoBack;
        [ObservableProperty]
        private bool canGoForward;
        public MainViewModel() { }
    }
}
