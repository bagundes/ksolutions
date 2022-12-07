namespace KS.Core.V01.Security;

public class Crypt
{
    /// <summary>
    /// Encrypt the text using encrypt system default.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Encrypt(string text)
    {
        var key = V01.Console.File.ContentFile("key", true);
        return TripeCrypt.Encrypt(text, key);
    }
    
    /// <summary>
    /// Decrypt the data using encrypt system default.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    /// <exception cref="KCoreException">2 - Invalid tag.</exception>
    public static string Decrypt(string text)
    {
        var key = V01.Console.File.ContentFile("key", true);
        if (text.StartsWith(TripeCrypt.DATA))
        {
            return TripeCrypt.Decrypt(text, key);
        }

        if (text.StartsWith("@"))
            throw new KCoreException(2);
        else
            return text;
    }
}