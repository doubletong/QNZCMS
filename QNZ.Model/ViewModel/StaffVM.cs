using QNZ.Resources.Common;
using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    #region Front
    public class OrganizationVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

    }
    public class StaffVM
    {

        public int Id { get; set; }
        public string Name { get; set; }     
        public string Post { get; set; }
        public string Referrer { get; set; }
        public string MasterTime { get; set; }
        public string Gender { get; set; }
    }
   

    public class StaffPageVM
    {
        public IEnumerable<StaffVM> RecommendStaffs { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<StaffVM> Staffs { get; set; }
    }

    

    public class StaffDetailVM
    {
        public Staff StaffDetail { get; set; }
        public IEnumerable<StaffVM> Staffs { get; set; }

    }
    #endregion


    #region Back

    public class OrganizationBVM
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
    public class OrganizationList
    {
        public IEnumerable<OrganizationBVM> Categories { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
    }
    public class OrganizationIM:PageMetaIM
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

    public class StaffBVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public string MasterTime { get; set; }
        public string Post { get; set; }
        public string Referrer { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }


    }
    public class StaffListVM
    {
        public int? OrganizationId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<StaffBVM> Staffs { get; set; }
    }

    public class StaffIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Name")]
        public string Name { get; set; }


        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Gender")]
        public string Gender { get; set; }

        [StringLength(250, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

       

        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Referrer")]  
        public string Referrer { get; set; }


        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "MasterTime")]
        public string MasterTime { get; set; }


        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Post")]
        public string Post { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]  
        public string Photo { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Organization")]
        public int OrganizationId { get; set; }

      
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
       

    }
    #endregion
}
