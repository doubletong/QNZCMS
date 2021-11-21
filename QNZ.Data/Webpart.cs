using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class Webpart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]

        [StringLength(100)]
        public string PositionCode { get; set; }
        
        public bool Active { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        /// <summary>
        /// 权重，值越高越排前
        /// </summary>
        public int Importance { get; set; }
    }
}
