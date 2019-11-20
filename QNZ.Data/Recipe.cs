using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class Recipe
    {
        public Recipe()
        {
            CustomerRecipes = new HashSet<CustomerRecipe>();
            RecipesItems = new HashSet<RecipesItem>();
        }

        public int Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string CustomerMobile { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Recipes")]
        public virtual User User { get; set; }
        [InverseProperty("Recipes")]
        public virtual ICollection<CustomerRecipe> CustomerRecipes { get; set; }
        [InverseProperty("Recipes")]
        public virtual ICollection<RecipesItem> RecipesItems { get; set; }
    }
}