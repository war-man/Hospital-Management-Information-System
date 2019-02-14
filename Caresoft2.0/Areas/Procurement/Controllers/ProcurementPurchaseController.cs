using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.Areas.Procurement.Reports;
using Caresoft2._0.Areas.Procurement.Repository;
using Caresoft2._0.Areas.Procurement.ViewModel;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Caresoft2._0.Areas.Procurement.Reports.ProvisionalPO;
using Caresoft2._0.Areas.Procurement.Reports.CompareProvisionalPO;
using System.Net;
using PagedList;
using System.Globalization;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Areas.Procurement.Controllers
{
    [Auth]
    public class ProcurementPurchaseController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();
        CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: Procurement/Purchase
        public ActionResult Index()
        {
            return View();
        }

        #region Add New Invoice

        public ActionResult AddNewInvoice(int? InvoiceNo)
        {
            if (Session["InvoiceNumber"] != null)
            {
                int invoiceNumber = Convert.ToInt32(Session["InvoiceNumber"]);

                InvoiceAndInvoiceDetails invoiceAndInvoiceDetails = new InvoiceAndInvoiceDetails()
                {
                    Invoice = db.Invoice.Where(p => p.Id == invoiceNumber).FirstOrDefault(),
                    InvoiceDetails = db.InvoiceDetail.Where(p => p.InvoiceNo == invoiceNumber).OrderByDescending(p => p.Id).ToList()

                };

                return View(invoiceAndInvoiceDetails);
            }
            else if (InvoiceNo != null)
            {
                int invoiceNumber = Convert.ToInt32(InvoiceNo);

                InvoiceAndInvoiceDetails invoiceAndInvoiceDetails = new InvoiceAndInvoiceDetails()
                {
                    Invoice = db.Invoice.Where(p => p.Id == invoiceNumber).FirstOrDefault(),
                    InvoiceDetails = db.InvoiceDetail.Where(p => p.InvoiceNo == invoiceNumber).OrderByDescending(p => p.Id).ToList()

                };

                return View(invoiceAndInvoiceDetails);
            }

            InvoiceAndInvoiceDetails invoiceAndInvoiceDetail = new InvoiceAndInvoiceDetails()
            {
                Invoice = new Models.Invoice(),
                InvoiceDetails = new List<Models.InvoiceDetail>()
            };

            return View(invoiceAndInvoiceDetail);
        }

        [HttpPost]
        public ActionResult AddNewInvoice(AddNewInvoice addNewInvoice)
        {
            //add to details table
            RepoProcurement<Models.Invoice> invoice = new RepoProcurement<Models.Invoice>();
            RepoProcurement<Models.InvoiceDetail> invoiceDetails = new RepoProcurement<Models.InvoiceDetail>();

            //if (Session["InvoiceNumber"] == null)
            //{
            //    db.Invoice.Add(addNewInvoice.invoice);
            //    db.SaveChanges();

            //    Session["InvoiceNumber"] = addNewInvoice.invoice.Id;
            //}

            var invoiceexists = db.Invoice.FirstOrDefault( e => e.InvoiceNo == addNewInvoice.invoice.InvoiceNo);
            if (invoiceexists == null)
            {
                var SupplierId = db.supplier.FirstOrDefault(e => e.Supplier_Name == addNewInvoice.invoice.SupplierName);
                addNewInvoice.invoice.SupplierId = SupplierId.Supplier_Code;
                db.Invoice.Add(addNewInvoice.invoice);
                db.SaveChanges();

                invoiceexists = db.Invoice.FirstOrDefault(e => e.InvoiceNo == addNewInvoice.invoice.InvoiceNo);

                Session["InvoiceNumber"] = addNewInvoice.invoice.Id;
            }


            //make the last invoice the invoice number for the invoice detail

            addNewInvoice.invoiceDetails.InvoiceNo = invoiceexists.Id;
            var drugid = db.Drug.Where(p => p.Name.Contains(addNewInvoice.invoiceDetails.Item)).FirstOrDefault().Id;
            addNewInvoice.invoiceDetails.DrugId = drugid;

            if (invoiceexists == null)
            {
                //add new invoice detail if invoice
                db.InvoiceDetail.Add(addNewInvoice.invoiceDetails);
                db.SaveChanges();
            }
            else if(invoiceexists != null && !invoiceexists.FinalApproval)
            {
                //add new invoice detail if invoice hasent been authorized
                db.InvoiceDetail.Add(addNewInvoice.invoiceDetails);
                db.SaveChanges();
            }
               

           

            int invNo = Convert.ToInt32(Session["InvoiceNumber"]);
            var model = db.InvoiceDetail.Where(p => p.InvoiceNo == invoiceexists.Id).OrderByDescending(p => p.Id).ToList();

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_InvoiceList.cshtml", model);
        }

        //search supplier names
        public JsonResult SearchSupplierName(string name)
        {
            //RepoProcurement<supplier> db = new RepoProcurement<supplier>();

            if (!string.IsNullOrEmpty(name))
            {
                var data = db.supplier.Where(p => p.Supplier_Name.Contains(name)).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //search drug names
        public JsonResult SearchItemName(string name)
        {

            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => p.Name.Contains(name))
                                        .Select(x => new { x.Id, x.Name, x.UnitsPack, }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNewDrug(Drug drug)
        {
            db.Drug.Add(drug);
            db.SaveChanges();

            //if (ModelState.IsValid)
            //{
            //    //RepoProcurement<Drug> repoProcurement = new RepoProcurement<Drug>();
            //    //repoProcurement.Add(drug);

            //    db.Drug.Add(drug);
            //    db.SaveChanges();
            //}
            return Json("");
        }

        //when the button Add New Is clicked
        public ActionResult AddNew()
        {
            Session["InvoiceNumber"] = null;
            IEnumerable<AddNewInvoice> model = null;
            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_InvoiceList.cshtml", model);
        }

        public ActionResult SaveButtonInvoiceMain(SaveInvoiceMain model)
        {
            int invoiceNumber = Convert.ToInt32(Session["InvoiceNumber"]);
            var invoice = db.Invoice.Find(invoiceNumber);

            if (Session["InvoiceNumber"] != null)
            {
                invoice.InvoiceAmount = model.Amount;
                invoice.InvoiceDiscount = model.discount;
                db.Entry(invoice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["InvoiceNumber"] = null;

                var InvoiceDetails = new List<Models.InvoiceDetail>();

                return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_InvoiceList.cshtml", InvoiceDetails);

            }

            return Json("Invoice is null");
        }

        public ActionResult GenerateReport()
        {
            if (Session["InvoiceNumber"] != null)
            {

                int invoiceNo = Convert.ToInt32(Session["InvoiceNumber"]);
                var _invoice = db.Invoice.Where(p => p.Id == invoiceNo).Select(x => new { x.Id, x.SupplierName, x.InvoiceDate }).FirstOrDefault();
                var _InvoiceDetails = db.InvoiceDetail.Where(p => p.InvoiceNo == invoiceNo).ToList();

                DataSetInvoices dataSetInvoices = new DataSetInvoices();
                dataSetInvoices.Invoice.AddInvoiceRow(_invoice.Id.ToString(), _invoice.SupplierName, _invoice.InvoiceDate.ToString());

                //loop through the invoice details
                foreach (var item in _InvoiceDetails)
                {
                    dataSetInvoices.InvoiceDetails.AddInvoiceDetailsRow(
                        item.Id.ToString(),
                        item.Item,
                        item.per.ToString(),
                        item.Units.ToString(),
                        item.VatAmt,
                        item.Discount,
                        item.FreeUnits.ToString(),
                        item.Amount,
                        item.BatchNo,
                        item.ExpDate.ToString()
                        );
                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Areas/Procurement/Reports/Invoice.rpt")));
                rd.SetDataSource(dataSetInvoices);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(
                    CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Invoice.pdf");

            }

            return Json("Invoice Number is Null", JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult EditInvoiceDetail(int? Id)
        {
            if (Id != null)
            {
                int InvoiceId = (int)Id;

                var invoiceDetails = db.InvoiceDetail.Find(InvoiceId);
                return Json(invoiceDetails, JsonRequestBehavior.AllowGet);
            }
            return Json("");
        }

        [HttpPost]
        public ActionResult EditInvoiceDetail(Models.InvoiceDetail InvoiceDetailsEdit)
        {
            if (InvoiceDetailsEdit != null)
            {
                int Id = InvoiceDetailsEdit.Id;
                db.Entry(InvoiceDetailsEdit).State = EntityState.Modified;
                db.SaveChanges();

                int invoiceNumber = (int)InvoiceDetailsEdit.InvoiceNo;

                var model = db.InvoiceDetail.Where(p => p.InvoiceNo == invoiceNumber).OrderByDescending(p => p.Id).ToList();
                return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_InvoiceList.cshtml", model);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //edit invoice 
        public ActionResult EditInvoice(int? page)
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

            List<Models.Invoice> invoices = new List<Models.Invoice>();

            if (Session["StartDate"] != null && Session["ToDate"] != null)
            {
                DateTime fromDate = (DateTime)Session["StartDate"];
                DateTime toDate = (DateTime)Session["ToDate"];

                invoices = db.Invoice.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
                ViewBag.SearchDates = true;

            }
            else if (Session["SupplierName"] != null)
            {
                var supplierName = Session["SupplierName"].ToString();

                invoices = db.Invoice.Where(x => x.SupplierName.Contains(supplierName))
                                .OrderByDescending(p => p.Id)
                                .ToList();
            }
            else
            {
                invoices = db.Invoice.Where(x => x.InvoiceDate == todaysDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
            }


            return View(invoices.ToPagedList(pageIndex, pageSize));
        }
        // search invoices using supplier names
        public ActionResult GetSupplierInvoiceDetail(string supplierName, int? page)
        {
            Session["StartDate"] = null;
            Session["ToDate"] = null;


            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var todaysDate = DateTime.Now;
            //var nextDay = todaysDate.AddDays(1);

            //var data = db.Invoice.Where(x=>x.InvoiceDate >= todaysDate && x.InvoiceDate <= nextDay).ToList();
            var data = db.Invoice.Where(x => x.SupplierName.Contains(supplierName))
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageIndex, pageSize);

            ViewBag.SearchPagination = true;
            Session["SupplierName"] = supplierName;

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_InvoiceDetails.cshtml", data);
        }

        public struct Dates
        {
            public string fromDate { get; set; }
            public string toDate { get; set; }
        }
        //search invoices using dates
        public ActionResult SearchByDatesInvoice(Dates dates)
        {
            Session["SupplierName"] = null;
            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);

            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var Data = db.Invoice.Where(x => x.InvoiceDate >= fDate && x.InvoiceDate <= tDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageindex, pagesize);
            ViewBag.SearchDates = true;
            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_InvoiceDetails.cshtml", Data);
        }

        public ActionResult AddProvisionalPo()
        {

            if (Session["ProvisionalPurchaseOrderId"] != null)
            {
                int id = (int)Session["ProvisionalPurchaseOrderId"];

                ProvisionalPurchaseOrder provisionalPurchaseOrder = db.ProvisionalPurchaseOrder
                                                                        .Where(p => p.Id == id)
                                                                        .Include(p => p.POItemsDetail)
                                                                        .Include(p => p.Suppliers)
                                                                        .FirstOrDefault();
                //perform explicit loading 
                foreach (var item in provisionalPurchaseOrder.POItemsDetail)
                {
                    db.Entry(item).Reference(x => x.Drug).Load();
                }

                ProvisionalPurchaseOrderViewModel model = new ProvisionalPurchaseOrderViewModel()
                {
                    Suppliers = db.supplier.ToList(),
                    ProvisionalPOItemsDetail = provisionalPurchaseOrder.POItemsDetail
                };


                return View(model);
            }
            else
            {
                ProvisionalPurchaseOrderViewModel model = new ProvisionalPurchaseOrderViewModel()
                {
                    Suppliers = db.supplier.ToList(),
                    ProvisionalPOItemsDetail = new List<ProvisionalPOItemsDetail>()
                };

                return View(model);
            }

        }


        //approve invoice 
        public ActionResult ApproveInvoice(int? page)
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

            List<Models.Invoice> invoices = new List<Models.Invoice>();

            if (Session["StartDate"] != null && Session["ToDate"] != null)
            {
                DateTime fromDate = (DateTime)Session["StartDate"];
                DateTime toDate = (DateTime)Session["ToDate"];

                invoices = db.Invoice.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
                ViewBag.SearchDates = true;

            }
            else if (Session["SupplierName"] != null)
            {
                var supplierName = Session["SupplierName"].ToString();

                invoices = db.Invoice.Where(x => x.SupplierName.Contains(supplierName))
                                .OrderByDescending(p => p.Id)
                                .ToList();
            }
            else
            {
                invoices = db.Invoice.Where(x => x.InvoiceDate == todaysDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
            }


            return View(invoices.ToPagedList(pageIndex, pageSize));
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

            var Data = db.Invoice.Where(x => x.InvoiceDate >= fDate && x.InvoiceDate <= tDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageindex, pagesize);
            ViewBag.SearchDates = true;
            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_ApproveInvoice.cshtml", Data);
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
            var data = db.Invoice.Where(x => x.SupplierName.Contains(supplierName))
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageIndex, pageSize);

            ViewBag.SearchPagination = true;
            Session["SupplierName"] = supplierName;

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_ApproveInvoice.cshtml", data);
        }

        public ActionResult ApproveSpecificInvoice(int id)
        {


            var loggedInUser = db2.Users.Find(Session["UserId"]);
            var UserName = loggedInUser.Username;


            var invoice = db.Invoice.Where(p => p.Id == id).FirstOrDefault();

            invoice.ApprovedBy = UserName;
            invoice.FinalApproval = true;

            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();

            List<Models.Invoice> invoices = new List<Models.Invoice>();

            var todaysDate = DateTime.Now.Date;

            Models.ItemMaster itemMaster;

            List<Models.ItemMaster> lstItemsToBeAdded = new List<Models.ItemMaster>();
            var approvedItems = db.InvoiceDetail.Where(p => p.InvoiceNo == id).ToList();

            //Add items in invoice to item master table
            foreach (var item in approvedItems)
            {
                var drug = db.Drug.Where(p => p.Id == item.DrugId).FirstOrDefault();

                itemMaster = new Models.ItemMaster();

                itemMaster.ItemName = drug.Name;
                itemMaster.BatchNo = item.BatchNo;
                itemMaster.GenericDrugName = drug.genericDrugName?.Name ?? "";
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

            db.ItemMaster.AddRange(lstItemsToBeAdded);
            db.SaveChanges();

            if (Session["StartDate"] != null && Session["ToDate"] != null)
            {
                DateTime fromDate = (DateTime)Session["StartDate"];
                DateTime toDate = (DateTime)Session["ToDate"];

                invoices = db.Invoice.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
                ViewBag.SearchDates = true;

            }
            else if (Session["SupplierName"] != null)
            {
                var supplierName = Session["SupplierName"].ToString();

                invoices = db.Invoice.Where(x => x.SupplierName.Contains(supplierName))
                                .OrderByDescending(p => p.Id)
                                .ToList();
            }
            else
            {
                invoices = db.Invoice.Where(x => x.InvoiceDate == todaysDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList();
            }

            int? page = 1;
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;


            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_ApproveInvoice.cshtml", invoices.ToPagedList(pageIndex, pageSize));
        }

        [HttpPost]
        public ActionResult AddProvisionalPo(AddNewPO addNewPO)
        {


            if (addNewPO != null)
            {
                if (Session["ProvisionalPurchaseOrderId"] == null)
                {
                    //here we add the provisional 
                    db.ProvisionalPurchaseOrder.Add(addNewPO.provisionalPurchaseOrder);
                    db.SaveChanges();

                    var id = addNewPO.provisionalPurchaseOrder.Id;

                    //here i get the provisional order i just saved 
                    var provisionalPurchaseOrder = db.ProvisionalPurchaseOrder
                                                     .Include(p => p.Suppliers)
                                                     .Include(p => p.POItemsDetail)
                                                     .Where(p => p.Id == id)
                                                     .FirstOrDefault();
                    //here add the navigation property for suppliers
                    foreach (var item in addNewPO.suppliers)
                    {
                        var supplier = db.supplier
                                         .Where(p => p.Supplier_Code == item)
                                         .FirstOrDefault();

                        provisionalPurchaseOrder.Suppliers.Add(supplier);
                    }

                    db.SaveChanges();


                    db.ProvisionalPOItemsDetail.Add(addNewPO.provisionalPOItemsDetail);
                    db.SaveChanges();

                    provisionalPurchaseOrder.POItemsDetail.Add(addNewPO.provisionalPOItemsDetail);
                    db.SaveChanges();

                    Session["ProvisionalPurchaseOrderId"] = id;

                    var model = provisionalPurchaseOrder.POItemsDetail.ToList();

                    foreach (var item in model)
                    {
                        //explicit loading
                        db.Entry(item).Reference(x => x.Drug).Load();
                    }



                    return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_ProvisionalPurchaseList.cshtml", model);
                }
                else
                {
                    int id = (int)Session["ProvisionalPurchaseOrderId"];

                    var ProvisionalPurchaseOrder = db.ProvisionalPurchaseOrder
                                                        .Where(p => p.Id == id)
                                                        .Include(p => p.POItemsDetail)
                                                        .Include(x => x.POItemsDetail.Select(t => t.Drug))
                                                        .FirstOrDefault();

                    ProvisionalPOItemsDetail provisionalPOItemsDetail = new ProvisionalPOItemsDetail()
                    {
                        DrugId = addNewPO.provisionalPOItemsDetail.DrugId,
                        Quantity = addNewPO.provisionalPOItemsDetail.Quantity

                    };

                    ProvisionalPurchaseOrder.POItemsDetail.Add(provisionalPOItemsDetail);
                    db.SaveChanges();

                    var model = ProvisionalPurchaseOrder.POItemsDetail.ToList();

                    foreach (var item in model)
                    {
                        db.Entry(item).Reference(x => x.Drug).Load();
                    }

                    return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_ProvisionalPurchaseList.cshtml", model);
                }

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchItemList(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => p.Name.Contains(name)).Select(p => new { p.Name, p.Id }).ToList();
                return Json(data);
            }
            return Json("");
        }

        public ActionResult ProvisionalPOReport()
        {
            if (Session["ProvisionalPurchaseOrderId"] != null)
            {
                int id = (int)Session["ProvisionalPurchaseOrderId"];

                var data = db.ProvisionalPurchaseOrder.Where(p => p.Id == id)
                                                .Include(x => x.POItemsDetail)
                                                .Include(y => y.Suppliers)
                                                .FirstOrDefault();

                ProvisionalPO provisionalPO = new ProvisionalPO();

                foreach (var item in data.POItemsDetail)
                {
                    db.Entry(item).Reference(p => p.Drug).Load();

                    provisionalPO.ProvisionalPoList.AddProvisionalPoListRow(
                        DrugName: item.Drug.Name,
                        Quantity: item.Quantity.ToString(),
                        DrugId: item.DrugId.ToString()
                        );
                }

                string name = data.Suppliers.Select(p => p.Supplier_Name).Take(1).FirstOrDefault();
                string date = data.Date.Date.ToString();

                string nameOfFile = "ProvisionalPo" + name + DateTime.Now.ToString() + ".pdf";

                provisionalPO.ProvisionalOrder.AddProvisionalOrderRow(

                    SupplierName: name,
                    Date: date
                    );

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Areas/Procurement/Reports/ProvisionalPO/ProvisionalReport.rpt")));
                rd.SetDataSource(provisionalPO);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(
                    CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", nameOfFile);
            }

            return View("AddProvisionalPo");
        }

        public ActionResult AcceptProvisionalPo(string Id)
        {
            //diasble lazy loading to prevent 
            db.Configuration.LazyLoadingEnabled = false;

            var model = db.ProvisionalPurchaseOrder.OrderByDescending(p => p.Id).Select(p => p.Id).ToList();


            if (Id != null)
            {
                int id = Convert.ToInt32(Id);

                Session["Provisional_POId"] = id;

                var provisionalPurchasOrder = db.ProvisionalPurchaseOrder
                                                    .Where(p => p.Id == id)
                                                    .Include(x => x.Suppliers)
                                                    .Include(y => y.POItemsDetail)
                                                    .FirstOrDefault();

                provisionalPurchasOrder.Suppliers.Select(x => new { x.Supplier_Code, x.Supplier_Name }).ToList();

                foreach (var item in provisionalPurchasOrder.POItemsDetail)
                {
                    db.Entry(item).Reference(x => x.Drug).Load();
                }

                provisionalPurchasOrder.POItemsDetail.Select(p => new { p.Drug.Name, p.Id, p.Quantity }).ToList();



                return Json(provisionalPurchasOrder, JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AcceptProvisionalPo(SaveAcceptProvisionlPO saveAcceptProvisionlPO)
        {
            if (saveAcceptProvisionlPO != null)
            {
                if (Session["Provisional_POId"] != null)
                {
                    int id = (int)Session["Provisional_POId"];

                    if (id == saveAcceptProvisionlPO.ProvisionalNumber)
                    {
                        var ProPO = db.ProvisionalPurchaseOrder
                                        .Include(p => p.POItemsDetail)
                                        .Where(p => p.Id == id)
                                        .FirstOrDefault();

                        var poItem = ProPO.POItemsDetail.Where(x => x.DrugId == saveAcceptProvisionlPO.POItemDetailsNumber).FirstOrDefault();
                        poItem.CostPerUnit = saveAcceptProvisionlPO.CostPerUnit;
                        poItem.TotalCost = saveAcceptProvisionlPO.TotalCost;
                        db.SaveChanges();


                        var data = db.ProvisionalPurchaseOrder
                                            .Include(p => p.POItemsDetail)
                                            .Where(p => p.Id == id)
                                            .FirstOrDefault();

                        foreach (var item in data.POItemsDetail)
                        {
                            db.Entry(item).Reference(p => p.Drug).Load();
                        }

                        return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_AcceptProvisionalPOLst.cshtml", data);

                    }
                }
            }
            return View();
        }

        public ActionResult CompleteProvisionalPo()
        {
            int id = Convert.ToInt32(Session["Provisional_POId"]);

            Session["Provisional_POId"] = null;

            //TODO: Save Provisional Po Data


            ProvisionalPurchaseOrder provisionalPurchaseOrder = new ProvisionalPurchaseOrder()
            {
                POItemsDetail = new List<ProvisionalPOItemsDetail>()
            };

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_AcceptProvisionalPOLst.cshtml", provisionalPurchaseOrder);
        }

        public ActionResult CompareAcceptedProvisionalPO(string Id)
        {
            if (Id == null)
            {
                var model = db.ProvisionalPurchaseOrder.OrderByDescending(p => p.Id).Select(p => p.Id).ToList();

                return View(model);
            }

            int ID = Convert.ToInt32(Id);

            var data = db.ProvisionalPurchaseOrder
                                .Include(p => p.POItemsDetail)
                                .Include(p => p.Suppliers)
                                .Where(p => p.Id == ID)
                                .FirstOrDefault();

            foreach (var item in data.POItemsDetail)
            {
                db.Entry(item).Reference(x => x.Drug).Load();
            }

            CompareProvisionalOrder compareProvisionalOrder = new CompareProvisionalOrder()
            {
                provisionalPOItemsDetail = data.POItemsDetail,
                Supplier = data.Suppliers.FirstOrDefault().Supplier_Name.ToString()
            };

            Session["CompareAcceptedProvisionalPO"] = ID;
            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_CompareAcceptedProvisionalPO.cshtml", compareProvisionalOrder);
        }

        public ActionResult CompareAcceptedProvisionalPOReport()
        {
            int? Id = (int)Session["CompareAcceptedProvisionalPO"];


            if (Id != null)
            {
                int ID = Convert.ToInt32(Id);

                var data = db.ProvisionalPurchaseOrder
                                    .Include(p => p.POItemsDetail)
                                    .Include(p => p.Suppliers)
                                    .Where(p => p.Id == ID)
                                    .FirstOrDefault();

                foreach (var item in data.POItemsDetail)
                {
                    db.Entry(item).Reference(x => x.Drug).Load();
                }

                string supplier = data.Suppliers.FirstOrDefault().Supplier_Name.ToString();

                CompareProvisionalPO datasource = new CompareProvisionalPO();
                datasource.Supplier.AddSupplierRow(supplier);

                foreach (var item in data.POItemsDetail)
                {
                    datasource.POItems.AddPOItemsRow(
                        item.Drug.Name,
                        item.Quantity.ToString(),
                        "",
                        item.CostPerUnit.ToString(),
                        item.TotalCost.ToString()
                        );
                }

                string nameOfFile = supplier + DateTime.Now + " .xls";

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Areas/Procurement/Reports/CompareProvisionalPO/ReportCompareProvisionalPO.rpt")));
                rd.SetDataSource(datasource);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(
                    CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", nameOfFile);

            }

            return View("CompareAcceptedProvisionalPO");
        }

        public ActionResult ApproveProvisionalPO()
        {
            var data = db.ProvisionalPurchaseOrder.OrderByDescending(p => p.Id)
                                                .Select(p => p.Id).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult ApproveProvisionalPO(ApproveProvisionalPOData model)
        {
            int id = Convert.ToInt32(model.id);
            var provisionalPO = db.ProvisionalPurchaseOrder.Include(p => p.POItemsDetail)
                                                           .Include(p => p.Suppliers)
                                                           .Where(p => p.Id == id)
                                                           .FirstOrDefault();
            //load names of drusgs
            foreach (var item in provisionalPO.POItemsDetail)
            {
                db.Entry(item).Reference(x => x.Drug).Load();
            }

            var POitems = provisionalPO.POItemsDetail.ToList();

            //add the quantities to get the total number
            int quantity = 0;
            foreach (var item in POitems)
            {
                quantity = quantity + item.Quantity;
            }

            //add the total amount per each po item
            double totalQuantity = 0;
            foreach (var item in POitems)
            {
                totalQuantity += item.TotalCost;
            }

            ReturnApproveProvisionalPO returnApproveProvisionalPO = new ReturnApproveProvisionalPO()
            {
                ProvisionalPOId = provisionalPO.Id,
                supplierName = provisionalPO.Suppliers.FirstOrDefault().Supplier_Name,
                TotalItems = provisionalPO.POItemsDetail.Count(),
                TotalQuantity = quantity,
                TotalAmount = totalQuantity

            };

            return PartialView("~/Areas/Procurement/Views/Shared/_ApprovedProvisionalPo.cshtml", returnApproveProvisionalPO);

        }

        public ActionResult Edit(int Id)
        {
            Session["InvoiceNumber"] = null;
            //Session["InvoiceNumber"] = Id;
            return RedirectToAction("AddNewInvoice", "ProcurementPurchase", new { InvoiceNo = Id });
        }

        public ActionResult CreatePurchaseOrder()
        {
            return View();
        }

        public ActionResult SupplierWiseInvoiceReport()
        {
            return View();
        }

        public ActionResult PurchaseWiseInvoiceReport()
        {
            return View();
        }

        public ActionResult GenerateInvoiceNumber()
        {
            var todaysDate = DateTime.Now;
            var data = db.Invoice.Where(x => x.InvoiceDate.Month == todaysDate.Month && x.InvoiceDate.Year == todaysDate.Year).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult GenerateInvoiceNumber(GenerateInvoiceNumber generateInvoiceNumber)
        {
            //if (generateInvoiceNumber != null)
            //{
            //    var model = db.Invoice.Where(p => p.Id == generateInvoiceNumber.Id).FirstOrDefault();

            //    if (model != null)
            //    {
            //        model.InvoiceNo = generateInvoiceNumber.invoiceNumber;
            //        db.Entry(model).State = EntityState.Modified;
            //        db.SaveChanges();

            //        return Json("");
            //    }
            //}

            return Json("");
        }

        public ActionResult StockAdjustment(DateTime? StartTime, DateTime? EndTime)
        {

            if (StartTime == null || EndTime == null)
            {
                StartTime = DateTime.Today;
                EndTime = DateTime.Today;

            }
            ViewBag.StartTime = StartTime;
            ViewBag.EndTime = EndTime;

            var stocksAdjusted = db.StockAdjusted.Where(p => p.ItemMaster.StoreName == "MS" &&
           DbFunctions.TruncateTime(p.DateAdjusted) >= DbFunctions.TruncateTime(StartTime) &&
           DbFunctions.TruncateTime(p.DateAdjusted) <= DbFunctions.TruncateTime(EndTime)).
           Include(p => p.ItemMaster).ToArray();

            //var stocksAdjusted = db.StockAdjusted.Where(p => p.ItemMaster.StoreName == "")
            //    .Include(p => p.ItemMaster).ToArray();

            return View(stocksAdjusted);
        }

        public ActionResult AdjustmentStock()
        {
            return View();
        }

        //method for getting the items in Adjustment View
        public JsonResult SearchItemListItem(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.ItemMaster.Where(p => p.ItemName.Contains(name)).Select(p => new { p.ItemName, p.Id }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //searches and returns the batch numbers to the select list in adjustment stock
        public ActionResult SearchBatchNumbers(string name)
        {
            var data = db.ItemMaster.Where(p => p.ItemName.Contains(name)).Select(p => new { p.BatchNo }).ToList();
            return Json(data);
        }

        public ActionResult ItemDetails(AdjustmentStockData stockData)
        {
            var data = db.ItemMaster.Where(p => p.ItemName == stockData.itemName && p.BatchNo == stockData.batchNo).Select(x => new { x.CurrentStock, x.CostPriceUnit, x.Id }).FirstOrDefault();

            Session["ItemDescId"] = data.Id;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateItemCost(string newCost, string type, int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var item = db.ItemMaster.Find(id);

            ViewBag.AdjustedQuantity = newCost;

            if (type == "Surplus")
            {
                var nCost = Convert.ToInt32(item.CurrentStock) + Convert.ToInt32(newCost);
                item.CurrentStock = nCost;

            }
            else
            {
                var nCost = Convert.ToInt32(item.CurrentStock) - Convert.ToInt32(newCost);
                item.CurrentStock = nCost;
            }

            db.Entry(item).State = EntityState.Modified;

            db.StockAdjusted.Add(new StockAdjusted()
            {
                ItemMasterId = id,
                DateAdjusted = DateTime.Now,
                UserId = Convert.ToInt32(Session["UserId"]),
                Quantity = Convert.ToInt32(newCost),
                Type = type,
                Department = "PR"
            });


            db.SaveChanges();

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_tblAdjustmentStock.cshtml", item);
        }


        //public ActionResult UpdateItemCost(string newCost)
        //{
        //    int Id = Convert.ToInt32(Session["ItemDescId"]);
        //    db.Configuration.LazyLoadingEnabled = false;

        //    var item = db.ItemMaster.Find(Id);

        //    ViewBag.AdjustedQuantity = newCost;

        //    var nCost = Convert.ToInt32(item.CurrentStock) + Convert.ToInt32(newCost);
        //    item.CurrentStock = nCost;
        //    db.Entry(item).State = EntityState.Modified;
        //    db.SaveChanges();

        //    return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_tblAdjustmentStock.cshtml", item);
        //}

        public ActionResult InvoicePayement(DateTime? StartDate, DateTime? EndDate, int? SupplierId,int? page)
        {
            ViewBag.Suppliers = db.supplier.ToList();

            db.Configuration.LazyLoadingEnabled = false;
            var data = db.Invoice.Include(u => u.Supplier).Where(e => e.FinalApproval).OrderByDescending(p => p.Id).Take(10);

            var _EndDate = DateTime.Now;
            var _StartDate = DateTime.Now;
            if (StartDate != null && EndDate != null)
            {
                _StartDate = (DateTime)StartDate;
                _EndDate = (DateTime)EndDate;
            }

            _EndDate = (DateTime)_EndDate.AddDays(1);
            data = data.Where(e => e.InvoiceDate >= _StartDate && e.InvoiceDate <= _EndDate);

            if (SupplierId != null && SupplierId > 0)
            {
                data = data.Where(e => e.SupplierId == SupplierId);
            }
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            return View(data.ToPagedList(pageIndex, pageSize));
        }
        public ActionResult InvoicePaymentDetails(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int ID = (int)Id;

                var invoice = db.Invoice.Where(e => e.SupplierId == Id).ToList();

                return View(invoice);
            }
        }

        public class TotalInvoicePaymentList
        {
            public List<TempInvoicePayment> tempInvoices { get; set; }
        }
        public class TempInvoicePayment
        {
            public int Id { get; set; }
            public double PayAmount { get; set; }
        }

        [HttpPost]
        public int InvoicePaymentDetails(string TotalInvoicePaymentList)
        {
            //var totalInvoicePaymentList = (TotalInvoicePaymentList)TotalInvoicePaymentList;

            if (ModelState.IsValid)
            {
                //foreach (var item in totalInvoicePaymentList.tempInvoices)
                //{
                //    var paidAmount = db.ProcurementInvoiceTransactions.Sum(e => e.TransactionAmountPaid);
                //    var totalAmount = db.InvoiceDetail.Where(f => f.InvoiceNo == item.Id).Sum(e => (e.CostPerCasePack * e.NoOfPack) + +(e.VatAmt + e.FreightCharges 
                //    + e.PackingCharges) - e.Discount);

                //    var obj = new ProcurementInvoiceTransactions()
                //    {
                //        InvoiceId = item.Id,
                //        TransactionAmountPaid = item.PayAmount,
                //        PendingBalance = totalAmount - (paidAmount + item.PayAmount),
                //        CreatedDate = DateTime.Now,
                //    };

                //    db.ProcurementInvoiceTransactions.Add(obj);
                //}


                return db.SaveChanges();

            }
            return 0;
        }

        public ActionResult GRNReturn()
        {

            return View();
        }

        public ActionResult GetGRNNOs(int SupplierID)
        {
            var Supplier = db.supplier.FirstOrDefault(e => e.Supplier_Code == SupplierID);

            var Invoices = db.Invoice.Where(e => e.Id.Equals(0));
            if (Supplier != null)
            {
                Invoices = db.Invoice.Where(e => e.SupplierName == Supplier.Supplier_Name);

            }
            return Json(Invoices, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupplierItems(int InvoiceNo)
        {
            var InvoiceDetails = db.InvoiceDetail.Where(e => e.InvoiceNo == InvoiceNo);

            return Json(InvoiceDetails, JsonRequestBehavior.AllowGet);
        }


    }
}
