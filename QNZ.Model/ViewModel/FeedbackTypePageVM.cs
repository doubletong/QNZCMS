using PagedList.Core;
using QNZ.Model.Admin.InputModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class FeedbackTypePageVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
    
        public int TotalCount { get; set; }
        public StaticPagedList<FeedbackTypeVM> FeedbackTypes { get; set; }

        public FeedbackTypeIM FeedbackType { get; set; }
    }
}
