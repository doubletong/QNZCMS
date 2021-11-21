using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QNZ.Resources.Common;
using QNZ.Data;
using X.PagedList;

using QNZ.Infrastructure.Helper;
using QNZ.Data.Enums;
using System.Xml.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using QNZ.Model.Administrator;
using QNZ.Model.Administrator.InputModel;
using QNZ.Model.Administrator.ViewModel;
using QNZ.Model.Settings;
using QNZCMS.Services;
using Serilog.Context;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("qnz-admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class PostsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly IConfiguration _config;
        private readonly IWritableOptions<AdminBlogSet> _writableLocations;
        public PostsController(QNZContext context, IMapper mapper, IConfiguration config, IWritableOptions<AdminBlogSet> writableLocations)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _writableLocations = writableLocations;
        }
        // GET: Admin/Posts
        public async Task<IActionResult> Index(string keyword, string sort, int? categoryId, int? page)
        {
            var vm = new PostListVM()
            {
                PageIndex = page is null or <= 0 ? 1 : page.Value,
                Keyword = keyword,
                CategoryId = categoryId,
                PageSize = _config.GetValue<int>("Modules:Blog:Administrator:PageSize"),
            };

            //var pageSize = SettingsManager.Post.PageSize;
            var query = _context.Posts.Include(d=>d.Category).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

            if (categoryId>0)
                query = query.Where(d => d.CategoryId == categoryId);


            ViewData["ViewSortParm"] = sort == "view" ? "view_desc" : "view";
            ViewData["TitleSortParm"] = sort == "title" ? "title_desc" : "title";
            ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

            query = sort switch
            {
                "view" => query.OrderBy(s => s.ViewCount),
                "view_desc" => query.OrderByDescending(s => s.ViewCount),
                "title" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
              
                _ => query.OrderByDescending(s => s.CreatedDate),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query     
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<PostBVM>(_mapper.ConfigurationProvider).ToListAsync();
      

            vm.Posts = new StaticPagedList<PostBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);

            var categories = await _context.PostCategories.AsNoTracking()
                 .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Title");

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Posts.Include(d=>d.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

      
        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new PostIM();
            if (id == null)
            {
                vm.Active = true;
           
             
            }
            else
            {
                var article = await _context.Posts.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<PostIM>(article);

                var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.ARTICLE && d.ObjectId == vm.Id.ToString());

                if (pm != null)
                {
                    vm.SEOTitle = pm.Title;
                    vm.SEOKeywords = pm.Keywords;
                    vm.SEODescription = pm.Description;
                }

            }
            var categories = await _context.PostCategories.AsNoTracking()
               .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Title");

            return View(vm);

        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostIM article)
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
                    ModuleType = (short)ModuleType.ARTICLE
                  
                };


                if (article.Id > 0)
                {
                    var model = await _context.Posts.FirstOrDefaultAsync(d => d.Id == article.Id);
                    if (model == null)
                    {
                        AR.Setfailure(Messages.HttpNotFound);
                        return Json(AR);
                    }
                    model = _mapper.Map(article, model);

                    if (User.Identity != null) model.UpdatedBy = User.Identity.Name;
                    model.UpdatedDate = DateTime.Now;


                    _context.Entry(model).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Post));
                    pm.ObjectId = model.Id.ToString();


                }
                else
                {
                    var model = _mapper.Map<Post>(article);

                    if (User.Identity != null) model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;


                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add((object)model);
                    await _context.SaveChangesAsync();
                    pm.ObjectId = model.Id.ToString();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Post));
                 
                }

             

                await CreatedUpdatedPageMetaAsync(_context, pm);

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(article.Id))
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
                _writableLocations.Update(opt => {
                    opt.PageSize1 = pageSize;
                });
                
                const string logEvent = "分页设置";
                using (LogContext.PushProperty("LogEvent", logEvent))
                {
                    Serilog.Log.Information("博客文章分页设置:数量[{pageSize}]" , pageSize );
                }
                return Json(AR);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex,"错误:{@error}", ex.Message);  
                AR.Setfailure(ex.Message);
                return Json(AR);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy(int id)
        {

            var article = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            if (User.Identity != null) article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
       
            article.Active = false;
            article.Title = $"{article.Title}【拷贝】"; 

            _context.Posts.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        // POST: Admin/Posts/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.Posts.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Posts.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Posts/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Posts.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Posts.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Posts.Where(d => ids.Contains(d.Id)).ToListAsync();

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

            return Json(AR);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
