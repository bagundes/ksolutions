using KS.Core.Base;
using KS.Core.V01.Definition;

namespace KS.Core.V01.Models;

/// <summary>
/// Kurumin Variable Markup Language.
/// </summary>
[Obsolete]
public class KvmlModel : BaseModel<KvmlModel>
{


    /// <summary>
    /// Source that created the variable
    /// </summary>
    public string Source { get; internal set; }
    /// <summary>
    /// Variable type
    /// </summary>
    public Values.KvmlTypeEnum TypeEnum { get; internal set; }
    /// <summary>
    /// Variable Name
    /// </summary>
    public string Name { get; internal set; }
    /// <summary>
    /// Variable value
    /// </summary>
    public dynamic Value { get; set; }


    public KvmlModel(Reflections.Stack stack, string value)
    {
        var variable = value.Split('=');
        Load(stack.@namespace,variable[0], variable[1]);
    }
    
    /// <summary>
    /// Load the variable.
    /// </summary>
    /// <param name="type">Type of project contains variable</param>
    /// <param name="value">For example @variable=value</param>
    public KvmlModel(Type type, string value)
    {
        var variable = value.Split('=');
        Load(type.Namespace, variable[0], variable[1]);
    }

    /// <summary>
    /// Load the variable
    /// </summary>
    /// <param name="type">Type of project contains variable</param>
    /// <param name="name">Variable name (with definition type)</param>
    /// <param name="value">Value</param>
    public KvmlModel(Type type, string name, dynamic value)
        => Load(type.Namespace, name, value);
    
    private void Load(string @namespace, string name, dynamic value)
    {
        if (@namespace != null)
            Source = @namespace;
        else
            throw new TypeAccessException($"The namespace is null or empty");
        
        Name = name;
        Value = value;
        DefineType();
    }

    private void DefineType()
    {
        var type = Name[0];

        try
        {
            TypeEnum = (Values.KvmlTypeEnum)Enum.ToObject(typeof(Values.KvmlTypeEnum), type);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"The variable name: {Name} is not have the KVML definition. {e.Message}");
        }
    }

    public override string ToString()
    {
        if(TypeEnum == Values.KvmlTypeEnum.Global)
            return $"GLOBAL: {Name}={Value}";
        else
            return $"{Source}: {Name}={Value}";
    }
}

