using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class SolutionBVM
    {
            public int Id { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "Title")]
            public string Title { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "Description")]
            public string Description { get; set; }
            [Display(ResourceType = typeof(Labels), Name = "WorkCount")]
            public int WorkCount { get; set; }
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
    public class SolutionIM
    {

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        [StringLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "FontIcon")]
        public string FontIcon { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
}
