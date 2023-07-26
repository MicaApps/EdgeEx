using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class BookmarkViewModel
    {
        private ISqlSugarClient db;
        private ICallerToolkit caller;
        public ObservableCollection<Bookmark> BookmarkFolders { get; } = new ObservableCollection<Bookmark>();
        public ObservableCollection<Bookmark> CurrentBookmarks { get; } = new ObservableCollection<Bookmark>();
        public BookmarkViewModel(ISqlSugarClient sqlSugarClient,ICallerToolkit callerToolkit)
        {
            db = sqlSugarClient;
            caller = callerToolkit;
        }
        public Bookmark InitBookmarks(Uri navigateUri = null)
        {
            string[] urls = null;
            Bookmark selected = null;
            if (navigateUri!=null)
            {
                urls = navigateUri.ToString().Replace("edgeex://bookmarks/","").Split("/");
            }
            BookmarkFolders.Clear();
            object[] inIds = db.Queryable<Bookmark>()
                .Where(it => it.IsFolder)
                .Select(it => it.Uri).ToList().Cast<object>().ToArray();
            foreach (Bookmark bookmark in db.Queryable<Bookmark>().Where(it => it.IsFolder)
                .ToTree(it => it.Children, it => it.FolderId, "root", inIds))
            {
                BookmarkFolders.Add(bookmark);
                if (urls!=null&& bookmark.Uri == urls[0])
                {
                    selected = bookmark;
                }
                
            }
            if(selected != null)
            {
                int i = 1;
                for (;i < urls.Length;i++)
                {
                    if (selected == null) break;
                    selected = selected.Children.FirstOrDefault(x => x.Uri == urls[i]);
                }
                if(i == urls.Length)
                {
                   return selected;
                }
            }
            return null;
        }
        public void SetCurrentBookmarks(Bookmark bookmark)
        {
            if (bookmark != null)
            {
                CurrentBookmarks.Clear();
                foreach (Bookmark item in db.Queryable<Bookmark>().Where(x => !x.IsFolder && x.FolderId == bookmark.Uri).ToList())
                {
                    item.Icon ??= "ms-appx:///Assets/DefaultIcon.png";
                    item.Screenshot ??= "ms-appx:///Assets/DefaultScreenshot.png";
                    CurrentBookmarks.Add(item);
                }
            }
            
        }
        public void SetAddress(Bookmark bookmark, string persistenceId, string tabItemName)
        {  
            List<string> addresss = new List<string> { bookmark.Uri };
            while (bookmark!=null && bookmark?.FolderId != "root")
            {
                bookmark = db.Queryable<Bookmark>().First(x => x.Uri == bookmark.FolderId);
                addresss.Add(bookmark.Uri);
            }
            addresss.Reverse();
            caller.UriNavigationCompleted(this, persistenceId, tabItemName,
                   new Uri("EdgeEx://Bookmarks/" + String.Join('/', addresss)),
                   null, null);
        }
    }
}
