using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Resources.Admin;
using QNZ.Data;
using X.PagedList;
using QNZ.Infrastructure.Helper;
using QNZ.Data.Enums;
using QNZ.Infrastructure.Cache;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class SocialAppsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public SocialAppsController(YicaiyunContext context, IMapper mapper, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        // GET: Admin/SocialApps
        public async Task<IActionResult> Index(string keyword, string orderby, string sort, int? page)
        {
            var vm = new SocialAppListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword,            
                PageSize = 10,
                   OrderBy = orderby,
                Sort = sort
            };

            //var pageSize = SettingsManager.SocialApp.PageSize;
            var query = _context.SocialApps.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

           

            var gosort = $"{orderby}_{sort}";
            query = gosort switch
            {
                "importance_asc" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                "title_asc" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
              
                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query     
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<SocialAppBVM>(_mapper.ConfigurationProvider).ToListAsync();
      

            vm.SocialApps = new StaticPagedList<SocialAppBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);

         

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

      

      
        // GET: Admin/SocialApps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new SocialAppIM();
            if (id == null)
            {
                vm.Active = true;              
                vm.Importance = 0;
            }
            else
            {
                var article = await _context.SocialApps.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<SocialAppIM>(article);          

            }
           

            return View(vm);

        }

        // POST: Admin/SocialApps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SocialAppIM article)
        {
       

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
              

                if (article.Id > 0)
                {
                    var model = await _context.SocialApps.FirstOrDefaultAsync(d => d.Id == article.Id);
                    if (model == null)
                    {
                        AR.Setfailure(Messages.HttpNotFound);
                        return Json(AR);
                    }
                    model = _mapper.Map(article, model);

                    model.UpdatedBy = User.Identity.Name;
                    model.UpdatedDate = DateTime.Now;


                    _context.Entry(model).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.SocialApp));
               


                }
                else
                {
                    var model = _mapper.Map<SocialApp>(article);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;


                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add(model);
                    await _context.SaveChangesAsync();
            

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.SocialApp));
                 
                }
                _cacheService.Invalidate("SOCIALAPP");

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialAppExists(article.Id))
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

        //[HttpPost]
        //public JsonResult PageSizeSet(int pageSize)
        //{
        //    try
        //    {
        //        var xmlFile = PlatformServices.Default.MapPath("/Config/SocialAppSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("PageSize").SetValue(pageSize);
        //        doc.Save(xmlFile);


        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy(int id)
        {

            var article = await _context.SocialApps.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

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
            _context.SocialApps.Add(article);
            await _context.SaveChangesAsync();

            _cacheService.Invalidate("SOCIALAPP");

            return Json(AR);
        }
        // POST: Admin/SocialApps/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.SocialApps.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.SocialApps.Remove(c);
            await _context.SaveChangesAsync();

            _cacheService.Invalidate("SOCIALAPP");

            return Json(AR);
        }

        // POST: Admin/SocialApps/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.SocialApps.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.SocialApps.RemoveRange(c);
            await _context.SaveChangesAsync();

            _cacheService.Invalidate("SOCIALAPP");

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.SocialApps.Where(d => ids.Contains(d.Id)).ToListAsync();

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

            _cacheService.Invalidate("SOCIALAPP");

            return Json(AR);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> IsTop(int[] ids, bool isTop)
        //{

        //    var c = await _context.SocialApps.Where(d => ids.Contains(d.Id)).ToListAsync();

        //    if (c == null)
        //    {
        //        AR.Setfailure(Messages.HttpNotFound);
        //        return Json(AR);
        //    }
        //    foreach (var item in c)
        //    {
        //        item.Recommend = isTop ? false : true;
        //        _context.Entry(item).State = EntityState.Modified;
        //    }

        //    await _context.SaveChangesAsync();

        //    return Json(AR);
        //}

        private bool SocialAppExists(int id)
        {
            return _context.SocialApps.Any(e => e.Id == id);
        }
    }
}
