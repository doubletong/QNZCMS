using PagedList.Core;

namespace QNZ.Model.Admin.ViewModel
{
    public class FeedbackPageVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<FeedbackVM> Feedbacks { get; set; }
        
    }
}
