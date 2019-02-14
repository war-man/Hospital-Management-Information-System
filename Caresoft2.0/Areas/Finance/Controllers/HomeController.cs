using System;
using System.Linq;
using System.Web.Mvc;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Areas.Finance.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Finance/Home
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Banking()
        {
            ViewBag.Banks = db.FinanceBanks.ToList();
			ViewBag.MpesaAmount = db.FinanceCashPointReconciliations.Sum(e => e.MpesaActualAmount);
			ViewBag.cashAmount = db.FinanceCashPointReconciliations.Sum(e => e.CashActualAmount);

			return View();
        }

        public ActionResult BankReconciliation()
        {
			ViewBag.banks = db.FinanceBanks.ToList();
            return View();
        }
        public ActionResult BankCharges()
        {
			ViewBag.PaymentModes = db.PaymentModes.ToList();
			ViewBag.banks = db.FinanceBanks.ToList();
			return View();
        }

        public ActionResult DirectBanking()
        {
			ViewBag.Banks = db.FinanceBanks.ToList();
			return View();
        }

        public ActionResult ConfirmBanking()
        {
			ViewBag.DateBanked = db.FinanceBankings.Where(e => ! e.FinanceComfirmBankings.Any(f => f.BankingID == e.ID)).ToList();

			return View();
        }

        public ActionResult TotalShiftReport()
        {
			var data = db.Shifts.Where(e => e.Endtime == null);
			return View(data);
        }
        public ActionResult Payments()
        {
            return View();
        }

        public ActionResult LumpsumPayments()
        {
            return View();
        }
        public ActionResult Cosultants()
        {
            return View();
        }

        public ActionResult PaymentsAdjustment()
        {
            return View();

		}

        public ActionResult RaiseVoucher()
        {
            return View();
        }

        public ActionResult ApproveVoucher()
        {
            return View();
        }

        public ActionResult PettyPayments()
        {
            return View();
        }

        public ActionResult DirectNGOPayments()
        {
            return View();
        }
        public ActionResult DirectCashtPayments()
        {
            return View();
        }

        public ActionResult PettyCashAdjustment()
        {
            return View();
        }

        public ActionResult ReconcileCash()
        {

            return View(db.Users.ToList());
        }

        public ActionResult ShiftsList(int? id)
        {
            var content = "<option>--Select</option>";
            var user = db.Users.Find(id);
            if (user != null)
            {
                var shifts = user.Shifts.OrderByDescending(e => e.Id).Where(e => e.Endtime != null && e.CashPointReconciliations.Count() < 1).ToList();
                foreach (var shift in shifts)
                {
                    content += "<option value=" + shift.Id + ">" + shift.Id + "</option>";
                }
            }

            return Content(content);
        }

        public ActionResult ShiftDetails(int? id)
        {
            var shift = db.Shifts.Find(id);
            var open = true;
            var duration = DateTime.Now - shift.StartTime;
            if (shift.Endtime != null)
            {
                open = false;
                duration = shift.Endtime.Value - shift.StartTime;
            }
            var obj = new
            {
                STime = shift.StartTime.ToString("yyyy-MMM-dd H:m:s"),
                ETime = shift.Endtime.Value.ToString("yyyy-MMM-dd H:m:s"),
                Duration = duration,
                Open = open,
                Amounts = new
                {
                    Cash = shift.BillPayments
                 .Where(e => e.PaymentMode.PaymentModeName.ToLower().Trim().Equals("cash"))
                 .Sum(e => e.BillAmount),
                    Cheques = shift.BillPayments
                 .Where(e => e.PaymentMode.PaymentModeName.ToLower().Trim().Equals("cheque"))
                 .Sum(e => e.BillAmount)

                }
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostReconciliation(int ShiftId, double ActualAmountCash, double ActualAmountCheque, double MpesaActualAmount, String ChequeNumbers)
        {

            var shift = db.Shifts.Find(ShiftId);
           FinanceCashPointReconciliation entry = new FinanceCashPointReconciliation();
            entry.ShiftId = ShiftId;
            entry.CashActualAmount = ActualAmountCash;
            entry.ChequeActualAmount = ActualAmountCheque;
            entry.TimeAdded = DateTime.Now;
            entry.UserId = 1; /*int.Parse(Session["UserId"].ToString());*/
            entry.BranchId = 1;
            entry.MpesaActualAmount = MpesaActualAmount;
            entry.ChequeNumbers = ChequeNumbers;
            db.FinanceCashPointReconciliations.Add(entry);
            db.SaveChanges();

            return Json(new { Status = true });
        }

		public ActionResult PostBanking(DateTime FromDate, DateTime ToDate, double CashAmountToBank, double MpesaAmountToBank, String ChequeNumbers, String GIName, int AccNo, String Branch)
		{
			FinanceBanking entry = new FinanceBanking();
			entry.FromDate = FromDate;
			entry.ToDate = ToDate;
			entry.DateBanked = DateTime.UtcNow;
			entry.ChequeNumbers = ChequeNumbers;
			entry.CashAmountToBank = CashAmountToBank;
			entry.MpesaAmountToBank = MpesaAmountToBank;
			entry.GIName = GIName;
			entry.AccNo = AccNo;
			entry.Branch = Branch;
			entry.UserId = 1; /*int.Parse(Session["UserId"].ToString());*/
			db.FinanceBankings.Add(entry);
			db.SaveChanges();

			return Json(new { Status = true });
		}

		public ActionResult getBankDetails(string GIName)
        {
            var data = db.FinanceBanks.Where(e => e.GIName == GIName).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }	
			
		public ActionResult PostDirectBanking( FinanceDirectBanking DirectBanking)
		{			
			if (ModelState.IsValid)
			{
				DirectBanking.UserId = 1;
				DirectBanking.Date = DateTime.UtcNow;
				db.FinanceDirectBankings.Add(DirectBanking);
				db.SaveChanges();
				return RedirectToAction("DirectBanking");
			}
			return RedirectToAction("DirectBanking");
		}

		public ActionResult getTotals(DateTime FromDate, DateTime ToDate)
		{
			ToDate = ToDate.AddDays(1);
			var data = db.FinanceCashPointReconciliations.Where(e => e.TimeAdded < ToDate && e.TimeAdded >= FromDate); 
			var cash = data.Sum(e => e.CashActualAmount);
			var mpesa = data.Sum(e => e.MpesaActualAmount);
			var obj = new {
				cash = cash,
				mpesa = mpesa
			};
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult getBanking( int id)
		{

			var obj = db.FinanceBankings.FirstOrDefault(e => e.ID == id);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult PostComfirmation( FinanceComfirmBanking PostComfirmation)
		{
			PostComfirmation.DateComfirmed = DateTime.UtcNow;
			PostComfirmation.User = 1;			
			if (ModelState.IsValid)
			{
				db.FinanceComfirmBankings.Add(PostComfirmation);				
				db.SaveChanges();
			}
			return RedirectToAction("ConfirmBanking");
		}

		public ActionResult getCashTotals(int AccNo)
		{
			var sumCash = db.FinanceBankings.Where(e => e.AccNo == AccNo);
			var Cash = (sumCash.Sum(e => e.CashAmountToBank)) + (sumCash.Sum(e => e.MpesaAmountToBank));
			return Json(Cash, JsonRequestBehavior.AllowGet);
		}
				
	}

}