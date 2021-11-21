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
    [Authorize(Policy = "Permission")]
    //[Authorize]
    public class HomeController : BaseController
    {
     
        private readonly QNZContext _context;

        public HomeController(QNZContext context)
        {
            _context = context;           

        }
        [Route("/qnz-admin")]
        [Route("/qnz-admin/home")]
        [Route("/qnz-admin/home/index")]
        public async Task<IActionResult> IndexAsync()
        {
            HomePageVM vm = new HomePageVM
            {
                ArticleCount = await _context.Articles.CountAsync(),
                ProductCount = await _context.Products.CountAsync(),
                ExhibitionCount = await _context.Exhibitions.CountAsync(),
                VideoCount = await _context.Videos.CountAsync(),
                PhotoCount = await _context.Photos.CountAsync(),
                JobCount = await _context.Jobs.CountAsync(),
                DocumentCount = await _context.Documents.CountAsync(),
                BranchCount = await _context.Branches.CountAsync(),
                PageCount = await _context.Pages.CountAsync(),
                MemoCount = await _context.Memorabilia.CountAsync(),
                StaffCount = await _context.Staffs.CountAsync()
            };
           
        


            return View(vm);
        }
        public async Task<IActionResult> PluginsAsync()
        {
            var vm = new PluginsPageVM
            {
                NavCount = await _context.Navigations.CountAsync(),
                AdvertCount = await _context.Advertisements.CountAsync(),
                SocialCount = await _context.SocialApps.CountAsync()
          
            };
            return View(vm);
        }

    }
}