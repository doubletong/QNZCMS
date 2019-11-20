using System.Collections.Generic;
using QNZ.Data;

namespace QNZ.Model.Admin.ViewModel.Identity
{
    public class SetRoleMenusVM
    {
        public int[] MenuIds { get; set; }
        public IEnumerable<Menu> Menus { get; set; }
        public int RoleId { get; set; }
    }
}
