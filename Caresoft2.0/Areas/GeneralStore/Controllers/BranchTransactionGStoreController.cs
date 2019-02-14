using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.GeneralStore.Controllers
{
    [Auth]
    public class BranchTransactionGStoreController : Controller
    {

        private ProcurementDbContext db = new ProcurementDbContext();

        // GET: GeneralStore/BranchTransactionGStore
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult ItemReceivedDetails()
        {

            var data = db.ItemsRecieved.ToList();
            return View(data);
        }

        public ActionResult ItemsRecievedPartial(string Name)
        {
            if (Name != null)
            {
                var data = db.ItemsRecieved.Where(p => p.processed == Name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/BranchTransactionMStore/_lstItemsRecieved.cshtml", data);
            }
            return null;
        }

        public ActionResult AcknowledgementDetail()
        {
            var data = db.RecieveDetails.ToList();
            return View(data);
        }

        public ActionResult AcknowledgementDetailPartial(string Name)
        {
            if (Name != null)
            {
                var data = db.RecieveDetails.Where(p => p.processed == Name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/BranchTransactionMStore/_lstAcknowledgement.cshtml", data);
            }
            return null;
        }
    }
}