using KS.DB.Base;

namespace KS.DB;

/// <summary>
/// Connections Shared.
/// </summary>
public class Connections
{
    private static Dictionary<string, Base.IConnFactory> _conns = new Dictionary<string, IConnFactory>();

    /// <summary>
    /// Add the connection
    /// </summary>
    /// <param name="connFactory"></param>
    /// <param name="alias"></param>
    /// <returns></returns>
    public static string Add(Base.IConnFactory connFactory, string? alias = null)   
    {
        if (String.IsNullOrEmpty(alias))  alias = KS.Core.V01.Security.Hash.RandomNumber(5, 5).ToString();

        _conns.Add(alias, connFactory);
        return alias;
    }

    /// <summary>
    /// Get the connection
    /// </summary>
    /// <param name="alias"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static T Get<T>(string alias) where T : Base.IConnFactory
    {
        if (!_conns.ContainsKey(alias))
            throw new KeyNotFoundException($"The connection {alias} does not exists.");
        else
            return (T) _conns[alias];

    }
}