using QNZ.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Front.ViewModel
{
    public class HomePageVM
    {
        public IEnumerable<SolutionVM> Solutions { get; set; }
        public IEnumerable<AdvertisementVM> Adverts { get; set; }
        public IEnumerable<ShopeVM> Shopes { get; set; }
        public VideoVM Video { get; set; }
    }

}
