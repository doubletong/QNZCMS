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
using PagedList.Core;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class WorksController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public WorksController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: Admin/Works
        public async Task<IActionResult> Index(int? page, int? solutionId)
        {
            var vm = new WorkPageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                SolutionId = solutionId

            };
            const int pageSize = 12;

            var query = _context.Works.Include(d => d.Solution).Include(d=>d.Client).AsNoTracking().AsQueryable();

            if (vm.SolutionId > 0)
            {
                query = query.Where(d => d.SolutionId == vm.SolutionId);
            }

            vm.TotalCount = await query.CountAsync();
            var works = await query.OrderByDescending(d => d.Id)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<WorkBVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            vm.Works = new StaticPagedList<WorkBVM>(works, vm.PageIndex, pageSize, vm.TotalCount);

            ViewData["Solutions"] = new SelectList(await _context.Solutions.AsNoTracking()
                .OrderByDescending(d => d.Importance).ToListAsync(), "Id", "Title");

            return View(vm);
        }

        // GET: Admin/Works/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Works
                .Include(w => w.Client)
                .Include(w => w.Solution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // GET: Admin/Works/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients.OrderByDescending(d => d.Importance), "Id", "ClientName");
            ViewData["SolutionId"] = new SelectList(_context.Solutions.OrderByDescending(d=>d.Importance), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkIM work)
        {
           

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            try
            {
                var model = _mapper.Map<Work>(work);

                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;

                //model.Body = WebUtility.HtmlEncode(page.Body);

                _context.Add(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Work));
                return Json(AR);

            }
            catch (Exception er)
            {
                AR.Setfailure(er.Message);
                return Json(AR);
            }
        }

        // GET: Admin/Works/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            var im = _mapper.Map<WorkIM>(work);
            ViewData["ClientId"] = new SelectList(_context.Clients.OrderByDescending(d => d.Importance), "Id", "ClientName");
            ViewData["SolutionId"] = new SelectList(_context.Solutions.OrderByDescending(d => d.Importance), "Id", "Title");
            return View(im);
        }

        // POST: Admin/Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkIM work)
        {
          

            if (id != work.Id)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {

                var model = await _context.Works.SingleOrDefaultAsync(d => d.Id == id);

                model = _mapper.Map(work, model);
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;


                _context.Update(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Post));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkExists(work.Id))
                {
                    AR.Setfailure(Messages.HttpNotFound);
                    return Json(AR);
                }
                else
                {
                    throw;
                }
            }
        }
        
        // POST: Admin/Works/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
  
            var post = await _context.Works.FirstOrDefaultAsync(d => d.Id == id);

            if (post == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Works.Remove(post);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool WorkExists(int id)
        {
            return _context.Works.Any(e => e.Id == id);
        }
    }
}
