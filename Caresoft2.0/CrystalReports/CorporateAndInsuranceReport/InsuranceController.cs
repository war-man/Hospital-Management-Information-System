using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.CorporateAndInsuranceReport
{
    [Auth]
    public class InsuranceController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private DateTime fromDate;
        private DateTime toDate;

        // GET: Insurance
        public ActionResult Index()
        {
            return View();
        }
        public class InsuranceCorporateViewModel
        {
            public string PatientName { get; set; }
            public string OPDNo { get; set; }
            public string Service { get; set; }
            public string InsuranceCorporateName { get; set; }
            public string AwardAmount { get; set; }
        }
        public ActionResult GenerateReport(DateTime FromDate, DateTime ToDate)

        {
            CaresoftHMISEntities db = new CaresoftHMISEntities();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/CorporateAndInsuranceReport/CorporateInsurance.rpt")));


            rd.SetDataSource(Insurance);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetParameterValue("fromDate", FromDate);
            rd.SetParameterValue("toDate", ToDate);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "InsuranceCorporateReport" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);

        }
        private object Insurance
        {
            get
            {
                var AllInsuredPatients = db.Tariffs.Where(p =>p.CompanyId!=1).ToList();
                var InsuredPatients = new Caresoft2._0.CrystalReports.CorporateAndInsuranceReport.Insurance();
                var lstInsuredPatients = db.BillServices.Where(p => p.BillNo != 0).ToList();

                foreach (var item in lstInsuredPatients)
                {
                    var pt = db.OpdRegisters.FirstOrDefault(e=>e.Id==PatientId);

                    InsuredPatients.InsuranceCorporate.AddInsuranceCorporateRow(pt.Patient.FName + "" + pt.Patient.LName, item.OPDNo.ToString(), item.ServiceName.ToString(), pt.Tariff.TariffName, item.Award.ToString());
                }
                return InsuredPatients;
            }
        }

        public int PatientId { get; private set; }
    }
}