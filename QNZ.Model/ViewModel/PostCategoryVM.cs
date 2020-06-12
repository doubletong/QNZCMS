using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QNZ.Resources.Admin;

namespace QNZ.Model.ViewModel
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
    }
    public class PostCategoryIM : PageMetaIM
    {    

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        [StringLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsAliasUnique", "PostCategories", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
}