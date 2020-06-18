using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class HomePageVM
    {
        public int ArticleCount { get; set; }
        public int ExhibitionCount { get; set; }
        public int DocumentCount { get; set; }
        public int VideoCount { get; set; }
        public int ProductCount { get; set; }
        public int JobCount { get; set; }
        public int BranchCount { get; set; }

        public int PageCount { get; set; }
        public int PhotoCount { get; set; }
        public int MemoCount { get; set; }
        public int StaffCount { get; set; }
    }

    public class PluginsPageVM
    {
     
        public int AdvertCount { get; set; }
        public int SocialCount { get; set; }
        public int NavCount { get; set; }
    


    }

}
