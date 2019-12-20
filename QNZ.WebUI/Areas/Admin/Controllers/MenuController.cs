using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QNZ.Data;
using QNZ.Services;
using QNZ.Services.Menus;
using QNZ.Data.Enums;
using SIG.Infrastructure.Configs;
using QNZ.Model.Admin.ViewModel;
using SIG.Resources.Admin;
using QNZ.Model.ViewModel;
using SIG.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class MenuController : BaseController
    {
        private ICacheService _cache;
        private readonly YicaiyunContext _context;
   
        private readonly IMenuCategoryServices _menuCategoryService;
        private readonly IMenuServices _menuService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IMapper _mapper;

        public MenuController(IMenuServices menuService, IMenuCategoryServices menuCategoryService, IViewRenderService viewRenderService, IMapper mapper,
            YicaiyunContext context, ICacheService memoryCache)
        {
            _menuService = menuService;
            _menuCategoryService = menuCategoryService;
            _viewRenderService = viewRenderService;
            _mapper = mapper;
            _context = context;
            _cache = memoryCache;
        }

        //
        // GET: /Admin/Menu/ 
        public async Task<ActionResult> Index()
        {
            var menuCategory = await _context.MenuCategories.FindAsync(SettingsManager.Menu.BackMenuCId);
                //_menuCategoryService.GetById(SettingsManager.Menu.BackMenuCId);
            //  var vm = _mapper.Map<MenuCategoryVM>(menuCategory);      
            return View(menuCategory);
        }

        /// <summary>
        /// 获取单组菜单
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        public IActionResult GetMenus(int categoryId)
        {
            //不支持控制器视图查看
            return ViewComponent("MenuList", new { categoryId = categoryId });
        }


        [HttpGet]
        public ActionResult CreateMenu(int categoryId, int parentId)
        {
            var vMenu = new Menu();
            MenuIM newDto = _mapper.Map<MenuIM>(vMenu);

            newDto.CategoryId = (int)categoryId;
            newDto.ParentId = parentId;
            return PartialView("_MenuCreate", newDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateMenu(MenuIM menu)
        {

            if (ModelState.IsValid)
            {
                var vMenu = _mapper.Map<Menu>(menu);

                var parentMenu = await _context.Menus.FindAsync(vMenu.ParentId.Value);
                    //_menuService.GetById(vMenu.ParentId.Value);
                vMenu.LayoutLevel = parentMenu.LayoutLevel + 1;
                vMenu.CreatedBy = User.Identity.Name;
                vMenu.CreatedDate = DateTime.Now;



                //自动添加通用操作
                if (vMenu.CategoryId == SettingsManager.Menu.BackMenuCId && vMenu.MenuType == (short)MenuType.PAGE)
                {
               
                    vMenu.InverseParent.Add(new Menu
                    {
                        Title = "编辑",
                        Controller = vMenu.Controller,
                        Action = "Edit",
                        Area = vMenu.Area,
                        MenuType = (short)MenuType.ACTION,
                        CategoryId = vMenu.CategoryId,
                        LayoutLevel = vMenu.LayoutLevel + 1,

                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now,
                    });

                    vMenu.InverseParent.Add(new Menu
                    {
                        Title = "显示/隐藏",
                        Controller = vMenu.Controller,
                        Action = "IsActive",
                        Area = vMenu.Area,
                        MenuType = (short)MenuType.ACTION,
                        CategoryId = vMenu.CategoryId,
                        LayoutLevel = vMenu.LayoutLevel + 1,

                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now,
                    });

                    vMenu.InverseParent.Add(new Menu
                    {
                        Title = "删除",
                        Controller = vMenu.Controller,
                        Action = "Delete",
                        Area = vMenu.Area,
                        MenuType = (short)MenuType.ACTION,
                        CategoryId = vMenu.CategoryId,
                        LayoutLevel = vMenu.LayoutLevel + 1,

                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now,
                    });

                    vMenu.InverseParent.Add(new Menu
                    {
                        Title = "分页设置",
                        Controller = vMenu.Controller,
                        Action = "PageSizeSet",
                        Area = vMenu.Area,
                        MenuType = (short)MenuType.ACTION,
                        CategoryId = vMenu.CategoryId,
                        LayoutLevel = vMenu.LayoutLevel + 1,
                        // ParentId = result,
                        CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now,
                    });

                }

                var result = _context.Add(vMenu);
                await _context.SaveChangesAsync();
                    //_menuService.Create(vMenu);
                //_menuService.CreateAndSort(vMenu);           
               // _menuService.ResetSort(menu.CategoryId);

              //  var menus = _menuService.GetLevelMenusByCategoryId(vMenu.CategoryId);
                AR.Id = menu.CategoryId;
                //AR.Data = ViewComponent("MenuList", new { categoryId = menu.CategoryId });  //await _viewRenderService.RenderToStringAsync("Admin/Menu/_MenuList", menus);
                var cacheKey = "MENU";
                _cache.Invalidate(cacheKey);

                AR.SetSuccess("已成功新增菜单");
                return Json(AR);
            }

            AR.Setfailure("编辑菜单失败");
            return Json(AR);
            //   return RedirectToAction("Index");

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> EditMenu(int id)
        {
            Menu vMenu = await _context.Menus.FindAsync(id);
            //_menuService.GetById(id);
            MenuIM dto = _mapper.Map<MenuIM>(vMenu);
            return PartialView("_MenuEdit", dto);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditMenu(MenuIM menu)
        {

            if (ModelState.IsValid)
            {
                Menu vMenu = _mapper.Map<Menu>(menu);

                Menu orgMenu = await _context.Menus.FindAsync(vMenu.Id);
                orgMenu.Title = vMenu.Title;
                orgMenu.MenuType = vMenu.MenuType;
                orgMenu.Active = vMenu.Active;
                orgMenu.Action = vMenu.Action;
                orgMenu.Area = vMenu.Area;
                orgMenu.CategoryId = vMenu.CategoryId;
                orgMenu.Controller = vMenu.Controller;
                orgMenu.Iconfont = vMenu.Iconfont;
                orgMenu.ParentId = vMenu.ParentId;
                orgMenu.Url = vMenu.Url;
                orgMenu.UpdatedBy = User.Identity.Name;
                orgMenu.UpdatedDate = DateTime.Now;

                _menuService.Update(orgMenu);

                var cacheKey = "MENU";              
                _cache.Invalidate(cacheKey);
              
                // _menuService.ResetSort(orgMenu.CategoryId);

                //  var menus = _menuService.GetLevelMenusByCategoryId(vMenu.CategoryId);
                AR.Id = vMenu.CategoryId;
                //// using a Model
                //string html = view.Render("Emails/Test", new Product("Apple"));

                //// using a Dictionary<string, object>
                //var viewData = new Dictionary<string, object>();
                //viewData["Name"] = "123456";

                //string html = view.Render("Emails/Test", viewData);

                //AR.Data = await _viewRenderService.RenderToStringAsync("_MenuList", menus);

                AR.SetSuccess("已成功保存菜单");
                return Json(AR);

            }

            AR.Setfailure("编辑菜单失败");
            return Json(AR);



        }



        // POST: Admin/User/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {

            Menu vMenu = await _context.Menus.Include(d => d.InverseParent).FirstOrDefaultAsync(d=>d.Id == id);
            // _menuService.GetByIdWithChilds(id);

            if (vMenu != null)
            {

                int cid = vMenu.CategoryId;
                if (SettingsManager.Menu.BackMenuCId == cid)
                {
                    if (User.Identity.Name != SettingsManager.User.Founder)
                    {
                        AR.SetWarning(string.Format(Messages.NotFounderCanNotDelete, EntityNames.Menu));
                        return Json(AR);
                    }

                }

                // var childMenuCount = _menuService.GetMenuCount(id);
                if (vMenu.InverseParent.Count > 1)
                {
                    AR.Setfailure(string.Format(Messages.AlertDeleteFailureHasChild, EntityNames.Menu));
                    return Json(AR);
                }

                _context.Remove(vMenu);
                await _context.SaveChangesAsync();
                //  vMenu.Roles.Clear();
                // _menuService.Delete(vMenu);
              
                //_menuService.ResetSort(vMenu.CategoryId);

                var cacheKey = "MENU";
                _cache.Invalidate(cacheKey);
                //var menus = await _menuService.GetMenus(cid);
                //return PartialView("_MenuList", menus);
                AR.SetSuccess(string.Format(Messages.AlertDeleteSuccess, EntityNames.Menu));
                return Json(AR);
            }


            AR.Setfailure(string.Format(Messages.AlertDeleteFailure, EntityNames.Menu));
            return Json(AR);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult UpDownMove(int id, bool isUp, int categoryId)
        {

            if (isUp)
            {
                var result = _menuService.UpMoveMenu(id);

                if (result == 0)
                {
                    AR.SetInfo("已经在第一位！");
                    return Json(AR);
                }
            }
            else
            {
                var result = _menuService.DownMoveMenu(id);

                if (result == 0)
                {
                    AR.SetInfo("已经在末位！");
                    return Json(AR);
                }

            }

           // var menus = _menuService.GetLevelMenusByCategoryId(categoryId);
            AR.Id = categoryId;
            // AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);
            var cacheKey = "MENU";
            _cache.Invalidate(cacheKey);

            AR.SetSuccess("菜单排位成功！");
            return Json(AR);

            //AR.Setfailure("菜单排位失败！");
            //return Json(AR);

        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> IsExpand(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                menu.IsExpand = !menu.IsExpand;
                //_menuService.Update(menu);
                _context.Entry(menu).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                var cacheKey = "MENU";
                _cache.Invalidate(cacheKey);

                AR.SetSuccess(Messages.AlertActionSuccess);
                return Json(AR);

            }
            AR.Setfailure(Messages.AlertActionFailure);
            return Json(AR);

        }

        [HttpPost]
        public async Task<JsonResult> IsActive(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                menu.Active = !menu.Active;
             
                _context.Entry(menu).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var cacheKey = "MENU";
                _cache.Invalidate(cacheKey);

                AR.SetSuccess(Messages.AlertActionSuccess);
                return Json(AR);

            }
            AR.Setfailure(Messages.AlertActionFailure);
            return Json(AR);

        }

        [HttpPost]
        public JsonResult ResetSort(int id)
        {
            try
            {
                _menuService.ResetSort(id);

                //var menus = _menuService.GetLevelMenusByCategoryId(id);
                AR.Id = id;
                // AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);

                var cacheKey = "MENU";
                _cache.Invalidate(cacheKey);

                AR.SetSuccess(Messages.AlertActionSuccess);
                return Json(AR);
            }
            catch (Exception er)
            {
                AR.Setfailure(er.Message);
                return Json(AR);
            }
        }


        [HttpGet]
        public async Task<IActionResult> MoveMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
           // var menus = _menuService.GetLevelMenusByCategoryId(menu.CategoryId);
            MoveMenuVM vm = new MoveMenuVM
            {
                Id = id,
             //   Menus = menus, //_mapper.Map<List<Menu>, List<MenuVM>>(menus),
                CurrentParentId = (int)menu.ParentId,
                CategoryId = menu.CategoryId
            };

            return PartialView("_MoveMenu", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveMenuAsync(int Id, int menuId)
        {

            if (Id > 0 && menuId > 0)
            {
                var parentMenu = await _context.Menus.FindAsync(menuId);
                var menu = await _context.Menus.FindAsync(Id);
                menu.ParentId = menuId;
                menu.LayoutLevel = parentMenu.LayoutLevel + 1;

                _context.Entry(menu).State = EntityState.Modified;
                await _context.SaveChangesAsync();
               

                //_menuService.ResetSort(menu.CategoryId);

                AR.Id = menu.CategoryId;
                // AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);

                var cacheKey = "MENU";
                _cache.Invalidate(cacheKey);
                return Json(AR);

            }
            AR.Setfailure("移动菜单失败");
            return Json(AR);
        }
    }
}