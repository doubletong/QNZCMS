using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("MobileCodeSet")]
    public partial class MobileCodeSet
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }
        [Required]
        [StringLength(6)]
        public string ValidateCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public bool IsUsed { get; set; }
    }
}