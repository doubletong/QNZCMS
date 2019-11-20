using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            PcategoryProducts = new HashSet<PcategoryProduct>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string SubTitle { get; set; }
        [StringLength(150)]
        public string ImageUrl { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public int Importance { get; set; }
        public bool InMenu { get; set; }
        public bool Recommend { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<PcategoryProduct> PcategoryProducts { get; set; }
    }
}