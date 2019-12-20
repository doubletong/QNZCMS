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
using QNZ.Data;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using SIG.Infrastructure.Configs;
using SIG.Infrastructure.Helper;
using SIG.Resources.Admin;
using X.PagedList;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class AdvertisingSpacesController : BaseController
    {
        private readonly YicaiyunContext _context;
        private readonly IMapper _mapper;
        public AdvertisingSpacesController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Admin/AdvertisingSpaces
        public async Task<IActionResult> Index(string keyword, string sort, int? page)
        {
            var vm = new AdvertisingSpaceListVM()
            {
                PageIndex = page ?? 1,
                PageSize = SettingsManager.Ads.PageSize,
                Keyword = keyword
            };


            var query = _context.AdvertisingSpaces.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword) || d.Code.Contains(keyword));


            vm.TotalCount = await query.CountAsync();
            //ViewData["ViewSortParm"] = sort == "view" ? "view_desc" : "view";
            ViewData["ImportanceSortParm"] = sort == "importance" ? "importance_desc" : "importance";
            ViewData["TitleSortParm"] = sort == "title" ? "title_desc" : "title";
            ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

            query = sort switch
            {
                //"view" => query.OrderBy(s => s.ViewCount),
                //"view_desc" => query.OrderByDescending(s => s.ViewCount),
                "title" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "importance" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                _ => query.OrderByDescending(s => s.Importance),
            };

            var advspaces = await query
                .Skip((vm.PageIndex - 1) * vm.PageSize)
                .Take(vm.PageSize).ProjectTo<AdvertisingSpaceVM>(_mapper.ConfigurationProvider).ToListAsync();


            vm.AdvertisingSpaces = new StaticPagedList<AdvertisingSpaceVM>(advspaces, vm.PageIndex, vm.PageSize, vm.TotalCount);

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

        // GET: Admin/AdvertisingSpaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisingSpace = await _context.AdvertisingSpaces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisingSpace == null)
            {
                return NotFound();
            }

            return View(advertisingSpace);
        }

       

        // GET: Admin/AdvertisingSpaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new AdvertisingSpaceIM();
            if (id == null)
            {

                vm.Active = true;
                vm.Importance = 0;

                return View(vm);
            }

            var advertisingSpace = await _context.AdvertisingSpaces.FindAsync(id);
            if (advertisingSpace == null)
            {
                return NotFound();
            }

            vm = _mapper.Map<AdvertisingSpaceIM>(advertisingSpace);
 
            return View(vm);


        }

        // POST: Admin/AdvertisingSpaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Sketch,Importance,Active,Code")] AdvertisingSpaceIM advertisingSpace)
        {

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
                if (advertisingSpace.Id > 0)
                {

                    var model = await _context.AdvertisingSpaces.FindAsync(advertisingSpace.Id);
                    model = _mapper.Map(advertisingSpace, model);

                      
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = User.Identity.Name;

                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Position));


                }
                else
                {
                    var model = _mapper.Map<AdvertisingSpace>(advertisingSpace);
                
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;

                    _context.Add(model);
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Page));


                }

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisingSpaceExists(advertisingSpace.Id))
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

        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.AdvertisingSpaces.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.AdvertisingSpaces.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.AdvertisingSpaces.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            foreach (var item in c)
            {
                item.Active = isLock ? false : true;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/AdvertisingSpaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisingSpace = await _context.AdvertisingSpaces.FindAsync(id);
            _context.AdvertisingSpaces.Remove(advertisingSpace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisingSpaceExists(int id)
        {
            return _context.AdvertisingSpaces.Any(e => e.Id == id);
        }


        [AllowAnonymous]
        public async Task<JsonResult> IsCodeUnique(string code, int? id)
        {
            var query = _context.AdvertisingSpaces.Where(d => d.Code == code).AsQueryable();
            if (id > 0)
            {
                query = query.Where(d => d.Id != id.Value);
            }

            var result = await query.CountAsync();

            return !(result > 0)
                ? Json(true)
                : Json(false);

        }

    }
}
