using KS.Core.Base;

namespace KS.Core.Dev.Models;

public class ColumnDataModel : BaseModel<ColumnDataModel>
{
    /// <summary>
    /// Column position
    /// </summary>
    public int Pos { get; internal set; }
    /// <summary>
    /// Column name
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Values from Column
    /// </summary>
    public List<dynamic?> Values { get; internal set; }

    public int Count() => Values.Count();
    
    /// <summary>
    /// Get value.
    /// </summary>
    /// <param name="index">Index value position</param>
    /// <returns>If position is greater than list, it will return null</returns>
    public dynamic? GetValue(int index)
    {
        if (Values.Count() < index)
            return Values[index];
        else
            return null;
    }
}