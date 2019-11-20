using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SIG.Resources.Front;

namespace QNZ.Model.Admin.InputModel.Identity
{
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

}
