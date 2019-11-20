﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Cart")]
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OpenId { get; set; }
        [StringLength(50)]
        public string Province { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Consignee { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string ZipCode { get; set; }

        [InverseProperty("Cart")]
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}