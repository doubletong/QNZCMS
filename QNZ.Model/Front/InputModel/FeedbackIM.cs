using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Front.InputModel
{
    public class FeedbackIM
    {    
        public int FeedbackTypeId { get; set; }
        [Required]
        [StringLength(maximumLength:500)]
        public string Message { get; set; }
        public string Mobile { get; set; }
        public string[] ImageUrls { get; set; }
    }
}
