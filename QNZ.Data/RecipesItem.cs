using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("RecipesItem")]
    public partial class RecipesItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RecipesId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("RecipesItems")]
        public virtual Product Product { get; set; }
        [ForeignKey("RecipesId")]
        [InverseProperty("RecipesItems")]
        public virtual Recipe Recipes { get; set; }
    }
}