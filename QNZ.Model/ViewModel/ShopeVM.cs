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
   
    public class ShopeVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int ToLeft { get; set; }
        public int ToTop { get; set; }
        public int Importance { get; set; }
        public byte IconType { get; set; }

    }
   

    public class ShopePageVM
    {

        public int? Type { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<ShopeVM> Shopes { get; set; }
    }

    

    public class ShopeDetailVM
    {
        public Shope ShopeDetail { get; set; }
        public Shope ShopePrev { get; set; }
        public Shope ShopeNext { get; set; }
    }
    #endregion


    #region Back


    public class ShopeBVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public byte IconType { get; set; }
        public int ToTop { get; set; }
        public int ToLeft { get; set; }
        public int Importance { get; set; }   

        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class ShopeListVM
    {
 
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<ShopeBVM> Shopes { get; set; }
    }

    public class ShopeIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "ShopName")]
        public string Name { get; set; }
      
        [Display(ResourceType = typeof(Labels), Name = "ToLeft")]
        [Range(0, 100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Validations))]
        public int  ToLeft { get; set; }

        [Range(0,100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "ToTop")]
        public int ToTop { get; set; }
     

        [Display(ResourceType = typeof(Labels), Name = "Importance")]   
        public int Importance { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "IconType")]
        public byte IconType { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }
    #endregion
}
