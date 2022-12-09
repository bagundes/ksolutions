using SAPbobsCOM;

namespace KS.SBO.DI.V10;


/// <summary>
/// Create a sap message.
/// Ref: https://blogs.sap.com/2021/08/24/how-to-pass-alert-message-through-di-api-with-linked-buttons/
/// </summary>
public class Message
{
    private SAPbobsCOM.Message _msg;
    private SAPbobsCOM.MessagesService _msgSrv;

    /// <summary>
    /// Send a internal message
    /// </summary>
    /// <param name="sender">OUSR.USERID</param>
    public Message(int sender)
    {
        var cmpSrv = KSCon.Di.GetCompanyService();
        _msgSrv = cmpSrv.GetBusinessService(ServiceTypes.MessagesService) as MessagesService;
        _msg = _msgSrv.GetDataInterface(MessagesServiceDataInterfaces.msdiMessage) as SAPbobsCOM.Message;
        _msg.User = sender;
        
    }

    /// <summary>
    /// Define Recepient
    /// </summary>
    /// <param name="userCode">OUSR.USER_CODE</param>
    /// <param name="sendInternal">Send internal message</param>
    /// <param name="sendEmail">Send email message</param>
    /// <returns>Recipient index</returns>
    public int Recipient(string userCode, bool sendInternal, bool sendEmail)
    {
        SAPbobsCOM.RecipientCollection recipientCollection = _msg.RecipientCollection;
        recipientCollection.Add();
        var index = recipientCollection.Count - 1;
        recipientCollection.Item(index).SendInternal = sendInternal ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
        recipientCollection.Item(index).SendEmail = sendEmail ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
        recipientCollection.Item(index).UserCode = userCode;

        return index;
    }

    /// <summary>
    /// Create data column. The parameters as column name and yellow arrow, it will be defined
    /// if the column does not exists.
    /// </summary>
    /// <param name="name">Column name - Ex: "BP"</param>
    /// <param name="objType">Object type- Ex: 2</param>
    /// <param name="objKey">Primary key - Ex: "C0001"</param>
    /// <param name="value">Field Value - Ex: "Customer: Max Ltd"</param>
    /// <param name="link">Create yellow arrow</param>
    public void DataColumn(string name, string objType, string objKey, string value = "", bool link = true )
    {
        MessageDataColumn dataColumn = null;
        var dataColumns = _msg.MessageDataColumns;
        
        #region Check column name exists
        for (int i = 0; i < dataColumns.Count; i++)
        {
            if (dataColumns.Item(i).ColumnName.Equals(name))
            {
                dataColumn = dataColumns.Item(i);
                break;
            }
        }
        
        #endregion

        if (dataColumn is null)
        {
            dataColumn = dataColumns.Add();
            dataColumn.ColumnName = name;
            dataColumn.Link = link ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
        }

        var line = dataColumn.MessageDataLines.Add();
        line.Value = objKey;
        line.Object = objType;
        line.ObjectKey = String.IsNullOrEmpty(value) ? objKey : value;
    }
    
    /// <summary>
    /// Send the message
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="message">Message to send</param>
    public void Send(string subject, string message)
    {
        _msg.Subject = subject;
        _msg.Text = message;
        
        _msgSrv.SendMessage(_msg);
    }


    /// <summary>
    /// Create a message inbox.
    /// </summary>
    /// <param name="userCode">OUSR.USER_CODE will receive the meassage</param>
    /// <param name="subject">Subject</param>
    /// <param name="message">Message</param>
    /// <param name="rows">Rows to create DataColumn. The row need to have the columns: Name,ObjType,ObjKey,Value </param>
    public static void MessageInbox(string userCode, string subject, string message, KDB.Models.RowModel[]? rows)
    {
        var msg = new Message(KSCon.Di.UserSignature);
        msg.Recipient(userCode, true, false);

        if (rows is not null)
        {
            foreach (var row in rows)
            {
                msg.DataColumn(
                    row["Name"].ToString(),
                    row["ObjType"].ToString(),
                    row["ObjKey"].ToString(),
                    row["Value"].ToString());
            }
        }
    }
}