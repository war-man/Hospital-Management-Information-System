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
        #region Applicant Resume
        public ActionResult Resume()
        {
            return View();
        }
        public ActionResult GenerateResumeReport(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            var BasicInformation = GetBasicInformationData(FromDate, ToDate);
            var BioData = GetBioData(FromDate, ToDate);
            var Qualification = GetQuaificationData(FromDate, ToDate);
            var WorkExperience = GetWorkExperienceData(FromDate, ToDate);
            var Referees = GetRefereesData(FromDate, ToDate);

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/HumanResource/Reports/ApplicantResume/MainReport.rpt")));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Rd.Subreports["BasicInforReport.rpt"].SetDataSource(BasicInformation);
            Rd.Subreports["BioDataReport.rpt"].SetDataSource(BioData);
            Rd.Subreports["QualificationReport.rpt"].SetDataSource(Qualification);
            Rd.Subreports["WorkExperienceReport.rpt"].SetDataSource(WorkExperience);
            Rd.Subreports["RefereesReport.rpt"].SetDataSource(Referees);

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Applicant Resume" + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        private object GetBasicInformationData(DateTime fromDate, DateTime toDate)
        {
            var BasicInformation = new HumanResource.Reports.ApplicantResume.BasicInformation();
            var lstBasicInformation = db.HRFileInformations.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();
            foreach (var app in lstBasicInformation)
            {
                BasicInformation.Basic.AddBasicRow(app.FirstName,
                                                   app.OtherNames,
                                                   app.Surname,
                                                   app.ApplicantId);
            }
            return BasicInformation;
        }

        private object GetBioData(DateTime fromDate, DateTime toDate)
        {
            var BioData = new HumanResource.Reports.ApplicantResume.BioData();
            var lstBioData = db.HRBioDatas.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var app in lstBioData)
            {
                BioData.Bio.AddBioRow(app.Gender,
                                      app.DateOfBirth.ToString(),
                                      app.MaritalStatus,
                                      app.NationalIdNo.ToString(),
                                      app.Ethnicity,
                                      app.Nationality,
                                      app.TelNo1.ToString(),
                                      app.PostalAddress,
                                      app.PostalCode.ToString(),
                                      app.EmailAddress,
                                      app.Town);
                
            }
            return BioData;
        }

        private object GetQuaificationData(DateTime fromDate, DateTime toDate)
        {
            var Qualification = new HumanResource.Reports.ApplicantResume.Qualification();
            var lstQualification = db.HRQualifications.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach(var app in lstQualification)
            {
                Qualification._Qualification.AddQualificationRow(app.EducationLevel,
                                                                 app.QualificationsAttained,
                                                                 app.GradeObtained,
                                                                 app.InstitutionAttended,
                                                                 app.GraduationYear.ToString(),
                                                                 app.DateOfQualification.ToString(),
                                                                 app.Institution,
                                                                 app.TitleCourse,
                                                                 app.Grade);
            }
            return Qualification;
        }

        private object GetWorkExperienceData(DateTime fromDate, DateTime toDate)
        {
            var WorkExperience = new HumanResource.Reports.ApplicantResume.WorkExperience();
            var lstWorkExperience = db.HRWorkExperiences.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var app in lstWorkExperience)
            {
                WorkExperience.Work.AddWorkRow(app.BeginDate.ToString(),
                                               app.EndDate.ToString(),
                                               app.Employer,
                                               app.Position,
                                               app.Salary.ToString());
            }
            return WorkExperience;
        }

        private object GetRefereesData(DateTime fromDate, DateTime toDate)
        {
            var Referees = new HumanResource.Reports.ApplicantResume.Referees();
            var lstReferees = db.HRReferees.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var app in lstReferees)
            {
                Referees._Referees.AddRefereesRow(app.Name,
                                                  app.Organization,
                                                  app.Title,
                                                  app.PostalAddress,
                                                  app.Telephone.ToString());
            }
            return Referees;
        }
        #endregion
        #region Memo Report
        public ActionResult Memo()
        {
            return View();
        }

        public class MemoViewModel
        {
            public string DateOfMemo { get; set; }
            public string Code { get; set; }
            public string Subject { get; set; }
            public string GradingOfMemo { get; set; }
            public string MemoDetails { get; set; }
           
        }
        public ActionResult GenerateMemoReport(DateTime FromDate, DateTime ToDate)

        {
            var fromDate = FromDate;
            var toDate = ToDate;

            var Memo = new HumanResource.Reports.Memo.DatasetMemo();
            var lstMemos = db.HRMemos.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var mem in lstMemos)
            {
                Memo.Memos.AddMemosRow(mem.DateOfMemo.ToString(),
                                       mem.Code,
                                       mem.Subject,
                                       mem.GradingOfMemo,
                                       mem.MemoDetails);  
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/HumanResource/Reports/MemoMemoReport.rpt")));
            
            rd.SetDataSource(Memo);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Memo Report" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);

        }
        #endregion
        #region Employees History
        public ActionResult EmployeeHistory()
        {
            return View();
        }

        public class EmployeeHistoryViewModel
        {
            public string StaffId { get; set; }
            public string EmployeeName { get; set; }
            public string Employer { get; set; }
            public string BeginDate { get; set; }
            public string EndDate { get; set; }
            public string Position { get; set; }
            public string Salary { get; set; }
            public string InterviewDate { get; set; }
            public string HireDate { get; set; }
            public string Designation { get; set; }
            public string JobGroup { get; set; }
            public string JobGrade { get; set; }
        }
        public ActionResult GenerateEmployeesHistoryReport(DateTime FromDate, DateTime ToDate)

        {
            var fromDate = FromDate;
            var toDate = ToDate;

            var Employee = new HumanResource.Reports.EmployeeHistory.DatasetEmployee();
            var lstEmployeesHistory = db.HRStaffWorkExperiences.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate)
                                .ToList();

            foreach (var emp in lstEmployeesHistory)
            {
                var his = db.HRFileDetails.FirstOrDefault(p => p.StaffId != null);
                Employee.Employee.AddEmployeeRow(emp.StaffId,
                                                 his.FirstName + "" +his.OtherNames,
                                                 emp.Employer,
                                                 emp.BeginDate.ToString(),
                                                 emp.EndDate.ToString(),
                                                 emp.Position,
                                                 emp.Salary.ToString(),
                                                 his.InterviewDate.ToString(),
                                                 his.HireDate.ToString(),
                                                 his.Designation,
                                                 his.JobGroup,
                                                 his.JobGrade);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/HumanResource/Reports/EmployeeHistory/EmployeeReport.rpt")));
            
            rd.SetDataSource(Employee);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            
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