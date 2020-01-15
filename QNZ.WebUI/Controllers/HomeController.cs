using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using QNZCMS.Models;
using SIG.Infrastructure.Cache;
using SIG.Infrastructure.Configs;

namespace QNZCMS.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ICacheService _cache;
        public HomeController(IWebHostEnvironment hostingEnvironment, ICacheService cache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
        }
    

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ChacheShow()
        {
            _cache.Set("test", "this is test", 15);

            return Content(_cache.IsSet("MENUS_CATEGORY_1").ToString());
        }
        public IActionResult ChacheShow1()
        {
            _cache.Invalidate("MENU");

            return Content(_cache.IsSet("MENUS_CATEGORY_1").ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Upload()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> UploadSmallFile(IFormFile file)
        //{
        //    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
        //    fileName = fileName.Contains("\\")
        //       ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
        //       : fileName.Trim('"');
        //    // full path to file in temp location
        //    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, SettingsManager.File.RootDirectory, fileName);
        //    //   var filePath = Path.GetTempFileName();
          
        //    if (file.Length > 0)
        //    {
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }

        //    // process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[DisableFormValueModelBinding]
        //public async Task<IActionResult> UploadStreamingFile()
        //{
        //    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
        //    //fileName = fileName.Contains("\\")
        //    //   ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
        //    //   : fileName.Trim('"');

        //    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, SettingsManager.File.RootDirectory,"001.png");
        //    // full path to file in temp location
        // //  var filePath = Path.GetTempFileName();

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await Request.StreamFile(stream);
        //    }

        //    // process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok();
        //}
    }
}
