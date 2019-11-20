
using System.Collections.Generic;
using QNZ.Data;

namespace QNZ.Model.Admin.ViewModel.Menus
{
    public class MoveMenuVM
    {
        public int Id { get; set; }
        public int? CurrentParentId { get; set; }
        public IEnumerable<Menu> Menus { get; set; }

        public int CategoryId { get; set; }

    }
}
