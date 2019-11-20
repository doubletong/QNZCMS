﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Recipes = new HashSet<Recipe>();
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreateDate { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        public short Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActivityDate { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        public string PasswordHash { get; set; }
        [StringLength(150)]
        public string PhotoUrl { get; set; }
        [Column("QQ")]
        [StringLength(50)]
        public string Qq { get; set; }
        [StringLength(50)]
        public string RealName { get; set; }
        public string SecurityStamp { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Recipe> Recipes { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}