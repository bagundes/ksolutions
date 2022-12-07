namespace KS.Core.Base;

public interface IBaseModel
{
    public string GetHash();
}

public class BaseModel<T> : IBaseModel
{

    /// <summary>
    /// Create Unique ID for this model.
    /// </summary>
    /// <returns>MD5</returns>
    public virtual string GetHash()
    {
        var hash = string.Empty;
        var properties = this.GetType().GetProperties();
        var typeModel = typeof(T);
        
        foreach (var property in properties)
            hash += property.GetValue(this, null).ToString();

        return KS.Core.V01.Security.Hash.MD5(hash);
    }
    
    /// <summary>
    /// Compare if the models contains the same fields values.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public virtual bool Equals(IBaseModel obj)
    {
        return GetHash() == obj.GetHash();
    }

    /// <summary>
    /// Convert model to json
    /// </summary>
    /// <returns></returns>
    public virtual string ToJson()
    {
        return KS.Core.V01.Dynamic.ObjectToJson(this);
    }
    
}