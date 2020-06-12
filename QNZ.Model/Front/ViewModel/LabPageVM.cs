using QNZ.Data;
using QNZ.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Front.ViewModel
{
    public class LabPageVM
    {
        public IEnumerable<PhotoVM> Photos { get; set; }
        public IEnumerable<ArticleVM> Articles { get; set; }
        public Page LabIntro { get; set; }
        public Page LabIntro2 { get; set; }

    }
}
