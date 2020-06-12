using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using QNZCMS.Utilities;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;

namespace QNZCMS.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class FilesController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
     

        private readonly string _rootDirectory;
        // Get the default form options so that we can use them to set the default 
        // limits for request body data.
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public FilesController( IWebHostEnvironment hostingEnvironment, IConfiguration config)
        {
          
            _rootDirectory = config.GetValue<string>("QNZFinder:RootDirectory");
            _hostingEnvironment = hostingEnvironment;
         
        }

   

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
           
            string filename = ImageHandler.GetRandomFileName(Path.GetExtension(file.FileName), 10000);
            string relativeloc = _rootDirectory + "/"+filename;
            string webRootPath = _hostingEnvironment.WebRootPath;
       
            string newPath = Path.Combine(webRootPath + _rootDirectory.Replace('/','\\'), filename);

            if (file != null && file.Length > 0 && file.ContentType.Contains("image"))
            {


                try
                {
                    //path = Path.Combine(HttpContext.Server.MapPath(saveloc), Path.GetFileName(filename));
                    //file.SaveAs(path);
                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return Json(new { location = relativeloc });
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Upload Failed: " + e.Message);
                 
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Upload Failed: 错误: 只接受图片格式文件！");
            
            }


        }


       
        [HttpPost]
        public async Task<IActionResult> UploadFiles(string filePath, List<IFormFile> files)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
           // var filePath = Path.GetTempFileName();
            foreach (var iFormFile in files)
            {
                if (iFormFile.Length > 0)
                {
                    string filename = ImageHandler.GetRandomFileName(Path.GetExtension(iFormFile.FileName), 10000);
                    string saveFilePath = Path.Combine(webRootPath, filePath.Replace('/','\\'), filename);

                    using (var stream = new FileStream(saveFilePath, FileMode.Create))
                    {
                        await iFormFile.CopyToAsync(stream);
                    }
                }
            }
            //return RedirectToAction("Home");
            return StatusCode(StatusCodes.Status200OK, "Upload Success！ ");
        }

        public async Task<IActionResult> DownloadFile(string path, string filename)
        {
            if (filename == null || filename.Length == 0)
                return Content("No file selected for download.");
            var filePath = Path.Combine(path, filename);
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream
            (filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "text/plain", Path.GetFileName(path));
        }
    }
}