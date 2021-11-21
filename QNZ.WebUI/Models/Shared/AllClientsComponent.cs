using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;

namespace QNZCMS.Models.Shared
{


    [ViewComponent(Name = "AllClients")]
    public class AllClientsComponent : ViewComponent
    {

        private readonly QNZContext _context;

        public AllClientsComponent(QNZContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Clients
                .Where(d => d.Recommend==true && d.Active==true)
                .OrderByDescending(d => d.Id).Take(6).ToListAsync();
            return View("Default", items);
        }
    }
}