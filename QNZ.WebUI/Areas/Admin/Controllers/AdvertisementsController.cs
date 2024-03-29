﻿using System;
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
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;
using QNZ.Resources.Common;
using X.PagedList;
using QNZ.Infrastructure.Cache;
using QNZ.Model.Administrator;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class AdvertisementsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly ICacheService _cacheService;
        public AdvertisementsController(QNZContext context, IMapper mapper, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        // GET: Admin/Advertisements
        public async Task<IActionResult> Index(string keyword, string sort, int? spaceId, int? page)
        {
            var vm = new AdvertisementListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword,
                SpaceId = spaceId,
                PageSize = SettingsManager.Ads.PageSize
            };

            //var pageSize = SettingsManager.Article.PageSize;
            var query = _context.Advertisements.Include(d => d.Space).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

            if (spaceId > 0)
                query = query.Where(d => d.SpaceId == spaceId);


            ViewData["ImportanceSortParm"] = sort == "importance" ? "importance_desc" : "importance";
            ViewData["TitleSortParm"] = sort == "title" ? "title_desc" : "title";
            ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

            query = sort switch
            {
                "importance" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                "title" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),

                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var list = await query
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<AdvertisementVM>(_mapper.ConfigurationProvider).ToListAsync();


            vm.Advertisments = new StaticPagedList<AdvertisementVM>(list, vm.PageIndex, vm.PageSize, vm.TotalCount);

            var spaces = await _context.AdvertisingSpaces.AsNoTracking()
                 .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Spaces"] = new SelectList(spaces, "Id", "Title");

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }



        // GET: Admin/Advertisements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new AdvertisementIM();
            if (id == null)
            {
                vm.Active = true;
                vm.Importance = 0;
            }
            else
            {
                var article = await _context.Advertisements.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<AdvertisementIM>(article);

            }
            var categories = await _context.AdvertisingSpaces.AsNoTracking()
               .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["SpaceId"] = new SelectList(categories, "Id", "Title");

            return View(vm);


        }

        // POST: Admin/Advertisements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Description,WebLink,ImageUrl,ImageUrlMobile,Importance,Active,SpaceId")] AdvertisementIM advertisement)
        {


            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {


                if (advertisement.Id > 0)
                {
                    var model = await _context.Advertisements.FirstOrDefaultAsync(d => d.Id == advertisement.Id);
                    if (model == null)
                    {
                        AR.Setfailure(Messages.HttpNotFound);
                        return Json(AR);
                    }
                    model = _mapper.Map(advertisement, model);

                    model.UpdatedBy = User.Identity.Name;
                    model.UpdatedDate = DateTime.Now;


                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Carousel));


                }
                else
                {
                    var model = _mapper.Map<Advertisement>(advertisement);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;

                    _context.Add(model);
                    await _context.SaveChangesAsync();


                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Carousel));

                }
                _cacheService.Invalidate("ADVERTISEMENT");
                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisementExists(advertisement.Id))
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy(int id)
        {

            var article = await _context.Advertisements.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
            article.Active = false;
            article.Title = $"{article.Title}【拷贝】";

            _context.Advertisements.Add(article);
            await _context.SaveChangesAsync();
            _cacheService.Invalidate("ADVERTISEMENT");
            return Json(AR);
        }

        // POST: Admin/Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _context.Advertisements.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Advertisements.Remove(c);
            await _context.SaveChangesAsync();
            _cacheService.Invalidate("ADVERTISEMENT");
            return Json(AR);
        }

        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Advertisements.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Advertisements.RemoveRange(c);
            await _context.SaveChangesAsync();
            _cacheService.Invalidate("ADVERTISEMENT");
            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Advertisements.Where(d => ids.Contains(d.Id)).ToListAsync();

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
            _cacheService.Invalidate("ADVERTISEMENT");
            return Json(AR);
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }
    }
}
