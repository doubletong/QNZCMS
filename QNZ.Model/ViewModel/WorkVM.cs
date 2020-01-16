using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    public class WorkBVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "FinishYear")]
        public int FinishYear { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Abstract { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "ViewCount")]
        public int ViewCount { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Demourl")]
        public string Demourl { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Solution")]
        public string SolutionTitle { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ClientName")]
        public string ClientName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]
        public bool Recommend { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedBy")]
        public string CreatedBy { get; set; }



    }

    public class WorkPageVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
        public int? SolutionId { get; set; }
        public StaticPagedList<WorkBVM> Works { get; set; }
    }

    public class WorkIM:PageMetaIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "FinishYear")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^-?\d*$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormat")]
        public int FinishYear { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public string Body { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Abstract { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Solution")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int SolutionId { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Client")]
        public int? ClientId { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Demourl")]
        [Url(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormat")]
        public string Demourl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]
        public bool Recommend { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }

    }
}
