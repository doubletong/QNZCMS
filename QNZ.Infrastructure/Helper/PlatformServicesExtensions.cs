﻿using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Runtime.InteropServices;


namespace QNZ.Infrastructure.Helper
{
    public static class PlatformServicesExtensions
    {
        public static string MapPath(this PlatformServices services, string path)
        {
            var result = path ?? string.Empty;
            if (services.IsPathMapped(path) == false)
            {
                var wwwroot = services.WwwRoot();
                if (result.StartsWith("~", StringComparison.Ordinal))
                {
                    result = result.Substring(1);
                }
                if (result.StartsWith("/", StringComparison.Ordinal))
                {
                    result = result.Substring(1);
                }
                if(IsWindowRunTime())
                    result = Path.Combine(wwwroot, result.Replace('/', '\\'));
                else
                    result = Path.Combine(wwwroot, result.Replace('\\', '/'));
            }

            return result;
        }

        public static string UnmapPath(this PlatformServices services, string path)
        {
            var result = path ?? string.Empty;
            if (services.IsPathMapped(path))
            {
                var wwwroot = services.WwwRoot();
                result = result.Remove(0, wwwroot.Length);
                result = result.Replace('\\', '/');

                var prefix = (result.StartsWith("/", StringComparison.Ordinal) ? "~" : "~/");
                result = prefix + result;
            }

            return result;
        }
        
        private static bool IsWindowRunTime()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

   
       

        public static bool IsPathMapped(this PlatformServices services, string path)
        {
            var result = path ?? string.Empty;
            return result.StartsWith(services.Application.ApplicationBasePath,
                StringComparison.Ordinal);
        }

        public static string WwwRoot(this PlatformServices services)
        {
            // todo: take it from project.json!!!
            var result = Path.Combine(services.Application.ApplicationBasePath, "wwwroot");
            return result;
        }
    }
}
