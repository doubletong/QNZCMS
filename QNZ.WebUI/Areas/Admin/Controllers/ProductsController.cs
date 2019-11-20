using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PagedList.Core;
using SIG.Infrastructure.Helper;
using SIG.Model.Admin.InputModel;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class ProductsController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ProductsController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: Admin/Products
        public async Task<IActionResult> Index(string keyword, int? page)
        {
            ProductPageVM vm = new ProductPageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword
            };
            var pageSize = 10;

            var query = _context.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Name.Contains(keyword) || d.SubName.Contains(keyword));


            vm.TotalCount = await query.CountAsync();
            var products = await query.OrderByDescending(d=>d.Id).ProjectTo<ProductVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();


           foreach(var item in products)
            {
                var categories = _context.ProductCategories.Where(d => d.PcategoryProducts.Any(c => c.ProductId == item.Id)).ToList();
                item.CategoryTitle = string.Join("、", categories.Select(d=>d.Title).ToArray());
            }

            vm.Products = new StaticPagedList<ProductVM>(products, vm.PageIndex, pageSize, vm.TotalCount);


            return View(vm);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ProductDetailVM>(product);
            model.Description = WebUtility.HtmlDecode(product.Description);

            return View(model);
        }
      
        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Stores"] = new SelectList(await _context.Stores.AsNoTracking().ToListAsync(), "Id", "Name");
            ViewData["Categories"] = new SelectList(await _context.ProductCategories.AsNoTracking().ToListAsync(), "Id", "Title");

            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("Id,StoreId,CategoryIds ,Name,SubName,Specification,Price, OriginalPrice,Stock, Description,Summary,Thumbnail,FullImages,Active")] ProductIM product)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            try
            {
                var model = _mapper.Map<Product>(product);

                model.Description = WebUtility.HtmlEncode(product.Description);

                foreach (var item in product.CategoryIds)
                {
                    model.PcategoryProducts.Add(new PcategoryProduct { CategoryId = item, ProductId = product.Id });
                }

                _context.Add(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Product));
                return Json(AR);
            }catch(Exception er)
            {
                AR.Setfailure(er.Message);
                return Json(AR);
            }
           
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(d=>d.PcategoryProducts).SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ProductIM>(product);
            model.Description = WebUtility.HtmlDecode(product.Description);
            model.CategoryIds = product.PcategoryProducts.Select(d => d.CategoryId).ToArray();

            ViewData["Stores"] = new SelectList(await _context.Stores.AsNoTracking().ToListAsync(), "Id", "Name");
            ViewData["Categories"] = new SelectList(await _context.ProductCategories.AsNoTracking().ToListAsync(), "Id", "Title");

            return View(model);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreId,CategoryIds ,Name,SubName,Specification,Price, OriginalPrice,Stock, Description,Summary,Thumbnail,FullImages,Active")] ProductIM product)
        {
            if (id != product.Id)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
                var model = await _context.Products.SingleOrDefaultAsync(d => d.Id == id);

                model = _mapper.Map(product, model);
                model.Description = WebUtility.HtmlEncode(product.Description);

                var list = _context.PcategoryProducts.Where(d => d.ProductId == model.Id).ToList();
                _context.RemoveRange(list);

                foreach(var item in product.CategoryIds)
                {
                    _context.Add(new PcategoryProduct { CategoryId = item, ProductId = product.Id });
                }

                _context.Update(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Product));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(d=>d.CartItems)
                .Include(d=>d.OrderDetails)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ProductDetailVM>(product);
            model.Description = WebUtility.HtmlDecode(product.Description);

            model.HasData = product.OrderDetails.Any() || product.CartItems.Any();

            return View(model);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        [HttpPost]
   
        public async Task<IActionResult> UploadAsync()
        {
            string filePathName = string.Empty;
            //  long size = 0;
            var files = Request.Form.Files;
            //foreach (var file in files)
            //{
            //var filename = ContentDispositionHeaderValue
            //                .Parse(files[0].ContentDisposition)
            //                .FileName
            //                .Trim('"');
            var folderDate = string.Format("{0:yyyyMMdd}", DateTime.Now);
            string localPath = _hostingEnvironment.ContentRootPath + $@"\upload\image\{folderDate}";

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
          

            var fileName = Path.GetFileNameWithoutExtension(files[0].FileName);
            string ex = Path.GetExtension(files[0].FileName);
            fileName = FileHelper.GetFileName(fileName, localPath, ex);
            filePathName = localPath + "\\" + fileName + ex;
        
            // size += file.Length;
            using (FileStream fs = System.IO.File.Create(filePathName))
            {
                await files[0].CopyToAsync(fs);
                await fs.FlushAsync();
            }

            var imgUrl = $"/upload/image/{folderDate}/{fileName}{ex}";
            //}
            // string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            return Json(imgUrl);

          
        }


        [HttpPost]
      
        public async Task<ActionResult> UpLoadImages(string id, string name, string type, string lastModifiedDate, int size, IFormFile file)
        {
            string filePathName = string.Empty;

            //
            var folderDate = string.Format("{0:yyyyMMdd}", DateTime.Now);
            string dir = $"/upload/image/{folderDate}";
            string localPath = _hostingEnvironment.ContentRootPath + $@"\upload\image\{folderDate}"; 
            // string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Uploads");
            if (Request.Form.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string ex = Path.GetExtension(file.FileName);
            fileName = FileHelper.GetFileName(fileName, localPath, ex);
            filePathName = fileName + ex;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            var orgUrl = Path.Combine(localPath, filePathName);

            using (var stream = new FileStream(orgUrl, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            //using (FileStream fs = System.IO.File.Create(orgUrl))
            //{
            //    await file.CopyToAsync(fs);
            //    await fs.FlushAsync();
            //}

            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                fileName = dir + "/" + filePathName
            });

        }
        [HttpPost]
        [Authorize]
        public ActionResult RemoveImage(string img)
        {
            try
            {             
                var filePath = img.Replace("/","\\");
              //  string localPath = _hostingEnvironment.ContentRootPath + $@"\upload\image\{folderDate}";
                var orgUrl = _hostingEnvironment.ContentRootPath + filePath;
                if (System.IO.File.Exists(orgUrl))
                {
                    System.IO.File.Delete(orgUrl);
                }

                AR.SetSuccess(Messages.AlertActionSuccess);
                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }

        }

    }
}
