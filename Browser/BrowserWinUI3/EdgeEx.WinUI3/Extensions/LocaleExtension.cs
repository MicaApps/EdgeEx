using EdgeEx.WinUI3.Enums;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Markup;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Markup;

namespace EdgeEx.WinUI3.Extensions
{
    /// <summary>
    /// Localization
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal sealed class LocaleExtension : MarkupExtension
    {
        /// <summary>
        /// Key
        /// </summary>
        public ResourceKey Key { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return App.Current.Services.GetService<ResourceToolkit>().GetString(Key);
        }
    }
}