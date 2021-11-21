using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Model.ViewModel;

namespace QNZCMS.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QNZContext _context;
        public WorksController(QNZContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/Works
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkFVM>>> GetWorks(int page = 1)
        {

            return await _context.Works.Where(d=>d.Active)
                .OrderByDescending(d => d.FinishYear).ThenByDescending(d=>d.Id).Skip(page * 10)
                .Take(10).ProjectTo<WorkFVM>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // GET: api/Works/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Work>> GetWork(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work == null)
            {
                return NotFound();
            }
            work.ViewCount++;
            _context.Update(work);
            await _context.SaveChangesAsync();

            return work;
        }

        
    }
}
