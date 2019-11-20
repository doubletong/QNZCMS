using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
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
    public class ClientsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ClientsController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Admin/Clients
        public async Task<IActionResult> Index(string keyword, int? page)
        {
            var vm = new ClientListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword
            };
            var pageSize = 10;

            var query = _context.Clients.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.ClientName.Contains(keyword));


            vm.TotalCount = await query.CountAsync();
            var clients = await query.OrderByDescending(d=>d.Importance)
                .ProjectTo<ClientBVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            // var list = _mapper.Map<IEnumerable<ProductVM>>(agents);

            vm.Clients = new StaticPagedList<ClientBVM>(clients, vm.PageIndex, pageSize, vm.TotalCount);


            return View(vm);
        }

        // GET: Admin/Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Admin/Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ClientIM client)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            var model = _mapper.Map<Client>(client);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Identity.Name;

            _context.Add(model);
            await _context.SaveChangesAsync();


            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Client));
            return Json(AR);
        }

        // GET: Admin/Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            var im = _mapper.Map<ClientIM>(client);

            return View(im);
        }

        // POST: Admin/Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientIM im)
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
                var model = await _context.Clients.FindAsync(id);

                model = _mapper.Map(im, model);
                model.UpdatedBy = User.Identity.Name;
                model.UpdatedDate = DateTime.Now;

                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(im.Id))
                {
                    AR.Setfailure(Messages.HttpNotFound);
                    return Json(AR);
                }
                else
                {
                    AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.Client));
                    return Json(AR);
                }
            }
        

            AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Client));
            return Json(AR);
        }

        // GET: Admin/Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Admin/Clients/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _context.Clients.Include(d=>d.Works).FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            if (c.Works.Any())
            {
                AR.Setfailure(Messages.AlertDeleteFailureHasChild);
                return Json(AR);
            }

            _context.Clients.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
