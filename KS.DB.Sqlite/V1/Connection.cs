using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using KS.DB.Enums;
using KS.DB.V1.Models;
using Microsoft.VisualBasic;

namespace KS.DB.Sqlite.V1;

public class Connection : KS.DB.Base.IConnFactory
{
    private string _source;
    private string _path;
    public bool Connected => _sqlite.State == ConnectionState.Open;
    private SQLiteConnection _sqlite;
    public bool Cached { get; internal set; } = false;

    
    public Connection(string path)
    {
        try
        {
            
            if(!System.IO.File.Exists(path))
                SQLiteConnection.CreateFile(path);
            
            _source = path;
            _path = path;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    public void Connect()
    {
        _sqlite =  new SQLiteConnection($"Data Source={_path};Version=3;");
        _sqlite.Open();
    }

    public void Disconnect()
    {
        if(Connected)
            _sqlite.Close();
    }

    public int Execute(string sql)
    {
        var com =  new SQLiteCommand(sql, _sqlite);
        return com.ExecuteNonQuery();
    }

    public int Insert(string table, KS.DB.V1.Models.RowModel row)
    {

        var values = new List<string>();
        var columns = String.Join(',', row.Columns.Select(t => $"\"{t.ToString()}\""));

    for (int i = 0; i < row.Length; i++)
        {
            values.Add(row.GetSqlValue(i));
        }

        var sql = $"INSERT INTO \"{table}\" ({String.Join(',',columns)}) VALUES ({String.Join(',',values)})";

        return Execute(sql);
    }

    private void CreateTable(string table, KS.DB.V1.Models.RowModel row)
    {
        if(Count("sqlite_master",$"type='table' AND name='{table}'") > 0) return;
        
        var sql = $"CREATE TABLE \"{table}\" (\n";
        var columns = new List<string>();
        
        for (int i = 0; i < row.Length; i++)
        {
            var col = row.Columns[i];
            var valType = row[i].GetType().ToString();
            
            switch (valType)
            {
                case "System.String": columns.Add($"\"{col}\" TEXT");
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64": columns.Add($"\"{col}\" INTEGER");
                    break;
                case "System.Decimal":columns.Add($"\"{col}\" NUMERIC");
                    break;
                case "System.Boolean": columns.Add($"\"{col}\" TEXT");
                    break;
                default: throw new NotImplementedException($"Sqlite: {col} ({valType}) does not implemented");
            }
        }

        var foo = Strings.Join(columns.ToArray(), ";");
        Execute($"CREATE TABLE \"{table}\" ({foo});");
    }
    
    
    public IEnumerable<RowModel> Query(string sql)
    {
        var com =  new SQLiteCommand(sql, _sqlite);
        var reader = com.ExecuteReader();

        while (reader.NextResult())
        {
            var row = new RowModel();
            for(int i = 0; i < reader.FieldCount; i++)
                row.Add(reader.GetName(i), reader.GetValue(i));
            yield return row;
        }
    }
    
    public void Cache(Cache cache)
    {
        var connected = Connected;
        Dispose();
        
        switch (cache)
        {
            case Enums.Cache.Disable:
                if (Cached)
                {
                    System.IO.File.Delete(_path);
                    _path = _source;
                    Cached = false;
                }
                break;
            case Enums.Cache.Enable:
                if (!Cached)
                {
                    _path = KS.Core.V01.Console.File.TempCopy(_source);
                    Cached = true;
                }
                break;
            case Enums.Cache.Rebuild:
                Cache(Enums.Cache.Disable);
                Cache(Enums.Cache.Enable);
                break;
        }
        
        if(connected) Connect();
    }

    public int Count(string from)
    {
        throw new NotImplementedException();
    }

    public int Count(string from, string? where)
    {
        var sql = $"SELECT COUNT(*) AS \"COUNT\" FROM \"{from}\" ";
        sql += where is not null ? $"WHERE {where}" : String.Empty;

        var res = (RowModel) Query(sql);

        return (int)res[0];

    }

    public void Dispose()
    {
        if(Connected)
            _sqlite.Close();
    }
}