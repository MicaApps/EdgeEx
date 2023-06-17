namespace Bluebird.Core;

public class FavoritesHelper
{
    public static void AddFavoritesItem(string title, string url)
    {
        Json.AddItemToJson("Favorites.json", title, url);
    }
}
