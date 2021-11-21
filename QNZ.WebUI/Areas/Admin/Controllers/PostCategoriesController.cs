using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Resources.Common;
using Microsoft.AspNetCore.Authorization;
using QNZ.Data.Enums;
using QNZ.Model.Administrator;
using QNZ.Model.Administrator.InputModel;
using QNZ.Model.Administrator.ViewModel;
using Serilog.Context;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("qnz-admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class PostCategoriesController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        public PostCategoriesController(QNZContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/PostCategories
        public async Task<IActionResult> Index(string keyword,string orderby = "importance", string sort = "desc")
        {
            var vm = new PostCategoryList
            {
                Keyword = keyword,
                OrderBy = orderby,
                Sort = sort
            };
            try
            {
                var query = _context.PostCategories.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(d => d.Title.Contains(keyword) || d.Description.Contains(keyword));
         
                // ViewData["ImportanceSortParm"] = sort == "importance" ? "importance_desc" : "importance";
                // ViewData["TitleSortParm"] = sort == "title" ? "title_desc" : "title";
                // ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";
                
                var goSort = $"{orderby}_{sort}";     

                query = goSort switch
                {              
                    "title_asc" => query.OrderBy(s => s.Title),
                    "title_desc" => query.OrderByDescending(s => s.Title),
                    "date_asc" => query.OrderBy(s => s.CreatedDate),
                    "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                    "importance" => query.OrderBy(s => s.CreatedDate),
                    "importance_desc" => query.OrderByDescending(s => s.CreatedDate),
                    _ => query.OrderByDescending(s => s.Importance),
                };

                vm.Categories = await query.ProjectTo<PostCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync();

                return View(vm);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex,"页面列表读取错误:{@error}", ex.Message);
                throw;
            }

            
        }

        // GET: Admin/PostCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.PostCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            // var model = _mapper.Map<PostCategoryIM>(productCategory);
            return View(model);
        }


        // GET: Admin/PostCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new PostCategoryIM
            {
                Active = true,
                Importance = 0
            };
            if (id == null)
            {
                return View(vm);
            }
         
            var category = await _context.PostCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<PostCategoryIM>(category);

            var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.ARTICLECATEGORY && d.ObjectId == category.Alias);

            if (pm == null) return View(model);
            
            model.SEOTitle = pm.Title;
            model.SEOKeywords = pm.Keywords;
            model.SEODescription = pm.Description;

            return View(model);
          
        }

        // POST: Admin/PostCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Id,Title,ImageUrl,Alias,Description,Importance,Active,SEOTitle,SEOKeywords,SEODescription")] PostCategoryIM im, int id = 0)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

            if (id == 0)
            {
             
                var model = _mapper.Map<PostCategory>(im);
                if (User.Identity != null) model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                _context.Add(model);

                await _context.SaveChangesAsync();
                
                using (LogContext.PushProperty("LogEvent", Buttons.Add))
                {
                    Serilog.Log.Information("添加博客分类:[{title}]", model.Title);  
                }
                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.PostCategory));
                return Json(AR);
            }
          
            if (id != im.Id)
            {
                AR.Setfailure("未发现此分类");
                return Json(AR);
            }


            try
            {
                var model = await _context.PostCategories.FindAsync(id);
                model = _mapper.Map(im, model);

                if (User.Identity != null) model.UpdatedBy = User.Identity.Name;
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
                
                using (LogContext.PushProperty("LogEvent", Buttons.Update))
                {
                    Serilog.Log.Information("修改博客分类:[{title}]", model.Title);  
                }

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.PostCategory));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostCategoryExists(im.Id))
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }
                else
                {
                    AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.PostCategory));
                    return Json(AR);
                }
            }
        }


        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {
            var exitArticles = await _context.Articles.AnyAsync(d => ids.Contains(d.CategoryId));
            if (exitArticles)
            {
                AR.Setfailure(Messages.HasChildCanNotDelete);
                return Json(AR);
            }

            var c = await _context.PostCategories.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.PostCategories.RemoveRange(c);
            await _context.SaveChangesAsync();

            using (LogContext.PushProperty("LogEvent", Buttons.Delete))
            {
                Serilog.Log.Information("删除博客分类:ID[{logIds}]", string.Join(",",ids));  
            }
            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.PostCategories.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            foreach (var item in c)
            {
                item.Active = !isLock;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            var logEvent = isLock ? "锁定" : "激活";
            using (LogContext.PushProperty("LogEvent", logEvent))
            {
                Serilog.Log.Information("{action}博客分类:ID[{pageIds}]", logEvent,string.Join(",",ids));  
            }
            
            return Json(AR);
        }



        // POST: Admin/Articles/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var c = await _context.PostCategories.FirstOrDefaultAsync(d => d.Id == id);
            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.PostCategories.Remove(c);
            await _context.SaveChangesAsync();
            
            using (LogContext.PushProperty("LogEvent", Buttons.Delete))
            {
                Serilog.Log.Information("删除博客分类:[{title}]",c.Title);  
            }

            return Json(AR);
        }


        //
        // [HttpPost]
        // public async Task<IActionResult> UploadAsync()
        // {
        //     //  long size = 0;
        //     var files = Request.Form.Files;
        //     //foreach (var file in files)
        //     //{
        //     var filename = ContentDispositionHeaderValue
        //                     .Parse(files[0].ContentDisposition)
        //                     .FileName
        //                     .Trim('"');
        //     var filePath = _hostingEnvironment.WebRootPath + $@"\uploads\{filename}";
        //     // size += file.Length;
        //     using (FileStream fs = System.IO.File.Create(filePath))
        //     {
        //         await files[0].CopyToAsync(fs);
        //         await fs.FlushAsync();
        //     }
        //
        //     var imgUrl = "/Uploads/" + filename;
        //     //}
        //     // string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
        //     return Json(imgUrl);
        //
        //   
        // }


        private bool PostCategoryExists(int id)
        {
            return _context.PostCategories.Any(e => e.Id == id);
        }

        [AllowAnonymous]
        public async Task<JsonResult> IsAliasUnique(string seoName, int? id)
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
    }
}
