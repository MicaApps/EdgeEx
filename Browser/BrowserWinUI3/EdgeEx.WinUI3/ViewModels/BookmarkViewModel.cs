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
        public ObservableCollection<Bookmark> BookmarkFolders { get; } = new ObservableCollection<Bookmark>();
        public ObservableCollection<Bookmark> CurrentBookmarks { get; } = new ObservableCollection<Bookmark>();
        public BookmarkViewModel(ISqlSugarClient sqlSugarClient)
        {
            db = sqlSugarClient;
            
        }
        public void InitBookmarks()
        {
            BookmarkFolders.Clear();
            object[] inIds = db.Queryable<Bookmark>()
                .Where(it => it.IsFolder)
                .Select(it => it.Uri).ToList().Cast<object>().ToArray();
            foreach (Bookmark bookmark in db.Queryable<Bookmark>().Where(it => it.IsFolder)
                .ToTree(it => it.Children, it => it.FolderId, "root", inIds))
            {
                BookmarkFolders.Add(bookmark);
            }
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
    }
}
