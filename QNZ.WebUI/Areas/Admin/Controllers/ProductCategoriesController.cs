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
using SIG.Model.ViewModel;
using SIG.Model.Admin.InputModel;
using SIG.Model.Admin.ViewModel;
using YCY.Data;
using SIG.Resources.Admin;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoriesController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ProductCategoriesController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/ProductCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCategories.ProjectTo<ProductCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync());
        }

        // GET: Admin/ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ProductCategoryIM>(productCategory);
            return View(model);
        }

        // GET: Admin/ProductCategories/Create
        public IActionResult Create()
        {
            var im = new ProductCategoryIM
            {
                CreatedDate = DateTime.Now,
                Importance = 0
            };
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SubTitle,ImageUrl,CreatedDate,Importance,InMenu,Recommend")] ProductCategoryIM im)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

           
                var model = _mapper.Map<ProductCategory>(im);
              
                    
                _context.Add(model);
                
                await _context.SaveChangesAsync();
               // return RedirectToAction(nameof(Index));

            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.ProductCategory));
            return Json(AR);
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ProductCategoryIM>(productCategory);
            return View(model);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,SubTitle,ImageUrl,CreatedDate,Importance,InMenu,Recommend")] ProductCategoryIM im)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            if (id != im.Id)
            {
                AR.Setfailure("未发现此分类");
                return Json(AR);
            }


            try
            {
                var model = _mapper.Map<ProductCategory>(im);

                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(im.Id))
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }
                else
                {
                    AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.ProductCategory));
                    return Json(AR);
                }
            }
            //  return RedirectToAction(nameof(Index));

            AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.ProductCategory));
            return Json(AR);
        }

        // GET: Admin/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ProductCategoryIM>(productCategory);

            return View(model);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }
    }
}
