using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Models;
using EdgeEx.WinUI3.Toolkits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace EdgeEx.WinUI3.Adapters
{
    /// <summary>
    /// Bookmarks Adapter for Chrome/Edge
    /// </summary>
    public class BookmarkAdapter
    {
        private LocalSettingsToolkit localSettingsToolkit;
        private ISqlSugarClient db;
        private ICallerToolkit caller;
        private ResourceToolkit resourceToolkit;
        private double total;
        private double current = 0 ;
        public BookmarkAdapter()
        {
            localSettingsToolkit = App.Current.Services.GetService<LocalSettingsToolkit>();
            db = App.Current.Services.GetService<ISqlSugarClient>();
            caller = App.Current.Services.GetService<ICallerToolkit>();
            resourceToolkit = App.Current.Services.GetService<ResourceToolkit>();
        }
        public async Task<bool> ImportAsync(StorageFile file)
        {
            string text = await FileIO.ReadTextAsync(file);
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(text);
            IElement items = document.QuerySelector(@"body > dl");
            int all = document.GetElementsByTagName("A").Length;
            caller.Loading(this, true, resourceToolkit.GetString(Enums.ResourceKey.ImportBookmarks) + "...");
            total = db.Queryable<Bookmark>().Count();
            foreach (IElement item in items.Children)
            {
                if (item.NodeName == "DT")
                {
                    await CheckNodeAsync(item, "root");
                }
            }
            double b = db.Queryable<Bookmark>().Count();
            caller.Loading(this, false,"");
            return total != b;
        }
        public static DateTime TimeStampToDateTime(long timeStamp, bool inMilli = false)
        {
            DateTimeOffset dateTimeOffset = inMilli ? DateTimeOffset.FromUnixTimeMilliseconds(timeStamp) : DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            return dateTimeOffset.LocalDateTime;
        }
        public async Task<string> SaveImageFromBase64Async(string base64String, StorageFolder folder = null)
        {
            if (folder == null)
            {
                string appDataThumbsPath = localSettingsToolkit.GetString(Enums.LocalSettingName.AppDataThumbsPath);
                if (!Directory.Exists(appDataThumbsPath))
                {
                    Directory.CreateDirectory(appDataThumbsPath);
                }
                folder = await StorageFolder.GetFolderFromPathAsync(appDataThumbsPath);
            }
            if (base64String == null) return null;
            byte[] bytes = Convert.FromBase64String(base64String.Replace("data:image/png;base64,", ""));
            string fileName = $"{EncryptingHelper.CreateMd5(bytes)}.png";
            string path = Path.Combine(folder.Path, fileName);
            if (File.Exists(path)) return path;
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ms.AsRandomAccessStream());
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetSoftwareBitmap(softwareBitmap);
                    encoder.IsThumbnailGenerated = false;
                    await encoder.FlushAsync();
                }
                return file.Path;
            }
        }

        private async Task CheckNodeAsync(IElement element, string folderId)
        {
            IElement node = element?.FirstElementChild;
            if (element != null && node != null)
            {
                // is not Folder
                if (node.NodeName == "A")
                {
                    Bookmark book = new Bookmark()
                    {
                        Uri = node.GetAttribute("href"),
                        Title = node.TextContent,
                        IsFolder = false,
                        FolderId = folderId,
                        Icon = await SaveImageFromBase64Async(node.GetAttribute("icon")),
                        CreateTime = TimeStampToDateTime(Convert.ToInt64(node.GetAttribute("add_date"))),
                    };
                    db.Storageable(book).ExecuteCommand();
                    current++;
                    caller.LoadingProgress(this, Math.Round(current / total, 2));
                }
                else if (node.NodeName == "H3")
                {
                    Bookmark bookmarkFolder = new Bookmark()
                    {
                        Title = node.TextContent,
                        IsFolder = true,
                        Uri = node.GetAttribute("add_date"),
                        FolderId = folderId,
                        CreateTime = TimeStampToDateTime(Convert.ToInt64(node.GetAttribute("add_date"))),
                        LastModified = TimeStampToDateTime(Convert.ToInt64(node.GetAttribute("last_modified"))),
                    };
                    if (element.ChildElementCount > 1)
                    {
                        IElement dl = element.Children[1];
                        if (dl?.NodeName == "DL")
                        {
                            foreach (var item in dl.Children)
                            {
                                if (item.NodeName == "DT")
                                {
                                    await CheckNodeAsync(item, bookmarkFolder.Uri);
                                }
                            }
                            db.Storageable(bookmarkFolder).ExecuteCommand();
                        }
                    }
                }
            }
        }
    }
}
