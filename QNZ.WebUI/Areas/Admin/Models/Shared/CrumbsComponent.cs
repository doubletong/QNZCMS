using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Infrastructure.Configs;

namespace QNZCMS.Areas.Admin.Models.Shared
{
    [ViewComponent(Name = "Crumbs")]
    public class CrumbsComponent : ViewComponent
    {
        private IMemoryCache _cache;
        private readonly QNZContext _context;
        public CrumbsComponent(QNZContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

      
        public async Task<IViewComponentResult> InvokeAsync(int categoryId, ViewContext viewContext)
        {
            return View("Crumbs", await CurrenMenuCrumbsAsync(categoryId, viewContext));
        }


        public async Task<List<QNZ.Data.Menu>> CurrenMenuCrumbsAsync(int categoryId, ViewContext viewContext)
        {
            var controller = viewContext.RouteData.Values["controller"].ToString();
            var action = viewContext.RouteData.Values["action"].ToString();

            var area = string.Empty;
            object areaObj;
            if (viewContext.RouteData.Values.TryGetValue("area", out areaObj))
            {
                area = areaObj.ToString();
            }
            // string area = Site.CurrentArea(), controller = Site.CurrentController(), action = Site.CurrentAction();
            //string actionName = ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //var aa= ControllerContext.ActionDescriptor.ActionName

            var rource = await GetShowMenusAsync(categoryId);
            List<QNZ.Data.Menu> menus = new List<QNZ.Data.Menu>();

            QNZ.Data.Menu vMenu = rource.FirstOrDefault(m => area.Equals(m.Area, StringComparison.OrdinalIgnoreCase)
            && controller.Equals(m.Controller, StringComparison.OrdinalIgnoreCase)
            && action.Equals(m.Action, StringComparison.OrdinalIgnoreCase));

            if (vMenu != null)
                await RecursiveLoadAsync(vMenu, menus);

            return menus;
        }

        /// <summary>
        /// 递归获取父项
        /// </summary>
        /// <param name="vMenu"></param>
        /// <param name="Parents"></param>
        private async Task RecursiveLoadAsync(QNZ.Data.Menu vMenu, List<QNZ.Data.Menu> Parents)
        {
            Parents.Insert(0, vMenu);
            if (vMenu.ParentId != null)
            {
                var rource = await GetShowMenusAsync(vMenu.CategoryId);
                QNZ.Data.Menu parentMenu = rource.FirstOrDefault(m => m.Id == vMenu.ParentId);
                if (parentMenu != null)
                {
                    await RecursiveLoadAsync(parentMenu, Parents);
                }
                   
            }
        }

        public async Task<List<QNZ.Data.Menu>> GetShowMenusAsync(int categoryId)
        {
            var cacheKey = $"AllMenus_{categoryId}";
            // Look for cache key.
            if (!_cache.TryGetValue(cacheKey, out List<QNZ.Data.Menu> menus))
            {
                // Key not in cache, so get data.
                menus = await _context.Menus.AsNoTracking().Where(d => d.CategoryId == categoryId).ToListAsync();
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                // Save data in cache.
                _cache.Set(cacheKey, menus, cacheEntryOptions);
            }

            menus = menus.Where(m => m.MenuType == (short)MenuType.NOLINK ||
                m.MenuType == (short)MenuType.PAGE).OrderBy(m => m.Importance).ToList();      

            return menus;

        }

    }
}
