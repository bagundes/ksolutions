namespace KS.SBO.V10;

public class Connection
{
    public static SAPbouiCOM.Application? Ui { get; internal set; }
    public static SAPbobsCOM.Company? Di { get; internal set; }

    /// <summary>
    /// Connect SAP B1 Client by UI and DI APIs
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Connect()
    {
        try
        {
            if (Ui is null)
            {
                var sboGuiApi = new SAPbouiCOM.SboGuiApi();
                string? identifier;
                #if DEBUG
                identifier = Lists.Developement.AddonIdentifier;
                #else
                identifier = Environment.GetCommandLineArgs().GetValue(1);
                #endif
              
                sboGuiApi.Connect(identifier);
                Ui = sboGuiApi.GetApplication();
                Di = (SAPbobsCOM.Company)Ui.Company.GetDICompany();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}