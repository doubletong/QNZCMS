using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SIG.Model.Admin.InputModel;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class TeamController : BaseController
    {
        
        private IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        public TeamController(YicaiyunContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        
        
        public async Task<IActionResult> Index(int? page)
        {
            var vm = new TeamPageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value
               
            };
            const int pageSize = 12;

            var query = _context.Teams.AsNoTracking().AsQueryable();
          
            vm.TotalCount = await query.CountAsync();
            var teams = await query.OrderByDescending(d=>d.Importance).ThenByDescending(d=>d.Id)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<TeamVM>(_mapper.ConfigurationProvider)
                .ToListAsync();



            vm.Teams = new StaticPagedList<TeamVM>(teams, vm.PageIndex, pageSize, vm.TotalCount);


            return View(vm);
        }
        
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TeamIM team)
        {
            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }
            try
            {
               
                var model = _mapper.Map<Team>(team);
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;
                
                
                _context.Add(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertCreateSuccess, EntityNames.Team));
                return Json(AR);

            }
            catch(Exception er)
            {
                AR.Setfailure(er.Message);
                return Json(AR);
            }
          
        }
        
        // GET: Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TeamIM>(team);
          

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  TeamIM team)
        {
            if (id != team.Id)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            if (!ModelState.IsValid)
            {
                AR.Setfailure(GetModelErrorMessage());
                return Json(AR);
            }


            try
            {
            
                //page.Body = Regex.Replace(page.Body, @"(?i)(?<=<img\b[^>]*?src=(['""]?))(?!http://)[^'""]+(?=\1)", "http://sbe.anyacos.com$0");

                //var ishas = await _context.Pages.CountAsync(d => d.SeoName == page.SeoName.ToLower() && d.Id != id);
                //if (ishas > 0)
                //{
                //    ModelState.AddModelError("SeoName", "已经存在此友好网址");
                //    AR.Setfailure(GetModelErrorMessage());
                //    return Json(AR);
                //}

                var model = await _context.Teams.SingleOrDefaultAsync(d => d.Id == id);

                model = _mapper.Map(team, model);
                model.UpdatedBy = User.Identity.Name;
                model.UpdatedDate = DateTime.Now;
      


                _context.Update(model);
                await _context.SaveChangesAsync();

                AR.SetSuccess(string.Format(Messages.AlertUpdateSuccess, EntityNames.Team));
                return Json(AR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(team.Id))
                {
                    AR.Setfailure(Messages.HttpNotFound);
                    return Json(AR);
                }
                else
                {
                    throw;
                }
            }


        }
        
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(d=>d.Id==id);

            if (team == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            _context.Remove(team);
            await _context.SaveChangesAsync();

            return Json(AR);
        }
        
        private bool PageExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

    }
}