using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class FileManagerController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
      
    }
}