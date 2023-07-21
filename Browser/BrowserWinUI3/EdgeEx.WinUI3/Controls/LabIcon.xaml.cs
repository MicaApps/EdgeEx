using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3.Controls
{
    public sealed partial class LabIcon : UserControl
    {
        public LabIcon()
        {
            this.InitializeComponent();
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        { 
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Border s = sender as Border;
            s.Background = new SolidColorBrush("#FF363636".ToColor());
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Border s = sender as Border;
            s.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
