using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "HomeArticles")]
    public class HomeArticlesComponet : ViewComponent
    {

        private readonly YicaiyunContext _context;

        public HomeArticlesComponet(YicaiyunContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Articles
                .Where(d => d.Active==true)
                .OrderByDescending(d => d.Id).Take(15).ToListAsync();
            return View("Default", items);
        }
    }
}
