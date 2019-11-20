using QNZ.Data.Enums;
using SIG.Resources.Front;
using System.ComponentModel.DataAnnotations;

namespace QNZ.Model.Front.InputModel
{
    public class RegisterIM
    {
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        public string Mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "MobileCode")]
        public string MobileCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "StringLengthWithMiniLength", ErrorMessageResourceType = typeof(Validations), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "AppType")]
        public AppType AppType { get; set; }
    }
}
