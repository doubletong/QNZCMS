﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int StoreId { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("OrderDetails")]
        public virtual Order Order { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("OrderDetails")]
        public virtual Product Product { get; set; }
        [ForeignKey("StoreId")]
        [InverseProperty("OrderDetails")]
        public virtual Store Store { get; set; }
    }
}