﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("MailingAddress")]
    public partial class MailingAddress
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Province { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [Required]
        [StringLength(250)]
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        public string Consignee { get; set; }
        [Required]
        [StringLength(11)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string ZipCode { get; set; }
        [StringLength(100)]
        public string OpenId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("OpenId")]
        [InverseProperty("MailingAddresses")]
        public virtual Customer Open { get; set; }
    }
}