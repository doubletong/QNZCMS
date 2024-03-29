﻿using System;
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
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class SolutionsController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        public SolutionsController(QNZContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        [Route("/admin/solutions")]
        [Route("/admin/solutions/index")]      
        public async Task<IActionResult> Index(string sort,string keyword)
        {
            var vm = new SolutionListVM
            {
                Keyword = keyword
            };

            var query = _context.Solutions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword) || d.Description.Contains(keyword));
         
            ViewData["ImportanceSortParm"] = sort == "importance" ? "importance_desc" : "importance";
            ViewData["TitleSortParm"] = sort == "title" ? "title_desc" : "title";
            ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

            query = sort switch
            {              
                "title" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "importance" => query.OrderBy(s => s.CreatedDate),
                "importance_desc" => query.OrderByDescending(s => s.CreatedDate),
                _ => query.OrderByDescending(s => s.Importance),
            };

            vm.Solutions = await query.ProjectTo<SolutionBVM>(_mapper.ConfigurationProvider).ToListAsync();

            return View(vm);
        }

        // GET: Admin/Solutions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Solutions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            // var model = _mapper.Map<SolutionIM>(productCategory);
            return View(model);
        }


        // GET: Admin/Solutions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new SolutionIM
            {
                Active = true,
                Importance = 0
            };
            if (id == null)
            {
                return View(vm);
            }
         
            var category = await _context.Solutions.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<SolutionIM>(category);
            model.Products = !string.IsNullOrEmpty(model.RelatedProducts) ? model.RelatedProducts.Split("|") : null;;

            var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.SOLUTION && d.ObjectId == category.Id.ToString());

            if (pm != null)
            {
                model.SEOTitle = pm.Title;
                model.SEOKeywords = pm.Keywords;
                model.SEODescription = pm.Description;
            }

            var categories = await _context.Products.AsNoTracking()
             .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Products"] = new SelectList(categories, "Id", "Title");


            return View(model);
          
        }

        // POST: Admin/Solutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Id,Title,SubTitle,Thumbnail,ImageUrl,Body,Description,RelatedProducts,Products,Importance,Active,SEOTitle,SEOKeywords,SEODescription")] SolutionIM im, int id = 0)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

            im.RelatedProducts = im.Products != null ? string.Join("|", im.Products) : null; ;

            if (id == 0)
            {
             
                var model = _mapper.Map<Solution>(im);
            
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                _context.Add(model);

                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Solution));
                return Json(AR);
            }
            else
            {
                if (id != im.Id)
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }


                try
                {
                    var model = await _context.Solutions.FindAsync(id);
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
                        ModuleType = (short)ModuleType.SOLUTION,
                        ObjectId = im.Id.ToString()
                    };

                    await CreatedUpdatedPageMetaAsync(_context, pm);

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Solution));
                    return Json(AR);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolutionExists(im.Id))
                    {
                        AR.Setfailure("未发现此分类");
                        return Json(AR);
                    }
                    else
                    {
                        AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.Solution));
                        return Json(AR);
                    }
                }

            }
           
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

            var c = await _context.Solutions.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Solutions.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Solutions.Where(d => ids.Contains(d.Id)).ToListAsync();

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


            var c = await _context.Solutions.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Solutions.Remove(c);
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


        private bool SolutionExists(int id)
        {
            return _context.Solutions.Any(e => e.Id == id);
        }

        //[AllowAnonymous]
        //public async Task<JsonResult> IsAliasUnique(string seoName, int? id)
        //{
        //    var query = _context.Pages.Where(d => d.SeoName == seoName).AsQueryable();
        //    if (id > 0)
        //    {
        //        query = query.Where(d => d.Id != id.Value);
        //    }

        //    var result = await query.CountAsync();

        //    return !(result > 0)
        //        ? Json(true)
        //        : Json(false);

        //}
    }
}
