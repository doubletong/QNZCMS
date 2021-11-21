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
    [ViewComponent(Name = "SiteNavMobile")]
    public class SiteNavMobileComponent : ViewComponent
    {
        private readonly QNZContext _context;
        private readonly ICacheService _cacheService;
        public SiteNavMobileComponent(QNZContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            string keyNav = $"NAVIGATIONS_{categoryId}_TRUE_HIERARCHY";

            if (_cacheService.IsSet(keyNav))
            {
                return View("Default", (List<Navigation>)_cacheService.Get(keyNav));
            }
            else
            {
                var items = await _context.Navigations.Include(d => d.InverseParent)
                  .ThenInclude(sub => sub.InverseParent)
                  .Where(d => d.CategoryId == categoryId)
                  .Where(d => d.ParentId == null)
                  .Where(d => d.Active == true)
                  .OrderBy(d => d.Importance).ToListAsync();

                _cacheService.Set(keyNav, items,30);

                return View("Default", items);
            }        
                       
        }
    }
}
