namespace KS.Core.Base;

public class InitBase
{
    private static Dictionary<string, IInit> Inits = new Dictionary<string, IInit>();
    
    public static void Load<T>(T init) where T : IInit
    {
        var key = init.GetType().Namespace;
        if(key != null && Inits.ContainsKey(key)) return;

        try
        {
            init.Dependencies();
            init.Configure();
            init.Start();
            
            Inits.Add(key, init);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        
    }

    public static void End() => throw new NotImplementedException();

    public static void Error()
    {
        
    }

}