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
using PagedList.Core;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.ViewModel;
using SIG.Resources.Admin;
using QNZ.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    //[Authorize(Policy = "Permission")]
    public class ArticlesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public ArticlesController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: Admin/Articles
        public async Task<IActionResult> Index(string keyword, int? page)
        {
            var vm = new ArticleListVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword
            };
            var pageSize = 10;

            var query = _context.Articles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));


            vm.TotalCount = await query.CountAsync();
            var clients = await query.OrderByDescending(d => d.Id)
                .ProjectTo<ArticleBVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            // var list = _mapper.Map<IEnumerable<ProductVM>>(agents);

            vm.Articles = new StaticPagedList<ArticleBVM>(clients, vm.PageIndex, pageSize, vm.TotalCount);


            return View(vm);
        }

        // GET: Admin/Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Admin/Articles/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/Articles/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ArticleIM article)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    _context.Add(article);
        //    //    await _context.SaveChangesAsync();
        //    //    return RedirectToAction(nameof(Index));
        //    //}
        //    //return View(article);

        //    if (!ModelState.IsValid)
        //    {
        //        AR.Setfailure(GetModelErrorMessage());
        //        return Json(AR);
        //    }
        //    try
        //    {


        //        var model = _mapper.Map<Article>(article);

        //        model.CreatedBy = User.Identity.Name;
        //        model.CreatedDate = DateTime.Now;


        //        //model.Body = WebUtility.HtmlEncode(page.Body);

        //        _context.Add(model);
        //        await _context.SaveChangesAsync();

        //        AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Article));
        //        return Json(AR);

        //    }
        //    catch (Exception er)
        //    {
        //        AR.Setfailure(er.Message);
        //        return Json(AR);
        //    }
        //}

        // GET: Admin/Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vm = new ArticleIM();
            if (id == null)
            {
                vm.Active = true;
                vm.PubDate = DateTime.Now;

                return View(vm);
            }
            else
            {
                var article = await _context.Articles.FindAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                vm = _mapper.Map<ArticleIM>(article);
                return View(vm);
            }     
            
        }

        // POST: Admin/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleIM article)
        {
            if (id != article.Id)
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

                var model = await _context.Articles.SingleOrDefaultAsync(d => d.Id == id);

                model = _mapper.Map(article, model);
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;


                _context.Update(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Post));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(article.Id))
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

        // POST: Admin/Articles/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
     

            var c = await _context.Articles.FirstOrDefaultAsync(d => d.Id == id);

            if (c == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Articles.Remove(c);
            await _context.SaveChangesAsync();

            return Json(AR);
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
