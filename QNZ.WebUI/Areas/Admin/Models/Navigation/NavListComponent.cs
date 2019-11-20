using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QNZ.Data;

namespace SIG.SIGCMS.Areas.Admin.Models.Navigation
{
    [ViewComponent(Name = "NavList")]
    public class NavListComponent : ViewComponent
    {
        private IMemoryCache _cache;
        private readonly YicaiyunContext _context;
        public NavListComponent(YicaiyunContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
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

            var MenuTree = CreatedMenuList(menus.Where(m => m.ParentId == null), menus);

            return View("NavList", MenuTree);
            // return View("Menus", _menuServices.CurrenMenuCrumbs(SettingsManager.Menu.BackMenuCId, viewContext));
        }

        private string CreatedMenuList(IEnumerable<QNZ.Data.Menu> levelMenus, IEnumerable<QNZ.Data.Menu> menus, string menuTree = "", bool isExpand = true)
        {
            var exClassName = isExpand ? "" : "hidden";
            menuTree = menuTree + $"<ul class=\"menuTree {exClassName}\">";
            foreach (var item in levelMenus.OrderBy(d=>d.Importance))
            {
                item.InverseParent = menus.Where(m => m.ParentId == item.Id).OrderBy(c => c.Importance).ToList();
                var hasChild = item.InverseParent.Any();
                var hasMenusClassName = hasChild ? "hasMenus" : "";
                menuTree = menuTree + $"<li class=\"item-container {hasMenusClassName}\"><div>";
                if (hasChild)
                {
                    var url = Url.Action("IsExpand", new { id = item.Id });
                    if (item.IsExpand)
                    {

                        menuTree = menuTree + $"<a href = \"#\" class=\"expmenu expandmenu\" data-url=\"{url}\"><i class=\"iconfont icon-minus-square-fill\"></i></a>";
                    }
                    else
                    {
                        menuTree = menuTree + $"<a href = \"#\" class=\"expmenu\" data-url=\"{url}\"><span class=\"iconfont icon-plus-square-fill\"></span></a>";
                    }
                }
                menuTree = menuTree + item.Title;
                menuTree = menuTree + $"<a href = \"{Url.Action("UpDownMove", new { id = item.Id, isUp = true, categoryId = item.CategoryId })}\" title=\"向上\" data-ajax = \"true\"  " +
                           $"data-ajax-method = \"POST\" data-ajax-mode = \"replace\" data-ajax-update=\"#edit-container\" data-ajax-begin = \"onBegin\" " +
                           $"data-ajax-complete = \"onComplete\" data-ajax-failure = \"onFailed\" data-ajax-success = \"onSuccessSave\" ><i class=\"iconfont icon-up-circle\" data-icon=\"icon-up-circle\"></i></a>";
                menuTree = menuTree + $"<a href = \"{Url.Action("UpDownMove", new { id = item.Id, isUp = false, categoryId = item.CategoryId })}\" title=\"向下\" data-ajax = \"true\"  " +
                           $"data-ajax-method = \"POST\" data-ajax-mode = \"replace\" data-ajax-update=\"#edit-container\" data-ajax-begin = \"onBegin\" " +
                           $"data-ajax-complete = \"onComplete\" data-ajax-failure = \"onFailed\" data-ajax-success = \"onSuccessSave\" ><i class=\"iconfont icon-down-circle\" data-icon=\"icon-down-circle\"></i></a>";
                menuTree = menuTree +
                           $"<a href=\"{Url.Action("MoveMenu", "Navigation", new { id = item.Id })}\" data-ajax = \"true\" title = \"移动菜单\" " +
                           $"data-ajax-method = \"GET\" data-ajax-mode = \"replace\" data-ajax-update=\"#edit-container\" data-ajax-begin = \"onBegin\" " +
                           $"data-ajax-complete = \"onComplete\" data-ajax-failure = \"onFailed\" data-ajax-success = \"onSuccess\">" +
                           $"<i class=\"iconfont icon-right-circle\"></i></a>";

                menuTree = menuTree +
                           $"<a href=\"{Url.Action("CreateMenu", "Navigation", new { parentId = item.Id, categoryId = item.CategoryId })}\"  " +
                           $" data-ajax = \"true\" title = \"新增子菜单\" " +
                           $"data-ajax-method = \"GET\" data-ajax-mode = \"replace\" data-ajax-update=\"#edit-container\" data-ajax-begin = \"onBegin\" " +
                           $"data-ajax-complete = \"onComplete\" data-ajax-failure = \"onFailed\" data-ajax-success = \"onSuccess\">" +
                           $"<i class=\"iconfont icon-plus-circle\"></i></a>";

                menuTree = menuTree +
                           $"<a href=\"{Url.Action("EditMenu", "Navigation", new { id = item.Id })}\"  data-ajax = \"true\" title = \"编辑菜单\" " +
                           $"data-ajax-method = \"GET\" data-ajax-mode = \"replace\" data-ajax-update=\"#edit-container\" data-ajax-begin = \"onBegin\" " +
                           $"data-ajax-complete = \"onComplete\" data-ajax-failure = \"onFailed\" data-ajax-success = \"onSuccess\">" +
                           $"<i class=\"iconfont icon-edit-square\"></i></a>";

                if (item.Active)
                {
                    menuTree = menuTree +
                               $"<a href = \"#\" data-url = \"{Url.Action("IsActive", new { id = item.Id })}\" class=\"active-item\" title=\"锁定\" data-action=\"激活\"> " +
                               $"<i class=\"iconfont icon-eye-fill\"></i></a>";
                }
                else
                {
                    menuTree = menuTree +
                               $"<a href = \"#\" data-url = \"{Url.Action("IsActive", new { id = item.Id })}\" class=\"active-item\" title=\"激活\" data-action=\"锁定\"> " +
                               $"<i class=\"iconfont icon-eye\"></i></a>";
                }
                menuTree = menuTree +
                           $"<a href = \"#\" data-url=\"{Url.Action("Delete", new { id = item.Id })}\" class=\"delete-item\" title=\"移除菜单\"" +
                           $" data-id=\"{item.Id}\" data-categoryid=\"{item.CategoryId}\"><i class=\"iconfont icon-delete\"></i></a>";



                menuTree = menuTree + $"</div>";
                if (hasChild)
                {
                    menuTree = CreatedMenuList(item.InverseParent, menus, menuTree, item.IsExpand);
                }
                menuTree = menuTree + $"</li>";
            }

            menuTree = menuTree + $"</ul>";
            return menuTree;
        }
    }
}
