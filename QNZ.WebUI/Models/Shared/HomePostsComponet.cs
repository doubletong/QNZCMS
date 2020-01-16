using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "HomePosts")]
    public class HomePostsComponet : ViewComponent
    {
        private readonly YicaiyunContext _context;

        public HomePostsComponet(YicaiyunContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Posts
                .Where(d => d.Active==true)
                .OrderByDescending(d => d.Id).Take(15).ToListAsync();
            return View("Default", items);
        }
    }
}
