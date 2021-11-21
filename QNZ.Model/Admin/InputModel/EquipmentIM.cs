using QNZ.Resources.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Admin.InputModel
{
    public class EquipmentIM
    {
        [Display(ResourceType = typeof(Labels), Name = "EquipmentId")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string EquipmentId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Labels), Name = "Agent")]
        public int AgentId { get; set; }
  
    }
}
