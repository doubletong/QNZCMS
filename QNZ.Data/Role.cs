using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            RoleMenus = new HashSet<RoleMenu>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsSys { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}