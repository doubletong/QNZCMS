﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("OrderComment")]
    public partial class OrderComment
    {
        [Key]
        public int OrderId { get; set; }
        public string Comment { get; set; }
        [Column(TypeName = "numeric(3, 1)")]
        public decimal Rating { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("OrderComment")]
        public virtual Order Order { get; set; }
    }
}