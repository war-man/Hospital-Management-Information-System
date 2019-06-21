using CaresoftHMISDataAccess;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Appointments.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        //private CaresoftHMISEntities db2 = new CaresoftHMISEntities();
        //private CaresoftHMISEntities db3 = new CaresoftHMISEntities();
        public ActionResult Index()
        {
            ViewBag.Doctors = db.Users.ToList();
            ViewBag.Speciality = db.Departments.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Appointment(DoctorsSchedule doctorsSchedule)
        {
            doctorsSchedule.CreatedDate = DateTime.UtcNow.ToLocalTime();
            if (ModelState.IsValid)
            {//ToDo : select user from session
                doctorsSchedule.UserId = (int)Session["UserId"];

                db.DoctorsSchedules.Add(doctorsSchedule);
                db.SaveChanges();

                ViewBag.SucessFullySaved = true;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        //public ActionResult Doctors()
        //{
        //    var docs = db.Employees.Select(e=>e.FName).ToList();                       
        //    return Json(docs, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Patient_Appointment()
        {
            ViewBag.PatientAppointment = db.Patients.ToList();
            ViewBag.Speciality = db.Departments.ToList();
            ViewBag.Doctors = db.Users.Where(e => e.DoctorsSchedules.Any()).ToList();
            //var docshedule = db.DoctorsSchedules.ToList();
            //List<User> lstDoctors = new List<User>();
            //foreach (var doc in docshedule)
            //{
            //    var thedoctor = db.Users.Where(p => p.EmployeeId == doc.Doctor).FirstOrDefault();
            //    if (thedoctor != null)
            //    {
            //        lstDoctors.Add(thedoctor);
            //    }
            //}
            //ViewBag.LstDoctors = lstDoctors;
            return View();
        }

        [HttpPost]
        public ActionResult PatientAppointment(PatientAppointment patientAppointment)
        {
            if (ModelState.IsValid)
            {
                patientAppointment.User = (int)Session["UserId"];
                patientAppointment.OPD_IPD = db.Patients.FirstOrDefault(e => e.RegNumber.Equals(patientAppointment.RegNumber)).Id;
                db.PatientAppointments.Add(patientAppointment);


                db.SaveChanges();

                ViewBag.SucessFullySaved = true;
                return RedirectToAction("Patient_Appointment");
            }
            return RedirectToAction("Patient_Appointment");
        }

        public ActionResult SearchPatient(String searchString)
        {
            ViewBag.PatientAppointment = db.Patients.ToList();
            var data = db.Patients.Where(p => p.RegNumber.Contains(searchString)).Select(e => e.RegNumber).Take(20).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getpatientdetails(string RegNo)
        {
            var patient = db.Patients.Where(c => c.RegNumber == RegNo).FirstOrDefault();
            return Json(patient, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoctorLeave()
        {
            return View();
        }
        public ActionResult ViewAppointment(int? page)
        {
            page = page ?? 1;

            ViewBag.IsMain = true;
            var data = db.PatientAppointments;

            return View(data.ToPagedList((int)page, 10));
        }
        public ActionResult AppointmentReport()
        {
            ViewBag.AppointmentReports = db.PatientAppointments.ToList();
            return View();
        }
        public ActionResult getDoctors(DateTime doa, TimeSpan time)
        {
            var day = doa.DayOfWeek;
            var Doctors = db.DoctorsSchedules.Where(e => e.Days.Contains(day.ToString()) && (e.TimeFrom <= time && e.ToTime > time))
                .Select(x => new { Id = x.Doctor, Name = x.User1.Employee.FName + " " + x.User1.Employee.OtherName + " ( " + x.User1.Username + " )" }).ToList();
            return Json(Doctors, JsonRequestBehavior.AllowGet);

        }
    }
}