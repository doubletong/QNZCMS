using System.ComponentModel.DataAnnotations;
using SIG.Resources.Admin;

namespace QNZ.Model.Admin.InputModel.Identity
{
    public class RoleIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "RoleName")]
        public string RoleName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
    }
}