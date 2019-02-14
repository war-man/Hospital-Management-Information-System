using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class billingController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
       // private CareSoftLabsEntities labDb = new CareSoftLabsEntities();

        // GET: HIV/billing
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Delete(int id)
        {
            var loggedInUser = db.Users.Find(1);
            var billItem = db.BillServices.Find(id);

            if (billItem == null)
            {
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }

            var anyUserDeleteItems = true;

            var usersDeleteUnpaidItems = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Trim() == "users_delete_unpaid_items");

            if (usersDeleteUnpaidItems != null)
            {
                if (usersDeleteUnpaidItems.Value.Trim().ToLower() != "yes")
                {
                    anyUserDeleteItems = false;
                }
            }
            else
            {
                var keyValuePair = new KeyValuePair();
                keyValuePair.Key_ = "users_delete_unpaid_items";
                keyValuePair.Value = "NO";
                db.KeyValuePairs.Add(keyValuePair);
                db.SaveChanges();
            }
            if (billItem.IPDBillPartialPayments.Count() > 0 || billItem.Paid || billItem.Offered || billItem.Award > 0 || (billItem.WaivedAmount ?? 0) > 0)
            {
                return Json(new { Status = "warning", Message = "Sorry. This item cannot be deleted. It has either been offered, paid for, awarded or waived." }, JsonRequestBehavior.AllowGet);

            }
            if (anyUserDeleteItems || billItem.UserId == loggedInUser.Id || loggedInUser.UserRole.RoleName.ToLower() == "sa" || loggedInUser.UserRole.RoleName.ToLower() == "admin")
            {
                db.BillServices.Remove(billItem);
                db.SaveChanges();
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "warning", Message = "Sorry. You are not allowed to delete this item. Only the person who added it can do so." }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}