using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    #region view model
    public class ProductVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        public string Thumbnail { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Stock")]
        public int Stock { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "SubTitle")]
        public string SubName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Price")]
        public decimal Price { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "OriginalPrice")]
        public decimal? OriginalPrice { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Category")]
        public string CategoryTitle { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Unit")]
        public string Unit { get; set; }
    }



    public class ProductDetailVM
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "FullImage")]
        public string FullImages { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "SubTitle")]
        public string SubName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Price")]
        public decimal Price { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        public string Summary { get; set; }


        public bool HasData { get; set; }

    }

 

    public class PageListProduct
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<ProductListVM> Products { get; set; }

    }

    public class ProductListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }   
        public string Unit { get; set; }
    }
    public class ProductDetailWithBuyersVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }

        public string VideoUrl { get; set; }
        public string Description { get; set; }
        public int StoreId { get; set; }
        public int Stock { get; set; }
      
        public string Unit { get; set; }
        public string StoreName { get; set; }
        public string StoreLogo { get; set; }
        public string FullImages { get; set; }
        public string[] Images
        {
            get
            {
                return !string.IsNullOrEmpty(FullImages) ? FullImages.Split('|') : null;
            }
        }

        public IEnumerable<Buyer> Buyers { get; set; }

        public int TotalBuyerCount { get; set; }
      
    }

    #endregion

    #region input model
    public class ProductIM
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "SubTitle")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string SubName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Price")]
        public decimal Price { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "OriginalPrice")]
        public decimal? OriginalPrice { get; set; }
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Specification")]
        public string Specification { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Stock")]
        public int Stock { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Unit")]
        public string Unit { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Store")]
        public int StoreId { get; set; }
        public string VideoUrl { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Description")]
        //[DataType(DataType.Html)]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Summary")]
        //[Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Summary { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Thumbnail")]
        [StringLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Thumbnail { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "FullImage")]
        [StringLength(500, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string FullImages { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Category")]
        public int[] CategoryIds { get; set; }

    }

    #endregion
}
