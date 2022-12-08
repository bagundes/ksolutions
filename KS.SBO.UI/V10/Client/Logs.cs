using KS.SBO.UI.V10.Lists;
using SAPbouiCOM;
using Items = KS.SBO.UI.V10.Forms.Items;

namespace KS.SBO.UI.V10.Client;

public static class Logs
{
    /// <summary>
    /// Get data saved in the matrix on System Messages Log
    /// </summary>
    /// <returns>Matrix serialized as xml</returns>
    public static string? GetSystemMessageLogData()
    {
        var form = KSUi.Forms.Form.GetActivate(TypeEx.SystemMessagesLogData);
        if (form is null)
        {
            Client.Menus.Click(Lists.MenusUid.Windows.SystemMessageLog.Uid);
            var data = GetSystemMessageLogData();
            Forms.Form.Close(TypeEx.SystemMessagesLogData);
            return data;
        }
            
            
        var mtx = Items.GetUiControl<SAPbouiCOM.Matrix>("3",ref form);
        var ret = mtx.SerializeAsXML(BoMatrixXmlSelect.mxs_All);

        return ret;
    }
}