using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Caresoft2._0.Areas.MedicalStore.ViewModels;
using CaresoftHMISDataAccess;
using PagedList;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Caresoft2._0.Areas.Procurement.ViewModel;

namespace Caresoft2._0.Areas.PharmacyModule.Controllers
{
    [Auth]
    public class PharmacyPatientTransactionController : Controller
    {
        private ProcurementDbContext db = new ProcurementDbContext();
        private CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: PharmacyModule/PharmacyPatientTransaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchPatientIssueVoucher(int? page)
        {
            if(!page.HasValue)
            {
                Session["StartDate"] = null;
                Session["ToDate"] = null;
            }

            ViewBag.StartDate = Session["StartDate"];
            ViewBag.ToDate = Session["ToDate"];



            List<int> pharmacyPatients = new List<int>();

            if(Session["StartDate"]!=null && Session["ToDate"]!=null )
            {
                var fDate = (DateTime)Session["StartDate"];
                var tDate = (DateTime)Session["ToDate"];
                
                //here we add one day cause entity framework considers even time 
                tDate = tDate.AddDays(1);

                var pts = db2.Medications.Include(x => x.OpdRegister)
                                                  .Where(p => p.TimeAdded > fDate && p.TimeAdded <= tDate)
                                                  .Select(p => new { p.OPDNo })
                                                  .Distinct()
                                                  .OrderByDescending(p => p.OPDNo)
                                                  .ToList();
                foreach (var item in pts)
                {
                    pharmacyPatients.Add(item.OPDNo);
                }



            }
            else
            {
                var todaysDate = DateTime.Now.Date;
           
                var pts = db2.Medications.Include(x => x.OpdRegister)
                                                    .Where(p => p.TimeAdded >= todaysDate && p.Issued ==false)
                                                    .Select(p => new { p.OPDNo })
                                                    .Distinct()
                                                    .OrderByDescending(p => p.OPDNo)
                                                    .ToList();
                foreach (var item in pts)
                {
                    pharmacyPatients.Add(item.OPDNo);
                }
            }
            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            simulationPatientIssueVoucher = GetListOfPatientsWithDetails(pharmacyPatients);

            //now we add the walk ins 
            var todaysDat = DateTime.Now.Date;
            //todaysDat = todaysDat.AddDays(-1);

            var dataFromWalkIns = db.Walkings.Where(p => p.TimeAdded > todaysDat && p.Issued == false).OrderByDescending(p=>p.PatientIdentifierId).ToList();

            //select distinct Ids
            var distinctIds = dataFromWalkIns.DistinctBy(p => p.PatientIdentifierId).Select(p => p.PatientIdentifierId).ToList();
            List<SimulationPatientIssueVoucher> datFromWalkIns = new List<SimulationPatientIssueVoucher>();

            foreach (var item in distinctIds)
            {
                SimulationPatientIssueVoucher voucher = new SimulationPatientIssueVoucher();

                var thatWalkInPt = db.Walkings.Where(p => p.PatientIdentifierId == item).ToList();

                var isPaid = false;
                var isPatiallyPaid = false;

                if (thatWalkInPt.All(p => p.Paid == true))
                {
                    isPaid = true;
                }
                else if (thatWalkInPt.Any(p => p.Paid == true))
                {
                    isPatiallyPaid = true;
                }

                voucher.PatientsName = thatWalkInPt.FirstOrDefault().PatientsName;
                voucher.Opd = default;
                voucher.Regno = "Walk in Patient #" + item;
                voucher.TransactionId = item;
                voucher.Id = item;
                voucher.HivNo = default;
                voucher.Ipd = default;
                voucher.totalAmount = thatWalkInPt.Sum(p => p.Subtotal);
                voucher.isPaid = isPaid;
                voucher.IsWalkIn = true;
                voucher.isPatiallyPaid = isPatiallyPaid;

                datFromWalkIns.Add(voucher);
            }

            simulationPatientIssueVoucher.AddRange(datFromWalkIns);


            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            return View(simulationPatientIssueVoucher.ToPagedList(pageIndex,pageSize));
        }

        public struct Dates
        {
            public string fromDate { get; set; }
            public string toDate { get; set; }
        }
        //search Medications Using Dates
        public ActionResult SearchByDatesInvoice(Dates dates)
        {

            Session["StartDate"] = dates.fromDate;
            Session["EndDate"] = dates.toDate;

            Session["SupplierName"] = null;
            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);
            tDate = tDate.AddDays(1);

            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            ViewBag.fDate = fDate;
            ViewBag.tDate = tDate;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            List<int> pharmacyPatients = new List<int>();

