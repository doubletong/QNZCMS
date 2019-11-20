﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Work")]
    public partial class Work
    {
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public string Body { get; set; }
        [StringLength(300)]
        public string Abstract { get; set; }
        public int? FinishYear { get; set; }
        [StringLength(150)]
        public string Thumbnail { get; set; }
        [Required]
        [StringLength(150)]
        public string Imageurl { get; set; }
        [StringLength(150)]
        public string Demourl { get; set; }
        public int Viewcount { get; set; }
        [StringLength(100)]
        public string Keywords { get; set; }
        public bool? Recommend { get; set; }
        public bool Active { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public int SolutionId { get; set; }
        public int? ClientId { get; set; }

        [ForeignKey("ClientId")]
        [InverseProperty("Works")]
        public virtual Client Client { get; set; }
        [ForeignKey("SolutionId")]
        [InverseProperty("Works")]
        public virtual Solution Solution { get; set; }
    }
}