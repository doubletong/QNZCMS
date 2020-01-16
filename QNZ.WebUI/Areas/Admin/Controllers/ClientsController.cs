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
using QNZ.Data.Enums;
using SIG.Infrastructure.Configs;
using X.PagedList;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class ClientsController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ClientsController(YicaiyunContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Clients
        public async Task<IActionResult> Index(string sort,string keyword, int? page)
        {
            var vm = new ClientListVM
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword
            };
            var pageSize = SettingsManager.Client.PageSize;

            var query = _context.Clients.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.ClientName.Contains(keyword) || d.Homepage.Contains(keyword));
         
            ViewData["ImportanceSortParm"] = sort == "importance" ? "importance_desc" : "importance";
            ViewData["nameSortParm"] = sort == "title" ? "title_desc" : "title";
            ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

            query = sort switch
            {              
                "name" => query.OrderBy(s => s.ClientName),
                "name_desc" => query.OrderByDescending(s => s.ClientName),
                "date" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "importance" => query.OrderBy(s => s.CreatedDate),
                "importance_desc" => query.OrderByDescending(s => s.CreatedDate),
                _ => query.OrderByDescending(s => s.Importance),
            };

            vm.TotalCount = await query.CountAsync();
            var clients = await query
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<ClientBVM>(_mapper.ConfigurationProvider).ToListAsync();

            vm.Clients = new StaticPagedList<ClientBVM>(clients, vm.PageIndex, pageSize, vm.TotalCount);

            ViewBag.PageSizes = new SelectList(Site.PageSizes());
            return View(vm);
        }

        // GET: Admin/Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            // var model = _mapper.Map<ClientIM>(productCategory);
            return View(model);
        }


        // GET: Admin/Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new ClientIM
            {
                Active = true,
                Importance = 0
            };
            if (id == null)
            {
                return View(vm);
            }
         
            var category = await _context.Clients.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ClientIM>(category);

            //var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.ARTICLECATEGORY && d.ObjectId == category.Alias);

            //if (pm != null)
            //{
            //    model.SEOTitle = pm.Title;
            //    model.SEOKeywords = pm.Keywords;
            //    model.SEODescription = pm.Description;
            //}

            return View(model);
          
        }

        // POST: Admin/Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Id,ClientName,LogoURL,Importance,Active,SEOTitle,SEOKeywords,SEODescription")] ClientIM im, int id = 0)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

            if (id == 0)
            {
             
                var model = _mapper.Map<Client>(im);
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                _context.Add(model);

                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Client));
                return Json(AR);
            }
          
                if (id != im.Id)
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }


                try
                {
                    var model = await _context.Clients.FindAsync(id);
                    model = _mapper.Map(im, model);

                    model.UpdatedBy = User.Identity.Name;
                    model.UpdatedDate = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    //var pm = new PageMeta
                    //{
                    //    Title = im.SEOTitle,
                    //    Description = im.SEODescription,
                    //    Keywords = im.SEOKeywords,
                    //    ModuleType = (short)ModuleType.ARTICLECATEGORY,
                    //    ObjectId = im.Alias
                    //};

                    //await CreatedUpdatedPageMetaAsync(_context, pm);

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Client));
                    return Json(AR);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(im.Id))
                    {
                        AR.Setfailure("未发现此分类");
                        return Json(AR);
                    }
                    else
                    {
                        AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.Client));
                        return Json(AR);
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

            var c = await _context.Clients.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Clients.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Clients.Where(d => ids.Contains(d.Id)).ToListAsync();

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


            var c = await _context.Clients.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Clients.Remove(c);
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


        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

     
    }
}
