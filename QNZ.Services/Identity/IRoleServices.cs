using System;
using System.Collections.Generic;
using System.Text;

using QNZ.Data;

namespace QNZ.Services.Identity
{
    public interface IRoleServices
    {
        Role GetById(int id);
        Role GetByIdWithRoleMenu(int id, bool disableTracking = true);
        IEnumerable<Role> GetAll();
        IEnumerable<Role> GetRolesByUserId(Guid userId);
        void Update(Role role);
        void Create(Role role);
        void Delete(Role role);
        void SetRoleMenus(int RoleId, int[] menuId);
    }
}
