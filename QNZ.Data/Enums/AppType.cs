using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZ.Data.Enums
{
    public enum AppType : short
    {
        [Display(Name = "公众号")]
        GongZongHao = 1,      
        [Display(Name = "小程序")]
        XiaoChengXu = 2,
        [Display(Name = "后台")]
        Desktop = 3

    }
}
