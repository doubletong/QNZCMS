using PagedList.Core;
using QNZ.Data.Enums;
using QNZ.Model.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class OrderPagedVM
    {
        public int PageIndex { get; set; }
        //public int? StoreId { get; set; }
        public string Mobile { get; set; }
        public byte? Status { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<OrderVM> Orders { get; set; }
    }

    public class OrderVM
    {
        public int Id { get; set; }
        public byte Status { get; set; }   
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Nickname { get; set; }
        public string CustomerMobile { get; set; }
        public ICollection<OrderDetailVM> OrderDetails { get; set; }
    }

    public class OrderPageFVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }       
        public IEnumerable<OrderFVM> Orders { get; set; }
    }
    public class OrderFVM
    {
        public int Id { get; set; }   
    
        public decimal Amount { get; set; }
        //public string Cashier { get; set; }
        public DateTime CreatedDate { get; set; }
        //public int CustomerId { get; set; }
        public IEnumerable<OrderDetailFVM> OrderDetails { get; set; }

        public string Mobile { get; set; }
   
        public short Status { get; set; }
    }

    public class OrderDetailFVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Amount
        {
            get
            {
                return Quantity * Price;
            }
        }
    }

    public class OrderDetailFullVM
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }
        //public string Cashier { get; set; }
        public DateTime CreatedDate { get; set; }
        //public int CustomerId { get; set; }
        public List<OrderDetailFVM> OrderDetails { get; set; }

        public string Remark { get; set; }
        public byte Status { get; set; }
      
        public string Province { get; set; }
    
        public string City { get; set; }
  
        public string District { get; set; }
  
        public string Address { get; set; }
     
        public string Consignee { get; set; }
     
        public string Mobile { get; set; }
    
        public string ZipCode { get; set; }

    }


    public class ConfirmOrder
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }    
        public string Mobile { get; set; }
        public string Remark { get; set; }
    }

    public class OrderingVM
    {
        public int[] ItemIds { get; set; }
        public string Remark { get; set; }
    }

    public class OrderRateVM
    {
        public int OrderId { get; set; }
        public string Comment { get; set; }
        public decimal Rating { get; set; }
    }


    public class OrderDetailVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Thumbnail { get; set; }
        public string ProductName { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }
}
