using KS.Core.Lists;

namespace KS.Core.V01.Console;

public static class Directory
{
    /// <summary>
    /// check if directory exists
    /// </summary>
    /// <param name="path">Path</param>
    /// <param name="formated">If path contains variable, the system will replace it.</param>
    /// <returns>Directory exist</returns>
    public static bool Exists(string path, out string formated)
    {
        var variable = Constants.ROOT_PATH.GetType().Name;

        if (path.Equals(variable))
        {
            formated = Constants.ROOT_PATH;
            return true;
        }
        else
        {
            if (System.IO.Directory.Exists(path))
            {
                formated = (new System.IO.DirectoryInfo(path).FullName);
                return true;
            }
        }

        formated = null;
        return false;
    }

    /// <summary>
    /// Get content directory.
    /// </summary>
    /// <returns></returns>
    public static string Content => System.IO.Path.Combine(Path.GetDirectoryName(typeof(Directory).Assembly.Location), "content");
}