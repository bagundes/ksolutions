namespace KS.Core;

public class KCoreException : Base.BaseException
{
    public KCoreException(int code, params string[] data) : base (Init.Product.@namespace, code, data)
    {
    }
}