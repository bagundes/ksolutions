using System.Management;
using System.Drawing;
namespace KS.Windows.V1;

public class Screen
{

    public static string Print()
    {
        var size = Size();
        return Print(size.width, size.height);
    }
    
    /// <summary>
    /// Create a print screen
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns>Image</returns>
    public static string Print(int width, int height)
    {
        using var bitmap = new Bitmap(width, height);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(0, 0, 0, 0,
                bitmap.Size, CopyPixelOperation.SourceCopy);
        }

        var file = KCore.Console.File.CreateTempFile("png");
        
        bitmap.Save(file, System.Drawing.Imaging.ImageFormat.Png);

        return file;
    }

    public static (int width, int height) Size()
    {
        int width = 0, height = 0;
        //create a management scope object
        ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");

        //create object query
        ObjectQuery query = 
            new ObjectQuery("SELECT * FROM Win32_VideoController " 
                            + "Where DeviceID=\"VideoController1\"");

        //create object searcher
        ManagementObjectSearcher searcher =
            new ManagementObjectSearcher(scope, query);

        //get a collection of WMI objects
        ManagementObjectCollection queryCollection = 
            searcher.Get();

        //enumerate the collection.
        foreach (ManagementObject m in queryCollection)
        {
            
            // access properties of the WMI object
            width = Convert.ToInt32(m["CurrentVerticalResolution"]);
            height = Convert.ToInt32(m["CurrentVerticalResolution"]);
            
            Console.WriteLine("CurrentHorizontalResolution : {0}", 
                m["CurrentHorizontalResolution"]);
            Console.WriteLine("CurrentVerticalResolution : {0}", 
                m["CurrentVerticalResolution"]);
        }

        return (width, height);
    }
}