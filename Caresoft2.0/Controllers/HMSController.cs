﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class HMSController : Controller
    {
        // GET: HMS
        public ActionResult Index()
        {
            return View();
        }
    }
}