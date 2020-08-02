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
    [ViewComponent(Name = "SocialApp")]
    public class SocialAppComponent : ViewComponent
    {
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public SocialAppComponent(YicaiyunContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            string keyNav = $"SOCIALAPPS_TRUE_ALL";

            if (!_cacheService.IsSet(keyNav))
            {
               // return View("Default", (List<Navigation>)_cacheService.Get(keyNav));
                var socialapps = await _context.SocialApps.Where(d => d.Active==true)
                    .OrderByDescending(d=>d.Importance).ThenBy(d=>d.Id).ToListAsync();

                _cacheService.Set(keyNav, socialapps, 30);
            }

            var vm = (List<SocialApp>)_cacheService.Get(keyNav);
     

            return View("Default", vm);
        }
    }
}
