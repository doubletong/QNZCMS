using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;

namespace QNZCMS.Controllers
{
    public class OpenSourceController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public OpenSourceController(YicaiyunContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "opensource");
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        public async Task<IActionResult> QNZAdmin()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "qnzadmin");
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }
    }
}