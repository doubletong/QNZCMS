using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class MailingAddressVM
    {
        public int Id { get; set; }       
        public string Province { get; set; }      
        public string City { get; set; }       
        public string District { get; set; }     
        public string Address { get; set; }      
        /// <summary>
        /// 收件人
        /// </summary>
        public string Consignee { get; set; }       
        public string Mobile { get; set; }      
        public string ZipCode { get; set; }
        public bool Active { get; set; }
    }
}
