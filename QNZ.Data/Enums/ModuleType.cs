using System.ComponentModel.DataAnnotations;

namespace QNZ.Data.Enums
{
    public enum ModuleType : short
    {
        //[Display(ResourceType = typeof(Resources.ModuleNames), Name = "Menu")]
        [Display(Name = "菜单")]
        MENU = 1,
        [Display(Name = "新闻分类")]
        ARTICLECATEGORY = 7,
        [Display(Name = "新闻")]
        ARTICLE = 2,
        [Display(Name = "产品")]
        PRODUCT = 3,
        [Display(Name = "页面")]
        PAGE = 4,
        [Display(Name = "货品分类")]
        GOODSCATEGORY = 5,
        [Display(Name = "货品")]
        GOODS = 6,
        [Display(Name = "链接分类")]
        LINKCATEGORY = 8,
        [Display(Name = "视频")]
        VIDEO = 9,
        [Display(Name = "公告")]
        ANNOUNCEMENT = 9,
        [Display(Name = "相册")]
        ALBUM = 10,
        [Display(Name = "人才招聘")]
        JOB = 11,
        [Display(Name = "解决方案")]
        SOLUTION = 12,
        [Display(Name = "案例分类")]
        WORKTYPE = 13,
        [Display(Name = "案例")]
        WORK = 14,
        [Display(Name = "博客")]
        BLOG = 15,
        [Display(Name = "团队")]
        TEAM = 16,
        [Display(Name = "产品分类")]
        CATEGORY = 17,
        [Display(Name = "视频分类")]
        VIDEOCATEGORY = 18


    }
}
