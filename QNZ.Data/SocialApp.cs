﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class SocialApp
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Account { get; set; }
        [StringLength(150)]
        public string Url { get; set; }
        [StringLength(150)]
        public string Icon { get; set; }
        [Column("QRCode")]
        [StringLength(150)]
        public string Qrcode { get; set; }
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
    }
}