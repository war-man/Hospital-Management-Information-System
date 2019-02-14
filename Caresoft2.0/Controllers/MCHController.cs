using Caresoft2._0.CustomData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CaresoftHMISDataAccess;

using Newtonsoft.Json;

using System.Data;

namespace Caresoft2._0.Controllers
{
    [Auth]

    public class MCHController : Controller
    {

        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        public ActionResult Index()
        {
            return RedirectToAction("OpdList", "emr");
        }
        /*
                public JsonResult GetData()
                {
                    DataSet ds = db.GetViews();
                    List<MCHWeightGainChart> list = new List<MCHWeightGainChart>();
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(new MCHWeightGainChart
                        {
                         Weight=Convert.ToInt32( dr["Weight"]),
                          WeekNo = Convert.ToInt32(dr["WeekNo"])
                        });
                    }
                    return Json(list, JsonRequestBehavior.AllowGet);

                }
                */


        public ActionResult Notification()
        {
            return View();
        }
/*
        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetContacts(notificationRegisterTime);
            var list2 = NC.GetFP(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdated"] = DateTime.Now;

            var list3 = new List<object>() ;
            list3.Add(list);
            list3.Add(list2);

            return new JsonResult { Data = list3,  JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        */
        public ActionResult PostNatalExam(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }
        public ActionResult ClinicalNotes(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }
        public ActionResult SaveClinicalNotes(MCHClinicalNote data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHClinicalNotes.Add(data);
            db.SaveChanges();

            return RedirectToAction("ClinicalNotes", new { id = data.OPDNo });
        }
        public ActionResult AntenatalCareVisit(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult Colposcopy(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult MaternalProfile(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            ViewBag.Maritals = db.MaritalStatus.ToList();
            ViewBag.Relationships = db.Relationships.ToList();

            return View(data);
        }

        public ActionResult SaveMaternalProfile(MCHMaternalProfile data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHMaternalProfiles.Add(data);
            db.SaveChanges();

            return RedirectToAction("MaternalProfile", new { id = data.OPDNo});
        }

        public ActionResult MedicalAndSergicalHistory(int? id )
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }



        public ActionResult SaveMedicalAndSergicalHistory(MCHMedicalAndSergicalHistory data)
        {
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;
            data.UserId = (int)Session["UserId"];
            db.MCHMedicalAndSergicalHistories.Add(data);
            db.SaveChanges();

            

            return RedirectToAction("MedicalAndSergicalHistory", new { id = data.OPDNo });
        }

        public ActionResult PreviousPregnancy(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult WeightGainChart(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();

            //List<DataPoint> dataPoints = new List<DataPoint>();



            //dataPoints = new List<DataPoint>();
            //dataPoints.Add(new DataPoint(1512441720000, 36.2));
            //dataPoints.Add(new DataPoint(1512447240000, 36.5));
            //dataPoints.Add(new DataPoint(1512453960000, 36.5));
            //dataPoints.Add(new DataPoint(1512461700000, 36.4));
            //dataPoints.Add(new DataPoint(1512466320000, 36.7));
            //dataPoints.Add(new DataPoint(1512472020000, 36.8));
            //dataPoints.Add(new DataPoint(1512475440000, 36.7));
            //dataPoints.Add(new DataPoint(1512477540000, 36.9));
            //dataPoints.Add(new DataPoint(1512482280000, 37.1));
            //dataPoints.Add(new DataPoint(1512486900000, 37.2));
            //dataPoints.Add(new DataPoint(1512490800000, 37.2));
            //dataPoints.Add(new DataPoint(1512494160000, 37));

            //ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints);

/*
            DataTable dt = new DataTable();
            var ds = db.MCHWeightGainCharts;
            List<MCHWeightGainChart> list = new List<MCHWeightGainChart>();
            foreach (var dr in ds)
            {

                DataRow row = dt.NewRow();

                row.SetField<double>("Weight", dr.Weight);
                row.SetField<int>("WeekNo", dr.WeekNo);

                dt.Rows.Add(row);
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints);
            }
            */
            return View(data);


        }

        public ActionResult SaveWeightGainChart(MCHWeightGainChart data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHWeightGainCharts.Add(data);
            db.SaveChanges();

            return RedirectToAction("WeightGainChart", new { id = data.OPDNo });
        }


        public ActionResult PhysicalExam(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult AntenatalProfile(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult SaveAntenatalProfile(MCHAntenatalProfile data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHAntenatalProfiles.Add(data);
            db.SaveChanges();

            return RedirectToAction("AntenatalProfile", new { id = data.OPDNo });
        }

        public ActionResult PresentPregnancy(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult PreventativeServices(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }

        public ActionResult SavePreventativeServices(MCHPreventativeService data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHPreventativeServices.Add(data);

            if (db.SaveChanges() > 0)
            {

                var pa = new PatientAppointment();

                pa.User = data.UserId;

                pa.Date_of_Appointment = data.NextVisit;
                pa.Explanation = data.Vaccine;
                pa.OPD_IPD = data.OPDNo;



                db.PatientAppointments.Add(pa);
                db.SaveChanges();
            }

            return RedirectToAction("PreventativeServices", new { id = data.OPDNo });
        }

        public ActionResult Delivery(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }
        public ActionResult BabyPostNatal(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }
        public ActionResult SaveDelivery(MCHDelivery data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHDeliveries.Add(data);
            db.SaveChanges();

            return RedirectToAction("Delivery", new { id = data.OPDNo });
        }
        public ActionResult SaveCancerScreening(MCHCancerScreening data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHCancerScreenings.Add(data);
            db.SaveChanges();

            return RedirectToAction("PostNatalExam", new { id = data.OPDNo });
        }
        public ActionResult SaveMotherPostNatal(MCHMotherPostNatalExam data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHMotherPostNatalExams.Add(data);
            db.SaveChanges();

            return RedirectToAction("PostNatalExam", new { id = data.OPDNo });
        }

        public ActionResult SaveBabyPostNatal(MCHBabyPostNatalEXam data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.DateAdded = DateTime.Now;

            db.MCHBabyPostNatalEXams.Add(data);
            db.SaveChanges();

            return RedirectToAction("PostNatalExam", new { id = data.OPDNo });
        }

            public ActionResult FamilyPlanning(int? id)
        {
            var data = new EMR_OPD_Data();

            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.MasterPostNatalTests = db.MasterPostNatalTests.ToList();
            return View(data);
        }
        public ActionResult SaveFamilyPlanning(MCHFamilyPlaning data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;
            db.MCHFamilyPlanings.Add(data);
            

            if (db.SaveChanges() > 0)
            {

                var pa = new PatientAppointment();

                pa.User = data.UserId;
               
                pa.Date_of_Appointment = data.NextVisit.Value;
                pa.Explanation = data.ContraceptiveAdministered;
                pa.OPD_IPD = data.OPDNo;
               
             

                db.PatientAppointments.Add(pa);
                db.SaveChanges();
            }

            return RedirectToAction("FamilyPlanning", new { id = data.OPDNo });
        }

        public ActionResult SavePhysicalExam(MCHPhysicalExamination data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;
            db.MCHPhysicalExaminations.Add(data);
            db.SaveChanges();

            return RedirectToAction("PhysicalExam", new { id = data.OPDNo });
        }

        public ActionResult SavePreviousPregnancy(MCHPreviousPregnancy data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;
            db.MCHPreviousPregnancies.Add(data);
            db.SaveChanges();

            return Json(new{Status="success"});
        }

        public ActionResult SavePresentPregnancy(MCHPresentPregnancy data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;
            db.MCHPresentPregnancies.Add(data);
            db.SaveChanges();

            return Json(new { Status = "success" });

        }






        public JsonResult GetPieRenderer()
        {
            var DbResult = from d in db.MCHWeightGainCharts
                           select new
                           {
                               d.WeekNo,
                               d.Weight
                           };

            return Json(DbResult, JsonRequestBehavior.AllowGet);
        }



    }
}                 

   