using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
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
        private readonly QNZContext _context;
        private readonly ICacheService _cacheService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProductsController(QNZContext context, IMapper mapper, ICacheService cacheService, IWebHostEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _context = context;
            _cacheService = cacheService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> IndexAsync(int? cid)
        {
            var allCategories = await _context.ProductCategories.Where(d => d.Active == true)
                .OrderByDescending(d => d.Importance).ThenBy(d => d.Id)
                .ProjectTo<ProductCategoryVM>(_mapper.ConfigurationProvider).ToListAsync();
            var vm = new ProductPageVM
            {
               
                Categories = allCategories.Where(d=>d.ParentId==null)

            };

            if (cid > 0)
            {
                vm.Category = allCategories.FirstOrDefault(d => d.Id == cid);

                vm.SubCategories = allCategories.Where(d => d.ParentId == cid || d.ParentId == vm.Category.ParentId).Where(d=>d.ParentId !=null );
                var ids = vm.SubCategories.Select(d => d.Id).ToList();
                ids.Add(cid.Value);

                if (vm.Category.ParentId == null)
                {
                    vm.Products = await _context.Products.Where(d => ids.Contains(d.CategoryId) && d.Active==true)
                     .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id)
                     .ProjectTo<ProductVM>(_mapper.ConfigurationProvider).ToListAsync();
                }
                else
                {
                    vm.Products = await _context.Products.Where(d => d.CategoryId== cid && d.Active == true)
                    .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id)
                    .ProjectTo<ProductVM>(_mapper.ConfigurationProvider).ToListAsync();
                }
             

                vm.CategoryId = cid;
            }
            else
            {
               vm.Category = vm.Categories.FirstOrDefault();

             
                if (vm.Category != null)
                {
                    vm.SubCategories = allCategories.Where(d => d.ParentId == vm.Category.Id);
                    var ids = vm.SubCategories.Select(d => d.Id).ToList();
                    ids.Add(vm.Category.Id);


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

        public async Task<IActionResult> DownloadAsync(int id)
        {
            var file = await _context.Products.FindAsync(id);
            if (file == null)
                return NotFound();

            var path = _hostingEnvironment.WebRootPath + file.FileUrl;
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            file.DownloadCount++;

            _context.Update(file);
            await _context.SaveChangesAsync();

            var fs = new FileStream(path, FileMode.Open);
            return File(fs, "application/octet-stream", Path.GetFileName(path));
        }
    }
}
