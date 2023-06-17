using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowserDialogs.DialogTypes.AreYouSureDialog
{
    public enum DialogState
    {
        Favorites,
        History,
        Apps
    }
    public sealed partial class SureDialog : ContentDialog
    {
        public DialogState State { get; set; }
        public string Message { get; set; }
        public SureDialog()
        {
            this.InitializeComponent();
            this.Loaded += SureDialog_Loaded;
        }

        private void SureDialog_Loaded(object sender, RoutedEventArgs e)
        {
            switch (this.State)
            {
                case DialogState.Favorites:
                    this.Title = "Delete All Favorites";

                    break;
                case DialogState.History:
                    this.Title = "Delete All History";

                    break;
                case DialogState.Apps:
                    this.Title = "Delete Current App";

                    break;
            }
        }
    }
}
