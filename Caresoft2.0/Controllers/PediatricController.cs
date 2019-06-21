using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    public class PediatricController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: Pediatric
        #region Pedriatic Admission
        public ActionResult Index()
        {
            ViewBag.PedriaticAdmission = db.PedriaticAdmissions.ToList();
            var data = db.PedriaticAdmissions.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SavePediatricAdmissionData(PedriaticAdmission data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;
            
            db.PedriaticAdmissions.Add(data);
            db.SaveChanges();

            return RedirectToAction("Pedriatic");
        }
        #endregion
        #region Pedriatic Admission Record
        public ActionResult Pedriatic()
        {
            ViewBag.PedriaticAdmissionRecord = db.PedriaticAdmissionRecords.ToList();
            var data = db.PedriaticAdmissionRecords.ToList();
            return View(data);

        }
        [HttpPost]
        public ActionResult SavePediatricAdmissionRecordsData(PedriaticAdmissionRecord data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;

            db.PedriaticAdmissionRecords.Add(data);

            db.SaveChanges();

            return RedirectToAction("Pedriatic");
        }
        #endregion
    }
}