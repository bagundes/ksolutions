namespace KS.Test.SBO.UI;

public class Client
{
    [SetUp]
    public void Setup()
    {
        KSCon.Connect();
    }

    [Test]
    public void Log()
    {
        //var form = KSUi.Forms.Form.GetActivateForm("683");
        var log = KSUi.Client.Logs.GetSystemMessageLogData();
        Assert.Pass();
    }
}