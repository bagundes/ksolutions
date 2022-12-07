using System.Text.RegularExpressions;
using NCrontab;

namespace KS.Core.V01;

public class Dynamic
{
    /// <summary>
    /// Convert Object to Json
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="crypt"></param>
    /// <returns></returns>
    public static String ObjectToJson(object obj, bool crypt = false)
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

        if (crypt)
            json = Security.Crypt.Encrypt(json);
        
        return json;
    }
    
    /// <summary>
    /// Convert Json to object
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? JsonToObject<T>(string data)
    {
        var json = Security.Crypt.Decrypt(data);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    /// Check if value is a number. The method needs to be improved
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumber(object value)
    {
        if (value is int) return true;
        if (value is double) return true;
        if (value is decimal) return true;
        
        Regex r = new Regex(@"(\d+)");
        return r.Match(value.ToString()).Success;
    }

    /// <summary>
    /// Return true when occurrence is compatible date time now.
    /// </summary>
    /// <param name="cron"></param>
    /// <returns></returns>
    public bool CheckCron(string cron)
    {
        if (cron.Equals("* * * * *")) return true;
        
        var schedule = CrontabSchedule.Parse(cron);
        var date = schedule.GetNextOccurrence(DateTime.Now);
        var mask = "yyMMddhhmm";
        
        return date.ToString(mask).Equals(DateTime.Now.ToString(mask));
    }
}