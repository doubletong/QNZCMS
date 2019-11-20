using System.Collections.Generic;
using QNZ.Data;

namespace QNZ.Model.Admin.ViewModel.Menus
{
    public class LeftNavVM
    {     
        public IEnumerable<Menu> Menus { get; set; }
        public Menu CurrentMenu { get; set; }
    }
}
