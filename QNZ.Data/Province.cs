using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Province")]
    public partial class Province
    {
        public Province()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int? Sort { get; set; }
        [StringLength(50)]
        public string Remark { get; set; }

        [InverseProperty("Province")]
        public virtual ICollection<City> Cities { get; set; }
    }
}