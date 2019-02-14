using Caresoft2._0.Areas.PharmacyModule.ViewModel;
using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caresoft2._0.Areas.PharmacyModule.Reports.PharmacyReports;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using Microsoft.Ajax.Utilities;
using Caresoft2._0.Areas.PharmacyModule.Reports.CurrentStock;
using Caresoft2._0.Areas.PharmacyModule.Reports.ItemStockLedger;
using Caresoft2._0.Areas.PharmacyModule.Reports.CashSummary;
using Caresoft2._0.Areas.PharmacyModule.Reports.PatientSummaryReport;
using Caresoft2._0.Areas.PharmacyModule.Reports.DrugWiseReport;
using Caresoft2._0.Areas.PharmacyModule.Reports.CashRegister;
using System.Data;
using CaresoftHMISDataAccess;
using Caresoft2._0.Areas.PharmacyModule.Reports.SalesProfit;
using Caresoft2._0.Areas.PharmacyModule.Reports.DonorWiseSupplier;
using Caresoft2._0.Areas.PharmacyModule.Reports.DonorWiseIssue;
using PagedList;
using Caresoft2._0.CrystalReports.ReportHeader;
using Caresoft2._0.Areas.PharmacyModule.Reports.StockSummary;
using Caresoft2._0.CrystalReports;
using System.Data.Entity;

namespace Caresoft2._0.Areas.PharmacyModule.Controllers
{
    [Auth]
    public class PharmacyReportsController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();
        CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: PharmacyModule/PharmacyReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockPosition()
        {

            var data = db.ItemMaster.OrderBy(p => p.ItemName).Select(p=>p.ItemName).Distinct().ToList();
            ViewBag.Items = data;

            ViewBag.Drugs = db.Drug.OrderBy(p => p.Name).ToList();

            var drugs = db.Drug.ToList();

            List<PharmacyStockPosition> stockPositions = new List<PharmacyStockPosition>();

            var todaysDate = DateTime.Now.Date;

            var tomorrow = todaysDate.AddDays(1);
            foreach (var item in drugs)
            {
                var stockPosition = GetItemsById(item.Id, todaysDate, tomorrow);
                stockPositions.AddRange(stockPosition);
            }



            return View(stockPositions);
            
        }

        public ActionResult SearchStockByDatesStockPosition(Dates dates)
        {
            var drugs = db.Drug.ToList();

            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);
            tDate = tDate.AddDays(1);

            Session["fDateStockPosition"] = fDate; 
            Session["tDateStockPosition"] = tDate;

            List<PharmacyStockPosition> stockPositions = new List<PharmacyStockPosition>();

