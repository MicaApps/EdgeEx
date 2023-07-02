using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services;
using Webbrowser_winui3.Services.Interface;

namespace Webbrowser_winui3.ViewModels
{
    public sealed class FavListPageViewModel : ViewModelBase
    {
        public ObservableCollection<FavFileModel> FavFileModels { get; set; }
        public FavFileModel FavFileModelSelected { get; set; }
        private readonly IFavlistService favlistService;
        public FavListPageViewModel()
        {
            this.favlistService = new FavlistService();
            FavFileModels = new ObservableCollection<FavFileModel>();
        }
        public override async void OnStartup(object parameter)
        {
            var ls = await favlistService.GetFavFileModels();
            FavFileModels = new ObservableCollection<FavFileModel>(ls);
        }
        public IRelayCommand SelectedCommand => new Lazy<IRelayCommand>(new RelayCommand(() => 
        {
            var a = FavFileModelSelected;
        })).Value;
        public IRelayCommand<FavFileModel> NavigatedCommand => new Lazy<IRelayCommand<FavFileModel>>(new RelayCommand<FavFileModel>((obj) =>
        {
            if (string.IsNullOrEmpty(obj.Url) ==false)
            {
                MainViewModel.OpenWebPageCommand.Execute(obj.Url);
            }
        })).Value;
    }
}
