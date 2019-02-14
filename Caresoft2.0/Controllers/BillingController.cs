using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LabsDataAccess;
using System.Net;
using Caresoft2._0.Areas.Procurement.Models;
using Microsoft.Ajax.Utilities;
using static Caresoft2._0.Controllers.IPDController;
using System.Data.Entity.Core.Objects;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using Caresoft2._0.CrystalReports.IpdInvoice;
using Caresoft2._0.CrystalReports;
using System.Web.Mvc.Html;
using Caresoft2._0.Controllers.API;
using Caresoft2._0.UniversalHelpers;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class BillingController : Controller
    {
        private AuthClass authClass = new AuthClass();
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private CareSoftLabsEntities labDb = new CareSoftLabsEntities();


        // GET: Billing view
        public ActionResult Index(int? id, string type)
        {
            if (id != null)
            {
                new JPayController().Get((int)id);
            }

            var loggedInUser = db.Users.Find(Session["UserId"]);
            var keyBillEdit = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower() == "cashiers_edit_bill");
            if (keyBillEdit == null)
            {
                keyBillEdit = new KeyValuePair();
                keyBillEdit.Key_ = "cashiers_edit_bill";
                keyBillEdit.Value = "NO";
                db.KeyValuePairs.Add(keyBillEdit);
                db.SaveChanges();
            }

            if (loggedInUser.UserRole.RoleName.ToLower().Equals("billadjuster") ||
                loggedInUser.UserRole.RoleName.ToLower().Equals("sa") || ((keyBillEdit!=null) && (keyBillEdit.Value.ToLower()=="yes"))
                )
            {
                ViewBag.CanAdjust = true;
            }

            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);            

            if (shift == null )
            {
                //open shift if not opened
                return RedirectToAction("StartShift");
            }

            if (shift.StartTime.Date != DateTime.Today)
            {
                //If yesterdays shift is still runnning
                db.Shifts.Find(shift.Id).Endtime = DateTime.Today;
                db.SaveChanges();
                return RedirectToAction("StartShift");
            }

            var data = new OPDBillingFormData();
            data.OpdRegister = new OpdRegister();

            if (id != null)
            {
                if (type != "walkin")
                {
                    var opdEntry = db.OpdRegisters.Find(id);
                    if (opdEntry == null)
                    {
                        return RedirectToAction("BillingQueue");
                    }
                    data.BillServices = opdEntry.BillServices.ToList();
                    data.Drugs = opdEntry.Medications.ToList();
                    data.OpdRegister = opdEntry;
                    var myWallet = db.EWallets.Where(e => e.PatientId == data.OpdRegister.PatientId).ToList();

                    ViewBag.EwalletBalance = (myWallet.Where(e => e.Direction == 1).Sum(e => e.AmountTransacted) 
                        - myWallet.Where(e => e.Direction == 0).Sum(e => e.AmountTransacted));
                }
                else
                {
                    //walkins meddications
                    ViewBag.WalkinMedications = procDB.Walkings.Where(e => e.PatientIdentifierId == id).ToList();
                }
            }

            data.ServiceGroups = db.ServiceGroups.ToList();
            data.PaymentModes = db.PaymentModes.ToList();
            if (data.PaymentModes.Count() < 1)
            {
                new Seeder().SeedPaymentModes();
                data.PaymentModes = db.PaymentModes.ToList();
            }


            data.Shift = shift.Id;
            ViewBag.BillingType = type;

            //if (type != null && type.ToLower().Equals("ipdinvoice"))
            //{
            //    return View("IpdInvoice", data);
            //}

            try
            {
                var x = db.OpdRegisters.Find(id)?.PatientTurnAroundTimes?.FirstOrDefault(f =>
                                             f.Department.Equals("billing"));

                if (x.FullfilmentTime == null)
                {
                    x.FullfilmentTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
            catch (Exception e) { };

            return View(data);
        }

        public ActionResult StartShift()
        {
            var loggedInUser = db.Users.Find(Session["UserId"]);
            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);
            if (shift != null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                ViewBag.Message = TempData["SHiftClosedMessage"].ToString();
            } catch (Exception) {

            }
            return View();
        }

        [HttpPost]
        public ActionResult StartShift(String ShiftUsername, String ShiftPassword)
        {
            var loggedInUser = db.Users.Find(Session["UserId"]);
            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);
            if (shift != null)
            {
                //proceed to billing form
                return RedirectToAction("Index");
            }
            if (loggedInUser.Username.ToLower() == ShiftUsername.ToLower() && loggedInUser.Password == ShiftPassword)
            {
                Shift newShift = new Shift();
                newShift.StartTime = DateTime.Now;
                newShift.Owner = loggedInUser.Id;
                db.Shifts.Add(newShift);
                db.SaveChanges();
                return RedirectToAction("BillingQueue");
            }

            ViewBag.LogginError = "Invalid Username/Password Combination!";
            return View();
        }

        public ActionResult CloseShift()
        {

            var loggedInUser = db.Users.Find(Session["UserId"]);
            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);
            if (shift != null)
            {
                //close shift if exists
                shift.Endtime = DateTime.Now;
                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ShiftClosedMessage"] = "Shift Closed Successfully!";
            }
            return RedirectToAction("StartShift");
        }

        public ActionResult LoadBillingForm(int? Id, string type="")
        {
            //TODO pass string ServicesFilter and change the ViewBag.ServicesFilter dynamically
            ViewBag.ServicesFilter = "All";
            var loggedInUser = db.Users.Find(Session["UserId"]);

            OPDBillingFormData data = new OPDBillingFormData();
            if (type == "walkin")
            {
                ViewBag.WalkinMedications = procDB.Walkings.Where(e => e.PatientIdentifierId == Id).ToList();
            }
            else
            {
                data.OpdRegister = db.OpdRegisters.Find(Id);
                data.ServiceGroups = db.ServiceGroups.ToList();
                data.BillServices = data.OpdRegister.BillServices.OrderByDescending(e => e.Service.DepartmentId).ToList();
                data.Drugs = db.Medications.Where(e => e.OPDNo == Id).ToList();
                data.Patient = data.OpdRegister.Patient;

                var myWallet = db.EWallets.Where(e => e.PatientId == data.OpdRegister.PatientId).ToList();

                ViewBag.EwalletBalance = (myWallet.Where(e => e.Direction == 1).Sum(e => e.AmountTransacted)
                           - myWallet.Where(e => e.Direction == 0).Sum(e => e.AmountTransacted));
            }

            var keyBillEdit = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower() == "cashiers_edit_bill");

            if (loggedInUser.UserRole.RoleName.ToLower().Equals("billadjuster") 
                || loggedInUser.UserRole.RoleName.ToLower().Equals("social worker")
                ||loggedInUser.UserRole.RoleName.ToLower().Equals("sa")
                || loggedInUser.UserRole.RoleName.ToLower().Equals("socialworker")
                || ((keyBillEdit != null) && (keyBillEdit.Value.ToLower() == "yes")))
            {
                ViewBag.CanAdjust = true;
            }


            
            data.ServiceGroups = db.ServiceGroups.ToList();
            data.PaymentModes = db.PaymentModes.ToList();
            data.Shift = db.Shifts.FirstOrDefault(e => e.Owner == loggedInUser.Id && e.Endtime == null).Id;
            return PartialView("BillingForm", data);

        }

        // GET: Billing view
        public ActionResult BillingQueue(int page=1)
        {
            var todaysBills = db.OpdRegisters.Where(e => e.Date >= DateTime.Today
            //&& !e.Status.Trim().ToLower().Equals("draft")
            && (e.BillServices.ToList().Count() > 0) && !e.IsIPD).ToList();

            ViewBag.MedicalDeps = db.Departments.Where(e => e.IsMedical != null && e.IsMedical.Trim().ToLower().Equals("yes")).ToList();
            ViewBag.Walkins = procDB.Walkings.Where(e=>e.TimeAdded>=DateTime.Today)
                .DistinctBy(e => e.PatientIdentifierId).ToList();

            int itemsperpage = 15;
            var offset = (itemsperpage * page) - itemsperpage;
            ViewBag.Page = page;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(todaysBills.Count() / itemsperpage)));
            return View("BillingQueue", todaysBills.Skip(offset).Take(itemsperpage));
        }



        public ActionResult IPDBillingList()
        {
            var ipdReg = db.OpdRegisters.Where(e => e.IsIPD && !e.Discharged && e.Status.ToLower().Trim() != "closed").OrderByDescending(e=>e.Id).Take(20).ToList();
            ViewBag.isIPDList = true;
            return View(ipdReg);
        }

        private ProcurementDbContext procDB = new ProcurementDbContext();
        // GET: OPD list// todays list by default
        public ActionResult FIlterBillingList(FilterOPD filterOPD) //[Reuse OPD filter object]
        {
            string dep = filterOPD.Department.Trim().ToLower();
            filterOPD.StartDate += TimeSpan.Parse("00:00:00");
            filterOPD.EndDate += TimeSpan.Parse("23:59:59");

            ViewBag.Mode = "AJAX";

            ViewBag.Walkins = procDB.Walkings.Where(e=>e.TimeAdded >= DateTime.Today).ToList();

            var opdList = db.OpdRegisters.Where(e => e.Date >= filterOPD.StartDate
            && e.Date <= filterOPD.EndDate
            /*&& !e.Status.Trim().ToLower().Equals("draft")*/).ToList();
            if (filterOPD.SearchText != null)
            {
                opdList = db.OpdRegisters.Where(e => (e.Patient.FName.Contains(filterOPD.SearchText) ||
                e.Patient.MName.Contains(filterOPD.SearchText) || e.Patient.LName.Contains(filterOPD.SearchText) ||
                e.Patient.RegNumber.Contains(filterOPD.SearchText) || e.Id.ToString().Equals(filterOPD.SearchText)) &&
                (e.Date >= filterOPD.StartDate && e.Date <= filterOPD.EndDate)
               /* && !e.Status.Trim().ToLower().Equals("draft")*/).ToList();
            }
            else
            {
                opdList = db.OpdRegisters.Where(e => (e.Date >= filterOPD.StartDate && e.Date <= filterOPD.EndDate)
                /*&& !e.Status.Trim().ToLower().Equals("draft")*/).ToList();
            }

            ViewBag.SearchMeta = "Search results for " + filterOPD.SearchText
                + " for dates between " + filterOPD.StartDate + " and "
                + filterOPD.EndDate + " from " + filterOPD.Department + " department.";
            ViewBag.ResultsSize = opdList.Count();

            if (dep != null && dep != "all")
            {
                //) 
                opdList = opdList.Where(e => (e.Department != null && e.Department.Trim().ToLower() == dep && e.PatientReferals.Count() < 1)
                || (e.PatientReferals.Count() > 0 && e.PatientReferals.LastOrDefault().Department.DepartmentName.Trim().ToLower().Equals(dep))).ToList();
            }

            return PartialView("BillingQueue", opdList.Take(15));
        }

        public class SearchText
        {
            public String Text { get; set; }
        }
        [HttpPost]
        public ActionResult SearchServices(SearchText searchText)
        {
            var text = searchText.Text.Trim();
            var div = "";
            if (text.Length > 0)
            {
                div += "<div class='suggestion'>";
                var services = db.Services.Where(e => e.ServiceName.Contains(text));

                foreach (var ser in services)
                {
                    var entry = "<p data-service-id='" + ser.Id + "'>" + ser.ServiceName + "-" + ser.Department.DepartmentName + "</p>";
                    div += entry;
                }
                div += "</div>";
            }
            return Content(div);
        }

        public ActionResult OPDBilling(int? id)
        {
            //open shift if not opened
            var loggedInUser = db.Users.Find(Session["UserId"]);
            var shiftNumber = 0;
            var shift = db.Shifts.FirstOrDefault(e => e.Owner.Equals(loggedInUser.Id)
                && e.Endtime == null);//this is my existing unclosed shift
            if (shift != null)
            {
                shiftNumber = shift.Id;
            }
            else
            {
                //create a new shift
                Shift newShift = new Shift
                {
                    Owner = loggedInUser.Id,
                    StartTime = DateTime.Now

                };

                db.Shifts.Add(newShift);
                db.SaveChanges();

                shiftNumber = newShift.Id;
            }

            if (id != null)
            {
                //get billing form with preloaded billnumber/opd number

                ViewBag.HasModel = true;
                OPDBillingFormData data = new OPDBillingFormData();
                data.BillServices = db.BillServices.Where(e => e.OPDNo == id)
                    .OrderBy(e => e.Id)
                    .OrderByDescending(e => e.Service.DepartmentId).ToList();
                data.OpdRegister = db.OpdRegisters.FirstOrDefault(e => e.Id == id);
                data.Departments = db.Departments.ToList();
                data.Shift = shiftNumber;
                data.OPDBillingQueue = db.OpdRegisters.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("index");
            }


        }

        public JsonResult OPDRegisterJson(String search)
        {
            //recommendend -> return todays register only

            List<AutoCompleteData> opdRegister = db.OpdRegisters.Where
                (e => (e.Patient.FName.Contains(search) || e.Patient.LName.Contains(search) || e.Patient.RegNumber.Contains(search)) &&
                (e.Date >= DateTime.Today /*&& !e.Status.Trim().ToLower().Equals("draft")*/)).Select(x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.Patient.RegNumber + " " + x.Patient.Salutation + " " + x.Patient.FName + " " + x.Patient.MName + " " + x.Patient.LName
                }).ToList();
            return new JsonResult { Data = opdRegister, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult IPDBills(String search)
        {
            List<AutoCompleteData> IPDList = db.OpdRegisters.Where
                (e => (e.Patient.FName.Contains(search) || e.Patient.LName.Contains(search) || e.Patient.RegNumber.Contains(search)) && e.IsIPD && !e.Discharged
                && e.Status.ToLower().Trim() == "active").Select(x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.Patient.Salutation + " " + x.Patient.FName + " " + x.Patient.MName + " " + x.Patient.LName,
                    RegNumber = x.Patient.RegNumber
                }).ToList();
            return new JsonResult { Data = IPDList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BillsForWaiver(String search)
        {
            //List<AutoCompleteData> IPDList = db.OpdRegisters.Where
            //    (e => (e.Patient.FName.Contains(search) || e.Patient.LName.Contains(search) || e.Patient.RegNumber.Contains(search))
            //    && e.Status.ToLower().Trim() == "active" && ((e.Date == DateTime.Today && !e.IsIPD) || (e.IsIPD && !e.Discharged) )).Select(x => new AutoCompleteData
            //    {
            //        Id = x.Id,
            //        Name = x.Patient.Salutation + " " + x.Patient.FName + " " + x.Patient.MName + " " + x.Patient.LName,
            //        RegNumber = x.Patient.RegNumber
            //    }).ToList();


            List<AutoCompleteData> list = db.OpdRegisters.Where
                (e => (e.Patient.FName.Contains(search) || e.Patient.LName.Contains(search) || e.Patient.RegNumber.Contains(search))
                && (e.BillServices.Where(b => !b.Paid).Count() > 0 || e.Medications.Where(b => !b.Paid).Count() > 0))
                .Select(x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.Patient.Salutation + " " +
                    x.Patient.FName + " " + x.Patient.MName + " " +
                    x.Patient.LName + " ("+x.Date+")",
                    RegNumber = x.Patient.RegNumber
                }).ToList();
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult SearchService(String search)
        {
            List<AutoCompleteData> services = db.Services.Where
                (e => e.ServiceName.Contains(search)).Select(x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.Department.DepartmentName + "-" + x.ServiceName
                }).Take(20).ToList();
            return new JsonResult { Data = services, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class BillPaymentData
        {
            public int PaymentMode { get; set; }
            public BillPayment BillData { get; set; }
            public List<IPDBillPartialPayment> PatialPayments { get; set; }
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

        public class AddServiceToBillData
        {
            public int ServiceId { get; set; }
            public int OPDNo { get; set; }
        }

        [HttpPost]
        public ActionResult AddServiceToBill(AddServiceToBillData data)
        {

            if (data.OPDNo != null && data.OPDNo != 0)
            {
                new Methods().QueueTime(data.OPDNo, "Billing");
            }

            //Attempt add service to bill
            var tariffId = db.Tariffs.First(e => e.TariffName.ToLower().Trim().Equals("cash")).Id;
            if (data.OPDNo != 0)
            {
                tariffId = db.OpdRegisters.Find(data.OPDNo).Tariff.Id;
            }

            var service = db.Services.Find(data.ServiceId);

            var price = 0.0; var award = 0.0;
            price = service.CashPrice;

            var priceEntry = db.ServicesPrices.FirstOrDefault(e => e.TariffId == tariffId && e.ServiceId == data.ServiceId);
            if (priceEntry != null)
            {
                award = priceEntry.Award;
                if (priceEntry.AwardUnit.ToLower().Trim().Equals("percent"))
                {
                    award = price * award / 100;
                }
            }

            return Json(new { Price = price, Award = award });
        }



        public ActionResult LoadIPDBill(int id)
        {
            var ipdEntry = db.OpdRegisters.Find(id);
            if (ipdEntry == null)
            {
                return Json(new { status = "warning", message = "Patient not found!" }, JsonRequestBehavior.AllowGet);
            }
            
            return PartialView("WaiverTable", ipdEntry);


        }


        public ActionResult PaymentModeForm(string mode, int id)
        {
            var opd = db.OpdRegisters.Find(id);
            ViewBag.OPDNo = id;
        
            if (opd.Patient.Mobile != null && opd.Patient.Mobile.Trim() !="")
            {
                ViewBag.Mobile = int.Parse(opd.Patient.Mobile); //To simply remove the preceeding 0 to get a valid MSISDN
            }
            return PartialView("PaymentModes/" + mode);
        }

        public ActionResult Waiver()
        {
            ViewBag.ReasonsForWaiver = db.WaiverReasons.ToList();
            return View();
        }

        public class WaiverData
        {
            public Waiver Waiver { get; set; }
            public List<WaivedService> WaivedServices { get; set; }
        }

        public class WaivedService
        {
            public int ServiceId { get; set; }
            public String ServiceType { get; set; }
            public double WaivedAmount { get; set; }
        }

        [HttpPost]
        public ActionResult SaveWaiver(WaiverData data)
        {
            data.Waiver.UserId = (int)Session["UserId"];
            
            data.Waiver.BranchId = (int)Session["UserBranchId"];
            data.Waiver.DateAdded = DateTime.Now;

            db.Waivers.Add(data.Waiver);
            db.SaveChanges();


            foreach (var entry in data.WaivedServices)
            {
                if (entry.ServiceType.ToLower().Equals("drugs"))
                {
                    var medic = db.Medications.Find(entry.ServiceId);
                    medic.WaivedAmount = entry.WaivedAmount;
                    medic.WaiverNo = data.Waiver.Id;
                    medic.Paid = true;
                    db.SaveChanges();

                }
                else
                {
                    var bs = db.BillServices.Find(entry.ServiceId);
                    bs.WaivedAmount = entry.WaivedAmount;
                    bs.WaiverNo = data.Waiver.Id;
                    if (db.SaveChanges() > 0)
                    {
                        new HouseKeeping().AttemptMarkPaid(bs.Id, 0);

                    }
                }
                
            }

            return Json(new { status = "success", message = "Waiver saved successfully!" });
        }

        public class BillAdjustmentData
        {
            public String EntryType { get; set; }
            public int EntryId { get; set; }
            public double Award { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }
        public class ListBillAdjustmentData
        {
            public List<BillAdjustmentData> Adjustments { get; set; }
        }
        [HttpPost]
        public ActionResult SaveBillAdjustment(ListBillAdjustmentData data)
        {
            foreach(var entry in data.Adjustments)
            {
                var log = new BillAdjustmentLog();
                log.UserId = (int)Session["UserId"];
                log.BranchId = 1;
                log.DateAdded = DateTime.Now;

                log.FinalAward = entry.Award;
                log.FinalPrice = entry.Price;
                log.FinalQty = entry.Quantity;

                if (entry.EntryType.ToLower().Trim() == "drugs")
                {
                    var drugBill = db.Medications.Find(entry.EntryId);

                    log.MedicationId = drugBill.Id;
                    var initAward = 0.0;
                    double.TryParse(drugBill.Award.ToString(), out initAward);
                    log.InitialAward = initAward;
                    log.InitialPrice = drugBill.UnitPrice;
                    log.InitialQty = (int)drugBill.QuantityIssued;


                    
                    drugBill.UnitPrice = entry.Price;
                    drugBill.Award = entry.Award;
                    drugBill.QuantityIssued = entry.Quantity;
                    drugBill.Subtotal = ((drugBill.QuantityIssued ?? 0) * drugBill.UnitPrice) -
                        ((drugBill.QuantityIssued ?? 0) * (drugBill.Award??0));

                    
                }else if (entry.EntryType.ToLower().Trim() == "walkin-drugs")
                {
                    var walkinDrug = procDB.Walkings.Find(entry.EntryId);

                    log.WalkinMediCationId = walkinDrug.Id;
                    log.InitialPrice = walkinDrug.UnitPrice;
                    log.InitialQty = walkinDrug.QuantityIssued;

                    walkinDrug.UnitPrice = entry.Price;
                    walkinDrug.QuantityIssued = entry.Quantity;
                    walkinDrug.Subtotal = walkinDrug.QuantityIssued * walkinDrug.UnitPrice;
                    
                    procDB.SaveChanges();

                }
                else
                {
                    var billService = db.BillServices.Find(entry.EntryId);

                    log.BillServiceId = billService.Id;
                    log.InitialAward = billService.Award;
                    log.InitialPrice = billService.Price;
                    log.InitialQty = (int)billService.Quatity;


                    billService.Price = entry.Price;
                    billService.Award = entry.Award;
                    billService.Quatity = entry.Quantity;

                }
                db.BillAdjustmentLogs.Add(log);

                //Check if fully paid
                if ((entry.Award * entry.Quantity) < (entry.Price * entry.Quantity))
                {
                    //if not fully paid set bill paid flag to false
                    var billService = db.BillServices.Find(entry.EntryId);
                    if (billService != null)
                    {
                        billService.Paid = false;
                    }

                }
                else
                {
                    //if fully paid set bill paid flag to true
                    var billService = db.BillServices.Find(entry.EntryId);
                    if (billService != null)
                    {
                        billService.Paid = true;
                    }
                }
                db.SaveChanges();
            }
            return Json(data);
        }


        public ActionResult Adjustment(int? id)
        {
            if(id != null)
            {
                return RedirectToAction("Index", new { id = id });
            }
            else
            {
                return RedirectToAction("BillingQueue");
            }
            
        }

        public ActionResult MPesaGetWay(string msisdn, string businessNo)
        {
            using (var client = new WebClient())
            {
                
                var result = client.DownloadString(string.Format("https://softcom.co.ke/mgateway/facility_query.php?msisdn={0}&businessNo={1}",
                    msisdn, businessNo));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult StrackOutIds(string Ids)
        {
            using (var client = new WebClient())
            {

                var result = client.DownloadString(string.Format("https://softcom.co.ke/mgateway/facility_query.php?confirmIds={0}",
                    Ids));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AdvancedPayment()
        {
            //return Content("This module is not available at the moment.");
            return View();
        }

        public class AdvancedPayentData
        {
            public string PaymentMode { get; set; }
            public int PatientId { get; set; }
            public double ReceivedAmount { get; set; }
        }
        [HttpPost]
        public ActionResult SaveAdvancedPayment(AdvancedPayentData data)
        {
            var wallet = new EWallet();
            wallet.AmountTransacted = data.ReceivedAmount;
            wallet.PatientId = data.PatientId;
            wallet.UserId = (int)Session["UserId"];
            wallet.TimeAdded = DateTime.Now;
            wallet.Direction = 1; //One is deposit
            wallet.Description = "Avanced Payment";
            
            wallet.BranchId = (int)Session["UserBranchId"] ;

            db.EWallets.Add(wallet);
            db.SaveChanges();
            var myWallet = db.EWallets.Where(e => e.PatientId == wallet.PatientId).ToList();
            var bal = (myWallet.Where(e => e.Direction == 1).Sum(e => e.AmountTransacted)
                        - myWallet.Where(e => e.Direction == 0).Sum(e => e.AmountTransacted));
            return Json(new { Status = "success", Message="Avanced Payment Saved Successfully!", EWalletBalance  = bal});
        }

        public ActionResult SearchPatient(string search)
        {
            search = search.ToLower().Trim();
            List<PatientsAutoCompleteData> patients = db.Patients.Where(e => e.RegNumber.Contains(search) ||
            e.FName.Contains(search) || e.MName.Contains(search) || e.LName.Contains(search)).Include(e=>e.EWallets).Select(
                x => new PatientsAutoCompleteData
                {
                    PatientId = x.Id,
                    RegNumber = x.RegNumber,
                    Name = x.Salutation.Trim() + " " + x.FName.Trim() + " " + x.MName.Trim() + " " + x.LName.Trim(),
                    Gender = x.Gender,
                    Age = (DateTime.Now.Year - x.DOB.Value.Year).ToString() +" Yrs" ,
                    HomeAddress = x.HomeAddress

                }).Take(15).ToList();

            var wallets = db.EWallets.ToList();

            foreach(var pat in patients)
            {
                var myWalletTransactions = wallets.Where(e => e.PatientId == pat.PatientId).ToList();
                if (myWalletTransactions.Count()>0)
                {
                    pat.EWalletBalance = (myWalletTransactions.Where(e => e.Direction == 1).Sum(e => e.AmountTransacted) 
                        - myWalletTransactions.Where(e => e.Direction == 0).Sum(e => e.AmountTransacted));
                }
                else
                {
                    pat.EWalletBalance = 0;
                }
            }
            return new JsonResult { Data = patients, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult FilterIpdList(string search)
        {
            search = search.Trim().ToLower();
            var ipdReg = new List<OpdRegister>();

            if (search == "")
            {
                ipdReg = db.OpdRegisters.Where(e => e.IsIPD && !e.Discharged && e.Status.ToLower().Trim() != "closed")
                    .Take(20).OrderByDescending(a => a.Id).ToList();
            }
            else
            {
               ipdReg = db.OpdRegisters.Where(e => e.IsIPD && !e.Discharged && e.Status.ToLower().Trim() != "closed"
            && (e.Patient.FName.StartsWith(search) || e.Patient.MName.StartsWith(search) || e.Patient.LName.StartsWith(search)
            || e.Patient.RegNumber.Contains(search) )).Take(20).OrderByDescending(a => a.Id).ToList();
            }
            
            return PartialView(ipdReg);
        }

        public ActionResult Delete(int id)
        {
            var loggedInUser = db.Users.Find((int)Session["UserId"]);
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
            if (anyUserDeleteItems || billItem.UserId == loggedInUser.Id  || loggedInUser.UserRole.RoleName.ToLower()=="sa" || loggedInUser.UserRole.RoleName.ToLower() == "admin")
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


        public ActionResult Refund(int page = 1)
        {
            var todaysReceipts = db.BillPayments.Where(e => e.DateAdded >= DateTime.Today
            && e.AmountFromWallet == 0 ).ToList();

            ViewBag.MedicalDeps = db.Departments.Where(e => e.IsMedical != null && e.IsMedical.Trim().ToLower().Equals("yes")).ToList();

            int itemsperpage = 15;
            var offset = (itemsperpage * page) - itemsperpage;
            ViewBag.Page = page;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(todaysReceipts.Count() / itemsperpage)));
            return View("ReceiptList", todaysReceipts.Skip(offset).Take(itemsperpage).OrderByDescending(e=>e.Id));
        }

        public ActionResult FilterReceiptList(FilterOPD filterOPD, int page = 1)
        {
            var start = filterOPD.StartDate;
            var end = filterOPD.EndDate.AddDays(1);
            ViewBag.Mode = "AJAX";
            var todaysReceipts = db.BillPayments.Where(e => e.DateAdded >= start && e.DateAdded<end
            && e.AmountFromWallet == 0).ToList();

            if (filterOPD.SearchText != null)
            {
                todaysReceipts = todaysReceipts.Where(e =>
                e.OPDNo.ToString().Contains(filterOPD.SearchText) ||
                (e.OpdRegister.Patient.FName + e.OpdRegister.Patient.LName).Contains(filterOPD.SearchText) ||
                e.Id.ToString().Contains(filterOPD.SearchText)).ToList();
            }
            ViewBag.MedicalDeps = db.Departments.Where
                (e => e.IsMedical != null && e.IsMedical.Trim().ToLower().Equals("yes")).ToList();

            int itemsperpage = 15;
            var offset = (itemsperpage * page) - itemsperpage;
            ViewBag.Page = page;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(todaysReceipts.Count() / itemsperpage)));
            return PartialView("ReceiptList", todaysReceipts.Skip(offset)
                .Take(itemsperpage).OrderByDescending(e => e.Id));
        }


        public ActionResult ConfirmRefund(int id)
        {
            var data = db.BillPayments.Find(id);
            return View(data);
        }

      
        public class RefundFormData
        {
            public int ReceiptNo { get; set; }
            public double RefundedAmount { get; set; }
            public string RefundedMedications{ get; set; }
            public String RefundedServices { get; set; }
            public int RefundedItemsCount { get; set; }
            public String ReasonForRefund { get; set; }
        }

        public ActionResult SaveRefund(RefundFormData data)
        {
            var refund = new Refund();
            refund.ReceiptNo = data.ReceiptNo;
            refund.RefundedAmount = data.RefundedAmount;
            refund.RefundedItemsCount = data.RefundedItemsCount;
            refund.ReasonForRefund = data.ReasonForRefund;
            
            refund.BranchId = (int)Session["UserBranchId"] ; ;
            refund.UserId = (int)Session["UserId"];
            refund.DateAdded = DateTime.Now;
            db.Refunds.Add(refund);
            db.SaveChanges();

            //loop to add all refunded items to refundeditems table

           if(data.RefundedServices !=null)
            {
                var items = data.RefundedServices.Split(',');
                foreach(var i in items)
                {
                    if (i.Trim() != "")
                    {
                        var refundItem = new RefundedItem();
                        refundItem.BillServiceId = Convert.ToInt32(i);
                        refundItem.UserId = (int)Session["UserId"];
                        refundItem.BranchId = (int)Session["UserBranchId"] ; ;
                        refundItem.DateAdded = DateTime.Now;
                        refundItem.RefundId = refund.Id;
                        db.RefundedItems.Add(refundItem);
                        
                    }
                }
                db.SaveChanges();
            }

            if (data.RefundedMedications != null)
            {
                var items = data.RefundedMedications.Split(',');
                foreach (var i in items)
                {
                    if (i.Trim() != "")
                    {
                        var refundItem = new RefundedItem();
                        refundItem.MedicationId = Convert.ToInt32(i);
                        refundItem.UserId = (int)Session["UserId"];
                        refundItem.BranchId = (int)Session["UserBranchId"] ; ;
                        refundItem.DateAdded = DateTime.Now;
                        refundItem.RefundId = refund.Id;
                        db.RefundedItems.Add(refundItem);

                    }
                }
                db.SaveChanges();
            }


            return Json(new { Message = "Refund confirmed successfully. Tracking number "+refund.Id.ToString().PadLeft(4,'0'),
                Status="success", TrackingNumber = refund.Id.ToString().PadLeft(4,'0')}); 
        }


        public ActionResult RefundsList(int page = 1)
        {
            var todaysRefunds = db.Refunds.Where(e => !e.Remitted).OrderByDescending(e=>e.Id).ToList();

            ViewBag.MedicalDeps = db.Departments.Where(e => e.IsMedical != null && e.IsMedical.Trim().ToLower().Equals("yes")).ToList();

            int itemsperpage = 15;
            var offset = (itemsperpage * page) - itemsperpage;
            ViewBag.Page = page;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(todaysRefunds.Count() / itemsperpage)));
            return View( todaysRefunds.Skip(offset).Take(itemsperpage).OrderByDescending(e => e.Id));
        }

        public ActionResult RemitRefund(int id)
        {
            var loggedInUser = db.Users.Find((int)Session["UserId"]);

            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);

            if (shift == null)
            {
                return RedirectToAction("StartShift");
            }

            ViewBag.AcceptRemit = true;

            if (shift.StartTime.Date == DateTime.Today 
                && (shift.BillPayments.Sum(e => e.PaidAmount) - shift.Refunds.Sum(e => e.RefundedAmount)) <
                db.Refunds.Find(id).RefundedAmount)
            {
                ViewBag.AcceptRemit = false;
                ViewBag.DeclineRemitReason = "You have limited funds to perform the transaction";
            }

            ViewBag.ShiftId = shift.Id;
            ViewBag.FacilityName = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Trim() == "facilityname").Value;
            ViewBag.FacilityAddress = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Trim() == "facilityaddress").Value;
            ViewBag.FacilityContact = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Trim() == "facilitycontactnumber").Value;
            return View(db.Refunds.Find(id));
        }


        [HttpPost]
        public ActionResult ConfirmRemitRefund(int id)
        {
            var loggedInUser = db.Users.Find((int)Session["UserId"]);

            var shift = loggedInUser.Shifts.FirstOrDefault(e => e.Endtime == null);


            if (shift == null)
            {
                return Json(new { Mesage = "Unauthorized request. Please start a new shift to continue.",  Status = "danger"});
            }

            var refund = db.Refunds.Find(id);
            refund.Remitted = true;
            refund.RemittedAt = DateTime.Now;
            refund.ShiftId = shift.Id;
            db.SaveChanges();


            return Json(new {
                Message = "Refund Confirmed Successfully! Please issue refund amount to customer.",
                Status = "success",
                RemittedBy = shift.User.Username,
                RemittedOn = refund.RemittedAt.Value.ToString("dd-MMM-yy hh:mm:s tt")
            });
        }


        //Kogi Code
        public ActionResult IpdInvoice(int id)
        {
            var data = db.OpdRegisters.Find(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult IpdInvoice()
        {
            return View();
        }

        public ActionResult GetJPayments(int id)
        {
            //new JPayController().Get((int)id);

            var jp = db.JPayments.Where(e => e.OPDNo.Equals(id)).ToList();

            if (jp.Count() > 0)
            {
                return Json(jp.Sum(e => e.Amount), JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);

        }

        public ActionResult downloadIpdInvoice(string type, int id)
        {
            var dataset = new DSIpdInvoice();
            var i = 1;
            var TTBal = 0.0;

            var opd = db.OpdRegisters.Find(id);

            dataset.Patient.AddPatientRow(
                (opd.Patient.FName + " " + opd.Patient.MName + " " + opd.Patient.LName).ToUpper(),
                opd.Tariff.Company.CompanyName.ToUpper(),
                opd.Tariff.TariffName.ToUpper(),
                opd.Patient.RegNumber,
                type.ToUpper()
                );


            foreach (var data in opd.BillServices.ToList())
            {
                var Quantity = data.Quatity;
                var Price = data.Price;
                var WaivedAmount = data.WaivedAmount ?? 0;
                var Award = data.Award * data.Quatity;
                var Paid = data.BillPayment?.PaidAmount ?? 0;
                var PP = db.IPDBillPartialPayments.FirstOrDefault(f => f.BillServiceId == data.Id)?.AllocatedAmount??0;
                var SubTotal = (Price * Quantity);
                var Bal = (Price * Quantity) - (Award + WaivedAmount + PP);
                TTBal += Bal;


                dataset.IpdInvoice.AddIpdInvoiceRow(
                    i.ToString(),
                    data.Service.Department.DepartmentName.ToUpper(),
                    data.Service.ServiceName.ToUpper(),
                    Quantity.ToString(),
                    Price.ToString(),
                    SubTotal.ToString(),
                    Award.ToString(),
                    WaivedAmount.ToString(),
                    (Paid + PP),
                    Bal
                );
                i++;
            }

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/IpdInvoice/RPTIpdInvoice.rpt")));
            Rd.SetDataSource(dataset);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = type + "IPD Invoice" + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
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