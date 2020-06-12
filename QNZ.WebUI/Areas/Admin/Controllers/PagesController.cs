using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;
using QNZ.Resources.Admin;
using X.PagedList;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class PagesController : BaseController
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public PagesController(IWebHostEnvironment hostingEnvironment, YicaiyunContext context, IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _mapper = mapper;      
        }

        // GET: Admin/Pages
        public async Task<IActionResult> Index(string keyword, int? page, string orderby = "importance", string sort = "desc")
        {
           PageListVM vm = new PageListVM()
            {
                PageIndex = page??1,
                PageSize = SettingsManager.Page.PageSize,
                Keyword = keyword,
                OrderBy = orderby,
                Sort = sort
            };
        

            var query = _context.Pages.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword) || d.Body.Contains(keyword));        


            vm.TotalCount = await query.CountAsync();
            var gosort = $"{orderby}_{sort}";         

            query = gosort switch
            {
                "view_asc" => query.OrderBy(s => s.ViewCount),
                "view_desc" => query.OrderByDescending(s => s.ViewCount),
                "title_asc" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "importance_asc" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                _ => query.OrderByDescending(s => s.Id),
            };

            var pages = await query
                .Skip((vm.PageIndex - 1) * vm.PageSize)
                .Take(vm.PageSize).ProjectTo<PageVM>(_mapper.ConfigurationProvider).ToListAsync();
          

            vm.Pages = new StaticPagedList<PageVM>(pages, vm.PageIndex, vm.PageSize, vm.TotalCount);

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

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

            var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.PAGE && d.ObjectId == vm.SeoName);

            if (pm != null)
            {
                vm.SEOTitle = pm.Title;
                vm.SEOKeywords = pm.Keywords;
                vm.SEODescription = pm.Description;
            }
           

            return View(vm);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Importance,Body,SeoName,Active,SEOTitle,SEOKeywords,SEODescription")] PageIM page)
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
                    model.UpdatedBy = User.Identity.Name;

                    _context.Update(model);
                    await _context.SaveChangesAsync();                 

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Page));
                 

                }
                else
                {
                    var model = _mapper.Map<Page>(page);
                    model.SeoName = model.SeoName.ToLower();
                    model.Body = WebUtility.HtmlEncode(page.Body);
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;

                    _context.Add(model);
                    await _context.SaveChangesAsync();                

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Page));                  


                }

                var pm = new PageMeta
                {
                    Title = page.SEOTitle,
                    Description = page.SEODescription,
                    Keywords = page.SEOKeywords,
                    ModuleType = (short)ModuleType.PAGE,
                    ObjectId = page.SeoName
                };

                await CreatedUpdatedPageMetaAsync(_context, pm);

                return Json(AR);

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



        [HttpPost]
        public JsonResult PageSizeSet(int pageSize)
        {
            try
            {
                var xmlFile = PlatformServices.Default.MapPath("/Config/PageSettings.config");
                XDocument doc = XDocument.Load(xmlFile);

                var item = doc.Descendants("Settings").FirstOrDefault();
                item.Element("PageSize").SetValue(pageSize);
                doc.Save(xmlFile);


                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }
        }


        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _context.Pages.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Pages.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);

        }
        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Pages.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Pages.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids,bool isLock)
        {

            var c = await _context.Pages.Where(d => ids.Contains(d.Id)).ToListAsync();

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
