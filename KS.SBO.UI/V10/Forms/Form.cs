namespace KS.SBO.UI.V10.Forms;

public class Form
{
    /// <summary>
    /// Get activated form by Type name
    /// </summary>
    /// <param name="type"></param>
    /// <returns>Form</returns>
    public static SAPbouiCOM.Form? GetActivate(string type)
    {
        for (var i = 0; i < KSCon.Ui.Forms.Count; i++)
        {
            var form = KSCon.Ui.Forms.Item(i) as SAPbouiCOM.Form;

            var title = form.Title;
            var uniqueID = form.UniqueID;
            var foo = form.TypeEx;
            
            if (form.TypeEx.Equals(type))
                return form;
        }


        return null;
    }

    /// <summary>
    /// Close form opened. 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool Close(string type)
    {
        var form = GetActivate(type);
        if (form is not null)
        {
            form.Close();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Show or hide the form.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="visible"></param>
    /// <returns></returns>
    public static bool Visible(string type, bool visible)
    {
        var form = GetActivate(type);
        if (form is not null)
        {
            form.Visible = visible;
            return true;
        }
        else
        {
            return false;
        }
    }
}