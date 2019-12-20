using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QNZ.Data.Enums;
using QNZ.Services.Menus;


namespace QNZCMS.Areas.Admin.Models.Menu
{
    [ViewComponent(Name = "MoveMenu")]
    public class MoveMenuComponent : ViewComponent
    {
        private readonly IMenuServices _menuServices;

        public MoveMenuComponent(IMenuServices menuServices)
        {
            _menuServices = menuServices;

        }
        public IViewComponentResult Invoke(int id)
        {
            var menu = _menuServices.GetById(id);
            var menus = _menuServices.GetMenusByCategoryId(menu.CategoryId);
            var MenuTree = CreatedMenuList(menus.Where(d=>d.ParentId== null), menu);

            return View("MoveMenu", MenuTree);
            // return View("Menus", _menuServices.CurrenMenuCrumbs(SettingsManager.Menu.BackMenuCId, viewContext));
        }

        private string CreatedMenuList(IEnumerable<QNZ.Data.Menu> levelMenus, QNZ.Data.Menu menu, string menuTree = "")
        {
            
            menuTree = menuTree + $"<ul class=\"menuTree menuTreeRole list-unstyled\">";
            foreach (var item in levelMenus.OrderBy(m=>m.Importance))
            {
                var hasMenus = item.InverseParent.Any() ? "hasMenus" : "";
                if (item.MenuType == (short)MenuType.PAGE || item.MenuType == (short)MenuType.NOLINK)
                {
                    menuTree = menuTree + $"<li class=\"{hasMenus}\" >";
                    menuTree = menuTree + $"<label class=\"{(item.Id == menu.Id ? "text-danger" : "")}\">";
                    menuTree = menuTree + $"<input type = \"radio\" name=\"menuId\" value=\"{item.Id}\" {(menu.ParentId == item.Id?"checked":"")} {(item.Id==menu.Id? "disabled" : "")} />";
                    menuTree = menuTree + item.Title;
                    menuTree = menuTree + $"</label>";

                    if(item.InverseParent.Any())
                    {
                        menuTree = CreatedMenuList(item.InverseParent, menu, menuTree);
                    }
                    menuTree = menuTree + $"</li>";
                    
                }
                
            }

            menuTree = menuTree + $"</ul>";
            return menuTree;
        }
    }
}
