using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    public partial class CustomerRecipe
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }
        public int RecipesId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("RecipesId")]
        [InverseProperty("CustomerRecipes")]
        public virtual Recipe Recipes { get; set; }
    }
}