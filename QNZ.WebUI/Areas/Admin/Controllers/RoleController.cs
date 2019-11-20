using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

using SIG.Infrastructure.Configs;
using SIG.Model.Admin.InputModel.Identity;
using SIG.Model.Admin.ViewModel;
using SIG.Resources.Admin;
using SIG.Services.Identity;
using SIG.Services.Menus;
using TZGCMS.Model.Admin.ViewModel.Identity;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class RoleController : BaseController
    {
        private readonly IRoleServices _roleServices;
        //private readonly IMenuServices _menuServices;
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        public RoleController(IRoleServices roleServices, IMapper mapper, IMemoryCache memoryCache)
        {
            _roleServices = roleServices;
            //_menuServices = menuServices;
            _mapper = mapper;
            _cache = memoryCache;
        }

        // GET: Role
        public IActionResult Index()
        {
            var roles = _roleServices.GetAll().Where(r => r.Id != SettingsManager.Role.Founder);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_List", roles);
            }
            return View(roles);
        }

        //[AllowAnonymous]
        //public PartialViewResult List()
        //{
        //    var roles = _roleServices.GetAll().Where(r => r.Id != SettingsManager.Role.Founder);
        //    return PartialView("_List", roles);
        //}



        [HttpGet]
        // GET: Roles/Create
        public ActionResult SetRoleMenus(int id)
        {
            var role = _roleServices.GetById(id);
          //  var menus = _menuServices.GetMenusByCategoryId(SettingsManager.Menu.BackMenuCId);
          //  int[] menuIds = role.RoleMenus?.Select(m => m.MenuId).ToArray();

            SetRoleMenusVM vm = new SetRoleMenusVM
            {
                RoleId = id,
               // Menus = menus,
               // MenuIds = menuIds
            };
            return PartialView("_SetRoleMenus",vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetRoleMenus(int RoleId, int[] menuId)
        {

            if (RoleId > 0)
            {
                _roleServices.SetRoleMenus(RoleId, menuId);


                var cacheKey = "AllMenus_6_1";
                _cache.Remove(cacheKey);

                return Json(AR);
            }
            AR.Setfailure("编辑角色权限失败");
            return Json(AR);
        }

        [HttpGet]
        public ActionResult EditRole(int? id)
        {
            var role = (id > 0) ? _roleServices.GetById(id.Value) : new Role();
            var vm = _mapper.Map<RoleIM>(role);
            return PartialView("_EditRole", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(RoleIM role)
        {

            if (ModelState.IsValid)
            {
                if (role.Id > 0)
                {
                    Role vRole = _roleServices.GetById(role.Id);
                    if (!vRole.IsSys)
                    {
                        vRole.RoleName = role.RoleName;
                        vRole.Description = role.Description;
                        _roleServices.Update(vRole);
                    }
                    else
                    {
                        //  return new HttpStatusCodeResult(500, "系统角色不可编辑");
                        AR.SetWarning("系统角色不可编辑");
                        return Json(AR);
                    }

                }
                else
                {
                    var vm = _mapper.Map<Role>(role);
                    _roleServices.Create(vm);
                }
             
                AR.SetSuccess(String.Format(Messages.AlertCreateSuccess, EntityNames.Role));
                return Json(AR);
            }
            //return new HttpStatusCodeResult(500, "编辑角色失败");

            AR.Setfailure("编辑角色失败");
            return Json(AR);
        }



        // POST: Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            var role = _roleServices.GetById(id);
            if (role.IsSys)
            {
                AR.SetWarning("系统角色，不可以删除！");
                return Json(AR);
            }
            _roleServices.Delete(role);
            return Json(AR);

        }

    }
}