using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace FireBrowserFavorites
{
    public class Json
    {
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public static async Task CreateJsonFileAsync(string file, string title, string url)
        {
            // Generate json
            string json = JsonConvert.SerializeObject(new List<Globals.JsonItems> {
            new Globals.JsonItems
            {
                Title = title,
                Url = url
            }
            });

            // create json file
            await localFolder.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);

            // get json file
            var fileData = await localFolder.GetFileAsync(file);

            // write json to json file
            await FileIO.WriteTextAsync(fileData, json);
        }

        public static async Task AddItemToJson(string file, string title, string url)
        {
            var fileData = await localFolder.TryGetItemAsync(file);
            if (fileData == null) await CreateJsonFileAsync(file, title, url);
            else
            {
                // get json file content
                string json = await FileIO.ReadTextAsync(fileData as IStorageFile);

                // new historyitem
                Globals.JsonItems newHistoryitem = new Globals.JsonItems()
                {
                    Title = title,
                    Url = url
                };

                // Convert json to list
                List<Globals.JsonItems> historylist = JsonConvert.DeserializeObject<List<Globals.JsonItems>>(json);

                // Add new historyitem
                historylist.Insert(0, newHistoryitem);

                // Convert list to json
                string newJson = JsonConvert.SerializeObject(historylist);

                // Write json to json file
                await FileIO.WriteTextAsync(fileData as IStorageFile, newJson);
            }
        }

        public static async Task<List<Globals.JsonItems>> GetListFromJsonAsync(string file)
        {
            var fileData = await localFolder.TryGetItemAsync(file);
            if (fileData == null) return null;
            else
            {
                string filecontent = await FileIO.ReadTextAsync(fileData as IStorageFile);
                return JsonConvert.DeserializeObject<List<Globals.JsonItems>>(filecontent);
            }
        }
    }

}
