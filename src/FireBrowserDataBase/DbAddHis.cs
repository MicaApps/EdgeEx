using Microsoft.Data.Sqlite;
using System;
using Windows.Storage;

namespace FireBrowserDataBase
{
    public class DbAddHis
    {
        public void AddHistData(string address, string title)
        {

            SqliteConnection m_dbConnection = new SqliteConnection($"Data Source={ApplicationData.Current.LocalFolder.Path}/History.db;");
            m_dbConnection.Open();

            var selectCmd = m_dbConnection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM urlsDb WHERE url = @url AND title = @title AND last_visit_time = @lastVisitTime";
            selectCmd.Parameters.AddWithValue("@url", address);
            selectCmd.Parameters.AddWithValue("@title", title);
            selectCmd.Parameters.AddWithValue("@lastVisitTime", DateTimeOffset.Now.ToUnixTimeSeconds());

            try
            {
                var reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    var updateCmd = m_dbConnection.CreateCommand();
                    updateCmd.CommandText = "UPDATE urlsDb SET visit_count = visit_count + 1, last_visit_time = @lastVisitTime WHERE url = @url AND title = @title AND last_visit_time = @lastVisitTime";
                    updateCmd.Parameters.AddWithValue("@url", address);
                    updateCmd.Parameters.AddWithValue("@title", title);
                    updateCmd.Parameters.AddWithValue("@lastVisitTime", DateTimeOffset.Now.ToUnixTimeSeconds());
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    var insertCmd = m_dbConnection.CreateCommand();
                    insertCmd.CommandText = "INSERT OR IGNORE INTO urlsDb (url, title, visit_count, last_visit_time) VALUES (@url, @title, @visitCount, @lastVisitTime)";
                    insertCmd.Parameters.AddWithValue("@url", address);
                    insertCmd.Parameters.AddWithValue("@title", title);
                    insertCmd.Parameters.AddWithValue("@visitCount", 1);
                    insertCmd.Parameters.AddWithValue("@lastVisitTime", DateTimeOffset.Now.ToUnixTimeSeconds());
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex)
            {
                if (ex.ErrorCode == 19) // constraint violation error code
                {
                    // execute the update command
                    var updateCmd = m_dbConnection.CreateCommand();
                    updateCmd.CommandText = "UPDATE urlsDb SET visit_count = visit_count + 1, last_visit_time = @lastVisitTime WHERE url = @url AND title = @title AND last_visit_time = @lastVisitTime";
                    updateCmd.Parameters.AddWithValue("@url", address);
                    updateCmd.Parameters.AddWithValue("@title", title);
                    updateCmd.Parameters.AddWithValue("@lastVisitTime", DateTimeOffset.Now.ToUnixTimeSeconds());
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    throw; // rethrow the exception if it's not a constraint violation error
                }
            }

            m_dbConnection.Close();


        }
    }
}
