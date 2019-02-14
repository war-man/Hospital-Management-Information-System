using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class PatientSearchController : Controller
    {
        public CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: HIV/PatientSearch
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchPatient(string search)
        {
            var data = db.Patients.Where(p => p.RegNumber.Contains(search));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}