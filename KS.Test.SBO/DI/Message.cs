using Microsoft.VisualBasic;

namespace KS.Test.SBO.DI;

public class Message
{
    [SetUp]
    public void Setup()
    {
        KSCon.Connect();
    }

    [Test]
    public void InternalMessage()
    {
        var msg = new KSDi.Message(KSCon.Di.UserSignature);
        msg.Recipient("manager", true, true);
        msg.DataColumn("TEST", "2", "V70000", "Supplier: SMD Technologies", true);
        msg.Send($"{DateTime.Now}", "Message test");
            
        Assert.Pass();    
    }
}