using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.BloodBank.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
		private CaresoftHMISEntities db = new CaresoftHMISEntities();
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult CampOrganizer()
		{
			return PartialView();
		}
		public ActionResult CampInformation()
		{
			return PartialView();
		}
		public ActionResult DonorInformation()
		{
			return PartialView();
		}
		public ActionResult DonorBloodGrouping()
		{
			return PartialView();
		}
		public ActionResult DonorBloodBankTest()
		{
			return PartialView();
		}
		public ActionResult BloodComponets()
		{
			return PartialView();
		}
		public ActionResult BloodBankInformation()
		{
			return PartialView();
		}
		public ActionResult PatientRequisition()
		{
			return PartialView();
		}
		public ActionResult PatientBloodGrouping()
		{
			return PartialView();
		}
		public ActionResult CrossMatch()
		{
			return PartialView();
		}
		public ActionResult IssueBloodBag()
		{
			return PartialView();
		}
		public ActionResult PatientBill()
		{
			return PartialView();
		}
		public ActionResult AdvanceBillAmount()
		{
			return PartialView();
		}
		public ActionResult BillRefund()
		{
			return PartialView();
		}
		public ActionResult MasterSetting()
		{
			return PartialView();
		}
		public ActionResult PostCampOrganizer(BloodBankCampOrganizer CampOrganizer)
		{
			if (ModelState.IsValid)
			{
				Session["UserId"] = CampOrganizer.Id;
				CampOrganizer.Id = (int)Session["UserId"];
				CampOrganizer.Date = DateTime.UtcNow;
				db.BloodBankCampOrganizers.Add(CampOrganizer);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}
	}
}