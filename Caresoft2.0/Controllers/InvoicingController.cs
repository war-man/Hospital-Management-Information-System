using Caresoft2._0.CrystalReports;
using Caresoft2._0.CrystalReports.InsuranceInvoice;
using Caresoft2._0.Models;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    public class InvoicingController : Controller
    {
        private CareSoftLabsEntities dblabs = new CareSoftLabsEntities();
        private CaresoftHMISEntities dbhmis = new CaresoftHMISEntities();

        // GET: Invoicing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FinalizepatientInvoice()
        {
            ViewBag.NHIFRebates = dbhmis.Rebates.ToList();

            ViewBag.NHIFCategories = dbhmis.Tariffs.Where(t => t.Company.CompanyName.ToLower()
           .Equals("nhif")).ToList();

            return View();
        }


        [HttpPost]
        public ActionResult FinalizepatientInvoice(FinalizepatientInvoiceData FinalizepatientInvoiceData,string type)
        {
            var data = FinalizepatientInvoiceData;

            var uid = (int)Session["UserId"];
            var User = dbhmis.Users.FirstOrDefault(e => e.Id == uid);

            {
                var opd = dbhmis.OpdRegisters.Find(data.FinalizepatientInvoice.OPDId);
                if (opd != null)
                {
                    var IIv = dbhmis.InsuranceInvoices.FirstOrDefault(e => e.OpdId == opd.Id && e.CompanyId == 
                    FinalizepatientInvoiceData.FinalizepatientInvoice.CompanyId);

                    if (IIv == null)
                    {
                        IIv = new CaresoftHMISDataAccess.InsuranceInvoice()
                        {
                            OpdId = data.FinalizepatientInvoice.OPDId,
                            NhifRebate = data.FinalizepatientInvoice.NHIFRebate,
                            NhifDifference = data.FinalizepatientInvoice.NHIFDiff,
                            TotalAmount = data.FinalizepatientInvoice.TotalAmount,
                            Preouth = data.FinalizepatientInvoice.PreOuth,
                            CompanyId = data.FinalizepatientInvoice.CompanyId,
                            BranchId = User.Employee.BranchId??1,
                            UserId = User.Id,
                            Finalized = false,
                            CreatedTime = DateTime.Now
                        };

                        dbhmis.InsuranceInvoices.Add(IIv);

                        if (dbhmis.SaveChanges() > 0)
                        {
                            addInsuranceServiceAllocations(data.FinalizepatientInvoice.AllocationArray, IIv, type);
                        }
                    }
                    else
                    {
                        addInsuranceServiceAllocations(data.FinalizepatientInvoice.AllocationArray, IIv, type);
                    }

                    if (type.ToLower().Equals("finalize"))
                    {
                        IIv.Finalized = true;

                    }

                    if (type.ToLower().Equals("finalize"))
                    {
                        //printInvoiceReport(IIv.OpdId);
                        return Json(1);


                    }
                    return Json(2);
                }
            }
            return Json(0);

        }

        private void addInsuranceServiceAllocations(List<AllocatedData> allocationArray, CaresoftHMISDataAccess.InsuranceInvoice IIv, string type)
        {
            var uid = (int)Session["UserId"];
            var User = dbhmis.Users.FirstOrDefault(e => e.Id == uid);

            foreach (var aa in allocationArray.Where(e => e.BSID != 0))
            {
                if (aa.BSID == 0 || aa.AllocatedAmount == 0)
                {              
                    continue;
                }

                //Check if Item Is a Service Or a Drug
                if (aa.Type == "Services")
                {
                    var isa = dbhmis.InvoiceServiceAllocations.Where(e => e.InvoiceId == IIv.id &&
                                    e.BillServiceId == aa.BSID).ToList();

                    var billService = dbhmis.BillServices.FirstOrDefault(e => e.Id == aa.BSID);

                    if (isa?.Sum(e => e.AmountAllocated) < billService.Quatity * billService.Price && aa.AllocatedAmount > 0)
                    {
                        dbhmis.InvoiceServiceAllocations.Add(
                            new InvoiceServiceAllocation()
                            {
                                InvoiceId = IIv.id,
                                BillServiceId = aa.BSID,
                                ServiceName = billService.ServiceName,
                                AmountAllocated = aa.AllocatedAmount,
                                BranchId = User.Employee.BranchId ?? 1,
                                CreatedDate = DateTime.Now
                            });


                        if (dbhmis.SaveChanges() > 0)
                        {
                            var isa2 = dbhmis.InvoiceServiceAllocations.Where(e => e.InvoiceId == IIv.id &&
                                            e.BillServiceId == aa.BSID).ToList();

                            billService.Award = isa2.Sum(e => e.AmountAllocated) / billService.Quatity;

                            if (isa2.Sum(e => e.AmountAllocated) >= billService.Price * billService.Quatity
                                - (billService.WaivedAmount ?? 0 + billService.IPDBillPartialPayments.Sum(e => e.AllocatedAmount)))
                            {
                                if (type.ToLower().Equals("finalize"))
                                {
                                    billService.Paid = true;

                                }
                            }
                            else
                            {
                                billService.Paid = false;
                            }

                            dbhmis.SaveChanges();

                        }
                    }
                } else if (aa.Type == "Medications")
                {
                    var isa = dbhmis.InvoiceServiceAllocations.Where(e => e.InvoiceId == IIv.id &&
                                    e.MedicationId == aa.BSID).ToList();

                    var medication = dbhmis.Medications.FirstOrDefault(e => e.Id == aa.BSID);

                    if (isa?.Sum(e => e.AmountAllocated) < medication.Quantity * medication.UnitPrice && aa.AllocatedAmount > 0)
                    {
                        dbhmis.InvoiceServiceAllocations.Add(
                            new InvoiceServiceAllocation()
                            {
                                InvoiceId = IIv.id,
                                MedicationId = aa.BSID,
                                ServiceName = medication.DrugName,
                                AmountAllocated = aa.AllocatedAmount,
                                BranchId = User.Employee.BranchId ?? 1,
                                CreatedDate = DateTime.Now
                            });


                        if (dbhmis.SaveChanges() > 0)
                        {
                            var isa2 = dbhmis.InvoiceServiceAllocations.Where(e => e.InvoiceId == IIv.id &&
                                            e.MedicationId == aa.BSID).ToList();

                            medication.Award = isa2.Sum(e => e.AmountAllocated) / medication.Quantity;

                            if (isa2.Sum(e => e.AmountAllocated) >= medication.UnitPrice * medication.Quantity
                                - (medication.WaivedAmount ?? 0 + medication.IPDBillPartialPayments.Sum(e => e.AllocatedAmount)))
                            {
                                if (type.ToLower().Equals("finalize"))
                                {
                                    medication.Paid = true;

                                }
                            }
                            else
                            {
                                medication.Paid = false;
                            }

                            dbhmis.SaveChanges();

                        }
                    }
                }
            }

           
        }

        public ActionResult IPDInvoicing()
        {
            return View();
        }

        public ActionResult IPDServicesOffered(int Id, string type)
        {
            ViewBag.Type = type;

            //Get Patient Data
            var OPD = dbhmis.OpdRegisters.FirstOrDefault(e => e.Id == Id);

            return PartialView(OPD);
        }

        public ActionResult SearchPatients(string search, string Type)
        {
            search = search.Trim();

            var pat = dbhmis.OpdRegisters.Where(p => p.IsIPD == true && p.BillServices.Count() > 0 && ((p.Patient.FName + p.Patient.MName + p.Patient.LName)
            .Contains(search) || p.Patient.RegNumber.Contains(search))).Select( x=>
                    new { 
                        PatientId = x.Patient.Id,
                        Name = x.Patient.FName + " " + x.Patient.MName + " " + x.Patient.LName,
                        x.Patient.RegNumber,
                        OPD = x.Id,
                        Type = x.IsIPD,
                        SchemeName = x.Tariff.TariffName,
                        SchemeId = x.Tariff.Id,
                        TimeAdded = x.TimeAdded.ToString(),
                        AdmissionDate = x.AdmissionDate,
                        TotalBill = (x.BillServices),
                        TotalMedBill = (x.Medications),
                        TotalDiscount = 0,
                        TotalDeposits = 0,
                        BillServices = x.BillServices,
                        InsuranceInvoices = x.InsuranceInvoices,
                        NoOfDays = 0
                    }
                ).Take(20).ToList();


         

            var objectlist = new List<object>();

            foreach (var spat in pat)
            {
                var NoOfDays = 0;

                if (spat.AdmissionDate.HasValue)
                {
                    NoOfDays = (int)(DateTime.Today.AddDays(1) - (DateTime)spat.AdmissionDate).TotalDays;

                }
                double TotalBill = 0;
                double PaidBill = 0;
                double TotalAwarded = 0;

                if (spat.TotalBill != null)
                {
                    TotalBill = (spat.TotalBill?.Sum(e => e.Price * e.Quatity)??0) + 
                        (spat.TotalMedBill?.Where(f => f.Available == true).Sum(e => e.UnitPrice * e.Quantity) ?? 0);

                    PaidBill = ((spat.BillServices?.Sum(e => e.BillPayment?.PaidAmount)??0) +
                        (spat.BillServices?.Sum(e => e.IPDBillPartialPayments?.Sum(f => f.AllocatedAmount))??0));

                    TotalAwarded = spat.InsuranceInvoices?.Sum(e => e.InvoiceServiceAllocations?.Sum(f => f.AmountAllocated))??0;
                }
                objectlist.Add(
                    new {
                        spat.PatientId,
                        spat.Name,
                        spat.RegNumber,
                        spat.OPD,
                        spat.Type,
                        spat.SchemeId,
                        spat.SchemeName,
                        spat.TimeAdded,
                        spat.AdmissionDate,
                        TotalBill,
                        PaidBill,
                        TotalAwarded,
                        spat.TotalDiscount,
                        spat.TotalDeposits,
                        NoOfDays
                    }
                );
            }
            return Json(objectlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchInsurances(string search, int VisitNo)
        {
            var opd = dbhmis.OpdRegisters.Find(VisitNo);

            var insurance = dbhmis.Companies.Where(p => p.CompanyName.ToLower().Contains(search.ToLower()) 
            && p.CompanyType.CompanyTypeName.ToLower().Equals("insurance")).Select(x =>
                   new {
                       AccNo = x.Id,
                       Id = x.Id,
                       Name = x.CompanyName
                   }
                ).Take(20).ToList();

            List<object> data = new List<object>();

            foreach ( var i in insurance)
            {
                data.Add(new {
                    i.Id,
                    i.Name,
                    i.AccNo,
                    valid = opd.InsuranceCards.Any(e => e.Tariff.Company.Id.Equals(i.Id))
                    });
            }
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult printInvoiceReport(int id, int CompanyId)
        {
            var OPD = dbhmis.OpdRegisters.Find(id);

            if (OPD == null)
            {
                return null;
            }
            DSInsuranceInvoic dataSet = new DSInsuranceInvoic();

            var pat = dbhmis.PatientDetails.FirstOrDefault(e => e.OpdId == id);

            dataSet.PatientDetails.AddPatientDetailsRow(
                    OPD.Patient.Id,
                    OPD.Patient.RegNumber,
                    OPD.Patient.Salutation ?? "",
                    OPD.Patient.FName,
                    OPD.Patient.MName ?? "",
                    OPD.Patient.LName,
                    OPD.Patient.Gender,
                    OPD.Patient.DOB ?? DateTime.Now,
                    OPD.Patient.Mobile,
                    OPD.Patient.HomeAddress ?? "",
                    OPD.Id,
                    OPD.Date,
                    OPD.InsuranceInvoices?.FirstOrDefault(e => e.CompanyId == CompanyId)?.id?? 0,
                    OPD.InsuranceInvoices?.FirstOrDefault(e => e.CompanyId == CompanyId)?.CreatedTime?? DateTime.Now,
                    OPD.InsuranceCards?.FirstOrDefault(e => e.Tariff.CompanyId == CompanyId)?.Tariff?.Company?.CompanyType?.CompanyTypeName??"",
                    OPD.InsuranceCards?.FirstOrDefault(e => e.Tariff.CompanyId == CompanyId)?.Tariff?.TariffName??"",
                    OPD.InsuranceCards?.FirstOrDefault(e => e.Tariff.CompanyId == CompanyId)?.CardNo?? "N?A",
                    OPD.InsuranceCards?.FirstOrDefault(e => e.Tariff.CompanyId == CompanyId)?.MemberNo??"",
                    pat.ConsultantDoctor,
                    pat.Department ?? "IPD",
                    pat.ReferringDoctor ?? "",

                    pat.InvoiceVerifyUserId??1,
                    OPD.InsuranceCards?.FirstOrDefault(e => e.Tariff.CompanyId == CompanyId)?.Tariff?.Company?.CompanyName ?? "",
                    pat.MemberPatientRelationShip,
                    pat.InvoiceVerifyUserName
                    );

            //Bill Services
            var ibs = dbhmis.InvoiceBills.Where(e => e.OPDId.Equals(id) && e.CompanyId == CompanyId).ToList();
            foreach (var ib in ibs)
            {
                dataSet.InvoiceBill.AddInvoiceBillRow(
                    ib.OPDId,
                    ib.PatientId,
                    ib.ConsultantDoctor ?? "N/A",
                    ib.ReferringDoctor?? "N/A",
                    ib.BroughtBy??"N/A",

                    ib.IsIPD,
                    ib.Username,
                    ib.BranchId??1,
                    ib.TimeAdded??DateTime.Now,
                    ib.InsuranceInvoiceId,

                    ib.NhifRebate??0,
                    ib.TotalAmount??0,
                    ib.NhifDifference??0,
                    ib.CompanyId,
                    ib.Batch ?? "",

                    ib.InsuranceInvoiceUserId,
                    ib.InsuranceInvoiceAllocId,
                    ib.BillServiceId,
                    ib.ServiceName.ToUpper(),
                    ib.AmountAllocated,
                    ib.DateOfService,
                    ib.ServiceRequestDoctor,
                    ib.DepartmentName.ToUpper()
                );
            }


            //Medication Bill Services
            var mbs = dbhmis.InvoiceMedicationBills.Where(e => e.OPDId.Equals(id)).ToList();
            foreach (var ib in mbs)
            {
                dataSet.InvoiceBill.AddInvoiceBillRow(
                    ib.OPDId,
                    ib.PatientId,
                    ib.ConsultantDoctor ?? "N/A",
                    ib.ReferringDoctor ?? "N/A",
                    ib.BroughtBy ?? "N/A",

                    ib.IsIPD,
                    ib.Username,
                    ib.BranchId ?? 1,
                    ib.TimeAdded ?? DateTime.Now,
                    ib.InsuranceInvoiceId,

                    ib.NhifRebate ?? 0,
                    ib.TotalAmount ?? 0,
                    ib.NhifDifference ?? 0,
                    ib.CompanyId,
                    ib.Batch ?? "",

                    ib.InsuranceInvoiceUserId,
                    ib.InsuranceInvoiceAllocId,
                    0,
                    ib.ServiceName.ToUpper(),
                    ib.AmountAllocated,
                    ib.MedicationTimeAdded,
                    ib.ServiceRequestDoctor,
                    "PHARMACY"
                );
            }

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/InsuranceInvoice/InsuranceInvoice.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Insurance Incoice " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public bool checkIfInsuranceValid(int VisitNo, int CompanyId)
        {
            var opd = dbhmis.OpdRegisters.Find(VisitNo);
            return opd.InsuranceCards.Any(e => e.Tariff.Company.Id.Equals(CompanyId));
        }
    }
}