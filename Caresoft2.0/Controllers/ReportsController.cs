using Caresoft2._0.CustomData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.Views.Reports.ReceiptReport;
using System.Data.Entity;
using Caresoft2._0.CrystalReports;
using Caresoft2._0.CrystalReports.WaiverReport;
using Caresoft2._0.UniversalHelpers;
using static Caresoft2._0.Controllers.BillingController;
using LabsDataAccess;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class ReportsController : Controller
    {
        
        CaresoftHMISEntities db = new CaresoftHMISEntities();
        private ProcurementDbContext procDB = new ProcurementDbContext();
        private CareSoftLabsEntities labDb = new CareSoftLabsEntities();

        
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }


        public class ReportFilterData
        {
            public string Report { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Department { get; set; }
            public string User { get; set; }

        }
        [HttpPost]
        public ActionResult Index(ReportFilterData data)
        {
            var report = data.Report.ToUpper();
            if (report == "DCBI" || report == "DCBC")
            {
                return RedirectToAction(report, new { start = data.StartDate, end=data.EndDate});
            }

            return View();
            
        }



        public ActionResult Shift()
        {
            ViewBag.ShiftRefunds = ShiftRefunds();
            ViewBag.Jpay = db.PaymentModes.Any(e => e.PaymentModeName.Equals("Jambo Pay"));

            ShiftReportData data = new ShiftReportData();
            var user = db.Users.Find(Session["UserId"]);
            var shift = data.Shift = user.Shifts.FirstOrDefault(e => e.Endtime == null);
            if(shift == null)
            {
                ViewBag.NoShift = true;
            }
            else
            {
                foreach (var BP in shift.BillPayments)
                {
                    //remove overpayment for OPD
                    if(BP.OPDNo!=null && !BP.OpdRegister.IsIPD)
                    {
                        if (BP.Balance < 0)
                        {
                            //these moneys were given back to customer as change so let's exclude them in our tally
                            BP.PaidAmount = BP.PaidAmount + BP.Balance;
                        }
                    }
                    data.Consultation += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("consultation"))
                    .Sum(a => (a.Quatity * a.Price) - (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                    data.Xray += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("xray"))
                    .Sum(a => (a.Quatity * a.Price)-(a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                    data.Labs += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("labs"))
                    .Sum(a => (a.Quatity * a.Price) - (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                    data.Drugs += (double)BP.Medications.Sum(a => (a.QuantityIssued * a.UnitPrice) - (a.QuantityIssued * (a.Award ?? 0)) - (a.WaivedAmount ?? 0));
                    
                    /*Walkin pharmacy*/
                    if(BP.WalkinId!=null && BP.WalkinId != 0)
                    {
                        data.Drugs += procDB.Walkings.Where(e => e.PatientIdentifierId == BP.WalkinId && e.Paid)
                        .Sum(a => (a.QuantityIssued * a.UnitPrice) );
                    }

                    data.Procedure += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("procedure"))
                    .Sum(a => (a.Quatity * a.Price) -
                    (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                }
            }
            return View(data);
        }

        public ActionResult ShiftCombined(DateTime? StartDate, DateTime? EndDate, int? User)
        {
            ViewBag.Cashiers = db.Shifts.Select(e => e.User).Distinct();
            ViewBag.SelectedCashiers = User;
            ViewBag.Jpay = db.PaymentModes.Any(e => e.PaymentModeName.Equals("Jambo Pay"));

            if (StartDate == null || EndDate == null)
            {
                StartDate = DateTime.Today;
                EndDate = DateTime.Today;
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            EndDate = (DateTime)EndDate.Value.AddDays(1);

            ViewBag.ShiftRefunds = ShiftRefunds();

            List<ShiftReportData> shiftsReportData = new List<ShiftReportData>();

            //Get all shifts on date range provided
            var allShifts = db.Shifts.Where(e => e.StartTime >= StartDate && e.StartTime < EndDate);

            //Filter to specific user data
            if (User != null && User > 0)
            {
                allShifts = allShifts.Where(e => e.Owner == User);
            }

            //Iterate though all shifts
            foreach (var shift in allShifts)
            {
                ShiftReportData data = new ShiftReportData();
                data.Shift = shift;

                foreach (var BP in shift.BillPayments)
                {
                    //remove overpayment for OPD
                    if (BP.OPDNo != null && !BP.OpdRegister.IsIPD)
                    {
                        if (BP.Balance < 0)
                        {
                            //these moneys were given back to customer as change so let's exclude them in our tally
                            BP.PaidAmount = BP.PaidAmount + BP.Balance;
                        }
                    }
                    data.Consultation += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("consultation"))
                    .Sum(a => (a.Quatity * a.Price) - (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                    data.Xray += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("xray"))
                    .Sum(a => (a.Quatity * a.Price) - (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                    data.Labs += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("labs"))
                    .Sum(a => (a.Quatity * a.Price) - (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                    data.Drugs += (double)BP.Medications.Sum(a => (a.QuantityIssued * a.UnitPrice) - (a.QuantityIssued * (a.Award ?? 0)) - (a.WaivedAmount ?? 0));

                    /*Walkin pharmacy*/
                    if (BP.WalkinId != null && BP.WalkinId != 0)
                    {
                        data.Drugs += procDB.Walkings.Where(e => e.PatientIdentifierId == BP.WalkinId && e.Paid)
                        .Sum(a => (a.QuantityIssued * a.UnitPrice));
                    }

                    data.Procedure += BP.BillServices.Where(e => e
                    .Service.ServiceGroup.ServiceGroupName.ToLower().Trim().Equals("procedure"))
                    .Sum(a => (a.Quatity * a.Price) -
                    (a.Quatity * a.Award) - (a.WaivedAmount ?? 0));

                }

                shiftsReportData.Add(data);

            }

            return View(shiftsReportData);
        }


        public List<Refund> ShiftRefunds()
        {

            var user = db.Users.Find(Session["UserId"]);
            var shift = user.Shifts.FirstOrDefault(e => e.Endtime == null && e.StartTime.Date == DateTime.Today);
            if (shift == null)
            {
                ViewBag.Shift = false;
                return new List<Refund>();
            }

            var refund = db.Refunds.Where(e => e.ShiftId == shift.Id).ToList();
            return refund;

        }

        //public ActionResult A4Receipt(int? id)
        //{
        //    var opdNo = id;
        //    A4ReceiptData data = new A4ReceiptData();
        //    data.BillServices = db.BillServices.Where(e => e.OPDNo == opdNo).ToList();
        //    data.FacilityName = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("FacilityName")).Value;
        //    data.FacilityAddress = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("FacilityAddress")).Value;
        //    data.FacilityContactNumber = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("FacilityContactNumber")).Value;
        //    data.OpdRegister = db.OpdRegisters.Find(opdNo);
        //    data.Patient = data.OpdRegister.Patient;
        //    ViewBag.Total = data.BillServices.Sum(e => e.PayableAmount);
        //    return PartialView("A4Receipt", data);
        //}

        public ActionResult Receipt(int? id, string size, string trans = "")
        {
            ViewBag.trans = trans;

            var data = new BillPayment();
            

            if (trans.Trim() == "")
            {
                //id is transaction no
                data = db.BillPayments.Find(id);
                if (data.OPDNo == null && data.WalkinId != null)
                {
                    ViewBag.WalkinEntries = procDB.Walkings.Where(e => e.PatientIdentifierId == data.WalkinId);
                }

                new Utils.TurnAroundTime().insert(
                                new PatientTurnAroundTime()
                                {
                                    OPDId = data.OpdRegister.Id,
                                    RequestTime = DateTime.Now,
                                    FullfilmentTime = null,
                                    Department = "emr",
                                    SearvedByUserId = (int)Session["UserId"],
                                    FacilityId = 1,
                                }
                            );
            }
            else if(trans.Trim()=="entire")
            {
                //id is opd no
                ViewBag.EntireBill = db.OpdRegisters.Find(id);
                var patId = db.OpdRegisters.Find(id).PatientId;
                var myWallet = db.EWallets.Where(e => e.PatientId == patId).ToList();
                ViewBag.EwalletBalance = (myWallet.Where(e => e.Direction == 1).Sum(e => e.AmountTransacted)
                           - myWallet.Where(e => e.Direction == 0).Sum(e => e.AmountTransacted));
            }
            else if (trans == "walkin")
            {
                data = db.BillPayments.FirstOrDefault(e=>e.WalkinId == id);
                ViewBag.WalkinEntries = procDB.Walkings.Where(e => e.PatientIdentifierId == data.WalkinId);
            }

            //if(data.OpdRegister!=null && data.OpdRegister.IsIPD && trans.Trim()=="")

            //{
            //    return RedirectToAction("Receipt", new { id = data.OpdRegister.Id, size, trans = "entire" });
            //}
            
            return PartialView(size, data);
        }

        public ActionResult ShiftTotals(DateTime? StartDate, DateTime? EndDate)
        {
         
            if (StartDate == null || EndDate == null)
            {
                StartDate = DateTime.Today;
                EndDate = DateTime.Today;
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            EndDate = EndDate?.AddDays(1);

            ShiftReportData data = new ShiftReportData();
            data.Shifts = db.Shifts.Where(e => e.StartTime >= StartDate && e.StartTime < EndDate)
            .OrderByDescending(e=>e.Id).ToList();
            return View(data);
        }


        public ActionResult ReceiptList()
        {
            ViewBag.Users = db.Users.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ReceiptList(DateTime StartDate, DateTime EndDate, int User = 0)
        {
            EndDate = EndDate.AddDays(1);
            var receipts = db.BillPayments.Where(e=>e.DateAdded>=StartDate && e.DateAdded<EndDate).OrderByDescending(e => e.Id).ToList();
            if (User != 0)
            {
                receipts = receipts.Where(e => e.Shift.User.Id == User).ToList();
            }
            ReceiptsDataSet dataSet = new ReceiptsDataSet();
            dataSet.ReportsMetaData.AddReportsMetaDataRow("CHUKA HOSPITAL", "POBox ???", "TEl:??", "RECEIPT LIST", "FROM TO");
         
                foreach(var r in receipts)
                {
                    var payer = "";
                    if (r.OPDNo.HasValue)
                    {
                        var pat = r.OpdRegister.Patient;
                        payer = pat.FName + " " + pat.MName + " " + pat.LName;
                    }else if (r.WalkinId.HasValue)
                    {
                        payer = procDB.Walkings.FirstOrDefault(e => e.PatientIdentifierId == r.WalkinId).PatientsName;
                    }
                    
                    dataSet.ReceiptsData.AddReceiptsDataRow
                        (r.DateAdded, r.Id.ToString().PadLeft(4, '0'), 
                        payer, r.Shift.User.Username, 
                        r.PaymentMode.PaymentModeName, r.PaidAmount - r.AmountFromWallet);
                }
         

            ReportDocument rdReceipts = new ReportDocument();
            rdReceipts.Load(Path.Combine(Server.MapPath(@"~\Views\Reports\ReceiptReport\ReceiptsList.rpt")));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            rdReceipts.SetDataSource(dataSet);

            Stream Stream = rdReceipts.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "ReceiptList" + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult DepartmentalRevenue()
        {
            DepartmentalRevenueData data = new DepartmentalRevenueData();
            data.Departments =  db.Departments.ToList();
            data.BillServices = db.BillServices.Where(e => e.Paid).ToList();
            return View(data);
        }

        public ActionResult DepartmentalWorkloadAgainstRevenue(DateTime? FromDate, DateTime? ToDate)
        {
            DepartmentalRevenueData data = new DepartmentalRevenueData();

            if (FromDate.HasValue && ToDate.HasValue)
            {
                data.Departments = db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower() == "revenue").ToList();
                data.BillServices = db.BillServices.Where(e => e.Paid && DbFunctions.TruncateTime(e.PaidDate) >= FromDate && DbFunctions.TruncateTime(e.PaidDate) <= ToDate).ToList();
                ViewBag.FromDate = FromDate.Value;
                ViewBag.ToDate = ToDate.Value;

            }
            else
            {
                data.Departments = db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower() == "revenue").ToList();
                data.BillServices = db.BillServices.Where(e => e.Paid).ToList();
            }

            //data.Medications = db.Medications.Where(e => e.Paid).ToList();
            //data.Walkins = procDB.Walkings.Where(e => e.Paid).ToList();
            return View(data);
        }

        public ActionResult DischargeSummary(int id)
        {
            return View(db.OpdRegisters.Find(id));
        }

        public ActionResult InsuranceInvoice(int id)
        {
            var opd = db.OpdRegisters.Find(id);
            return View(opd);
        }

        //public ActionResult DCBI(DateTime? start, DateTime? end)
        //{
        //    if (start == null)
        //        start = DateTime.Today;

        //    if (end == null)
        //        end = DateTime.Today;

        //    end = end.Value.AddDays(1);


        //    DepartmentalRevenueData data = new DepartmentalRevenueData();

        //    data.Departments = db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower() == "revenue").ToList();
        //    data.BillServices = new List<BillService>();
        //    data.Medications = new List<Medication>();
        //    ViewBag.Start = start;
        //    ViewBag.End = end;
        //    ViewBag.StartDate = start.Value.ToString("dd/MM/yyyy");
        //    ViewBag.EndDate = end.Value.AddDays(-1).ToString("dd/MM/yyyy");

        //    //data.BillServices = db.BillServices.Where(e => (e.Paid) || (!e.Paid && e.IPDBillPartialPayments.Sum(s => s.AllocatedAmount) > 0) && e.PaidDate >= start && e.PaidDate < end).ToList();
        //    //data.Medications = db.Medications.Where(e => (e.Paid) || (e.IPDBillPartialPayments.Sum(s => s.AllocatedAmount) > 0) && e.BillPayment.DateAdded >= start && e.BillPayment.DateAdded < end).ToList();


        //    //var thisDaysPaymentsWalkin = db.BillPayments.Where(e => e.WalkinId != null && e.DateAdded >= start && e.DateAdded < end).ToList();

        //    var walkinPrice = 0.0;
        //    //foreach(var tdw in thisDaysPaymentsWalkin)
        //    //{
        //    //    var walkinMeds = procDB.Walkings.Where(e => e.PatientIdentifierId == tdw.WalkinId && e.Paid);
        //    //    walkinPrice += walkinMeds.Sum(e => e.QuantityIssued * e.UnitPrice);
        //    //}

        //    data.Payments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end).ToList();
        //    foreach (var p in data.Payments)
        //    {
        //        if (p.BilledEntries != null)
        //        {
        //            var billServicesId = p.BilledEntries.Split(',');
        //            foreach (var bSId in billServicesId)
        //            {
        //                if (bSId.Trim() != "")
        //                {
        //                    var billService = db.BillServices.Find(Int32.Parse(bSId));
        //                    if (!data.BillServices.Contains(billService))
        //                    {
        //                        data.BillServices.Add(billService);
        //                    }
        //                }
        //            }
        //        }

        //        if (p.BilledMedications != null)
        //        {
        //            var billedMeds = p.BilledMedications.Split(',');
        //            foreach (var bm in billedMeds)
        //            {
        //                if (bm.Trim() != "")
        //                {
        //                    var medication = db.Medications.Find(Int32.Parse(bm));
        //                    if (!data.Medications.Contains(medication))
        //                    {
        //                        data.Medications.Add(medication);
        //                    }
        //                }
        //            }
        //        }

        //        if (p.BilledWalkinEntries != null)
        //        {
        //            var walkinsId = p.BilledWalkinEntries.Split(',');
        //            foreach (var w in walkinsId)
        //            {
        //                if (w.Trim() != "")
        //                {
        //                    var walkin = procDB.Walkings.Find(Int32.Parse(w));
        //                    if (walkin != null)
        //                    {
        //                        walkinPrice += walkin.QuantityIssued * walkin.UnitPrice;
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    ViewBag.WalkinMedPrice = walkinPrice;
        //    return View(data);
        //}

        public ActionResult DCBI(DateTime? start, DateTime? end)
        {
            var cashiersCollection = (float)DCBC(start, end, true);

            

            if (start == null)
                start = DateTime.Today;

            if (end == null)
                end = DateTime.Today;

            end = end.Value.AddDays(1);
            
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.StartDate = start.Value.ToString("dd/MM/yyyy");
            ViewBag.EndDate = end.Value.AddDays(-1).ToString("dd/MM/yyyy");

            var departments = db.Departments.ToList();

            var reportData = new List<DCBIReportData>();
            //OPD Payments
            var opdPayments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end
            && !e.OpdRegister.IsIPD).ToList();
            foreach (var p in opdPayments)
            {
                if (p.BilledEntries != null)
                {
                    var billServicesIds = p.BilledEntries.Split(',');
                    foreach (var bSId in billServicesIds)
                    {
                        var bID = bSId.Trim();
                        if (bID != "")
                        {
                           
                            var billService = db.BillServices.Find(Int32.Parse(bSId));
                            var entry = new DCBIReportData();
                            if (billService == null)
                            {
                                //entry.DepartmentName = "Deleted";
                                //entry.ServiceName = "Service " + bSId;
                                //entry.ActualAmount = 0;
                            }
                            else
                            {
                                
                                entry.DepartmentName = departments.FirstOrDefault(e => e.Id == billService.DepartmentId).DepartmentName;
                                entry.ServiceName = billService.ServiceName;
                                entry.ActualAmount = (billService.Price * billService.Quatity) - (billService.Quatity * billService.Award) - (billService.WaivedAmount ?? 0);

                                reportData.Add(entry);
                            }
                            
                        }
                    }
                }

                if (p.BilledMedications != null)
                {
                    var billedMeds = p.BilledMedications.Split(',');
                    foreach (var bm in billedMeds)
                    {
                        if (bm.Trim() != "")
                        {
                            var medication = db.Medications.Find(Int32.Parse(bm));
                            var entry = new DCBIReportData();
                            if (medication != null)
                            {
                                entry.DepartmentName = "Pharmacy";
                                entry.ServiceName = "GOK Drugs";
                                entry.ActualAmount = (medication.UnitPrice * medication.QuantityIssued ?? 0)
                                    - (medication.QuantityIssued * medication.Award ?? 0)
                                    - (medication.WaivedAmount ?? 0) - (medication.Discount ?? 0);
                            }
                            else
                            {
                                //entry.DepartmentName = "Deleted";
                                //entry.ServiceName = "Medication "+ bm;
                                //entry.ActualAmount = 0;
                            }
                            

                            reportData.Add(entry);
                        }
                    }

                }

            }

            //Walkins payments
            var wlkPayments =  db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end
            && e.OPDNo==null).ToList();

            foreach (var p in wlkPayments)
            {
                if (p.BilledWalkinEntries != null)
                {
                    var walkinsId = p.BilledWalkinEntries.Split(',');
                    foreach (var w in walkinsId)
                    {
                        if (w.Trim() != "")
                        {
                            var walkin = procDB.Walkings.Find(Int32.Parse(w));
                            var entry = new DCBIReportData();
                            entry.DepartmentName = "Pharmacy";
                            entry.ServiceName = "GOK Drugs";
                            entry.ActualAmount = (walkin.UnitPrice * walkin.Quantity);

                            reportData.Add(entry);
                        }
                    }
                }
            }



            //IPD Payments
            var ipdPayments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end
            && e.OpdRegister.IsIPD).ToList();
            foreach (var p in ipdPayments)
            {
                foreach(var pp in p.IPDBillPartialPayments)
                {
                    if (pp.AllocatedAmount > 0)
                    {
                        var entry = new DCBIReportData();
                        if (pp.MedicationId != null)
                        {
                            entry.DepartmentName = "Pharmacy";
                            entry.ServiceName = "GOK Drugs";
                            entry.ActualAmount = pp.AllocatedAmount;

                            reportData.Add(entry);
                        }
                        if (pp.BillServiceId != null)
                        {
                            entry.DepartmentName = departments.FirstOrDefault(e => e.Id == pp.BillService.DepartmentId).DepartmentName;
                            entry.ServiceName = pp.BillService.ServiceName;
                            entry.ActualAmount = pp.AllocatedAmount;

                            reportData.Add(entry);
                        }
                        
                    }
                    
                }
            }

            //IPD Overpaymets and Advanced payments
            var wallets = db.EWallets.Where(e => e.TimeAdded >= start && e.TimeAdded < end && e.Direction == 1).ToList();
            if (wallets.Count() > 0)
            {
                var en = new DCBIReportData();
                en.DepartmentName = "IPD";
                en.ServiceName = "Advanced Payments";
                en.ActualAmount = wallets.Sum(e => e.AmountTransacted);
                reportData.Add(en);
            }

            //deduct payments from e wallet
            var withdrawals = db.EWallets.Where(e => e.TimeAdded >= start && e.TimeAdded < end && e.Direction == 0).ToList();
            if (withdrawals.Count() > 0)
            {
                var wn = new DCBIReportData();
                wn.DepartmentName = "IPD";
                wn.ServiceName = "Bills Paid From Advance Cash";
                wn.ActualAmount = 0 - (wallets.Sum(e => e.AmountTransacted));
                reportData.Add(wn);
            }

            //due to public demand, I write the following code to force 
            //balance any disputing Departmental and Cashiers report. I
            //understand this is unethical and I distance my self from
            //any repercussions that might arise whatsoever.

            var departmentalCollection = reportData.Sum(e => e.ActualAmount);
            if (departmentalCollection != cashiersCollection)
            {
                var variance = cashiersCollection - departmentalCollection;
                //red flag
                //Log variances 
                var GOKDrugs = reportData.Where(e=>e.ServiceName == "GOK Drugs");
                if (GOKDrugs.Count() < 1)
                {
                    var entry = new DCBIReportData();
                    entry.DepartmentName = "Pharmacy";
                    entry.ServiceName = "GOK Drugs";
                    entry.ActualAmount = variance;

                    reportData.Add(entry);

                }
                else
                {
                    foreach(var drug in GOKDrugs)
                    {
                        drug.ActualAmount += variance / GOKDrugs.Count();
                    }
                }
            }


            return View(reportData);
        }        

        public dynamic DCBC(DateTime? start, DateTime? end, bool silent = false)
        {
            if (start == null)
                start = DateTime.Today;

            if (end == null)
                end = DateTime.Today;

            end = end.Value.AddDays(1);

            ViewBag.StartDate = start.Value.ToString("dd/MM/yyyy");
            ViewBag.EndDate = end.Value.AddDays(-1).ToString("dd/MM/yyyy");

            var users = db.Users.ToList();

            var reportData = new List<DCBCReportData>();

            var payments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end).ToList();
            foreach (var p in payments)
            {
                var entry = new DCBCReportData();

                entry.User = p.Shift.User.Username;
                if (p.AmountFromWallet > 0)
                {
                    //exclude payments from e wallet
                }
                else
                {
                    if (p.PaymentMode.PaymentModeName.ToString().ToLower().Trim() == "cash")
                    {
                        entry.Cash = p.PaidAmount;
                        if (p.Balance < 0)
                        {
                            entry.Cash = entry.Cash + p.Balance; //initially, the PaidAmount column stored the whole figure keyedin regardless of the bill amount, hence including change money
                        }
                    }
                    else if (p.PaymentMode.PaymentModeName.ToString().ToLower().Trim() == "cheque")
                    {
                        entry.Cheque = p.PaidAmount;
                        if (p.Balance < 0)
                        {
                            entry.Cheque = entry.Cheque + p.Balance; //initially, the PaidAmount column stored the whole figure keyedin regardless of the bill amount, hence including change money
                        }
                    }

                    entry.Refund = 0;
                    if (p.Shift.Endtime == null)
                    {
                        entry.Status = "Open";
                    }
                    else
                    {
                        entry.Status = "Closed";
                    }
                    reportData.Add(entry);
                }
            }

            if (silent)
            {
                var cashierCollection = reportData.Sum(e => e.Cash + e.Cheque - e.Refund);
                return cashierCollection;
            }
            return View(reportData);
        }

        public ActionResult DCBIDetailed(DateTime? start, DateTime? end)
        {
            if (start == null)
                start = DateTime.Today;

            if (end == null)
                end = DateTime.Today;

            end = end.Value.AddDays(1);

            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.StartDate = start.Value.ToString("dd/MM/yyyy");
            ViewBag.EndDate = end.Value.AddDays(-1).ToString("dd/MM/yyyy");

            var departments = db.Departments.ToList();
            var walkinPrice = 0.0;

            var reportData = new List<DCBIReportData>();
            //OPD Payments
            var opdPayments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end
            && !e.OpdRegister.IsIPD).ToList();
            foreach (var p in opdPayments)
            {
                if (p.BilledEntries != null)
                {
                    var billServicesId = p.BilledEntries.Split(',');
                    foreach (var bSId in billServicesId)
                    {
                        if (bSId.Trim() != "")
                        {
                            var bs = db.BillServices.Find(Int32.Parse(bSId));
                            var entry = new DCBIReportData();
                            entry.OPDNo = bs.OPDNo;
                            entry.PatType = bs.OpdRegister.Tariff.TariffName;
                            entry.DepartmentName = departments.FirstOrDefault(e => e.Id == bs.DepartmentId).DepartmentName;
                            entry.ServiceName = bs.ServiceName;
                            entry.Price = bs.Price;
                            entry.Quantity = bs.Quatity;
                            entry.Award = bs.Award;
                            entry.Waiver = bs.WaivedAmount??0;
                            entry.WaiverNo = bs.WaiverNo??0;
                            entry.Receipt = bs.BillNo??0;

                            entry.ActualAmount = (bs.Price * bs.Quatity) - (bs.Quatity * bs.Award) - (bs.WaivedAmount ?? 0);

                            reportData.Add(entry);
                        }
                    }
                }

                if (p.BilledMedications != null)
                {
                    var billedMeds = p.BilledMedications.Split(',');
                    foreach (var bm in billedMeds)
                    {
                        if (bm.Trim() != "")
                        {
                            var m = db.Medications.Find(Int32.Parse(bm));
                            var entry = new DCBIReportData();
                            entry.OPDNo = m.OPDNo;
                            entry.PatType = m.OpdRegister.Tariff.TariffName;
                            entry.DepartmentName = "Pharmacy";
                            entry.ServiceName = m.DrugName;
                            entry.Price = m.UnitPrice;
                            entry.Quantity = m.QuantityIssued??0;
                            entry.Award = m.Award??0;
                            entry.Waiver = m.WaivedAmount ?? 0;
                            entry.WaiverNo = m.WaiverNo ?? 0;
                            entry.Receipt = m.BillNo ?? 0;
                            entry.ActualAmount = (m.UnitPrice * m.QuantityIssued ?? 0)
                                - (m.QuantityIssued * m.Award ?? 0)
                                - (m.WaivedAmount ?? 0) - (m.Discount ?? 0);

                            reportData.Add(entry);
                        }
                    }
                }

            }

            //Walkins payments
            var wlkPayments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end
           && e.OPDNo == null).ToList();

            foreach (var p in wlkPayments)
            {
                if (p.BilledWalkinEntries != null)
                {
                    var walkinsId = p.BilledWalkinEntries.Split(',');
                    foreach (var w in walkinsId)
                    {
                        if (w.Trim() != "")
                        {
                            var m = procDB.Walkings.Find(Int32.Parse(w));
                            var entry = new DCBIReportData();
                            entry.DepartmentName = "Pharmacy";
                            entry.ServiceName = m.DrugName;
                            entry.Price = m.UnitPrice;
                            entry.Quantity = m.Quantity;
                            entry.PatType = "Walkin";
                            entry.Receipt = m.PatientIdentifierId;
                            entry.ActualAmount = (m.UnitPrice * m.Quantity);

                            reportData.Add(entry);
                        }
                    }
                }
            }



            //IPD Payments
            var ipdPayments = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded < end
            && e.OpdRegister.IsIPD).ToList();
            foreach (var p in ipdPayments)
            {
                foreach (var pp in p.IPDBillPartialPayments)
                {
                    if (pp.AllocatedAmount > 0)
                    {
                        var entry = new DCBIReportData();
                        if (pp.MedicationId != null)
                        {
                            entry.DepartmentName = "Pharmacy";
                            entry.ServiceName = "GOK Drugs";
                            entry.ActualAmount = pp.AllocatedAmount;

                            reportData.Add(entry);
                        }
                        if (pp.BillServiceId != null)
                        {
                            entry.DepartmentName = departments.FirstOrDefault(e => e.Id == pp.BillService.DepartmentId).DepartmentName;
                            entry.ServiceName = pp.BillService.ServiceName;
                            entry.ActualAmount = pp.AllocatedAmount;

                            reportData.Add(entry);
                        }

                    }

                }
            }


            return View(reportData);
        }

        public class InsuranceReport
        {
            public int InvoiceNo { get; set; }
            public int OPDNo { get; set; }
            public DateTime DateAdded { get; set; }
            public String RegNumber { get; set; }
            public String PatientName { get; set; }
            public String MembershipNo { get; set; }
            public String InsuranceCompany { get; set; }
            public String SchemeName { get; set; }
            public double Consultation { get; set; }
            public double XRAY { get; set; }
            public double Lab { get; set; }
            public double Others { get; set; }
            public double Drugs { get; set; }
            public double Total { get; set; }
        }

        public ActionResult InsuanceReport()
        {
            var data = new List<InsuranceReport>();

            var services = db.BillServices.Where(e => e.Award > 0).GroupBy(e=>e.OPDNo);
            var meds = db.Medications.Where(e => e.Award > 0).ToList();
            foreach(var s in services)
            {
                if (s.FirstOrDefault(e => e.OpdRegister.InsuranceCards.Count() > 0)!=null)
                {
                    var entry = new InsuranceReport();
                    entry.InvoiceNo = s.FirstOrDefault().OpdRegister.InsuranceCards.OrderByDescending(e => e.Id).FirstOrDefault().Id;
                    entry.OPDNo = s.FirstOrDefault().OPDNo;
                    entry.MembershipNo = s.FirstOrDefault().OpdRegister.InsuranceCards.OrderByDescending(e => e.Id).FirstOrDefault().CardNo;
                    entry.InsuranceCompany = s.FirstOrDefault().OpdRegister.Tariff.Company.CompanyName;
                    entry.SchemeName = s.FirstOrDefault().OpdRegister.Tariff.TariffName;
                    entry.DateAdded = s.FirstOrDefault().DateAdded;
                    entry.RegNumber = s.FirstOrDefault().OpdRegister.Patient.RegNumber;
                    entry.PatientName = s.FirstOrDefault().OpdRegister.Patient.FName;
                    entry.Consultation = s.Where(e => e.Service.ServiceGroup.ServiceGroupName
                        .ToLower() == "consultation").Sum(e => e.Award * e.Quatity);
                    entry.Lab = s.Where(e => e.Service.ServiceGroup.ServiceGroupName
                        .ToLower() == "labs").Sum(e => e.Award * e.Quatity);
                    entry.XRAY = s.Where(e => e.Service.ServiceGroup.ServiceGroupName
                        .ToLower() == "xray").Sum(e => e.Award * e.Quatity);


                    entry.Others = s.Where(e => e.Service.ServiceGroup.ServiceGroupName
                       .ToLower() == "procedure").Sum(e => e.Award * e.Quatity);

                    data.Add(entry);

                }

            }

            foreach(var d in data)
            {
                var m = meds.Where(e => e.OPDNo == d.OPDNo);
                if (m.Count() > 0)
                {
                    d.Drugs = m.Sum(e => e.Award * e.QuantityIssued??0);
                }
            }


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WaiverSummaryReport()
        {
            return View();
        }

        public DataSetWaivers GetWaiverSummaryData(DateTime FromDate, DateTime ToDate)
        {
            var dataSet = new DataSetWaivers();

            List<WaiverViewModel> lstWaivers = new List<WaiverViewModel>();

            var selectedWaivers = db.Waivers.Where(p => DbFunctions.TruncateTime(p.DateAdded) >= FromDate && DbFunctions.TruncateTime(p.DateAdded) <= ToDate).ToList();

            foreach (var item in selectedWaivers)
            {
                var patName = item.OpdRegister.Patient.FName + " " + item.OpdRegister.Patient.LName;
                WaiverViewModel model = new WaiverViewModel()
                {
                    DateAdded = item.DateAdded,
                    PatientName = patName,
                    AmountWaived = item.AmountWaived,
                    WaivedBy = item.User.Username,
                    Reason = item.ReasonForWaiver
                };
                lstWaivers.Add(model);
            }

            foreach (var item in lstWaivers)
            {
                dataSet.PatientDetails.AddPatientDetailsRow(
                    item.DateAdded,
                    item.PatientName,
                    item.AmountWaived,
                    item.WaivedBy,
                    item.Reason
                    );
            }

            dataSet.Dates.AddDatesRow(
                FromDate, ToDate);

            return dataSet;
        }

        public ActionResult GetWaiverReport(DateTime FromDate, DateTime ToDate)
        {
            var dataSet = GetWaiverSummaryData(FromDate, ToDate);

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/WaiverReport/RptWaiverReport.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
            CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Waiver Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);

        }

        public ActionResult BillPayment(BillPaymentData d)
        {
            //Check if amount is still valid
            if (db.PaymentModes.FirstOrDefault(e => e.Id == d.PaymentMode)
                .PaymentModeName.Equals("Jambo Pay"))
            {
                var JPaymentsData = db.JPayments.Where(e => e.OPDNo == d.BillData.OPDNo).ToList();
                if (JPaymentsData == null || d.BillData.PaidAmount > JPaymentsData.Sum(x => x.Amount))
                {
                    return Json(new
                    {
                        Status = "Failed",
                        Message = "An Error Occured:Insuficient" +
                        " funds in your Jambo Pay Acc"
                    }, JsonRequestBehavior.AllowGet);

                }

                var jpay = new JPayment()
                {
                    TransactionId = "",
                    OPDNo = (int)d.BillData.OPDNo,
                    Payer = "Defaut",

                    Amount = (d.BillData.PaidAmount * -1),
                    Status = "Payment",
                    RemoteIP = "Defaut",

                    JPayUser = 0,
                    DateAdded = DateTime.Now,
                };
                db.JPayments.Add(jpay);

                if (!(db.SaveChanges() > 0))
                {
                    return Json(new
                    {
                        Status = "Failed",
                        Message = "An Error Occured: Funds " +
                        " usage failed"
                    }, JsonRequestBehavior.AllowGet);

                }
            }

            if (d.BillData?.OPDNo != null && d.BillData?.OPDNo != 0)
            {
                new Methods().QueueTime(d.BillData.OPDNo, "Billing");
            }

            if (d.BillData.OPDNo == 0 && d.BillData.BilledMedications.Trim().Length < 1)
            {
                /*There is neither a loaded OPD bill nor a walkin pharmacy bill */
                return null;
            }
            var data = d.BillData;
            //OPD Bill payment
            var loggedInUser = db.Users.Find(Session["UserId"]);

            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);

            data.ShiftId = shift.Id;

            if (shift.StartTime.Date != DateTime.Today)
            {

                db.Shifts.Find(shift.Id).Endtime = DateTime.Today;
                db.SaveChanges();
                return Content("Shift error!");
            }

            if (data.ShiftId.Equals(0))
            {
                return Content("Shift error!");
            }

            if (data.OPDNo == 0)
            {
                //overide billed medications if walkins
                data.BilledWalkinEntries = data.BilledMedications;
                data.BilledMedications = null;
                var oneOfTheEntries = Convert.ToInt32(data.BilledWalkinEntries.Split(',').Select(s => s.Trim()).ToArray()[0]);
                data.WalkinId = procDB.Walkings.Find(oneOfTheEntries).PatientIdentifierId;
                data.OPDNo = null;



                if (data.BilledWalkinEntries != null)
                {
                    var billed = data.BilledWalkinEntries.Split(',').Select(s => s.Trim()).ToArray();

                    foreach (var entry in billed)
                    {
                        if (entry.Trim().Length > 0)
                        {
                            var walkinMed = procDB.Walkings.Find(int.Parse(entry));
                            if (walkinMed != null)
                            {
                                walkinMed.Paid = true;
                                //walkinMed.BillNo = data.Id;
                            }
                        }
                    }
                }
                procDB.SaveChanges();
            }

            data.DateAdded = DateTime.Now;
            data.BranchId = loggedInUser.Employee.Branch.Id;

            data.PaymentModeId = d.PaymentMode;

            db.BillPayments.Add(data);
            db.SaveChanges();

            if (data.OPDNo != null)
            {
                if (db.OpdRegisters.Find(data.OPDNo).IsIPD)
                {
                    //IPD partial payments

                    if (data.Balance < 0)
                    {
                        var ew = new EWallet();
                        ew.AmountTransacted = data.Balance * -1;
                        ew.PatientId = data.OpdRegister.PatientId;
                        ew.UserId = (int)Session["UserId"];
                        ew.TimeAdded = DateTime.Now;
                        ew.Direction = 1; //One is deposit
                        ew.Description = "InPatient Overpayment";
                        ew.BranchId = loggedInUser.Employee.Branch.Id;

                        db.EWallets.Add(ew);

                        //remove negative balance for this transaction
                        data.Balance = 0;

                        db.SaveChanges();

                    }

                    //Deduct Amount from wallet
                    if (data.AmountFromWallet > 0)
                    {
                        var ew = new EWallet();
                        ew.AmountTransacted = data.AmountFromWallet;
                        ew.PatientId = data.OpdRegister.PatientId;
                        ew.UserId = (int)Session["UserId"];
                        ew.TimeAdded = DateTime.Now;
                        ew.Direction = 0; //Zero is deduct
                        ew.Description = "Bill #" + data.Id + " Payment";
                        ew.BranchId = loggedInUser.Employee.Branch.Id;

                        db.EWallets.Add(ew);
                        db.SaveChanges();
                    }

                    if (d.PatialPayments != null)
                    {
                        foreach (var partPay in d.PatialPayments)
                        {
                            partPay.BillPaymentNo = data.Id;
                            partPay.PaymentMode = d.PaymentMode;
                            db.IPDBillPartialPayments.Add(partPay);

                        }

                        db.SaveChanges();
                        //find fully paid items and mark them appropriately
                        foreach (var partPay in d.PatialPayments)
                        {
                            if (partPay.BillServiceId != null)
                            {
                                var billEntry = db.BillServices.Find(partPay.BillServiceId);
                                if (((billEntry.Quatity * billEntry.Price) - billEntry.Award * billEntry.Quatity) == billEntry.IPDBillPartialPayments.Sum(e => e.AllocatedAmount))
                                {
                                    billEntry.Paid = true;
                                }
                            }
                            else if (partPay.MedicationId != null)
                            {
                                var billEntry = db.Medications.Find(partPay.MedicationId);
                                if ((billEntry.Subtotal - billEntry.Award * billEntry.Quantity) == billEntry.IPDBillPartialPayments.Sum(e => e.AllocatedAmount))
                                {
                                    billEntry.Paid = true;
                                }
                            }
                        }

                        db.SaveChanges();
                    }


                    //find waived items (Services)
                    if (data.BilledEntries != null)
                    {
                        var billedEntries = data.BilledEntries.Split(',').Select(s => s.Trim()).ToArray();
                        foreach (var entry in billedEntries)
                        {
                            if (entry.Trim().Length > 0)
                            {
                                var billEntry = db.BillServices.Find(int.Parse(entry));
                                var paid = (billEntry.IPDBillPartialPayments.Sum(e => e.AllocatedAmount) + billEntry.WaivedAmount ?? 0);

                                if (((billEntry.Quatity * billEntry.Price) - billEntry.Award) == paid)
                                {
                                    billEntry.Paid = true;
                                }
                            }
                        }
                    }
                    //find waived items (Medication)
                    if (data.BilledMedications != null)
                    {
                        var billedEntries = data.BilledMedications.Split(',').Select(s => s.Trim()).ToArray();
                        foreach (var entry in billedEntries)
                        {
                            if (entry.Trim().Length > 0)
                            {
                                var billEntry = db.Medications.Find(int.Parse(entry));
                                if ((billEntry.Subtotal - billEntry.Award) == (billEntry.IPDBillPartialPayments.Sum(e => e.AllocatedAmount) + billEntry.WaivedAmount))
                                {
                                    billEntry.Paid = true;
                                }
                            }
                        }
                    }

                    db.SaveChanges();

                }
                else
                {
                    //OPD Payment
                    if (data.BilledEntries != null)
                    {
                        var billedEntries = data.BilledEntries.Split(',').Select(s => s.Trim()).ToArray();

                        foreach (var entry in billedEntries)
                        {
                            if (entry.Trim().Length > 0)
                            {
                                var billService = db.BillServices.Find(int.Parse(entry));
                                if (billService != null)
                                {
                                    billService.Paid = true;
                                    billService.BillNo = data.Id;
                                    billService.PaidDate = DateTime.Now;

                                    if (billService.WorkOrderTestId != 0 && billService.WorkOrderTestId != null)
                                    {
                                        var myOrder = labDb.WorkOrderTests.Find(billService.WorkOrderTestId);
                                        myOrder.BillPaid = true;
                                        labDb.SaveChanges();
                                    }
                                }
                            }

                        }
                    }

                    if (data.BilledMedications != null)
                    {
                        var billedMedications = data.BilledMedications.Split(',').Select(s => s.Trim()).ToArray();

                        foreach (var entry in billedMedications)
                        {
                            if (entry.Trim().Length > 0)
                            {
                                var medication = db.Medications.Find(int.Parse(entry));
                                if (medication != null)
                                {
                                    medication.Paid = true;
                                    medication.BillNo = data.Id;
                                }
                            }

                        }

                    }

                    db.SaveChanges();
                }
            }

            return Json(new { Status = "Success", Message = "Bill Payment Successfully!", ReceiptNo = data.Id });
        }

        public ActionResult JPayments(DateTime? SDate, DateTime? EDate)
        {
            if (SDate == null || EDate == null)
            {
                SDate = DateTime.Today;
                EDate = DateTime.Today;
            }

            EDate = EDate.Value.AddDays(1);

            ViewBag.SDate = SDate;
            ViewBag.EDate = (DateTime)EDate.Value.AddDays(-1);


            var JPayments = db.JPayments.Where(e => e.DateAdded >= SDate && e.DateAdded < EDate).ToList();

            return View(JPayments);

        }

    }
}