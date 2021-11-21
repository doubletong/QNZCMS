using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.ViewModel;

namespace QNZCMS.Controllers
{
    public class WorkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        public WorkController(QNZContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        // GET
        public async Task<IActionResult> Index(int year = 2019)
        {
            var vm = new WorkPageFVM
            {
                Year = year,
                Years = await _context.Works.AsNoTracking().Where(d=>d.Active)                    
                    .Select(d=>d.FinishYear.Value).Distinct().OrderByDescending(c => c).ToListAsync(),
                Works = await _context.Works.AsNoTracking()
                    .Where(d => d.Active && d.FinishYear == year)
                    .OrderByDescending(d => d.Id)
                    .ProjectTo<WorkFVM>(_mapper.ConfigurationProvider).ToListAsync()

            };        
            
           
            //ViewData["Year"] = year;
            return View(vm);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var work = await _context.Works.Include(d => d.Solution).FirstOrDefaultAsync(d => d.Id == id && d.Active);
            if (work == null)
                return NotFound();

            work.ViewCount++;
            _context.Update(work);
            await _context.SaveChangesAsync();

            var vm = new WorkDetailVM
            {
                WorkDetail = work,
                WorkPrev = await _context.Works.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < work.Id && d.Active),
                WorkNext = await _context.Works.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > work.Id && d.Active)
            };
            return View(vm);
        }
    }
}