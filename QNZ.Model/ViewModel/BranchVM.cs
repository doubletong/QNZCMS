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
   
    public class BranchVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string Contact { get; set; }
        public string Homepage { get; set; }
        public string Business { get; set; }

    }
   

    public class BranchPageVM
    {

        public int? Type { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<BranchVM> Branchs { get; set; }
    }

    

    public class BranchDetailVM
    {
        public Branch BranchDetail { get; set; }
        public Branch BranchPrev { get; set; }
        public Branch BranchNext { get; set; }
    }
    #endregion


    #region Back


    public class BranchBVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Homepage { get; set; }
        public string Contact { get; set; }
        public string ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public int Importance { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }


    }
    public class BranchListVM
    {
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<BranchBVM> Branches { get; set; }
    }

    public class BranchIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "CompanyName")]
        public string Name { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        public string ImageUrl { get; set; }



        [StringLength(250, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Content")]
        public string Body { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Homepage")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Homepage { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Address")]
        public string Address { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Contact")]
        public string Contact { get; set; }

        [StringLength(300, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "MainBusiness")]
        public string Business { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }
    #endregion
}
