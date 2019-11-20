using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class AgentVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "WechatId")]
        public string WechatId { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        public string Mobile { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "AgentName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Province")]
        public string Province { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "District")]
        public string District { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Principal")]
        public string Principal { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "EquipmentCount")]
        public int EquipmentCount { get; set; }

    }
}
