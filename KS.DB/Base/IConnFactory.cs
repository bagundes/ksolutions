namespace KS.DB.Base;

public interface IConnFactory : IDisposable
{
    bool Connected { get; }
    void Connect();
    void Disconnect();
    int Execute(string sql);
    IEnumerable<KS.DB.V1.Models.RowModel> Query(string sql);

    bool Cached { get; }

    /// <summary>
    /// Update the cache system status. This function does not work for all Connection Factory.
    /// </summary>
    /// <param name="cache"></param>
    void Cache(KS.DB.Enums.Cache cache);

    int Count(string from, string? where);
}