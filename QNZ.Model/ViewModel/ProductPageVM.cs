using PagedList.Core;
using QNZ.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
     public class ProductPageVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
   
        public int TotalCount { get; set; }
        public StaticPagedList<ProductVM> Products { get; set; }

    }
}
