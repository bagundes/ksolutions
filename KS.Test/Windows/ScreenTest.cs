
using NUnit.Framework;

namespace KS.Test.Windows;

public class ScreenTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void PrintScreen()
    {
        var file = KS.Windows.V1.Screen.Print();
        
        Assert.True(System.IO.File.Exists(file));
        Assert.Pass();
    }
    
    [Test]
    public void ScreenResolution()
    {
        var res = KS.Windows.V1.Screen.Size();
        Assert.True(res.width > 0);
        Assert.True(res.height > 0);
        
        Assert.Pass();
    }
}