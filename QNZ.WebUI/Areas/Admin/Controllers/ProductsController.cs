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
using QNZ.Model.ViewModel;
using QNZ.Resources.Common;
using QNZ.Data;
using X.PagedList;
using QNZ.Infrastructure.Helper;
using QNZ.Data.Enums;
using Microsoft.Extensions.Configuration;
using QNZ.Model.Administrator;
using QNZ.Model.Settings;
using QNZCMS.Services;
using Serilog.Context;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("qnz-admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class ProductsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly IConfiguration _config;
        private readonly IWritableOptions<AdminProductSet> _writableLocations;
        public ProductsController(QNZContext context, IMapper mapper,IConfiguration config, IWritableOptions<AdminProductSet> writableLocations)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _writableLocations = writableLocations;
        }
        // GET: Admin/Products
        public async Task<IActionResult> Index(string keyword, string orderby, string sort, int? categoryId, int? page)
        {
            var vm = new ProductListVM()
            {
                PageIndex = page is null or <= 0 ? 1 : page.Value,
                Keyword = keyword,
                CategoryId = categoryId,
                PageSize = _config.GetValue<int>("Modules:Product:Administrator:PageSize"),
                OrderBy = orderby,
                Sort = sort
            };

            //var pageSize = SettingsManager.Product.PageSize;
            var query = _context.Products.Include(d => d.Category).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

            if (categoryId > 0)
                query = query.Where(d => d.CategoryId == categoryId);


            var gosort = $"{orderby}_{sort}";
            query = gosort switch
            {
                "importance_asc" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                "view_asc" => query.OrderBy(s => s.DownloadCount),
                "view_desc" => query.OrderByDescending(s => s.DownloadCount),
                "title_asc" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),

                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<ProductBVM>(_mapper.ConfigurationProvider).ToListAsync();


            vm.Products = new StaticPagedList<ProductBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var categories = await _context.ProductCategories.OrderByDescending(d => d.Importance).ProjectTo<ProductCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync();
            var catelist = new List<ProductCategoryBVM>();
            foreach (var item in categories.Where(d => d.ParentId == null))
            {              
                LoadCategories(catelist, item, 0, categories);
            }
            ViewData["Categories"] = new SelectList(catelist, "Id", "Title");


            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }


        private void LoadCategories(List<ProductCategoryBVM> catelist, ProductCategoryBVM item, int level,  List<ProductCategoryBVM> categories)
        {
            level++;
            string fuhao = "";
            for (int i = 1; i < level; i++)
            {
                fuhao += "— ";
            }
            item.Title = fuhao + item.Title;
            catelist.Add(item);
            var list = categories.Where(d => d.ParentId == item.Id);
            if (list.Any())
            {
                foreach (var sub in list)
                {                  
                     LoadCategories(catelist, sub, level, categories);                  

                }

            }
        }



        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new ProductIM();
            if (id == null)
            {
                vm.Active = true;
              

            }
            else
            {
                var article = await _context.Products.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<ProductIM>(article);

                var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.ARTICLE && d.ObjectId == vm.Id.ToString());

                if (pm != null)
                {
                    vm.SEOTitle = pm.Title;
                    vm.SEOKeywords = pm.Keywords;
                    vm.SEODescription = pm.Description;
                }

            }

            var categories = await _context.ProductCategories.OrderByDescending(d => d.Importance).ProjectTo<ProductCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync();
            var catelist = new List<ProductCategoryBVM>();
            foreach (var item in categories.Where(d => d.ParentId == null))
            {
                LoadCategories(catelist, item, 0, categories);
            }
            ViewData["Categories"] = new SelectList(catelist, "Id", "Title");

            return View(vm);

        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductIM article)
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
                    var model = await _context.Products.FirstOrDefaultAsync(d => d.Id == article.Id);
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

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Product));
                    pm.ObjectId = model.Id.ToString();


                }
                else
                {
                    var model = _mapper.Map<Product>(article);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;


                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    pm.ObjectId = model.Id.ToString();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Product));

                }



                await CreatedUpdatedPageMetaAsync(_context, pm);

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(article.Id))
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
                    opt.PageSize = pageSize;
                });
                
                const string logEvent = "分页设置";
                using (LogContext.PushProperty("LogEvent", logEvent))
                {
                    Serilog.Log.Information("产品分页设置:数量[{pageSize}]" , pageSize );
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

            var article = await _context.Products.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            if (User.Identity != null) article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;

            article.Active = false;
            //article.Recommend = false;
            article.Title = $"{article.Title}【拷贝】";

            _context.Products.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        // POST: Admin/Products/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var c = await _context.Products.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Products.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Products/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Products.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Products.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Products.Where(d => ids.Contains(d.Id)).ToListAsync();

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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsTop(int[] ids, bool isTop)
        {

            var c = await _context.Products.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            foreach (var item in c)
            {
                item.Recommend = !isTop;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
