using QNZ.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Front.ViewModel
{
    public class HistoryVM
    {
        public IEnumerable<MemorabiliaVM> Memorabilias { get; set; }
        public IEnumerable<int> Years { get; set; }
    }
}
