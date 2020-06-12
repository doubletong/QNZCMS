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
    [ViewComponent(Name = "Breadcrumb")]
    public class BreadcrumbComponent : ViewComponent
    {
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public BreadcrumbComponent(YicaiyunContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            string keyNav = $"NAVIGATIONS_{categoryId}_TRUE_LIST";

            if (!_cacheService.IsSet(keyNav))
            {
               // return View("Default", (List<Navigation>)_cacheService.Get(keyNav));
                var navigations = await _context.Navigations.Where(d => d.Active).OrderByDescending(d=>d.Importance).ToListAsync();
                _cacheService.Set(keyNav, navigations, 30);
            }

            var navList = (List<Navigation>)_cacheService.Get(keyNav);

            var url = Request.Path.ToString();
            //  var navlist = await _context.Navigations.Where(d => d.CategoryId == 1 && d.Active).ToListAsync();
            var currentNav = navList.FirstOrDefault(d=> d.CategoryId == categoryId && (d.Url == url || d.Url == (url + "/index")));

            var breadcrumb = new List<Navigation>();

            if (currentNav != null)
            {
                breadcrumb.Insert(0, currentNav);
                var parent = navList.FirstOrDefault(d=>d.Id == currentNav.ParentId);
                while (parent != null)
                {
                    breadcrumb.Insert(0, parent);
                    parent = parent.Parent;
                }
            }

            return View("Default", breadcrumb);
        }
    }
}