            var pts = db2.Medications.Include(x => x.OpdRegister)
                                                 .Where(p => p.TimeAdded >= fDate && p.TimeAdded <= tDate)
                                                 .Select(p => new { p.OPDNo })
                                                 .Distinct()
                                                 .OrderByDescending(p => p.OPDNo)
                                                 .ToList();
            foreach (var item in pts)
            {
                pharmacyPatients.Add(item.OPDNo);
            }


            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();
            //run the method to get the list of patients
            simulationPatientIssueVoucher = GetListOfPatientsWithDetails(pharmacyPatients);


            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchPatientVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(pageindex, pagesize));
        }

        public async Task<ActionResult> SearchPatient(string search)
        {
            
            var search2 = Regex.Replace(search, @"\s+", "");

            db2.Configuration.LazyLoadingEnabled = false;
            var data = db2.Patients.Where(e => e.RegNumber.Contains(search) || e.FName.Contains(search) ||
            e.LName.Contains(search) || e.MName.Contains(search) || e.Mobile.Contains(search) ||
            e.NationalId.Contains(search) || e.Salutation.Contains(search) || e.Email.Contains(search) ||
            e.HomeAddress.Contains(search) || (search2.Contains(e.FName) && search2.Contains(e.LName))
            || (search2.Contains(e.FName) && search2.Contains(e.MName)) || (search2.Contains(e.MName) 
            && search2.Contains(e.LName)))
            .Select(d => new
            {
                Name = d.FName + " " + d.MName + " " + " " + d.LName + " ",
                RegNumber = d.RegNumber,
                NationalId = d.NationalId,
                Mobile = d.Mobile,
                Id = d.Id,

            }).OrderByDescending(e => e.Id).Take(30).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //search patient medication using Registration Number
        public ActionResult SearchMedicationByPatientRegNo(string regNo)
        {
            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var patients = db2.Medications.Where(reg => reg.OpdRegister.Patient.RegNumber.Equals(regNo))
                .Select(e => e.OpdRegister.Patient).ToList();

            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            if (patients == null || patients.Count() == 0)
            {
                return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchPatientVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(pageindex, pagesize));

            }


            //var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Select(p => p.OPDNo).Distinct().ToList();
            var pharmacyPatients = patients.FirstOrDefault().OpdRegisters.Select(p => p.Id).Distinct().ToList();

           
            simulationPatientIssueVoucher = GetListOfPatientsWithDetails(pharmacyPatients);

            Session["regNo"] = regNo;
            
            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchPatientVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(pageindex, pagesize));
        }

        //search patients medication by opd number
        public ActionResult SearchPatientByOpdOrIpdNumber(int OpdNo)
        {
            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;


            var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();

            var pharmacyPatients = new List<int>();
            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            if(DistinctPersonFromMedication!=null)
            {
                pharmacyPatients.Add(DistinctPersonFromMedication.Id);
                simulationPatientIssueVoucher = GetListOfPatientsWithDetails(pharmacyPatients);
            }

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchPatientVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(pageindex, pagesize));
        }

        public ActionResult EditPatientIssueVoucher(int? page)
        {
            int PageSize = 10;
            int PageNumber = 1;

            PageNumber = page.HasValue ? page.Value : 1;

            var todaysDate = DateTime.Now.Date;
            int transactionId = 1;

            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            #region Medications for Registered Patients

            var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Where(p => p.TimeAdded >= todaysDate && p.Issued == true).Select(p => p.OPDNo).Distinct().ToList();

            foreach (var OpdNo in pharmacyPatients)
                {
                    var DistinctPersonFromMedication = db2.OpdRegisters
                                                          .Include(p => p.Patient)
                                                          .Include(p => p.BillPayments)
                                                          .Where(p => p.Id == OpdNo)
                                                          .FirstOrDefault();


                var discount = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().Discount.HasValue ?(double)db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().Discount.Value : 0;
                var billPaid = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillPayment;
                var paidAmount = billPaid == null ? 0 : billPaid.PaidAmount;
                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();


                simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                simulationPatientIssue.Opd = OpdNo.ToString();
                simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                simulationPatientIssue.TransactionId = transactionId;
                simulationPatientIssue.Id = OpdNo;
                simulationPatientIssue.HivNo = default;
                simulationPatientIssue.Ipd = default;
                simulationPatientIssue.VoucherNumber = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillNo.ToString();
                simulationPatientIssue.DateIssued = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().TimeAdded;
                simulationPatientIssue.totalAmount = db2.Medications.Where(p => p.OPDNo == OpdNo).Sum(p => p.Subtotal);
                simulationPatientIssue.PaidAmount = paidAmount;
                simulationPatientIssue.Discount = discount;
                

                simulationPatientIssueVoucher.Add(simulationPatientIssue);
                transactionId++;
                }

            #endregion

            #region WalkIn Patient Details 

            var walkins = db.Walkings.Where(p => p.TimeAdded >= todaysDate && p.Issued == true)
                                     .Select(p => p.PatientIdentifierId)
                                     .Distinct().ToList();

            foreach (var item in walkins)
            {
                var WalkInPatientDetails = db.Walkings.Where(p => p.PatientIdentifierId == item).ToList();
                
                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();

                simulationPatientIssue.VoucherNumber = item.ToString();
                simulationPatientIssue.DateIssued = WalkInPatientDetails.FirstOrDefault().TimeAdded;
                simulationPatientIssue.Regno = "#"+item;
                simulationPatientIssue.Opd = default;
                simulationPatientIssue.HivNo = default;
                simulationPatientIssue.ClinicNo = default;
                simulationPatientIssue.PatientsName = WalkInPatientDetails.FirstOrDefault().PatientsName;
                simulationPatientIssue.totalAmount = WalkInPatientDetails.Sum(p => p.Subtotal);
                simulationPatientIssue.PaidAmount = WalkInPatientDetails.Sum(p => p.Subtotal);
                simulationPatientIssue.Discount = 0;
                simulationPatientIssue.IsWalkIn = true;
                simulationPatientIssue.TransactionId = transactionId;

                simulationPatientIssueVoucher.Add(simulationPatientIssue);
                transactionId++;
            }
            #endregion

            var patientsData = new List<SimulationPatientIssueVoucher>();

            patientsData = simulationPatientIssueVoucher.OrderByDescending(p => p.TransactionId).ToList();


            return View(patientsData.ToPagedList(PageNumber,PageSize));

        }

        //Search Vouchers for editing using Dates
        public ActionResult SearchEditPatientIssuerVoucher(Dates dates)
        {
            int PageSize = 10;
            int PageNumber = 1;

            var fromDate = Convert.ToDateTime(dates.fromDate);
            var toDate = Convert.ToDateTime(dates.toDate);
            var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Where(p=>p.TimeAdded>=fromDate &&p.TimeAdded<=toDate).Select(p => p.OPDNo).Distinct().ToList();
            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {
                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Include(p => p.Patient)
                                                      .Include(p => p.BillPayments)
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();


                var discount = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().Discount.HasValue ? db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().Discount.Value : 0;
                var billPaid = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillPayment;
                var paidAmount = billPaid == null ? 0 : billPaid.PaidAmount;
                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();


                simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                simulationPatientIssue.Opd = OpdNo.ToString();
                simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                simulationPatientIssue.TransactionId = 0;
                simulationPatientIssue.Id = OpdNo;
                simulationPatientIssue.HivNo = default;
                simulationPatientIssue.Ipd = default;
                simulationPatientIssue.VoucherNumber = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillNo.ToString();
                simulationPatientIssue.DateIssued = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().TimeAdded;
                simulationPatientIssue.totalAmount = db2.Medications.Where(p => p.OPDNo == OpdNo).Sum(p => p.Subtotal);
                simulationPatientIssue.PaidAmount = paidAmount;
                simulationPatientIssue.Discount = discount;

                simulationPatientIssueVoucher.Add(simulationPatientIssue);
            }

            Session["EditIssueVoucherFromDate"] = fromDate;
            Session["EditIssueVoucherToDate"] = toDate;

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_EditPatientIssueVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(PageNumber, PageSize));
        }

        public ActionResult SearchEditPatientIssueVoucherByRegNo(string regNo)
        {
            int PageSize = 10;
            int PageNumber = 1;


            var patients = db2.Patients.Where(reg => reg.RegNumber.Contains(regNo)).Include(p => p.OpdRegisters).ToList();

            //var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Select(p => p.OPDNo).Distinct().ToList();
            var pharmacyPatients = patients.FirstOrDefault().OpdRegisters.Select(p => p.Id).Distinct().ToList();

            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {
                
                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Include(p => p.Patient)
                                                      .Include(p => p.BillPayments)
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();

                var getDiscountForPatient = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault();
                var discount = getDiscountForPatient == null ? 0 : getDiscountForPatient.Discount.HasValue ? getDiscountForPatient.Discount.Value : 0;
                var billPaid = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault() == null ? null: db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillPayment;
                var paidAmount = billPaid == null ? 0 : billPaid.PaidAmount;

                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();

                var medicationSelected = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault();

                //Here w perform a check to see if the patient has been treated by checking medications
               
                if(medicationSelected == null)
                {
                    //medication being null insinuates that a patient has not been treated. 
                }
                else
                {
                    simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                    simulationPatientIssue.Opd = OpdNo.ToString();
                    simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                    simulationPatientIssue.TransactionId = 0;
                    simulationPatientIssue.Id = OpdNo;
                    simulationPatientIssue.HivNo = default;
                    simulationPatientIssue.Ipd = default;
                    simulationPatientIssue.VoucherNumber =  medicationSelected.BillNo.ToString();
                    simulationPatientIssue.DateIssued = medicationSelected.TimeAdded;
                    simulationPatientIssue.totalAmount = db2.Medications.Where(p => p.OPDNo == OpdNo).Sum(p => p.Subtotal);
                    simulationPatientIssue.PaidAmount = paidAmount;
                    simulationPatientIssue.Discount = discount;

                    simulationPatientIssueVoucher.Add(simulationPatientIssue);
                }
               
            }

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_EditPatientIssueVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(PageNumber, PageSize));

        }

        /* Region for Cancelled Issue Voucher*/
        public ActionResult SearchCanceledIssueVoucher(int? page)
        {
            int pageSize = 10;
            int currentPage = 1;
            //use of coalesce operator(??) it returns the returns left hand operand if not null else return right hand operator
            currentPage = page ?? 1;
            
            var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Where(p=>p.TimeAdded == DateTime.Now).Select(p => p.OPDNo).Distinct().ToList();

            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {

                List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

                var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();

                foreach (var dat in dataFromMedication)
                {
                    var itemMaster = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();

                    SimulationTreatment simulation = new SimulationTreatment()
                    {
                        ItemMaster = itemMaster,
                        Rate = (int)dat.UnitPrice,
                        units = dat.Quantity,
                        Amount = dat.UnitPrice * dat.Quantity,
                        NoOfDays = dat.Day,
                        Description = dat.Notes

                    };

                    LstSimulationTreatment.Add(simulation);
                }




                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Include(p => p.Patient)
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();

           


                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher()
                {
                    PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName,
                    Opd = OpdNo.ToString(),
                    Regno = DistinctPersonFromMedication.Patient.RegNumber,
                    TransactionId = 0,
                    Id = OpdNo,
                    HivNo = default,
                    Ipd = default,
                    SimulationTreatment = LstSimulationTreatment
                };


               

                simulationPatientIssueVoucher.Add(simulationPatientIssue);
            }

            return View(simulationPatientIssueVoucher.ToPagedList(currentPage,pageSize));
        }

        //Search cancelled Vouchers using Dates
        public ActionResult SearchCancelPatientIssueVoucherByDates(Dates dates)
        {
            int PageSize = 10;
            int PageNumber = 1;

            var fromDate = Convert.ToDateTime(dates.fromDate);
            var toDate = Convert.ToDateTime(dates.toDate);
            var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Where(p => p.TimeAdded >= fromDate && p.TimeAdded <= toDate).Select(p => p.OPDNo).Distinct().ToList();
            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {
                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Include(p => p.Patient)
                                                      .Include(p => p.BillPayments)
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();


                var discount = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().Discount.HasValue ? db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().Discount.Value : 0;
                var billPaid = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillPayment;
                var paidAmount = billPaid == null ? 0 : billPaid.PaidAmount;
                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();


                simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                simulationPatientIssue.Opd = OpdNo.ToString();
                simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                simulationPatientIssue.TransactionId = 0;
                simulationPatientIssue.Id = OpdNo;
                simulationPatientIssue.HivNo = default;
                simulationPatientIssue.Ipd = default;
                simulationPatientIssue.VoucherNumber = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillNo.ToString();
                simulationPatientIssue.DateIssued = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().TimeAdded;
                simulationPatientIssue.totalAmount = db2.Medications.Where(p => p.OPDNo == OpdNo).Sum(p => p.Subtotal);
                simulationPatientIssue.PaidAmount = paidAmount;
                simulationPatientIssue.Discount = discount;

                simulationPatientIssueVoucher.Add(simulationPatientIssue);
            }

            Session["EditIssueVoucherFromDate"] = fromDate;
            Session["EditIssueVoucherToDate"] = toDate;

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchCancelIssuedVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(PageNumber, PageSize));
        }

        //search cancelled vouchers using patients name
        public ActionResult SearchCancelPatientIssueVoucherByPatientsName(string patientsName)
        {
            int PageSize = 10;
            int PageNumber = 1;

            var patients = db2.Patients.Where(reg => reg.RegNumber.Contains(patientsName)).Include(p => p.OpdRegisters).ToList();

            //var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Select(p => p.OPDNo).Distinct().ToList();
            var pharmacyPatients = patients.FirstOrDefault().OpdRegisters.Select(p => p.Id).Distinct().ToList();

            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {

                List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

                var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();

                foreach (var dat in dataFromMedication)
                {
                    var itemMaster = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();

                    SimulationTreatment simulation = new SimulationTreatment()
                    {
                        ItemMaster = itemMaster,
                        Rate = (int)dat.UnitPrice,
                        units = dat.Quantity,
                        Amount = dat.UnitPrice * dat.Quantity,
                        NoOfDays = dat.Day,
                        Description = dat.Notes

                    };

                    LstSimulationTreatment.Add(simulation);
                }

                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Include(p => p.Patient)
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();

                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher()
                {
                    PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName,
                    Opd = OpdNo.ToString(),
                    Regno = DistinctPersonFromMedication.Patient.RegNumber,
                    TransactionId = 0,
                    Id = OpdNo,
                    HivNo = default,
                    Ipd = default,
                    SimulationTreatment = LstSimulationTreatment
                };

                simulationPatientIssueVoucher.Add(simulationPatientIssue);
            }

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchCancelIssuedVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(PageNumber, PageSize));
        }

        public struct pt
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        //this method returns a list of patients 
        public ActionResult SearchPatientName(string patientsName)
        {
            var patientNames = db2.Patients.Where(p => p.FName.Contains(patientsName) || p.LName.Contains(patientsName) || p.MName.Contains(patientsName))
                                           .Select(p=>new { p.FName,p.MName,p.LName,p.Id})
                                           .ToList();

            List<pt> patients = new List<pt>();

            foreach (var item in patientNames)
            {
                pt pat = new pt()
                {
                    Name = item.FName + " " + item.MName + " " + item.LName,
                    Id = item.Id
                };

                patients.Add(pat);
            }

            return Json(patients, JsonRequestBehavior.AllowGet);
        }

        //when a patient is searched by name, we return the id from them then search using this method
        public ActionResult SearchCancelledIssueVoucherByPatient(int Id)
        {
            int PageSize = 10;
            int PageNumber = 1;


            //var patients = db2.Patients.Where(p=>p.Id == Id).Include(p => p.OpdRegisters).FirstOrDefault();
            var patients = db2.Patients.Where(p => p.Id == Id).Include(p => p.OpdRegisters).ToList();

            //var pharmacyPatients = db2.Medications.Include(x => x.OpdRegister).Select(p => p.OPDNo).Distinct().ToList();
            var pharmacyPatients = patients.FirstOrDefault().OpdRegisters.Select(p => p.Id).Distinct().ToList();

            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {

                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Include(p => p.Patient)
                                                      .Include(p => p.BillPayments)
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();

                var getDiscountForPatient = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault();
                var discount = getDiscountForPatient == null ? 0 : getDiscountForPatient.Discount.HasValue ? getDiscountForPatient.Discount.Value : 0;
                var billPaid = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault() == null ? null : db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault().BillPayment;
                var paidAmount = billPaid == null ? 0 : billPaid.PaidAmount;

                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();

                var medicationSelected = db2.Medications.Where(p => p.OPDNo == OpdNo).FirstOrDefault();

                //Here w perform a check to see if the patient has been treated by checking medications

                if (medicationSelected == null)
                {
                    //medication being null insinuates that a patient has not been treated. 
                }
                else
                {
                    simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                    simulationPatientIssue.Opd = OpdNo.ToString();
                    simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                    simulationPatientIssue.TransactionId = 0;
                    simulationPatientIssue.Id = OpdNo;
                    simulationPatientIssue.HivNo = default;
                    simulationPatientIssue.Ipd = default;
                    simulationPatientIssue.VoucherNumber = medicationSelected.BillNo.ToString();
                    simulationPatientIssue.DateIssued = medicationSelected.TimeAdded;
                    simulationPatientIssue.totalAmount = db2.Medications.Where(p => p.OPDNo == OpdNo).Sum(p => p.Subtotal);
                    simulationPatientIssue.PaidAmount = paidAmount;
                    simulationPatientIssue.Discount = discount;

                    simulationPatientIssueVoucher.Add(simulationPatientIssue);
                }

            }
            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_SearchCancelIssuedVoucher.cshtml", simulationPatientIssueVoucher.ToPagedList(PageNumber, PageSize));

        }

        public ActionResult CancelIssuedVoucher(int Id)
        {
            int id = Id;

            Session["CancelledIssueVoucher"] = id;

            int OpdNo = id;
            var DistinctPersonFromMedication = db2.OpdRegisters
                                               .Include(p => p.Patient)
                                               .Where(p => p.Id == OpdNo)
                                               .FirstOrDefault();
            var age = DateTime.Now.Year - ((DateTime)DistinctPersonFromMedication.Patient.DOB).Year;

            SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher()
            {
                PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName,
                Opd = OpdNo.ToString(),
                Regno = DistinctPersonFromMedication.Patient.RegNumber,
                TransactionId = 0,
                Id = OpdNo,
                DoctorsName = DistinctPersonFromMedication.ConsultantDoctor,
                Age = age.ToString(),
                Gender = DistinctPersonFromMedication.Patient.Gender,
                DateIssued = DistinctPersonFromMedication.TimeAdded
            };

            List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

            var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();

            foreach (var dat in dataFromMedication)
            {
                var itemMaster = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();

                SimulationTreatment simulation = new SimulationTreatment()
                {
                    ItemMaster = itemMaster,
                    Rate = (int)dat.UnitPrice,
                    units = dat.Quantity,
                    Amount = dat.UnitPrice * dat.Quantity,
                    NoOfDays = dat.Day,
                    Description = dat.Notes

                };

                LstSimulationTreatment.Add(simulation);
            }

            simulationPatientIssue.SimulationTreatment = LstSimulationTreatment;

            
            return View(simulationPatientIssue);
        }

        public ActionResult SimulationTreatmentToBeCancelled(int Id)
        {
            //var data = db.SimulationTreatment.Where(p => p.Id == Id).FirstOrDefault();

            //db.Entry(data).Reference(p => p.ItemMaster).Load();

            int OpdNo = Convert.ToInt32(Session["CancelledIssueVoucher"]);

            var medication = db2.Medications.Where(p => p.OPDNo == OpdNo && p.DrugId == Id).FirstOrDefault();
            var drugInfo = db.ItemMaster.Where(p => p.Id == Id).FirstOrDefault();

            SimulationTreatment simulationTreatment = new SimulationTreatment()
            {
                ItemMaster = drugInfo,
                Rate = (int)medication.UnitPrice,
                units = medication.QuantityIssued.Value,
                Amount = medication.Subtotal,
                ItemMasterId = drugInfo.Id,
            };


            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_LstCancelledSimulatedIssuedVoucher.cshtml", simulationTreatment);
        }

        public struct RefundData
        {
            //This struct Holds data for refund
            public int Id { get; set; }
            public int ItemRefund { get; set; }
            public int totalRefund { get; set; }

        }

        public ActionResult SaveCancelledTreatment(RefundData refundData)
        {

            int OpdNo = Convert.ToInt32(Session["CancelledIssueVoucher"]);
            var medication = db2.Medications.Where(p => p.OPDNo == OpdNo && p.DrugId == refundData.Id).FirstOrDefault();

            if(medication == null)
            {
                return Json("null",JsonRequestBehavior.AllowGet);
            }
            else
            {
                MedicationRefund medicationRefund = new MedicationRefund()
                {
                    ItemMasterId = medication.DrugId.Value,
                    PatientId = medication.OPDNo,
                    RefundAmount = refundData.totalRefund,
                    RefundQty = refundData.ItemRefund,
                    Rate = (int)medication.UnitPrice,
                    RefundDate = DateTime.Now,
                    VoucherNo = medication.OPDNo,
                };
                db.MedicationRefund.Add(medicationRefund);
                db.SaveChanges();

                //Here we update the quantity issued in medication table
                medication.QuantityIssued = medication.QuantityIssued - refundData.ItemRefund;
                db2.Entry(medication).State = EntityState.Modified;
                db2.SaveChanges();


                // here we update the current stock of the drug, we add refunded items 
                var item = db.ItemMaster.Where(p => p.Id == medication.DrugId).FirstOrDefault();
                item.CurrentStock = item.CurrentStock + refundData.ItemRefund;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

            }


            SimulationTreatment simulationTreatment = null;
            
            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_LstCancelledSimulatedIssuedVoucher.cshtml", simulationTreatment);
        }

        public ActionResult CancelVoucherReqFromNurse()
        {
            return View();
        }

        public ActionResult ItemRefund()
        {
            var data = db.MedicationRefund.Include(p => p.ItemMaster)
                                          .OrderByDescending(p => p.Id)
                                          .ToArray();
            return View(data);
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

            var stocksAdjusted = db.StockAdjusted.Where(p=>p.ItemMaster.StoreName=="P" && 
            DbFunctions.TruncateTime(p.DateAdjusted) >= DbFunctions.TruncateTime(StartTime) && 
            DbFunctions.TruncateTime(p.DateAdjusted) <= DbFunctions.TruncateTime(EndTime)).
            Include(p=>p.ItemMaster).ToArray();

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
                var data = db.ItemMaster.Where(p =>
                (p.Drug != null && (p.Drug.IsActive == null || (bool)p.Drug.IsActive)) &&
                p.ItemName.Contains(name) && p.StoreName =="P").DistinctBy(f => f.ItemName).
                    Select(p => new { p.ItemName, Id = p.DrugId }).Take(20).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchBatchNumbers(int id)
        {
            var data = db.ItemMaster.Where(p => p.DrugId ==(id) && p.StoreName == "P").DistinctBy(f => f.BatchNo).Select(p => new { p.BatchNo }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemDetails(AdjustmentStockData stockData)
        {
            var data = db.ItemMaster.Where(p => p.StoreName == "P" && p.ItemName == stockData.itemName && p.BatchNo == stockData.batchNo).Select
                (x => new { x.CurrentStock, x.CostPriceUnit, x.Id }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemDetailsMS(AdjustmentStockData stockData)
        {
            var data = db.ItemMaster.Where(p => p.StoreName == "MS" && p.ItemName == stockData.itemName && p.BatchNo == stockData.batchNo).Select
                (x => new { x.CurrentStock, x.CostPriceUnit, x.Id }).FirstOrDefault();
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
                Department = "P"
            });


            db.SaveChanges();

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_tblAdjustmentStock.cshtml", item);
        }

        public ActionResult DirectPatientIssueVoucher(int? Id)
        {
            ViewBag.Doses = db.Dose.ToList();
            if (Id.HasValue)
            {

                var medz = db2.Medications.Where(p => p.OPDNo == Id.Value).ToList();

                //this is to distinguish the patients viewed by pharmacy and those not
                foreach (var item in medz)
                {
                    item.IsFromDoctor = false;
                    db2.Entry(item).State = EntityState.Modified;
                }
                db2.SaveChanges();
                //end of region


                SimulationData simulationData = new SimulationData();

                #region Get Patient Information ie name, age etc
                int OpdNo = Id.Value;
                var DistinctPersonFromMedication = db2.OpdRegisters
                                                   .Include(p => p.Patient)
                                                   .Where(p => p.Id == OpdNo)
                                                   .FirstOrDefault();
                
                var age = DateTime.Now.Year - (DistinctPersonFromMedication.Patient.DOB.Value).Year;

                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher()
                {
                    PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName,
                    Opd = OpdNo.ToString(),
                    Regno = DistinctPersonFromMedication.Patient.RegNumber,
                    TransactionId = 0,
                    Id = OpdNo,
                    DoctorsName = DistinctPersonFromMedication.ConsultantDoctor,
                    Age = age.ToString(),
                    Gender = DistinctPersonFromMedication.Patient.Gender,
                    DateIssued = DistinctPersonFromMedication.TimeAdded
                };

                simulationData.SimulationPatientIssueVoucher = simulationPatientIssue;

                #endregion

                List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

                var patientsMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();
                var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();
                //here we check whether all the drugs in medication are in item Master
                bool CheckIfAllDrugsAreInItemMaster = true;

                foreach (var item in patientsMedication)
                {
                    //var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                    //var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                    //if (drugToBeChecked == null)
                    //{
                    //    CheckIfAllDrugsAreInItemMaster = false;
                    //}

                    if (item.Available == null || item.Available.Value == false)
                    {
                        CheckIfAllDrugsAreInItemMaster = false;
                    }

                }

                if (CheckIfAllDrugsAreInItemMaster == true)
                {
                    LstSimulationTreatment.AddRange(GetAllDrugsInItemMaster(patientsMedication));
                }
                else
                {
                    //select only drugs in the item Master and separate them from the others
                    var medicationInItemMaster = new List<Medication>();
                    var medicationNotInItemMaster = new List<Medication>();
                    var medicationNotInDrugMaster = new List<Medication>();

                    foreach (var item in patientsMedication)
                    {
                        if (item.DrugId==null || item.DrugId==0)
                        {

                            medicationNotInItemMaster.Add(item);

                        }
                        else
                        {
                            //add if statement such that if item Available is true it searches a differently
                            if (item.Available == true)
                            {
                                var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                                if (drugToBeChecked != null)
                                {
                                    medicationInItemMaster.Add(item);
                                }
                                else
                                {
                                    medicationNotInItemMaster.Add(item);
                                }
                            }
                            else
                            {
                                var drugToBeChecked = db.ItemMaster.Where(p => p.DrugId == item.DrugId).FirstOrDefault();

                                if (drugToBeChecked != null)
                                {
                                    medicationInItemMaster.Add(item);
                                }
                                else
                                {
                                    medicationNotInItemMaster.Add(item);
                                }
                            }

                        }



                    }

                    //so now i have two lists 

                    //now for the drugs not in drug master
                    foreach (var dat in medicationNotInItemMaster)
                    {
                        SimulationTreatment simulation = new SimulationTreatment();

                        ItemMaster master = new ItemMaster()
                        {
                            ItemName = dat.DrugName
                        };

                        simulation.ItemMaster = master;
                        simulation.Rate = (int)dat.UnitPrice;
                        simulation.units = dat.QuantityIssued??default;
                        simulation.Amount = dat.Subtotal;
                        simulation.NoOfDays = dat.Day;
                        simulation.Description = dat.Notes;
                        simulation.Id = dat.Id;
                        simulation.Available = false;
                        simulation.isPaid = false;
                        simulation.isIssued = dat.Issued;
                        simulation.DrugIsNotInHospitalDataBase = true;
                        LstSimulationTreatment.Add(simulation);
                    }

                    //now for the drugs that are in item Master
                    var allDrugsInItemMaster = GetAllDrugsInItemMaster(medicationInItemMaster);
                    LstSimulationTreatment.AddRange(allDrugsInItemMaster);

                }
 
                simulationData.LstSimulationTreatment = LstSimulationTreatment;
               
                simulationData.opdRegister = db2.OpdRegisters.Find(int.Parse(simulationData.SimulationPatientIssueVoucher.Opd));
                return View(simulationData);
            }

            SimulationData data = new SimulationData()
            {
                SimulationPatientIssueVoucher = new SimulationPatientIssueVoucher(),
                LstSimulationTreatment = new List<SimulationTreatment>()
            };

            if (Session["DrugsForWalkInPatient"] != null)
            {
                data.LstSimulationTreatment = (List<SimulationTreatment>)Session["DrugsForWalkInPatient"];

            }
            if (Session["IssuedItems"] != null)
            {
                ViewBag.itemIssued = "True";
            }

            Session["IssuedItems"] = null;
            ViewBag.WalkInPatient = true;

            return View(data);
        }

        public ActionResult DirectPatientIssueVoucherWalkIn(int? Id)
        {
            ViewBag.Doses = db.Dose.ToList();
            ViewBag.WalkInPatient = true;
            ViewBag.Dose = db.Dose.ToList();
            SimulationData simulationData = new SimulationData();

            simulationData.SimulationPatientIssueVoucher = new SimulationPatientIssueVoucher();

            List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

            if (Id.HasValue)
            {
                SimulationPatientIssueVoucher issueVoucher = new SimulationPatientIssueVoucher()
                {
                    Id = Id.Value
                };

                simulationData.SimulationPatientIssueVoucher = issueVoucher;

                var patientIdentifierId = Id;
                var patientsMedication = db.Walkings.Where(p => p.PatientIdentifierId == patientIdentifierId).OrderByDescending(p=>p.PatientIdentifierId).ToList();

                ViewBag.PatientName = patientsMedication.FirstOrDefault().PatientsName;

                foreach (var item in patientsMedication)
                {
                    SimulationTreatment simulation = new SimulationTreatment();

                        simulation.ItemMaster = db.ItemMaster.Find(item.DrugId);
                        //simulation.ItemMaster.ItemName = item.DrugName;
                        simulation.Rate = (int)item.UnitPrice;
                        simulation.units = item.Quantity;
                        simulation.Amount = item.Subtotal;
                        simulation.NoOfDays = item.Day;
                        simulation.Description = item.Notes;
                        simulation.Id = item.Id;
                        simulation.Available = true;
                        simulation.isPaid = item.Paid;
                        simulation.isIssued = item.Issued;
                        simulation.IsWalkIn = true;
                        
                    
                    LstSimulationTreatment.Add(simulation);
                }

              
            }

            if (LstSimulationTreatment.All(p => p.isPaid == true))
            {
                ViewBag.IssueButton = true;
            }

            simulationData.LstSimulationTreatment = LstSimulationTreatment;


            return View(simulationData);
        }

        private List<SimulationTreatment> GetAllDrugsInItemMaster(List<Medication> patientsMedication)
        {

            List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();
            bool checkIfDrugsAreIssued = patientsMedication.All(p => p.Issued == true);

            if (checkIfDrugsAreIssued == true)
            {
                foreach (var dat in patientsMedication)
                {
                    var itemMaster = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();
                    SimulationTreatment simulation = new SimulationTreatment();

                    if (itemMaster != null)
                    {
                        simulation.ItemMaster = itemMaster;
                        simulation.Rate = (int)dat.UnitPrice;
                        simulation.units = dat.QuantityIssued ?? default;
                        simulation.Amount = dat.Subtotal;
                        simulation.NoOfDays = dat.Day;
                        simulation.Description = dat.Notes;
                        simulation.Id = dat.Id;
                        simulation.Available = dat.Available.Value;
                        simulation.isPaid = true;
                        simulation.isIssued = true;
                    }

                    LstSimulationTreatment.Add(simulation);
                }

            }
            else
            {
                bool checkIfItemsArePaid = patientsMedication.All(p => p.Paid == true);

                //check if all items are paid
                if (checkIfItemsArePaid == true)
                {
                    foreach (var dat in patientsMedication)
                    {
                        var itemMaster = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();
                        SimulationTreatment simulation = new SimulationTreatment();

                        if (itemMaster != null)
                        {
                            itemMaster.Drug = new Drug() { Dose = new Dose() { Name = dat.Frequency } };
                            simulation.ItemMaster = itemMaster;
                            simulation.Rate = (int)dat.UnitPrice;
                            simulation.units = dat.Quantity;
                            simulation.Amount = dat.Subtotal;
                            simulation.NoOfDays = dat.Day;
                            simulation.Description = dat.Notes;
                            simulation.Id = dat.Id;
                            simulation.Available = dat.Available.Value;
                            simulation.isPaid = true;
                            simulation.isIssued = dat.Issued;
                        }

                        if (dat.IsFromDoctor != null && !dat.IsFromDoctor.Value)
                        {
                            simulation.IsWalkIn = true;
                        }
                        LstSimulationTreatment.Add(simulation);
                    }
                }
                else
                {
                    bool checkIfAvailableIsTrue = patientsMedication.All(p => p.Available == true);

                    if (checkIfAvailableIsTrue == true)
                    {
                        foreach (var dat in patientsMedication)
                        {
                            var itemMaster = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();
                            SimulationTreatment simulation = new SimulationTreatment();

                            if (itemMaster != null)
                            {
                            itemMaster.Drug = new Drug() { Dose = new Dose() { Name = dat.Frequency } };
                                simulation.ItemMaster = itemMaster;
                                simulation.Rate = (int)dat.UnitPrice;
                                simulation.units = dat.Quantity;
                                simulation.Amount = dat.Subtotal;
                                simulation.NoOfDays = dat.Day;
                                simulation.Description = dat.Notes;
                                simulation.Id = dat.Id;
                                simulation.Available = true;
                                simulation.isPaid = dat.Paid;
                                simulation.isIssued = dat.Issued;
                            }
                            if (dat.IsFromDoctor != null && !dat.IsFromDoctor.Value)
                            {
                                simulation.IsWalkIn = true;
                            }
                            LstSimulationTreatment.Add(simulation);
                        }
                    }
                    else
                    {
                        foreach (var dat in patientsMedication)
                        {
                            //var itemMaster = db.Drug.Where(p => p.Id == dat.DrugId).FirstOrDefault();
                            // here it will display the medications 
                            SimulationTreatment simulation = new SimulationTreatment();

                            if (dat.Available == true)
                            {
                                ItemMaster master = new ItemMaster()
                                {
                                    ItemName = dat.DrugName,
                                    CurrentStock = db.ItemMaster.FirstOrDefault(p=>p.Id == dat.DrugId).CurrentStock
                                    
                                };
                                master.Drug = new Drug() { Dose = new Dose() { Name = dat.Frequency } };
                                master.Drug = new Drug() { Dose = new Dose() { Name = dat.Frequency } };

                                simulation.ItemMaster = master;
                                simulation.Rate = (int)dat.UnitPrice;
                                simulation.units = dat.QuantityIssued ?? default;
                                simulation.Amount = dat.Subtotal;
                                simulation.NoOfDays = dat.Day;
                                simulation.Description = dat.Notes;
                                simulation.Id = dat.Id;
                                simulation.Available = true;
                                simulation.isPaid = dat.Paid;
                                simulation.isIssued = dat.Issued;
                            }
                            else
                            {
                                ItemMaster master = new ItemMaster()
                                {
                                    ItemName = dat.DrugName
                                };

                                master.Drug = new Drug() { Dose = new Dose() { Name = dat.Frequency } };
                                simulation.ItemMaster = master;
                                simulation.Rate = (int)dat.UnitPrice;
                                simulation.units = dat.QuantityIssued ?? default;
                                simulation.Amount = dat.Subtotal;
                                simulation.NoOfDays = dat.Day;
                                simulation.Description = dat.Notes;
                                simulation.Id = dat.Id;
                                simulation.Available = false;
                                simulation.isPaid = false;
                                simulation.isIssued = dat.Issued;
                            }

                            if (dat.IsFromDoctor != null && !dat.IsFromDoctor.Value)
                            {
                                simulation.IsWalkIn = true;
                            }
                            LstSimulationTreatment.Add(simulation);
                        }
                    }
                }
            }

            return LstSimulationTreatment;
        }

        public ActionResult MarkDrugInMedicationAsAvailable(int Id, bool? notAvailable)
        {
            var exempt_under_five = db2.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("exempt_under_five"));

            if (Id == null || Id == 0)
            {
                return Json("null", JsonRequestBehavior.AllowGet);
            }
            Medication medication = db2.Medications.Find(Id);
            if (medication.DrugId == null)
            {
                var item = db.ItemMaster.Where(e => e.ItemName == medication.DrugName &&
               e.CurrentStock >= medication.Quantity).FirstOrDefault();

               // if (notAvailable.HasValue? notAvailable.Value : false)
               // {
               //     item = db.ItemMaster.Where(e => e.ItemName == medication.DrugName &&
               //e.CurrentStock >= medication.Quantity).FirstOrDefault();
               // }
               
                if (item != null)
                {
                    medication.DrugId = item.Id;
                }
            }
            List<ItemMaster> AllBatches = new List<ItemMaster>();

            if (medication.Available == false)
            {
                #region Select Batch Number Algorithm
                //here i get all the drugs with the same drug id in tem Master table
                //select where expirydate is greater than todays date and 
                //current stock is greater than zero
                //the current stock used is summation of where expirydate is greater than tody and current stock is greater than zero
                //return store flag

                var items = db.ItemMaster.Where(p =>p.StoreName=="P" && ((p.DrugId == medication.DrugId) ||
                (p.Drug.Name == medication.DrugName))).ToList();

                var todaysDate = DateTime.Now;


                AllBatches = new List<ItemMaster>();

                //here i check whether the drug exists in item master
                if (items.Sum(p => p.CurrentStock) >= medication.Quantity)
                {
                    if (items.All(e => e.ExpiryDate.HasValue && e.ExpiryDate.Value < todaysDate))
                    {
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    }

                    foreach (var item in items)
                    {
                        if (item.ExpiryDate.HasValue)
                        {
                            if (item.ExpiryDate.Value > todaysDate && item.CurrentStock > 0)
                            {
                                AllBatches.Add(item);
                            }
                        }
                        else
                        {
                            if (item.CurrentStock > 0)
                            {
                                AllBatches.Add(item);
                            }

                        }
                    }
                }


                // AllBatches = AllBatches.Where(p => p.CurrentStock > 0 && p.ExpiryDate > todaysDate).ToList();

                ItemMaster itemMaster = AllBatches.FirstOrDefault();
                var currentStock = AllBatches.Sum(p => p.CurrentStock);

                #endregion

                //Here we check whether the item exists in item master table
                if (itemMaster != null)
                {
                    if (!notAvailable.HasValue)
                    {

                        if (medication.Quantity > currentStock)
                        {
                            ViewBag.IssuedQuantityIsGreater = true;
                            ViewBag.CurrentStock = itemMaster.CurrentStock;

                            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_EditIssuedDrugs.cshtml", medication);

                        }
                        else
                        {
                            //set medication drug id to item master drug id so that we can keep track of stock
                            int DrugId = medication.DrugId.Value;
                            medication.DrugId = itemMaster.Id;
                            medication.Available = true;
                            medication.QuantityIssued = medication.Quantity;
                            medication.OriginalDrugId = DrugId;

                            #region Select Patients Tarrif Algorithm
                            //Here we find tarrif that patient belongs by getting it from OpdRegister
                            //then we get the tarrif medication cost, then assign it to the medication unit price

                            var tarrifId = db2.OpdRegisters.Find(medication.OPDNo).TariffId;

                            var tarrifAmount = db.DrugTariffs.Where(p => p.TariffId == tarrifId && p.DrugId == DrugId).FirstOrDefault();

                            if (tarrifAmount != null)
                            {
                                medication.UnitPrice = tarrifAmount.DrugUnitPrice;
                                medication.Subtotal = medication.QuantityIssued.Value * tarrifAmount.DrugUnitPrice;

                                if (exempt_under_five != null && (exempt_under_five.Value).ToLower()
                                    .Equals("yes"))
                                {
                                    if (tarrifAmount.IsUnder5 && (DateTime.Now.Year - (db2.OpdRegisters.Find(medication.OPDNo).Patient.DOB)?.Year) <= 5)
                                    {
                                        medication.Award = medication.Subtotal;
                                        medication.Paid = true;
                                    }
                                }
                            }
                            else
                            {
                                medication.UnitPrice = itemMaster.SellingPriceUnit;
                                medication.Subtotal = medication.QuantityIssued.Value * itemMaster.SellingPriceUnit;


                                if (exempt_under_five != null && (exempt_under_five.Value).ToLower()
                                    .Equals("yes"))
                                {
                                    if ((DateTime.Now.Year - (db2.OpdRegisters.Find(medication.OPDNo).Patient.DOB)?.Year) <= 5)
                                    {
                                        medication.Award = medication.Subtotal;
                                        medication.Paid = true;
                                    }
                                }

                            }
                            #endregion

                            db2.Entry(medication).State = EntityState.Modified;
                            db2.SaveChanges();

                            SimulationData simulationData = new SimulationData();
                            simulationData = GetTreatmentForPatient(medication.OPDNo);
                            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", simulationData.LstSimulationTreatment);
                        }
                    }
                    else
                    {
                        medication.Available = false;
                        db2.Entry(medication).State = EntityState.Modified;
                        db2.SaveChanges();

                        SimulationData simulationData = new SimulationData();
                        simulationData = GetTreatmentForPatient(medication.OPDNo);
                        return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", simulationData.LstSimulationTreatment);
                    }
                }
                else
                {
                    //therefore if the drug is not in our store
                    //it still remains in medication but i update the column that 
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                #region Select Batch Number Algorithm
                //here i get all the drugs with the same drug id in tem Master table
                //select where expirydate is greater than todays date and 
                //current stock is greater than zero
                //the current stock used is summation of where expirydate is greater than tody and current stock is greater than zero
                //return store flag

                var items = db.ItemMaster.Where(p => p.CurrentStock > medication.Quantity && p.Id == medication.DrugId).ToList();

                var todaysDate = DateTime.Now;


                AllBatches = new List<ItemMaster>();

                //here i check whether the drug exists in item master
                if (items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        if (item.ExpiryDate.HasValue)
                        {
                            
                            if (notAvailable.Value)
                            {
                                AllBatches.Add(item);
                            }else if (item.ExpiryDate.Value > todaysDate && item.CurrentStock > 0)
                            {
                                AllBatches.Add(item);
                            }

                        }
                        else
                        {
                            if (item.CurrentStock > 0)
                            {
                                AllBatches.Add(item);
                            }

                        }
                    }
                }


                // AllBatches = AllBatches.Where(p => p.CurrentStock > 0 && p.ExpiryDate > todaysDate).ToList();

                ItemMaster itemMaster = AllBatches.FirstOrDefault();
                var currentStock = AllBatches.Sum(p => p.CurrentStock);


                #endregion

                //Here we check whether the item exists in item master table
                if (itemMaster != null)
                {
                    if (!notAvailable.HasValue)
                    {

                        if (medication.Quantity > currentStock)
                        {
                            ViewBag.IssuedQuantityIsGreater = true;
                            ViewBag.CurrentStock = itemMaster.CurrentStock;

                            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_EditIssuedDrugs.cshtml", medication);

                        }
                        else
                        {
                            //set medication drug id to item master drug id so that we can keep track of stock
                            int DrugId = medication.DrugId.Value;
                            medication.DrugId = itemMaster.Id;
                            medication.Available = true;
                            medication.QuantityIssued = medication.Quantity;

                            #region Select Patients Tarrif Algorithm
                            //Here we find tarrif that patient belongs by getting it from OpdRegister
                            //then we get the tarrif medication cost, then assign it to the medication unit price

                            var tarrifId = db2.OpdRegisters.Find(medication.OPDNo).TariffId;

                            var tarrifAmount = db.DrugTariffs.Where(p => p.TariffId == tarrifId && p.DrugId == DrugId).FirstOrDefault();

                            if (tarrifAmount != null)
                            {
                                medication.UnitPrice = tarrifAmount.DrugUnitPrice;
                                medication.Subtotal = medication.QuantityIssued.Value * tarrifAmount.DrugUnitPrice;


                                if (exempt_under_five != null && (exempt_under_five.Value).ToLower()
                                    .Equals("yes"))
                                {
                                    if (tarrifAmount.IsUnder5 && (DateTime.Now.Year - (db2.OpdRegisters.Find(medication.OPDNo).Patient.DOB)?.Year) <= 5)
                                    {
                                        medication.Award = medication.Subtotal;
                                        medication.Paid = true;
                                    }
                                }
                            }
                            else
                            {
                                medication.UnitPrice = itemMaster.SellingPriceUnit;
                                medication.Subtotal = medication.QuantityIssued.Value * itemMaster.SellingPriceUnit;

                                if (exempt_under_five != null && (exempt_under_five.Value).ToLower()
                                    .Equals("yes"))
                                {
                                    if (tarrifAmount.IsUnder5 && (DateTime.Now.Year - (db2.OpdRegisters.Find(medication.OPDNo).Patient.DOB)?.Year) <= 5)
                                    {
                                        medication.Award = medication.Subtotal;
                                        medication.Paid = true;
                                    }
                                }
                            }
                            #endregion

                            db2.Entry(medication).State = EntityState.Modified;
                            db2.SaveChanges();

                            SimulationData simulationData = new SimulationData();
                            simulationData = GetTreatmentForPatient(medication.OPDNo);
                            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", simulationData.LstSimulationTreatment);
                        }
                    }
                    else
                    {
                        medication.DrugId = medication.OriginalDrugId;
                        medication.Available = false;
                        db2.Entry(medication).State = EntityState.Modified;
                        db2.SaveChanges();

                        SimulationData simulationData = new SimulationData();
                        simulationData = GetTreatmentForPatient(medication.OPDNo);
                        return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", simulationData.LstSimulationTreatment);
                    }
                }
                else
                {
                    //therefore if the drug is not in our store
                    //it still remains in medication but i update the column that 
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
            }

            
        }

        public ActionResult ConfirmIssueLessDrugQuantity(int Id)
        {

            Medication medication = db2.Medications.Where(p => p.Id == Id).FirstOrDefault();

            var itemMasterId = medication.DrugId;
            ItemMaster itemMaster = db.ItemMaster.Find(itemMasterId);

            //here we are updating quantity issued so that the quantity that doctor prescribes remains untouched
            medication.Available = true;
            medication.QuantityIssued = itemMaster.CurrentStock;
            medication.Subtotal = itemMaster.CurrentStock * medication.UnitPrice;
            db2.Entry(medication).State = EntityState.Modified;
            db2.SaveChanges();

            SimulationData simulationData = new SimulationData();
            simulationData = GetTreatmentForPatient(medication.OPDNo);
            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", simulationData.LstSimulationTreatment);
            
        }
        public JsonResult SearchDrugbyName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => (p.IsActive == null || (bool)p.IsActive) 
                && p.Name.Contains(name)).Take(20).ToList();

                var OnlyDrugsFromPharmacy = db2.KeyValuePairs.FirstOrDefault(e =>
                e.Key_.Equals("OnlyDrugsFromPharmacy"));

                if (OnlyDrugsFromPharmacy == null)
                {
                    db2.KeyValuePairs.Add(
                        new KeyValuePair()
                        {
                            Key_ = "OnlyDrugsFromPharmacy",
                            Value = "No",
                            Owner = "Dev"
                        }
                    );
                    db.SaveChanges();
                }
                else if (OnlyDrugsFromPharmacy.Value.ToLower().Equals("yes"))
                {
                    var toberemoved = new List<Drug>();

                    foreach (var item in data)
                    {
                        var drug = db.ItemMaster.Any(e => e.DrugId == item.Id
                        && (e.ExpiryDate == null || (e.ExpiryDate < DateTime.Today)) && e.CurrentStock > 0
                        && e.StoreName.Equals("P"));

                        if (!drug)
                        {
                            toberemoved.Add(item);
                        }
                    }

                    data.RemoveAll(e => toberemoved.Contains(e));

                }
                return Json(data.Select(p => p.Name).ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public struct DrugDetails{
            public string BatchNo { get; set; }
            public int Id { get; set; }
            public double SellingPrice { get; set; }
            public int CurrentStock { get; set; }
            public string ExpiryDate { get; set; }

        }
        public JsonResult SearchItemSortingWithBatch(string name)
        {
            db.Configuration.LazyLoadingEnabled = false;

            if (!string.IsNullOrEmpty(name))
            {
                //TODO: Add a method to select only one batch number from the list of batch numbers
                var main_data = db.ItemMaster.Where(p => p.ItemName.Contains(name)&&p.StoreName=="P").ToList();

                if(!main_data.Any())
                {
                    return Json("Null",JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<DrugDetails> drugDetailsList = new List<DrugDetails>();
                    foreach (var data in main_data)
                    {
                        var DrugUnitPrice = 0;
                        var price = db.DrugTariffs.FirstOrDefault(e => e.DrugId == data.DrugId);

                        if (price != null)
                        {
                            DrugUnitPrice = (int)price.DrugUnitPrice;
                        }
                        drugDetailsList.Add(
                            new DrugDetails()
                            {
                                Id = data.Id,
                                SellingPrice = DrugUnitPrice,
                                CurrentStock = data.CurrentStock,
                                BatchNo = data.BatchNo,
                                ExpiryDate = data.ExpiryDate.HasValue ? ((DateTime)data.ExpiryDate).ToString("dd-MM-yyyy") : "Non Expiry Item"
                            }
                        );
                    }

                    return Json(drugDetailsList, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchDrugByComposition(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {

                var data = db.GenericDrugName.Where(p => p.Name.Contains(name)).Select(p => new {p.Name,p.Id}).ToList();
                db.Configuration.LazyLoadingEnabled = false;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchDrugNamesUsingCompositionId(int Id)
        {
                var data = db.Drug.Where(p => p.GenericDrugNameId ==Id).Select(p => new { p.Name }).ToList();
                
                return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveTreatmentData(SimulationTreatment simulationTreatment, int Dose)
        {
            if (simulationTreatment != null)
            {
                var dose = db.Dose.Find(Dose);

                var itemMaster = db.ItemMaster.Where(p => p.Id == 
                simulationTreatment.ItemMasterId).FirstOrDefault();

                Medication medication = new Medication()
                {
                    OPDNo = simulationTreatment.SimulationPatientIssueVoucherId,
                    DrugId = simulationTreatment.ItemMasterId,
                    OriginalDrugId = itemMaster.DrugId ?? 0,
                    DrugName = itemMaster.ItemName,
                    Frequency = dose.Name,
                    Day = simulationTreatment.NoOfDays,
                    Quantity = simulationTreatment.units,
                    QuantityIssued = simulationTreatment.units,
                    RoutingAdmin = "1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(simulationTreatment.NoOfDays),
                    UserId = Convert.ToInt32(Session["UserId"]),
                    BranchId = default,
                    TimeAdded = DateTime.Now,
                    UnitPrice = simulationTreatment.Rate,
                    Paid = false,
                    Issued=false,
                    BillNo = 18,
                    Discount = 0,
                    Subtotal = simulationTreatment.Amount,
                    Available = true,
                    IsFromDoctor = false
                };
                db2.Medications.Add(medication);
                db2.SaveChanges();
                

                int OpdNo = simulationTreatment.SimulationPatientIssueVoucherId;
                //here we get the medications for the particular opd number
                var LstSimulationTreatment = GetTreatmentForPatient(OpdNo);

                return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", LstSimulationTreatment.LstSimulationTreatment);
            }

            SimulationTreatment dat = null;

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", dat);

        }
        
        public ActionResult AddTreatmentDataWalkIn(SimulationTreatment simulationTreatment)
        {

            SimulationData simulationData = new SimulationData()
            {
                LstSimulationTreatment = new List<SimulationTreatment>()
            };


            var drugId = db.ItemMaster.Where(p => p.Id == simulationTreatment.ItemMasterId).FirstOrDefault().DrugId;
            var drugTarrifs = db.DrugTariffs.Where(p => p.DrugId == drugId).ToList();
            var drugTarrif = drugTarrifs.Where(p => p.TarrifName.ToLower().Contains("cash") ).FirstOrDefault();

            if (simulationTreatment != null)
            {
                simulationTreatment.Available = true;
                simulationTreatment.isPaid = false;
                simulationTreatment.Rate = (int)drugTarrif.DrugUnitPrice;

                if (Session["DrugsForWalkInPatient"] != null)
                {
                    simulationData.LstSimulationTreatment = (List<SimulationTreatment>)Session["DrugsForWalkInPatient"];
                    simulationData.LstSimulationTreatment.Add(simulationTreatment);
                    Session["DrugsForWalkInPatient"] = simulationData.LstSimulationTreatment;
                }
                else
                {
                    simulationData.LstSimulationTreatment.Add(simulationTreatment);
                    Session["DrugsForWalkInPatient"] = simulationData.LstSimulationTreatment;
                }
            }

            foreach (var item in simulationData.LstSimulationTreatment)
            {
                item.ItemMaster = db.ItemMaster.Where(p => p.Id == item.ItemMasterId).FirstOrDefault();
            }
            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", simulationData.LstSimulationTreatment);

        }

        public int RemoveTreatmentDataWalkIn(int id)
        {
            if (Session["DrugsForWalkInPatient"] != null)
            {                
                List<SimulationTreatment> tempdata = (List<SimulationTreatment>)Session["DrugsForWalkInPatient"];

                var singleItem = tempdata.ElementAt(id);

                if (singleItem != null)
                {
                    tempdata.RemoveAt(id);
                }

                Session["DrugsForWalkInPatient"] = tempdata;

            }
            return 1;
        }

        public ActionResult SaveDrugsForWalkInPatient(string PatientName)
        {
            if (PatientName == null || PatientName.Length == 0)
            {
                PatientName = "Patient " + ((db.Walkings.Count() + 1)).ToString();
            }
            int newUserId = default;
            if (Session["DrugsForWalkInPatient"]!=null)
            {
                var DrugsForWalkInPatient = (List<SimulationTreatment>)Session["DrugsForWalkInPatient"];
                
                var walkings = db.Walkings.OrderByDescending(p => p.Id).FirstOrDefault();

                if (walkings == null)
                {
                    newUserId = 1;
                }
                else
                {
                    newUserId = walkings.PatientIdentifierId + 1;
                }

                foreach (var item in DrugsForWalkInPatient)
                {
                    var itemMaster = db.ItemMaster.Where(p => p.Id == item.ItemMasterId).FirstOrDefault();
                    Walking walking = new Walking()
                    {

                        DrugId = item.ItemMasterId,
                        DrugName = item.ItemMaster.ItemName, //itemMaster.ItemName,
                        //Frequency = 1,
                        UnitPrice = item.Rate,
                        Day = item.NoOfDays,
                        Quantity = item.units,
                        RoutingAdmin = 1,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(item.NoOfDays),
                        Subtotal = item.Amount,
                        TimeAdded = DateTime.Now,
                        PatientIdentifierId = newUserId,
                        Paid = false,
                        Issued = false,
                        PatientsName = PatientName
                    };

                    db.Walkings.Add(walking);
                    
                }

                db.SaveChanges();
            }

            ViewBag.TheBillingUserId = newUserId;
            List <SimulationTreatment> dat = new List<SimulationTreatment>();
            Session["DrugsForWalkInPatient"] = null;
            
            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", dat);
        }

       
        public ActionResult IssueItems(int Id)
        {
            var medications = db2.Medications.Where(p => p.OPDNo == Id && !p.Issued && p.Paid).ToList();
            int LastTransactionId = new int();

            //get the last transaction Id 
            var allMedications = db2.Medications.Where(p=>p.TransactionId.HasValue == true).OrderByDescending(p => p.Id).FirstOrDefault();

            if (allMedications != null)
            {
                LastTransactionId = allMedications.TransactionId.Value;
            }
            else
            {
                LastTransactionId = 1;
            }

            List<DrugTransactions> lstDrugTransactions = new List<DrugTransactions>();

            foreach (var item in medications)
            {
                //here we check if item is already issued, we dnt update the current stock
                if (item.Issued == false)
                {
                    if (item.DrugId != 0 && item.Available == false)
                    {

                    }
                    else
                    {
                        //// here we update the issued to true and also update the quantity issued
                        //item.Issued = true;

                        //if (!item.OpdRegister.IsIPD)
                        //{
                        //    item.Paid = true;
                        //}
                        //item.QuantityIssued = item.Quantity;
                        //item.TransactionId = LastTransactionId + 1;


                        var itemMaster = db.ItemMaster.Where(p => p.StoreName.Equals("P") 
                        && p.Drug.Name.Equals(item.DrugName)).ToList();

                        if (itemMaster.Count > 0 && itemMaster.Sum(e => e.CurrentStock) 
                            >= item.QuantityIssued)
                        {
                            // here we update the issued to true and also update the quantity issued
                            item.Issued = true;

                            if (!item.OpdRegister.IsIPD)
                            {
                                item.Paid = true;
                            }
                            item.QuantityIssued = item.Quantity;
                            item.TransactionId = LastTransactionId + 1;


                            //here we update the current stock minus the items issued

                            //Check if the current batch can sustain else get other batches

                            foreach (var item_ in itemMaster)
                            {
                                if (item.QuantityIssued.Value <= 0)
                                {
                                    continue;
                                }
                                if (item_.CurrentStock >= item.QuantityIssued.Value)
                                {
                                    item_.CurrentStock = item_.CurrentStock - item.QuantityIssued.Value;
                                    db.Entry(item_).State = EntityState.Modified;
                                }
                                else
                                {
                                    item.QuantityIssued = item.QuantityIssued.Value - item_.CurrentStock;

                                    item_.CurrentStock = item_.CurrentStock - item_.CurrentStock;
                                    db.Entry(item_).State = EntityState.Modified;

                                }
                            }                            
                            
                        }
                        else
                        {
                            //if drug is not in item master then we update the property DrugIsNotInItemMaster
                            item.DrugIsNotInItemMaster = true;
                        }

                        //here i then update the drug in item master
                        db2.Entry(item).State = EntityState.Modified;

                        DrugTransactions drugTransactions = new DrugTransactions()
                        {

                            ItemMasterId = item.DrugId.Value,
                            PatientId = item.OPDNo,
                            QuantityIssued = item.QuantityIssued.Value,
                            TransactionDate = DateTime.Now,
                            Rate = (int)item.UnitPrice
                        };

                        lstDrugTransactions.Add(drugTransactions);
                    }
                }
                

            }

            db.DrugTransactions.AddRange(lstDrugTransactions);
            db2.SaveChanges();
            db.SaveChanges();

            Session["IssuedItems"] = "IssuedItems";

            return RedirectToAction("SearchPatientIssueVoucher");
        }

        public ActionResult IssueItemsDirectVoucher(int Id)
        {

            var PatientsIdentifier = Id;

            var dataFromWalkIn = db.Walkings.Where(p => p.PatientIdentifierId == PatientsIdentifier).ToList();
            List<DrugTransactions> lstDrugTransactions = new List<DrugTransactions>();

            foreach (var item in dataFromWalkIn)
            {
                item.QuantityIssued = item.Quantity;
                item.Issued = true;
                db.Entry(item).State = EntityState.Modified;

                //update the drug transactions table
                DrugTransactions drugTransactions = new DrugTransactions()
                {

                    ItemMasterId = item.DrugId,
                    PatientId = item.PatientIdentifierId,
                    QuantityIssued = item.QuantityIssued,
                    TransactionDate = DateTime.Now,
                    Rate = (int)item.UnitPrice,
                    IsWalkIn = true
                };

                lstDrugTransactions.Add(drugTransactions);

                //update item master for stocks
                var ItemMaster = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                if (ItemMaster != null)
                {
                    //here we update the current stock minus the items issued
                    ItemMaster.CurrentStock = ItemMaster.CurrentStock - item.QuantityIssued;
                    db.Entry(ItemMaster).State = EntityState.Modified;
                }
            }

           
            db.DrugTransactions.AddRange(lstDrugTransactions);
            db2.SaveChanges();
            db.SaveChanges();

            Session["IssuedItems"] = "IssuedItems";

            return RedirectToAction("SearchPatientIssueVoucher");
        }

        public ActionResult EditIssuedDrugs(int Id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var Data = db2.Medications.Find(Id);
            ViewBag.EditIssuedDrugs = true;
            ViewBag.Doses = db.Dose.ToList();
            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_EditIssuedDrugs.cshtml",Data);
        }

        public ActionResult SaveEditedDrug(Medication model)
        {
            var Medication = db2.Medications.Find(model.Id);

            Medication.UnitPrice = model.UnitPrice;
            Medication.Frequency = model.Frequency;
            Medication.Quantity = model.Quantity;
            Medication.QuantityIssued = model.Quantity;
            Medication.Day = model.Day;
            Medication.Notes = model.Notes;
            Medication.Subtotal = model.Subtotal;

            db2.Entry(Medication).State = EntityState.Modified;
            db2.SaveChanges();


            List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

            var patientsMedication = db2.Medications.Where(p => p.OPDNo == Medication.OPDNo).ToList();
            //var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();
            //here we check whether all the drugs in medication are in item Master
            bool CheckIfAllDrugsAreInItemMaster = true;

            foreach (var item in patientsMedication)
            {
                var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                if (drugToBeChecked == null)
                {
                    CheckIfAllDrugsAreInItemMaster = false;
                }
            }

            if (CheckIfAllDrugsAreInItemMaster == true)
            {
                LstSimulationTreatment.AddRange(GetAllDrugsInItemMaster(patientsMedication));
            }
            else
            {
                //select only drugs in the item Master and separate them from the others
                var medicationInItemMaster = new List<Medication>();
                var medicationNotInItemMaster = new List<Medication>();

                foreach (var item in patientsMedication)
                {
                    var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                    if (drugToBeChecked != null)
                    {
                        medicationInItemMaster.Add(item);
                    }
                    else
                    {
                        medicationNotInItemMaster.Add(item);
                    }
                }

                //so now i have two lists 

                //now for the drugs not in drug master
                foreach (var dat in medicationNotInItemMaster)
                {
                    SimulationTreatment simulation = new SimulationTreatment();

                    ItemMaster master = new ItemMaster()
                    {
                        ItemName = dat.DrugName
                    };

                    simulation.ItemMaster = master;
                    simulation.Rate = (int)dat.UnitPrice;
                    simulation.units = dat.QuantityIssued ?? default;
                    simulation.Amount = dat.Subtotal;
                    simulation.NoOfDays = dat.Day;
                    simulation.Description = dat.Notes;
                    simulation.Id = dat.Id;
                    simulation.Available = false;
                    simulation.isPaid = false;
                    simulation.isIssued = dat.Issued;
                    simulation.DrugIsNotInHospitalDataBase = true;
                    LstSimulationTreatment.Add(simulation);
                }

                //now for the drugs that are in item Master
                var allDrugsInItemMaster = GetAllDrugsInItemMaster(medicationInItemMaster);
                LstSimulationTreatment.AddRange(allDrugsInItemMaster);

            }

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", LstSimulationTreatment);
        }

        public ActionResult DeleteIssuedDrugs(int Id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var Data = db2.Medications.Find(Id);
            ViewBag.DeleteIssuedDrugs = true;
            return PartialView("~/Areas/PharmacyModule/Views/PharmacyPatientTransaction/_EditIssuedDrugs.cshtml", Data);
        }

        public ActionResult DeleteConfirmed(int Id)
        {
            var entry = db2.Medications.Find(Id);
            if (entry.Issued || entry.Paid)
            {
                return null;
            }
            int opdno = entry.OPDNo;
            db2.Entry(entry).State = EntityState.Deleted;
            db2.SaveChanges();

            List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

            var patientsMedication = db2.Medications.Where(p => p.OPDNo == entry.OPDNo).ToList();
            //var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();
            //here we check whether all the drugs in medication are in item Master
            bool CheckIfAllDrugsAreInItemMaster = true;

            foreach (var item in patientsMedication)
            {
                var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                if (drugToBeChecked == null)
                {
                    CheckIfAllDrugsAreInItemMaster = false;
                }
            }

            if (CheckIfAllDrugsAreInItemMaster == true)
            {
                LstSimulationTreatment.AddRange(GetAllDrugsInItemMaster(patientsMedication));
            }
            else
            {
                //select only drugs in the item Master and separate them from the others
                var medicationInItemMaster = new List<Medication>();
                var medicationNotInItemMaster = new List<Medication>();

                foreach (var item in patientsMedication)
                {
                    var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                    if (drugToBeChecked != null)
                    {
                        medicationInItemMaster.Add(item);
                    }
                    else
                    {
                        medicationNotInItemMaster.Add(item);
                    }
                }

                //so now i have two lists 

                //now for the drugs not in drug master
                foreach (var dat in medicationNotInItemMaster)
                {
                    SimulationTreatment simulation = new SimulationTreatment();

                    ItemMaster master = new ItemMaster()
                    {
                        ItemName = dat.DrugName
                    };

                    simulation.ItemMaster = master;
                    simulation.Rate = (int)dat.UnitPrice;
                    simulation.units = dat.QuantityIssued ?? default;
                    simulation.Amount = dat.Subtotal;
                    simulation.NoOfDays = dat.Day;
                    simulation.Description = dat.Notes;
                    simulation.Id = dat.Id;
                    simulation.Available = false;
                    simulation.isPaid = false;
                    simulation.isIssued = dat.Issued;
                    simulation.DrugIsNotInHospitalDataBase = true;
                    LstSimulationTreatment.Add(simulation);
                }

                //now for the drugs that are in item Master
                var allDrugsInItemMaster = GetAllDrugsInItemMaster(medicationInItemMaster);
                LstSimulationTreatment.AddRange(allDrugsInItemMaster);

            }

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", LstSimulationTreatment);

        }

        public ActionResult SomethingToBeReturned()
        {
            db2.Configuration.LazyLoadingEnabled = false;
            var dataFromMedication = db2.Medications.ToList();
            PartialViewResult pv = new PartialViewResult();
            var view = PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", dataFromMedication);
            string difference = "true";
            return Json(new { vw = view, xs = difference }, JsonRequestBehavior.AllowGet);

            
        }

        public ActionResult IssueToDepartment()
        {
            ViewBag.Items = db.ItemMaster.OrderBy(f => f.ItemName).Where(e => 
            (e.Drug != null &&( e.Drug.IsActive == null || (bool)e.Drug.IsActive))
            && e.StoreName.Equals("P")).ToList();

            ViewBag.Departments = db2.Departments.OrderBy(p => p.DepartmentName).ToList();
            //ViewBag.Items = db.ItemMaster.Select(p => p.ItemName).Distinct().ToList();

            List<DepartmentVoucherItem> data = new List<DepartmentVoucherItem>();

            if(Session["DepartmentVoucherItems"] !=null)
            {
                data = (List<DepartmentVoucherItem>)Session["DepartmentVoucherItems"];

                foreach (var item in data)
                {
                    var drug = db.ItemMaster.Where(p => p.Id == item.ItemMasterId).FirstOrDefault();
                    item.ItemMaster = drug;
                }
            }
           

            return View(data);
        }

        public JsonResult SearchItemForIssueInDept(string name)
        {
            db.Configuration.LazyLoadingEnabled = false;

            if (!string.IsNullOrEmpty(name))
            {
                //TODO: Add a method to select only one batch number from the list of batch numbers
                var data = db.ItemMaster.Where(p => p.ItemName.Contains(name)).FirstOrDefault();

                if (data == null)
                {
                    return Json("Null", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DrugDetails drugDetails = new DrugDetails()
                    {
                        Id = data.Id,
                        SellingPrice = data.SellingPriceUnit,
                        CurrentStock = data.CurrentStock,
                        BatchNo = data.BatchNo,
                        ExpiryDate = data.ExpiryDate.HasValue ? data.ExpiryDate.Value.ToString("dd-MM-yyyy") : "Non Expiry Item"
                    };

                    return Json(drugDetails, JsonRequestBehavior.AllowGet);
                }

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //search drug names
        public JsonResult SearchItemName(int id)
        {

            if (id > 0)
            {

                var name = db.ItemMaster.Find(id).ItemName;
                var data = db.ItemMaster.Where(e => e.ItemName == name && e.StoreName == "P");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddDeptVoucher(DepartmentIssueVoucherModel model)
        {
            List<DepartmentVoucherItem> LstDepartmentVouchersItems = new List<DepartmentVoucherItem>();

            if (model != null)
            {
                if (Session["DepartmentId"] == null)
                {
                    DepartmentVoucher departmentVoucher = new DepartmentVoucher();
                    departmentVoucher.DepartmentId = model.DepartmentId;
                    //db.DepartmentVoucher.Add(departmentVoucher);
                    //db.SaveChanges();

                    Session["DepartmentId"] = model.DepartmentId;
                }

                DepartmentVoucherItem departmentVoucherItem = new DepartmentVoucherItem()
                {
                    Amount = model.Amount,
                    ItemMasterId = model.ItemMasterId,
                    Units = model.Units
                };

                if(Session["DepartmentVoucherItems"]==null)
                {
                    LstDepartmentVouchersItems.Add(departmentVoucherItem);
                    Session["DepartmentVoucherItems"] = LstDepartmentVouchersItems;
                }
                else
                {
                    LstDepartmentVouchersItems = (List<DepartmentVoucherItem>)Session["DepartmentVoucherItems"];
                    LstDepartmentVouchersItems.Add(departmentVoucherItem);
                }
                

               

            }

            var data = (List<DepartmentVoucherItem>)Session["DepartmentVoucherItems"];

            foreach (var item in data)
            {
                var drug = db.ItemMaster.Where(p => p.Id == item.ItemMasterId).FirstOrDefault();
                item.ItemMaster = drug;
            }

            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", data);

        }

        public ActionResult SaveIssueVoucher()
        {
            //used the attach method to update entities
            List<DepartmentVoucherItem> departmentVoucherItems = new List<DepartmentVoucherItem>();

            if (Session["DepartmentVoucherId"] != null)
            {
                int id = (int)Session["DepartmentVoucherId"];
                var data = db.DepartmentVoucher.Include(p => p.DepartmentVoucherItems)
                                               .Where(p => p.VoucherNO == id)
                                               .FirstOrDefault();
                //TODO: Does not load the department voucher items, load them to calculate totals 
                var totalsCalculation = db.DepartmentVoucherItem.Where(p => p.DepartmentVoucherId == id).Sum(x => x.Amount);

                //db.DepartmentVoucher.Attach(data);
                data.IsItemIssued = true;
                data.IssueDate = DateTime.Now;
                data.Total = totalsCalculation;


                if (db.SaveChanges() == 1)
                {
                    var depVoucherItems = db.DepartmentVoucherItem.Include(k => k.ItemMaster).Include(o => o.DepartmentVoucher).Where(p => p.DepartmentVoucherId == id).ToList();

                    foreach (var dit in depVoucherItems)
                    {
                        var departmentToIssue = db2.Departments.Find(dit.DepartmentVoucher.DepartmentId);

                        DrugTransactions drugTransaction = new DrugTransactions()
                        {
                            DepartmentId = data.DepartmentId,
                            ItemMasterId = dit.ItemMasterId,
                            TransactionDate = DateTime.Now,
                            QuantityIssued = dit.Units,
                            User = (int)Session["UserId"],
                            Rate = Convert.ToInt32(dit.ItemMaster.CostPriceUnit)
                        };

                        db.DrugTransactions.Add(drugTransaction);
                        db.SaveChanges();

                        if (dit.DepartmentVoucher.DepartmentId == db2.Departments.FirstOrDefault(e => e.DepartmentName == "Pharmacy").Id)
                        {

                        }
                        else
                        {
                            var DepartmentItem = db.ItemMaster.FirstOrDefault(e => e.StoreName == departmentToIssue.DepartmentName
                            && e.ItemName == dit.ItemMaster.ItemName && e.BatchNo == dit.ItemMaster.BatchNo);

                            if (DepartmentItem != null)
                            {
                                DepartmentItem.CurrentStock = DepartmentItem.CurrentStock + dit.Units;
                                db.SaveChanges();
                            }
                            else
                            {
                                var item = dit.ItemMaster;

                                var newIM = new ItemMaster()
                                {
                                    ItemName = item.ItemName,
                                    MfgCoName = item.MfgCoName,
                                    GenericDrugName = item.GenericDrugName,
                                    BatchNo = item.BatchNo,
                                    Supplier = item.Supplier,
                                    Category = item.Category,
                                    CurrentStock = item.CurrentStock,
                                    MfgDate = item.MfgDate,
                                    ReorderLevel = item.ReorderLevel,
                                    ICDTenCode = item.ICDTenCode,
                                    barCode = item.barCode,
                                    CasePackRate = item.CasePackRate,
                                    CostPriceUnit = item.CostPriceUnit,
                                    SellingPriceUnit = item.SellingPriceUnit,
                                    ExpiryStatus = item.ExpiryStatus,
                                    ExpiryDate = item.ExpiryDate,
                                    PurchaseDate = item.PurchaseDate,
                                    AssetStatus = item.AssetStatus,
                                    WarantyExpiryDate = item.WarantyExpiryDate,
                                    DateDisposed = item.DateDisposed,
                                    DateCommisioned = item.DateCommisioned,
                                    UnitsPack = item.UnitsPack,
                                    StoreName = item.StoreName,
                                    RackName = item.RackName,
                                    CellInRack = item.CellInRack,
                                    DrugId = item.DrugId,
                                };

                                newIM.CurrentStock = dit.Units;
                                newIM.StoreName = departmentToIssue.DepartmentName;

                                db.ItemMaster.Add(newIM);
                                db.SaveChanges();
                            }
                        }

                        var MItem = db.ItemMaster.FirstOrDefault(e => e.Id == dit.ItemMaster.Id);

                        MItem.CurrentStock = MItem.CurrentStock - dit.Units;

                        int res = db.SaveChanges();

                        
                    }

                    Session["DepartmentVoucherId"] = null;

                }
                Session["DepartmentVoucherId"] = null;

                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", new List<DepartmentVoucherItem>());

            }
            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", departmentVoucherItems);
        }

        public ActionResult GetIssueVoucherList()
        {
            var data = new List<DepartmentVoucherItem>();

            if (Session["DepartmentVoucherId"] != null)
            {
                int id = (int)Session["DepartmentVoucherId"];
                data = db.DepartmentVoucherItem.Include(p => p.ItemMaster)
                                                   .Where(p => p.DepartmentVoucherId == id)
                                                   .ToList();
            }
            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", data);

        }

        public ActionResult NewIssueVoucher()
        {
            Session["DepartmentVoucherId"] = null;
            return RedirectToAction("IssueToDepartment");

        }

        //public ActionResult SaveIssueVoucher()
        //{
        //    //used the attach method to update entities
        //    List<DepartmentVoucherItem> departmentVoucherItems = new List<DepartmentVoucherItem>();

        //    if (Session["DepartmentId"] != null && Session["DepartmentVoucherItems"]!=null)
        //    {
        //        #region Initial Code
        //        //int id = (int)Session["DepartmentId"];
        //        //var data = db.DepartmentVoucher.Include(p => p.DepartmentVoucherItems)
        //        //                               .Where(p => p.VoucherNO == id)
        //        //                               .FirstOrDefault();
        //        ////TODO: Does not load the department voucher items, load them to calculate totals 
        //        //var totalsCalculation = db.DepartmentVoucherItem.Where(p => p.DepartmentVoucherId == id).Sum(x => x.Amount);

        //        //db.DepartmentVoucher.Attach(data);
        //        //data.IsItemIssued = true;
        //        //data.IssueDate = DateTime.Now;
        //        //data.Total = totalsCalculation;
        //        //db.SaveChanges();
        //        #endregion

        //        int departmentId = (int)Session["DepartmentId"];
        //        departmentVoucherItems = (List<DepartmentVoucherItem>)Session["DepartmentVoucherItems"];

        //        DepartmentVoucher departmentVoucher = new DepartmentVoucher()
        //        {
        //            DepartmentId = departmentId,
        //            Total = departmentVoucherItems.Sum(p => p.Amount)
        //        };

        //        db.DepartmentVoucher.Add(departmentVoucher);

        //        foreach (var item in departmentVoucherItems)
        //        {
        //            item.DepartmentVoucherId = departmentVoucher.VoucherNO;
        //        }

        //        db.DepartmentVoucherItem.AddRange(departmentVoucherItems);

        //        db.SaveChanges();

        //        Session["DepartmentId"] = null;
        //        Session["DepartmentVoucherItems"] = null;
        //        departmentVoucherItems = new List<DepartmentVoucherItem>();

        //        return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", departmentVoucherItems);

        //    }
        //    return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", departmentVoucherItems);
        //}

        public ActionResult SearchDepartmentIssuedVouchers()
        {
            var data = db.DepartmentVoucher.Where(p => p.IsItemIssued == true)
                                          .OrderByDescending(p => p.VoucherNO)
                                          .ToList();

            return View(data);
        }

        #region private method
        private SimulationData GetTreatmentForPatient(int OpdNo)
        {
            SimulationData simulationData = new SimulationData();

            List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

            var patientsMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();
            //var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();
            //here we check whether all the drugs in medication are in item Master
            bool CheckIfAllDrugsAreInItemMaster = true;

            foreach (var item in patientsMedication)
            {
                var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                if (drugToBeChecked == null)
                {
                    CheckIfAllDrugsAreInItemMaster = false;
                }
            }



            if (CheckIfAllDrugsAreInItemMaster == true)
            {
                LstSimulationTreatment.AddRange(GetAllDrugsInItemMaster(patientsMedication));
            }
            else
            {
                //select only drugs in the item Master and separate them from the others
                var medicationInItemMaster = new List<Medication>();
                var medicationNotInItemMaster = new List<Medication>();

                foreach (var item in patientsMedication)
                {
                    if (item.Available == true)
                    {
                        var drugToBeChecked = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                        if (drugToBeChecked != null)
                        {
                            medicationInItemMaster.Add(item);
                        }
                        else
                        {
                            medicationNotInItemMaster.Add(item);
                        }
                    }
                    else
                    {
                        var drugToBeChecked = db.ItemMaster.Where(p => p.DrugId == item.DrugId).FirstOrDefault();

                        if (drugToBeChecked != null)
                        {
                            medicationInItemMaster.Add(item);
                        }
                        else
                        {
                            medicationNotInItemMaster.Add(item);
                        }
                    }
                }

                //so now i have two lists 

                //now for the drugs not in drug master
                foreach (var dat in medicationNotInItemMaster)
                {
                    SimulationTreatment simulation = new SimulationTreatment();

                    ItemMaster master = new ItemMaster()
                    {
                        ItemName = dat.DrugName
                    };

                    simulation.ItemMaster = master;
                    simulation.Rate = (int)dat.UnitPrice;
                    simulation.units = dat.QuantityIssued ?? default;
                    simulation.Amount = dat.Subtotal;
                    simulation.NoOfDays = dat.Day;
                    simulation.Description = dat.Notes;
                    simulation.Id = dat.Id;
                    simulation.Available = false;
                    simulation.isPaid = false;
                    simulation.isIssued = dat.Issued;
                    simulation.DrugIsNotInHospitalDataBase = true;
                    LstSimulationTreatment.Add(simulation);
                }

                //now for the drugs that are in item Master
                var allDrugsInItemMaster = GetAllDrugsInItemMaster(medicationInItemMaster);
                LstSimulationTreatment.AddRange(allDrugsInItemMaster);
            }

            simulationData.LstSimulationTreatment = LstSimulationTreatment;

                //int OpdNo = OpdNo;
                //var DistinctPersonFromMedication = db2.OpdRegisters
                //                                   .Include(p => p.Patient)
                //                                   .Where(p => p.Id == OpdNo)
                //                                   .FirstOrDefault();
                //var age = DateTime.Now.Year - ((DateTime)DistinctPersonFromMedication.Patient.DOB).Year;

                //SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher()
                //{
                //    PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName,
                //    Opd = OpdNo.ToString(),
                //    Regno = DistinctPersonFromMedication.Patient.RegNumber,
                //    TransactionId = 0,
                //    Id = OpdNo,
                //    DoctorsName = DistinctPersonFromMedication.ConsultantDoctor,
                //    Age = age.ToString(),
                //    Gender = DistinctPersonFromMedication.Patient.Gender,
                //    DateIssued = DistinctPersonFromMedication.TimeAdded
                //};

                //simulationData.SimulationPatientIssueVoucher = simulationPatientIssue;

                //List<SimulationTreatment> LstSimulationTreatment = new List<SimulationTreatment>();

                //var dataFromMedication = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();

                //foreach (var dat in dataFromMedication)
                //{
                //    var itemMasTer = db.ItemMaster.Where(p => p.Id == dat.DrugId).FirstOrDefault();

                //    SimulationTreatment simulation = new SimulationTreatment()
                //    {
                //        ItemMaster = itemMasTer,
                //        Rate = (int)dat.UnitPrice,
                //        units = dat.Quantity,
                //        Amount = dat.UnitPrice * dat.Quantity,
                //        NoOfDays = dat.Day,
                //        Description = dat.Notes,
                //        Id = dat.Id,
                //        Available = dat.Available.HasValue ? (bool)dat.Available : false

                //    };

                //    LstSimulationTreatment.Add(simulation);
                //}

                //simulationData.LstSimulationTreatment = LstSimulationTreatment;

                return simulationData;

        }

        //this method is used in search patient issue voucher
        private List<SimulationPatientIssueVoucher> GetListOfPatientsWithDetails(List<int> pharmacyPatients)
        {
            List<SimulationPatientIssueVoucher> simulationPatientIssueVoucher = new List<SimulationPatientIssueVoucher>();

            foreach (var OpdNo in pharmacyPatients)
            {
                var isPatiallyPaid = false;
                var isPaid = false;

                var DistinctPersonFromMedication = db2.OpdRegisters
                                                      .Where(p => p.Id == OpdNo)
                                                      .FirstOrDefault();
                var patientsMedications = db2.Medications.Where(p => p.OPDNo == OpdNo).ToList();

                //var billPayment = DistinctPersonFromMedication.Medications.Select(p => p.BillPayment);
                //bool paidAmount = DistinctPersonFromMedication.Medications.All(p => p.Paid == true) ? true : false;
                bool paidAmount = DistinctPersonFromMedication.Medications.Where(p => p.Subtotal>0).All(p=>p.Paid==true)?true:false;

                //here i check whether all the drugs given have the property isfromdoctornull, if true, it means patient is from doctor
                bool checkIfPatientIsFromDoctor = patientsMedications.All(p => p.IsFromDoctor == null);

                SimulationPatientIssueVoucher simulationPatientIssue = new SimulationPatientIssueVoucher();

                if (checkIfPatientIsFromDoctor)
                {
                  

                    if (DistinctPersonFromMedication.Medications.All(p => p.Paid == true))
                    {
                        isPaid = true;
                    }
                    else if (DistinctPersonFromMedication.Medications.Any(p => p.Paid == true))
                    {
                        isPatiallyPaid = true;
                    }
                    simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                    simulationPatientIssue.Opd = OpdNo.ToString();
                    simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                    simulationPatientIssue.TransactionId = OpdNo;
                    simulationPatientIssue.Id = OpdNo;
                    simulationPatientIssue.HivNo = default;
                    simulationPatientIssue.Ipd = default;
                    simulationPatientIssue.totalAmount = DistinctPersonFromMedication.Medications.Sum(p => p.Subtotal);
                    simulationPatientIssue.isPaid = false;
                    simulationPatientIssue.isPatiallyPaid = isPatiallyPaid;
                    //simulationPatientIssue.isFromDoctor = true;

                }
                else
                {
                    //check if all drugs in medication are in the item master
                    bool allDrugsAreInItemMaster = true;

                    foreach (var item in patientsMedications)
                    {
                        var drugInItemMaster = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                        if (drugInItemMaster == null)
                        {
                            allDrugsAreInItemMaster = false;
                        }
                    }

                    if (allDrugsAreInItemMaster)
                    {
                        var checkPaid = patientsMedications.All(p => p.Paid == true);

                        if (checkPaid)
                        {
                            simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                            simulationPatientIssue.Opd = OpdNo.ToString();
                            simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                            simulationPatientIssue.TransactionId = OpdNo;
                            simulationPatientIssue.Id = OpdNo;
                            simulationPatientIssue.HivNo = default;
                            simulationPatientIssue.Ipd = default;
                            simulationPatientIssue.totalAmount = DistinctPersonFromMedication.Medications.Sum(p => p.Subtotal);
                            simulationPatientIssue.isPaid = true;
                            simulationPatientIssue.isFromDoctor = false;
                            simulationPatientIssue.isPatiallyPaid = false;

                        }
                        else
                        {
                            var checkPartiallyPaid = patientsMedications.Any(p => p.Paid == true);

                            simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                            simulationPatientIssue.Opd = OpdNo.ToString();
                            simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                            simulationPatientIssue.TransactionId = OpdNo;
                            simulationPatientIssue.Id = OpdNo;
                            simulationPatientIssue.HivNo = default;
                            simulationPatientIssue.Ipd = default;
                            simulationPatientIssue.totalAmount = DistinctPersonFromMedication.Medications.Sum(p => p.Subtotal);
                            simulationPatientIssue.isPaid = false;
                            simulationPatientIssue.isFromDoctor = false;
                            simulationPatientIssue.isPatiallyPaid = checkPartiallyPaid;
                        }
                    
                    }
                    else
                    {
                        bool checkIfDrugsArePaid = true;
                        //select only the drugs in item Master
                        List<int> drugsInItemMaster = new List<int>();

                        foreach (var item in patientsMedications)
                        {
                            var drugInItemMaster = db.ItemMaster.Where(p => p.Id == item.DrugId).FirstOrDefault();

                            if(drugInItemMaster!=null)
                            {
                                //here i take the PK of the table and not the drug id as implied
                                drugsInItemMaster.Add(item.Id);
                            }
                        }

                        //check if paid == true
                        foreach (var item in drugsInItemMaster)
                        {
                            var selected = patientsMedications.Where(p => p.Id == item).FirstOrDefault();

                            if(selected!=null)
                            {
                                //check if drug is paid for
                                if (selected.Paid == true)
                                {
                                    checkIfDrugsArePaid = true;
                                }
                                else
                                {
                                    checkIfDrugsArePaid = false;
                                }

                            }

                        }

                        if (checkIfDrugsArePaid)
                        {
                            simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                            simulationPatientIssue.Opd = OpdNo.ToString();
                            simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                            simulationPatientIssue.TransactionId = OpdNo;
                            simulationPatientIssue.Id = OpdNo;
                            simulationPatientIssue.HivNo = default;
                            simulationPatientIssue.Ipd = default;
                            simulationPatientIssue.totalAmount = DistinctPersonFromMedication.Medications.Sum(p => p.Subtotal);
                            simulationPatientIssue.isPaid = true;
                            simulationPatientIssue.isFromDoctor = false;
                            simulationPatientIssue.isPatiallyPaid = false;

                        }
                        else
                        {
                            var checkPartiallyPaid = patientsMedications.Any(p => p.Paid == true);

                            simulationPatientIssue.PatientsName = DistinctPersonFromMedication.Patient.Salutation + " " + DistinctPersonFromMedication.Patient.FName + " " + DistinctPersonFromMedication.Patient.MName + " " + DistinctPersonFromMedication.Patient.LName;
                            simulationPatientIssue.Opd = OpdNo.ToString();
                            simulationPatientIssue.Regno = DistinctPersonFromMedication.Patient.RegNumber;
                            simulationPatientIssue.TransactionId = OpdNo;
                            simulationPatientIssue.Id = OpdNo;
                            simulationPatientIssue.HivNo = default;
                            simulationPatientIssue.Ipd = default;
                            simulationPatientIssue.totalAmount = DistinctPersonFromMedication.Medications.Sum(p => p.Subtotal);
                            simulationPatientIssue.isPaid = false;
                            simulationPatientIssue.isFromDoctor = false;
                            simulationPatientIssue.isPatiallyPaid = checkPartiallyPaid;


                        }
                    }


                }



                simulationPatientIssueVoucher.Add(simulationPatientIssue);
            }

            
            return simulationPatientIssueVoucher;

        }

        #endregion

    }
}