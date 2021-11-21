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
    public class JobCategoryVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

    }
    public class JobVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Source { get; set; }
        public DateTime Pubdate { get; set; }
        public string Thumbnail { get; set; }
        public string SliderImage { get; set; }
    }
   

    public class JobPageVM
    {
        public int BranchId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<Job> Jobs { get; set; }
    }

    

    public class JobDetailVM
    {
        public Job JobDetail { get; set; }
        public Job JobPrev { get; set; }
        public Job JobNext { get; set; }
    }
    #endregion


    #region Back

    public class JobCategoryBVM
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
    public class JobCategoryList
    {
        public IEnumerable<JobCategoryBVM> Categories { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
    }
    public class JobCategoryIM:PageMetaIM
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
        [Remote("IsAliasUnique", "JobCategories", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }

    public class JobBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryTitle { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }     
        public string Department { get; set; }
        public string CompanyName { get; set; }
        public int Number { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
      
 

    }
    public class JobListVM
    {
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<JobBVM> Jobs { get; set; }
    }

    public class JobIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }



        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

       

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Address")]
        public string Address { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Department")]
        public string Department { get; set; }

       
        [Display(ResourceType = typeof(Labels), Name = "Number")]
        public int Number { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        public string HrEmail { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Company")]
        public int BranchId { get; set; }
        

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
       

    }
    #endregion
}
