using QNZ.Resources.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class StoreVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "StoreName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Contact")]
        public string Contact { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Address")]
        public string Address { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Province")]
        public string Province { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "District")]
        public string District { get; set; }     
        [Display(ResourceType = typeof(Labels), Name = "Coordinate")]
        public string Coordinate { get; set; }
        
        public bool  HasData { get; set; }



    }
}
