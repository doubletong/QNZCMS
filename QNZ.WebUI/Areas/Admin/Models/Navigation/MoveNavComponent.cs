using Microsoft.AspNetCore.Mvc;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Services.Menus;
using System.Collections.Generic;
using System.Linq;

namespace SIG.SIGCMS.Areas.Admin.Models.Navigation
{
    [ViewComponent(Name = "MoveNav")]
    public class MoveNavComponent : ViewComponent
    {
        //private readonly IMenuServices _menuServices;
        private readonly YicaiyunContext _context;
        public MoveNavComponent(YicaiyunContext context)
        {
            _context = context;

        }
        public IViewComponentResult Invoke(int id)
        {
            var menu = _context.Navigations.Find(id);
            var menus = _context.Navigations.Where(d=>d.CategoryId == menu.CategoryId).ToList();
            var MenuTree = CreatedMenuList(menus.Where(d => d.ParentId == null), menu);

            return View("MoveNav", MenuTree);
            // return View("Menus", _menuServices.CurrenMenuCrumbs(SettingsManager.Menu.BackMenuCId, viewContext));
        }

        private string CreatedMenuList(IEnumerable<QNZ.Data.Navigation> levelMenus, QNZ.Data.Navigation menu, string menuTree = "")
        {

            menuTree += $"<ul class=\"menuTree menuTreeRole list-unstyled\">";
            foreach (var item in levelMenus.OrderBy(m => m.Importance))
            {
                var hasMenus = item.InverseParent.Any() ? "hasMenus" : "";
                if (item.MenuType == (short)MenuType.PAGE || item.MenuType == (short)MenuType.NOLINK)
                {
                    menuTree += $"<li class=\"{hasMenus}\" >";
                    menuTree += $"<label class=\"{(item.Id == menu.Id ? "text-danger" : "")}\">";
                    menuTree += $"<input type = \"radio\" name=\"menuId\" value=\"{item.Id}\" {(menu.ParentId == item.Id ? "checked" : "")} {(item.Id == menu.Id ? "disabled" : "")} />";
                    menuTree += item.Title;
                    menuTree += $"</label>";

                    if (item.InverseParent.Any())
                    {
                        menuTree = CreatedMenuList(item.InverseParent, menu, menuTree);
                    }
                    menuTree += $"</li>";

                }

            }

            menuTree += $"</ul>";
            return menuTree;
        }
    }
}
