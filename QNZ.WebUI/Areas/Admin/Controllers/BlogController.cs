using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class BlogController: BaseController
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public BlogController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/ProductCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostCategories.Include(d=>d.Posts).OrderByDescending(d=>d.Importance)
                .ProjectTo<PostCategoryBVM>(_mapper.ConfigurationProvider).ToListAsync());
        }
        
        public IActionResult Create()
        {
          
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Importance,Active")] PostCategoryIM im)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }

           
            var model = _mapper.Map<PostCategory>(im);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
                
            _context.Add(model);
            await _context.SaveChangesAsync();
       

            AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.PostCategory));
            return Json(AR);
        }
        
        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.PostCategories.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var im = _mapper.Map<PostCategoryIM>(model);
            return View(im);
        }
        
        
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Importance,Active")] PostCategoryIM im)
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
                var model = await  _context.PostCategories.FindAsync(id);
                
                model = _mapper.Map(im,model);
                model.UpdatedBy = User.Identity.Name;
                model.UpdatedDate = DateTime.Now;

                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostCategoryExists(im.Id))
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }
                else
                {
                    AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.PostCategory));
                    return Json(AR);
                }
            }
            //  return RedirectToAction(nameof(Index));

            AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.PostCategory));
            return Json(AR);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move(int oId, int toId)
        {
       

            try
            {
                var posts = await  _context.Posts.Where(d=>d.CategoryId == oId).ToListAsync();
                foreach (var post in posts)
                {
                    post.CategoryId = toId;
                    _context.Update(post);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostCategoryExists(toId))
                {
                    AR.Setfailure("未发现此分类");
                    return Json(AR);
                }
                else
                {
                    AR.Setfailure(string.Format(Messages.AlertUpdateFailure, EntityNames.PostCategory));
                    return Json(AR);
                }
            }
            //  return RedirectToAction(nameof(Index));

            AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.PostCategory));
            return Json(AR);
        }
        
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var postCategory  = await _context.PostCategories.FirstOrDefaultAsync(d=>d.Id==id);

            if (postCategory == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.PostCategories.Remove(postCategory);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        
        private bool PostCategoryExists(int id)
        {
            
            return _context.PostCategories.Any(e => e.Id == id);
        }
    }
}