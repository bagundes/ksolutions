namespace KS.Core.V01.Extensions;

public static class StringEstension
{
    public static bool StartWith(this string value, params string[] values)
    {
        foreach (var par in values)
        {
            if (value.StartsWith(par))
                return true;
        }

        return false;
    }

    /// <summary>
    ///  Returns a substring of this string.
    /// </summary>
    /// <param name="value">String</param>
    /// <param name="start">Index start</param>
    /// <param name="length">Length</param>
    /// <returns>String limited the length size</returns>
    public static string SubString1(this string value, int start, int length = 5000)
    {
        var size = value.Length - start;
        if (size >= length)
            return value.Substring(start, length);
        else
            return value.Substring(start, size);
    }
        
        
        
    
}