using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Infrastructure.Cache;

namespace QNZCMS.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DocumentsController(YicaiyunContext context, IMapper mapper, ICacheService cacheService, IWebHostEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _context = context;
            _cacheService = cacheService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> DownloadAsync(int id)
        {
            var file = await _context.Documents.FindAsync(id);
            if (file == null)
                return NotFound();

            var path = _hostingEnvironment.WebRootPath+file.FileUrl;
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            file.DownloadCount++;

            _context.Update(file);
            await _context.SaveChangesAsync();

            var fs = new FileStream(path, FileMode.Open); 
            return File(fs, "application/octet-stream", Path.GetFileName(path));
        }
    }
}