using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using PagedList;
using ProcurementDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Accounting.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private CaresoftProcurementEntities db2 = new CaresoftProcurementEntities();
        // GET: Accounting/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApproveVoucher(int? page)
        {
            if (page == null)
            {
                Session["StartDate"] = null;
                Session["ToDate"] = null;
                Session["SupplierName"] = null;
            }

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var todaysDate = DateTime.Now.Date;

            List<ProcurementDataAccess.Invoice> invoices = new List<ProcurementDataAccess.Invoice>();

            if (Session["StartDate"] != null && Session["ToDate"] != null)
            {
                DateTime fromDate = (DateTime)Session["StartDate"];
                DateTime toDate = (DateTime)Session["ToDate"];

                invoices = db2.Invoices.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
                ViewBag.SearchDates = true;

            }
            else if (Session["SupplierName"] != null)
            {
                var supplierName = Session["SupplierName"].ToString();

                invoices = db2.Invoices.Where(x => x.SupplierName.Contains(supplierName))
                                .OrderByDescending(p => p.Id)
                                .ToList();
            }
            else
            {
                invoices = db2.Invoices.Where(x => x.InvoiceDate == todaysDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
            }


            return View(invoices.ToPagedList(pageIndex, pageSize));
        }


        public struct Dates
        {
            public string fromDate { get; set; }
            public string toDate { get; set; }
            public string tarrif { get; set; }
        }

        //search approved/disapproved invoice by dates
        public ActionResult SearchByDatesInvoiceApprovedDisapproved(Dates dates)
        {
            Session["SupplierName"] = null;
            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);

            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var Data = db2.Invoices.Where(x => x.InvoiceDate >= fDate && x.InvoiceDate <= tDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageindex, pagesize);
            ViewBag.SearchDates = true;
            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/Accounting/Views/Home/_ApproveBills.cshtml", Data);
        }

        //search (dis)approved invoiced by supplier name
        public ActionResult GetSupplierInvoiceApprovedDisapproved(string supplierName, int? page)
        {
            Session["StartDate"] = null;
            Session["ToDate"] = null;


            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var todaysDate = DateTime.Now;
            //var nextDay = todaysDate.AddDays(1);

            //var data = db.Invoice.Where(x=>x.InvoiceDate >= todaysDate && x.InvoiceDate <= nextDay).ToList();
            var data = db2.Invoices.Where(x => x.SupplierName.Contains(supplierName))
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageIndex, pageSize);

            ViewBag.SearchPagination = true;
            Session["SupplierName"] = supplierName;


            return PartialView("~/Areas/Accounting/Views/Home/_ApproveBills.cshtml", data);
        }

        public ActionResult ApproveSpecificInvoice(int id)
        {


            var loggedInUser = db.Users.Find(Session["UserId"]);
            var UserName = loggedInUser.Username;


            var invoice = db2.Invoices.Where(p => p.Id == id).FirstOrDefault();

            invoice.ApprovedBy = UserName;
            invoice.FinalApproval = true;

            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();

            List<ProcurementDataAccess.Invoice> invoices = new List<ProcurementDataAccess.Invoice>();

            var todaysDate = DateTime.Now.Date;

            ProcurementDataAccess.ItemMaster itemMaster;

            List<ProcurementDataAccess.ItemMaster> lstItemsToBeAdded = new List<ProcurementDataAccess.ItemMaster>();
            var approvedItems = db2.InvoiceDetails.Where(p => p.InvoiceNo == id).ToList();

            //Add items in invoice to item master table
            foreach (var item in approvedItems)
            {
                var drug = db2.Drugs.Where(p => p.Id == item.DrugId).FirstOrDefault();

                itemMaster = new ProcurementDataAccess.ItemMaster();

                itemMaster.ItemName = drug.Name;
                itemMaster.BatchNo = item.BatchNo;
                itemMaster.GenericDrugName = drug.GenericDrugName?.Name ?? "";
                itemMaster.MfgCoName = item.MfgCoNm;
                itemMaster.Supplier = invoice.SupplierName;
                itemMaster.CurrentStock = item.Units;
                itemMaster.MfgDate = item.MfgDate.ToString();
                itemMaster.ReorderLevel = drug.ReorderLevel;
                itemMaster.UnitsPack = drug.UnitsPack;
                itemMaster.CostPriceUnit = item.CostPrice;
                itemMaster.ExpiryStatus = item.ExpiryStatus;
                itemMaster.PurchaseDate = item.ReceviedDate.ToString();
                itemMaster.barCode = "";
                itemMaster.CasePackRate = item.CasePackRate;
                itemMaster.StoreName = item.StoreFlag;

                lstItemsToBeAdded.Add(itemMaster);
            }

            db2.ItemMasters.AddRange(lstItemsToBeAdded);
            db.SaveChanges();

            if (Session["StartDate"] != null && Session["ToDate"] != null)
            {
                DateTime fromDate = (DateTime)Session["StartDate"];
                DateTime toDate = (DateTime)Session["ToDate"];

                invoices = db2.Invoices.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
                ViewBag.SearchDates = true;

            }
            else if (Session["SupplierName"] != null)
            {
                var supplierName = Session["SupplierName"].ToString();

                invoices = db2.Invoices.Where(x => x.SupplierName.Contains(supplierName))
                                .OrderByDescending(p => p.Id)
                                .ToList();
            }
            else
            {
                invoices = db2.Invoices.Where(x => x.InvoiceDate == todaysDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
            }

            int? page = 1;
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;


            return PartialView("~/Areas/Accounting/Views/Home/_ApproveBills.cshtml", invoices.ToPagedList(pageIndex, pageSize));
        }
        public ActionResult EditInvoice(int id)
        {

            var data = db2.Invoices.Find(id);





            return PartialView(data);
        }
        public ActionResult ApprovalForm(int id)
        {
            ViewBag.ApprovalId = id;
            return PartialView(db2.Invoices.Find(id));
        }

        public ActionResult CalculateInterest(int id)
        {
            ViewBag.InterestId = id;
            return PartialView(db.CreditTransfers.Find(id));
        }
        public ActionResult TotalSchemes(int id)
        {
            ViewBag.OpdNo = id;
            var data = db.BillServices.Where(e => e.OPDNo == id);
            return PartialView(data);
        }



        [HttpPost]
        public int EditInvoice(Invoice supplier)
        {
            if (ModelState.IsValid)
            {

                db2.Entry(supplier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return 1;

            }
            return 0;
        }
        [HttpPost]
        public ActionResult SaveApprovalForm(InvoiceApproval data)
        {
            var test = db2.Invoices.Where(e=> e.Id == data.InvoiceNo).FirstOrDefault().InvoiceAmount;


            if (test > data.ApprovalAmount) 
            {

                data.InvoiceAmount = test;
                data.BranchId = 1;
                data.UserId = (int)Session["UserId"];
                data.DateAdded = DateTime.Now;
                db2.InvoiceApprovals.Add(data);
                db2.SaveChanges();

                return View("ApproveBills");
            }
            else 
            {
                
                var script = 
                    
                    "alert('Message from Server')";
                return JavaScript(script);
            }
            
        }


        public ActionResult CreditTransfer()
		{
			return View();
		}
        [HttpPost]
        public ActionResult SaveCreditTransfer(CreditTransfer data)
        {
            data.BranchId = 1;
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            db.CreditTransfers.Add(data);
            db.SaveChanges();


            return View("CreditTransfer");
        }
       
        public ActionResult UpdateCreditTransfer(CreditTransfer data)
        {
            CreditTransfer item = db.CreditTransfers.Find(data.Id);
            var entry = db.CreditTransfers.Find(data.Id);
            entry.AccountNo = data.AccountNo;
            entry.Payer = data.Payer;
            //if (item != null)
            //{
            //    item.AccountNo = data.AccountNo;
            //   // item.Payer = data.Payer;
            //    UpdateModel(item);
            //    db.SaveChanges();
            //}
            db.SaveChanges();

            return View("ChangePayer");
        }
        public ActionResult UpdateCreditTransferInt(CreditTransfer data)
        {
            CreditTransfer item = db.CreditTransfers.Find(data.Id);
            var entry = db.CreditTransfers.Find(data.Id);
            entry.Interest = data.Interest;
            entry.Time = data.Time;
            entry.Rate = data.Rate;
            //if (item != null)
            //{
            //    item.AccountNo = data.AccountNo;
            //   // item.Payer = data.Payer;
            //    UpdateModel(item);
            //    db.SaveChanges();
            //}
            db.SaveChanges();

            return View("ChangePayer");
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
        public ActionResult SearchCreditTransfer(string search)
        {
            //search = search.ToLower().Trim();
            List<CreditAutoCompleteData> credit = db.CreditTransfers.OrderByDescending(f => f.Id).Where(e => e.AccountNo.Contains(search) ||
            e.Payer.Contains(search)).Select(
                x => new CreditAutoCompleteData
                {
                    AccountNo = x.AccountNo,
                    Payer = x.Payer,
                    SchemeName = x.SchemeName,

                    Id = x.Id,
                    DateAdded =x.DateAdded,
                    ReceiptDate = x.ReceiptDate,
                    Amount = x.Amount,
                    BalanceLeft = x.BalanceLeft,
                    OtherDetails = x.OtherDetails,
                    ReceiptNo = x.ReceiptNo,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    OriginalBalance = x.OriginalBalance,
                    

                }).Take(15).ToList();

            var jsonResult = Json(credit, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public ActionResult ChargeInterest()
		{
			return View();
		}
        //  Charge interest
        public ActionResult SearchByDebitDates(Dates dates)
        {
            Session["Debtor"] = null;
            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);

            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var Data = db.CreditTransfers.Where(x => x.DateAdded >= fDate && x.DateAdded <= tDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageindex, pagesize);
            ViewBag.SearchDates = true;
            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/Accounting/Views/Home/_ChargeInterest.cshtml", Data);
        }
        public ActionResult GetDebit(string supplierName, int? page)
        {
            Session["StartDate"] = null;
            Session["ToDate"] = null;


            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var todaysDate = DateTime.Now;
            //var nextDay = todaysDate.AddDays(1);

            //var data = db.Invoice.Where(x=>x.InvoiceDate >= todaysDate && x.InvoiceDate <= nextDay).ToList();
            var data = db.CreditTransfers.Where(x => x.Payer.Contains(supplierName))
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageIndex, pageSize);

            ViewBag.SearchPagination = true;
            Session["Debtor"] = supplierName;


            return PartialView("~/Areas/Accounting/Views/Home/_ChargeInterest.cshtml", data);
        }

        public ActionResult Debit(int id)
        {


            var loggedInUser = db.Users.Find(Session["UserId"]);
            var UserName = loggedInUser.Username;


            var invoice = db2.Invoices.Where(p => p.Id == id).FirstOrDefault();

            invoice.ApprovedBy = UserName;
            invoice.FinalApproval = true;

            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();

            List<ProcurementDataAccess.Invoice> invoices = new List<ProcurementDataAccess.Invoice>();

            var todaysDate = DateTime.Now.Date;

            ProcurementDataAccess.ItemMaster itemMaster;

            List<ProcurementDataAccess.ItemMaster> lstItemsToBeAdded = new List<ProcurementDataAccess.ItemMaster>();
            var approvedItems = db2.InvoiceDetails.Where(p => p.InvoiceNo == id).ToList();

            //Add items in invoice to item master table
            foreach (var item in approvedItems)
            {
                var drug = db2.Drugs.Where(p => p.Id == item.DrugId).FirstOrDefault();

                itemMaster = new ProcurementDataAccess.ItemMaster();

                itemMaster.ItemName = drug.Name;
                itemMaster.BatchNo = item.BatchNo;
                itemMaster.GenericDrugName = drug.GenericDrugName?.Name ?? "";
                itemMaster.MfgCoName = item.MfgCoNm;
                itemMaster.Supplier = invoice.SupplierName;
                itemMaster.CurrentStock = item.Units;
                itemMaster.MfgDate = item.MfgDate.ToString();
                itemMaster.ReorderLevel = drug.ReorderLevel;
                itemMaster.UnitsPack = drug.UnitsPack;
                itemMaster.CostPriceUnit = item.CostPrice;
                itemMaster.ExpiryStatus = item.ExpiryStatus;
                itemMaster.PurchaseDate = item.ReceviedDate.ToString();
                itemMaster.barCode = "";
                itemMaster.CasePackRate = item.CasePackRate;
                itemMaster.StoreName = item.StoreFlag;

                lstItemsToBeAdded.Add(itemMaster);
            }

            db2.ItemMasters.AddRange(lstItemsToBeAdded);
            db.SaveChanges();

            if (Session["StartDate"] != null && Session["ToDate"] != null)
            {
                DateTime fromDate = (DateTime)Session["StartDate"];
                DateTime toDate = (DateTime)Session["ToDate"];

                invoices = db2.Invoices.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
                ViewBag.SearchDates = true;

            }
            else if (Session["SupplierName"] != null)
            {
                var supplierName = Session["SupplierName"].ToString();

                invoices = db2.Invoices.Where(x => x.SupplierName.Contains(supplierName))
                                .OrderByDescending(p => p.Id)
                                .ToList();
            }
            else
            {
                invoices = db2.Invoices.Where(x => x.InvoiceDate == todaysDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
            }

            int? page = 1;
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;


            return PartialView("~/Areas/Accounting/Views/Home/_ApproveBills.cshtml", invoices.ToPagedList(pageIndex, pageSize));
        }



        public ActionResult ReturnedClaims()
		{
			return View();
		}

        public ActionResult SchemeReport()
        {
            var data = db.BillPayments.ToList();
            return View(data);
        }
        public class SchemeAmount
        {
            public int Id { get; set; }
            public Nullable<int> OPDNo { get; set; }
            public Nullable<int> WalkinId { get; set; }
            public int ShiftId { get; set; }
            public double BillAmount { get; set; }
            public double PaidAmount { get; set; }
            public double AwardedAmount { get; set; }
            public double AmountFromWallet { get; set; }
            public double Balance { get; set; }
            public double Discount { get; set; }
            public string BilledEntries { get; set; }
            public string BilledMedications { get; set; }
            public string BilledWalkinEntries { get; set; }
            public int PaymentModeId { get; set; }
            public string Currency { get; set; }
            public int BranchId { get; set; }
            public System.DateTime DateAdded { get; set; }

            //public virtual OpdRegister OpdRegister { get; set; }
            //public virtual PaymentMode PaymentMode { get; set; }
            public virtual Shift Shift { get; set; }
        }


        public ActionResult SearchByDatesScheme(Dates dates)
        {
            Session["SupplierName"] = null;
            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);

            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var Data = db.BillPayments.Where(x => x.DateAdded >= fDate && x.DateAdded <= tDate && x.OPDNo != null) 
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
           
            if(dates.tarrif != null)
            {
                Data = Data.Where(e => e.OpdRegister.Tariff.TariffName == dates.tarrif ).ToList();
            }
            ViewBag.item = Data.Count();
            ViewBag.total = Data.Select(e => e.AwardedAmount).Sum();

            ViewBag.SearchDates = true;
            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/Accounting/Views/Home/_PatientShemes.cshtml", Data);
        }



        //public ActionResult SearchItemId(int Id)
        //{
        //    var initialDate = DateTime.Now.AddYears(-10);
        //    var todaysDate = DateTime.Now.Date;
        //    var tomorrow = todaysDate.AddDays(1);
        //    var pharmacyStockPositions = GetItemsById(Id, initialDate, tomorrow);

        //    //session to be used together with generating the report so its important
        //    //Session["ItemName"] = name;

        //    return PartialView("~/Areas/Pharmarcy/Views/PharmacyReports/_lstStockPosition.cshtml", pharmacyStockPositions);
        //}






        public ActionResult ResubmittClaims()
		{
			return View();
		}
		public ActionResult AllocateSchemeInvoices()
		{
            //using(var data = new CaresoftHMISEntities()) { 

            //ViewBag.Amount = data.BillPayments.SqlQuery("select * from BillPayments").ToList<BillPayment>();

            //}
            var data = db.BillPayments.ToList();

            return View(data);
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
        [HttpPost]
        public ActionResult SaveDebitNote(DebitNote data)
        {
            
            

               
                data.BranchId = 1;
                data.UserId = (int)Session["UserId"];
                data.DateAdded = DateTime.Now;
                db2.DebitNotes.Add(data);
                db2.SaveChanges();

                return View("DebitNotes");
            
            

        }
        public ActionResult SearchInvoice(string search)
        {
            //search = search.ToLower().Trim();
            List<InvoiceAutoCompleteData> invoice = db2.Invoices.OrderByDescending(f => f.Id).Where(e => e.InvoiceNo.Contains(search) ||
            e.SupplierName.Contains(search)).Select(
                x => new InvoiceAutoCompleteData
                {
                    InvoiceNo = x.InvoiceNo,
                    SupplierName = x.SupplierName,
                    SupplierId = x.SupplierId,
                    
                    InvoiceDate = x.InvoiceDate,

                }).Take(15).ToList();

            var jsonResult = Json(invoice, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


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