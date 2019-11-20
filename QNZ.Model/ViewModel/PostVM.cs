using System;
using System.ComponentModel.DataAnnotations;
using PagedList.Core;
using SIG.Resources.Admin;

namespace QNZ.Model.ViewModel
{
    public class PostBVM
         {
             public int Id { get; set; }
             [Display(ResourceType = typeof(Labels), Name = "Title")]
             public string Title { get; set; }  
             [Display(ResourceType = typeof(Labels), Name = "Category")]
             public string CategoryTitle { get; set; }  
             [Display(ResourceType = typeof(Labels), Name = "ViewCount")]
             public int ViewCount { get; set; }     
             [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
             public string Thumbnail { get; set; }   
             [Display(ResourceType = typeof(Labels), Name = "Recommend")]
             public bool Recommend { get; set; }   
             [Display(ResourceType = typeof(Labels), Name = "Active")]
             public bool Active { get; set; } 
             [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
             public DateTime CreatedDate { get; set; }
             [Display(ResourceType = typeof(Labels), Name = "Author")]
             public string CreatedBy { get; set; }
         }
    
    public class PostPageVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }     
        public int TotalCount { get; set; }
        public int? CategoryId { get; set; }
        public StaticPagedList<PostBVM> Posts { get; set; }
    }
    
    public class PostIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public string Body { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Summary { get; set; }    
        
        [Display(ResourceType = typeof(Labels), Name = "Category")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
        
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]   
        public string Thumbnail { get; set; }   
        
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]   
        public bool Recommend { get; set; }      
        
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }

    }
}