using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.AdmittedPatients
{
    [Auth]
    public class AdmittedController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        //private object admissionData;
        //private object dataSet;
        //private object admittedPatientDataset;
        private object PatientsAdmitted;
        private DateTime? fromDate;
        private DateTime? toDate;

        // GET: Admitted
        public ActionResult Index()
        {
            return View();
        }

        public class AdmittedPatientsViewModel
        {
            public string DateAdmitted { get; set; }
            public string PatientName { get; set; }
            public string RegNo { get; set; }
            public string WardName { get; set; }
            public string BedNo { get; set; }
            public string PatientCategory { get; set; }
            public string Diagnosis { get; set; }
            public string Doctor { get; set; }


        }
        public ActionResult GenerateReport(DateTime FromDate, DateTime ToDate)

        {
            CaresoftHMISEntities db = new CaresoftHMISEntities();
            //var toDate = DateTime.Now;
            //var fromDate = new DateTime();



            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/AdmittedPatients/Admitted.rpt")));


            rd.SetDataSource(Admitted);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetParameterValue("fromDate", FromDate);
            rd.SetParameterValue("toDate", ToDate);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = " Admitted Patients" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);

        }
        private object Admitted
        {
            get
            {
                var AllPatientsAdmitted = db.OpdRegisters.Where(p => p.AdmissionDate >= fromDate && p.AdmissionDate <= toDate)
                                    .ToList();
                var admittedPatients = new Caresoft2._0.CrystalReports.AdmittedPatients.PatientsAdmitted();
                var lstAdmittedPatients = db.OpdRegisters.Where(p => p.IsIPD == true).ToList();

                foreach (var pat in lstAdmittedPatients)
                {
                    var PatientId = pat.PatientId;
                    var pt = db.Patients.FirstOrDefault(e => e.Id == PatientId);
                    var dg = db.PatientDiagnosis.FirstOrDefault(p => p.Id != 0);
                    admittedPatients._PatientsAdmitted.AddPatientsAdmittedRow(pat.AdmissionDate.ToString(), pt.FName +
                        " " + pt.LName, pt.RegNumber, pat.HSBed.HSWard.WardName.ToString(), pat.HSBed.BedName.ToString(), pt.PatientCategory, dg.FinalDiagnosis, pat.ConsultantDoctor);
                }
                return admittedPatients;
            }






        }


    }
}

    

