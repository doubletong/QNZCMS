using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Administrator.ViewModel;
using X.PagedList;
using Microsoft.Extensions.Configuration;
using QNZ.Infrastructure.Helper;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace QNZCMS.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("qnz-admin/[controller]/[action]")]
    public class WebpartsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly IConfiguration _config;
        public WebpartsController(IWebHostEnvironment hostingEnvironment, QNZContext context, IMapper mapper, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _mapper = mapper;
            _config = config;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(string keyword, int? page, string orderby = "importance", string sort = "desc")
        {
            var vm = new WebpartListVM()
            {
                PageIndex = page??1,
                PageSize = _config.GetValue<int>("Modules:Webpart:Administrator:PageSize"),
                Keyword = keyword,
                OrderBy = orderby,
                Sort = sort
            };
        
            var query = _context.Webparts.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword) || d.Body.Contains(keyword));        


            vm.TotalCount = await query.CountAsync();
            var goSort = $"{orderby}_{sort}";         

            query = goSort switch
            {
                "title_asc" => query.OrderBy(s => s.Title),
                "title_desc" => query.OrderByDescending(s => s.Title),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "importance_asc" => query.OrderBy(s => s.Importance),
                "importance_desc" => query.OrderByDescending(s => s.Importance),
                _ => query.OrderByDescending(s => s.Id),
            };

            var pages = await query
                .Skip((vm.PageIndex - 1) * vm.PageSize)
                .Take(vm.PageSize).ProjectTo<WebpartVM>(_mapper.ConfigurationProvider).ToListAsync();
          

            vm.Webparts = new StaticPagedList<WebpartVM>(pages, vm.PageIndex, vm.PageSize, vm.TotalCount);

            ViewBag.PageSizes = new SelectList(Site.PageSizes());

            return View(vm);
        }
    }
}
