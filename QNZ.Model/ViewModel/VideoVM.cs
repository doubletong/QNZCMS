using QNZ.Resources.Admin;
using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace QNZ.Model.ViewModel
{
    #region Front
   
    public class VideoVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public string VideoUrl { get; set; }
   
    }
   

    public class VideoPageVM
    {

        public int? Type { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<VideoVM> Videos { get; set; }
    }

    

    public class VideoDetailVM
    {
        public Video VideoDetail { get; set; }
        public Video VideoPrev { get; set; }
        public Video VideoNext { get; set; }
    }
    #endregion


    #region Back


    public class VideoBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }

        public string VideoUrl { get; set; }
        public string Description { get; set; }
        public int  Importance { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }      
        public bool Recommend { get; set; }

    }
    public class VideoListVM
    {
   
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<VideoBVM> Videos { get; set; }
    }

    public class VideoIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Cover")]
        public string Cover { get; set; }



        [StringLength(250, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Discription { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "VideoUrl")]
        public string VideoUrl { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Recommend")]
        public bool Recommend { get; set; }

    }
    #endregion
}
