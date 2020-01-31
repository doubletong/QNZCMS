using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Controllers
{
    public class DropzoneController : Controller
    {
        private IWebHostEnvironment _environment;

        public DropzoneController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadVM vm)
        {
            var fileurl = vm.filePath;
            try
            {
                if (vm.file.Length > 0)
                {
                    string folderRoot = Path.Combine(_environment.WebRootPath, "Uploads");
                    string filePath = Guid.NewGuid() + Path.GetExtension(vm.file.FileName);
                    filePath = Path.Combine(folderRoot, filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.file.CopyToAsync(stream);
                    }
                }
                return Ok(new { success = true, message = "File Uploaded" + fileurl });
            }
            catch (Exception er)
            {
                return BadRequest(new { success = false, message = "Error file failed to upload" + er.Message});
            }
        }
    }

    public class UploadVM {
        public IFormFile file { get; set; }
        public string filePath { get; set; }
    }

}