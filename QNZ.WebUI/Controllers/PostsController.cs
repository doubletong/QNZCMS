using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Front.ViewModel;

namespace QNZCMS.Controllers
{
    public class PostsController : BaseController
    {

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public PostsController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        // GET
        public async Task<IActionResult> Index(int? cid)
        {
            var query = _context.Posts.Include(d => d.Category).Where(d => d.Active==true).AsNoTracking();
            if (cid != null)
            {
                query = query.Where(d => d.CategoryId == cid);
            }

            var list = await query
                .OrderByDescending(d => d.Id).Skip(0).Take(15).ToListAsync();

            var vm = new PostListVM
            {
                Posts = list,
                CategoryId = cid
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> LoadPosts([FromForm] int? cid, int page)
        {
            var query = _context.Posts.Include(d => d.Category).AsNoTracking();
            if (cid != null)
            {
                query = query.Where(d => d.CategoryId == cid);
            }

            var list = await query
                .Where(d => d.Active==true).OrderByDescending(d => d.Id).Skip(page * 15).Take(15).ToListAsync();



            return PartialView("_LoadPosts", list);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var post = await _context.Posts.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id && d.Active==true);
            if (post == null)
                return NotFound();

            post.ViewCount++;
            _context.Update(post);
            await _context.SaveChangesAsync();

            var vm = new PostDetailVM
            {
                PostDetail = post,
                PostPrev = await _context.Posts.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < post.Id && d.Active==true),
                PostNext = await _context.Posts.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > post.Id && d.Active==true)
            };

            return View(vm);


        }
    }
}