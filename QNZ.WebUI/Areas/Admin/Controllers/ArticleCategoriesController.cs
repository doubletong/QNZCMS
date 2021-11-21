using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QNZ.Model.ViewModel;
using QNZ.Model.Admin.InputModel;
using QNZ.Model.Admin.ViewModel;
using QNZ.Data;
using QNZ.Resources.Common;
using QNZ.Infrastructure.Helper;
using Microsoft.AspNetCore.Authorization;
using QNZ.Data.Enums;
using QNZ.Model.Administrator;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("qnz-admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class ArticleCategoriesController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        public ArticleCategoriesController(QNZContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/ArticleCategories
        public async Task<IActionResult> Index( string keyword, string orderby = "importance", string sort = "desc")
        {
            var vm = new ArticleCategoryList
            {
                Keyword = keyword,
                OrderBy = orderby,
                Sort = sort
            };

            var query = _context.ArticleCategories.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword) || d.Description.Contains(keyword));

       
            var gosort = $"{orderby}_{sort}";

            query = gosort switch
            {
                "title_asc" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "importance_asc" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                _ => query.OrderByDescending(s => s.Id),
            };

            vm.Categories = await query.ProjectTo<ArticleCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync();

            return View(vm);
        }

     

        // GET: Admin/ArticleCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new ArticleCategoryIM
            {
                Active = true,
                Importance = 0
            };
            if (id == null)
            {
                return View(vm);
            }
         
            var category = await _context.ArticleCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ArticleCategoryIM>(category);

            var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.ARTICLECATEGORY && d.ObjectId == category.Alias);

            if (pm != null)
            {
                model.SEOTitle = pm.Title;
                model.SEOKeywords = pm.Keywords;
                model.SEODescription = pm.Description;
            }

            return View(model);
          
        }

        // POST: Admin/ArticleCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Id,Title,ImageUrl,Alias,Description,Importance,Active,SEOTitle,SEOKeywords,SEODescription")] ArticleCategoryIM im, int id = 0)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

            if (id == 0)
            {
             
                var model = _mapper.Map<ArticleCategory>(im);
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                _context.Add(model);

                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.ArticleCategory));
                return Json(AR);
            }
          
                if (id != im.Id)
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }


                try
                {
                    var model = await _context.ArticleCategories.FindAsync(id);
                    model = _mapper.Map(im, model);

                    model.UpdatedBy = User.Identity.Name;
                    model.UpdatedDate = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    var pm = new PageMeta
                    {
                        Title = im.SEOTitle,
                        Description = im.SEODescription,
                        Keywords = im.SEOKeywords,
                        ModuleType = (short)ModuleType.ARTICLECATEGORY,
                        ObjectId = im.Alias
                    };

                    await CreatedUpdatedPageMetaAsync(_context, pm);

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.ArticleCategory));
                    return Json(AR);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleCategoryExists(im.Id))
                    {
                        AR.Setfailure("未发现此分类");
                        return Json(AR);
                    }
                    else
                    {
                        AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.ArticleCategory));
                        return Json(AR);
                    }
                }
               
              
            
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy(int id)
        {

            var article = await _context.ArticleCategories.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

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

            _context.ArticleCategories.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {
            var exitAricles = await _context.Articles.AnyAsync(d => ids.Contains(d.CategoryId));
            if (exitAricles)
            {
                AR.Setfailure(Messages.HasChildCanNotDelete);
                return Json(AR);
            }

            var c = await _context.ArticleCategories.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.ArticleCategories.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.ArticleCategories.Where(d => ids.Contains(d.Id)).ToListAsync();

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



        // POST: Admin/Articles/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var c = await _context.ArticleCategories.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.ArticleCategories.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }



        [HttpPost]
        public async Task<IActionResult> UploadAsync()
        {
            //  long size = 0;
            var files = Request.Form.Files;
            //foreach (var file in files)
            //{
            var filename = ContentDispositionHeaderValue
                            .Parse(files[0].ContentDisposition)
                            .FileName
                            .Trim('"');
            var filePath = _hostingEnvironment.WebRootPath + $@"\uploads\{filename}";
            // size += file.Length;
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                await files[0].CopyToAsync(fs);
                await fs.FlushAsync();
            }

            var imgUrl = "/Uploads/" + filename;
            //}
            // string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            return Json(imgUrl);

          
        }


        private bool ArticleCategoryExists(int id)
        {
            return _context.ArticleCategories.Any(e => e.Id == id);
        }

        [AllowAnonymous]
        public async Task<JsonResult> IsAliasUnique(string Alias, int? id)
        {
            var query = _context.ArticleCategories.Where(d => d.Alias == Alias).AsQueryable();
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
