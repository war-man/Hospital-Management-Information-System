using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class FinanceController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: Finance
        public ActionResult CashPointReconciliation()
        {

            return View(db.Users.ToList());
        }

        public ActionResult ShiftsList(int? id)
        {
            var content = "<option>--Select</option>";
            var user = db.Users.Find(id);
            if (user != null)
            {
                var shifts = user.Shifts.OrderByDescending(e=>e.Id).Where(e=>e.Endtime !=null && e.CashPointReconciliations.Count()<1).ToList();
                foreach(var shift in shifts)
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
                Amounts = new { Cash = shift.BillPayments
                 .Where(e => e.PaymentMode.PaymentModeName.ToLower().Trim().Equals("cash"))
                 .Sum(e => e.BillAmount),
                 Cheques = shift.BillPayments
                 .Where(e => e.PaymentMode.PaymentModeName.ToLower().Trim().Equals("cheque"))
                 .Sum(e => e.BillAmount)

                }
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostReconciliation(int ShiftId, double ActualAmountCash, double ActualAmountCheque)
        {

            var shift = db.Shifts.Find(ShiftId);
            FinanceCashPointReconciliation entry = new FinanceCashPointReconciliation();
            entry.ShiftId = ShiftId;
            entry.CashActualAmount = ActualAmountCash;
            entry.ChequeActualAmount = ActualAmountCheque;
            entry.TimeAdded = DateTime.Now;
            entry.UserId = int.Parse(Session["UserId"].ToString());
            
            entry.BranchId = (int)Session["UserBranchId"] ;
            db.FinanceCashPointReconciliations.Add(entry);
            db.SaveChanges();

            return Json(new { Status = true});
        }
    }
}