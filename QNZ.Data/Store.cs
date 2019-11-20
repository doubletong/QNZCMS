﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Store")]
    public partial class Store
    {
        public Store()
        {
            CartItems = new HashSet<CartItem>();
            OrderDetails = new HashSet<OrderDetail>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Contact { get; set; }
        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        [Required]
        [StringLength(250)]
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        public string Province { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(150)]
        public string Thumbnail { get; set; }
        [Column(TypeName = "decimal(10, 6)")]
        public decimal Longitude { get; set; }
        [Column(TypeName = "decimal(10, 6)")]
        public decimal Latitude { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public string Body { get; set; }

        [InverseProperty("Store")]
        public virtual ICollection<CartItem> CartItems { get; set; }
        [InverseProperty("Store")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty("Store")]
        public virtual ICollection<Product> Products { get; set; }
    }
}