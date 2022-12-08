namespace KS.SBO.UI.V10.Forms;

public static class Items
{
    /// <summary>
    /// Get the ui control from form
    /// </summary>
    /// <param name="uniqueId">Unique ID</param>
    /// <param name="form">Form to get the ui control</param>
    /// <typeparam name="T">Type of control</typeparam>
    /// <returns>Control</returns>
    public static T GetUiControl<T>(string uniqueId, ref SAPbouiCOM.Form form)
    {
        var item = (T)form.Items.Item(uniqueId).Specific;
        return item;
    }
}