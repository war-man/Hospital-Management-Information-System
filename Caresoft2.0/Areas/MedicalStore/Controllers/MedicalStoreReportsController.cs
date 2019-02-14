using Caresoft2._0.Areas.PharmacyModule.Reports.CurrentStock;
using Caresoft2._0.Areas.PharmacyModule.Reports.DrugWiseReport;
using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.CrystalReports;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.MedicalStore.Controllers
{
    [Auth]
    public class MedicalStoreReportsController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();
        CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: MedicalStore/MedicalStoreReports
        public ActionResult Index()
        {
            return View();
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

            var data = db.ItemMaster.Where(p => p.StoreName == "MS");

            if (data != null && ItemName != null && ItemName.Length > 0)
            {
                data = data.Where(e => e.ItemName.Contains(ItemName));
            }

            if (CurrentStockFilter != null && !CurrentStockFilter.Equals("all"))
            {
                if (CurrentStockFilter.Equals("greater"))
                {
                    data = data.Where(e => e.CurrentStock > 0);

                  //  data = data.Where(e => e.CurrentStock > 0);
                }

                if (CurrentStockFilter.Equals("less"))
                {
                    data = data.Where(e => e.CurrentStock.Equals(ItemName));

                    //data = data.Where(e => e.CurrentStock == 0);
                }


            }

            if (IsDonor != null && (bool)IsDonor)
            {

            }

            return View(data.ToList().ToPagedList(PageIndex, PageSize));
        }

        public ActionResult GetCurrentStockReport(string ItemName, string CurrentStockFilter, bool? IsDonor)
        {
            var items = db.ItemMaster.Where(p => p.StoreName == "MS").ToList();

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


        public ActionResult CurrentStock1(int? page, string ItemName, int? ItemId, bool? CurrentStockFilter, bool? IsDonor)
        {
            ViewBag.ItemName = ItemName;
            ViewBag.ItemId = ItemId;
            ViewBag.CurrentStockFilter = CurrentStockFilter;
            ViewBag.IsDonor = IsDonor;

            int PageIndex = 1;
            int PageSize = 10;
            PageIndex = page.HasValue ? page.Value : 1;

            var data = db.ItemMaster.Where(p => p.StoreName == "MS").ToList();

            if (ItemName != null && ItemName.Length > 0)
            {
                data = data.Where(e => e.ItemName.Contains(ItemName)).ToList();
            }

            if (CurrentStockFilter != null && (bool)CurrentStockFilter)
            {
                data = data.Where(e => e.CurrentStock == 0).ToList();
            }

            if (IsDonor != null && (bool)IsDonor)
            {

            }

            return View(data.ToPagedList(PageIndex, PageSize));
        }

        public ActionResult GetCurrentStockReport1(string ItemName, bool? CurrentStockFilter, bool? IsDonor)
        {
            var items = db.ItemMaster.Where(p => p.StoreName == "MS").ToList();

            if (ItemName != null && ItemName.Length > 0)
            {
                items = items.Where(e => e.ItemName.Contains(ItemName)).ToList();
            }

            if (CurrentStockFilter != null && (bool)CurrentStockFilter)
            {
                items = items.Where(e => e.CurrentStock == 0).ToList();
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
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "MSCurrentStock " + DateTime.Now.ToString() + ".pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult SearchItemFilter(string search, string filter)
        {
            var data = db.ItemMaster.DistinctBy(x => x.DrugId).Where(e => e.ItemName.ToLower().Contains(search.ToLower()) 
            || e.BatchNo.ToLower().Contains(search.ToLower()));

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
                    CurrentStock = item.CurrentStock,
                    CostPrice = item.CostPrice,
                    SellingPrice = item.Selling,
                    ProfitAmount = item.ProfitAmount,
                    Profit = item.Profit
                };
                LstDrugWiseViewModel.Add(viewModel);
            }

            //LstDrugWiseViewModel = LstDrugWiseViewModel.OrderBy(p => p.VoucherDate).ToList();
            LstDrugWiseViewModel = LstDrugWiseViewModel.OrderBy(p => p.DrugName).ToList();


            return PartialView("~/Areas/PharmacyModule/Views/PharmacyReports/_DrugWiseReport.cshtml", LstDrugWiseViewModel);
        }

        private DrugWiseReportDataSet GetDrugTWiseReportData(DateTime FromDate, DateTime ToDate, int? DrugId, int? insuaranceCompany)
        {
            DrugWiseReportDataSet drugWiseReportDataSet = new DrugWiseReportDataSet();

            var data = new List<DrugTransactions>();

            if (DrugId.HasValue && insuaranceCompany.Value == 0)
            {
                var DrugItems = db.ItemMaster.Where(p => p.DrugId == DrugId.Value && p.StoreName == "MS").ToList();
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

                            if (patientsName == null || patientsName == "")
                            {
                                if (item.DepartmentId != null)
                                {
                                    patientsName = db2.Departments.FirstOrDefault(p => p.Id == item.DepartmentId.Value)?.DepartmentName ?? "";
                                    patientsName = patientsName + "-#Department";
                                }
                            }

                            drugWiseReportDataSet.DrugWiseReport.AddDrugWiseReportRow(
                                    drug.ItemName ?? "",
                                    drug.BatchNo ?? "",
                                    walkInData.PatientIdentifierId.ToString() ?? "",
                                    walkInData.TimeAdded.ToString("dd-MM-yyyy") ?? "",
                                    patientsName ?? "",
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
                                        drug.ItemName ?? "",
                                        drug.BatchNo ?? "",
                                        medication.BillNo.ToString() ?? "",
                                        item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                        patientsName ?? "",
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
                                        drug.ItemName ?? "",
                                        drug.BatchNo ?? "",
                                        "",
                                        item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                        patientsName ?? "",
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
                var DrugItems = db.ItemMaster.Where(p => p.DrugId == DrugId.Value && p.StoreName == "MS").ToList();
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
                                    drug.ItemName ?? "",
                                    drug.BatchNo ?? "",
                                    medication.BillNo.ToString() ?? "",
                                    item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                    patientsName ?? "",
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
                                    drug.ItemName ?? "",
                                    drug.BatchNo ?? "",
                                    "",
                                    item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                    patientsName ?? "",
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
                    var drug = db.ItemMaster.Where(x => x.Id == item.ItemMasterId && x.StoreName == "MS").FirstOrDefault();
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
                                    drug.ItemName ?? "",
                                    drug.BatchNo ?? "",
                                    medication.BillNo.ToString() ?? "",
                                    item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                    patientsName ?? "",
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
                                    drug.ItemName ?? "",
                                    drug.BatchNo ?? "",
                                    "",
                                    item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                    patientsName ?? "",
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
                            var drug = db.ItemMaster.Where(p => p.Id == walkInData.DrugId && p.StoreName == "MS").FirstOrDefault();
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
                                        drug.ItemName ?? "",
                                        drug.BatchNo ?? "",
                                        walkInData.PatientIdentifierId.ToString() ?? "",
                                        walkInData.TimeAdded.ToString("dd-MM-yyyy") ?? "",
                                        patientsName ?? "",
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
                        var drug = db.ItemMaster.Where(p => p.Id == item.ItemMasterId && p.StoreName == "MS").FirstOrDefault();


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
                                        drug.ItemName ?? "",
                                        drug.BatchNo ?? "",
                                        medication.BillNo.ToString() ?? "",
                                        item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                        patientsName ?? "",
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
                                        drug.ItemName ?? "",
                                        drug.BatchNo ?? "",
                                        "",
                                        item.TransactionDate.ToString("dd-MM-yyyy") ?? "",
                                        patientsName ?? "",
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

            foreach (var item in drugWiseReportDataSet.DrugWiseReport.OrderByDescending(p => p.CurrentStock).ToList())
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

        public ActionResult GetDrugWiseReport(DateTime FromDate, DateTime ToDate, int? DrugId, int? insuaranceCompany)
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

    }
}