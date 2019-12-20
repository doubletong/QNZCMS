using QNZ.Data;
using QNZ.Data.Enums;
using SIG.Resources.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Model.ViewModel
{
    public class MenuVM
    {

    }
    public class MoveMenuVM
    {
        public int Id { get; set; }
        public int? CurrentParentId { get; set; }
        public IEnumerable<Menu> Menus { get; set; }

        public int CategoryId { get; set; }

    }
    public class LeftNavVM
    {
        public IEnumerable<Menu> Menus { get; set; }
        public Menu CurrentMenu { get; set; }
    }
    public class MenuIM
    {
        public MenuIM()
        {
            this.ChildMenus = new HashSet<MenuIM>();
            this.MenuType = MenuType.PAGE;
        }

        public int Id { get; set; }
        [Display(Name = "菜单名称", Prompt = "菜单名称")]
        [Required(ErrorMessage = "请输入菜单名称")]
        public string Title { get; set; }
        [Display(Name = "链接地址", Prompt = "链接地址")]
        public string Url { get; set; }

        [Display(Name = "域")]
        public string Area { get; set; }
        [Display(Name = "控制器")]
        public string Controller { get; set; }
        [Display(Name = "操作")]
        public string Action { get; set; }

        [Display(Name = "类型", Prompt = "类型")]
        public MenuType MenuType { get; set; }

        [Display(Name = "图标")]
        public string Iconfont { get; set; }

        [Display(Name = "排序", Prompt = "排序")]
        [Required(ErrorMessage = "请输入排序")]

        public int Importance { get; set; }

        [Display(Name = "激活", Prompt = "激活")]
        public bool Active { get; set; }

        [Display(Name = "父级菜单")]
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<MenuIM> ChildMenus { get; set; }

        //public bool CanEdit
        //{
        //    get
        //    {
        //        return HttpContext.User.IsInRole("创始人");
        //    }
        //}

        //public bool IsOpen {
        //    get {
        //       return ChildMenus.Where(m => m.Area.Equals(Site.CurrentArea(), StringComparison.InvariantCultureIgnoreCase)
        //                && m.Controller.Equals(Site.CurrentController(), StringComparison.InvariantCultureIgnoreCase)
        //                && m.Action.Equals(Site.CurrentAction(), StringComparison.InvariantCultureIgnoreCase)
        //                && m.Active && m.MenuType == MenuType.PAGE).Any();
        //    }
        //}

        //public bool IsCurrent { get {
        //        return (Site.CurrentController().Equals(Controller, StringComparison.InvariantCultureIgnoreCase)
        //        && Site.CurrentAction().Equals(Action, StringComparison.InvariantCultureIgnoreCase)
        //        && Site.CurrentArea().Equals(Area, StringComparison.InvariantCultureIgnoreCase));
        //    } }

    }

    #region menu categories
    public class MenuCategoryVM
    {
        public int Id { get; set; }     
        public string Title { get; set; }   
        public int Importance { get; set; }    
        public bool Active { get; set; }
    }
    public class MenuCategoryIM
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
    #endregion
}
