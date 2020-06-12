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
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Resources.Admin;
using QNZ.Data;
using X.PagedList;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;
using QNZ.Data.Enums;
using System.Xml.Linq;
using Microsoft.Extensions.PlatformAbstractions;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class ExhibitionsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ExhibitionsController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: Admin/Exhibitions
        public async Task<IActionResult> Index(string keyword, string orderby, string sort, int? categoryId, int? page)
        {
            var vm = new ExhibitionListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword,
                CategoryId = categoryId,
                PageSize = 10,
                   OrderBy = orderby,
                Sort = sort
            };

            //var pageSize = SettingsManager.Exhibition.PageSize;
            var query = _context.Exhibitions.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

           

            var gosort = $"{orderby}_{sort}";
            query = gosort switch
            {
                "view" => query.OrderBy(s => s.ViewCount),
                "view_desc" => query.OrderByDescending(s => s.ViewCount),
                "title" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date" => query.OrderBy(s => s.DateStart),
                "date_desc" => query.OrderByDescending(s => s.DateStart),
              
                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query     
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<ExhibitionBVM>(_mapper.ConfigurationProvider).ToListAsync();
      

            vm.Exhibitions = new StaticPagedList<ExhibitionBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);

         

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

      

      
        // GET: Admin/Exhibitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new ExhibitionIM();
            if (id == null)
            {
                vm.Active = true;
                vm.DateStart = DateTime.Now;
                vm.DateEnd = DateTime.Now;
            }
            else
            {
                var article = await _context.Exhibitions.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<ExhibitionIM>(article);

                var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.EXHIBITION && d.ObjectId == vm.Id.ToString());

                if (pm != null)
                {
                    vm.SEOTitle = pm.Title;
                    vm.SEOKeywords = pm.Keywords;
                    vm.SEODescription = pm.Description;
                }

            }
           

            return View(vm);

        }

        // POST: Admin/Exhibitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExhibitionIM article)
        {
       

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
                var pm = new PageMeta
                {
                    Title = article.SEOTitle,
                    Description = article.SEODescription,
                    Keywords = article.SEOKeywords,
                    ModuleType = (short)ModuleType.EXHIBITION

                };


                if (article.Id > 0)
                {
                    var model = await _context.Exhibitions.FirstOrDefaultAsync(d => d.Id == article.Id);
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

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Exhibition));
                    pm.ObjectId = model.Id.ToString();


                }
                else
                {
                    var model = _mapper.Map<Exhibition>(article);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;


                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    pm.ObjectId = model.Id.ToString();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Exhibition));
                 
                }

             

                await CreatedUpdatedPageMetaAsync(_context, pm);

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExhibitionExists(article.Id))
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
        //        var xmlFile = PlatformServices.Default.MapPath("/Config/ExhibitionSettings.config");
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

            var article = await _context.Exhibitions.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
        
            article.Active = false;
            article.Recommend = false;
            article.Title = $"{article.Title}【拷贝】"; 

            _context.Exhibitions.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        // POST: Admin/Exhibitions/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.Exhibitions.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Exhibitions.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Exhibitions/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Exhibitions.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Exhibitions.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Exhibitions.Where(d => ids.Contains(d.Id)).ToListAsync();

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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsTop(int[] ids, bool isTop)
        {

            var c = await _context.Exhibitions.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            foreach (var item in c)
            {
                item.Recommend = isTop ? false : true;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool ExhibitionExists(int id)
        {
            return _context.Exhibitions.Any(e => e.Id == id);
        }
    }
}
