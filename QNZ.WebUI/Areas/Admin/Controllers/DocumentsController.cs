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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class DocumentsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DocumentsController(YicaiyunContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: Admin/Documents
        public async Task<IActionResult> Index(string keyword, string orderby, string sort, int? categoryId, int? page)
        {
            var vm = new DocumentListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword,
                CategoryId = categoryId,
                PageSize = 10,
                   OrderBy = orderby,
                Sort = sort
            };

            //var pageSize = SettingsManager.Document.PageSize;
            var query = _context.Documents.Include(d=>d.Category).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

            if (categoryId>0)
                query = query.Where(d => d.CategoryId == categoryId);


            var gosort = $"{orderby}_{sort}";
            query = gosort switch
            {
                "view" => query.OrderBy(s => s.DownloadCount),
                "view_desc" => query.OrderByDescending(s => s.DownloadCount),
                "title" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date" => query.OrderBy(s => s.Pubdate),
                "date_desc" => query.OrderByDescending(s => s.Pubdate),
              
                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query     
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<DocumentBVM>(_mapper.ConfigurationProvider).ToListAsync();
      

            vm.Documents = new StaticPagedList<DocumentBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);

            var categories = await _context.DocCategories.AsNoTracking()
                 .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Title");

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

      

      
        // GET: Admin/Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new DocumentIM();
            if (id == null)
            {
                vm.Active = true;
                vm.PubDate = DateTime.Now;
             
            }
            else
            {
                var article = await _context.Documents.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<DocumentIM>(article);

                //var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.ARTICLE && d.ObjectId == vm.Id.ToString());

                //if (pm != null)
                //{
                //    vm.SEOTitle = pm.Title;
                //    vm.SEOKeywords = pm.Keywords;
                //    vm.SEODescription = pm.Description;
                //}

            }
            var categories = await _context.DocCategories.AsNoTracking()
               .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Title");

           

            return View(vm);

        }

        // POST: Admin/Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DocumentIM article)
        {
       

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
                //var pm = new PageMeta
                //{
                //    Title = article.SEOTitle,
                //    Description = article.SEODescription,
                //    Keywords = article.SEOKeywords,
                //    ModuleType = (short)ModuleType.ARTICLE
                  
                //};

                string webRootPath = _hostingEnvironment.WebRootPath;
               

                if (article.Id > 0)
                {
                    var model = await _context.Documents.FirstOrDefaultAsync(d => d.Id == article.Id);
                    if (model == null)
                    {
                        AR.Setfailure(Messages.HttpNotFound);
                        return Json(AR);
                    }
                    model = _mapper.Map(article, model);

                    model.UpdatedBy = User.Identity.Name;
                    model.UpdatedDate = DateTime.Now;

                    string filePath = webRootPath + model.FileUrl;
                    if (System.IO.File.Exists(filePath))
                    {
                        var fz = await System.IO.File.ReadAllBytesAsync(filePath);
                        model.FileSize = fz.Length;
                    }
                


                    _context.Entry(model).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Document));
                    // pm.ObjectId = model.Id.ToString();


                }
                else
                {
                    var model = _mapper.Map<Document>(article);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;

                    string filePath = webRootPath + model.FileUrl;
                    if (System.IO.File.Exists(filePath))
                    {
                        var fz = await System.IO.File.ReadAllBytesAsync(filePath);
                        model.FileSize = fz.Length;
                    }

                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    //pm.ObjectId = model.Id.ToString();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Document));
                 
                }             

                //await CreatedUpdatedPageMetaAsync(_context, pm);

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(article.Id))
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
        //        var xmlFile = PlatformServices.Default.MapPath("/Config/DocumentSettings.config");
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

            var article = await _context.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
            article.Pubdate = DateTime.Now;
            article.Active = false;
            //article.Recommend = false;
            article.Title = $"{article.Title}【拷贝】"; 

            _context.Documents.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        // POST: Admin/Documents/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Documents.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Documents/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Documents.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Documents.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Documents.Where(d => ids.Contains(d.Id)).ToListAsync();

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



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> IsTop(int[] ids, bool isTop)
        //{

        //    var c = await _context.Documents.Where(d => ids.Contains(d.Id)).ToListAsync();

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

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
