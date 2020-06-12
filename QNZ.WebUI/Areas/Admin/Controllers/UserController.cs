using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using QNZ.Data;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Services;
using QNZ.Services.Identity;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;
using QNZ.Resources.Admin;
using X.PagedList;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class UserController : BaseController
    {

        private readonly IUserServices _userServices;
        private readonly IRoleServices _roleServices;
        private readonly IViewRenderService _viewRenderService;
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;

        public UserController(IUserServices userServices, IRoleServices roleServices,
            IViewRenderService viewRenderService, IMapper mapper, YicaiyunContext context)
        {
            _userServices = userServices;
            _roleServices = roleServices;
            _viewRenderService = viewRenderService;
            _mapper = mapper;
            _context = context;

        }

        // GET: User
        // [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? page, string sort, string keyword, DateTime? startDate, DateTime? endDate, int? roleId)
        {
            var vm = new UserListVM
            {
                StartDate = startDate,
                EndDate = endDate,
                Keyword = keyword,
                RoleId = roleId,
                PageIndex = page ?? 1,
                PageSize = SettingsManager.User.PageSize,
                SetPasswordIM = new SetPasswordIM()
            };


            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(keyword)) {
                query = query.Where(d => d.RealName.Contains(keyword) && d.UserName.Contains(keyword));
            }

            if (startDate != null)
            {
                query = query.Where(d => d.CreateDate >= startDate);
            }
            if (endDate != null)
            {
                query = query.Where(d => d.CreateDate <= endDate);
            }
            if (roleId != null)
            {
                query = query.Where(d => d.UserRoles.Any(c => c.RoleId == roleId));
            }


            ViewData["EmailSortParm"] = sort == "email" ? "email_desc" : "email";
            ViewData["MobileSortParm"] = sort == "mobile" ? "mobile_desc" : "mobile";
            ViewData["UsernameSortParm"] = sort == "username" ? "username_desc" : "username";
            ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

            query = sort switch
            {
                "email" => query.OrderBy(s => s.Email),
                "email_desc" => query.OrderByDescending(s => s.Email),
                "username" => query.OrderBy(s => s.UserName),
                "username_desc" => query.OrderByDescending(s => s.UserName),
                "date" => query.OrderBy(s => s.CreateDate),
                "date_desc" => query.OrderByDescending(s => s.CreateDate),
                "mobile" => query.OrderBy(s => s.Mobile),
                "mobile_desc" => query.OrderByDescending(s => s.Mobile),
                _ => query.OrderByDescending(s => s.CreateDate),
            };

            vm.TotalCount = await query.CountAsync();

           var list = await query.Skip((vm.PageIndex - 1)* vm.PageSize).Take(vm.PageSize).ToListAsync();


            vm.Users = new StaticPagedList<User>(list, vm.PageIndex, vm.PageSize, vm.TotalCount);


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserList", list);
            }

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            var roleList = _roleServices.GetAll().Where(m => m.Id != SettingsManager.Role.Founder).ToList();
            var roles = new SelectList(roleList, "Id", "RoleName");
            ViewBag.Roles = roles;

            return View(vm);

        }


        [HttpPost]
        public JsonResult PageSizeSet(int pageSize)
        {
            try
            {
                var xmlFile = PlatformServices.Default.MapPath("~/Config/UserSettings.config");
                //xmlFile = xmlFile.ToLower().Replace(@"\bin\debug\netcoreapp2.0", "");
              //  var xmlFile = HttpHelper.HttpContext.MapPath("~/Config/UserSettings.config");
                XDocument doc = XDocument.Load(xmlFile);

                var item = doc.Descendants("Settings").FirstOrDefault();
                item.Element("PageSize").SetValue(pageSize);
                doc.Save(xmlFile);

                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }
        }

        // GET: User/Details/5
        [HttpGet]
        public IActionResult Details(Guid id)
        {

            var user = _userServices.GetByIdWidthUserRolos(id);
           
            if (user == null)
            {
                return NotFound();
            }

            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    return PartialView("_Detail", vm);
            //}
            var vm = _mapper.Map<UserDetailVM>(user);
            vm.Roles = _roleServices.GetAll().Where(d => user.UserRoles.Any(u => u.RoleId == d.Id)).Select(d => d.RoleName).ToArray();

            return PartialView("_Detail", vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUserForItem(Guid id)
        {
            var user = _userServices.GetByIdWidthUserRolos(id);
            if (user == null)
            {
                return NotFound();
            }
            return PartialView("_UserItem", user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            UserIM vm = new UserIM();
            return PartialView("_UserCreate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(UserIM model)
        {
            if (!ModelState.IsValid)
            {
                var errorMes = GetModelErrorMessage();
                AR.Setfailure(errorMes);
                return Json(AR);
            }


            var result = _userServices.CreateUser(model.UserName, model.Email, model.Password, model.DisplayName,model.Mobile);
            if (result == 1)
            {
                AR.Setfailure(Messages.CannotRegisterEmail);
                return Json(AR);
            }

            if (result == 2)
            {
                AR.Setfailure(Messages.CannotRegisterUserName);
                return Json(AR);
            }

            if (result == 3)
            {
                AR.Setfailure("手机号已经存在");
                return Json(AR);
            }

            //int count;
            //int pageSize = SettingsManager.User.PageSize;
            //var list = _userServices.GetPagedElements(0, pageSize, string.Empty, null, null, null, out count);
            //List<UserVM> userList = _mapper.Map<List<User>, List<UserVM>>(list);
            //AR.Data = RenderPartialViewToString("_UserList", list);
            AR.Id = "0";
            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.User));
            return Json(AR);


        }

        [AllowAnonymous]
        public JsonResult IsUserNameUnique(string UserName)
        {
            var result = _userServices.IsExistUserName(UserName);

            return result
                ? Json(false)
                : Json(true);
        }
        [AllowAnonymous]
        public JsonResult IsEmailUnique(string Email)
        {
            var result = _userServices.IsExistEmail(Email);

            return result
                ? Json(false)
                : Json(true);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            ProfileIM Profiles = new ProfileIM();
            if (id == null)
            {
                return PartialView("_UserEdit", Profiles);
            }
            var user = _userServices.GetById(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            Profiles = _mapper.Map<ProfileIM>(user);
            return PartialView("_UserEdit", Profiles);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ProfileIM profile)
        {
            if (!ModelState.IsValid)
            {
                var errorMes = GetModelErrorMessage();
                AR.Setfailure(errorMes);
                return Json(AR);
            }

            var user = _userServices.GetById(profile.Id);
            if (user == null)
            {
                AR.Setfailure("不存在此用户！");
                return Json(AR);
                // return Json(false);
            }

            var result = _context.Users.Count(u => u.Email == profile.Email && u.Id != profile.Id);
            if( result > 0)
            {
                AR.Setfailure("此邮箱已经存在！");
                return Json(AR);
            }

            var result1 = _context.Users.Count(u => u.Mobile == profile.Mobile && u.Id != profile.Id);
            if (result > 0)
            {
                AR.Setfailure("此手机号已经存在！");
                return Json(AR);
            }
            try
            {
                user.Email = profile.Email;
                user.RealName = profile.RealName;
                user.IsActive = profile.IsActive;
                //user.DepartmentId = profile.DepartmentId;
                //user.PositionId = profile.PositionId;
              //  user.QQ = profile.QQ;
                user.Mobile = profile.Mobile;
             //   user.Gender = profile.Gender;
                user.Birthday = profile.Birthday;
                _userServices.Update(user);
                // var userVM = _mapper.Map<UserVM>(user);

                AR.Id = user.Id;
                //var aa = PartialView("_UserItem", user).ToString();
                //AR.Data = aa;
                //AR.Data = await _viewRenderService.RenderAsync("User/_UserItem", user);

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.User));
                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }

        }
        [AllowAnonymous]
        public JsonResult IsEmailUniqueAtEdit(string email, Guid id)
        {
            var result = _userServices.IsExistEmail(email, id);

            return result
                ? Json(false)
                : Json(true);
        }


        [HttpGet]
        public PartialViewResult SetRole(Guid id)
        {
            var user = _userServices.GetByIdWidthUserRolos(id);
            var userRoles = user.UserRoles;
            SetUserRolesVM vm = new SetUserRolesVM
            {
                UserId = id,
                RoleIds = userRoles?.Select(r => r.RoleId).ToArray(),
                Roles = _roleServices.GetAll().Where(r => r.Id != SettingsManager.Role.Founder)
            };

            return PartialView("_SetRole", vm);
        }


        [HttpPost]
        public JsonResult SetRole(Guid userId, int[] roleId)
        {
            try
            {
                _userServices.SetRole(userId, roleId);
                //if (User.Identity.Name == user.UserName)
                //{
                //    SetUserCookies(true, user);
                //}
          
                AR.Id = userId;
              //  AR.Data = RenderPartialViewToString("_UserItem", user);
                AR.SetSuccess(Messages.AlertActionSuccess);
                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }
        }


        //public void SetUserCookies(bool isPersist, User user)
        //{
        //    var roles = _roleServices.GetAll().Where(r => r.UserRoles.Any(ur => ur.UserId == user.Id))
        //        .Select(m => m.RoleName).ToArray();  
        //    //user.Roles.Select(m => m.RoleName).ToArray();

        //    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel()
        //    {
        //        UserId = user.Id,
        //        RealName = user.RealName,
        //        Avatar = string.IsNullOrEmpty(user.PhotoUrl) ? SettingsManager.User.DefaultAvatar : user.PhotoUrl,
        //        Roles = roles
        //    };
        //    //serializeModel.Menus = GetUserMenus(user.);
        //    TimeSpan timeout = FormsAuthentication.Timeout;
        //    DateTime expire = DateTime.Now.Add(timeout);


        //    string userData = JsonConvert.SerializeObject(serializeModel, Formatting.Indented,
        //        new JsonSerializerSettings()
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });


        //    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
        //             1,
        //             user.UserName,
        //             DateTime.Now,
        //             expire,
        //             isPersist,
        //             userData);

        //    string encTicket = FormsAuthentication.Encrypt(authTicket);
        //    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //    System.Web.HttpContext.Current.Response.Cookies.Add(faCookie);
        //}


        [HttpGet]
        public ActionResult SetPassword(Guid id)
        {
            SetPasswordIM model = new SetPasswordIM
            {
                UserId = id
            };
            return PartialView("_SetPassword", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SetPassword(SetPasswordIM model)
        {
            if (!ModelState.IsValid)
            {
                var errorMes = GetModelErrorMessage();
                AR.Setfailure(errorMes);
                return Json(AR);
            }

            var user = await _context.Users.FindAsync(model.UserId);
            //try
            //{

            var securityStamp = Hash.GenerateSalt();
            var pwdHash = Hash.HashPasswordWithSalt(model.NewPassword, securityStamp);

            user.SecurityStamp = Convert.ToBase64String(securityStamp);
            user.PasswordHash = pwdHash;
            _context.Update(user);
            var result = await _context.SaveChangesAsync();

            

            //var result = _userServices.SetPassword(model.UserId, model.NewPassword);
            if (result>0)
            {
                AR.Id = model.UserId;
                AR.SetSuccess(Messages.AlertActionSuccess);
                return Json(AR);
            }
            AR.Setfailure(Messages.AlertActionFailure);
            return Json(AR);

        }


        //[HttpPost]
        //public JsonResult IsActive(Guid id)
        //{
        //    var user = _userServices.GetById(id);
        //    if (user != null)
        //    {
        //        user.IsActive = !user.IsActive;
        //        _userServices.Update(user);
        //        //    var userVM = _mapper.Map<UserVM>(user);
        //        AR.Id = user.Id;
        //     //   AR.Data = RenderPartialViewToString("_UserItem", user);

        //        AR.SetSuccess(Messages.AlertActionSuccess);
        //        return Json(AR);

        //    }
        //    AR.Setfailure(Messages.AlertActionFailure);
        //    return Json(AR);

        //}


        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(Guid[] ids)
        {

            var c = await _context.Users.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Users.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(Guid[] ids, bool isLock)
        {

            var c = await _context.Users.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            foreach (var item in c)
            {
                item.IsActive = isLock ? false : true;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Json(AR);
        }



        // POST: Users/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(Guid id)
        {
            var user = _userServices.GetById(id);

            if (user == null)
            {
                AR.Setfailure("未找到此用户！");
                return Json(AR);
            }

            if (user.Id.ToString() == SettingsManager.User.Founder)
            {
                AR.SetWarning("创始人帐号，不可以删除！");
                return Json(AR);
            }

            _userServices.Delete(user);
            return Json(AR);
        }
    }

    public static class ViewResultExtensions
    {
        public static string ToHtml(this PartialViewResult result, HttpContext httpContext)
        {
            var feature = httpContext.Features.Get<IRoutingFeature>();
            var routeData = feature.RouteData;
            var viewName = result.ViewName ?? routeData.Values["action"] as string;
            var actionContext = new ActionContext(httpContext, routeData, new ControllerActionDescriptor());
            var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;
            var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, true) ?? options.Value.ViewEngines.Select(x => x.FindView(actionContext, viewName, true)).FirstOrDefault(x => x != null);
            var view = viewEngineResult.View;
            var builder = new StringBuilder();

            using (var output = new StringWriter(builder))
            {
                var viewContext = new ViewContext(actionContext, view, result.ViewData, result.TempData, output, htmlHelperOptions);

                view
                    .RenderAsync(viewContext)
                    .GetAwaiter()
                    .GetResult();
            }

            return builder.ToString();
        }
    }
}