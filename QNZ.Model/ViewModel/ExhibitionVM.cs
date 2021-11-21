using QNZ.Resources.Common;
using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    #region Front
   
    public class ExhibitionVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Address { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Thumbnail { get; set; }

    }
   

    public class ExhibitionPageVM
    {

        public int? Type { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<ExhibitionVM> Exhibitions { get; set; }
    }

    

    public class ExhibitionDetailVM
    {
        public Exhibition ExhibitionDetail { get; set; }
        public Exhibition ExhibitionPrev { get; set; }
        public Exhibition ExhibitionNext { get; set; }
    }
    #endregion


    #region Back


    public class ExhibitionBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Booth { get; set; }
        public string Thumbnail { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
      
        public bool Recommend { get; set; }

    }
    public class ExhibitionListVM
    {
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<ExhibitionBVM> Exhibitions { get; set; }
    }

    public class ExhibitionIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }



        [StringLength(250, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Content")]
        public string Body { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Booth")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Booth { get; set; }

        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Address")]
        public string Address { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "DateStart")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "DateEnd")]
        public DateTime DateEnd { get; set; }



        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]
        public bool Recommend { get; set; }

    }
    #endregion
}
