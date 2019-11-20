
using System;
using PagedList.Core;
using QNZ.Model.Admin.InputModel.Identity;
using QNZ.Data;

namespace QNZ.Model.Admin.ViewModel.Identity
{
    public class UserListVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string Keyword { get; set; }

        //[DateLessThan("EndDate", ErrorMessage = "开始日期必须小于截止日期")]
        public DateTime? StartDate { get; set; }
       
        public DateTime? EndDate { get; set; }
        public int? RoleId { get; set; }

        public StaticPagedList<User> Users { get; set; }

        public SetPasswordIM SetPasswordIM { get; set; }

    }
}
