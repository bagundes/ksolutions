namespace KS.Network.V1.Helpers;

public static class UriHelper
{
    /// <summary>
    /// Combine URI and paths, removing double reverse dash.
    /// </summary>
    /// <param name="uri">address</param>
    /// <param name="paths">paths</param>
    /// <returns>URI (user toString to get uri)</returns>
    public static Uri CombineToString(string uri, params string[] paths)
    {
        var path = String.Join('/', paths).Replace("//", "/"); 
        return new Uri(new Uri(uri), path);
    }
}