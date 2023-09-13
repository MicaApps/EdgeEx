using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using EdgeEx.WinUI3.Extensions;
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
        /// <summary>
        /// Import bookmarks from File
        /// </summary>
        public async Task<bool> ImportAsync(StorageFile file)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(await FileIO.ReadTextAsync(file));
            var items = document.QuerySelector(@"body > dl");
            total = document.GetElementsByTagName("A").Length;
            caller.Loading(this, true, resourceToolkit.GetString(Enums.ResourceKey.ImportBookmarks) + "...");
            int all = db.Queryable<Bookmark>().Count();
            foreach (IElement item in items.Children)
            {
                if (item.NodeName == "DT")
                {
                    await CheckNodeAsync(item, "root");
                }
            }
            var b = db.Queryable<Bookmark>().Count();
            caller.Loading(this, false,"");
            Log.Information("Import Bookmarks {Count} from {File} Success", total, file.DisplayName);
            return all != b;
        }
        /// <summary>
        /// Save the bookmark icon
        /// </summary>
        public async Task<string> SaveImageFromBase64Async(string base64String, StorageFolder folder = null)
        {
            if (folder is null)
            {
                var appDataThumbsPath = localSettingsToolkit.GetString(Enums.LocalSettingName.AppDataThumbsPath);
                if (!Directory.Exists(appDataThumbsPath))
                {
                    Directory.CreateDirectory(appDataThumbsPath);
                }
                folder = await StorageFolder.GetFolderFromPathAsync(appDataThumbsPath);
            }
            if (base64String is null) return null;
            var bytes = Convert.FromBase64String(base64String.Replace("data:image/png;base64,", ""));
            var fileName = $"{EncryptingHelper.CreateMd5(bytes)}.png";
            var path = Path.Combine(folder.Path, fileName);
            if (File.Exists(path)) return path;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using var ms = new MemoryStream(bytes);
            var decoder = await BitmapDecoder.CreateAsync(ms.AsRandomAccessStream());
            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetSoftwareBitmap(softwareBitmap);
                encoder.IsThumbnailGenerated = false;
                await encoder.FlushAsync();
            }
            return file.Path;
        }
        /// <summary>
        /// Check bookmark node
        /// </summary>
        private async Task CheckNodeAsync(IElement element, string folderId)
        {
            var node = element?.FirstElementChild;
            if (element != null && node != null)
            {
                // is not Folder
                if (node.NodeName == "A")
                {
                    db.Storageable(new Bookmark()
                    {
                        Uri = node.GetAttribute("href"),
                        Title = node.TextContent,
                        IsFolder = false,
                        FolderId = folderId,
                        Icon = await SaveImageFromBase64Async(node.GetAttribute("icon")),
                        CreateTime = Convert.ToInt64(node.GetAttribute("add_date")).ToDateTime(),
                    }).ExecuteCommand();
                    current++;
                    caller.LoadingProgress(this, Math.Round(current / total, 2));
                }
                else if (node.NodeName == "H3")
                {
                    var bookmarkFolder = new Bookmark()
                    {
                        Title = node.TextContent,
                        IsFolder = true,
                        Uri = node.GetAttribute("add_date"),
                        FolderId = folderId,
                        CreateTime = Convert.ToInt64(node.GetAttribute("add_date")).ToDateTime(),
                        LastModified = Convert.ToInt64(node.GetAttribute("last_modified")).ToDateTime(),
                    };
                    if (element.ChildElementCount > 1)
                    {
                        var dl = element.Children[1];
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
