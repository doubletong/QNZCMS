using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QNZ.Data;

using QNZ.Model.Front.ViewModel;
using QNZ.Model.Settings;
using QNZ.Model.Site.ViewModel;
using QNZCMS.Services;
using Serilog.Events;
using X.PagedList;

using Site = QNZ.Infrastructure.Helper.Site;

namespace QNZCMS.Controllers
{
    public class ErrorLogsController : BaseController
    {
        private readonly IWritableOptions<SiteLogSet> _writableLocations;
     
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly IConfiguration _config;
        public ErrorLogsController(QNZContext context, 
            IMapper mapper,IConfiguration config,IWritableOptions<SiteLogSet> writableLocations)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _writableLocations = writableLocations;
        }
        // GET
        public async Task<IActionResult> Index(int? page, string orderby = "date", string sort = "desc")
        {
            try
            {
                var vm = new LogListVM()
                {
                    PageIndex = page??1,
                    PageSize = _config.GetValue<int>("Modules:Log:Site:PageSize"),
                    OrderBy = orderby,
                    Sort = sort
                };

                var query = _context.Logs.AsNoTracking().AsQueryable();
                var logEvent = LogEventLevel.Error.ToString();
                query = query.Where(d => d.Level == logEvent);

                vm.TotalCount = await query.CountAsync();
                var goSort = $"{orderby}_{sort}";    
                query = goSort switch
                {
                    "username_asc" => query.OrderBy(s => s.UserName),
                    "username_desc" => query.OrderByDescending(s => s.UserName),
                    "date_asc" => query.OrderBy(s => s.TimeStamp),
                    "date_desc" => query.OrderByDescending(s => s.TimeStamp),
                    _ => query.OrderByDescending(s => s.Id),
                };

                var logs = await query
                    .Skip((vm.PageIndex - 1) * vm.PageSize)
                    .Take(vm.PageSize).ProjectTo<LogVM>(_mapper.ConfigurationProvider).ToListAsync();
          
                vm.Logs = new StaticPagedList<LogVM>(logs, vm.PageIndex, vm.PageSize, vm.TotalCount);

                ViewBag.PageSizes = new SelectList(Site.PageSizes());

                return View(vm);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("日志列表读取错误:{@error}", ex);  
                // return null;  
                throw;
            }
           
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }

        [HttpPost]
        public JsonResult PageSizeSet(int pageSize)
        {
            // var pageSet = _config.GetValue<Log>("Modules:Log");
            // pageSet.Site.PageSize = pageSize;
            try
            {
                _writableLocations.Update(opt => {
                    opt.PageSize = pageSize;
                });
                return Json(AR);
            }
            catch (Exception ex)
            {
                AR.Setfailure(ex.Message);
                return Json(AR);
            }
        }
    }
}