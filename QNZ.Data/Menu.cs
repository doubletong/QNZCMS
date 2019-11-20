using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QNZ.Data
{
    [Table("Menu")]
    public partial class Menu
    {
        public Menu()
        {
            InverseParent = new HashSet<Menu>();
            RoleMenus = new HashSet<RoleMenu>();
        }

        public int Id { get; set; }
        public string Action { get; set; }
        public bool Active { get; set; }
        public string Area { get; set; }
        public int CategoryId { get; set; }
        public string Controller { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public string Iconfont { get; set; }
        public int Importance { get; set; }
        public bool IsExpand { get; set; }
        public int? LayoutLevel { get; set; }
        public short MenuType { get; set; }
        public int? ParentId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Url { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Menus")]
        public virtual MenuCategory Category { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("InverseParent")]
        public virtual Menu Parent { get; set; }
        [InverseProperty("Parent")]
        public virtual ICollection<Menu> InverseParent { get; set; }
        [InverseProperty("Menu")]
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}