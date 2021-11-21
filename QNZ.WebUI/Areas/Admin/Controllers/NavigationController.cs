using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QNZ.Data.Enums;
using QNZ.Infrastructure.Configs;
using QNZ.Model.Admin.ViewModel;
using QNZ.Services;
using QNZ.Services.Menus;
using QNZ.Data;
using QNZ.Resources.Common;
using QNZ.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using QNZ.Model;
using QNZ.Infrastructure.Cache;
using QNZ.Model.Administrator;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class NavigationController : BaseController
    {
        private ICacheService _cache;
        private readonly QNZContext _context;

   
        private readonly IMenuServices _menuService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IMapper _mapper;

        public NavigationController(IMenuServices menuService, IViewRenderService viewRenderService, IMapper mapper,
            QNZContext context, ICacheService memoryCache)
        {
            _menuService = menuService;         
            _viewRenderService = viewRenderService;
            _mapper = mapper;
            _context = context;
            _cache = memoryCache;
        }

        //
        // GET: /Admin/Menu/ 
        public async Task<ActionResult> Index()
        {
            var vm = await _context.NavigationCategories
                .ProjectTo<MenuCategoryVM>(_mapper.ConfigurationProvider).ToListAsync();   
            return View(vm);
        }


        [HttpGet]
        public ActionResult EditCategory()
        {
            var im = new NavigationCategoryIM
            {
                Importance = 0,
                Active = true
            };
            return View(im);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCategory([Bind("Id, Title, Importance, Active")] NavigationCategoryIM im)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

            var model = _mapper.Map<NavigationCategory>(im);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
            model.IsSys = false;
           
            _context.Add(model);

            await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Index));
            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.MenuCategory));
            return Json(AR);
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
            return ViewComponent("NavList", new { categoryId });
        }


        [HttpGet]
        public ActionResult CreateMenu(int categoryId, int? parentId)
        {
            var vMenu = new Navigation();
            NavIM newDto = _mapper.Map<NavIM>(vMenu);

            newDto.CategoryId = categoryId;
            newDto.ParentId = parentId;
            return PartialView("_MenuCreate", newDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateMenu(NavIM menu)
        {

            if (ModelState.IsValid)
            {
                var vMenu = _mapper.Map<Navigation>(menu);
                if (menu.ParentId != null)
                {
                    var parentMenu = await _context.Navigations.FindAsync(vMenu.ParentId.Value);
                    vMenu.LayoutLevel = parentMenu.LayoutLevel + 1;
                }
                else
                {
                    vMenu.LayoutLevel = 0;
                }
            
                vMenu.CreatedBy = User.Identity.Name;
                vMenu.CreatedDate = DateTime.Now;

                _context.Add(vMenu);
                await _context.SaveChangesAsync();

                var pm = new PageMeta
                {
                    Title = menu.SEOTitle,
                    Description = menu.SEODescription,
                    Keywords = menu.SEOKeywords,
                    ModuleType = (short)ModuleType.MENU,
                    ObjectId = vMenu.Url
                };

                await CreatedUpdatedPageMetaAsync(_context, pm);
            

                //  var menus = _menuService.GetLevelMenusByCategoryId(vMenu.CategoryId);
                AR.Id = menu.CategoryId;
                //AR.Data = ViewComponent("MenuList", new { categoryId = menu.CategoryId });  //await _viewRenderService.RenderToStringAsync("Admin/Menu/_MenuList", menus);
                var cacheKey = "NAVIGATION";
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
            var vMenu = await _context.Navigations.FindAsync(id);
            NavIM dto = _mapper.Map<NavIM>(vMenu);

            var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.MENU && d.ObjectId == dto.Url);

            if (pm != null)
            {
                dto.SEOTitle = pm.Title;
                dto.SEOKeywords = pm.Keywords;
                dto.SEODescription = pm.Description;
            }

            return PartialView("_MenuEdit", dto);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditMenu(NavIM menu)
        {

            if (ModelState.IsValid)
            {
               // Menu vMenu = _mapper.Map<Menu>(menu);

                var orgNav = await _context.Navigations.FindAsync(menu.Id);
                var im = _mapper.Map(menu, orgNav);

            
                im.UpdatedBy = User.Identity.Name;
                im.UpdatedDate = DateTime.Now;

                _context.Entry(im).State = EntityState.Modified;
                await _context.SaveChangesAsync();
              

                var cacheKey = "NAVIGATION";
                _cache.Invalidate(cacheKey);
                // _menuService.ResetSort(orgMenu.CategoryId);
                //  var menus = _menuService.GetLevelMenusByCategoryId(vMenu.CategoryId);
                AR.Id = im.CategoryId;
                //// using a Model
                //string html = view.Render("Emails/Test", new Product("Apple"));

                //// using a Dictionary<string, object>
                //var viewData = new Dictionary<string, object>();
                //viewData["Name"] = "123456";

                //string html = view.Render("Emails/Test", viewData);
                //AR.Data = await _viewRenderService.RenderToStringAsync("_MenuList", menus);

                var pm = new PageMeta
                {
                    Title = menu.SEOTitle,
                    Description = menu.SEODescription,
                    Keywords = menu.SEOKeywords,
                    ModuleType = (short)ModuleType.MENU,
                    ObjectId = menu.Url
                };

                await CreatedUpdatedPageMetaAsync(_context, pm);


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

            var vMenu = await _context.Navigations.Include(d => d.InverseParent).FirstOrDefaultAsync(d => d.Id == id);
                //_menuService.GetByIdWithChilds(id);

            if (vMenu != null)
            {


                // var childMenuCount = _menuService.GetMenuCount(id);
                if (vMenu.InverseParent.Count > 1)
                {
                    AR.Setfailure(string.Format(Messages.AlertDeleteFailureHasChild, EntityNames.Menu));
                    return Json(AR);
                }


                _context.Navigations.Remove(vMenu);
                await _context.SaveChangesAsync();

                //_menuService.ResetSort(vMenu.CategoryId);

                var cacheKey = "NAVIGATION";
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
        public async Task<IActionResult> UpDownMove(int id, bool isUp, int categoryId)
        {

            if (isUp)
            {
                var result = await UpMoveMenuAsync(id);

                if (result == 0)
                {
                    AR.SetInfo("已经在第一位！");
                    return Json(AR);
                }
            }
            else
            {
                var result = await DownMoveMenuAsync(id);

                if (result == 0)
                {
                    AR.SetInfo("已经在末位！");
                    return Json(AR);
                }

            }

            // var menus = _menuService.GetLevelMenusByCategoryId(categoryId);
            AR.Id = categoryId;
            // AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);
            var cacheKey = "NAVIGATION";
            _cache.Invalidate(cacheKey);

            AR.SetSuccess("菜单排位成功！");
            return Json(AR);

            //AR.Setfailure("菜单排位失败！");
            //return Json(AR);

        }


        /// <summary>
        /// 向上移动
        /// </summary>
        /// <param name = "id" ></param >
        /// < returns ></returns >
        private async Task<int> UpMoveMenuAsync(int id)
        {
            var vMenu = await _context.Navigations.FirstOrDefaultAsync(d => d.Id == id);
            //_unitOfWork.GetRepository<Menu>().GetFirstOrDefault(predicate: d => d.Id == id);
            var menuList = await GetMenusByCategoryIdAsync(vMenu.CategoryId);

            var prevMenu = await _context.Navigations.OrderByDescending(d => d.Importance).FirstOrDefaultAsync(d => d.ParentId == vMenu.ParentId && d.Id != vMenu.Id && d.Importance <= vMenu.Importance);



            //_unitOfWork.GetRepository<Menu>().GetFirstOrDefault(
            //predicate: d => d.ParentId == vMenu.ParentId && d.Id !=vMenu.Id && d.Importance <= vMenu.Importance,
            //orderBy: d => d.OrderByDescending(m => m.Importance));


            //if (prevMenu == null)
            //{
            //    // 已经在第一位
            //    return 0;
            //}
            var num = prevMenu.Importance - vMenu.Importance;
            if (num == 0)
            {
                vMenu.Importance -= 1;
            }
            else
            {
                prevMenu.Importance -= num;
                vMenu.Importance += num;
            }


            _context.Entry(vMenu).State = EntityState.Modified;
            _context.Entry(prevMenu).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync();

            return result;

        }

        /// <summary>
        /// 向下移动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<int> DownMoveMenuAsync(int id)
        {
            var vMenu = await _context.Navigations.FirstOrDefaultAsync(d => d.Id == id);
            var menuList = await GetMenusByCategoryIdAsync(vMenu.CategoryId);
            var nextMenu = await _context.Navigations.FirstOrDefaultAsync(d => d.ParentId == vMenu.ParentId && d.Id != vMenu.Id && d.Importance >= vMenu.Importance);
       

            //Menu nextMenu = menuList.Where(m => m.ParentId == vMenu.ParentId &&
            //    m.Importance > vMenu.Importance).OrderBy(m => m.Importance).FirstOrDefault();


            //if (nextMenu == null)
            //{
            //    // 已经在最后一位
            //    return 0;
            //}

            var num = nextMenu.Importance - vMenu.Importance;
            if (num == 0)
            {
                vMenu.Importance += 1;
            }
            else
            {
                nextMenu.Importance -= num;
                vMenu.Importance += num;
            }


            _context.Entry(vMenu).State = EntityState.Modified;
            _context.Entry(nextMenu).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync();

            return result;
        }

        private async Task<IEnumerable<Navigation>> GetMenusByCategoryIdAsync(int categoryId)
        {
            if (!SettingsManager.Site.EnableCaching)
            {              
                return await _context.Navigations.Include(d => d.InverseParent).Where(d => d.CategoryId == categoryId).OrderBy(d => d.Importance).ToListAsync();
            }

            var cacheKey = $"NAVIGATIONS_CATEGORY_{categoryId}";
            if (_cache.IsSet(cacheKey))
            {
                return (IEnumerable<Navigation>)_cache.Get(cacheKey);
            }

            var result = await _context.Navigations.Include(d => d.InverseParent).Where(d => d.CategoryId == categoryId).OrderBy(d => d.Importance).ToListAsync();
            _cache.Set(cacheKey, result, SettingsManager.Site.CacheDuration);
            return result;

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> IsExpand(int id)
        {
            var menu = await _context.Navigations.FindAsync(id);
            if (menu != null)
            {
                menu.IsExpand = !menu.IsExpand;
                _context.Entry(menu).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var cacheKey = "NAVIGATION";
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
            var menu = await _context.Navigations.FindAsync(id);
            if (menu != null)
            {
                menu.Active = !menu.Active;
                _context.Entry(menu).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var cacheKey = "NAVIGATION";
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

                var cacheKey = "NAVIGATION";
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
            var menu = await _context.Navigations.FindAsync(id);
            // var menus = _menuService.GetLevelMenusByCategoryId(menu.CategoryId);
            var vm = new MoveNavVM
            {
                Id = id,
                //   Menus = menus, //_mapper.Map<List<Menu>, List<MenuVM>>(menus),
                CurrentParentId = menu.ParentId,
                CategoryId = menu.CategoryId
            };

            return PartialView("_MoveMenu", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MoveMenu(int Id, int menuId)
        {

            if (Id > 0 && menuId > 0)
            {
                var parentMenu = _menuService.GetById(menuId);
                var menu = _menuService.GetById(Id);
                menu.ParentId = menuId;
                menu.LayoutLevel = parentMenu.LayoutLevel + 1;
                _menuService.Update(menu);

                //_menuService.ResetSort(menu.CategoryId);

                // var menus = _menuService.GetLevelMenusByCategoryId(menu.CategoryId);
                AR.Id = menu.CategoryId;
                // AR.Data = await _viewRenderService.RenderAsync("_MenuList", menus);

                var cacheKey = "NAVIGATION";
                _cache.Invalidate(cacheKey);
                return Json(AR);

            }
            AR.Setfailure("移动菜单失败");
            return Json(AR);
        }
    }
}