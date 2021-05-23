using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Areas.ElFinder.Controllers
{
    [Area("ElFinder")]
    public class FileManagerController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}