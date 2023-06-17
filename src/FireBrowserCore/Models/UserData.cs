using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace FireBrowserCore.Models
{
    public class UserData
    {
        public enum FocusMode
        {
            AllowAll,
            BlockAll
        }
        public enum FavoriteIconType
        {
            Favicon,
            Folder
        }
        public class UserDataBase : ObservableObject
        {
            public float AppVersion { get; set; }
            public List<Profile> Profiles { get; set; }
        }
        public class FavoriteIcon
        {
            public FavoriteIconType Type { get; set; }
        }

        public class Profile
        {
            public Favorite[] Favorites { get; set; }
            public Collection[] Collections { get; set; }
            public Installedapp[] InstalledApps { get; set; }
            public List<Site> NewTabPins { get; set; }
            public List<FocusList> Focus { get; set; }
        }

        public class Favorite
        {
            public string Name { get; set; }
            public string URL { get; set; }
            public List<Site> Content { get; set; }
        }

        public class FocusList
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public FocusMode Mode { get; set; }
        }

        public class Collection
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public List<Content> Content { get; set; }
        }

        public class Content
        {
            public string Type { get; set; }
            public object ItemContent { get; set; }
        }

        public class CollectionNote
        {
            public string Text { get; set; }
            public string Color { get; set; }
        }

        public class Tabsaside
        {
            public string Name { get; set; }
            public List<Site> Content { get; set; }
        }

        public class Site
        {
            public string Name { get; set; }
        }

        public class Installedapp
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public string URL { get; set; }
            public bool Pinned { get; set; }
        }
    }
}
