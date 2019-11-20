using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIG.Data.Entity;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class CommonController : Controller
    {
        private readonly YicaiyunContext _context;

        public CommonController(YicaiyunContext context)
        {
            _context = context;
        }

        // GET: Admin/Common
        public async Task<IActionResult> CitiesByProvince(string province)
        {
            return PartialView("_CitySelectItems",await _context.Cities.Where(d=>d.Province.Name == province).ToListAsync());
        }

        public async Task<IActionResult> DistrictsByCity(string city)
        {
            return PartialView("_DistrictSelectItems", await _context.Districts.Where(d => d.City.Name == city).ToListAsync());
        }
    }
}
