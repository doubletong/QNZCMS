﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QNZCMS.Controllers
{
    public class CooperationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}