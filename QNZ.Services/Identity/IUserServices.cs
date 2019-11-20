using System;
using System.Collections.Generic;
using System.Text;

using QNZ.Data;

namespace QNZ.Services.Identity
{
    public interface IUserServices
    {
        int CreateUser(string userName, string email, string password, string realName,string mobile);
        bool IsExistEmail(string email);
        bool IsExistEmail(string email, Guid id);
        bool IsExistUserName(string userName);
        //User SignIn(string username, string password);

        User GetById(Guid id);
        User GetByIdWidthUserRolos(Guid id);
        void Update(User user);
        void SetRole(Guid userId, int[] roleId);
        bool SetPassword(Guid userId, string password);
        bool Delete(User user);
        //IPagedList<User> GetPagedElements(int pageIndex, int pageSize, string keyword, DateTime? startDate,
        //    DateTime? endDate,
        //    int? roleId, out int count);
    }
}
