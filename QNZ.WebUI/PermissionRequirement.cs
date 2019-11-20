using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QNZ.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QNZCMS
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }
        public PermissionRequirement(string deniedAction)
        {
            DeniedAction = deniedAction;
        }
    }

    public class SetActionAttribute : Attribute
    {
        public string ActionName { get; set; }
    }
    //http://bubuko.com/infodetail-2371138.html
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 用户所有权限
        /// </summary>
        public IEnumerable<Menu> RoleMenus { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// 当前方法的名称
        /// </summary>
        private string _actionName = string.Empty;
       // readonly IMenuServices _menuServices;
        readonly YicaiyunContext _db;
        readonly ILogger _logger;
        public PermissionHandler(IHttpContextAccessor httpContextAccessor,/*IMenuServices menuServices*/ YicaiyunContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {

                //是否ajax
                bool isAjaxCall = httpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
                var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
                var httpMethod = httpContext.Request.Method;
                //请求Url
                var questUrl = httpContext.Request.Path.Value.ToLower();

                //登陆用户为admin 直接跳过
                if (isAuthenticated)
                {
                    var currentUser = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name)?.Value;

                    if (currentUser == "admin")
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }


                    var routeData = _httpContextAccessor.HttpContext.GetRouteData();

                    var areaName = routeData?.Values["area"]?.ToString();
                    var area = string.IsNullOrWhiteSpace(areaName) ? string.Empty : areaName;

                    var controllerName = routeData?.Values["controller"]?.ToString();
                    var controller = string.IsNullOrWhiteSpace(controllerName) ? string.Empty : controllerName;

                    var actionName = routeData?.Values["action"]?.ToString();
                    var action = string.IsNullOrWhiteSpace(actionName) ? string.Empty : actionName;


                    if (controller.ToLower() == "home" && area.ToLower() == "admin")
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    var userId = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid)?.Value;
                    var uid = new Guid(userId);

                    var roles = _db.UserRoles.Where(d => d.UserId == uid).ToList();
                    var roleIds = roles.Select(d => d.RoleId);
                    var menus = _db.RoleMenus.Where(d => roleIds.Contains(d.RoleId)).ToList();
                    // await _unitOfWork.GetRepository<RoleMenu>().GetManyAsync(predicate: m => roleIds.Contains(m.RoleId));
                    var menuIds = menus.Select(d => d.MenuId);
                    // return await _unitOfWork.GetRepository<Menu>().GetManyAsync(predicate: d => menuIds.Contains(d.Id));

                    RoleMenus = _db.Menus.Where(d => menuIds.Contains(d.Id));

                    //_menuServices.GetRolesMenusByUserId(new Guid(userId)).Result;
                    bool hasCurrentControllerRole = RoleMenus.Where(w => w.Action?.ToLower() == action.ToLower() && w.Area?.ToLower() == area.ToLower() &&
                        w.Controller?.ToLower() == controller.ToLower()).Any();
                    if (hasCurrentControllerRole)
                    {
                        //当前用户角色名
                        //var roleName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value.Split(",");
                        //if (RoleMenus.Where(w => /*roleName.Contains(w.RoleName) &&*/ w.Controller == controllerName.ToLower() && 
                        //w.Action?.ToLower() == _actionName.ToLower() && w.Area == areaName.ToLower()).Any())
                        //{
                        //有权限标记处理成功
                        context.Succeed(requirement);
                        //}
                    }
                }


             



            }




          
            return Task.CompletedTask;
        }
    }

}
