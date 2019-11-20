using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Front.InputModel
{
    public  class CustomerInfoIM
    {

        [StringLength(50)]
        public string Realname { get; set; }
        [StringLength(2)]
        public string Gender { get; set; }
        public int? Age { get; set; }
        [StringLength(100)]
        public string Workplace { get; set; }
    }

   
}
