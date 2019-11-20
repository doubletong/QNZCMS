using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QNZ.Data;
using QNZ.Model.ViewModel;

namespace QNZCMS.Controllers
{
  
    public class NewsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public NewsController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(int? page)
        {
            var vm = new ArticlePageVM()
            {
                Categories = await _context.ArticleCategories.Where(d => d.Active == true)
                    .OrderByDescending(d => d.Importance)
                    .ProjectTo<ArticleCategoryVM>(_mapper.ConfigurationProvider).ToListAsync(),
                PageIndex = page == null || page <= 0 ? 1 : page.Value
            };
            var pageSize = 20;

            var query = _context.Articles.Where(d => d.Active==true).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Id).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            vm.Articles = new StaticPagedList<ArticleVM>(articles, vm.PageIndex, pageSize, vm.TotalCount);

            return View(vm);
        }

        public async Task<IActionResult> List(int? page)
        {
            var vm = new ArticlePageVM()
            {
                Categories = await _context.ArticleCategories.Where(d => d.Active == true)
                    .OrderByDescending(d => d.Importance)
                    .ProjectTo<ArticleCategoryVM>(_mapper.ConfigurationProvider).ToListAsync(),
                PageIndex = page == null || page <= 0 ? 1 : page.Value
            };
            var pageSize = 20;

            var query = _context.Articles.Where(d => d.Active == true).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Id).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            vm.Articles = new StaticPagedList<ArticleVM>(articles, vm.PageIndex, pageSize, vm.TotalCount);

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var post = await _context.Articles.FirstOrDefaultAsync(d => d.Id == id && d.Active==true);
            if (post == null)
                return NotFound();

            post.ViewCount++;
            _context.Update(post);
            await _context.SaveChangesAsync();

            var vm = new ArticleDetailVM
            {
                ArticleDetail = post,
                ArticlePrev = await _context.Articles.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < post.Id && d.Active==true),
                ArticleNext = await _context.Articles.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > post.Id && d.Active==true)
            };

            return View(vm);
        }
    }
}
