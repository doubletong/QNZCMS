using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QNZ.Model.Front.ViewModel;

namespace QNZCMS.Controllers
{
    public abstract class BaseController : Controller
    {
        public AjaxResultVM AR = new AjaxResultVM();
    

        protected string GetModelErrorMessage()
        {
            string validationErrors = string.Join("|",
                ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
            return validationErrors;
        }
    }
}