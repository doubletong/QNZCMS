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

namespace QNZCMS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public ProductsController(YicaiyunContext context, IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _context = context;
            _cacheService = cacheService;
        }
        public async Task<IActionResult> IndexAsync(int? cid)
        {
            var vm = new ProductPageVM
            {
               
                Categories = await _context.ProductCategories.Where(d => d.Active == true)
                .OrderByDescending(d => d.Importance).ThenBy(d => d.Id)
                .ProjectTo<ProductCategoryVM>(_mapper.ConfigurationProvider).ToListAsync(),
                
            };

            if (cid > 0)
            {
                vm.Category = vm.Categories.FirstOrDefault(d => d.Id == cid);

                vm.Products = await _context.Products.Where(d => d.CategoryId == cid)
                        .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id)
                        .ProjectTo<ProductVM>(_mapper.ConfigurationProvider).ToListAsync();

                vm.CategoryId = cid;
            }
            else
            {
               vm.Category = vm.Categories.FirstOrDefault();

                if (vm.Category != null)
                {
                    vm.Products = await _context.Products.Where(d => d.CategoryId == vm.Category.Id)
                        .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id)
                        .ProjectTo<ProductVM>(_mapper.ConfigurationProvider).ToListAsync();

                    vm.CategoryId = vm.Category.Id;
                }
            }
            if (vm.Category.VideoId > 0)
            {
                var video = await _context.Videos.FirstOrDefaultAsync(d => d.Id == vm.Category.VideoId);
                vm.Video = _mapper.Map<VideoVM>(video);
            }
         
          
        

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }

        public IActionResult Download(int id)
        {
            return View();
        }
    }
}
