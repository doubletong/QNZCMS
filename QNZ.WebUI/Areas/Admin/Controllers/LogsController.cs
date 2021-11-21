using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QNZ.Data;
using QNZ.Infrastructure.Helper;
using QNZ.Model.Administrator;
using QNZ.Model.Administrator.ViewModel;
using QNZ.Model.Settings;
using QNZ.Resources.Common;
using QNZCMS.Services;
using Serilog.Context;
using Serilog.Events;
using X.PagedList;

namespace QNZCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("qnz-admin/[controller]/[action]")]
    //[Authorize(Policy = "Permission")]
    public class LogsController : BaseController
    {
    
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        private readonly IConfiguration _config;
        private readonly IWritableOptions<AdminLogSet> _writableLocations;
        public LogsController( QNZContext context, IMapper mapper,IConfiguration config,
            IWritableOptions<AdminLogSet> writableLocations)
        {
         
            _context = context;
            _mapper = mapper;
            _config = config;
            _writableLocations = writableLocations;
        }
        // GET
         public async Task<IActionResult> Index(string keyword, int? page, string orderby = "date", string sort = "desc")
        {
            try
            {
                var vm = new LogListVM()
                {
                    PageIndex = page??1,
                    PageSize = _config.GetValue<int>("Modules:Log:Administrator:PageSize"),
                    Keyword = keyword,
                    OrderBy = orderby,
                    Sort = sort
                };

                var query = _context.Logs.AsNoTracking().AsQueryable();
                var logEvent = LogEventLevel.Information.ToString();
                query = query.Where(d => d.Level == logEvent);
                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(d => d.Message.Contains(keyword) || d.LogEvent.Contains(keyword));        


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
         
         [HttpPost]
         public JsonResult PageSizeSet(int pageSize)
         {
             try
             {
                 _writableLocations.Update(opt => {
                     opt.PageSize = pageSize;
                 });
                
                 const string logEvent = "分页设置";
                 using (LogContext.PushProperty("LogEvent", logEvent))
                 {
                     Serilog.Log.Information("页面分页设置:数量[{pageSize}]" , pageSize );
                 }
                 return Json(AR);
             }
             catch (Exception ex)
             {
                 Serilog.Log.Error(ex,"错误:{@error}", ex.Message);  
                 AR.Setfailure(ex.Message);
                 return Json(AR);
             }
         }
         
         // POST: Admin/Articles/Delete/5
         [HttpDelete]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteMulti(int[] ids)
         {

             var c = await _context.Logs.Where(d => ids.Contains(d.Id)).ToListAsync();
             if (c == null)
             {
                 AR.Setfailure(Messages.HttpNotFound);
                 return Json(AR);
             }

             _context.Logs.RemoveRange(c);
             await _context.SaveChangesAsync();
             
             using (LogContext.PushProperty("LogEvent", Buttons.Delete))
             {
                 Serilog.Log.Information("删除日志:ID[{logIds}]", string.Join(",",ids));  
             }

             return Json(AR);
         }
         
        
         [HttpDelete]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteAll()
         {

             var logs = await  _context.Logs.ToListAsync();
         
             if (logs==null || !logs.Any())
             {
                 AR.SetWarning("未发现可删除的日志");
                 return Json(AR);
             }
             
             _context.Logs.RemoveRange(logs);
             await _context.SaveChangesAsync();
             
             using (LogContext.PushProperty("LogEvent", Buttons.Delete))
             {
                 Serilog.Log.Information("删除全部日志"); 
             }

             return Json(AR);
         }
    }
}