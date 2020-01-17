using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Front.ViewModel;

namespace QNZCMS.Controllers
{
    public class WorkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public WorkController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        // GET
        public async Task<IActionResult> Index(int year = 2018)
        {
            var query = _context.Works.AsNoTracking()
                .Where(d => d.Active);
           
            query = query.Where(d => d.FinishYear == year);
            
            var works = await query.OrderByDescending(d => d.Id).ToListAsync();
            ViewData["Year"] = year;
            return View(works);
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