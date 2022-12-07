using FluentFTP;

namespace KS.Network.Ftp;

public interface INavigate : IDisposable
{
    string Exception { get; }
    bool Connect();
    bool Upload(string path);
}


public class Navigate : INavigate
{
    private V1.Models.ConnectionBaseModel _conn;
    private FtpClient _client;
    
    /// <summary>
    /// In case any operation returns false, the exception will be saved here. 
    /// </summary>
    public string Exception { get; internal set; }
    
    public Navigate(V1.Models.ConnectionBaseModel conn)
    {
        _conn = conn;
    }

    public bool Connect()
    {
        _client = new FtpClient(_conn.Address, _conn.UserName, _conn.Password);
        _client.AutoConnect();

        return _client.IsConnected;
    }

    /// <summary>
    /// Upload the file.
    /// </summary>
    /// <param name="path">Full directory or file path. The files will be overwrite on the server</param>
    /// <returns>File uploaded</returns>
    public bool Upload(string path)
    {
        if (System.IO.File.Exists(path))
            return UploadFile(path, true);
        else if (System.IO.Directory.Exists(path))
            return UploadDirectory(path, true);
        else
        {
            Exception = "Does not possible define the path is directory or file";
            return false;
        }
    }

    private bool UploadDirectory(string path, bool overwrite)
    {
        // @todo - https://github.com/robinrodricks/FluentFTP/wiki/Quick-Start-Example
        throw new NotImplementedException();
    }

    private bool UploadFile(string file, bool overwrite)
    {
        try
        {
            if (!System.IO.File.Exists(file))
            {
                Exception = "The file does not exist";
                return false;
            }

            var fileInfo = new FileInfo(file);

            _client.UploadFile(fileInfo.FullName, fileInfo.Name);

            return true;
        }
        catch (Exception e)
        {
            Exception = e.Message;
            return false;
        }
        
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}