using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Controllers
{
    [Route("tiny-mce")]
    public class TinyMCEController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("browse")]
        public IActionResult Browse()
        {
            return View();
        }
    }
}