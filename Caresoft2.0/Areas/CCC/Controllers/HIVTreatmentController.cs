using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class HIVTreatmentController : Controller
    {
        public CaresoftHMISEntities db = new CaresoftHMISEntities();
        public string PatientName { get; private set; }
        // GET: HIV/HIVTreatment
        public ActionResult Index()
        {
            var HivTreatment = db.Patients.ToList();
            return View(HivTreatment.OrderByDescending(e => e.Id).Take(20).ToList());
        }
        [HttpPost]
        //public ActionResult HIVTreatment(HIVTreatment hIVTreatment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        hIVTreatment.Id = 1;
        //        hIVTreatment.OPDIPD = 2;
        //        hIVTreatment.CreatedDate = DateTime.Now;
        //        db.HIVTreatments.Add(hIVTreatment);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");

        //}


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
            
    }
