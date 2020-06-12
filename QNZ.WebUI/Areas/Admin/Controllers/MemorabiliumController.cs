using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Infrastructure.Helper;
using QNZ.Resources.Admin;
using X.PagedList;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class MemorabiliumController : BaseController
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public MemorabiliumController(IWebHostEnvironment hostingEnvironment, YicaiyunContext context, IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _mapper = mapper;      
        }

        // GET: Admin/Memorabilias
        public async Task<IActionResult> Index(string keyword,string orderby, string sort, int? page)
        {
           MemorabiliaListVM vm = new MemorabiliaListVM()
            {
                PageIndex = page??1,
                PageSize = 15,
                Keyword = keyword,
                OrderBy = orderby,
                Sort = sort
            };
        

            var query = _context.Memorabilia.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Description.Contains(keyword));        


            vm.TotalCount = await query.CountAsync();
            var gosort = $"{orderby}_{sort}";         

            query = gosort switch
            {
   
                "description_asc" => query.OrderBy(s => s.Description),
                "description_desc" => query.OrderByDescending(s => s.Description),
                "at_asc" => query.OrderBy(s => s.DateAt),
                "at_desc" => query.OrderByDescending(s => s.DateAt),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),

                _ => query.OrderByDescending(s => s.Id),
            };

            var pages = await query
                .Skip((vm.PageIndex - 1) * vm.PageSize)
                .Take(vm.PageSize).ProjectTo<MemorabiliaVM>(_mapper.ConfigurationProvider).ToListAsync();
          

            vm.Memorabilias = new StaticPagedList<MemorabiliaVM>(pages, vm.PageIndex, vm.PageSize, vm.TotalCount);

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }



        //GET: Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new MemorabiliaIM();
            if (id == null)
            {

                vm.Active = true;
                vm.DateAt = DateTime.Now;

                return View(vm);
            }

            var page = await _context.Memorabilia.SingleOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            vm = _mapper.Map<MemorabiliaIM>(page);
         

            return View(vm);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,DateAt,Description,Active")] MemorabiliaIM page)
        {

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
                if (page.Id > 0)
                {

                    var model = await _context.Memorabilia.SingleOrDefaultAsync(d => d.Id == page.Id);
                    model = _mapper.Map(page, model);               
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = User.Identity.Name;

                    _context.Update(model);
                    await _context.SaveChangesAsync();                 

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Memorabilia));
                 

                }
                else
                {
                    var model = _mapper.Map<Memorabilium>(page);         
          
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;

                    _context.Add(model);
                    await _context.SaveChangesAsync();                

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Memorabilia));                  


                }

            

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(page.Id))
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



        //[HttpPost]
        //public JsonResult PageSizeSet(int pageSize)
        //{
        //    try
        //    {
        //        var xmlFile = PlatformServices.Default.MapPath("/Config/PageSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("PageSize").SetValue(pageSize);
        //        doc.Save(xmlFile);


        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy(int id)
        {

            var article = await _context.Memorabilia.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
      
            article.Active = false;
            article.Description = $"{article.Description}【拷贝】";

            _context.Memorabilia.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _context.Memorabilia.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Memorabilia.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);

        }
        // POST: Admin/Articles/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Memorabilia.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Memorabilia.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids,bool isLock)
        {

            var c = await _context.Memorabilia.Where(d => ids.Contains(d.Id)).ToListAsync();

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
        

  

        private bool PageExists(int id)
        {
            return _context.Memorabilia.Any(e => e.Id == id);
        }
    }
}
