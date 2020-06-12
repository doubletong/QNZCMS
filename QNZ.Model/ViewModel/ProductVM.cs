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
    public class ProductCategoryVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

    }
    public class ProductVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public string Body { get; set; }
        public string Source { get; set; }
        public int Importance { get; set; }
        public string Thumbnail { get; set; }

    }
   

    public class ProductPageVM
    {
        public IEnumerable<ProductCategoryVM> Categories { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public int? CategoryId { get; set; }

    }

    

    public class ProductDetailVM
    {
        public Product ProductDetail { get; set; }
        public Product ProductPrev { get; set; }
        public Product ProductNext { get; set; }
    }
    #endregion


    #region Back


    public class ProductCategoryBVM
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
    public class ProductCategoryList
    {
        public IEnumerable<ProductCategoryBVM> Categories { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
    }
    public class ProductCategoryIM:PageMetaIM
    {

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsAliasUnique", "ProductCategories", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }

    public class ProductBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string CategoryTitle { get; set; }
        public int DownloadCount { get; set; }
        public DateTime CreatedDate { get; set; }     
     
        public bool Active { get; set; }
      
        public bool Recommend { get; set; }

    }
    public class ProductListVM
    {
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<ProductBVM> Products { get; set; }
    }

    public class ProductIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }



        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Document")]
        public string FileUrl { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Content")]
        public string Body { get; set; }



        [Display(ResourceType = typeof(Labels), Name = "Category")]
        public int CategoryId { get; set; }

       
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

    }
    #endregion
}
