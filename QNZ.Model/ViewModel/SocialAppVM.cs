using QNZ.Resources.Common;
using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Data;

using X.PagedList;

namespace QNZ.Model.ViewModel
{
    #region Front
   
    public class SocialAppVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string QRCode { get; set; }
        public string Account { get; set; }
        public string Url { get; set; }
        public int Importance { get; set; }
        public string Icon { get; set; }

    }
   

    public class SocialAppPageVM
    {

        public int? Type { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<SocialAppVM> SocialApps { get; set; }
    }

    

    public class SocialAppDetailVM
    {
        public SocialApp SocialAppDetail { get; set; }
        public SocialApp SocialAppPrev { get; set; }
        public SocialApp SocialAppNext { get; set; }
    }
    #endregion


    #region Back


    public class SocialAppBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Qrcode { get; set; }
        public string Account { get; set; }
        public string Url { get; set; }
        public int Importance { get; set; }
        public string Icon { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class SocialAppListVM
    {
 
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<SocialAppBVM> SocialApps { get; set; }
    }

    public class SocialAppIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Account")]      
        public string  Account { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "QRCode")]
       
        public string Qrcode { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Url")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Url { get; set; }
   

        [Display(ResourceType = typeof(Labels), Name = "Icon")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Icon { get; set; }



        [Display(ResourceType = typeof(Labels), Name = "Importance")]   
        public int Importance { get; set; }


     

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }
    #endregion
}
