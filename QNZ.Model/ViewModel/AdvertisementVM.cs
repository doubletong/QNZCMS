using Microsoft.AspNetCore.Mvc;
using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    public class AdvertisementListVM
    {
        public int? SpaceId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<AdvertisementVM> Advertisments { get; set; }
    }

    public class AdvertisementVM
    {
     
        public int Id { get; set; }
  
        public string Title { get; set; }
        public string Description { get; set; }
 
        public string WebLink { get; set; }
    
        public string ImageUrl { get; set; }
      
        public string ImageUrlMobile { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        public int SpaceId { get; set; }
        public string SpaceTitle { get; set; }
        public string CreatedBy { get; set; }
    
        public DateTime CreatedDate { get; set; }

    }

    public class AdvertisementIM
    {
        [Key]
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "WebLink")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string WebLink { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string ImageUrl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageUrlMobile")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string ImageUrlMobile { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "AdvertisingSpace")]
        public int SpaceId { get; set; }
    }

    public class AdvertisingSpaceListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<AdvertisingSpaceVM> AdvertisingSpaces { get; set; }
    }

    public class AdvertisingSpaceVM
    {
     
        public int Id { get; set; }
        public string Title { get; set; }
        public string Sketch { get; set; }
        public int? Importance { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } 

    }
    public class AdvertisingSpaceIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Sketch")]
        public string Sketch { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Code")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsCodeUnique", "AdvertisingSpaces", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Code { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
}
