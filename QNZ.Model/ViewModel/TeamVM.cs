using System.ComponentModel.DataAnnotations;
using PagedList.Core;
using QNZ.Resources.Admin;

namespace QNZ.Model.ViewModel
{
    public class TeamVM
    {
        
        public int Id { get; set; }
       
        [Display(ResourceType = typeof(Labels), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Photo")]
        public string PhotoUrl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Post")]
        public string Post { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
 
        public string QQ { get; set; }
     
        public string CodePen { get; set; }
       
        public string Twitter { get; set; }
   
        public string Github { get; set; }
        
        public string Facebook { get; set; }
        
        public string Weixin { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        public string Email{ get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance  { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
    public class TeamPageVM
    {
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<TeamVM> Teams { get; set; }
    }

    public class TeamIM
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Name")]
        public string Name { get; set; }

        // [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Photo")]
        public string PhotoUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Post")]
        public string Post { get; set; }

        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string QQ { get; set; }

        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string CodePen { get; set; }

        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Twitter { get; set; }

        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Github { get; set; }

        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Facebook { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Wechat")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Weixin { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Email")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Email { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }
}