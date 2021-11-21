using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Infrastructure.Cache;
using QNZ.Model.ViewModel;
using X.PagedList;

namespace QNZCMS.Controllers
{
    public class SolutionsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly ICacheService _cacheService;
        public SolutionsController(QNZContext context, IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _context = context;
            _cacheService = cacheService;
        }
        public async Task<IActionResult> IndexAsync()
        {      

            var vm = await _context.Solutions.Where(d => d.Active == true)
                .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id)
                .ProjectTo<SolutionVM>(_mapper.ConfigurationProvider).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Solutions                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            if(!string.IsNullOrEmpty(article.RelatedProducts))
            {
                var pids = article.RelatedProducts.Split("|");
              
                var listOfInts = pids.Select<string, int>(q => Convert.ToInt32(q));

                ViewData["Products"] = _context.Products.Where(d => listOfInts.Contains(d.Id)).OrderByDescending(d=>d.Importance);               
            }
                       

          // article.ViewCount++;
         //   _context.Update(article);
        //    await _context.SaveChangesAsync();

            //var vm = new ArticleDetailVM
            //{
            //    ArticleDetail = article,
            //    ArticlePrev = await _context.Articles.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < article.Id && d.Active == true && d.CategoryId == article.CategoryId),
            //    ArticleNext = await _context.Articles.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > article.Id && d.Active == true && d.CategoryId == article.CategoryId)
            //};


            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.SOLUTION);

            return View(article);
        }

        public async Task<IActionResult> DownloadsAsync(int? page)
        {
            string alias = "downloads";

            var vm = new DocumentPageVM
            {
                PageIndex = page ?? 1,
                PageSize = 15

            };

            var query = _context.Documents.Where(d => d.Active == true && d.Category.Alias == alias).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<DocumentVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Documents = new StaticPagedList<DocumentVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }

    }
}
