namespace KS.Core;

public class Init : Base.IInit
{
    
    public static (string? company, string? @namespace, string? product, string? version) Product { get; internal set; }
    
    public void Dependencies()
    {
        Product = V01.Console.Environment.GetProductDetails(this.GetType());
    }

    public void Configure()
    {
        // throw new NotImplementedException();
    }

    public void Start()
    {
        // throw new NotImplementedException();
    }

    public void Stop()
    {
        // throw new NotImplementedException();
    }

    public void Break()
    {
        // throw new NotImplementedException();
    }
}