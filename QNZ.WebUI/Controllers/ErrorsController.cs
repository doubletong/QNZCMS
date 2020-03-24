using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("errors/{code}")]
        public IActionResult InternalServerError(int code)
        {
            
            return View(code);
        }
      
    }
}