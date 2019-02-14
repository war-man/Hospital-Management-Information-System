using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Procurement.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        // GET: Procurement/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}