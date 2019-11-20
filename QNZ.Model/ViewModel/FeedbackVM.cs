using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class FeedbackVM
    {
        public int Id { get; set; }
 
        [Display(ResourceType = typeof(Labels), Name = "FeedbackType")]
        public string FeedbackTypeTitle { get; set; }
       
        [Display(ResourceType = typeof(Labels), Name = "Message")]
        public string Message { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Mobile")]
        public string Mobile { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        public string Avatar { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Customer")]
        public string CustomerName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Image")]
        public string ImageUrls { get; set; }
    }
}
