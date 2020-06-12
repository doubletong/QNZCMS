using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Infrastructure.Cache;
using QNZ.Resources.Admin;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class RolesController : BaseController
    {
    
        //private readonly IMenuServices _menuServices;
        private ICacheService _cache;
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public RolesController(IMapper mapper, YicaiyunContext context, ICacheService memoryCache)
        {
          
            _context = context;
            _mapper = mapper;
            _cache = memoryCache;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
                //_roleServices.GetAll().Where(r => r.Id != SettingsManager.Role.Founder);
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
        public async Task<ActionResult> SetRoleMenus(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return PartialView("_SetRoleMenus", role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetRoleMenus(int id, int[] menuId)
        {

            if (id > 0)
            {
                //_roleServices.SetRoleMenus(id, menuId);

                var rolemenus = _context.RoleMenus.Where(d => d.RoleId == id).ToList();
                _context.RemoveRange(rolemenus);             
         
                if (menuId != null)
                {
                    foreach (var mid in menuId)
                    {
                        _context.RoleMenus.Add(new RoleMenu { RoleId = id, MenuId = mid });                     
                    }
                }
                await _context.SaveChangesAsync();


                //var cacheKey = "MENU";
                //_cache.Invalidate(cacheKey);

                return Json(AR);
            }
            AR.Setfailure("编辑角色权限失败");
            return Json(AR);
        }

        [HttpGet]
        public async Task<ActionResult> EditRole(int? id)
        {
            
            var role = (id > 0) ? await _context.Roles.FindAsync(id.Value) : new Role();
            var vm = _mapper.Map<RoleIM>(role);
            return PartialView("_EditRole", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(RoleIM role)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
           
                if (role.Id > 0)
                {
                    Role vRole = await _context.Roles.FindAsync(role.Id);
                    if (!vRole.IsSys)
                    {
                        vRole.RoleName = role.RoleName;
                        vRole.Description = role.Description;
                        _context.Entry(vRole).State = EntityState.Modified;

                    }
                    else
                    {
                    
                        AR.SetWarning("系统角色不可编辑");
                        return Json(AR);
                    }

                }
                else
                {
                    var vm = _mapper.Map<Role>(role);
                    _context.Add(vm);
                }

                await _context.SaveChangesAsync();

                AR.SetSuccess(String.Format(Messages.AlertCreateSuccess, EntityNames.Role));
                return Json(AR);
      
        
        }



        // POST: Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {

            var role = await _context.Roles.FindAsync(id);
            if (role.IsSys)
            {
                AR.SetWarning("系统角色，不可以删除！");
                return Json(AR);
            }
            _context.Remove(role);
            await _context.SaveChangesAsync();
      
            return Json(AR);

        }

    }
}