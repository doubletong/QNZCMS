using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "AllTeams")]
    public class AllTeamsComponent:ViewComponent
    {
      
        private readonly YicaiyunContext _context;
        public AllTeamsComponent(YicaiyunContext context)
        {
            _context = context;
        }
        

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Teams.Where(d=>d.Active)
                .OrderByDescending(d=>d.Importance).ToListAsync();
            return View("Default",items);
        }
               
    }
}