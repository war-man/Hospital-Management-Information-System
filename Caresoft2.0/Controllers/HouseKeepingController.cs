using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class HouseKeepingController : Controller
    {
        // GET: HouseKeeping
        public ActionResult Index()
        {
            ViewBag.ShowTopMenu = true;
            
            ViewBag.IsNurse = false;
            return View();
        }
    }
}