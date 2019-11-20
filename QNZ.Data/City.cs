using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("City")]
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public int? Sort { get; set; }

        [ForeignKey("ProvinceId")]
        [InverseProperty("Cities")]
        public virtual Province Province { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<District> Districts { get; set; }
    }
}