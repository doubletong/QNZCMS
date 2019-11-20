using System;
using System.Collections.Generic;
using System.Text;
using QNZ.Data;

namespace QNZ.Services.Menus
{
    public interface IMenuCategoryServices
    {
        MenuCategory GetById(int id);
    }
}
