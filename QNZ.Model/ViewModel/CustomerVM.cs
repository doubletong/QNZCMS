using QNZ.Data.Enums;
using QNZ.Resources.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class CustomerVM
    {
        [Key]
        [StringLength(100)]
        public string OpenId { get; set; }
       
        [Display(ResourceType = typeof(Labels), Name = "WechatId")]
        public string WechatNickName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        public string Mobile { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "LastLogin")]
        public DateTime? LastLogin { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Avatar")]
        public string Avatar { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }

    public class Buyer
    {
        public int Id { get; set; }
        public string WechatNickName { get; set; }
        public string Avatar { get; set; }
    }

    //mobile = "{\"phoneNumber\":\"15988716993\",\"purePhoneNumber\":\"15988716993\",\"countryCode\":\"86\",\"watermark\":{\"timestamp\":1534813515,\"appid\":\"wxe1836219c6242f6f\"}}"
    public class MobileVM
    {
        public string phoneNumber { get; set; }
        public string purePhoneNumber { get; set; }
        public string countryCode { get; set; }
    }
    public class watermark
    {
        public string timestamp { get; set; }
        public string appid { get; set; }
    }
}
