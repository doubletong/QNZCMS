using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIG.Infrastructure.Configs
{
    public class ElfinderConfig
    {
      
        public static string WebRootPath { get; private set; }

        public static string MapPath(string path, string basePath = null)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = WebRootPath;
            }

            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(basePath, path);
        }
      
    }
}
