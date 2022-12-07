using System.Reflection;
using KS.Core.V01.Definition;
using KS.Core.V01.Extensions;
using KS.Core.V01.Models;

namespace KS.Core.V01.Console;

public static class Environment
{

    public static Dictionary<string, dynamic> GetCommandLineArgs()
    {
        var args = System.Environment.GetCommandLineArgs();
        var @params = new Dictionary<string, dynamic>();
        
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.StartsWith("--") && arg.Length > 2)
            {
                arg = arg.Substring(2);
                @params.Add(arg, args[++i]);
            }
            else if (arg.StartsWith("-") && arg.Length > 1)
            {
                arg = arg.Substring(1);
                @params.Add(arg, true);
            }
        }

        return @params;
    }

    /// <summary>
    /// Return the parameters and flags arguments.
    /// In case of arguments contains KVML, the method will register it.
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public static (Dictionary<string, string> @params, KvmlModel[] @variables, char[] flags) GetCommandLineArgs1()
    {
        var @params = new Dictionary<string, string>();
        var @variables = new List<KvmlModel>();
        var @flags = new List<char>();
        
        // @bfagundes - Convert Enum to array
        var kvmlChars =Enum.GetValues(typeof(Values.KvmlTypeEnum))
            .Cast<Values.KvmlTypeEnum>()
            .Select(d => ((char)d))
            .ToArray(); 
        
        
        var args = System.Environment.GetCommandLineArgs();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.StartsWith("--") && arg.Length > 2)
            {
                arg = arg.Substring(2);
                    @params.Add(arg, args[++i]);
            }
            else if (arg.StartsWith("-") && arg.Length > 1)
            {
                arg = arg.Substring(1);
                foreach (var f in arg)
                    flags.Add(f);
            }
            else if (arg[0].Contains(kvmlChars))
            {
                var stack = KS.Core.V01.Reflections.GetStackTraceClass();
                @variables.Add(new KvmlModel(stack, arg));
            }
            
        }

        return (@params, variables.ToArray(), flags.ToArray());
    }

    /// <summary>
    ///  Get product details from assembly properties.
    /// </summary>
    /// <param name="type">Refence type</param>
    /// <returns></returns>
    public static (string? company, string? @namespace, string? product, string? version) GetProductDetails(Type type)
    {
        var assembly = System.Reflection.Assembly.GetAssembly(type);
        string? company = null, @namespace = null, product = null, version = null;
        
        const string divisor = " | ";
        
        if (assembly != null)
        {
            object[] customAttributes;
            
            #region company
            customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);  
            if (customAttributes.Length > 0)
                company = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
            #endregion
            
            #region product
            customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

            if (customAttributes.Length > 0)
            {
                var foo = ((AssemblyProductAttribute)customAttributes[0]).Product;
                if (foo.Contains(divisor))
                {
                    var bar = foo.Split(divisor);
                    @namespace = bar[0].Trim();
                    product = bar[1].Trim();
                }
                else
                {
                    product = foo;
                }
            }
            #endregion
            
            #region version
            customAttributes = assembly.GetCustomAttributes(typeof(AssemblyVersionAttribute), false);
            if (customAttributes.Length > 0)
                version = ((AssemblyVersionAttribute)customAttributes[0]).Version;
            #endregion
            
        }  
        return (company, @namespace, product, version);  
    }
}