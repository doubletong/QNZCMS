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
using SIG.Resources.Admin;
using SIG.Infrastructure.Helper;
using Microsoft.AspNetCore.Authorization;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ArticleCategoriesController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ArticleCategoriesController(YicaiyunContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/ArticleCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticleCategories.ProjectTo<ArticleCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync());
        }

        // GET: Admin/ArticleCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.ArticleCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            // var model = _mapper.Map<ArticleCategoryIM>(productCategory);
            return View(model);
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
            else
            {
                var productCategory = await _context.ArticleCategories.FindAsync(id);
                if (productCategory == null)
                {
                    return NotFound();
                }
                var model = _mapper.Map<ArticleCategoryIM>(productCategory);
                return View(model);
            }

          
        }

        // POST: Admin/ArticleCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Id,Title,ImageUrl,Alias,Importance,Active")] ArticleCategoryIM im, int id = 0)
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
            else
            {
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
                //  return RedirectToAction(nameof(Index));

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.ArticleCategory));
                return Json(AR);
            }
           
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
