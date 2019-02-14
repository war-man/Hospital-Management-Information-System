using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using PagedList;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class HomeController : Controller
    {
        public CaresoftHMISEntities db = new CaresoftHMISEntities();

        public string PatientName { get; private set; }

        // GET: HIV/Home
        public ActionResult Index(int? page)
        {
          
            return View();
        }


        //[HttpPost]
        //public ActionResult patientregistration(PatientRegistration PatientRegistration)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        PatientRegistration.Id = 1;
        //        PatientRegistration.OPDIPD = 2;
        //        PatientRegistration.CreatedDate = DateTime.Now;
        //        db.PatientRegistrations.Add(PatientRegistration);
        //        db.SaveChanges();

        //    }
        //    return RedirectToAction("Index");

        //}


        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        //public ActionResult PatientRegistration()
        //{
        //    ViewBag.Message = "Patient Registration";
        //    return View();
        //}
    }
}






