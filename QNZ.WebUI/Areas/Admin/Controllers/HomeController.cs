using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Admin.ViewModel;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]  
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    //[Authorize]
    public class HomeController : BaseController
    {
     
        private readonly YicaiyunContext _context;

        public HomeController(YicaiyunContext context)
        {
            _context = context;
           

        }
        [Route("/admin")]
        [Route("/admin/home")]
        [Route("/admin/home/index")]
        public IActionResult Index()
        {
            HomePageVM vm = new HomePageVM
            {
                //FeedbackCount = await _context.Feedbacks.CountAsync(),
                //FeedbackTodayCount = await _context.Feedbacks.CountAsync(d => d.CreatedDate >= DateTime.Today && d.CreatedDate < DateTime.Today.AddDays(1)),
                //CustomerGZHCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.GongZongHao),
                //CustomerGZHTodayCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.GongZongHao && d.CreatedDate >= DateTime.Today && d.CreatedDate < DateTime.Today.AddDays(1)),
                //CustomerXCXCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.XiaoChengXu),
                //CustomerXCXTodayCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.XiaoChengXu && d.CreatedDate >= DateTime.Today && d.CreatedDate < DateTime.Today.AddDays(1)),
                //TodayRevenue = await _context.Orders.Where(d => d.CreatedDate >= DateTime.Today && d.CreatedDate < DateTime.Today.AddDays(1) && !d.Cancelled).SumAsync(d=>d.Amount),
                //TotalRevenue = await _context.Orders.Where(d=>!d.Cancelled).SumAsync(d => d.Amount)

            };
            return View(vm);
        }
        public IActionResult Report()
        {           
            return View();
        }

    }
}