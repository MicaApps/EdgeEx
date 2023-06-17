using CommunityToolkit.Mvvm.ComponentModel;
using FireBrowser.Core;
using System;
using System.Threading;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using static FireBrowser.MainPage;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.IO;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.AccessCache;
using System.Data.SqlTypes;
using Windows.Graphics.Printing;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PdfReader : Page
    {
        Passer param;

        public class PageListVisibility : ObservableObject
        {
            private Visibility _state;

            public PageListVisibility()
            {
                this.PageVisibility = Visibility.Collapsed;
            }

            public Visibility PageVisibility
            {
                get { return _state; }
                set { SetProperty(ref _state, value); }
            }
        }

        public static string CurrentPage
        {
            get; set;
        }
        public PdfReader()
        {
            this.InitializeComponent();
         
            ViewModel = new PageListVisibility();
        }
        public PageListVisibility ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            param = e.Parameter as Passer;

            param.Tab.Header = "FireBrowser - PdfReader";

            var parameter = param?.Param;

            if (parameter is IStorageItem args)
            {
                
            }
            else if (parameter is Uri)
            {
                
            }
            else
            {
              
            }
        }

        #region classes
        public async void open()
        {
            //Opens a file picker.
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.ViewMode = PickerViewMode.List;
            //Filters PDF files in the documents library.
            picker.FileTypeFilter.Add(".pdf");
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            //Reads the stream of the loaded PDF document.
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Stream fileStream = stream.AsStreamForRead();
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            //Loads the PDF document.
         
          
        }

        public async void PagesToggle()
        {
            if(ViewModel.PageVisibility == Visibility.Visible)
            {
                ViewModel.PageVisibility = Visibility.Collapsed;
            }
            else
            {
                ViewModel.PageVisibility = Visibility.Visible;
            }
        }

        public async void getFile()
        {
           
        }
       

        #endregion

     

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private async void ToolbarButtonClick(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag)
            {
                case "Open":
                    open();
                    break;
                case "Share":
                    getFile();
                    break;
                case "Print":
                   
                    break;
            }
        }



        private void PageNums_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)(sender as ToggleButton).IsChecked) { ViewModel.PageVisibility = Visibility.Visible; }
            else { ViewModel.PageVisibility = Visibility.Collapsed; }
        }

        private void PageNums_Unchecked(object sender, RoutedEventArgs e)
        {
          if(PageNums.IsChecked == false)
            {
                ViewModel.PageVisibility = Visibility.Collapsed;
            }
        }

        private async void Feedback_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://discord.gg/kYStRKBHwy"));
        }

     
    }
}
