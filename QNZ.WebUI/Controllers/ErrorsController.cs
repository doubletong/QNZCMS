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

        // GET: /<controller>/
        public IActionResult InternalServerError()
        {
            return View();
        }
        [Route("errors/404")]
        public IActionResult StatusCode404()
        {
            return View();
        }
        [Route("errors/400")]
        public IActionResult StatusCode400()
        {
            return View();
        }
    }
}