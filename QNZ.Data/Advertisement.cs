﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class Advertisement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }
        [StringLength(150)]
        public string WebLink { get; set; }
        [StringLength(150)]
        public string ImageUrl { get; set; }
        [StringLength(150)]
        public string ImageUrlMobile { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        public int SpaceId { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(SpaceId))]
        [InverseProperty(nameof(AdvertisingSpace.Advertisements))]
        public virtual AdvertisingSpace Space { get; set; }
    }
}