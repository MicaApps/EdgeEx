using Microsoft.Toolkit.Uwp.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FireBrowser.Controls
{
    public sealed partial class FluentIcon : FontIconExtension
    {
        public FluentIcon()
        {
            FontFamily = (FontFamily)Application.Current.Resources["FluentIcons"];
        }
    }
}
