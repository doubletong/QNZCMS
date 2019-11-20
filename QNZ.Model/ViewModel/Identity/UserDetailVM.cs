using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using QNZ.Data.Enums;

namespace QNZ.Model.Admin.ViewModel.Identity
{
    public class UserDetailVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
       
        public string Email { get; set; }
       
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public string RealName { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime? Birthday { get; set; }

        public Gender Gender { get; set; }
        public string GenderName{ get; set; }
        public string QQ { get; set; }

        public string Mobile { get; set; }

        public string[] Roles { get; set; }
    }
}
