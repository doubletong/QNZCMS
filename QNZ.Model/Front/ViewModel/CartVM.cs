using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNZ.Model.Front.ViewModel
{
    public class CartVM
    {
        public int Id { get; set; }
    
        public string OpenId { get; set; }
   
        public string Province { get; set; }
  
        public string City { get; set; }

        public string District { get; set; }

        public string Address { get; set; }

        public string Consignee { get; set; }
 
        public string Mobile { get; set; }
    
        public string ZipCode { get; set; }

        public List<CartItemVM> CartItems { get; set; }
        public decimal Amount {
            get {
                return CartItems.Sum(d => d.Quantity * d.Price);
            }
        }
    }

    public class CartItemVM
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public string ProductName { get; set; }
     
    }

    public class SetExpress
    {
        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Address { get; set; }

        public string Consignee { get; set; }

        public string Mobile { get; set; }

        public string ZipCode { get; set; }
    }
}
