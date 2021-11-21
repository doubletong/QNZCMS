using System.ComponentModel.DataAnnotations;
using QNZ.Model.ViewModel;
using QNZ.Resources.Common;

namespace QNZ.Model.Administrator.InputModel
{
    public class PostIM:PageMetaIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public string Body { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Summary { get; set; }    
        
        [Display(ResourceType = typeof(Labels), Name = "Category")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
        
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]   
        public string Thumbnail { get; set; }   
        
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]   
        public bool Recommend { get; set; }      
        
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }

    }
}