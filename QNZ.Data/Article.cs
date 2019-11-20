﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class Article
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string Body { get; set; }
        [StringLength(50)]
        public string Author { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(250)]
        public string Keywords { get; set; }
        [StringLength(150)]
        public string Thumbnail { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Pubdate { get; set; }
        public int ViewCount { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int CategoryId { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Articles")]
        public virtual ArticleCategory Category { get; set; }
    }
}