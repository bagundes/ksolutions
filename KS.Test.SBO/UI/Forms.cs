namespace KS.Test.SBO.UI;

public class Forms
{
    [SetUp]
    public void Setup()
    {
        KSCon.Connect();
    }

    [Test]
    public void Form()
    {
        var form = KSUi.Forms.Form.GetActivate("683");
        form = KSUi.Forms.Form.GetActivate("10000020L");
        Assert.Pass();
    }
}