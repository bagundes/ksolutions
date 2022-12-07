using KS.Shared;
using NUnit.Framework;

namespace KS.Test;

public class Shared_VariableTest
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Function()
    {
        var count0 = "COUNT";
        var key0 = KS.Shared.Variables.Add(count0);
        for (int i = 1; i < 11; i++)
            Assert.True(Shared.Variables.GetValue(key0).Equals(i.ToString()));
        
        var count1 = "COUNT:10";
        var key1 = KS.Shared.Variables.Add(count1);
        for (int i = 10; i < 20; i++)
            Assert.True(Shared.Variables.GetValue(key1).Equals(i.ToString()));
        
        var count2 = "COUNT:0,2";
        var key2 = KS.Shared.Variables.Add(count2);
        for (int i = 0; i < 11; i = i + 2)
            Assert.True(Shared.Variables.GetValue(key2).Equals(i.ToString()));
        
        var count3 = "COUNT:1,2";
        var key3 = KS.Shared.Variables.Add(count3);
        for (int i = 1; i < 11; i = i + 2)
            Assert.True(Shared.Variables.GetValue(key3).Equals(i.ToString()));
        
        
    }

    [Test]
    public void Variable()
    {
        var local0 = "{$VAR0:5|#,##0.00}";
        var local1 = "{$VAR1:TEST}";
        var global0 = "{@VAR0:5|#,##0.00}";
        var global1 = "{@VAR1:TEST}";
        
        Assert.True(Variables.IsVariable("SD ODSN OIFSAOI {@USER} SADA DSAD  {$TEST} ASDSA {%FOO}"));
        
    }
}