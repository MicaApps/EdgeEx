using Microsoft.UI.Xaml.Controls;
using System;

namespace Bluebird.Core;

public static class IconHelper
{
    public static IconSource ConvFavURLToIconSource(string url)
    {
        Uri faviconUrl = new(url);
        BitmapIconSource iconsource = new() { UriSource = faviconUrl, ShowAsMonochrome = false };
        return iconsource;
    }
}