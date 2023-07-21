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
    public sealed partial class AppAuthor : UserControl
    {
        public static readonly DependencyProperty IsCircleProperty =
           DependencyProperty.Register(nameof(IsCircle),
               typeof(bool),
               typeof(AppAuthor),
               new PropertyMetadata(default, new PropertyChangedCallback(OnIsCircleChanged)));
        public bool IsCircle
        {
            get { return (bool)GetValue(IsCircleProperty); }
            set { SetValue(IsCircleProperty, value); }
        }
        private static void OnIsCircleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AppAuthor control = (AppAuthor)d;
            control.IsCircle = (bool)e.NewValue;
        }
        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register(nameof(Title),
               typeof(string),
               typeof(AppAuthor),
               new PropertyMetadata(default, new PropertyChangedCallback(OnTitleChanged)));
        private Visibility Not(bool f)
        {
            return f ? Visibility.Collapsed : Visibility.Visible;
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AppAuthor control = (AppAuthor)d;
            control.Title = (string)e.NewValue;
        }
        public static readonly DependencyProperty DescriptionProperty =
           DependencyProperty.Register(nameof(Description),
               typeof(string),
               typeof(AppAuthor),
               new PropertyMetadata(default, new PropertyChangedCallback(OnDescriptionChanged)));
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AppAuthor control = (AppAuthor)d;
            control.Description = (string)e.NewValue;
        }
        public static readonly DependencyProperty ThumbProperty =
           DependencyProperty.Register(nameof(Thumb),
               typeof(string),
               typeof(AppAuthor),
               new PropertyMetadata(default, new PropertyChangedCallback(OnThumbChanged)));
        public string Thumb
        {
            get { return (string)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }
        private static void OnThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AppAuthor control = (AppAuthor)d;
            control.Thumb = (string)e.NewValue;
        }
        public static readonly DependencyProperty NavigateUriProperty =
           DependencyProperty.Register(nameof(NavigateUri),
               typeof(string),
               typeof(AppAuthor),
               new PropertyMetadata(default, new PropertyChangedCallback(OnNavigateUriChanged)));
        public string NavigateUri
        {
            get { return (string)GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }
        private static void OnNavigateUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AppAuthor control = (AppAuthor)d;
            control.NavigateUri = (string)e.NewValue;
        }
        public AppAuthor()
        {
            this.InitializeComponent();
        }
    }
}
