using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Theatre.Controllers
{
    public class OperationController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Theatre/Operation
        public ActionResult TheatreProcedures()
        {
            return View();
        }

        //Returns Patient Information
        public ActionResult PatientInformation(int id)
        {
            var pData = db.TheatrePatientBioDatas.Find(id);
            ViewBag.TheatreDesignations = db.TheatreDesignations.ToList();
        
            return View(pData);
        }

        //Returns Theatre Operation Personel
        public ActionResult OperationPersonel(int id)
        {
            var pData = db.TheatrePatientBioDatas.Find(id);
            ViewBag.TheatreDesignations = db.TheatreDesignations.ToList();

            return View(pData);
        }

        //Returns List of Proceedures Corresponding to a Search sting 
        public ActionResult SearchProcedures(string search)
        {
            var data = db.Services.Where(e => e.ServiceName.Contains(search));

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Returns Theatre Proceedures View
        public ActionResult Procedures()
        {
            ViewBag.ServicesList = db.Services.ToList();
            return View();
        }


        public class TempProcedure
        {
            public int DepartmentId { get; set; }
            public int ServiceId { get; set; }
            public int Quantity { get; set; }
        }

        public class TempProceedures
        {
            public int OPDNo { get; set; }
            public List<TempProcedure> tempProcedure { get; set; }
        }

        [HttpPost]
        public ActionResult Procedures(TempProceedures procedures)
        {
            int userId = (int)Session["UserId"];
            var successfulBilled = new List<int>();
            if (1 == 1)
            {
                foreach (var pro in procedures.tempProcedure)
                {
                    pro.DepartmentId = db.Services.Find(pro.ServiceId).DepartmentId;

                    var billServiceData = new BillServiceData() {

                        OPDNo = procedures.OPDNo,
                        DepartmentId = pro.DepartmentId,
                        ServiceId = pro.ServiceId,
                        Quantity = pro.Quantity,

                    };

                    var res  = new Caresoft2._0.Controllers.RegistrationController().AddItemToBill(billServiceData, userId);
                    if (res > 1)
                    {
                        successfulBilled.Add(billServiceData.ServiceId);
                    }

                }

                return Json(successfulBilled, JsonRequestBehavior.AllowGet);

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}