using System.Security.Cryptography;
using System.Text;

namespace KS.Core.V01.Security;

public class TripeCrypt
{
    public const string DATA = "@{TC_CRYPT}:";
    
    public static string Encrypt(string input, string key)
    {
        try
        {
            var inputArray = Encoding.UTF8.GetBytes(input);
            var tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            var cTransform = tripleDES.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return $"{DATA}{System.Convert.ToBase64String(resultArray, 0, resultArray.Length)}";
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            throw;
        }
        
    }

    public static string Decrypt(string input, string key)
    {
        input = input.Replace(DATA, "");
        var inputArray = System.Convert.FromBase64String(input);
        var tripleDES = new TripleDESCryptoServiceProvider();
        tripleDES.Key = Encoding.UTF8.GetBytes(key);
        tripleDES.Mode = CipherMode.ECB;
        tripleDES.Padding = PaddingMode.PKCS7;
        var cTransform = tripleDES.CreateDecryptor();
        var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        tripleDES.Clear();
        return Encoding.UTF8.GetString(resultArray);
    }
}