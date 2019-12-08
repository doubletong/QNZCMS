using QNZ.Data;
using QNZ.Data.Enums;
using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    //class NavigationVM
    //{
    //}

    public class NavIM : PageMetaIM
    {
        public NavIM()
        {
            this.InverseParent = new HashSet<NavIM>();
            this.MenuType = MenuType.PAGE;
        }

        public int Id { get; set; }
        [Display(Name = "菜单名称")]
        [Required(ErrorMessage = "请输入菜单名称")]
        public string Title { get; set; }
        [Display(Name = "链接地址")]
        public string Url { get; set; }

        [Display(Name = "类型")]
        public MenuType MenuType { get; set; }

        [Display(Name = "图标")]
        public string Iconfont { get; set; }

        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入排序")]

        public int Importance { get; set; }

        [Display(Name = "激活")]
        public bool Active { get; set; }

        [Display(Name = "父级菜单")]
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<NavIM> InverseParent { get; set; }
    }

    public class MoveNavVM
    {
        public int Id { get; set; }
        public int? CurrentParentId { get; set; }
        public IEnumerable<Navigation> Navigations { get; set; }

        public int CategoryId { get; set; }

    }

    public class NavigationCategoryIM
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        [Display(ResourceType = typeof(Labels), Name = "Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Importance")]
        public int Importance { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Active")]
        public bool Active { get; set; }


    }
}
