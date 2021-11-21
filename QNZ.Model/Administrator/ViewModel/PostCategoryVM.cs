using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QNZ.Resources.Common;

namespace QNZ.Model.Administrator.ViewModel
{
    public class PostCategoryBVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        public string Alias { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
        //[Display(ResourceType = typeof(Labels), Name = "ArticleCount")]
        //public int ArticleCount { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
         public DateTime CreatedDate { get; set; }
         [Display(ResourceType = typeof(Labels), Name = "CreatedBy")]
         public string CreatedBy { get; set; }
    }
    public class PostCategoryList
    {
        public IEnumerable<PostCategoryBVM> Categories { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
    }
    
}