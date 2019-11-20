using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        public IActionResult Index()
        {
            //var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            ////ViewBag.StatusCode = statusCode;
            //ViewBag.OriginalPath = feature?.OriginalPath;
            //ViewBag.OriginalQueryString = feature?.OriginalQueryString;

            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult test()
        {
            return View();
        }
    }
}