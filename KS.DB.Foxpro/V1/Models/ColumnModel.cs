using DbfDataReader;

namespace KS.DB.Foxpro.V1.Models;

public class ColumnModel
{
    public int Pos { get; internal set; }
    public string Name { get; internal set; }
    public DbfColumnType DbfColumnType { get; internal set; }
    public int Length { get; internal set; }
    public int DecimalCount { get; internal set; }
    public List<object?> Value { get; internal set; } = new List<object?>();
    

    public ColumnModel(DbfColumn dbfColumn)
    {
        Pos = dbfColumn.ColumnOrdinal ?? -1;
        Name = dbfColumn.ColumnName;
        DbfColumnType = dbfColumn.ColumnType;
        Length = dbfColumn.Length;
        DecimalCount = dbfColumn.DecimalCount;
    }

    public void Add(ref DbfDataReader.DbfDataReader dbfDataReader)
    {
        if (dbfDataReader.IsDBNull(Pos))
        {
            Value.Add(null);
            return;
        }

        switch (DbfColumnType)
        {
            case DbfColumnType.Character:
            case DbfColumnType.Memo:
                Value.Add(dbfDataReader.GetString(Pos));
                break;
            case DbfColumnType.Date:
                Value.Add(dbfDataReader.GetDateTime(Pos));
                break;
            case DbfColumnType.Number:
                Value.Add(dbfDataReader.GetInt32(Pos));
                break;
            default:
                throw new NotImplementedException();
        }
    }
}


/*
         Number = 'N',
        SignedLong = 'I',
        Float = 'F',
        Currency = 'Y',
        Date = 'D',
        DateTime = 'T',
        Boolean = 'L',
        Memo = 'M',
        Double = 'B',
        General = 'G',
        Character = 'C'
 */