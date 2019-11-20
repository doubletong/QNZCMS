using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIG.Infrastructure.Configs;
using SIG.Model.Admin.ViewModel;
//using SIG.Model.Admin.ViewModel.Log;
//using SIG.Services.Log;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
//    [Area("Admin")]
//    [Authorize(Policy = "Permission")]
//    public class LogController : BaseController
//    {
//        private readonly ILogServices _logServices;
//        private readonly IMapper _mapper;

//        public LogController(ILogServices logServices, IMapper mapper)
//        {
//            _logServices = logServices;
//            _mapper = mapper;

//        }



//        public IActionResult Index(int? page, DateTime? startDate, DateTime? expireDate, string level)
//        {
//            var logSearchVM = new LogSearchVM()
//            {
//                StartDate = startDate,
//                ExpireDate = expireDate,
//                Level = level,

//                PageIndex = (page ?? 1),
//                PageSize = SettingsManager.Log.PageSize
//            };
//            int totalCount;
//            logSearchVM.Logs = _logServices.SearchLogs(logSearchVM.PageIndex - 1, logSearchVM.PageSize, startDate,
//                expireDate, level, out totalCount);
//            logSearchVM.TotalUserCount = totalCount;

//            //logSearchVM.Logs = _mapper.Map<IEnumerable<LogVM>>(logs);

//            //var logsAsIPagedList =  new StaticPagedList<LogVM>(logSearchVM.Logs, logSearchVM.PageIndex, logSearchVM.PageSize, logSearchVM.TotalUserCount);
//            //ViewBag.OnePageOfLogs = logsAsIPagedList;

//            return View(logSearchVM);

//        }



//        [HttpPost]
//        public JsonResult Delete(string id)
//        {
//            try
//            {
//                if (id == "all")
//                {
//                    _logServices.RemoveAll();
                    
//                    AR.SetSuccess("已清空所有日志");
//                    return Json(AR);

//                }
//                else
//                {

//                    int logId;
//                    Int32.TryParse(id, out logId);
//                    _logServices.Delete(logId);

//                    AR.SetSuccess("已成功删除日志");
//                    return Json(AR);

//                }
//        }
//            catch (Exception ex)
//            {
//                AR.Setfailure(ex.Message);
//                return Json(AR);
//    }

//}
    //}
}