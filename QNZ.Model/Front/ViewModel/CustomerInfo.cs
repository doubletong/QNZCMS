using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Front.ViewModel
{
    public class CustomerInfo
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
        public string WechatId { get; set; }
        public string OpenId { get; set; }
        public string SessionKey { get; set; }
    }

    public class CustomerVM
    {
        public int Id { get; set; }
        public string Realname { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Workplace { get; set; }

    }
}
