using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class HIVRegistrationController : Controller
    {
        public CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: HIV/HIVRegistration
        public ActionResult Index(int id)
        {
            var patient=db.Patients.Find(id);
            return View(patient);
        }
    }
}