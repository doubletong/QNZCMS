using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("PCategoryProducts")]
    public partial class PcategoryProduct
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("PcategoryProducts")]
        public virtual ProductCategory Category { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("PcategoryProducts")]
        public virtual Product Product { get; set; }
    }
}