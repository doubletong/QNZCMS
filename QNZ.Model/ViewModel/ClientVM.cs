using QNZ.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    public class ClientBVM
    {
  
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "ClientName")]
        public string ClientName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Logo")]
        public string LogoURL { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Homepage")]
        public string Homepage { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]
        public bool Recommend { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedBy")]
        public string CreatedBy { get; set; }


    }
    public class ClientListVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
      
        public StaticPagedList<ClientBVM> Clients { get; set; }
    }

    public class ClientIM
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Name")]
        public string ClientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Logo")]
        public string LogoURL { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Homepage")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Homepage { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]
        public bool Recommend { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }
}
