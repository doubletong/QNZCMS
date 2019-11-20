using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SIG.Basic.Extensions;
using SIG.Infrastructure.Helper;

using QNZ.Data;

namespace QNZ.Services.Identity
{
    public class UserServices :IUserServices
    {
        private readonly YicaiyunContext _db;
        public UserServices(YicaiyunContext db)
        {
            _db = db;
        }

        //public User SignIn(string username, string password)
        //{
        //    var user = _unitOfWork.GetRepository<User>().GetFirstOrDefault(predicate:u=>u.UserName == username || u.Mobile == username);
        //    if (user == null) return null;

        //    var salt = Convert.FromBase64String(user.SecurityStamp);
        //    var pwdHash = Hash.HashPasswordWithSalt(password, salt);

        //    return user.PasswordHash == pwdHash ? user : null;
          
        //}
        public int CreateUser(string userName, string email, string password, string realName, string mobile)
        {
            
            if (IsExistEmail(email))
            {
                return 1; //1 邮箱已存在
            }
           
            if (IsExistUserName(userName))
            {
                return 2; //1 用户名已存在
            }
            if (IsExistMobile(mobile))
            {
                return 3; //1 手机号已存在
            }


            var securityStamp = Hash.GenerateSalt();
            var passwordHash = Hash.HashPasswordWithSalt(password, securityStamp);

            var newUser = new User()
            {
                UserName = userName,
                RealName = realName,
                Email = email,
                SecurityStamp = Convert.ToBase64String(securityStamp),
                PasswordHash = passwordHash,
                CreateDate = DateTime.Now,
                IsActive = true,
                Mobile = mobile
            };

            //_logger.LogInformation(string.Format(Logs.CreateMessage, EntityNames.User, userName));
            _db.Add(newUser);
            _db.SaveChanges();

            //_unitOfWork.GetRepository<User>().Insert(newUser);
            //_unitOfWork.SaveChanges();
            // SetUserCookies(false, newUser);

            return 0;

        }

        public bool IsExistEmail(string email)
        {
           var result = _db.Users.Count(u => u.Email == email);  
           return result > 0;
        }
        public bool IsExistMobile(string mobile)
        {
            var result = _db.Users.Count(u => u.Mobile == mobile);
            //_unitOfWork.GetRepository<User>().Count(u => u.Mobile == mobile);
            return result > 0;
        }
        public bool IsExistEmail(string email, Guid id)
        {
            var result = _db.Users.Count(u => u.Email == email && u.Id != id);
                // _unitOfWork.GetRepository<User>().Count(u => u.Email == email && u.Id!=id);
            return result > 0;
        }
        public bool IsExistMobile(string mobile, Guid id)
        {
            var result = _db.Users.Count(u => u.Mobile == mobile && u.Id != id);
            //_unitOfWork.GetRepository<User>().Count(u => u.Mobile == mobile && u.Id != id);
            return result > 0;
        }

        public bool IsExistUserName(string userName)
        {
            var result = _db.Users.Count(u => u.UserName == userName);
            //_unitOfWork.GetRepository<User>().Count(u => u.UserName == userName);
            return result > 0;
        }

        public User GetById(Guid id)
        {
            return _db.Users.Find(id);
                //_unitOfWork.GetRepository<User>().Find(id);
        }

        public User GetByIdWidthUserRolos(Guid Id)
        {
            return _db.Users.Include(d=>d.UserRoles).FirstOrDefault(d => d.Id == Id);
                
                //_unitOfWork.GetRepository<User>()
                //.GetFirstOrDefault(predicate: d => d.Id == id, include: d => d.Include(u => u.UserRoles));
        }

        public void Update(User user)
        {
            _db.Update(user);
            _db.SaveChanges();
        }

        public void SetRole(Guid userId, int[] roleId)
        {
            // var user = GetById(userId);
            var userRoles = _db.UserRoles.Where(d => d.UserId == userId).ToList();
                //_unitOfWork.GetRepository<UserRole>().GetMany(r => r.UserId == userId);
            _db.Remove(userRoles);
        
            foreach (var item in roleId)
            {
                _db.UserRoles.Add(new UserRole{UserId = userId,RoleId = item});
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SetPassword(Guid userId, string password)
        {
            var user = GetById(userId);
            //try
            //{

            var securityStamp = Hash.GenerateSalt();
            var pwdHash = Hash.HashPasswordWithSalt(password, securityStamp);

            user.SecurityStamp = Convert.ToBase64String(securityStamp);
            user.PasswordHash = pwdHash;
            this.Update(user);

            //log 

            //_logger.Info(string.Format(Logs.RestPwdMessage, user.EntityName, user.UserName));
            return true;
            //}
            //catch (Exception er)
            //{
            //    var message = String.Format(Logs.ErrorRestPwdMessage, user.EntityName);
            //    _logger.Error(message, er);
            //    return false;
            //}



        }
        public bool Delete(User user)
        {
             _db.Remove(user);
          _db.SaveChanges();
            return true;
        }

        //public IPagedList<User> GetPagedElements(int pageIndex, int pageSize, string keyword, DateTime? startDate, DateTime? endDate,
        //    int? roleId, out int count)
        //{
            
        //    Expression<Func<User, bool>> expression = d => true;

        //    if (!string.IsNullOrEmpty(keyword))
        //        expression = expression.AndAlso(g => g.UserName.Contains(keyword));
        //    if (startDate != null)
        //        expression = expression.AndAlso(m => m.CreateDate >= startDate);
        //    if (endDate != null)
        //        expression = expression.AndAlso(m => m.CreateDate <= endDate);
        //    if (roleId > 0)
        //        expression = expression.AndAlso(g => g.UserRoles.Any(m => m.RoleId == (int)roleId));

           
        //    count = _db.Users.Count(expression);

        //    var result = _unitOfWork.GetRepository<User>().GetPagedList(predicate: expression, 
        //        orderBy: d => d.OrderByDescending(l => l.CreateDate), 
        //        pageIndex: pageIndex, pageSize: pageSize);
            

        //    return result;

        //}

    }
}
