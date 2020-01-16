using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "HomeClients")]
    public class HomeClientsComponent: ViewComponent
    {
      
        private readonly YicaiyunContext _context;
        public HomeClientsComponent(YicaiyunContext context)
        {
            _context = context;
        }
        

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Clients
                .Where(d=>d.Recommend==true && d.Active==true)
                .OrderByDescending(d=>d.Importance).ToListAsync();
            return View("Default",items);
        }
               
    }
}