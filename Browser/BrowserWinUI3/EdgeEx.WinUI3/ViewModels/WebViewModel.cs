using CommunityToolkit.Mvvm.ComponentModel;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using SqlSugar;
using System;

namespace EdgeEx.WinUI3.ViewModels
{
    public partial class WebViewModel : ObservableObject
    {
        private ISqlSugarClient db;
        private ICallerToolkit caller;
        [ObservableProperty]
        private string title;
        [ObservableProperty]
        private string icon; 
        [ObservableProperty]
        private string url; 
        [ObservableProperty]
        private bool canGoBack; 
        [ObservableProperty]
        private bool canGoForward; 
        public WebViewModel(ISqlSugarClient sqlSugarClient,ICallerToolkit callerToolkit)
        {
            db = sqlSugarClient;
            caller = callerToolkit;
        }
        /// <summary>
        /// Add BookMark
        /// </summary>
        public void AddBookMark(string screenshot, string title,string folderId)
        {
            if (Icon != null && Url != null)
            { 
                db.Storageable(new BookMark()
                {
                    Uri = Url,
                    Icon = Icon,
                    Title = title,
                    CreateTime = DateTime.Now,
                    Screenshot = screenshot,
                    FolderId = folderId,
                    Description = "",
                }).ExecuteCommand();
            }
        }
        /// <summary>
        /// Delete BookMark
        /// </summary>
        public void DeleteBookMark()
        {
            if (db.Queryable<BookMark>().First(x => x.Uri == Url) is BookMark bookMark)
            {
                if (System.IO.File.Exists(bookMark.Screenshot))
                {
                    System.IO.File.Delete(bookMark.Screenshot);
                }
                db.Deleteable<BookMark>(bookMark).ExecuteCommand();
            }
        }
        /// <summary>
        /// Call <see cref="ICallerToolkit.UriNavigatedStartingEvent"/>
        /// </summary>
        public void CallUriNavigatedStarting(object sender,string persistenceId, string tabItemName, Uri uri)
        {
            caller.UriNavigatedStarting(sender, persistenceId, tabItemName, uri);
            Url = uri.ToString();
            CallFrameStatus(sender, persistenceId, tabItemName);
        }
        /// <summary>
        /// Call <see cref="ICallerToolkit.FrameStatusEvent"/>
        /// </summary>
        public void CallFrameStatus(object sender, string persistenceId, string tabItemName)
        {
            caller.FrameStatus(sender, persistenceId, tabItemName, CanGoBack, CanGoForward, true);
        }
        /// <summary>
        /// Call <see cref="ICallerToolkit.UriNavigationCompletedEvent"/>
        /// </summary>
        public void CallUriNavigationCompleted(object sender, string persistenceId, string tabItemName, string title,string icon,Uri uri)
        {
            if (title.Length > 0 && title[0] == '\"')
            {
                title = title[1..^1];
            }
            Title = title;
            Icon = icon;
            Url = uri.ToString();
            caller.UriNavigationCompleted(sender, persistenceId, tabItemName, uri, Title
                , new ImageIconSource()
                {
                    ImageSource = new BitmapImage(new Uri(icon)),
                }); 
            CallFrameStatus(sender, persistenceId, tabItemName);
        }
    }
}
