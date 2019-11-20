using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using SIG.Infrastructure.Configs;
using SIG.Infrastructure.Helper;
using SIG.Model.ViewModel;
using SIG.Model.Admin.ViewModel;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GeneralController : BaseController
    {
        //   const string folderName = "Config";
        private IHostingEnvironment _hostingEnvironment;
        public GeneralController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //public ViewResult Site()
        //{

        //    var info = SettingsManager.Site;
        //    var sle = SettingsManager.Article;
        //    var blog = SettingsManager.Blog;
        //    var caseSet = SettingsManager.Case;
        //    var productSet = SettingsManager.Product;
        //    var weixinSet = SettingsManager.WeiXin;
        //    var videoSet = SettingsManager.Video;

        //    ModuleSetIM vm = new ModuleSetIM
        //    {

        //        SiteInfo = new SiteInfoIM
        //        {
        //            SiteName = info.SiteName,
        //            SiteDomainName = info.SiteDomainName,
        //            WebNumber = info.WebNumber,
        //            BaiduSiteID = info.BaiduSiteID,
        //            IsClose = info.IsClose,
        //            CloseInfo = info.CloseInfo,
        //            GoogleAnalyticsID = info.GoogleAnalyticsID,
        //            LoginLogo = info.LoginLogo,
        //            DashboardLogo = info.DashboardLogo,
        //            MailTo = info.MailTo
        //        },
        //        ProductSet = new ProductSetIM
        //        {
        //            EnableCaching = productSet.EnableCaching,
        //            CacheDuration = productSet.CacheDuration,
        //            FrontPageSize = productSet.FrontPageSize,
        //            ThumbHeight = productSet.ThumbHeight,
        //            ThumbWidth = productSet.ThumbWidth,
        //            ImageHeight = productSet.ImageHeight,
        //            ImageWidth = productSet.ImageWidth,
        //            CategoryImageWidth = productSet.CategoryImageWidth,
        //            CategoryImageHeight = productSet.CategoryImageHeight
        //        },
        //        ArticleSet = new ArticleSetIM
        //        {
        //            EnableCaching = sle.EnableCaching,
        //            CacheDuration = sle.CacheDuration,
        //            FrontPageSize = sle.FrontPageSize,
        //            ThumbHeight = sle.ThumbHeight,
        //            ThumbWidth = sle.ThumbWidth,
        //            ImageHeight = sle.ImageHeight,
        //            ImageWidth = sle.ImageWidth

        //        },
        //        PostSet = new PostSetIM
        //        {
        //            EnableCaching = blog.EnableCaching,
        //            CacheDuration = blog.CacheDuration,
        //            FrontPageSize = blog.FrontPageSize,
        //            ThumbHeight = blog.ThumbHeight,
        //            ThumbWidth = blog.ThumbWidth
        //        },
        //        CaseSet = new CaseSetIM
        //        {
        //            EnableCaching = caseSet.EnableCaching,
        //            CacheDuration = caseSet.CacheDuration,
        //            FrontPageSize = caseSet.FrontPageSize,
        //            ThumbHeight = caseSet.ThumbHeight,
        //            ThumbWidth = caseSet.ThumbWidth
        //        },
        //        PhotoSet = new PhotoSetIM
        //        {
        //            EnableCaching = caseSet.EnableCaching,
        //            CacheDuration = caseSet.CacheDuration,
        //            ThumbHeight = caseSet.ThumbHeight,
        //            ThumbWidth = caseSet.ThumbWidth
        //        },
        //        WeiXinSet = new WeiXinSetIM()
        //        {
        //            Token = weixinSet.Token,
        //            AppId = weixinSet.AppId,
        //            AppSecret = weixinSet.AppSecret,
        //            EncodingAESKey = weixinSet.EncodingAESKey
        //        },
        //        VideoSet = new VideoSetIM()
        //        {
        //            FrontPageSize = videoSet.FrontPageSize,
        //            ThumbHeight = videoSet.ThumbHeight,
        //            ThumbWidth = videoSet.ThumbWidth,
        //            Timer = videoSet.Timer
        //        }
        //    };


        //    return View(vm);
        //}

        //[HttpPost]
        //public JsonResult VideoSet(VideoSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);

        //        return Json(AR);
        //    }

        //    try
        //    {
        //        string folderName = "/Config/VideoSettings.config";
        //        string webRootPath = _hostingEnvironment.WebRootPath;
        //        var xmlFile = Path.Combine(webRootPath, folderName);
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("FrontPageSize").SetValue(im.FrontPageSize);
        //        item.Element("ThumbHeight").SetValue(im.ThumbHeight);
        //        item.Element("ThumbWidth").SetValue(im.ThumbWidth);
        //        item.Element("Timer").SetValue(im.Timer);
        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}

        //[HttpPost]
        //public JsonResult Article(ArticleSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);

        //        return Json(AR);
        //    }

        //    try
        //    {
        //        var xmlFile = Server.MapPath("~/Config/ArticleSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("FrontPageSize").SetValue(im.FrontPageSize);
        //        item.Element("ThumbHeight").SetValue(im.ThumbHeight);
        //        item.Element("ThumbWidth").SetValue(im.ThumbWidth);
        //        item.Element("ImageHeight").SetValue(im.ImageHeight);
        //        item.Element("ImageWidth").SetValue(im.ImageWidth);
        //        item.Element("EnableCaching").SetValue(im.EnableCaching);
        //        item.Element("CacheDuration").SetValue(im.CacheDuration);

        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}

        //[HttpPost]
        //public JsonResult PhotoSet(PhotoSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);
        //        return Json(AR);
        //    }

        //    try
        //    {
        //        var xmlFile = Server.MapPath("~/Config/PhotoSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("EnableCaching").SetValue(im.EnableCaching);
        //        item.Element("CacheDuration").SetValue(im.CacheDuration);
        //        item.Element("ThumbHeight").SetValue(im.ThumbHeight);
        //        item.Element("ThumbWidth").SetValue(im.ThumbWidth);


        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}
        //[HttpPost]
        //public JsonResult Blog(PostSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);
        //        return Json(AR);
        //    }

        //    try
        //    {
        //        var xmlFile = Server.MapPath("~/Config/BlogSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("FrontPageSize").SetValue(im.FrontPageSize);
        //        item.Element("EnableCaching").SetValue(im.EnableCaching);
        //        item.Element("CacheDuration").SetValue(im.CacheDuration);
        //        item.Element("ThumbHeight").SetValue(im.ThumbHeight);
        //        item.Element("ThumbWidth").SetValue(im.ThumbWidth);


        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}

        //[HttpPost]
        //public JsonResult ProductSet(ProductSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);
        //        return Json(AR);
        //    }

        //    try
        //    {
        //        var xmlFile = Server.MapPath("~/Config/ProductSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("FrontPageSize").SetValue(im.FrontPageSize);
        //        item.Element("EnableCaching").SetValue(im.EnableCaching);
        //        item.Element("CacheDuration").SetValue(im.CacheDuration);
        //        item.Element("ThumbHeight").SetValue(im.ThumbHeight);
        //        item.Element("ThumbWidth").SetValue(im.ThumbWidth);
        //        item.Element("ImageHeight").SetValue(im.ImageHeight);
        //        item.Element("ImageWidth").SetValue(im.ImageWidth);
        //        item.Element("CategoryImageWidth").SetValue(im.CategoryImageWidth);
        //        item.Element("CategoryImageHeight").SetValue(im.CategoryImageHeight);

        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}

        //[HttpPost]
        //public JsonResult WeiXinSet(WeiXinSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);
        //        return Json(AR);
        //    }

        //    try
        //    {
        //        var xmlFile = Server.MapPath("~/Config/WeiXinSettings.config");
        //        var doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.Element("Token")?.SetValue(im.Token);
        //            item.Element("AppId")?.SetValue(im.AppId);
        //            item.Element("AppSecret")?.SetValue(im.AppSecret);
        //            item.Element("EncodingAESKey")?.SetValue(im.EncodingAESKey);

        //        }

        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}

        //[HttpPost]
        //public JsonResult CaseSet(CaseSetIM im)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        var errorMessage = GetModelErrorMessage();
        //        AR.Setfailure(errorMessage);
        //        return Json(AR);
        //    }

        //    try
        //    {
        //        var xmlFile = Server.MapPath("~/Config/CaseSettings.config");
        //        XDocument doc = XDocument.Load(xmlFile);

        //        var item = doc.Descendants("Settings").FirstOrDefault();
        //        item.Element("FrontPageSize").SetValue(im.FrontPageSize);
        //        item.Element("EnableCaching").SetValue(im.EnableCaching);
        //        item.Element("CacheDuration").SetValue(im.CacheDuration);
        //        item.Element("ThumbHeight").SetValue(im.ThumbHeight);
        //        item.Element("ThumbWidth").SetValue(im.ThumbWidth);


        //        doc.Save(xmlFile);

        //        return Json(AR);
        //    }
        //    catch (Exception ex)
        //    {
        //        AR.Setfailure(ex.Message);
        //        return Json(AR);
        //    }
        //}
        public IActionResult Index()
        {
            var info = SettingsManager.Site;

            var vm = new SiteInfoIM
            {
                SiteName = info.SiteName,
                SiteDomainName = info.SiteDomainName,
                WebNumber = info.WebNumber,
                BaiduSiteID = info.BaiduSiteID,
                IsClose = info.IsClose,
                CloseInfo = info.CloseInfo,
                GoogleAnalyticsID = info.GoogleAnalyticsID,
                LoginLogo = info.LoginLogo,
                DashboardLogo = info.DashboardLogo,
                MailTo = info.MailTo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSite(SiteInfoIM vm)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = GetModelErrorMessage();
                AR.Setfailure(errorMessage);
                return Json(AR);
            }

            try
            {
                var xmlFile = PlatformServices.Default.MapPath("/Config/GlobalSettings.config");       
                XDocument doc = XDocument.Load(xmlFile);

                var item = doc.Descendants("Settings").FirstOrDefault();
                item.Element("SiteName").SetValue(vm.SiteName ?? "");
                item.Element("SiteDomainName").SetValue(vm.SiteDomainName ?? "");
                item.Element("WebNumber").SetValue(vm.WebNumber ?? "");
                item.Element("BaiduSiteID").SetValue(vm.BaiduSiteID ?? "");
                item.Element("GoogleAnalyticsID").SetValue(vm.GoogleAnalyticsID ?? "");
                item.Element("IsClose").SetValue(vm.IsClose);
                item.Element("CloseInfo").SetValue(vm.CloseInfo ?? "");
                item.Element("DashboardLogo").SetValue(vm.DashboardLogo ?? "");
                item.Element("LoginLogo").SetValue(vm.LoginLogo ?? "");
                item.Element("MailTo").SetValue(vm.MailTo ?? "");
                doc.Save(xmlFile);

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