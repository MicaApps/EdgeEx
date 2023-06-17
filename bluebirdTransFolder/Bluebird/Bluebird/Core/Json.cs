using Windows.Storage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Bluebird.Core;

public class Json
{
    public static async void CreateJsonFile(string file, string title, string url)
    {
        // Generate json
        string json = "[{\"title\":\"" + title + "\"," + "\"url\":\"" + url + "\"}]";
        // create json file
        await localFolder.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);
        // get json file
        var fileData = await ApplicationData.Current.LocalFolder.GetFileAsync(file);
        // write json to json file
        await FileIO.WriteTextAsync(fileData, json);
    }

    public static async void AddItemToJson(string file, string title, string url)
    {
        var fileData = await localFolder.TryGetItemAsync(file);
        if (fileData == null) CreateJsonFile(file, title, url);
        else
        {
            // get json file content
            string json = await FileIO.ReadTextAsync((IStorageFile)fileData);
            // new historyitem
            JsonItems newHistoryitem = new()
            {
                Title = title,
                Url = url
            };
            // Convert json to list
            List<JsonItems> historylist = JsonConvert.DeserializeObject<List<JsonItems>>(json);
            // Add new historyitem
            historylist.Insert(0, newHistoryitem);
            // Convert list to json
            string newJson = JsonConvert.SerializeObject(historylist);
            // Write json to json file
            await FileIO.WriteTextAsync((IStorageFile)fileData, newJson);
        }
    }

    public static async Task<List<JsonItems>> GetListFromJsonAsync(string file)
    {
        var fileData = await localFolder.TryGetItemAsync(file);
        if (fileData == null) return null;
        else
        {
            string filecontent = await FileIO.ReadTextAsync((IStorageFile)fileData);
            return JsonConvert.DeserializeObject<List<JsonItems>>(filecontent);
        }
    }
}
