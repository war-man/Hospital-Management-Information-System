using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.DischargedPatients
{
    [Auth]
    public class DischargedController : Controller
    {
       private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private DateTime? fromDate;
        private DateTime? toDate;

        // GET: Discharged
        public class DichargedPatientsViewModel
        {
            public string PatientName { get; set; }
            public int AdmissionDate { get; set; }
            public int DischargeDate { get; set; }
            public int RegNo { get; set; }
            public int PatientId { get; set; }
            public string PatientCategory { get; set; }
            public string Diagnosis { get; set; }
            public string WardName { get; set; }
            public int BedNo { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateReport(DateTime? FromDate, DateTime? ToDate)
        {
            CaresoftHMISEntities db = new CaresoftHMISEntities();
            //var toDate = DateTime.Now;
            //var fromDate = DateTime.Now.AddYears(-2);

            ReportDocument rd = new ReportDocument();


            rd.Load(Path.Combine(Server.MapPath(@"~\CrystalReports\DischargedPatients\DischargedReport.rpt")));
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetDataSource(Discharged);


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Discharged " + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }
        private object Discharged
        {
            get
            {
                var AllPatientsDischarged = db.OpdRegisters.Where(p => p.DischargeDate >= fromDate && p.DischargeDate <= toDate)
                                    .ToList();


                var lstDischargedPatients = db.OpdRegisters
                    .Where(e => e.Discharged==true).ToList();
                
                var dischargedpatients = new Caresoft2._0.CrystalReports.DischargedPatients.Dischargedp.Dischargedpatients();

                foreach (var pat in lstDischargedPatients)
                {
                    //var temp_pat_diag = "";

                    //foreach (var d in pat.PatientDiagnosis.ToList())
                    //{
                    //    temp_pat_diag = temp_pat_diag + d.FinalDiagnosis + " | ";
                    //}
                    var PatientId = pat.PatientId;
                    var pt=db.Patients.FirstOrDefault(e=>e.Id==PatientId);
                    dischargedpatients.Discharged.AddDischargedRow(pt.FName + 
                        " " + pt.LName, pat.AdmissionDate.ToString()
                        , pat.DischargeDate.ToString(),
                        pt.RegNumber, pat.PatientId.ToString(),
                        pt.PatientCategory, "null",
                        pat.HSBed.HSWard.WardName.ToString(),
                        pat.HSBed.BedName.ToString());

                }
                return dischargedpatients;
            }
        }
    }
}