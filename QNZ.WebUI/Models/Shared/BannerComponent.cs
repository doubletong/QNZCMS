using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "Banner")]
    public class BannerComponent : ViewComponent
    {
        private readonly QNZContext _context;
        private readonly ICacheService _cacheService;
        public BannerComponent(QNZContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string code)
        {
            string keyNav = $"ADVERTISEMENTS_TRUE_ALL";

            if (!_cacheService.IsSet(keyNav))
            {              
                var adverts = await _context.Advertisements.Include(d=>d.Space).Where(d => d.Active)
                    .OrderByDescending(d => d.Importance).ToListAsync();
                _cacheService.Set(keyNav, adverts, 30);
            }

            var advertList = (List<Advertisement>)_cacheService.Get(keyNav);
            var advert = advertList.FirstOrDefault(d => d.Space.Code == code);
                
            return View("Default", advert);
                   
                       
        }
    }
}
