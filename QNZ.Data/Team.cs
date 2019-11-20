using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Team")]
    public partial class Team
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(150)]
        public string PhotoUrl { get; set; }
        [Required]
        [StringLength(100)]
        public string Post { get; set; }
        public string Description { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        [StringLength(100)]
        public string Twitter { get; set; }
        [Column("QQ")]
        [StringLength(50)]
        public string Qq { get; set; }
        [StringLength(100)]
        public string Facebook { get; set; }
        [StringLength(50)]
        public string Weixin { get; set; }
        [StringLength(100)]
        public string Github { get; set; }
        [StringLength(100)]
        public string CodePen { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
    }
}