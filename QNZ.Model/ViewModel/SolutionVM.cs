using QNZ.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{

    #region 前台视图
    public class SolutionVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }
      
        public string ImageUrl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }

   

    #endregion
    public class SolutionBVM
    {
            public int Id { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "Title")]
            public string Title { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "Description")]
            public string Description { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
            public string Thumbnail { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "Importance")]
            public int Importance { get; set; }

            [Display(ResourceType = typeof(Labels), Name = "Active")]
            public bool Active { get; set; }

            [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
            public DateTime CreatedDate { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "CreatedBy")]
            public string CreatedBy { get; set; }
        
    }
    public class SolutionListVM
    {
        public IEnumerable<SolutionBVM> Solutions { get; set; }
        public string Keyword { get; set; }
    }
    public class SolutionIM : PageMetaIM
    {

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "SubTitle")]
        public string SubTitle { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Description")]
        [StringLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        public string ImageUrl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]        

        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Content")]
        public string Body { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "RelatedProducts")]
        public string RelatedProducts { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "RelatedProducts")]
        public string[] Products { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
}
