using QNZ.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace QNZ.Model.ViewModel
{
    public class PageMetaIM
    {
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string SEOTitle { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string SEODescription { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Keywords")]     
        public string SEOKeywords { get; set; }
    }
}
