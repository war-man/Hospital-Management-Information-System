using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Nutrition.Controllers
{
    [Auth]
    public class NutritionHomeController : Controller
    {
        // GET: Nutrition/Home
        public CaresoftHMISEntities db = new CaresoftHMISEntities();

        public ActionResult NutritionScreening()
        {

            return View();
        }

        public ActionResult Index(int OpdNo)
        {
            ViewBag.OpdNo = OpdNo;

            ViewBag.MealTime = db.MealTypes.ToList();
            ViewBag.ItemName = db.ItemNames.ToList();

            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(OpdNo);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            ViewBag.Maritals = db.MaritalStatus.ToList();
            ViewBag.Relationships = db.Relationships.ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult AddNutritiondiet(NutritionDietchart diet)
        {
            ViewBag.OpdNo = diet.OPDIPDID;

            int uid = Convert.ToInt32(Session["UserId"]);
            diet.user_id = uid;
          // diet.date_time = DateTime.Today;

            if (ModelState.IsValid)
            {
                db.NutritionDietcharts.Add(diet);
                db.SaveChanges();

                ViewBag.success = true;
            }

            ViewBag.success = false;
            return RedirectToAction("Index", new { OpdNo = diet.OPDIPDID });
        }

        public ActionResult Index2(int OpdNo)
        {
            ViewBag.OpdNo = OpdNo;
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.MealTime = db.MealTypes.ToList();
            ViewBag.ItemName = db.ItemNames.ToList();

            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(OpdNo);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            ViewBag.Maritals = db.MaritalStatus.ToList();
            ViewBag.Relationships = db.Relationships.ToList();

            return View(data);

        }

        [HttpPost]
        public ActionResult AddNutritionScreeningDetails(NutritionScreeningDetail Nutrition)
        {
            ViewBag.OpdNo = Nutrition.OPDIPDID;

            int uid = Convert.ToInt32(Session["UserId"]);
            Nutrition.UserId = uid;

            if (ModelState.IsValid)
            {
                db.NutritionScreeningDetails.Add(Nutrition);
                db.SaveChanges();

                ViewBag.success = true;
            }

            ViewBag.success = false;

            return RedirectToAction("Index2" , new { OpdNo = Nutrition.OPDIPDID });
        }

        public ActionResult Doctors()
        {
            var docs = db.Employees.Select(e => e.FName).ToList();
            return Json(docs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Patient_Appointment()
        {
            return View();
        }
        public ActionResult DoctorLeave()
        {
            return View();
        }
        public ActionResult ViewAppointment()
        {
            return View();
        }
        public ActionResult AppointmentReport()
        {
            return View();
        }
    }
}