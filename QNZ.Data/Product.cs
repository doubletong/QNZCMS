using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            OrderDetails = new HashSet<OrderDetail>();
            PcategoryProducts = new HashSet<PcategoryProduct>();
            RecipesItems = new HashSet<RecipesItem>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string SubName { get; set; }
        [StringLength(100)]
        public string Specification { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? OriginalPrice { get; set; }
        [StringLength(150)]
        public string VideoUrl { get; set; }
        public string Description { get; set; }
        public int StoreId { get; set; }
        public int Stock { get; set; }
        [StringLength(50)]
        public string Unit { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        [StringLength(150)]
        public string Thumbnail { get; set; }
        [StringLength(500)]
        public string FullImages { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [ForeignKey("StoreId")]
        [InverseProperty("Products")]
        public virtual Store Store { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CartItem> CartItems { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<PcategoryProduct> PcategoryProducts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<RecipesItem> RecipesItems { get; set; }
    }
}