﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("PCategoryProducts")]
    public partial class PcategoryProduct
    {
        [Key]
        public int CategoryId { get; set; }
        [Key]
        public int ProductId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(ProductCategory.PcategoryProducts))]
        public virtual ProductCategory Category { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("PcategoryProducts")]
        public virtual Product Product { get; set; }
    }
}