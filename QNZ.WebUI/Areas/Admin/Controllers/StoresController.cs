using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

using SIG.Model.Admin.InputModel;
using SIG.Model.Admin.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class StoresController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;     

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public StoresController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Stores
        public async Task<IActionResult> Index(string keyword, int? page)
        {
            StorePageVM vm = new StorePageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
             
                Keyword = keyword
            };
            var pageSize = 10;

            var query = _context.Stores.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Name.Contains(keyword));        

            vm.TotalCount = await query.CountAsync();
            var agents = await query.Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var list = _mapper.Map<IEnumerable<StoreVM>>(agents);

            vm.Stores = new StaticPagedList<StoreVM>(list, vm.PageIndex, pageSize, vm.TotalCount);

         

            return View(vm);
        }

        // GET: Admin/Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores             
                .SingleOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StoreVM>(store);
            model.Coordinate = $"{store.Longitude},{store.Latitude}";

            return View(model);
        }

        // GET: Admin/Stores/Create
        public async Task<IActionResult> Create()
        {
          
            ViewData["Province"] = new SelectList(await _context.Provinces.AsNoTracking().ToListAsync(), "Name", "Name");
            return View();
        }

        // POST: Admin/Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Thumbnail,Contact,Phone,Address,Province,City,District,Coordinate,Body")] StoreIM store)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

            var model = _mapper.Map<Store>(store);

                       
            if(!string.IsNullOrEmpty(store.Coordinate) && store.Coordinate.Contains(","))
            {
                var cod = store.Coordinate.Split(",");
                decimal.TryParse(cod[0], out decimal lng);
                decimal.TryParse(cod[1], out decimal lat);
                model.Longitude = lng;
                model.Latitude = lat;
            }

            _context.Add(model);
            await _context.SaveChangesAsync();

            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Store));
            return Json(AR);
        }

        // GET: Admin/Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores.SingleOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StoreIM>(store);
            model.Coordinate = $"{store.Longitude},{store.Latitude}";

            ViewData["Province"] = new SelectList(_context.Provinces, "Name", "Name", store.Province);
            ViewData["City"] = new SelectList(_context.Cities.Where(d=>d.Province.Name == store.Province).ToList(), "Name", "Name", store.City);
            ViewData["District"] = new SelectList(_context.Districts.Where(d=>d.City.Name == store.City).ToList(), "Name", "Name", store.District);
        
            return View(model);
        }

        // POST: Admin/Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Thumbnail,Contact,Phone,Address,Province,City,District,Coordinate,Body")] StoreIM store)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            var model = await _context.Stores.SingleOrDefaultAsync(d => d.Id == id);
            if (model == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            model = _mapper.Map(store, model);

          


            if (!string.IsNullOrEmpty(store.Coordinate) && store.Coordinate.Contains(","))
            {
                var cod = store.Coordinate.Split(",");
                decimal.TryParse(cod[0], out decimal lng);
                decimal.TryParse(cod[1], out decimal lat);
                model.Longitude = lng;
                model.Latitude = lat;
            }

          
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Store));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(store.Id))
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

        // GET: Admin/Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
       
                .SingleOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StoreVM>(store);
            model.Coordinate = $"{store.Longitude},{store.Latitude}";
           // model.HasData = store.Orders.Any() || store.Carts.Any();

            return View(model);
        }

        // POST: Admin/Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Stores.Include(d=>d.Products)
                .Include(d=>d.OrderDetails).Include(d=>d.CartItems).SingleOrDefaultAsync(m => m.Id == id);

            if (store.Products.Any() || store.OrderDetails.Any() || store.CartItems.Any())
            {
                TempData["Error"] = "已存在相关数据，不可以删除。";
            }
         
            if (!string.IsNullOrEmpty(store.Thumbnail))
            {
                var filePath = _hostingEnvironment.WebRootPath + store.Thumbnail.Replace("/", "\\");
                if(System.IO.File.Exists(filePath)){
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Stores.Remove(store);
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

            //var files = HttpContext.Request.Form.Files;
            //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            //foreach (var file in files)
            //{
            //    if (file.Length > 0)
            //    {
            //        var fileName = ContentDispositionHeaderValue.Parse
            //            (file.ContentDisposition).FileName.Trim('"');
            //        //  System.Console.WriteLine(fileName);
            //        //  file.CopyTo(Path.Combine(uploads, fileName));
            //        using (var stream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
            //        {
            //            await file.CopyToAsync(stream);
            //        }
            //    }
            //}

            //return Ok("very sood");
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }
    }
}
