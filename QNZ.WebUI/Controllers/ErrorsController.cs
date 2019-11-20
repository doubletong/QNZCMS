using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}