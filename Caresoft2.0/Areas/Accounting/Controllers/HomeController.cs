using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Accounting.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        // GET: Accounting/Home
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult CreditTransfer()
		{
			return View();
		}
		
		public ActionResult MiscDebtors()
		{
			return View();
		}

		public ActionResult MergeSchemeAccounts()
		{
			return View();
		}

		public ActionResult ChangePayer()
		{
			return View();
		}

		public ActionResult ChargeInterest()
		{
			return View();
		}

		public ActionResult ReturnedClaims()
		{
			return View();
		}

		public ActionResult ResubmittClaims()
		{
			return View();
		}
		public ActionResult AllocateSchemeInvoices()
		{
			return View();
		}
		public ActionResult AllocateInvoicePartly()
		{
			return View();
		}
		public ActionResult AllocateInvoicebyPayer()
		{
			return View();
		}

		public ActionResult AllocationDocExcempted()
		{
			return View();				 
		}
		public ActionResult AllocationWithoutCredits()
		{
			return View();
		}
		public ActionResult AutomatedWriteoffs()
		{
			return View();
		}
		public ActionResult AllocateDoctorsInvoices()
		{
			return View();
		}
		public ActionResult ReviseAllocation()
		{
			return View();
		}
		public ActionResult Provision()
		{
			return View();
		}
		public ActionResult ProvisionsPerDebtors()
		{
			return View();
		}
		public ActionResult EnterBills()
		{
			return View();
		}
		public ActionResult LinkGMToCreditors()
		{
			return View();
		}
		public ActionResult DebitNotes()
		{
			return View();
		}
		public ActionResult ApproveBills()
		{
			return View();
		}
		public ActionResult CreditorsAllocation()
		{
			return View();
		}
		public ActionResult DoctorsLedger()
		{
			return View();
		}
		public ActionResult MiscCreditors()
		{
			return View();
		}
		public ActionResult Journal()
		{
			return View();
		}
		public ActionResult ProjectJournals()
		{
			return View();
		}
		public ActionResult WriteOffsDebits()
		{
			return View();
		}
		public ActionResult WriteOffsCredits()
		{
			return View();
		}
		public ActionResult MergeGLAcc()
		{
			return View();
		}
		public ActionResult AuditAccounts()
		{
			return View();
		}
	}
}