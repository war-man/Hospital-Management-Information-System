using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Theatre.Controllers
{
    public class HomeController : Controller
    {
        // GET: Theatre/Home
        public ActionResult Index()
        {
            new Seeder().setUpSettingsParameters();
            return Redirect("/Theatre/Registration");

        }
    }
}