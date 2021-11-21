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
using QNZ.Resources.Common;
using QNZ.Data;
using X.PagedList;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Helper;
using QNZ.Data.Enums;
using System.Xml.Linq;
using Microsoft.Extensions.PlatformAbstractions;
using QNZ.Model.Administrator;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Policy = "Permission")]
    public class VideosController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        public VideosController(QNZContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: Admin/Videos
        public async Task<IActionResult> Index(string keyword, string orderby, string sort, int? page)
        {
            var vm = new VideoListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword,         
                PageSize = 10,
                   OrderBy = orderby,
                Sort = sort
            };

            //var pageSize = SettingsManager.Video.PageSize;
            var query = _context.Videos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

           

            var gosort = $"{orderby}_{sort}";
            query = gosort switch
            {
                "importance_asc" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                "title_asc" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
              
                _ => query.OrderByDescending(s => s.Id),
            };


            vm.TotalCount = await query.CountAsync();
            var clients = await query     
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize).ProjectTo<VideoBVM>(_mapper.ConfigurationProvider).ToListAsync();
      

            vm.Videos = new StaticPagedList<VideoBVM>(clients, vm.PageIndex, vm.PageSize, vm.TotalCount);

         

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }

      

      
        // GET: Admin/Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new VideoIM();
            if (id == null)
            {
                vm.Active = true;
                vm.Importance = 0;

            }
            else
            {
                var article = await _context.Videos.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<VideoIM>(article);



            }
           

            return View(vm);

        }

        // POST: Admin/Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VideoIM article)
        {
       

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
               

                if (article.Id > 0)
                {
                    var model = await _context.Videos.FirstOrDefaultAsync(d => d.Id == article.Id);
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

                    AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Video));
            

                }
                else
                {
                    var model = _mapper.Map<Video>(article);

                    model.CreatedBy = User.Identity.Name;
                    model.CreatedDate = DateTime.Now;


                    //model.Body = WebUtility.HtmlEncode(page.Body);

                    _context.Add(model);
                    await _context.SaveChangesAsync();
               

                    AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Video));
                 
                }

             


                return Json(AR);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(article.Id))
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
        //        var xmlFile = PlatformServices.Default.MapPath("/Config/VideoSettings.config");
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

            var article = await _context.Videos.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            if (article == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            article.Id = 0;
            article.CreatedBy = User.Identity.Name;
            article.CreatedDate = DateTime.Now;
        
            article.Active = false;
            article.Recommend = false;
            article.Title = $"{article.Title}【拷贝】"; 

            _context.Videos.Add(article);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        // POST: Admin/Videos/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.Videos.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Videos.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        // POST: Admin/Videos/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(int[] ids)
        {

            var c = await _context.Videos.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Videos.RemoveRange(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsLock(int[] ids, bool isLock)
        {

            var c = await _context.Videos.Where(d => ids.Contains(d.Id)).ToListAsync();

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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsTop(int[] ids, bool isTop)
        {

            var c = await _context.Videos.Where(d => ids.Contains(d.Id)).ToListAsync();

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }
            foreach (var item in c)
            {
                item.Recommend = isTop ? false : true;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool VideoExists(int id)
        {
            return _context.Videos.Any(e => e.Id == id);
        }
    }
}
