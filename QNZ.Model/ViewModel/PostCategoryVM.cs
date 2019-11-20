using System;
using System.ComponentModel.DataAnnotations;
using SIG.Resources.Admin;

namespace QNZ.Model.ViewModel
{
    public class PostCategoryBVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ArticleCount")]
        public int ArticleCount { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
         public DateTime CreatedDate { get; set; }
         [Display(ResourceType = typeof(Labels), Name = "CreatedBy")]
         public string CreatedBy { get; set; }
    }
    
    public class PostCategoryIM
    {    

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        [StringLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Description { get; set; }
  

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
}