using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("FeedbackSet")]
    public partial class FeedbackSet
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string OpenId { get; set; }
        public int FeedbackTypeId { get; set; }
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(500)]
        public string ImageUrls { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }

        [ForeignKey("FeedbackTypeId")]
        [InverseProperty("FeedbackSets")]
        public virtual FeedbackTypeSet FeedbackType { get; set; }
        [ForeignKey("OpenId")]
        [InverseProperty("FeedbackSets")]
        public virtual Customer Open { get; set; }
    }
}