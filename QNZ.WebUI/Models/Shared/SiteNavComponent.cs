using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "SiteNav")]
    public class SiteNavComponent : ViewComponent
    {
        private readonly YicaiyunContext _context;
        public SiteNavComponent(YicaiyunContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            var items = await _context.Navigations.AsNoTracking().Where(d => d.CategoryId== categoryId)
                .OrderBy(d => d.Importance).ToListAsync();
            return View("Default", items);
        }
    }
}
