﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Post")]
    public partial class Post
    {
        public int Id { get; set; }
        [Required]
        [Column("title")]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(256)]
        public string Summary { get; set; }
        public string Body { get; set; }
        [StringLength(100)]
        public string Keywords { get; set; }
        public int Viewcount { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public bool? Recommend { get; set; }
        public bool? Active { get; set; }
        [StringLength(150)]
        public string Thumbnail { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Posts")]
        public virtual PostCategory Category { get; set; }
    }
}