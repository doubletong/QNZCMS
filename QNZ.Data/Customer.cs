﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Customer")]
    public partial class Customer
    {
        public Customer()
        {
            FeedbackSets = new HashSet<FeedbackSet>();
            MailingAddresses = new HashSet<MailingAddress>();
            Orders = new HashSet<Order>();
        }

        [Key]
        [StringLength(100)]
        public string OpenId { get; set; }
        [StringLength(50)]
        public string WechatNickName { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        public bool Active { get; set; }
        [StringLength(250)]
        public string Avatar { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [InverseProperty(nameof(FeedbackSet.Open))]
        public virtual ICollection<FeedbackSet> FeedbackSets { get; set; }
        [InverseProperty(nameof(MailingAddress.Open))]
        public virtual ICollection<MailingAddress> MailingAddresses { get; set; }
        [InverseProperty(nameof(Order.Open))]
        public virtual ICollection<Order> Orders { get; set; }
    }
}