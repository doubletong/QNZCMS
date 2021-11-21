using System;
using System.ComponentModel.DataAnnotations;
using QNZ.Resources.Administrator;
using X.PagedList;

namespace QNZ.Model.Site.ViewModel
{
    public class LogVM
    {
        public int Id { get; set; }
      
        [Display(ResourceType = typeof(Logs), Name = "Message")]
        public string Message { get; set; }
        // [Display(ResourceType = typeof(Labels), Name = "MessageTemplate")]
        // public string MessageTemplate { get; set; }
        // [Display(ResourceType = typeof(Labels), Name = "Level")]
        // public string Level { get; set; }
        [Display(ResourceType = typeof(Logs), Name = "TimeStamp")]
        public DateTimeOffset TimeStamp { get; set; }
        // [Display(ResourceType = typeof(Labels), Name = "Exception")]
        // public string Exception { get; set; }
        // [Display(ResourceType = typeof(Labels), Name = "Properties")]
        // public SqlXml Properties { get; set; }
        [Display(ResourceType = typeof(Logs), Name = "LogEvent")]
        public string LogEvent { get; set; }
        [Display(ResourceType = typeof(Logs), Name = "UserName")]
        public string UserName { get; set; }
        public string IP { get; set; }
    }
    
    public class LogListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public string Sort { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<LogVM> Logs { get; set; }
    }
}