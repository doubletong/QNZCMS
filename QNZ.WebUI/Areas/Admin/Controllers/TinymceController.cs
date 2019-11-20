using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class TinyMCEController : Controller
    {  

        [Route("tiny-mce/browse")]
        public IActionResult Browse()
        {
            return View();
        }
    }
}