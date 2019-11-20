using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PagedList.Core;
using SIG.Infrastructure.Helper;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class CustomersController : BaseController
    {

        private IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public CustomersController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index(string keyword,  int? page)
        {
            CustomerPageVM vm = new CustomerPageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword

            };
            var pageSize = 10;

            var query = _context.Customers.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Mobile.Contains(keyword));

          

            vm.TotalCount = await query.CountAsync();
            var list = await query.Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<CustomerVM>(_mapper.ConfigurationProvider).ToListAsync();
         //   var list = _mapper.Map<IEnumerable<CustomerVM>>(agents);

            vm.Customers = new StaticPagedList<CustomerVM>(list, vm.PageIndex, pageSize, vm.TotalCount);

          

            return View(vm);
        }


        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .SingleOrDefaultAsync(m => m.OpenId == id);
            if (customer == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<CustomerVM>(customer);

            return View(model);
        }
        //public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        //{
        //    var query = _context.Customers.AsQueryable();
        //    if (startDate != null)
        //    {
        //        query = query.Where(d => d.CreatedDate >= startDate);
        //    }
        //    if (endDate != null)
        //    {
        //        query = query.Where(d => d.CreatedDate < endDate);
        //    }


        //    StatisticsVM vm = new StatisticsVM
        //    {
        //        StartDate = startDate,
        //        EndDate = endDate,

        //        CustomerGZHCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.GongZongHao),
        //        CustomerGZHTodayCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.GongZongHao && d.CreatedDate >= DateTime.Today && d.CreatedDate < DateTime.Today.AddDays(1)),
               

        //        CustomerXCXCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.XiaoChengXu),
        //        CustomerXCXTodayCount = await _context.Customers.CountAsync(d => d.AppType == Data.Enums.AppType.XiaoChengXu && d.CreatedDate >= DateTime.Today && d.CreatedDate < DateTime.Today.AddDays(1)),
                
        //      };

        //    if(startDate!=null && endDate != null && endDate >startDate)
        //    {
        //        int day = Math.Abs(((TimeSpan)(startDate - endDate)).Days);
        //        if (day > 31)
        //        {
        //            TempData["Error"] = "时间间隔不能超过31天。";
        //        }
        //        else{
        //            vm.CustomerGZHCustomCount = await query.CountAsync(d => d.AppType == Data.Enums.AppType.GongZongHao);
        //            vm.CustomerXCXCustomCount = await query.CountAsync(d => d.AppType == Data.Enums.AppType.XiaoChengXu);
        //        }
        //    }
        //    if(startDate>=endDate)
        //    {
        //        TempData["Error"] = "结束日期不能小于开始日期。";
        //    }
        //    return View(vm);
        //}

      

        // GET: Admin/Customers/Create
        //public IActionResult Import()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Import([FromQuery] string handler)
        //{
        //    IFormFile file = Request.Form.Files[0];
        //    string folderName = "uploads";
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string newPath = Path.Combine(webRootPath, folderName);
        //    StringBuilder sb = new StringBuilder();
        //    if (!Directory.Exists(newPath))
        //    {
        //        Directory.CreateDirectory(newPath);
        //    }
        //    if (file.Length > 0)
        //    {
        //        string sFileExtension = Path.GetExtension(file.FileName).ToLower();
        //        ISheet sheet;
        //        string fullPath = Path.Combine(newPath, file.FileName);
        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //            stream.Position = 0;
        //            if (sFileExtension == ".xls")
        //            {
        //                HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
        //                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
        //            }
        //            else
        //            {
        //                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
        //                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
        //            }
        //            IRow headerRow = sheet.GetRow(0); //Get Header Row
        //            int cellCount = headerRow.LastCellNum;
        //            //sb.Append("<table class='table'><tr>");
        //            //for (int j = 0; j < cellCount; j++)
        //            //{
        //            //    NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
        //            //    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
        //            //    sb.Append("<th>" + cell.ToString() + "</th>");
        //            //}
        //            //sb.Append("</tr>");
        //            //sb.AppendLine("<tr>");
        //            List<string> noImports = new List<string>();
        //            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
        //            {
        //                IRow row = sheet.GetRow(i);
        //                if (row == null) continue;
        //                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

        //                var mobile = row.GetCell(0).ToString();
        //                var customer = await _context.Customers.FirstOrDefaultAsync(d => d.Mobile == mobile);

        //                if (customer == null)
        //                {
        //                    int age;
        //                    var pwd = row.GetCell(1).ToString();

        //                    var securityStamp = Hash.GenerateSalt();
        //                    var pwdHash = Hash.HashPasswordWithSalt(pwd, securityStamp);

                           
        //                    var c = new Customer
        //                    {
        //                        Mobile = row.GetCell(0).ToString(),
        //                        SecurityStamp = Convert.ToBase64String(securityStamp),
        //                        PasswordHash = pwdHash,
        //                        Realname = row.GetCell(2).ToString(),
        //                        AppType = Data.Enums.AppType.Desktop,
        //                        CreatedDate = DateTime.Now,
        //                        Active = true
                              

        //                    };
        //                    if(int.TryParse(row.GetCell(3).ToString(),out age) == true)
        //                    {
        //                        c.Age = age;
        //                    }

        //                    _context.Add(c);
        //                }
        //                else
        //                {
        //                    noImports.Add(mobile);
        //                }

        //            }

        //            var result = await _context.SaveChangesAsync();
        //            if (noImports.Count > 0)
        //            {
        //                AR.Setfailure("以下手机号" + string.Join("、", noImports) + "已存在，未能导入");
        //                return Json(AR);
        //            }
        //            if (result > 0)
        //            {
        //                AR.SetSuccess("已成功导入" + result + "条记录");
        //                return Json(AR);
        //            }

        //        }
        //    }
        //    else
        //    {
        //        AR.Setfailure("请选择导入文件");
        //        return Json(AR);
        //    }
        //    AR.Setfailure("导入失败");
        //    return Json(AR);
        //}

    }
}
