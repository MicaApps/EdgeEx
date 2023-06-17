using Microsoft.Data.Sqlite;
using System;
using System.IO;
using Windows.Storage;

namespace FireBrowserDataBase
{
    public class DbCreation
    {
        public static async void CreateDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("History.db", CreationCollisionOption.OpenIfExists);

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = Path.Combine(ApplicationData.Current.LocalFolder.Path, "History.db");

            using (var db = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                     "EXISTS urlsDb (Url NVARCHAR(2083) PRIMARY KEY NOT NULL, " +
                     "Title NVARCHAR(2048), " +
                     "Visit_Count INTEGER, " +
                     "Last_Visit_Time DATETIME)";


                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
    }
}
