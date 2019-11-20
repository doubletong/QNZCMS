using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class PostController : BaseController
    {
        // GET
        private IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public PostController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        
        
        public async Task<IActionResult> Index(int? page,int? CategoryId)
        {
            var vm = new PostPageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                CategoryId = CategoryId
               
            };
            const int pageSize = 12;

            var query = _context.Posts.Include(d=>d.PostCategory).AsNoTracking().AsQueryable();

            if (vm.CategoryId > 0)
            {
                query = query.Where(d => d.CategoryId == vm.CategoryId);
            }
          
            vm.TotalCount = await query.CountAsync();
            var teams = await query.OrderByDescending(d=>d.CreatedDate).ThenByDescending(d=>d.Id)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<PostBVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            vm.Posts = new StaticPagedList<PostBVM>(teams, vm.PageIndex, pageSize, vm.TotalCount);

            ViewData["Categories"] = new SelectList(await _context.PostCategories.AsNoTracking()
                .OrderByDescending(d=>d.Importance).ToListAsync(), "Id", "Title");

            return View(vm);
        }
        
        public async Task<IActionResult> Create()
        {
           
            ViewData["Categories"] = new SelectList(await _context.PostCategories.AsNoTracking()
                .OrderByDescending(d=>d.Importance).ToListAsync(), "Id", "Title");

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( PostIM post)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            try
            {
              

                var model = _mapper.Map<Post>(post);

                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                    
                    
                //model.Body = WebUtility.HtmlEncode(page.Body);

                _context.Add(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Post));
                return Json(AR);

            }
            catch(Exception er)
            {
                AR.Setfailure(er.Message);
                return Json(AR);
            }
          
        }
        
        // GET: Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<PostIM>(page);
         
            ViewData["Categories"] = new SelectList(await _context.PostCategories.AsNoTracking()
                .OrderByDescending(d=>d.Importance).ToListAsync(), "Id", "Title");
            
            return View(model);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostIM post)
        {
            if (id != post.Id)
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
          
                var model = await _context.Posts.SingleOrDefaultAsync(d => d.Id == id);

                model = _mapper.Map(post, model);
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;

           
                _context.Update(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Post));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
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
        
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var post  = await _context.Posts.FirstOrDefaultAsync(d=>d.Id==id);

            if (post == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        
        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

    }
}