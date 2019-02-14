using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.MISReports.Controllers
{
    public class HomeController : Controller
    {
        CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: MISReports/Home
        public ActionResult Index()
        {
            ViewBag.departmets = db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower().Equals("revenue")).ToList();
            ViewBag.Users = db.Users.ToList();

            return View();
        }
    }
}