using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZCMS.Services
{
    public class LocationExpanderService : IViewLocationExpander
    {

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //replace the Views to MyViews..  

            var viewLocationsFinal = new List<string>();
            if (!string.IsNullOrEmpty(context.Values["viewCustom"]))
            {
                foreach (var viewLocation in viewLocations)
                {
                    viewLocationsFinal.Add(viewLocation.Replace(".cshtml", ".mobile.cshtml"));
                }
            }
            viewLocationsFinal.AddRange(viewLocations);
            return viewLocationsFinal;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

            var userAgent = context.ActionContext.HttpContext.Request.Headers["User-Agent"].ToString().ToLower();
            var viewCustom = userAgent.Contains("android") || userAgent.Contains("iphone") ? "mobile" : "";
            context.Values["viewCustom"] = viewCustom;
        }
        // Configure services in startup.cs
        //services.Configure<RazorViewEngineOptions>(o =>
        //{
        //    o.ViewLocationExpanders.Add(new LocationExpanderService());
        //});
    }
}
