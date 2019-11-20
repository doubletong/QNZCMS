using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SIG.Infrastructure.Helper;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class PagesController : BaseController
    {
      

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public PagesController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;      
        }

        // GET: Admin/Pages
        public async Task<IActionResult> Index(string keyword, int? page)
        {
           PageListVM vm = new PageListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword
            };
            var pageSize = 10;

            var query = _context.Pages.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword) || d.Body.Contains(keyword));


            vm.TotalCount = await query.CountAsync();
            var pages = await query.Select(d => new PageVM
            {
                Id = d.Id,
                Title = d.Title,
                ViewCount = d.ViewCount,
                SeoName = d.SeoName,
                Active = d.Active
              
            }).Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            // var list = _mapper.Map<IEnumerable<ProductVM>>(agents);

            vm.Pages = new StaticPagedList<PageVM>(pages, vm.PageIndex, pageSize, vm.TotalCount);


            return View(vm);
        }


        // GET: Admin/Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .SingleOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }
            page.Body = WebUtility.HtmlDecode(page.Body);


            return View(page);
        }



        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Body,SeoName,Active")] PageIM page)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        AR.Setfailure(GetModelErrorMessage());
        //        return Json(AR);
        //    }
        //    try
        //    {
           

        //        var model = _mapper.Map<Page>(page);
        //        //model.SeoName = model.SeoName.ToLower();
        //        model.Body = WebUtility.HtmlEncode(page.Body);

        //        _context.Add(model);
        //        await _context.SaveChangesAsync();

        //        AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Page));
        //        return Json(AR);

        //    }
        //    catch(Exception er)
        //    {
        //        AR.Setfailure(er.Message);
        //        return Json(AR);
        //    }
          
        //}

        //GET: Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new PageIM();
            if (id == null)
            {

                vm.Active = true;
                vm.Importance = 0;

                return View(vm);
            }

            var page = await _context.Pages.SingleOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            vm = _mapper.Map<PageIM>(page);
            vm.Body = WebUtility.HtmlDecode(page.Body);

            return View(vm);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Importance,Body,SeoName,Active")] PageIM page)
        {

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
                if (page.Id > 0)
                {

                    var model = await _context.Pages.SingleOrDefaultAsync(d => d.Id == page.Id);
                    model = _mapper.Map(page, model);

                    model.SeoName = model.SeoName.ToLower();
                    model.Body = WebUtility.HtmlEncode(page.Body);
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = Site.CurrentUserName;

                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Page));
                    return Json(AR);

                }
                else
                {
                    var model = _mapper.Map<Page>(page);
                    model.SeoName = model.SeoName.ToLower();
                    model.Body = WebUtility.HtmlEncode(page.Body);
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = Site.CurrentUserName;

                    _context.Add(model);
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Page));
                    return Json(AR);


                }

               

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(page.Id))
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

        // GET: Admin/Pages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .SingleOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }
            page.Body = WebUtility.HtmlDecode(page.Body);


            return View(page);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await _context.Pages.SingleOrDefaultAsync(m => m.Id == id);
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
      
        [AllowAnonymous]
        public async Task<JsonResult> IsSeoNameUnique(string seoName, int? id)
        {
            var query = _context.Pages.Where(d => d.SeoName == seoName).AsQueryable();
            if (id > 0)
            {
                query = query.Where(d => d.Id != id.Value);
            }

            var result = await query.CountAsync();

            return !(result > 0)
                ? Json(true)
                : Json(false);

        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.Id == id);
        }
    }
}
