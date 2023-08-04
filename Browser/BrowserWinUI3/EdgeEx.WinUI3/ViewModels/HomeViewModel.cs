using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Models;
using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.ViewModels
{
    public class HomeViewModel:ObservableObject
    {
        private ISqlSugarClient db;
        public ObservableCollection<Bookmark> Bookmarks = new ObservableCollection<Bookmark>();
        public HomeViewModel(ISqlSugarClient _sqlSugarClient)
        {
            db = _sqlSugarClient;
             
        }
        public void Init()
        {
            var parent = "1613484749";
            Bookmarks.Clear();
            foreach (var book in db.Queryable<Bookmark>().Where(x => !x.IsFolder && x.FolderId == parent).ToList())
            {
                if (book.Icon == null) continue;
                Bookmarks.Add(book);
                //Log.Information(book.Icon);
            }
        }
    }
}
