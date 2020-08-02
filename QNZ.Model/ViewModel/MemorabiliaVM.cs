using Microsoft.AspNetCore.Mvc;
using QNZ.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
   public  class MemorabiliaVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Year")]
        public short Year { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Month")]
        public short? Month { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Date")]
        public short? Date { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }     
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

    }
    public class MemorabiliaIM 
    {
        public int Id { get; set; }
      
        [Display(ResourceType = typeof(Labels), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Description { get; set; }        

        [Display(ResourceType = typeof(Labels), Name = "Year")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "RequiredSelect")]
        public short Year { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Month")]

        public short? Month { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Date")]

        public short? Date { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }

    }

    public class MemorabiliaListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<MemorabiliaVM> Memorabilias { get; set; }
    }

}
