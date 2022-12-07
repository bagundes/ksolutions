namespace KS.Core.V01.Models;

public class CredentialModel : Base.CredentialBaseModel<CredentialModel>
{
    public CredentialModel(string user, string password, string? key = null)
    {
        UserName = user;
        if (!string.IsNullOrEmpty(key))
            ChangeTheKey(key);
        
        SetPassword(password);
    }
}