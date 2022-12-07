namespace KS.Test;
using NUnit.Framework;

public class FoxproTest
{
    private string frx;
    
    [SetUp]
    public void Setup()
    {
        frx = "/home/bfagundes/Files/Projects/Kurumin/Libraries/KuruminSolutions/KS/KS.Test/content/B_INDE04.dbf";
    }

    [Test]
    public void Connection()
    {
        using var conn = new KS.DB.Foxpro.V1.Connection(frx);

        var row = conn.Query("SELECT * FROM \"B_INDE04.DBF\"");
        
        
        Assert.Pass();
    }
}