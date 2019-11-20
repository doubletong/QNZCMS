using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;


using QNZ.Services.Identity;
using QNZ.Data;

namespace QNZ.Services.Identity
{
    public class RoleServices:IRoleServices
    {
        private readonly YicaiyunContext _db;
        public RoleServices(YicaiyunContext db)
        {
            _db = db;
        }
        public Role GetById(int id)
        {
            return _db.Roles.Find(id);
        }
        public Role GetByIdWithRoleMenu(int id,bool disableTracking = true)
        {
            return  _db.Roles.Include(d => d.RoleMenus).FirstOrDefault(d => d.Id == id);
                
                //_unitOfWork.GetRepository<Role>().GetFirstOrDefault(predicate:d=>d.Id == id,include: d=>d.Include(r=>r.RoleMenus), disableTracking: disableTracking);
        }


        public IEnumerable<Role> GetAll()
        {
            var menus = _db.Roles.ToList(); // _unitOfWork.GetRepository<Role>().GetMany(disableTracking:false);
            return menus;
        }

        public IEnumerable<Role> GetRolesByUserId(Guid userId)
        {
            var menus = _db.Roles.Include(d => d.UserRoles).Where(d => d.UserRoles.Any(u => u.UserId == userId));
                
                //_unitOfWork.GetRepository<Role>().GetMany(predicate: d=>d.UserRoles.Any(u=>u.UserId == userId),include:d=>d.Include(m=>m.UserRoles));
            return menus;
        }

        public void SetRoleMenus(int RoleId, int[] menuId)
        {

            var rolemenus = _db.RoleMenus.Where(d => d.RoleId == RoleId).ToList();
            //_unitOfWork.GetRepository<RoleMenu>().GetMany(d => d.RoleId == RoleId);
            //vRole.RoleMenus.Clear();
            _db.RemoveRange(rolemenus);
            _db.SaveChanges();
            //_unitOfWork.GetRepository<RoleMenu>().Delete(rolemenus);
            //_unitOfWork.SaveChanges();
            if (menuId != null)
            {
                foreach (var mid in menuId)
                {
                    _db.RoleMenus.Add(new RoleMenu { RoleId = RoleId, MenuId = mid });
                    //_unitOfWork.GetRepository<RoleMenu>().Insert(new RoleMenu { RoleId = RoleId, MenuId = mid });
                }
            }
            _db.SaveChanges();

            //var key = $"{EntityNames.Menu}s";
            //_cacheService.Invalidate(key); //取消缓存

        }

        public void Update(Role role)
        {
            _db.Update(role);
          //  _db.Entry(role).State = EntityState.Modified;
            _db.SaveChanges();
            // _unitOfWork.GetRepository<Role>().Update(role);
            //_unitOfWork.SaveChanges();
        }

        public void Create(Role role)
        {
            _db.Add(role);
            _db.SaveChanges();
            // _unitOfWork.GetRepository<Role>().Insert(role);
            //_unitOfWork.SaveChanges();
        }
        public void Delete(Role role)
        {
            _db.Remove(role);
            _db.SaveChanges();
            //_unitOfWork.GetRepository<Role>().Delete(role);
            //_unitOfWork.SaveChanges();

        }
    }
}
