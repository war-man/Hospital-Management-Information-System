using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.PatientModule.Controllers
{
	public class HomeController : Controller
	{
		private CaresoftHMISEntities db = new CaresoftHMISEntities();
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Clinical() 
		{
			return View();
		}
		public ActionResult LabReport()
		{
			return View();
		}
		public ActionResult RequestService()
		{
			return View();
		}
		public ActionResult FeedBack()
		{
			return View();
		}
		public ActionResult Appointment()
		{
			ViewBag.PatientAppointment = db.Patients.ToList();
			ViewBag.Speciality = db.Departments.ToList();
			ViewBag.Doctors = db.Users.Where(e => e.DoctorsSchedules.Any()).ToList();
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
		public ActionResult getDoctors(DateTime doa, TimeSpan time)
		{
			var day = doa.DayOfWeek;
			var Doctors = db.DoctorsSchedules.Where(e => e.Days.Contains(day.ToString()) && (e.TimeFrom <= time && e.ToTime > time))
				.Select(x => new { Id = x.Doctor, Name = x.User1.Employee.FName + " " + x.User1.Employee.OtherName + " ( " + x.User1.Username + " )" }).ToList();
			return Json(Doctors, JsonRequestBehavior.AllowGet);

		}
	}
}