using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("District")]
    public partial class District
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public int CityId { get; set; }
        public int? Sort { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("Districts")]
        public virtual City City { get; set; }
    }
}