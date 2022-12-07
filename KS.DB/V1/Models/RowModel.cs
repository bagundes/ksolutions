using System.Data;

namespace KS.DB.V1.Models;

/// <summary>
/// Row database.
/// </summary>
public class RowModel : KS.Core.Base.BaseModel<RowModel>
{
    private List<string> _columns;
    public string[] Columns => _columns.ToArray();
    
    private List<dynamic> _values;
    public int Length => _columns.Count();
    
    
    /// <summary>
    /// Select value per column position.
    /// </summary>
    /// <param name="index"></param>
    public dynamic this[int index]
    {
        get
        {
            if (index > Length)
                throw new IndexOutOfRangeException($"The {this.GetType().Name} row contains only {Length} columns");
            else
                return _values[index];
        }
    }

    /// <summary>
    /// Select value per column name.
    /// </summary>
    /// <param name="column"></param>
    public dynamic this[string column]
    {
        get
        {
            if (!_columns.Contains(column))
                throw new DataException($"The {this.GetType().Name} row does not contain the column {column}.");
            else
                return _values[_columns.IndexOf(column)];
        }
    }

    public RowModel()
    {
        _columns = new List<string>();
        _values = new List<dynamic>();
    }

    /// <summary>
    /// Load row data
    /// </summary>
    /// <param name="columns">Column name</param>
    /// <param name="values">Values</param>
    public RowModel(string[] columns, dynamic[] values)
    {
        _columns = new List<string>(columns);
        _values = new List<dynamic>(values);
    }

    /// <summary>
    /// Add values in the row.
    /// </summary>
    /// <param name="column">Column name</param>
    /// <param name="value">value</param>
    public void Add(string column, dynamic value)
    {
        _columns.Add(column);
        _values.Add(value);
    }

    /// <summary>
    /// Update cell.
    /// </summary>
    /// <param name="column">Column name</param>
    /// <param name="value">Value</param>
    /// <exception cref="DataException"></exception>
    public void Update(string column, dynamic value)
    {
        if (!_columns.Contains(column))
            throw new DataException($"Column: {column} does not exist");

        var index = _columns.IndexOf(column);
        _columns[index] = column;
        _values[index] = value;
    }

    public override string ToJson()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get value sql format.
    /// </summary>
    /// <param name="index">Column index</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetSqlValue(int index)
    {

        var value = _values[index];

        var valueType = value.GetType();

        switch (valueType.ToString())
        {
            case "System.String": return $"'{((string) value).Replace("'","''")}'";
            case "System.Int16":
            case "System.Int32":
            case "System.Int64": return $"{(int)value}";
            case "System.Decimal":return $"{(decimal)value}";
            case "System.Boolean": return ((bool)value) ? "'Y'" : "'N'";
            default: throw new NotImplementedException($"SqlValue: {value} ({valueType}) does not implemented");
        }
    }
}