using Newtonsoft.Json;

namespace KS.Core.Base;

public abstract class CredentialBaseModel<T> : KS.Core.Base.BaseModel<T>
{

    private string _userName = String.Empty;
    private string _password;
    private string _key;

    private string Key
    {
        get
        {
            if (String.IsNullOrEmpty(_key))
                return _userName + V01.Console.File.ContentFile("key", true);
            else
                return _key;
        }
    }

    /// <summary>
    /// User name.
    /// </summary>
    public string UserName
    {
        get
        {
            return _userName;
        }
        set
        {
            _userName = value;
        }
    }

    /// <summary>
    /// Encrypted password
    /// </summary>
    public string EPassword { get; set; }


    [JsonIgnore]
    public string Password => GetPassword();
    
    /// <summary>
    /// Get the password.
    /// </summary>
    /// <returns></returns>
    private string GetPassword()
    {
        if (!String.IsNullOrEmpty(_password))
            _password = KS.Core.V01.Security.TripeCrypt.Decrypt(EPassword, Key);

        return _password;
    }

    /// <summary>
    /// Set the password
    /// </summary>
    /// <param name="passwd"></param>
    public void SetPassword(string passwd)
    {
        _password = passwd;
        EPassword = KS.Core.V01.Security.TripeCrypt.Encrypt(passwd, Key);
    }

    /// <summary>
    /// Change the crypt key.
    /// </summary>
    /// <param name="newkey">New key</param>
    public void ChangeTheKey(string newkey)
    {
        var pwd = Password;
        _key = newkey;
        SetPassword(pwd);
        
    }
}