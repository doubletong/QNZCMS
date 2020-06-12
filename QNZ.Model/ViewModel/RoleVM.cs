﻿using QNZ.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class RoleIM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "RoleName")]
        public string RoleName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
    }
}
