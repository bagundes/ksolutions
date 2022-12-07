using System.Text.RegularExpressions;
using KS.Core.V01;
using KS.Core.V01.Definition;
using KS.Core.V01.Models;

namespace KS.Shared;

public class Variables
{
    public enum VariableTypeEnum
    {
        Global = '@',
        Local = '$',
        Internal = '%',
        
    }

    /// <summary>
    /// Create regex for variable
    /// </summary>
    public static string VariableRegex(VariableTypeEnum varType, bool value, bool mask)
    {
        if (value || mask)
            throw new NotImplementedException();
        
        return $"/{{{(char) varType}[A-Z]*}}/g"; 
    }

    /// <summary>
    /// Check if syntax contains variable definition.
    /// </summary>
    /// <param name="syntax">for example: "Good morning {@USER}."</param>
    /// <returns></returns>
    public static bool IsVariable(string syntax)
    {
     //syntax.StartsWith("{$") || syntax.StartsWith("{@") || syntax.StartsWith("{%");
     
     foreach (VariableTypeEnum variable in Enum.GetValues(typeof(VariableTypeEnum)))
     {
         var regex = new Regex(VariableRegex(variable, false, false));
         var ret = regex.IsMatch(syntax);
         if (ret) return true;
     }

     return false;
    }
    
    /// <summary>
    /// Add variable or function.
    /// </summary>
    /// <param name="syntax"></param>
    /// <returns></returns>
    public static string Add(string syntax)
    {
        if (IsVariable(syntax))
        {
            AddVariable(syntax);
            return String.Empty;
        }
        else
            return AddFunction(syntax);

    }

    /// <summary>
    /// remove variable or function.
    /// </summary>
    /// <param name="key"></param>
    public static void Remove(string key)
    {
        if(IsVariable(key))
            RemoveVariable(key);
        else
            RemoveFunction(key);
    }

    /// <summary>
    /// Get value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetValue(string key)
    {
        if (IsVariable(key))
            return ValueVariable(key);
        else
            return PrintFunction(key);

    }
    
    #region Function
    private static Dictionary<string, Function>? _functions;

    /// <summary>
    /// Add function
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>


    
    private static string AddFunction(string func)
    {
        _functions ??= new Dictionary<string, Function>();
        var key = KS.Core.V01.Security.Hash.RandomMD5().Substring(0, 10);
        _functions.Add(key, new Function(func));

        return key;
    }
    
    /// <summary>
    /// Remove function
    /// </summary>
    /// <param name="key"></param>
    private static void RemoveFunction(string key)
    {
        if (_functions.ContainsKey(key))
            _functions.Remove(key);
    }

    private static string PrintFunction(string key) => _functions[key].PrintValue();
    #endregion

    #region Variable

    private static Dictionary<string, (string mask, dynamic value)> _variables;

    private static void AddVariable(string value)
    {
        _variables ??= new Dictionary<string, (string mask, dynamic value)>(); 
        var ns = KS.Core.V01.Reflections.GetStackTraceClass(2);
        string[] values = new string[2];
        // {@VAR1:5000|#,##0.00}
        value = value
            .Replace("{", "")
            .Replace("}", "");

        var variable = value.Split(':');
        values = variable[1].Split('|');

        _variables.Add($"{ns.@namespace}{variable[0]}",(values[1], values[0]));
    }

    private static void RemoveVariable(string key)
    {
        
    }

    private static string ValueVariable(string key)
    {
        // @todo When variable is @global, don't need to check the namespace
        // @todo When variable is $local, only assembly can access the variable
        // @todo When variable is %internal, only the class can access it.
        // @todo The return need to be formated.
        
        throw new NotImplementedException();
        if (key.StartsWith("@"))
        {
            var data = _variables
                .Where(t => t.Key.EndsWith(key))
                .Select(t => t.Value).FirstOrDefault();
            
            
        }

        if(!_variables.ContainsKey(key)) return String.Empty;
        
        
        
        throw new NotImplementedException();
    }
    #endregion




    // private static readonly Dictionary<string, KS.Core.V01.Models.KvmlModel> _variables_ = new Dictionary<string, KvmlModel>();
    //
    // public static void Add(KS.Core.V01.Models.KvmlModel kvml)
    // {
    //     var name = kvml.ToString();
    //
    //     if (_variables_.ContainsKey(name))
    //         _variables_[name] = kvml;
    //     else
    //         _variables_.Add(name, kvml);
    // }
    //
    // public static void Add(KS.Core.V01.Models.KvmlModel[] kvml)
    // {
    //     foreach (var v in kvml)
    //         Add(v);
    // }
}