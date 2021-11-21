using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Resources.Common;
using QNZ.Resources.Administrator;
using X.PagedList;

namespace QNZ.Model.Administrator.ViewModel
{
    public class WebpartVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Webparts), Name = "PositionCode")]
        public string PositionCode { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
    public class WebpartListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<WebpartVM> Webparts { get; set; }
    }
}