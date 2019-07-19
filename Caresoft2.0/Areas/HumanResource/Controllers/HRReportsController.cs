using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.HumanResource.Controllers
{
    [Auth]
    public class HRReportsController : Controller
    {
        CaresoftHMISEntities db = new CaresoftHMISEntities();
      

        // GET: HumanResource/HRReports
        #region Applicants List Report
        public ActionResult Index()
        {
            return View();
        }
        
        public class ApplicantsListViewModel
        {
            public string ApplicantId { get; set; }
            public string ApplicantName { get; set; }
            public string DateOfApplication { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string NationalId { get; set; }
            public string PositionAppliedFor { get; set; }
           
        }
        public ActionResult GenerateReport(DateTime FromDate, DateTime ToDate)

        {
            var fromDate = FromDate;
            var toDate = ToDate;

            var AllApplicants = new HumanResource.Reports.ApplicantsList.ApplicantDataSet();
            var lstAllApplicants = db.HRApplicants.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var pat in lstAllApplicants)
            {
                var app = db.HRFileInformations.FirstOrDefault(p => p.ApplicantId != null);
                AllApplicants.Applicant.AddApplicantRow(pat.ApplicantId,
                                                        pat.FirstName + "" + pat.Surname,
                                                        app.DateOfApplication.ToString(),
                                                        pat.Mobile,
                                                        pat.Email,
                                                        pat.NationalId,
                                                        app.PositionAppliedFor
                                                        );
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/HumanResource/Reports/ApplicantsList/ApplicantReport.rpt")));


            rd.SetDataSource(AllApplicants);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetParameterValue("fromDate", FromDate);
            rd.SetParameterValue("toDate", ToDate);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Applicants List" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);

        }

        #endregion
        #region Staff List Report
        public ActionResult Staff()
        {
            return View();
        }

        public class StaffListViewModel
        {
            public string Id { get; set; }
            public string StaffName { get; set; }
            public string DateOfEnployment { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Department{ get; set; }
            public string Position { get; set; }

        }
        public ActionResult GenerateStaffReport(DateTime FromDate, DateTime ToDate)

        {
            var fromDate = FromDate;
            var toDate = ToDate;

            var AllStaff= new HumanResource.Reports.StaffList.StaffDataSet();
            var lstAllStaff = db.HRStaffRegistrations.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var pat in lstAllStaff)
            {
                AllStaff.Staff.AddStaffRow(pat.StaffId,
                                           pat.FirstName + "" + pat.Surname,
                                           pat.Department,
                                           pat.DateOfEmployment.ToString(),
                                           pat.Mobile,
                                           pat.Email,
                                           pat.Position
                                           );
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/HumanResource/Reports/StaffList/StaffReport.rpt")));


            rd.SetDataSource(AllStaff);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetParameterValue("fromDate", FromDate);
            rd.SetParameterValue("toDate", ToDate);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Applicants List" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);

        }
        #endregion
    }
}