using KS.Core.Dev.Models;

namespace KS.Core.Dev.Basket;

public class Result
{
    public List<ColumnDataModel> Columns { get; internal set; }= new List<ColumnDataModel>();
    
    public IEnumerable<List<ColumnDataModel>> Where(
        Func<ColumnDataModel, bool> predicate)
    {
        throw new NotImplementedException();

    }
    
    public IEnumerable<TResult> Select<TSource, TResult>(
        Func<TSource, int, TResult> selector)
    {
        throw new NotImplementedException();
    }
    
}