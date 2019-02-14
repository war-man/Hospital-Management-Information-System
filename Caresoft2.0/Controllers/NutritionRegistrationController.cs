using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class NutritionRegistrationController : Controller
    {
        public CaresoftHMISEntities db = new CaresoftHMISEntities();

        public ActionResult Index(int id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            ViewBag.Maritals = db.MaritalStatus.ToList();
            ViewBag.Relationships = db.Relationships.ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult AddNutritionAdultRegister(NutritionAdultRegister data)
        {
            db.NutritionAdultRegisters.Add(data);
            db.SaveChanges();


            return View("Index");
        }

        public ActionResult Index2(int id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            ViewBag.Maritals = db.MaritalStatus.ToList();
            ViewBag.Relationships = db.Relationships.ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult AddNutritionChildRegister(NutritionChildRegister info)
        {
            db.NutritionChildRegisters.Add(info);
            db.SaveChanges();


            return View("Index2");
        }

       
    }
}