namespace KS.Core.V01.Definition;

public class Function
{
    
    const string FN_MAX = "MAX";
    const string FN_MIN = "MIN";
    const string FN_COUNT = "COUNT";
    const string FN_PRINT = "PRINT";
    const string FN_VALUE = "VALUE";
    
    public enum State
    {
        True,
        False,
        End,
    }

    private State state; 
    
    private string value;
    /// <summary>
    /// Value to print.
    /// </summary>
    public string Value
    {
        get
        {
            if (string.IsNullOrEmpty(value))
                return counter.ToString();
            else
                return this.value;
        }
        set
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Counter variable.
    /// </summary>
    private int counter;
    /// <summary>
    /// Increment parameter.
    /// </summary>
    private int increment;
    /// <summary>
    /// Value minimum.
    /// </summary>
    private int min = -25000;
    /// <summary>
    /// Value maximum.
    /// </summary>
    private int max = 25000;
    /// <summary>
    /// Value to print.
    /// </summary>
    private string print = String.Empty;

    /// <summary>
    /// Print value without validate the status.
    /// </summary>
    public string Print
    {
        get
        {

            if (this.print.Contains("$"))
            {
               var print = this.print
                    .Replace("$VALUE", Value)
                    .Replace("$COUNT", counter.ToString())
                    .Replace("$MIN", min.ToString())
                    .Replace("$MAX", max.ToString());

                return print;
            }

            if (string.IsNullOrEmpty(print))
                return Value;
            else
                return this.print;
        }
    }

    public Function(string function)
    {
        var functions = function.Split(';');

        foreach (var func in functions)
        {
            var func_name = func;
            if(func.Contains(':'))
                func_name = func.Substring(0, func.IndexOf(':'));

            switch (func_name)
            {
                case FN_COUNT:
                    var ret0 = CountFunction(func);
                    counter = ret0.ni - ret0.nc;
                    increment = ret0.nc;
                    Counter();
                    break;
                case FN_MIN:
                    this.min = MinFunction(func);
                    break;
                case FN_MAX:
                    this.max = MaxFunction(func);
                    break;
                case FN_PRINT: 
                    this.print = PrintFunction(func);
                    break;
                case FN_VALUE:
                    this.value = ValueFunction(func);
                    break;
            }
        } 
        
    }
    
    /// <summary>
    /// Function Count
    /// </summary>
    /// <param name="function">For Example: COUNT:0,1</param>
    /// <returns>Initial and Counter numbers </returns>
    /// <exception cref="Exception"></exception>
    private static (int ni, int nc) CountFunction(string function)
    {
        if (!function.StartsWith(FN_COUNT))
            throw new Exception($"The function {function} does not contains COUNT definition");

        function = function.Replace(FN_COUNT, "");
        function = function.Replace(":", "");

        // Values default
        if (string.IsNullOrEmpty(function))  return (1, 1);
        if (!function.Contains(',')) return (int.Parse(function), 1);

        var values = function.Split(',');

        return (int.Parse(values[0]), int.Parse(values[1]));
    }

    
    /// <summary>
    /// Function Maximum
    /// </summary>
    /// <param name="function">For Example: MAX:10</param>
    /// <returns>Maximum value</returns>
    /// <exception cref="Exception"></exception>
    private int MaxFunction(string function)
    {
        

        if (!function.StartsWith(FN_MAX))
            throw new Exception($"The function {function} does not contains max definition");

        function = function.Replace(FN_MAX, "");
        function = function.Replace(":", "");

        // Values default
        if (string.IsNullOrEmpty(function) || function.Contains(','))
            throw new Exception($"The function {function} does not contains min definition");

        return int.Parse(function);
    }
    
    /// <summary>
    /// Function Min
    /// </summary>
    /// <param name="function">For Example: MIN:10</param>
    /// <returns>Minimum value</returns>
    /// <exception cref="Exception"></exception>
    private int MinFunction(string function)
    {
        if (!function.StartsWith(FN_MIN))
            throw new Exception($"The function {function} does not contains min definition");

        function = function.Replace(FN_MIN, "");
        function = function.Replace(":", "");

        // Values default
        if (string.IsNullOrEmpty(function) || function.Contains(','))
            throw new Exception($"The function {function} does not contains min definition");

        return int.Parse(function);
    }
    
    /// <summary>
    /// Function Print
    /// </summary>
    /// <param name="function">For Example: PRINT:$VALUE</param>
    /// <returns>Printed data</returns>
    /// <exception cref="Exception"></exception>
    private string PrintFunction(string function)
    {
        if (!function.StartsWith(FN_PRINT))
            throw new Exception($"The function {function} does not contains print definition");

        function = function.Replace(FN_PRINT, "");
        function = function.Replace(":", "");

        return function;
        
    }
    
    /// <summary>
    /// Function Value
    /// </summary>
    /// <param name="function">For Example: VALUE:hello</param>
    /// <returns>Value data</returns>
    /// <exception cref="Exception"></exception>
    private string ValueFunction(string function)
    {
        if (!function.StartsWith(FN_VALUE))
            throw new Exception($"The function {function} does not contains value definition");

        function = function.Replace(FN_VALUE, "");
        function = function.Replace(":", "");

        return function;
        
    }

    /// <summary>
    /// Print value validating the data
    /// </summary>
    /// <returns>Value</returns>
    public string PrintValue()
    {
        if (state == State.End) return String.Empty;
        
        var ret = string.Empty;
        if (state == State.True)
            ret = Print;
        else
            ret = String.Empty;

        Counter();
        return ret;
    }

    private void Counter()
    {
        if (state == State.End) return;
        
        counter = counter + increment;
        if (counter > max)
        {
            state = State.End;
            return;
        }

        if (counter >= min && counter <= max)
        {
            state = State.True;
            return;
        }

        state = State.False;
    }
}