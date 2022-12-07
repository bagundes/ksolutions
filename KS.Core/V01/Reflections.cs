using System.Diagnostics;

namespace KS.Core.V01;

public class Reflections
{
    #region Stack
    public class Stack
    {
        public string @namespace;

        public string @class;

        public string method;

        public Stack(string @namespace, string @class, string method)
        {
            this.@namespace = @namespace;
            this.@class = @class;
            this.method = method;
        }

        public override string ToString()
        {
            return $"{@namespace} -> {@class}.{method}";
        }
    }

    /// <summary>
    /// Stack trace class called
    /// </summary>
    /// <param name="call"></param>
    /// <returns></returns>
    public static Stack GetStackTraceClass(int call = 0)
    {
        try
        {
            StackTrace st = new StackTrace(fNeedFileInfo: false);
            StackFrame sf = st.GetFrame(call + 1);
            string nspace = sf.GetMethod().DeclaringType.Namespace;
            string @class = sf.GetMethod().DeclaringType.Name;
            string method = sf.GetMethod().Name;
            return new Stack(nspace, @class, method);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion
}