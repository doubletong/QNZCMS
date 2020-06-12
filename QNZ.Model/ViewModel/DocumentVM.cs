using QNZ.Resources.Admin;
using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    #region Front
    public class DocCategoryVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

    }
    public class DocumentVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public DateTime Pubdate { get; set; }

    }
   

    public class DocumentPageVM
    {
        public IEnumerable<DocumentVM> RecommendDocuments { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<DocumentVM> Documents { get; set; }
    }

    

    public class DocumentDetailVM
    {
        public Document DocumentDetail { get; set; }
        public Document DocumentPrev { get; set; }
        public Document DocumentNext { get; set; }
    }
    #endregion


    #region Back

    public class DocCategoryBVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")] 
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        public string ImageUrl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        public string Alias { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]     
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
       
    }
    public class DocCategoryList
    {
        public IEnumerable<DocCategoryBVM> Categories { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
    }
    public class DocCategoryIM:PageMetaIM
    {

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }

   

        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsAliasUnique", "DocCategories", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }

    public class DocumentBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryTitle { get; set; }
        public int DownloadCount { get; set; }
        public DateTime Pubdate { get; set; }     
        public string FileUrl { get; set; }
        public long FileSize { get; set; }
        public bool Active { get; set; }
      
 

    }
    public class DocumentListVM
    {
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<DocumentBVM> Documents { get; set; }
    }

    public class DocumentIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }



        [StringLength(250, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

       

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "FileUrl")]
        public string FileUrl { get; set; }

      

        [Display(ResourceType = typeof(Labels), Name = "Category")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Pubdate")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public DateTime PubDate { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
       

    }
    #endregion
}
