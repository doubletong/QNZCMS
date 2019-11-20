using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNZ.Data.Enums
{
    public enum MenuType : short
    {
        [Display(Name = "页面")]
        PAGE = 1,      
        [Display(Name = "操作")]
        ACTION = 2,
        [Display(Name = "无链接")]
        NOLINK = 3,
        [Display(Name = "外链")]
        OUTLINK = 4
    }
}
