using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("UserRole")]
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("UserRoles")]
        public virtual Role Role { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("UserRoles")]
        public virtual User User { get; set; }
    }
}