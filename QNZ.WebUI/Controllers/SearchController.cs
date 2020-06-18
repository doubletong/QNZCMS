using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QNZ.Data;
using QNZ.Infrastructure.Cache;
using QNZ.Model.ViewModel;
using X.PagedList;

namespace QNZCMS.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public SearchController(YicaiyunContext context, IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _context = context;
            _cacheService = cacheService;
        }
        public async Task<IActionResult> IndexAsync(string keyword, int? page)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return View();
            }

            var vm = new SearchArticlePageVM
            {
                Keyword = keyword,
                PageIndex = page ?? 1,
                PageSize = 12  
            };

            var query = _context.Articles.Include(d=>d.Category).Where(d => d.Active == true && (d.Title.Contains(keyword) || d.Body.Contains(keyword))).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Articles = new StaticPagedList<ArticleVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);

                    

            return View(vm);
        }
    }
}