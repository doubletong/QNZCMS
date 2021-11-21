using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Resources.Common;
using X.PagedList;

namespace QNZ.Model.Administrator.ViewModel
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
    
    public class PostListVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }     
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int? CategoryId { get; set; }
        public StaticPagedList<PostBVM> Posts { get; set; }
    }
    
   
}