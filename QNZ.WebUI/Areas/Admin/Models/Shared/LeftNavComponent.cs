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
using QNZ.Model.ViewModel;
using SIG.Infrastructure.Cache;
using SIG.Infrastructure.Configs;

namespace QNZCMS.Areas.Admin.Models.Shared
{
    [ViewComponent(Name = "LeftNav")]
    public class LeftNavComponent : ViewComponent
    {

        private ICacheService _cache;
        private readonly YicaiyunContext _context;
        public LeftNavComponent(YicaiyunContext context, ICacheService memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId, ViewContext viewContext)
        {

            //if (!User.IsInRole("系统管理员"))
            //{
            //    if (User.IsInRole("店员"))
            //    {
            //        var cacheKey1 = $"AllMenus_6_{categoryId}";
            //        // Look for cache key.
            //        if (!_cache.TryGetValue(cacheKey1, out List<QNZ.Data.Menu> menus1))
            //        {
            //            // 6是店员角色ID
            //            menus1 = await _context.Menus.AsNoTracking().Where(d => d.CategoryId == categoryId && d.RoleMenus.Any(r => r.RoleId == 6)).ToListAsync();
            //            // Set cache options.
            //            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
            //            // Save data in cache.
            //            _cache.Set(cacheKey1, menus1, cacheEntryOptions);
            //        }
            //        LeftNavVM vm1 = new LeftNavVM
            //        {
            //            Menus = menus1,  //_menuServices.GetLeftMenus(categoryId),//_menuServices.GetShowMenus(categoryId),
            //            CurrentMenu = GetCurrenMenu(categoryId, viewContext, menus1)
            //        };
            //        return View(vm1);
            //    }
            //}
            LeftNavVM vm = new LeftNavVM();


            var cacheKey = $"MENUS_CATEGORY_{categoryId}";
            if (_cache.IsSet(cacheKey))
            {
                var menus = (List<QNZ.Data.Menu>)_cache.Get(cacheKey);
                vm.Menus = menus;
                vm.CurrentMenu = GetCurrenMenu(categoryId, viewContext, menus);         
            }
            else
            {
               var menus = await _context.Menus.AsNoTracking().Where(d => d.CategoryId == categoryId).ToListAsync();
                _cache.Set(cacheKey, menus, SettingsManager.Site.CacheDuration);

                vm.Menus = menus;
                vm.CurrentMenu = GetCurrenMenu(categoryId, viewContext, menus);               

            }
            // Look for cache key.
            //if (!_cache.TryGetValue(cacheKey, out List<QNZ.Data.Menu> menus))
            //{
            //    // Key not in cache, so get data.
            //    menus = await _context.Menus.AsNoTracking().Where(d => d.CategoryId == categoryId).ToListAsync();
            //    // Set cache options.
            //    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
            //    // Save data in cache.
            //    _cache.Set(cacheKey, menus, cacheEntryOptions);
            //}

            return View(vm);

        }


        public QNZ.Data.Menu GetCurrenMenu(int categoryId, ViewContext viewContext, List<QNZ.Data.Menu> menus)
        {
         
            var controller = viewContext.RouteData.Values["controller"].ToString();
            var action = viewContext.RouteData.Values["action"].ToString();

            var area = string.Empty;
            object areaObj;
            if (viewContext.RouteData.Values.TryGetValue("area", out areaObj))
            {
                area = areaObj.ToString();
            }
            //string area = Site.CurrentArea(), controller = Site.CurrentController(), action = Site.CurrentAction();
            // var menus = await _context.Menus.Where(m => m.CategoryId == categoryId).ToListAsync();

            QNZ.Data.Menu vMenu = menus?.FirstOrDefault(m => area.Equals(m.Area, StringComparison.OrdinalIgnoreCase)
                                                                         && controller.Equals(m.Controller, StringComparison.OrdinalIgnoreCase)
                                                                         && action.Equals(m.Action, StringComparison.OrdinalIgnoreCase));


            if (vMenu == null)
                return null;

            if (vMenu.Active || vMenu.MenuType == (short)MenuType.PAGE)
                return vMenu;

            return RecursiveLoadMenu(vMenu.ParentId, menus);


        }


        private QNZ.Data.Menu RecursiveLoadMenu(int? parentId, List<QNZ.Data.Menu> menus)
        {
            // var menus = GetAllMenusByCategoryId(SettingsManager.Menu.BackMenuCId);

            QNZ.Data.Menu vMenu = menus.FirstOrDefault(m => m.ParentId == parentId && m.MenuType == (short)MenuType.PAGE);

            if (vMenu.Parent != null && (vMenu.Parent.MenuType != (short)MenuType.PAGE || !vMenu.Parent.Active))
            {
                return RecursiveLoadMenu(vMenu.ParentId, menus);
            }
            return vMenu.Parent;
        }

    }
}
