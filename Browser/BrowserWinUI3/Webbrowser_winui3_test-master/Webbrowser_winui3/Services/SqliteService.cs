using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace Webbrowser_winui3.Services;
public static class SqliteService
{
    public static SqliteConnection db;
    /// <summary>
    /// "{name}.db" "Name text,Url text,Date text"
    /// </summary>
    /// <param name="name"></param>
    public static async void CreatDBByName(string dbname,string tablename,string columns)
    {
        var file=await ApplicationData.Current.LocalFolder.CreateFileAsync($"{dbname}.db", CreationCollisionOption.OpenIfExists);
        db = new SqliteConnection($"Filename={file.Path}");
        db.Open();
        try
        {
            var tableCommand = $"Create table {tablename} ({columns})";
            SqliteCommand createTable = new SqliteCommand(tableCommand, db);
            createTable.ExecuteReader();
        }
        catch { }
        db.Close();
    }
    /// <summary>
    /// "{name}.db"
    /// </summary>
    /// <param name="name"></param>
    public static async void DeleteDBByName(string name)
    {
        var f=await ApplicationData.Current.LocalFolder.GetFileAsync($"{name}.db");
        await f.DeleteAsync();
    }
    /// <summary>
    /// command:SELECT * from table ORDER BY ID"
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static SqliteDataReader EditDBByCommand(string command)
    {
        db.Open();
        SqliteCommand selectCommand = new SqliteCommand(command, db);
        SqliteDataReader query = selectCommand.ExecuteReader();
        db.Close();
        return query;
    }
    /// <summary>
    /// columns:Id , Name values:1,'Mike'
    /// </summary>
    /// <param name="tablename"></param>
    /// <param name="columns"></param>
    /// <param name="value"></param>
    public static void InsertColumn(string tablename,string columns,string values)
    {
        db.Open();
        var tableCommand = $"insert into {tablename} (columns) values(values)";
        SqliteCommand createTable = new SqliteCommand(tableCommand, db);
        createTable.ExecuteReader();
        db.Close();
    }
    /// <summary>
    /// where:id=1,value='mike',SqliteDataReader.GetString(0),调用后执行SqliteService.db.Close();
    /// </summary>
    /// <param name="tablename"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public static SqliteDataReader ReadTableData(string tablename, string column,string orderby ,string where)
    {
        db.Open(); 
        string orderbyt = "";
        if (orderby != "") orderbyt = " order by " + orderby;
        string wheret = "";
        if (where != "") wheret = " where " + where;
        SqliteCommand selectCommand = new SqliteCommand($"Select {column} From {tablename}{orderby}{wheret}", db);
        SqliteDataReader query = selectCommand.ExecuteReader();
        return query;
    }
    /// <summary>
    /// where:id=1,value='mike'
    /// </summary>
    /// <param name="tablename"></param>
    /// <param name="column"></param>
    /// <param name="value"></param>
    /// <param name="whrer"></param>
    /// <returns></returns>
    public static SqliteDataReader UpdateTableData(string tablename, string column,string value,string where)
    {
        db.Open();
        SqliteCommand selectCommand = new SqliteCommand($"update {tablename} set {column}='{value}' where {where}", db);
        SqliteDataReader query = selectCommand.ExecuteReader();
        db.Close();
        return query;
    }
    /// <summary>
    /// where:id=1,value='mike' where="":Clear all table data
    /// </summary>
    /// <param name="tablename"></param>
    /// <param name="where"></param>
    public static void DeleteTableData(string tablename, string where)
    {
        db.Open();
        var wheret = "";
        if (where != "") wheret = $" where { where}";
        SqliteCommand selectCommand = new SqliteCommand($"delete from {tablename}{wheret}", db);
        SqliteDataReader query = selectCommand.ExecuteReader();
        db.Close();
    }
}
