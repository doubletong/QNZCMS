using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Areas.ElFinder.Controllers
{
    [Area("ElFinder")]
    public class TinyMCEController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

     
        public IActionResult Browse()
        {
            return View();
        }
    }
}