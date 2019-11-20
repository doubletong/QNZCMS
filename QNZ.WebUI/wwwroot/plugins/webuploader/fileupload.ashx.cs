using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BBICMS.WebUI.Plugins.webuploader
{
    /// <summary>
    /// fileupload 的摘要说明
    /// </summary>
    public class fileupload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request["REQUEST_METHOD"] == "OPTIONS")
            {
                context.Response.End();
            }
           

           
            SaveFile();
        }
        private void SaveFile()
        {
            string basePath = string.IsNullOrEmpty(HttpContext.Current.Request["path"]) ? "~/Uploads" : HttpContext.Current.Request["path"];
            string name;
            basePath = HttpContext.Current.Server.MapPath(basePath);
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var suffix = files[0].ContentType.Split('/');
            //获取文件格式  
            //var _suffix = suffix[1].Equals("jpeg", StringComparison.CurrentCultureIgnoreCase) ? "" : suffix[1];  
            var _suffix = suffix[1];
            var _temp = HttpContext.Current.Request["name"];
            //如果不修改文件名，则创建随机文件名  
            if (!string.IsNullOrEmpty(_temp))
            {
                name = _temp;
            }
            else
            {
                Random rand = new Random(24 * (int)DateTime.Now.Ticks);
                name = rand.Next() + "." + _suffix;
            }
            //文件保存  
            var full = basePath + "\\" + name;
            files[0].SaveAs(full);
            var _result = "{\"jsonrpc\" : \"2.0\", \"result\" : null, \"id\" : \"" + name + "\"}";
            HttpContext.Current.Response.Write(_result);


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
