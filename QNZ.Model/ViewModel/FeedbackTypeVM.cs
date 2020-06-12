using QNZ.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class FeedbackTypeVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]      
        public string Title { get; set; }
        public bool HasData { get; set; }
    }
}
