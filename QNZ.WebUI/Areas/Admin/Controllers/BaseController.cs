using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QNZ.Model.Admin.ViewModel;

namespace SIG.SIGCMS.Areas.Admin.Controllers
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
    }
}