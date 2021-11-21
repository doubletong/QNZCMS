using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZCMS.Models.Shared
{
    [ViewComponent(Name = "Photos")]
    public class PhotosComponent : ViewComponent
    {
        private readonly QNZContext _context;
        private readonly ICacheService _cacheService;
        public PhotosComponent(QNZContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int albumId)
        {
            string keyNav = $"ALBUM_{albumId}_TRUE";

            if (_cacheService.IsSet(keyNav))
            {
                return View("Default", (List<Navigation>)_cacheService.Get(keyNav));
            }
            else
            {
                var items = await _context.Albums.Include(d => d.Photos)               
                  .FirstOrDefaultAsync(d => d.Id == albumId && d.Active==true);

                _cacheService.Set(keyNav, items,30);

                return View("Default", items);
            }        
                       
        }
    }
}