            foreach (var item in drugs)
            {
                var stockPosition = GetItemsById(item.Id, fDate, tDate);
                stockPositions.AddRange(stockPosition);
            }

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyReports/_lstStockPosition.cshtml", stockPositions);
        }

        public ActionResult DisplayWebForm()
        {
            Response.Redirect("~/Areas/PharmacyModule/Views/PharmacyReports/DefaultReport.aspx");
            return View();
        }

        public ActionResult SearchItem(string name)
        {
            var pharmacyStockPositions = GetItems(name);

            Session["ItemName"] = name;

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyReports/_lstStockPosition.cshtml", pharmacyStockPositions);
        }

        public struct StockPositions{
            public int DrugId { get; set; }
            public string fromDate { get; set; }
            public string toDate { get; set; }

        }

        public ActionResult SearchItemId(int Id)
        {
            var initialDate = DateTime.Now.AddYears(-10);
            var todaysDate = DateTime.Now.Date;
            var tomorrow = todaysDate.AddDays(1);
            var pharmacyStockPositions = GetItemsById(Id, initialDate, tomorrow);

            //session to be used together with generating the report so its important
            //Session["ItemName"] = name;

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyReports/_lstStockPosition.cshtml", pharmacyStockPositions);
        }

        public ActionResult GenerateStockPositionReport()
        {
           

            if (Session["fDateStockPosition"] != null && Session["tDateStockPosition"] != null)
            {

                var fd = (DateTime)Session["fDateStockPosition"];
                var td = (DateTime)Session["tDateStockPosition"];

                var FDate = fd;
                var TDate = td;

                var Drugs = db.Drug.ToList();

                List<PharmacyStockPosition> StockPositions = new List<PharmacyStockPosition>();

                foreach (var item in Drugs)
                {
                    var stockPosition = GetItemsById(item.Id, FDate, TDate);
                    StockPositions.AddRange(stockPosition);
                }

                //dataset that links data with the report
                StockPositionDataSet stockPositionDataSet = new StockPositionDataSet();

                foreach (var item in StockPositions)
                {
                    stockPositionDataSet._StockPositionDataSet.AddStockPositionDataSetRow(
                        item.OpeningStock,
                        item.ItemName,
                        item.TotalPurchaseQuantity,
                        item.TotalIssueQuantity,
                        item.CurrentStock,
                        item.CostPrice,
                        item.ReOrderLevel
                        );
                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/PharmacyReports/StockPosition.rpt")));
                rd.SetDataSource(stockPositionDataSet);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(
                    CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "StockPosition.pdf");

            }

            var fDate = DateTime.Now.Date;
            fDate = default;
            var tDate = DateTime.Now.Date;
            tDate = tDate.AddDays(1);

            var drugs = db.Drug.ToList();

            List<PharmacyStockPosition> stockPositions = new List<PharmacyStockPosition>();

            foreach (var item in drugs)
            {
                var stockPosition = GetItemsById(item.Id, fDate, tDate);
                stockPositions.AddRange(stockPosition);
            }

            StockPositionDataSet StockPositionDataSet = new StockPositionDataSet();

            foreach (var item in stockPositions)
            {
                StockPositionDataSet._StockPositionDataSet.AddStockPositionDataSetRow(
                    item.OpeningStock,
                    item.ItemName,
                    item.TotalPurchaseQuantity,
                    item.TotalIssueQuantity,
                    item.CurrentStock,
                    item.CostPrice,
                    item.ReOrderLevel
                    );
            }

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/PharmacyReports/StockPosition.rpt")));
            Rd.SetDataSource(StockPositionDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "StockPosition.pdf");
        }

        public ActionResult CurrentStock(int? page, string ItemName, int? ItemId, string CurrentStockFilter, bool? IsDonor)
        {
            ViewBag.ItemName = ItemName;
            ViewBag.ItemId = ItemId;
            ViewBag.CurrentStockFilter = CurrentStockFilter;
            ViewBag.IsDonor = IsDonor;

            int PageIndex = 1;
            int PageSize = 10;
            PageIndex = page.HasValue ? page.Value : 1;

            var data = db.ItemMaster.Include(x => x.Drug).Where(p => p.StoreName == "P").ToList();

            if (ItemName != null && ItemName.Length > 0)
            {
                data = data.Where(e => e.ItemName.Contains(ItemName)).ToList();
            }

            if (CurrentStockFilter!=null && !CurrentStockFilter.Equals("all"))
            {
                if (CurrentStockFilter.Equals("greater"))
                {
                    data = data.Where(e => e.CurrentStock > 0).ToList();
                }

                if (CurrentStockFilter.Equals("less"))
                {
                    data = data.Where(e => e.CurrentStock == 0).ToList();
                }

                if (CurrentStockFilter.Equals("Active"))
                {
                    data = data.Where(e => e.Drug?.IsActive ?? true).ToList();
                }

                if (CurrentStockFilter.Equals("Inactive"))
                {
                    data = data.Where(e => e.Drug != null && e.Drug.IsActive != null && !(bool)e.Drug.IsActive).ToList();
                }

            }

            if (IsDonor != null && (bool)IsDonor)
            {

            }

            return View(data.ToPagedList(PageIndex, PageSize));
        }

        public ActionResult GetCurrentStockReport(string ItemName, string CurrentStockFilter, bool? IsDonor)
        {
            var items = db.ItemMaster.Where(p => p.StoreName == "P").ToList();

            if (ItemName != null && ItemName.Length > 0)
            {
                items = items.Where(e => e.ItemName.Contains(ItemName)).ToList();
            }

            if (CurrentStockFilter != null && !CurrentStockFilter.Equals("all"))
            {
                if (CurrentStockFilter.Equals("greater"))
                {
                    items = items.Where(e => e.CurrentStock > 0).ToList();
                }

                if (CurrentStockFilter.Equals("less"))
                {
                    items = items.Where(e => e.CurrentStock == 0).ToList();
                }


            }

            if (IsDonor != null && (bool)IsDonor)
            {

            }

            DataSetCurrentStock dataSet = new DataSetCurrentStock();

            if (items.Count() > 0)
            {
                foreach (var item in items.OrderBy(E => E.ItemName))
                {
                    var date = (item.ExpiryDate ?? DateTime.Now.Date.AddYears(1));

                    dataSet.ItemMasterItem.AddItemMasterItemRow(
                        item.BatchNo.ToUpper(),
                        item.ItemName.ToUpper(),
                        item.CurrentStock,
                        //DateTime.Now,
                        date,
                        item.ReorderLevel,
                        Convert.ToDecimal(item.CostPriceUnit)
                        );
                }
            }

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/CurrentStock/RptCurrentStock.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "PCurrentStock " + DateTime.Now.ToString() + ".pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult ItemStockPosition()
        {
            var drugs = db.Drug.ToList();
          
            List<PharmacyStockPosition> stockPositions = new List<PharmacyStockPosition>();
            var todaysDate = DateTime.Now.Date;

            var tomorrow = todaysDate.AddDays(1);
            foreach (var item in drugs)
            {
                var stockPosition = GetItemsById(item.Id,todaysDate, tomorrow);
                stockPositions.AddRange(stockPosition);
            }

            return View(stockPositions);
        }

        public struct Dates
        {
            public string fromDate { get; set; }
            public string toDate { get; set; }
        }

        public ActionResult SearchStockByDatesItemStockPosition(Dates dates)
        {
            var drugs = db.Drug.ToList();

            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);
            tDate = tDate.AddDays(1);

            List<PharmacyStockPosition> stockPositions = new List<PharmacyStockPosition>();

            foreach (var item in drugs)
            {
                var stockPosition = GetItemsById(item.Id, fDate, tDate);
                stockPositions.AddRange(stockPosition);
            }

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyReports/_lstItemStockLedger.cshtml", stockPositions);
        }

        public ActionResult GetItemStockLedgerReport()
        {
            
              var drugs = db.Drug.ToList();
          
            List<PharmacyStockPosition> stockPositions = new List<PharmacyStockPosition>();
            var todaysDate = DateTime.Now.Date;

            var tomorrow = todaysDate.AddDays(1);
            foreach (var item in drugs)
            {
                var stockPosition = GetItemsById(item.Id,todaysDate, tomorrow);
                stockPositions.AddRange(stockPosition);
            }



            DataSetItemStokLedger dataSetItemStokLedger = new DataSetItemStokLedger();

            foreach (var item in stockPositions)
            {
                dataSetItemStokLedger.ItemStockLedger.AddItemStockLedgerRow(
                    item.OpeningStock,
                    item.ClosingStock,
                    item.Date.Value,
                    item.ItemName,
                    item.BatchNumber,
                    item.TotalPurchaseQuantity,
                    item.TotalIssueQuantity);
                    
            }


            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/ItemStockLedger/RptItemStockLedger.rpt")));
            Rd.SetDataSource(dataSetItemStokLedger);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "ItemStockLedger.pdf");
        }

        public ActionResult CashSummaryReport()
        {
            return View();
        }

        public ActionResult GetCashReportSummary()
        {
            Reports.CashSummary.CashSummaryDataSet cashSummaryReportDataSet = new Reports.CashSummary.CashSummaryDataSet();

          
            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/CashSummary/CashSummaryReport.rpt")));
            Rd.SetDataSource(cashSummaryReportDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "CashSummary.pdf");
        }

        public ActionResult PatientReportSummary()
        {
            var insuranceCompanies = db2.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim().Contains("insurance") || p.CompanyName.Contains("cash")).ToList();

            return View(insuranceCompanies);
        }

        public ActionResult GetPatientReportSummary(DateTime FromDate, DateTime ToDate, int? OpdNo,int? insuaranceCompany)
        {
            PatientSummaryDataSet ReportDataSet = new PatientSummaryDataSet();

            ReportDataSet = GetPatientSummaryData(FromDate,ToDate,OpdNo,insuaranceCompany);

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/PatientSummaryReport/RptPatientSummaryReport.rpt")));
            Rd.SetDataSource(ReportDataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "PatientSummary.pdf");
        }

        public PatientSummaryDataSet GetPatientSummaryData(DateTime FromDate, DateTime ToDate, int? OpdNo,int? insuaranceCompany)
        {
            PatientSummaryDataSet dataSet = new PatientSummaryDataSet();

            var data = new List<Medication>();
            var walkInData = new List<Walking>();

            if (OpdNo.HasValue && insuaranceCompany==0)
            {
                data = db2.Medications.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate && p.OPDNo == OpdNo.Value)
                                        .OrderByDescending(p => p.Id)
                                        .ToList();
            }
            else if (OpdNo.HasValue && insuaranceCompany > 0)
            {
                data = db2.Medications.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate && p.OPDNo == OpdNo.Value)
                                      .Where(p=>p.OpdRegister.Tariff.CompanyId==insuaranceCompany)
                                      .OrderByDescending(p => p.Id)
                                      .ToList();
            }
            else if(OpdNo.HasValue==false && insuaranceCompany > 0)
            {
                data = db2.Medications.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate)
                                      .Where(p=>p.OpdRegister.Tariff.CompanyId==insuaranceCompany) 
                                      .OrderByDescending(p => p.Id)
                                      .ToList();

                var companySelected = db2.Companies.Where(p => p.Id==insuaranceCompany).FirstOrDefault();

                if(companySelected!=null && companySelected.CompanyName.Trim().ToLower().Contains("cash"))
                {
                    walkInData = db.Walkings.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate).ToList();

                }


            }
            else if(OpdNo.HasValue == false && insuaranceCompany == 0)
            {
                data = db2.Medications.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate)
                                        .OrderByDescending(p => p.Id)
                                        .ToList();
                walkInData = db.Walkings.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate).ToList();

            }



            foreach (var item in data)
            {
                if ((item.IsWalkIn == false || item.IsWalkIn == null) && item.Available==true)
                {
                    var itemMaster = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();
                    var pat = db2.OpdRegisters.Where(p => p.Id == item.OPDNo).FirstOrDefault();
                    var patientDetails = pat.Patient;

                    var totalBillAmount = item.UnitPrice * item.QuantityIssued.Value;
                    var awardAmount = item.Award ?? 0;
                    var paidAmount = 0.0;
                    if (item.Paid == true)
                    {
                        paidAmount = (item.QuantityIssued * item.UnitPrice) - (awardAmount + (item.WaivedAmount ?? 0) + (item.Discount ?? 0)) ?? 0;
                        //paidAmount = item.BillPayment?.PaidAmount ?? 0;
                    }
                    var dueAmount = totalBillAmount - (paidAmount + awardAmount);

                    dataSet.PatientSummary.AddPatientSummaryRow(
                                patientDetails.RegNumber,
                                pat.Id.ToString(),
                                patientDetails.FName + " " + patientDetails.LName,
                                item.TimeAdded,
                                itemMaster.BatchNo,
                                itemMaster.ItemName,
                                item.QuantityIssued.Value,
                                item.UnitPrice,
                                item.UnitPrice * item.QuantityIssued.Value,//total bill per item
                                0,//refund amount
                                item?.Discount ?? 0, //total discount per item
                                dueAmount, //due amount per item
                                awardAmount, // award amount per item
                                paidAmount //Total amount in hand
                                );
                }
                else
                {
                    //var itemMaster = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();
                    //var pat = db2.OpdRegisters.Where(p => p.Id == item.OPDNo).FirstOrDefault();
                    //var patientDetails = db.Walkings.Where(p => p.Id == item.WalkInId).FirstOrDefault();


                    
                    //dataSet.PatientSummary.AddPatientSummaryRow(
                    //            patientDetails.PatientIdentifierId.ToString(),
                    //            pat.Id.ToString(),
                    //            patientDetails.PatientsName,
                    //            item.TimeAdded,
                    //            itemMaster.BatchNo,
                    //            itemMaster.ItemName,
                    //            item.QuantityIssued.Value,
                    //            item.UnitPrice,
                    //            item.UnitPrice * item.QuantityIssued.Value,//total bill per item
                    //            0,//refund amount
                    //            item?.Discount ?? 0, //total discount per item
                    //            0, //due amount per item
                    //            0, // award amount per item
                    //            item.Subtotal //Total amount in hand
                    //            );


                }
            }

           
                foreach (var item in walkInData)
                {
                    var itemMaster = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();
                    
                    var patientDetails = item.PatientsName;

                    var totalBillAmount = item.UnitPrice * item.QuantityIssued;
                    var awardAmount = 0.0;
                    var paidAmount = 0.0;
                    if (item.Paid == true)
                    {
                        paidAmount = (item.QuantityIssued * item.UnitPrice) - (awardAmount);
                      
                    }
                    var dueAmount = totalBillAmount - (paidAmount + awardAmount);

                    dataSet.PatientSummary.AddPatientSummaryRow(
                                item.PatientIdentifierId.ToString(),
                                item.PatientIdentifierId.ToString(),
                                patientDetails,
                                item.TimeAdded,
                                itemMaster.BatchNo,
                                itemMaster.ItemName,
                                item.QuantityIssued,
                                item.UnitPrice,
                                item.UnitPrice * item.QuantityIssued,//total bill per item
                                0,//refund amount
                                0, //total discount per item
                                dueAmount, //due amount per item
                                awardAmount, // award amount per item
                                paidAmount //Total amount in hand
                                );
                }

            
            return dataSet;
        }

        public ActionResult DrugWiseReport()
        {
            var insuranceCompanies = db2.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim().Contains("insurance") || p.CompanyName.Contains("cash")).ToList();
            
            return View(insuranceCompanies);
        }

        public ActionResult DrugWisePartialView(DateTime FromDate, DateTime ToDate, int? DrugId, int? insuaranceCompany)
        {
            List<DrugWiseViewModel> LstDrugWiseViewModel = new List<DrugWiseViewModel>(); 
            var dataSet = GetDrugTWiseReportData(FromDate, ToDate, DrugId, insuaranceCompany);
            foreach (var item in dataSet.DrugWiseReport.ToList())
            {
                DrugWiseViewModel viewModel = new DrugWiseViewModel()
                {
                    DrugName = item.DrugName,
                    BatchNo = item.BatchNo,
                    VoucherNo = item.VoucherNo,
                    VoucherDate = item.VoucherDate,
                    IssueTo = item.IssueTo,
                    Quantity = item.Quantity,
                    CurrentStock= item.CurrentStock,
                    CostPrice = item.CostPrice,
                    SellingPrice = item.Selling,
                    ProfitAmount = item.ProfitAmount,
                    Profit = item.Profit
                };
                LstDrugWiseViewModel.Add(viewModel);
            }

            //LstDrugWiseViewModel = LstDrugWiseViewModel.OrderBy(p => p.VoucherDate).ToList();
            LstDrugWiseViewModel = LstDrugWiseViewModel.OrderBy(p => p.DrugName).ToList();


            return PartialView("~/Areas/PharmacyModule/Views/PharmacyReports/_DrugWiseReport.cshtml",LstDrugWiseViewModel);
        }

        private DrugWiseReportDataSet GetDrugTWiseReportData(DateTime FromDate, DateTime ToDate, int? DrugId, int? insuaranceCompany)
        {
            DrugWiseReportDataSet drugWiseReportDataSet = new DrugWiseReportDataSet();

            var data = new List<DrugTransactions>();
            
            if (DrugId.HasValue && insuaranceCompany.Value == 0)
            {
                var DrugItems = db.ItemMaster.Where(p => p.DrugId == DrugId.Value && p.StoreName=="P").ToList();
                foreach (var item in DrugItems)
                {
                    var dt = db.DrugTransactions
                             .Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate)
                             .Where(x => x.ItemMasterId == item.Id)
                             .ToList();

                    data.AddRange(dt);
                }
                foreach (var item in data)
                {
                    if (item.IsWalkIn == true)
                    {
                        var walkInData = db.Walkings.Where(x => x.Id == item.PatientId).FirstOrDefault();
                        var drug = db.ItemMaster.Find(walkInData.DrugId);
                        if (drug != null)
                        {
                            var CurrentStockForThatDay = 0;
                            if (item.PatientId.HasValue)
                            {
                                CurrentStockForThatDay = GetClosingStockOfSomeDrug(drug.Id, item.PatientId.Value, drug.CurrentStock, FromDate, ToDate);
                            }
                            else
                            {
                                CurrentStockForThatDay = GetClosingStockOfSomeDrugForDepartment(drug.Id, item.DepartmentId.Value, drug.CurrentStock, FromDate, ToDate);
                            }
                            
                            var patientsName = walkInData.PatientsName + " - #Walk in";

                            var profit = 0.0;

                            if (drug.CostPriceUnit != 0)
                            {
                                profit = ((walkInData.UnitPrice - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                            }

                            if(patientsName==null || patientsName == "")
                            {
                                if (item.DepartmentId != null)
                                {
                                    patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                    patientsName = patientsName + "-#Department";
                                }
                            }

                            drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                    drug.ItemName??"",
                                    drug.BatchNo??"",
                                    walkInData.PatientIdentifierId.ToString()??"",
                                    walkInData.TimeAdded.ToString("dd-MM-yyyy")??"",
                                    patientsName??"",
                                    item.QuantityIssued,
                                    CurrentStockForThatDay,
                                    drug.CostPriceUnit,
                                    walkInData.UnitPrice,
                                    (item.QuantityIssued * walkInData.UnitPrice) - (item.QuantityIssued * drug.CostPriceUnit),
                                    profit
                                );
                        }
                    }
                    else
                    {
                        var drug = db.ItemMaster.Find(item.ItemMasterId);

                        if (drug != null)
                        {
                            var CurrentStockForThatDay = 0;
                            if (item.PatientId.HasValue)
                            {
                                CurrentStockForThatDay = GetClosingStockOfSomeDrug(drug.Id, item.PatientId.Value, drug.CurrentStock, FromDate, ToDate);
                            }
                            else
                            {
                                CurrentStockForThatDay = GetClosingStockOfSomeDrugForDepartment(drug.Id, item.DepartmentId.Value, drug.CurrentStock, FromDate, ToDate);
                            }

                            var medication = db2.Medications.Where(p => p.OPDNo == item.PatientId)?.FirstOrDefault();

                            if (medication != null)
                            {
                                var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                                var profit = 0.0;

                                if (drug.CostPriceUnit != 0)
                                {
                                    profit = ((medication.UnitPrice - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                                }

                                if (patientsName == null || patientsName == "")
                                {
                                    if (item.DepartmentId != null)
                                    {
                                        patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                        patientsName = patientsName + "-Department";
                                    }
                                }

                                drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                        drug.ItemName??"",
                                        drug.BatchNo??"",
                                        medication.BillNo.ToString()??"",
                                        item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                        patientsName??"",
                                        item.QuantityIssued,
                                        CurrentStockForThatDay,
                                        drug.CostPriceUnit,
                                        medication.UnitPrice,
                                        (item.QuantityIssued * medication.UnitPrice) - (item.QuantityIssued * drug.CostPriceUnit),
                                        profit
                                    );
                            }
                            else
                            {

                                var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                                var profit = 0.0;

                                if (drug.CostPriceUnit != 0)
                                {
                                    profit = ((item.Rate - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                                }

                                if (patientsName == null || patientsName == "")
                                {
                                    if (item.DepartmentId != null)
                                    {
                                        patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                        patientsName = patientsName + " -Department";
                                    }
                                }

                                drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                        drug.ItemName??"",
                                        drug.BatchNo??"",
                                        "",
                                        item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                        patientsName??"",
                                        item.QuantityIssued,
                                        CurrentStockForThatDay,
                                        drug.CostPriceUnit,
                                        item.Rate,
                                        (item.QuantityIssued * item.Rate) - (item.QuantityIssued * drug.CostPriceUnit),
                                        profit
                                    );
                            }
                        }
                    }
                }

            }
            else if (DrugId.HasValue && insuaranceCompany.Value > 0)
            {
                var DrugItems = db.ItemMaster.Where(p => p.DrugId == DrugId.Value && p.StoreName == "P").ToList();
                var dt2 = new List<DrugTransactions>();

                //we get all drugs with that id irrespective of the companies
                foreach (var item in DrugItems)
                {
                    var dt = db.DrugTransactions
                             .Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate)
                             .Where(x => x.ItemMasterId == item.Id)
                             .ToList();

                    dt2.AddRange(dt);
                }

                //now we sort the drugs based on the insurance selected
                var dt3 = new List<DrugTransactions>();

                //we get all drugs with that id irrespective of the companies
                foreach (var item in dt2)
                {
                    var patientInOpd = db2.OpdRegisters
                                            .Where(p => p.Id == item.PatientId && p.Tariff.Company.Id == insuaranceCompany.Value)
                                            .FirstOrDefault();
                    if (patientInOpd != null)
                    {
                        dt3.Add(item);
                    }

                }

                data = dt3;

                foreach (var item in data)
                {
                    var drug = db.ItemMaster.Find(item.ItemMasterId);
                    if (drug != null)
                    {
                        var CurrentStockForThatDay = 0;

                        if (item.PatientId.HasValue)
                        {
                            CurrentStockForThatDay = GetClosingStockOfSomeDrug(drug.Id, item.PatientId.Value, drug.CurrentStock, FromDate, ToDate);
                        }
                        else
                        {
                            CurrentStockForThatDay = GetClosingStockOfSomeDrugForDepartment(drug.Id, item.DepartmentId.Value, drug.CurrentStock, FromDate, ToDate);
                        }
                        var medication = db2.Medications.Where(p => p.OPDNo == item.PatientId)?.FirstOrDefault();

                        if (medication != null)
                        {
                            var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                            var profit = 0.0;

                            if (drug.CostPriceUnit != 0)
                            {
                                profit = ((medication.UnitPrice - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                            }

                            if (patientsName == null || patientsName == "")
                            {
                                if (item.DepartmentId != null)
                                {
                                    patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                    patientsName = patientsName + "-Department";
                                }
                            }

                            drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                    drug.ItemName??"",
                                    drug.BatchNo??"",
                                    medication.BillNo.ToString()??"",
                                    item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                    patientsName??"",
                                    item.QuantityIssued,
                                    CurrentStockForThatDay,
                                    drug.CostPriceUnit,
                                    medication.UnitPrice,
                                    (item.QuantityIssued * medication.UnitPrice) - (item.QuantityIssued * drug.CostPriceUnit),
                                    profit
                                );
                        }
                        else
                        {

                            var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                            var profit = 0.0;

                            if (drug.CostPriceUnit != 0)
                            {
                                profit = ((item.Rate - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                            }

                            if (patientsName == null || patientsName == "")
                            {
                                if (item.DepartmentId != null)
                                {
                                    patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                    patientsName = patientsName + "-Department";
                                }
                            }

                            drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                    drug.ItemName??"",
                                    drug.BatchNo??"",
                                    "",
                                    item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                    patientsName??"",
                                    item.QuantityIssued,
                                    CurrentStockForThatDay,
                                    drug.CostPriceUnit,
                                    item.Rate,
                                    (item.QuantityIssued * item.Rate) - (item.QuantityIssued * drug.CostPriceUnit),
                                    profit
                                );
                        }
                    }
                }
            }
            else if (DrugId.HasValue == false && insuaranceCompany.Value > 0)
            {
                //select all drugs irrespective of drug id
                var dt2 = db.DrugTransactions.Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate).ToList();
                var dt3 = new List<DrugTransactions>();

                //we get only drugs that were issued to the insurance patientsof that specific company
                foreach (var item in dt2)
                {
                    var patientInOpd = db2.OpdRegisters
                                            .Where(p => p.Id == item.PatientId && p.Tariff.Company.Id == insuaranceCompany.Value)
                                            .FirstOrDefault();
                    if (patientInOpd != null)
                    {
                        dt3.Add(item);
                    }

                }

                data = dt3;

                foreach (var item in data)
                {
                    var drug = db.ItemMaster.Where(x=>x.Id == item.ItemMasterId && x.StoreName == "P").FirstOrDefault();
                    if (drug != null)
                    {
                        var CurrentStockForThatDay = 0;

                        if (item.PatientId.HasValue)
                        {
                            CurrentStockForThatDay = GetClosingStockOfSomeDrug(drug.Id, item.PatientId.Value, drug.CurrentStock, FromDate, ToDate);
                        }
                        else
                        {
                            CurrentStockForThatDay = GetClosingStockOfSomeDrugForDepartment(drug.Id, item.DepartmentId.Value, drug.CurrentStock, FromDate, ToDate);
                        }

                        var medication = db2.Medications.Where(p => p.OPDNo == item.PatientId)?.FirstOrDefault();

                        if (medication != null)
                        {
                            var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                            var profit = 0.0;

                            if (drug.CostPriceUnit != 0)
                            {
                                profit = ((medication.UnitPrice - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                            }

                            if (patientsName == null || patientsName == "")
                            {
                                if (item.DepartmentId != null)
                                {
                                    patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                    patientsName = patientsName + "-Department";
                                }
                            }

                            drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                    drug.ItemName??"",
                                    drug.BatchNo??"",
                                    medication.BillNo.ToString()??"",
                                    item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                    patientsName??"",
                                    item.QuantityIssued,
                                    CurrentStockForThatDay,
                                    drug.CostPriceUnit,
                                    medication.UnitPrice,
                                    (item.QuantityIssued * medication.UnitPrice) - (item.QuantityIssued * drug.CostPriceUnit),
                                    profit
                                );
                        }
                        else
                        {

                            var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();



                            var profit = 0.0;

                            if (drug.CostPriceUnit != 0)
                            {
                                profit = ((item.Rate - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                            }

                            if (patientsName == null || patientsName == "")
                            {
                                if (item.DepartmentId != null)
                                {
                                    patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                    patientsName = patientsName + "-Department";
                                }
                            }

                            drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                    drug.ItemName??"",
                                    drug.BatchNo??"",
                                    "",
                                    item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                    patientsName??"",
                                    item.QuantityIssued,
                                    CurrentStockForThatDay,
                                    drug.CostPriceUnit,
                                    item.Rate,
                                    (item.QuantityIssued * item.Rate) - (item.QuantityIssued * drug.CostPriceUnit),
                                    profit
                                );
                        }
                    }
                 
                }
            }
            else if (DrugId.HasValue == false && insuaranceCompany.Value == 0)
            {
                data = db.DrugTransactions.Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate).ToList();

                foreach (var item in data)
                {
                    if (item.IsWalkIn == true)
                    {
                        var walkInData = db.Walkings.Where(x => x.Id == item.PatientId).FirstOrDefault();

                        if (walkInData != null)
                        {
                            var drug = db.ItemMaster.Where(p=>p.Id == walkInData.DrugId &&p.StoreName=="P").FirstOrDefault();
                            if (drug != null)
                            {
                                var CurrentStockForThatDay = 0;

                                if (item.PatientId.HasValue)
                                {
                                    CurrentStockForThatDay = GetClosingStockOfSomeDrug(drug.Id, item.PatientId.Value, drug.CurrentStock, FromDate, ToDate);
                                }
                                else
                                {
                                    CurrentStockForThatDay = GetClosingStockOfSomeDrugForDepartment(drug.Id, item.DepartmentId.Value, drug.CurrentStock, FromDate, ToDate);
                                }

                                var patientsName = walkInData.PatientsName + " #Walk in";

                                var profit = 0.0;

                                if (drug.CostPriceUnit != 0)
                                {
                                    profit = ((walkInData.UnitPrice - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                                }

                                if (patientsName == null || patientsName == "")
                                {
                                    if (item.DepartmentId != null)
                                    {
                                        patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                        patientsName = patientsName + "-Department";
                                    }
                                }

                                drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                        drug.ItemName??"",
                                        drug.BatchNo??"",
                                        walkInData.PatientIdentifierId.ToString()??"",
                                        walkInData.TimeAdded.ToString("dd-MM-yyyy")??"",
                                        patientsName??"",
                                        item.QuantityIssued,
                                        CurrentStockForThatDay,
                                        drug.CostPriceUnit,
                                        walkInData.UnitPrice,
                                        (item.QuantityIssued * walkInData.UnitPrice) - (item.QuantityIssued * drug.CostPriceUnit),
                                        profit
                                    );
                            }
                        }
                    }
                    else
                    {
                        var drug = db.ItemMaster.Where(p => p.Id == item.ItemMasterId && p.StoreName == "P").FirstOrDefault();


                        if (drug != null)
                        {
                            var CurrentStockForThatDay = 0;

                            if (item.PatientId.HasValue)
                            {
                                CurrentStockForThatDay = GetClosingStockOfSomeDrug(drug.Id, item.PatientId.Value, drug.CurrentStock, FromDate, ToDate);
                            }
                            else
                            {
                                CurrentStockForThatDay = GetClosingStockOfSomeDrugForDepartment(drug.Id, item.DepartmentId.Value, drug.CurrentStock, FromDate, ToDate);
                            }

                            var medication = db2.Medications.Where(p => p.OPDNo == item.PatientId)?.FirstOrDefault();

                            if (medication != null)
                            {
                                var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                                var profit = 0.0;

                                if (drug.CostPriceUnit != 0)
                                {
                                    profit = ((medication.UnitPrice - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                                }

                                if (patientsName == null || patientsName == "")
                                {
                                    if (item.DepartmentId != null)
                                    {
                                        patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                        patientsName = patientsName + "-Department";
                                    }
                                }

                                drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                        drug.ItemName??"",
                                        drug.BatchNo??"",
                                        medication.BillNo.ToString()??"",
                                        item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                        patientsName??"",
                                        item.QuantityIssued,
                                        CurrentStockForThatDay,
                                        drug.CostPriceUnit,
                                        medication.UnitPrice,
                                        (item.QuantityIssued * medication.UnitPrice) - (item.QuantityIssued * drug.CostPriceUnit),
                                        profit
                                    );
                            }
                            else
                            {

                                var patientsName = db2.OpdRegisters.Where(p => p.Id == item.PatientId).Select(x => x.Patient.FName + " " + x.Patient.LName)?.FirstOrDefault();

                                var profit = 0.0;

                                if (drug.CostPriceUnit != 0)
                                {
                                    profit = ((item.Rate - drug.CostPriceUnit) / drug.CostPriceUnit) * 100;
                                }

                                if (patientsName == null || patientsName == "")
                                {
                                    if (item.DepartmentId != null)
                                    {
                                        patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                        patientsName = patientsName + "-Department";
                                    }
                                }

                                drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                        drug.ItemName??"",
                                        drug.BatchNo??"",
                                        "",
                                        item.TransactionDate.ToString("dd-MM-yyyy")??"",
                                        patientsName??"",
                                        item.QuantityIssued,
                                        CurrentStockForThatDay,
                                        drug.CostPriceUnit,
                                        item.Rate,
                                        (item.QuantityIssued * item.Rate) - (item.QuantityIssued * drug.CostPriceUnit),
                                        profit
                                    );
                            }
                        }
                    }
                }
            }

            DrugWiseReportDataSet dataSet2 = new DrugWiseReportDataSet();

            foreach (var item in drugWiseReportDataSet.DrugWiseReport.OrderByDescending(p=>p.CurrentStock).ToList())
            {

                dataSet2.DrugWiseReport.AddDrugWiseReportRow(
                    item.DrugName ?? "",
                    item.BatchNo ?? "",
                    item.VoucherNo ?? "",
                    item.VoucherDate ?? DateTime.Now.ToString("dd-MM-yyyy"),
                    item.IssueTo ?? "",
                    item.Quantity,
                    item.CurrentStock,
                    item.CostPrice,
                    item.Selling,
                    item.ProfitAmount,
                    item.Profit);
                
            }

            return dataSet2;
        }

        public ActionResult GetDrugWiseReport(DateTime FromDate, DateTime ToDate, int? DrugId , int? insuaranceCompany )
        {

            DrugWiseReportDataSet drugWiseReportDataSet = new DrugWiseReportDataSet();

            drugWiseReportDataSet = GetDrugTWiseReportData(FromDate, ToDate, DrugId, insuaranceCompany);

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/DrugWiseReport/DrugWiseReport.rpt")));
            Rd.SetDataSource(drugWiseReportDataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "DrugWiseSummary.pdf");
        }

        public JsonResult SearchDrugbyName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => p.Name.Contains(name)).Select(p => new { p.Name, p.Id}).Take(20).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillWiseDailySalesReport()
        {
            return View();
        }

        public ActionResult GetBillWiseDailySaleReport()
        {
            PatientSummaryDataSet ReportDataSet = new PatientSummaryDataSet();



            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/BillWiseDailyReportSale/BillWiseDailyReportSale.rpt")));
            //Rd.SetDataSource(ReportDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "BillWiseDailySale.pdf");
        }

        public ActionResult CashRegisterReport()
        {
            var insuranceCompanies = db2.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim().Contains("insurance") || p.CompanyName.Contains("cash")).ToList();

            return View(insuranceCompanies);
        }

        public ActionResult GetCashRegisterReport()
        {
           CashRegisterDataSet ReportDataSet = new CashRegisterDataSet();
            

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/CashRegister/CashRegisterReport.rpt")));
            //Rd.SetDataSource(ReportDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "CashRegister.pdf");
        }

        public CashRegisterDataSet GetCashRegisterData(DateTime FromDate, DateTime ToDate)
        {
            var dataSet = new CashRegisterDataSet();

            var selectedMedications = db2.Medications.Where(p => DbFunctions.TruncateTime(p.TimeAdded) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate).ToList();

            

            return dataSet;
        }

        public ActionResult SalesProfitReport()
        {
            return View();
        }

        public ActionResult GetSalesProfitReport()
        {
            SalesProfitReport ReportDataSet = new SalesProfitReport();


            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/SalesProfit/SalesProfitReport.rpt")));
            //Rd.SetDataSource(ReportDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "SalesProfitReport.pdf");
        }

        public ActionResult DonorWiseSupplierReport()
        {
            return View();
        }

        public ActionResult GetDonorWiseSupplierReport()
        {
            DonorWiseSupplierDataSet ReportDataSet = new DonorWiseSupplierDataSet();


            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/DonorWiseSupplier/DonorWiseSupplierReport.rpt")));
            //Rd.SetDataSource(ReportDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "DonorWiseSupplierReport.pdf");
        }

        public ActionResult DonorWiseIssueReport()
        {
            return View();
        }

        public ActionResult GetDonorWiseIssueReport()
        {
            DonorWiseIssueDataSet ReportDataSet = new DonorWiseIssueDataSet();


            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/DonorWiseIssue/DonorWiseIssueReport.rpt")));
            //Rd.SetDataSource(ReportDataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            return File(Stream, "application/pdf", "DonorWiseSupplierReport.pdf");
        }

        public ActionResult SearchItemFilter(string search, string filter)
       {
            search = search.ToLower();

            var data = db.ItemMaster.DistinctBy(x => x.DrugId).Where(e => e.ItemName.ToLower().Contains(search.ToLower())
       || e.BatchNo.ToLower().Contains(search.ToLower()));

            //var data = db.ItemMaster.DistinctBy(e => e.DrugId)
            //    .Where(e => e.ItemName.ToLower().Contains(search.ToLower()));

            if (filter != null && filter == "Current Stock = 0")
            {
                data = data.Where(e => e.CurrentStock == 0);
            }
            else if (filter != null && filter == "Is Donor")
            {
                // data = data.Where(e => e.BatchNo);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Private Methods
        public List<PharmacyStockPosition> GetItems(string name)
        {
            var itemsSelected = db.ItemMaster.Where(p => p.ItemName == name).ToList();
            var itemsSold = new List<SimulationTreatment>();

            foreach (var item in itemsSelected)
            {
                itemsSold.AddRange(db.SimulationTreatment.Where(p => p.ItemMasterId == item.Id).ToList());
            }


            List<PharmacyStockPosition> pharmacyStockPositions = new List<PharmacyStockPosition>();

            foreach (var item in itemsSelected)
            {
                PharmacyStockPosition pharmacyStockPosition = new PharmacyStockPosition()
                {   Id = item.Id,
                    OpeningStock = item.CurrentStock,
                    ItemName = item.ItemName,
                    TotalIssueQuantity = itemsSold.Sum(p => p.units),
                    CurrentStock = item.CurrentStock - itemsSold.Sum(p => p.units),
                    CostPrice = Convert.ToDecimal(item.CostPriceUnit),
                    ReOrderLevel = item.ReorderLevel
                };

                pharmacyStockPositions.Add(pharmacyStockPosition);

            }

            return pharmacyStockPositions;
        }

        public List<PharmacyStockPosition> GetItemsById(int Id, DateTime fromDate, DateTime toDate)
        {
            //This method returns a list of stock positions 
            //it receives a drug Id(Id) then searches the item Master 
            //it loops through item master and all items with that drug id are stored in itemsSelected

            var itemsSelected = db.ItemMaster.Where(p => p.DrugId == Id).ToList();

            List<DrugTransactions> drugTransactions = new List<DrugTransactions>();

            if(fromDate!=null && toDate!=null)
            {
                drugTransactions = db.DrugTransactions.Where(p=>p.TransactionDate>=fromDate && p.TransactionDate<=toDate).ToList();
            }
            else
            {
                drugTransactions = db.DrugTransactions.ToList();
            }

            
            var DrugsSold = new List<DrugTransactions>();

            List<PharmacyStockPosition> pharmacyStockPositions = new List<PharmacyStockPosition>();

            if (drugTransactions.Count() > 0)
            {
                foreach (var item in itemsSelected)
                {
                    DrugsSold.AddRange(drugTransactions.Where(p => p.ItemMasterId == item.Id).ToList());

                    int openingStock = new int();
                    double subtotal = new double();
                    int closingStock = new int();
                    DateTime transactionDate = new DateTime();
                    GetOpeningAndClosingStock(DrugsSold, item.CurrentStock, out openingStock, out closingStock, out subtotal, out transactionDate);

                    PharmacyStockPosition pharmacyStockPosition = new PharmacyStockPosition()
                    {
                        Id = item.Id,
                        OpeningStock = openingStock,
                        ItemName = item.ItemName,
                        TotalIssueQuantity = DrugsSold.Sum(p => p.QuantityIssued),
                        CurrentStock = item.CurrentStock,
                        CostPrice = Convert.ToDecimal(item.SellingPriceUnit),
                        ReOrderLevel = item.ReorderLevel,
                        TotalPurchaseQuantity = DrugsSold.Sum(p => p.QuantityIssued),
                        TotalAmountSold = subtotal,
                        BatchNumber = item.BatchNo,
                        ClosingStock = closingStock,
                        Date = transactionDate

                    };

                    pharmacyStockPositions.Add(pharmacyStockPosition);

                    DrugsSold = null;
                    DrugsSold = new List<DrugTransactions>();

                }
            }
            

            return pharmacyStockPositions;
        }

        private int GetClosingStockOfSomeDrug(int ItemMasterId, int PatientId, int currentStock, DateTime FromDate, DateTime toDate)
        {

            int CurrentStockForThatTime = 0;
            var drugTransaction = db.DrugTransactions.Where(p => p.ItemMasterId == ItemMasterId && p.PatientId == PatientId && DbFunctions.TruncateTime(p.TransactionDate) <= DbFunctions.TruncateTime(toDate))
                                    .OrderByDescending(x => x.Id)
                                    .FirstOrDefault();

            var itemMaster = db.ItemMaster.Where(p => p.Id == ItemMasterId).FirstOrDefault();

            if (itemMaster != null)
            {
                if (drugTransaction != null)
                {
                    var allTrasactionsBetweenDates = db.DrugTransactions.Where(p => p.TransactionDate >= drugTransaction.TransactionDate && p.ItemMasterId == ItemMasterId)
                                                  .ToList();

                    if (allTrasactionsBetweenDates.Count() > 0)
                    {
                        CurrentStockForThatTime = itemMaster.CurrentStock + allTrasactionsBetweenDates.Sum(p => p.QuantityIssued);
                        CurrentStockForThatTime = CurrentStockForThatTime - drugTransaction.QuantityIssued;
                    }
                    else
                    {
                        CurrentStockForThatTime = itemMaster.CurrentStock;
                    }
                }
                else
                {
                    CurrentStockForThatTime = itemMaster.CurrentStock;
                }
            }

           
          
            return CurrentStockForThatTime;
        }

        private int GetClosingStockOfSomeDrugForDepartment(int ItemMasterId, int DepartementId, int currentStock, DateTime FromDate, DateTime toDate)
        {

            int CurrentStockForThatTime = 0;
            var drugTransaction = db.DrugTransactions.Where(p => p.ItemMasterId == ItemMasterId && p.DepartmentId == DepartementId && DbFunctions.TruncateTime(p.TransactionDate) <= DbFunctions.TruncateTime(toDate))
                                    .OrderByDescending(x => x.Id)
                                    .FirstOrDefault();

            var itemMaster = db.ItemMaster.Where(p => p.Id == ItemMasterId).FirstOrDefault();

            if (itemMaster != null)
            {
                if (drugTransaction != null)
                {
                    var allTrasactionsBetweenDates = db.DrugTransactions.Where(p => p.TransactionDate >= drugTransaction.TransactionDate && p.ItemMasterId == ItemMasterId)
                                                  .ToList();

                    if (allTrasactionsBetweenDates.Count() > 0)
                    {
                        CurrentStockForThatTime = itemMaster.CurrentStock + allTrasactionsBetweenDates.Sum(p => p.QuantityIssued);
                        CurrentStockForThatTime = CurrentStockForThatTime - drugTransaction.QuantityIssued;
                    }
                    else
                    {
                        CurrentStockForThatTime = itemMaster.CurrentStock;
                    }
                }
                else
                {
                    CurrentStockForThatTime = itemMaster.CurrentStock;
                }
            }



            return CurrentStockForThatTime;
        }
        private void GetOpeningAndClosingStock(List<DrugTransactions> drugTransactions,int currentStock,out int openingStock,out int closingStock, out double subtotal,out DateTime transactionDate)
        {
            if(drugTransactions.Count()>0)
            {
                openingStock = drugTransactions.Sum(p => p.QuantityIssued) + currentStock;
                subtotal = 0;
                foreach (var item in drugTransactions)
                {
                    subtotal += (item.Rate * item.QuantityIssued);
                }
                closingStock = openingStock-drugTransactions.Sum(p=>p.QuantityIssued);
                transactionDate = drugTransactions.FirstOrDefault().TransactionDate;
            }
            else
            {
                openingStock =  currentStock;
                subtotal = 0;
                closingStock = currentStock;
                transactionDate = DateTime.Now;

            }
           
        }
        #endregion

        #region Added Code for Stock Summary Report

        public ActionResult StockSummary()
        {
            return View();
        }

        public ActionResult GetStockSummaryReport(string Format)
        {
            var dataSet = GetStockSummaryReportData();

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/StockSummary/RptStockSummary.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            if (Format != "PDF")
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "Stock Summary " + DateTime.Now.ToString("dd-MM-yyyy") + " .xlsx";

                return File(Stream, "application/xlsx", FileName);
            }
            else
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "Stock Summary " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

                return File(Stream, "application/pdf", FileName);
            }
        }

        private class DrugModel
        {
            public string Name { get; set; }
            public int CurrentStock { get; set; }
            public double UnitPrice { get; set; }
        }

        public DataSetStockSummary GetStockSummaryReportData()
        {
            DataSetStockSummary dataSet = new DataSetStockSummary();

            var AllDrugs = db.Drug.ToList();
            var AllItems = db.ItemMaster.Where(p => p.StoreName == "P").ToList();
            var AllDrugTarrifs = db.DrugTariffs.ToList();


            List<DrugModel> viewModel = new List<DrugModel>();

            foreach (var item in AllDrugs)
            {
                int currentStock = 0;
                int stock = AllItems.Where(p => p.DrugId == item.Id).Sum(x => x.CurrentStock);

                if (stock > 0)
                {
                    currentStock = stock;
                }

                DrugModel drugModel = new DrugModel()
                {
                    Name = item.Name,
                    CurrentStock = currentStock,
                    UnitPrice = AllDrugTarrifs.Where(p => p.DrugId == item.Id && p.TarrifName.ToLower() == "cash").FirstOrDefault()?.DrugUnitPrice ?? 0
                };

                viewModel.Add(drugModel);
            }

            foreach (var item in viewModel)
            {
                dataSet.Drugs.AddDrugsRow(
                    item.Name,
                    item.CurrentStock,
                    item.UnitPrice);
            }




            return dataSet;
        }

        private DataSetFacilityInformation GetAllReportHeader()
        {

            var facilityDataSet = new DataSetFacilityInformation();
            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            var facilityAddress = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityAddress").FirstOrDefault().Value;

            var facilityTelephone = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityContactNumber").FirstOrDefault().Value;

            var logoUrl = Path.Combine(Server.MapPath("~/Content/icons/HospitalLogo.png"));

            facilityDataSet.HospitalDetails.AddHospitalDetailsRow(
                HospitalName,
                facilityAddress,
                facilityTelephone,
                logoUrl
            );

            return facilityDataSet;
        }

        #endregion

        public void AddDepartmentVoucherItemsToDrugTransactions()
        {
            var AllDepartmentVouchers = db.DepartmentVoucher
                                              .Where(p => p.IsItemIssued == true)
                                              .Select(x=>x.DepartmentVoucherItems)
                                              .ToList();
            var AllDepartmentVoucherItems = db.DepartmentVoucherItem
                                  .Where(p => p.DepartmentVoucher.IsItemIssued == true)
                                  .Include(c=>c.ItemMaster)
                                  .Include(b=>b.DepartmentVoucher)
                                  .ToList();

            List<DrugTransactions> LstDrugTransactions = new List<DrugTransactions>();

            foreach (var item in AllDepartmentVoucherItems)
            {
                var drugTransactions = new DrugTransactions()
                {
                    ItemMasterId = item.ItemMasterId,
                    DepartmentId = item.DepartmentVoucherId,
                    QuantityIssued = item.Units,
                    Rate = Convert.ToInt32(item.ItemMaster.CostPriceUnit),
                    TransactionDate = item.DepartmentVoucher.IssueDate.Value
                };
                LstDrugTransactions.Add(drugTransactions);
            }

            db.DrugTransactions.AddRange(LstDrugTransactions);
            db.SaveChanges();

        }

        //From Babra

        public ActionResult StockReport()
        {
            return View();
        }

        public ActionResult GetStockMovementRpt(DateTime SDate, DateTime EDate)
        {

            //Stock stock = GetStockReportData(FromDate, ToDate);

            //Stock dataSetStock = new Stock();

            //add query to get all the data from db

            //dataSetStock._Stock.AddStockRow("Date(dd-MM-yyyy)", "DrugName", "Batch", "QtyStockIn", "QtyStockOut", "Balance", "UnitCost", "ValueOfStock", "SalePrice", "Sales", "GrossProfit");

            var dataSet = GetStockMovementData2(SDate, EDate.AddDays(1));

            ReportDocument rd = new ReportDocument();


            rd.Load(Path.Combine(Server.MapPath("~/Areas/PharmacyModule/Reports/StockMovement/RptStockMovement.rpt")));

            rd.SetDataSource(dataSet);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());




            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Stock Sample - " + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }

        public Reports.StockMovement.DataSetStockMovementShedule GetStockMovementData2(DateTime FromDate, DateTime ToDate)
        {
            Reports.StockMovement.DataSetStockMovementShedule dataSet = new Reports.StockMovement.DataSetStockMovementShedule();

            var drugTransactions = db.DrugTransactions
                .Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate)
                .ToList();

            var tempDT = db.DrugTransactions
               .Where(p => DbFunctions.TruncateTime(p.TransactionDate) < FromDate)
               .ToList();

            foreach (var dt in drugTransactions.GroupBy(e => e.ItemMasterId))
            {
                var ItemMasterId = dt.FirstOrDefault().ItemMasterId;
                var item = db.ItemMaster.FirstOrDefault(e => e.Id == ItemMasterId);
                dataSet.Stock.AddStockRow(DateTime.Today, item.ItemName, item.BatchNo, 0, dt.Sum(e => e.QuantityIssued), item.CurrentStock,
                    dt.FirstOrDefault().Rate, item.CurrentStock * item.CostPriceUnit, 0, 0, dt.Sum(e => e.Rate * item.SellingPriceUnit - e.Rate * item.CostPriceUnit));
            }

            //var allDrugs = db.Drug.ToList();

            //foreach (var drug in allDrugs)
            //{

            //    var allStockPositions = GetItemsById(drug.Id, default, DateTime.Now);

            //    foreach (var sp in allStockPositions)
            //    {
            //        var allSales = sp.OpeningStock - sp.CurrentStock;
            //        dataSet._Stock.AddStockRow(sp.Date??default,
            //            drug.Name,
            //            sp.BatchNumber,
            //            0,
            //            allSales,
            //            sp.ClosingStock,
            //            Convert.ToInt32(sp.CostPrice),
            //            Convert.ToDouble(sp.CostPrice * sp.ClosingStock),
            //            Convert.ToDouble(sp.CostPrice),
            //            sp.OpeningStock - sp.CurrentStock,
            //            allSales * sp.CurrentStock
            //            );
            //    }

            //}

            return dataSet;
        }

        public Reports.StockMovement.Stock GetStockMovementData(DateTime FromDate, DateTime ToDate)
        {
            Reports.StockMovement.Stock dataSet = new Reports.StockMovement.Stock();

            var drugTransactions = db.DrugTransactions
                .Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate)
                .ToList();

            foreach (var dt in drugTransactions)
            {
                var item = db.ItemMaster.FirstOrDefault(e => e.Id == dt.ItemMasterId);
                dataSet._Stock.AddStockRow(dt.TransactionDate, item.ItemName, item.BatchNo, 0, dt.QuantityIssued, item.CurrentStock,
                    dt.Rate, item.CurrentStock * item.SellingPriceUnit, item.SellingPriceUnit, 0, item.SellingPriceUnit - item.CostPriceUnit);
            }

            //var allDrugs = db.Drug.ToList();

            //foreach (var drug in allDrugs)
            //{

            //    var allStockPositions = GetItemsById(drug.Id, default, DateTime.Now);

            //    foreach (var sp in allStockPositions)
            //    {
            //        var allSales = sp.OpeningStock - sp.CurrentStock;

            //        dataSet._Stock.AddStockRow(sp.Date??default,
            //            drug.Name,
            //            sp.BatchNumber,
            //            0,
            //            allSales,
            //            sp.ClosingStock,
            //            Convert.ToInt32(sp.CostPrice),
            //            Convert.ToDouble(sp.CostPrice * sp.ClosingStock),
            //            Convert.ToDouble(sp.CostPrice),
            //            sp.OpeningStock - sp.CurrentStock,
            //            allSales * sp.CurrentStock
            //            );
            //    }

            //}

            return dataSet;
        }


    }
}