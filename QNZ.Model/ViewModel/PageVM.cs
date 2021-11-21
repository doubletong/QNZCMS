using Microsoft.AspNetCore.Mvc;
using QNZ.Resources.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    #region Front
    public class PageVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "SeoName")]
        public string SeoName { get; set; }
        
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ViewCount")]
        public int ViewCount { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

    }
    public class PageDetailVM:PageVM
    {
        public string Body { get; set; }
    }
    #endregion
    public class PageBVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "SeoName")]
        public string SeoName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ViewCount")]
        public int ViewCount { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

    }
    public class PageIM:PageMetaIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public string Body { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "SeoName")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsSeoNameUnique", "Pages", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string SeoName { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }

    }

    public class PageListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<PageVM> Pages { get; set; }
    }

}
