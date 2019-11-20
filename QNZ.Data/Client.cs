﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Client")]
    public partial class Client
    {
        public Client()
        {
            Works = new HashSet<Work>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string ClientName { get; set; }
        [Column("LogoURL")]
        [StringLength(150)]
        public string LogoUrl { get; set; }
        [StringLength(150)]
        public string Homepage { get; set; }
        public int Importance { get; set; }
        [Required]
        public bool? Active { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public bool Recommend { get; set; }

        [InverseProperty("Client")]
        public virtual ICollection<Work> Works { get; set; }
    }
}