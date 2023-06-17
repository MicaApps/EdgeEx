using System;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Launch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setup : Page
    {
        MediaPlayer mediaPlayer;
        public Setup()
        {
            this.InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            var formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            formattableTitleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            formattableTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            formattableTitleBar.InactiveBackgroundColor = Colors.Transparent;
            formattableTitleBar.ButtonPressedBackgroundColor = Colors.Transparent;

            Window.Current.SetTitleBar(TitleBar);

            mediaPlayer = new MediaPlayer();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Launch/firebrowser.mp4"));
            _mediaPlayerElement.SetMediaPlayer(mediaPlayer);

            mediaPlayer.CommandManager.IsEnabled = false;
            mediaPlayer.Play();
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.MinWidth = (FlowDirection == FlowDirection.LeftToRight) ? sender.SystemOverlayRightInset : sender.SystemOverlayLeftInset;
            TitleBar.Height = sender.Height;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameNext.Visibility = Visibility.Visible;
            FrameNext.Navigate(typeof(SetupSettings));
            _mediaPlayerElement.Visibility = Visibility.Collapsed;
            txt2.Visibility = Visibility.Collapsed;
            txt.Visibility = Visibility.Collapsed;
            btn.Visibility = Visibility.Collapsed;
        }
    }
}
