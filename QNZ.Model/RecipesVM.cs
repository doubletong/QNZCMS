using PagedList.Core;
using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model
{
    public class RecipeVM
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "UserName")]
        public string Username { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CustomerMobile")]
        public string CustomerMobile { get; set; }
    }

    public class RecipeIM
    {
        public int Id { get; set; }
    
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Description")]
        public string Description { get; set; }

        public List<RecipeItemIM> RecipeItems { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "CustomerMobile")]
        public string CustomerMobile { get; set; }
    }

    public partial class RecipeItemIM
    {
   
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

    }

    public class RecipePageVM
    {
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
        public int TotalCount { get; set; }
        public StaticPagedList<RecipeVM> Recipes { get; set; }
    }

    public partial class RecipesItemVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RecipesId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

    }

    public partial class CartItemVM
    {
       
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public string Unit { get; set; }

    }

    public partial class RecipesDetailVM
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
    
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }


        public string Username { get; set; }

        public decimal Amount { get; set; }
        public IList<RecipesItemVM> RecipesItems { get; set; }
       

    }
}
