using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.Front;
using QNZ.Model.ViewModel;

namespace QNZCMS.Controllers
{
    public class ErrorsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
     
      
        public ErrorsController(QNZContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;     
          
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("errors/{code}")]
        public async Task<IActionResult> InternalServerError(int code)
        {
            var vm = new ErrorVM
            {
                StatusCode = code
            };

            if (code == 404)
            {
                var page = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "404");
                if (page != null)
                {
                    page.ViewCount++;
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                    vm.PageDetail = _mapper.Map<PageDetailVM>(page);
                }
                   
            }
            else
            {
                var page = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "server_error");
                if (page != null)
                {
                    page.ViewCount++;
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                    vm.PageDetail = _mapper.Map<PageDetailVM>(page);
                }
                   
            }

            return View(vm);
        }
      
    }
}