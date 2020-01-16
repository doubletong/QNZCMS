using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class SolutionsController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public SolutionsController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Solutions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Solutions.Include(d=>d.Works)
                .OrderByDescending(d=>d.Importance)
                .ProjectTo<SolutionBVM>(_mapper.ConfigurationProvider).ToListAsync());
        }

        // GET: Admin/Solutions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solution = await _context.Solutions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solution == null)
            {
                return NotFound();
            }

            return View(solution);
        }

        // GET: Admin/Solutions/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( SolutionIM solution)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            var model = _mapper.Map<Solution>(solution);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Identity.Name;

            _context.Add(model);
            await _context.SaveChangesAsync();


            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Solution));
            return Json(AR);
        }

        // GET: Admin/Solutions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solution = await _context.Solutions.FindAsync(id);
            if (solution == null)
            {
                return NotFound();
            }
            var im = _mapper.Map<SolutionIM>(solution);
            return View(im);

        }

        // POST: Admin/Solutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SolutionIM im)
        {
         
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            if (id != im.Id)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }


            try
            {
                var model = await _context.Solutions.FindAsync(id);

                model = _mapper.Map(im, model);
                model.UpdatedBy = User.Identity.Name;
                model.UpdatedDate = DateTime.Now;

                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolutionExists(im.Id))
                {
                    AR.Setfailure(Messages.HttpNotFound);
                    return Json(AR);
                }
                else
                {
                    AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.Solution));
                    return Json(AR);
                }
            }
            //  return RedirectToAction(nameof(Index));

            AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Solution));
            return Json(AR);
        }



        // POST: Admin/Solutions/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = await _context.Solutions.Include(d=>d.Works).FirstOrDefaultAsync(d => d.Id == id);

            if (s == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            if (s.Works.Any())
            {
                AR.Setfailure(Messages.AlertDeleteFailureHasChild);
                return Json(AR);
            }

            _context.Solutions.Remove(s);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool SolutionExists(int id)
        {
            return _context.Solutions.Any(e => e.Id == id);
        }
    }
}
