using QNZ.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class ModuleSetIM
    {
        public ArticleSetIM ArticleSet { get; set; }
        public PostSetIM PostSet { get; set; }
        public SiteInfoIM SiteInfo { get; set; }
        public CaseSetIM CaseSet { get; set; }
        public PhotoSetIM PhotoSet { get; set; }
        public ProductSetIM ProductSet { get; set; }
        public WeiXinSetIM WeiXinSet { get; set; }
        public VideoSetIM VideoSet { get; set; }
    }

    public class VideoSetIM
    {
        [Display(ResourceType = typeof(Labels), Name = "PageSize")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int FrontPageSize { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbWidth { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Timer")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int Timer { get; set; }

    }

    public class WeiXinSetIM
    {

        public string Token { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string EncodingAESKey { get; set; }

    }
    public class ProductSetIM
    {
        [Display(ResourceType = typeof(Labels), Name = "PageSize")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int FrontPageSize { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbWidth { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "ImageHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ImageHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ImageWidth { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "CategoryImageHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int CategoryImageHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CategoryImageWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int CategoryImageWidth { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "EnableCaching")]
        public bool EnableCaching { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CacheDuration")]
        public int CacheDuration { get; set; }
    }

    public class PhotoSetIM
    {
        [Display(ResourceType = typeof(Labels), Name = "ThumbHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbWidth { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "EnableCaching")]
        public bool EnableCaching { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CacheDuration")]
        public int CacheDuration { get; set; }

    }

    public class CaseSetIM
    {
        [Display(ResourceType = typeof(Labels), Name = "PageSize")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int FrontPageSize { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbWidth { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "EnableCaching")]
        public bool EnableCaching { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CacheDuration")]
        public int CacheDuration { get; set; }


    }
    public class PostSetIM
    {
        [Display(ResourceType = typeof(Labels), Name = "PageSize")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int FrontPageSize { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbWidth { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "EnableCaching")]
        public bool EnableCaching { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CacheDuration")]
        public int CacheDuration { get; set; }
    }
    public class ArticleSetIM
    {
        [Display(ResourceType = typeof(Labels), Name = "PageSize")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int FrontPageSize { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ThumbWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ThumbWidth { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageWidth")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ImageWidth { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageHeight")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int ImageHeight { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "EnableCaching")]
        public bool EnableCaching { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CacheDuration")]
        public int CacheDuration { get; set; }
    }
    public class SiteInfoIM
    {
        [Display(ResourceType = typeof(Labels), Name = "SiteName")]
        public string SiteName { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "SiteDomainName")]
        //[Url(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormat")]
        public string SiteDomainName { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "WebNumber")]
        public string WebNumber { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "BaiduSiteID")]
        public string BaiduSiteID { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "EmailHr")]
        public string EmailHr { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddressInvalidFormat", ErrorMessage = null)]
        [StringLength(150, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "MaxLength")]
        public string MailTo { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "DashboardLogo")]
        public string DashboardLogo { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "LoginLogo")]
        public string LoginLogo { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "IsClose")]
        public bool IsClose { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CloseInfo")]
        public string CloseInfo { get; set; }
    }

    public class CompanyIM
    {
        [Display(ResourceType = typeof(Labels), Name = "CompanyName")]
        [StringLength(100, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Phone")]
        [StringLength(50, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "MaxLength")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Address")]
        [StringLength(250, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "MaxLength")]
        public string Address { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Fax")]
        [StringLength(50, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "MaxLength")]
        public string Fax { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        [StringLength(150, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "MaxLength")]
        public string Email { get; set; }
    }
}
