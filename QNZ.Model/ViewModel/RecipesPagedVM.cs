using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class RecipesPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<RecipeFVM> Recipes { get; set; }
    }

    public class RecipeFVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Realname { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class RecipeDetailFVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CustomerMobile { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<RecipeItemVM> RecipeItems { get; set; }
        public decimal Amount { get; set; }
    }

    public class RecipeItemVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string  StoreName { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; }
        public string  Thumbnail { get; set; }
    }


}
