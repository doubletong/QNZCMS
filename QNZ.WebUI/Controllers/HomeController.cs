using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QNZCMS.Models;
using QNZ.Infrastructure.Cache;
using QNZ.Data;
using Microsoft.EntityFrameworkCore;
using QNZ.Model.Front.ViewModel;
using QNZ.Model.ViewModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.FileProviders;
using System.IO;
using QNZ.Infrastructure.Helper;
using QNZ.Services;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace QNZCMS.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IViewRenderService _viewRenderService;

        private IWebHostEnvironment _hostingEnvironment;
        private readonly ICacheService _cacheService;
        private readonly YicaiyunContext _context;
        private readonly IMapper _mapper;
        private readonly IFileProvider _fileProvider;
        public HomeController(/*IViewRenderService viewRenderService,*/ IWebHostEnvironment hostingEnvironment, ICacheService cache, YicaiyunContext context, IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _cacheService = cache;
            _context = context;
            _mapper = mapper;
            _fileProvider = hostingEnvironment.WebRootFileProvider;
           // _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //var configFile = _hostingEnvironment.WebRootPath + "\\Config\\Global.json";
            //string json = System.IO.File.ReadAllText(configFile);
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            //jsonObj["Bots"][0]["Password"] = "new password123";
            //string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            //System.IO.File.WriteAllText(configFile, output);

            string keyNav = $"ADVERTISEMENTS_TRUE_ALL";

            if (!_cacheService.IsSet(keyNav))
            {
                var adverts = await _context.Advertisements.Include(d => d.Space).Where(d => d.Active)
                    .OrderByDescending(d => d.Importance).ToListAsync();
                _cacheService.Set(keyNav, adverts, 30);
            }

            var advertList = (List<Advertisement>)_cacheService.Get(keyNav);

            var ads = _mapper.Map<List<AdvertisementVM>>(advertList.Where(d => d.Space.Code == "A1001"));

            var video = await _context.Videos.FirstOrDefaultAsync(d => d.Recommend == true);

            var vm = new HomePageVM
            {
                Solutions = await _context.Solutions.Where(d => d.Active == true)
                 .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id)
                 .ProjectTo<SolutionVM>(_mapper.ConfigurationProvider).ToListAsync(),
                Adverts = ads,

                Shopes = await _context.Shopes.Where(d => d.Active == true).ProjectTo<ShopeVM>(_mapper.ConfigurationProvider).ToListAsync(),
                Video = _mapper.Map<VideoVM>(video)
            };


            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ChacheShow()
        {
            _cacheService.Set("test", "this is test", 15);

            return Content(_cacheService.IsSet("MENUS_CATEGORY_1").ToString());
        }
        public IActionResult ChacheShow1()
        {
            _cacheService.Invalidate("MENU");

            return Content(_cacheService.IsSet("MENUS_CATEGORY_1").ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Upload()
        {
            return View();
        }

        //public IActionResult Test()
        //{

        //    return new HtmlContentViewComponentResult("AllClients");
        
        //}

        public async Task<IActionResult> Test2Async()
        {
            string a = "[about ab=123 cd=323]";
            //  var a =  await  _viewRenderService.RenderAsync("Test");
            a = a.Remove(0, 1);
            a = a.Remove(a.Length-1, 1);

            var b = a.Split(' ');
            var c = b[1].Substring(b[1].IndexOf('='));
            c.Contains('=');
            return Content(b[0]);

        }



        [Route("/image/{w}/{h}/{*url}")]
        public IActionResult ResizeImage(string url, int w, int h)
        {

            if (w < 0 || h < 0) { return BadRequest(); }


            var fileName = url.Replace('/', '_');
            var thumbUrl = $"ThumbCache/w{w}_h{h}_{fileName}";
            var thumbPath = PathString.FromUriComponent("/" + thumbUrl);

            var thumbInfo = _fileProvider.GetFileInfo(thumbPath);

            if (thumbInfo.Exists)
            {
                // return File(thumbInfo.CreateReadStream(), "image/jpg");
                return File(thumbPath, "image/jpg");
                //return Content(thumbPath);
            }



            var imagePath = "/" + url;
            imagePath = imagePath.Replace('/', '\\');
            var orginPath = $"{_hostingEnvironment.WebRootPath}{imagePath}";

            thumbUrl = ("/" + thumbUrl).Replace('/', '\\');
            var outPath = $"{_hostingEnvironment.WebRootPath}{thumbUrl}";

            System.Drawing.Image originImage = System.Drawing.Image.FromFile(orginPath);
            var ext = Path.GetExtension(orginPath);


            if (originImage.Height == h && originImage.Width == w)
            {
                imagePath = PathString.FromUriComponent("/" + url);
                //var orginInfo = _fileProvider.GetFileInfo(imagePath);

                //return File(orginInfo.CreateReadStream(), "image/jpg");
                return File(imagePath, "image/jpg");
                //return Content(imagePath);

            }
            else
            {
                ImageHandler.MakeThumbnail(orginPath, outPath, w, h, "Cut", ext.ToLower());

                // return Content(thumbPath);
                return File(thumbPath, "image/jpg");
            }


            //thumbInfo = _fileProvider.GetFileInfo(thumbPath);
            // return File(thumbInfo.CreateReadStream(), "image/jpg");


        }



        //[HttpPost]
        //public async Task<IActionResult> UploadSmallFile(IFormFile file)
        //{
        //    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
        //    fileName = fileName.Contains("\\")
        //       ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
        //       : fileName.Trim('"');
        //    // full path to file in temp location
        //    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, SettingsManager.File.RootDirectory, fileName);
        //    //   var filePath = Path.GetTempFileName();

        //    if (file.Length > 0)
        //    {
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }

        //    // process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[DisableFormValueModelBinding]
        //public async Task<IActionResult> UploadStreamingFile()
        //{
        //    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
        //    //fileName = fileName.Contains("\\")
        //    //   ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
        //    //   : fileName.Trim('"');

        //    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, SettingsManager.File.RootDirectory,"001.png");
        //    // full path to file in temp location
        // //  var filePath = Path.GetTempFileName();

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await Request.StreamFile(stream);
        //    }

        //    // process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok();
        //}
    }
}
