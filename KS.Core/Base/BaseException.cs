namespace KS.Core.Base;

public abstract class BaseException : Exception
{
    public override string Message { get; }
    public int Code { get; internal set; }
    
    public string NS { get; internal set; }

    public BaseException(string ns, int code, params string[] data)
    {
        Code = code;
        NS = ns;
    }
    
}