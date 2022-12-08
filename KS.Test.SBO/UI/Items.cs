namespace KS.Test.SBO.UI;

public class Items
{
    [SetUp]
    public void Setup()
    {
        KSCon.Connect();
    }

    [Test]
    public void Item()
    {
        var form = KSUi.Forms.Form.GetActivate("683");
        form = KSUi.Forms.Form.GetActivate("10000020L");
        Assert.Pass();
    }
}