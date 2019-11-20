
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using QNZ.Data;

namespace QNZ.Services.Menus
{
    public interface IMenuServices
    {
        //List<Menu> GetFrontMenus(int categoryId);
        //Menu CreateAndSort(Menu menu);
        //// Menu UpdateAndSort(Menu menu);
        //void ResetSort(int categoryId);
        //Menu GetMenuWithChildMenus(int Id);
        //List<Menu> GetShowMenus(int categoryId);

        //Task<IEnumerable<Menu>> GetMenus(int categoryId, CancellationToken cancellationToken = default(CancellationToken));
        //int UpMoveMenu(int id);
        //int DownMoveMenu(int id);
        //List<Menu> GetFaltMenus(int categoryId);
        ////List<MenuVM> GetFaltMenus(int categoryId);
        //IEnumerable<Menu> GetLeftMenus(int categoryId);
        IEnumerable<Menu> GetMenusByCategoryId(int categoryId);
        IEnumerable<Menu> GetLevelMenusByCategoryId(int categoryId);
        Task<IEnumerable<Menu>> GetRolesMenusByUserId(Guid userId);
        Menu GetByIdWithChilds(int id);
        int UpMoveMenu(int id);
        int DownMoveMenu(int id);

        //List<Menu> CurrenMenuCrumbs(int categoryId, ViewContext viewContext);

        //Menu GetCurrenMenu(ViewContext viewContext);
        void ResetSort(int categoryId);

        Menu GetById(int id);
        bool Update(Menu menu);
        bool Create(Menu menu);
        bool Delete(Menu menu);
    }
}
