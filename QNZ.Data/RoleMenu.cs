using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("RoleMenu")]
    public partial class RoleMenu
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }

        [ForeignKey("MenuId")]
        [InverseProperty("RoleMenus")]
        public virtual Menu Menu { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("RoleMenus")]
        public virtual Role Role { get; set; }
    }
}