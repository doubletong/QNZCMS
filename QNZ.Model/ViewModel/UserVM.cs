using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Resources.Admin;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    public class UserListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string Keyword { get; set; }

        //[DateLessThan("EndDate", ErrorMessage = "开始日期必须小于截止日期")]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int? RoleId { get; set; }

        public StaticPagedList<User> Users { get; set; }

        public SetPasswordIM SetPasswordIM { get; set; }

    }
    public class UserIM
    {
        [Remote("IsUserNameUnique", "User", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "UserName")]
        public string UserName { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "DisplayName")]
        public string DisplayName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [RegularExpression("^1\\d{10}$",ErrorMessage ="手机格式不正确")]
        public string Mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "StringLengthWithMiniLength", ErrorMessageResourceType = typeof(Validations), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "ComparePassword")]
        public string ConfirmPassword { get; set; }

        [Remote("IsEmailUnique", "User", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        public string Email { get; set; }

    }
    public class SetPasswordIM
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileIM
    {
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public Guid Id { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "UserName")]
        public string UserName { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "RealName")]
        public string RealName { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        [Remote("IsEmailUniqueAtEdit", "User", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool IsActive { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Gender")]
        public Gender Gender { get; set; }

        //[QQ]
        public string QQ { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        //[ChinaMobile]
        public string Mobile { get; set; }
    }

    public class LoginIM
    {
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "UserName")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "RememberMe")]
        public bool RememberMe { get; set; }

        //[Display(ResourceType = typeof(Labels), Name = "CaptchaText")]
        //[Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        //public string CaptchaText { get; set; }

        public string ReturnUrl { get; set; }
    }


    public class RegisterIM
    {
        [Remote("IsUserNameUnique", "Account", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "UserName")]
        public string UserName { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "DisplayName")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [RegularExpression("^1\\d{10}$", ErrorMessage = "手机格式不正确")]
        public string Mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "StringLengthWithMiniLength", ErrorMessageResourceType = typeof(Validations), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "ComparePassword")]
        public string ConfirmPassword { get; set; }

        [Remote("IsEmailUnique", "Account", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        public string Email { get; set; }
    }

    public class UserDetailVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public string RealName { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime? Birthday { get; set; }

        public Gender Gender { get; set; }
        public string GenderName { get; set; }
        public string QQ { get; set; }

        public string Mobile { get; set; }

        public string[] Roles { get; set; }
    }

    public class SetUserRolesVM
    {
        public Guid UserId { get; set; }
        public int[] RoleIds { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }

    public class SetRoleMenusVM
    {
        public int[] MenuIds { get; set; }
        public IEnumerable<Menu> Menus { get; set; }
        public int RoleId { get; set; }
    }
}
