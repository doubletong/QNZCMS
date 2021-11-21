using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Model.Admin.ViewModel;
using QNZ.Model.Administrator;
using QNZ.Model.ViewModel;

namespace QNZCMS.Areas.Admin.Controllers
{
    public abstract class BaseController : Controller
    {
        public AjaxResultVM AR = new AjaxResultVM();
        protected string GetModelErrorMessage()
        {
            var validationErrors = string.Join("|",
                ModelState.Values.Where(d => d.Errors.Count > 0)
                    .SelectMany(d => d.Errors)
                    .Select(d => d.ErrorMessage)
                    .ToArray());
            return validationErrors;
        }

        protected async Task CreatedUpdatedPageMetaAsync(QNZContext db, PageMeta pm)
        {
            var origin = await db.PageMetas.FirstOrDefaultAsync(d => d.ModuleType == pm.ModuleType && d.ObjectId == pm.ObjectId);
            if (origin != null)
            {
                if(string.IsNullOrEmpty(pm.Title) && string.IsNullOrEmpty(pm.Keywords) && string.IsNullOrEmpty(pm.Description))
                {
                    db.Remove(origin);
                }
                else
                {
                    origin.Title = pm.Title;
                    origin.Keywords = pm.Keywords;
                    origin.Description = pm.Description;

                    db.Entry(origin).State = EntityState.Modified;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(pm.Title) || !string.IsNullOrEmpty(pm.Keywords) || !string.IsNullOrEmpty(pm.Description))
                {
                    db.Add(pm);
                }              
            }

            await db.SaveChangesAsync();
        }
    }
}