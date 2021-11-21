using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;

namespace QNZCMS.Models.Shared
{
    
        [ViewComponent(Name = "HomeWorks")]
        public class HomeWorksComponent : ViewComponent
        {

            private readonly QNZContext _context;

            public HomeWorksComponent(QNZContext context)
            {
                _context = context;
            }


            public async Task<IViewComponentResult> InvokeAsync()
            {
                var items = await _context.Works
                    .Where(d => d.Recommend==true && d.Active)
                    .OrderByDescending(d => d.Id).Take(6).ToListAsync();
                return View("Default", items);
            }
        }
    
}