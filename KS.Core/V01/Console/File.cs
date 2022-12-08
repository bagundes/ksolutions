namespace KS.Core.V01.Console;

public static class File
{
    #region Copy

    /// <summary>
    /// Copy the source to temporary folder
    /// </summary>
    /// <param name="file">Path</param>
    /// <returns>Temporary path</returns>
    public static string TempCopy(string file)
    {
        var tempFile = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(tempFile);
        System.IO.File.Copy(file, tempFile);

        return tempFile;
    }
    
    /// <summary>
    /// Create a temporary source.
    /// </summary>
    /// <param name="extension">Define extension source</param>
    /// <returns>Full temporary source path</returns>
    public static string CreateTempFile(string? extension)
    {
        var tempFile = System.IO.Path.GetTempFileName();
        
        if (!String.IsNullOrEmpty(extension))
        {
            System.IO.File.Delete(tempFile);
            if (!String.IsNullOrEmpty(extension))
            {
                tempFile = tempFile.Replace(".tmp", "");
                tempFile = $"{tempFile}.{extension}";
            }
        }

        return tempFile;
    }
    #endregion
    
    #region System
    
    /// <summary>
    /// Get contant source.
    /// </summary>
    /// <param name="name">Content source name</param>
    /// <param name="read">Read the source</param>
    /// <returns>Data source or path source</returns>
    /// <exception cref="KCoreException">1 - If the source does not exists.</exception>
    public static string ContentFile(string name, bool read )
    {
        var ret = FindFiles(V01.Console.Directory.Content, name);
        if (ret.Length == 0)
            throw new KCoreException(1, name);
        else
        {
            if (read)
                return System.IO.File.ReadAllText(ret[0]);
            else
                return ret[0];
        }
    }
    #endregion
    
    #region Search files
    /// <summary>
    /// Find files in the directory. Search application path
    /// </summary>
    /// <param name="searchPattern">search partner</param>
    /// <returns>Files full path</returns>
    public static string[] FindFiles(string searchPattern)
    {
        var path = System.Environment.CurrentDirectory;
        return FindFiles(path, searchPattern, System.IO.SearchOption.TopDirectoryOnly, new DateTime(), DateTime.Now.AddDays(1));
    }
        /// <summary>
        /// Find files in the directory
        /// </summary>
        /// <param name="path">Path to find</param>
        /// <param name="searchPattern">search partner</param>
        /// <returns>Files full path</returns>
        public static string[] FindFiles(string path, string searchPattern)
        {
            return FindFiles(path, searchPattern, System.IO.SearchOption.TopDirectoryOnly, new DateTime(), DateTime.Now.AddDays(1));
        }

        /// <summary>
        /// Find files in the directory
        /// </summary>
        /// <param name="path">Path to find</param>
        /// <param name="searchPattern">search partner</param>
        /// <param name="searchOption">Recursive directory</param>
        /// <returns>Files full path</returns>
        public static string[] FindFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            return FindFiles(path, searchPattern, searchOption, new DateTime(), DateTime.Now.AddDays(1));
        }

        
        /// <summary>
        /// Find files in the directory
        /// </summary>
        /// <param name="path">Path to find</param>
        /// <param name="searchPattern">search partner</param>
        /// <param name="searchOption">Recursive directory</param>
        /// <param name="startLastAccess">Last access between the dates</param>
        /// <param name="endLastAccess">Last access between the dates</param>
        /// <param name="byAccess">Check data by access</param>
        /// <returns>Files full path</returns>
        public static string[] FindFiles(string path, string searchPattern, System.IO.SearchOption searchOption, DateTime startLastAccess, DateTime endLastAccess, bool byAccess = true)
        {
            if (String.IsNullOrEmpty(searchPattern))
                return new string[0];

            List<string> res = new List<string>();

            var files = System.IO.Directory.GetFiles(path, searchPattern, searchOption);

            foreach (var file in files)
            {
                var foo = new FileInfo(file);
                if (byAccess)
                {
                    if (foo.LastAccessTime >= startLastAccess && foo.LastAccessTime <= endLastAccess)
                        res.Add(file);
                } else
                {
                    if (foo.CreationTime >= startLastAccess && foo.CreationTime <= endLastAccess)
                        res.Add(file);
                }
            }

            return res.ToArray();
        }

        public static string[] FindFiles(string path, string searchPattern, System.IO.SearchOption searchOption, DateTime access, bool startAccess, bool byAccess = true)
        {

            if (startAccess)
                return FindFiles(path, searchPattern, searchOption, access, DateTime.Now, byAccess);
            else
                return FindFiles(path, searchPattern, searchOption, DateTime.Now.AddYears(-100), access, byAccess);
        }
        #endregion
        
    #region Edit

    /// <summary>
    /// Move the file
    /// </summary>
    /// <param name="source">File source path</param>
    /// <param name="path">Directory to save</param>
    public static void Move(string source, string path)
    {
        var fileInfo = new FileInfo(source);
        System.IO.File.Move(source, System.IO.Path.Combine(path, fileInfo.Name));
    }
    #endregion
}