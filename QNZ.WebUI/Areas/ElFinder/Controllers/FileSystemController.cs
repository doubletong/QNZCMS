﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Areas.ElFinder.Controllers
{
    [Route("el-finder/file-system")]
    public class FileSystemController : Controller
    {
        [Route("connector")]
        public async Task<IActionResult> Connector()
        {
            var connector = GetConnector();
            return await connector.ProcessAsync(Request);
        }

        [Route("thumb/{hash}")]
        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = GetConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }

        private Connector GetConnector()
        {
            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            var root = new RootVolume(
                Startup.MapPath("~/Uploads"),
                $"{uri.Scheme}://{uri.Authority}/Uploads/",
                $"{uri.Scheme}://{uri.Authority}/el-finder/file-system/thumb/")
            {
                //IsReadOnly = !User.IsInRole("Administrators")
                IsReadOnly = false, // Can be readonly according to user's membership permission
                IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
                Alias = "Uploads", // Beautiful name given to the root/home folder
                //MaxUploadSizeInKb = 2048, // Limit imposed to user uploaded file <= 2048 KB
                //LockedFolders = new List<string>(new string[] { "Folder1" })
                ThumbnailSize = 120,

            };

            driver.AddRoot(root);

            return new Connector(driver)
            {
                // This allows support for the "onlyMimes" option on the client.
                MimeDetect = MimeDetectOption.Internal
            };
        }
    }
}
