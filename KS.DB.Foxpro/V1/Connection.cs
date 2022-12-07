using System.Collections.ObjectModel;
using System.Text;
using DbfDataReader;
using KS.DB.Enums;
using KS.DB.V1.Models;

namespace KS.DB.Foxpro.V1;

public class Connection : KS.DB.Base.IConnFactory
{
    public bool Cached { get; internal set; }
    public bool Connected { get; }

    public string VersionDescription { get; internal set; }
    public bool HasMemo { get; internal set; }
    public long RecordCount { get; internal set; }

    public KS.DB.Sqlite.V1.Connection? SqlLite { get; internal set;}
    private string _source;
    private string _path;
    private Collection<DbfColumn> _columns;
    private string[] _columnsName;
    private readonly string _sqliteTable;


    /// <summary>
    /// Connect DBase.
    /// </summary>
    /// <param name="path">dbf file path</param>
    /// <param name="columns">Columns to be cached</param>
    /// <exception cref="FileNotFoundException"></exception>
    public Connection(string path, params string[] columns)
    {
        if (!System.IO.File.Exists(path))
            throw new FileNotFoundException($"database not found: {path}");

        _path = path;
        _source = path;
        _columnsName = columns;
        
        Cache(Enums.Cache.Enable);
        _sqliteTable = (new System.IO.FileInfo(_source)).Name.ToUpper();
        Connect();
    }

    
    
    private string CreateTable()
    {
        var sql = $"CREATE TABLE \"{_sqliteTable}\" (\n";
        
        var fields = new List<string>();
        
        foreach (var column in _columns)
        {
            switch (column.ColumnType)
            {
                case DbfColumnType.Boolean:
                    fields.Add($"\"{column.ColumnName}\" TEXT");
                    break;
                case DbfColumnType.Character:
                    fields.Add($"\"{column.ColumnName}\" TEXT");
                    break;
                case DbfColumnType.Currency:
                    fields.Add($"\"{column.ColumnName}\" NUMERIC");
                    break;
                case DbfColumnType.DateTime:
                case DbfColumnType.Date:
                case DbfColumnType.Memo:
                case DbfColumnType.General:
                    fields.Add($"\"{column.ColumnName}\" TEXT");
                    break;
                case DbfColumnType.Float:
                case DbfColumnType.Double:
                case DbfColumnType.Number:
                    fields.Add($"\"{column.ColumnName}\" NUMERIC");
                    break;
            }
        }
        
        sql += String.Join(',', fields);
        sql += "\n);";

        return sql;
    
    }
    
    public void Connect()
    {
        if (SqlLite is not null && SqlLite.Connected) return;
        
        using var dbfTable = new DbfTable(_path, Encoding.UTF8);
        var header = dbfTable.Header;

        VersionDescription = header.VersionDescription;
        HasMemo = dbfTable.Memo != null;
        RecordCount = header.RecordCount;

        _columns = new Collection<DbfColumn>();

        foreach (var dbfColumn in dbfTable.Columns)
        {
            if (_columnsName is not null)
                if(_columnsName.Length > 0 && !_columnsName.Contains(dbfColumn.ColumnName))
                    continue;
            
            _columns.Add(dbfColumn);
        }

        var path = KS.Core.V01.Console.File.CreateTempFile("sqlite");
        SqlLite = new Sqlite.V1.Connection(path);
        SqlLite.Connect();
        SqlLite.Execute(CreateTable());
        
        var options = new DbfDataReaderOptions
        {
            SkipDeletedRecords = true
            // Encoding = EncodingProvider.GetEncoding(1252);
        };
        
        using var dbfDataReader = new DbfDataReader.DbfDataReader(_path, options);
        
        
        while (dbfDataReader.Read())
        {
            var row = new RowModel();
            
            var line = new List<object>();
            for (int i = 0; i < _columns.Count; i++)
            {
                var colIndex = _columns[i].ColumnOrdinal;

                if (colIndex is not null)
                {
                    var colName = _columns[i].ColumnName;
                    var value = _columns[i].ColumnOrdinal;
                    row.Add(colName, dbfDataReader.GetValue(colIndex.Value));
                }
            }

            SqlLite.Insert(_sqliteTable, row);

        }
        
    }

    public void Disconnect()
    {
        SqlLite.Dispose();
    }

    public int Execute(string sql)
    {
        return SqlLite.Execute(sql);
    }

    public IEnumerable<RowModel> Query(string sql)
    {
        return SqlLite.Query(sql);
    }
    
    public void Cache(Cache cache)
    {
        var connected = Connected;
        Dispose();
        
        switch (cache)
        {
            case Enums.Cache.Disable:
                throw new NotImplementedException("The foxpro connection works cache data only");
            case Enums.Cache.Enable:
                if (!Cached)
                {
                    _path = KS.Core.V01.Console.File.TempCopy(_source);
                    Cached = true;
                }
                break;
            case Enums.Cache.Rebuild:
                //Cache(Enums.Cache.Disable);
                Cache(Enums.Cache.Enable);
                break;
        }
        
        if(connected) Connect();

    }

    public int Count(string from, string? where)
    {
        return SqlLite.Count(from, where);
    }

    public void Dispose()
    {
        if(SqlLite is not null)
            SqlLite.Disconnect();
    }
}