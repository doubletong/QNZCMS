using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using QNZ.Resources.Admin;
using QNZ.Data;
using X.PagedList;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;
using QNZ.Data.Enums;
using System.Xml.Linq;
using Microsoft.Extensions.PlatformAbstractions;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class StaffsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public StaffsController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: Admin/Staffs
        public async Task<IActionResult> Index(string keyword, string orderby, string sort, int? orgId, int? page)
        {
            var vm = new StaffListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword,
                OrganizationId = orgId,
                PageSize = 10,
                   OrderBy = orderby,
                Sort = sort
            };

            //var pageSize = SettingsManager.Staff.PageSize;
            var query = _context.Staffs.Include(d=>d.Organization).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Name.Contains(keyword));

            if (orgId > 0)
                query = query.Where(d => d.OrganizationId == orgId);


            var gosort = $"{orderby}_{sort}";
            query = gosort switch
            {
                "importance" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                "name" => query.OrderBy(s => s.Name),
                "name_desc" => query.OrderByDescending(s => s.Name),
                "date" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
              
                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query     
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<StaffBVM>(_mapper.ConfigurationProvider).ToListAsync();
      

            vm.Staffs = new StaticPagedList<StaffBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);

            var categories = await _context.Organizations.AsNoTracking()
                 .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Title");

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

      

      
        // GET: Admin/Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new StaffIM();
            if (id == null)
            {
                vm.Active = true;
               
             
            }
            else
            {
                var article = await _context.Staffs.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<StaffIM>(article);

                var pm = await _context.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == (short)ModuleType.STAFF && d.ObjectId == vm.Id.ToString());

                if (pm != null)
                {
                    vm.SEOTitle = pm.Title;
                    vm.SEOKeywords = pm.Keywords;
                    vm.SEODescription = pm.Description;
                }

            }
            var categories = await _context.Organizations.AsNoTracking()
               .OrderByDescending(d => d.Importance).ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Title");

           

            return View(vm);

        }

        // POST: Admin/Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffIM article)
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
                    ModuleType = (short)ModuleType.STAFF
                  
                };


                if (article.Id > 0)
                {
                    var model = await _context.Staffs.FirstOrDefaultAsync(d => d.Id == article.Id);
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

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Staff));
                    pm.ObjectId = model.Id.ToString();


                }
                else
                {
                    var model = _mapper.Map<Staff>(article);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;


                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    pm.ObjectId = model.Id.ToString();

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Staff));
                 
                }

             

                await CreatedUpdatedPageMetaAsync(_context, pm);

                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(article.Id))
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
                var xmlFile = PlatformServices.Default.MapPath("/Config/StaffSettings.config");
                XDocument doc = XDocument.Load(xmlFile);

                var item = doc.Descendants("Settings").FirstOrDefault();
                item.Element("PageSize").SetValue(pageSize);
                doc.Save(xmlFile);


                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy(int id)
        {

            var article = await _context.Staffs.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
           
            article.Active = false;
          
            article.Name = $"{article.Name}【拷贝】"; 

            _context.Staffs.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        // POST: Admin/Staffs/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.Staffs.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Staffs.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Staffs/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Staffs.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Staffs.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Staffs.Where(d => ids.Contains(d.Id)).ToListAsync();

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



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> IsTop(int[] ids, bool isTop)
        //{

        //    var c = await _context.Staffs.Where(d => ids.Contains(d.Id)).ToListAsync();

        //    if (c == null)
        //    {
        //        AR.Setfailure(Messages.HttpNotFound);
        //        return Json(AR);
        //    }
        //    foreach (var item in c)
        //    {
        //        item.Recommend = isTop ? false : true;
        //        _context.Entry(item).State = EntityState.Modified;
        //    }

        //    await _context.SaveChangesAsync();

        //    return Json(AR);
        //}

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.Id == id);
        }
    }
}
