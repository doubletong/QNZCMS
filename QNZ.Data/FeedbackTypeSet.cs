using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("FeedbackTypeSet")]
    public partial class FeedbackTypeSet
    {
        public FeedbackTypeSet()
        {
            FeedbackSets = new HashSet<FeedbackSet>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [InverseProperty("FeedbackType")]
        public virtual ICollection<FeedbackSet> FeedbackSets { get; set; }
    }
}