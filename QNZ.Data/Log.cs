using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Log")]
    public partial class Log
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Application { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Logged { get; set; }
        [Required]
        [StringLength(50)]
        public string Level { get; set; }
        [Required]
        public string Message { get; set; }
        [StringLength(250)]
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
    }
}