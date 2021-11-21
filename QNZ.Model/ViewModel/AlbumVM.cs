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
    public class AlbumVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

    }
    public class PhotoVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Album { get; set; }

    }
   

    public class PhotoPageVM
    {
        public IEnumerable<AlbumVM> Albums { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<PhotoVM> Photos { get; set; }
    }

    

    public class PhotoDetailVM
    {
        public Photo PhotoDetail { get; set; }
        public Photo PhotoPrev { get; set; }
        public Photo PhotoNext { get; set; }
    }
    #endregion


    #region Back

    public class AlbumBVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")] 
        public string Title { get; set; }
        public string Alias { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Cover")]
        public string Cover { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]     
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }
    public class AlbumList
    {
        public IEnumerable<AlbumBVM> Albums { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
    }
    public class AlbumIM
    {

        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Cover { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormatSEOUrl")]
        [Remote("IsAliasUnique", "Albums", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "IsExist")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
    }

    public class PhotoBVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string AlbumTitle { get; set; }   
        public DateTime CreatedDate { get; set; }     
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int Importance { get; set; }

    }
    public class PhotoListVM
    {
        public int? AlbumId { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public StaticPagedList<PhotoBVM> Photos { get; set; }
    }

    public class PhotoIM:PageMetaIM
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }


        [StringLength(250, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }


        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "ImageURL")]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Album")]
        public int AlbumId { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }

    }
    #endregion
}
