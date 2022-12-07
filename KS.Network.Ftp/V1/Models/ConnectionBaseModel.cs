namespace KS.Network.Ftp.V1.Models;

public class ConnectionBaseModel : KS.Core.Base.CredentialBaseModel<ConnectionBaseModel>
{
    public virtual string Address { get; set; }
    public virtual string Path { get; set; }
}