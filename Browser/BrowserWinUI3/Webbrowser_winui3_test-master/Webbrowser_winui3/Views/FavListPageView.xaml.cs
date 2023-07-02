using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Webbrowser_winui3.ViewModels;
using Webbrowser_winui3.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Webbrowser_winui3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FavListPageView : Page
    {
        private readonly ViewModels.FavListPageViewModel vm;
        public FavListPageView()
        {
            this.InitializeComponent();
            this.DataContext= vm = new FavListPageViewModel();
            this.Loaded += FavListPageView_Loaded;
        }
        private void FavListPageView_Loaded(object sender, RoutedEventArgs e)
        {
            vm.OnStartup(null);
        }
        /// <summary>
        /// TreeView.SelectedItem不支持绑定，用Tapped事件触发数据更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = sender as TreeViewItem;

            if (item != null)
            {
                var x = item.DataContext as FavFileModel;

                //如果没有子集，就不更新右侧list
                if (x.FavChildren.Any())
                {
                    vm.FavFileModelSelected= x;
                }
            }
        }
        private void TreeView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var tv = sender as TreeViewItem;
            if (tv != null)
            {
                tv.IsExpanded = !tv.IsExpanded;

            }
        }

        private void ListViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                vm.NavigatedCommand.Execute(item.DataContext);
            }
        }
    }
}
