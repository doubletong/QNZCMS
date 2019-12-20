using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QNZ.Data;
using QNZ.Model.Front.ViewModel;
using QNZ.Model.ViewModel;
using SIG.Infrastructure.Helper;
using SIG.Resources.Front;

namespace QNZCMS.Controllers
{
    public class AccountController :  BaseController
    {

        private readonly ILogger<AccountController> _logger;
        private readonly TokenOptions _tokenOptions;
        private readonly YicaiyunContext _context;
        public AccountController(IOptions<TokenOptions> tokens, ILogger<AccountController> logger, YicaiyunContext context)
        {


            _tokenOptions = tokens.Value;
            _logger = logger;
            _context = context;


        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Register(RegisterIM model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        var errorMes = GetModelErrorMessage();
        //        AR.Setfailure(errorMes);
        //        return Json(AR);
        //        // return Json(false);
        //    }


        //    var result = _userServices.CreateUser(model.UserName, model.Email, model.Password, model.DisplayName,model.Mobile);

        //    if (result == 1)
        //    {
        //        AR.Setfailure(Messages.CannotRegisterEmail);
        //        _logger.LogError(Messages.CannotRegisterEmail);
        //        return Json(AR);
        //    }

        //    if (result == 2)
        //    {
        //        AR.Setfailure(Messages.CannotRegisterUserName);
        //        _logger.LogError(Messages.CannotRegisterUserName);
        //        return Json(AR);
        //    }


        //    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, model.UserName));
        //    _logger.LogError(string.Format(Messages.AlertCreateSuccess, model.UserName));
        //    return Json(AR);
        //}
        public IActionResult Login(string returnUrl)
        {
            var im = new LoginIM
            {
                ReturnUrl = returnUrl
            };

            return View(im);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginIM im)
        {


            if (im == null)
            {
                AR.Setfailure(Messages.InvalidUserNameOrPassword);
                return Json(AR);
            }

            // var lookupUser = _userServices.SignIn(im.Username, im.Password);

            var user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.UserName == im.Username);
            if (user == null)
            {
                AR.Setfailure(Messages.InvalidUserNameOrPassword);
                return Json(AR);
            }

            var salt = Convert.FromBase64String(user.SecurityStamp);
            var pwdHash = Hash.HashPasswordWithSalt(im.Password, salt);

            if (user.PasswordHash != pwdHash)
            {
                AR.Setfailure(Messages.InvalidUserNameOrPassword);
                return Json(AR);
            };



            // create claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("RealName", user.RealName??"无"),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // create identity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var userRoles = await _context.Roles.Where(d => d.UserRoles.Any(r => r.UserId == user.Id)).ToArrayAsync();
            //add a list of roles

            if (userRoles.Any())
            {
                var roles = string.Join(",", userRoles.Select(d => d.RoleName));
                identity.AddClaim(new Claim(ClaimTypes.Role, roles));
            }

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = im.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.Add(TimeSpan.FromDays(180))
            });

            AR.SetSuccess(Messages.Wellcome);
            return Json(AR);
        }

        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Access()
        {
            return View();
        }



        #region " Ajax "


        //public JsonResult IsUserNameUnique(string userName)
        //{
        //    var result = _userServices.IsExistUserName(userName);

        //    return result
        //        ? Json(false)
        //        : Json(true);
        //}

        //public JsonResult IsEmailUnique(string email)
        //{
        //    var result = _userServices.IsExistEmail(email);

        //    return result
        //        ? Json(false)
        //        : Json(true);
        //}
        //public JsonResult IsEmailUniqueAtEdit(string email, Guid id)
        //{
        //    var result = _userServices.IsExistEmail(email, id);

        //    return result
        //        ? Json(false)
        //        : Json(true);
        //}

        #endregion


    }


}