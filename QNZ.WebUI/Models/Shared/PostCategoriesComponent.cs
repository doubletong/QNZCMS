using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Front.ViewModel;


namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "PostCategories")]
    public class PostCategoriesComponent: ViewComponent
    {
      
        private readonly YicaiyunContext _context;
        public PostCategoriesComponent(YicaiyunContext context)
        {
            _context = context;
        }
        

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var items = await _context.PostCategories.AsNoTracking()
                .Where(d=>d.Active==true)
                .OrderByDescending(d=>d.Importance).ToListAsync();

            var vm = new PostCategoryListVM
            {
                Categories = items,
                CurrentId = id
            };
            return View("Default",vm);
        }
               
    }
}