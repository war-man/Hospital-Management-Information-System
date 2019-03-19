using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.Doctors
{
    public class DoctorController : Controller
    {
        CaresoftHMISEntities db = new CaresoftHMISEntities();
        //private object GetDocSampleData;
        private object doctor;
        //private object doc;
        //private object dataSet;

        // GET: Doctor
        public class DoctorWorkloadViewModel
        {
            public string Name { get; set; }
            public string Designation { get; set; }
            public double TotalNumberOfPatientsAttendedTo { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoctorsReport(DateTime? FromDate, DateTime? ToDate)
        {
            if (FromDate == null || ToDate == null)
            {
                FromDate = DateTime.Today;
                ToDate = DateTime.Today;
            }
            ToDate = ToDate.Value.AddDays(1);
            //DataTable Doctor = new DataTable();


            //var data = db.Users.Where(e => e.OpdRegisters
            //.Any(f => f.Date >= sdate && f.Date <= edate)).ToList();







            //foreach (var u in )
            //{
            //   var docDataset =new  Caresoft2._0.CrystalReports.Doctors.Doctors.Doc();
            //   docDataset .Doctors.AddDoctorsRow(u.Username,u.Employee.Designation.DesignationName,u.OpdRegisters.Count());


            //}

            //var lstDoctorsCliniciansNurses = db.Users.Where(e =>
            //    e.Employee.Designation.DesignationName.ToLower().Contains("nurse") ||
            //    e.Employee.Designation.DesignationName.ToLower().Contains("doctor") ||
            //    e.Employee.Designation.DesignationName.ToLower().Contains("clinician")).ToList();



            //var Doctor = new Caresoft2._0.CrystalReports.Doctors.Doctors.Doc();

            //foreach (var user in lstDoctorsCliniciansNurses)
            //{

            //    var data = db.PatientDiagnosis.Where(e => e.UserId == user.Id).ToList();
            //    var numberOfPatientsSeen = data.Count();
            //    Doctor.Doctors.AddDoctorsRow(user.Username, user.UserRole.RoleName ?? "", numberOfPatientsSeen);

            //}


            //Doctor.Doctors.AddDoctorsRow("Barbara", "Doctor", 9);
            ReportDocument rd = new ReportDocument();


            rd.Load(Path.Combine(Server.MapPath(@"~\CrystalReports\DoctorsReport\Doctors\Doc.rpt")));

            rd.SetDataSource(Doctor);
            //rd.SetParameterValue("fromDate", FromDate);
            //rd.SetParameterValue("toDate", ToDate);
            // rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());




            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Report Sample - " + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);


        }

        private object Doctor
        {
            get
            {
                var lstDoctorsCliniciansNurses = db.Users.Where(e =>
                e.Employee.Designation.DesignationName.ToLower().Contains("Nurse") ||
                e.Employee.Designation.DesignationName.ToLower().Contains("Doctor") ||
                e.Employee.Designation.DesignationName.ToLower().Contains("Clinician") ||
                e.Employee.Designation.DesignationName.ToLower().Contains("Lab Technician")).ToList();



                var Doctor = new Caresoft2._0.CrystalReports.DoctorsReport.Doctors.Doctor();

                foreach (var user in lstDoctorsCliniciansNurses)
                {

                    var data = db.PatientDiagnosis.Where(e => e.UserId == user.Id).ToList();
                    var numberOfPatientsSeen = data.Count();
                    Doctor._Doctor.AddDoctorRow(user.Username, user.UserRole.RoleName ?? "", "user.numberOfPatientsSeen");

                }

                return Doctor;
            }
        }
    }
}
