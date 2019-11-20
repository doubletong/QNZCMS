using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("MenuCategory")]
    public partial class MenuCategory
    {
        public MenuCategory()
        {
            Menus = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public bool Active { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public int Importance { get; set; }
        public bool IsSys { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Menu> Menus { get; set; }
    }
}