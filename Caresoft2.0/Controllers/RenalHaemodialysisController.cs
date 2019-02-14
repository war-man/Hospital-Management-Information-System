using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;

using Caresoft2._0.CustomData;

namespace Caresoft2._0.Controllers
{
    public class RenalHaemodialysisController : Controller


    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: RenalHaemodialysis
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientProfile(int? id=33496)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return PartialView(data);
        }

        public ActionResult SavePatientProfile(RenalPatientProfile data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.RenalPatientProfiles.Add(data);
            db.SaveChanges();

            return RedirectToAction("PatientProfile", new { id = data.OPDNo });
        }

        public ActionResult DialysisOrder(int? id)
        {

            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }
        public ActionResult SaveDialysisOrder(RenalDialysisOrder data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.RenalDialysisOrders.Add(data);
            db.SaveChanges();

            return RedirectToAction("DialysisOrder", new { id = data.OPDNo });
        }


        public ActionResult MachineChecks(int? id)
        {

            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult SaveMachineCheck(RenalMachineCheck data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.RenalMachineChecks.Add(data);
            db.SaveChanges();

            return RedirectToAction("MachineChecks", new { id = data.OPDNo });
        }

        public ActionResult DialysisInfo(int? id)
        {

            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult SaveDialysisInfo(RenalDialysisInfo data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.RenalDialysisInfoes.Add(data);
            db.SaveChanges();

            return RedirectToAction("DialysisInfo", new { id = data.OPDNo });
        }


        public ActionResult PostDialysisObservation(int? id)
        {

            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult SavePostDialysisObservation(RenalPostDialysisObservation data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.RenalPostDialysisObservations.Add(data);
            db.SaveChanges();

            return RedirectToAction("PostDialysisObservation", new { id = data.OPDNo });
        }


    }
}