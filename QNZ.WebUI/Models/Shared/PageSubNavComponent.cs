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
    [ViewComponent(Name = "PageSubNav")]
    public class PageSubNavComponent : ViewComponent
    {
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public PageSubNavComponent(YicaiyunContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int categoryId, string title)
        {
            string keyNav = $"NAVIGATIONS_{categoryId}_TRUE_{title}";

            if (_cacheService.IsSet(keyNav))
            {        
                return View("Default", (List<Navigation>)_cacheService.Get(keyNav));
            }
            else
            {
                var items = await _context.Navigations          
                  .Where(d => d.CategoryId == categoryId && d.Parent.Title == title && d.Active)
                  .OrderBy(d => d.Importance).ToListAsync();

                _cacheService.Set(keyNav, items,30);

                return View("Default", items);
            }        
                       
        }
    }
}
