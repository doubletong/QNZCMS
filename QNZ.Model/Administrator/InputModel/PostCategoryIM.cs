using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QNZ.Model.ViewModel;
using QNZ.Resources.Common;

namespace QNZ.Model.Administrator.InputModel
{
    public class PostCategoryIM : PageMetaIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        [StringLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsAliasUnique", "PostCategories", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
}