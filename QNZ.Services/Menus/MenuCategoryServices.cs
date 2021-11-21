using System;
using System.Collections.Generic;
using System.Text;
using QNZ.Data;

namespace QNZ.Services.Menus
{
    public class MenuCategoryServices: IMenuCategoryServices
    {
        private readonly QNZContext _db;
        public MenuCategoryServices(QNZContext db)
        {
            _db = db;
        }
        
        public MenuCategory GetById(int id)
        {
            return _db.MenuCategories.Find(id);
        }
    }
}
