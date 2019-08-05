using Caresoft2._0.Areas.CareSoftReports.Reports.HTCServiceReport;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.DepartmentalOutpatientAttendance;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.DiagnosisSummary;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.HTCServiceReport;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.InsuranceReport;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.PatientDiagnosisComments;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.PatientList;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.ServicesAndPrices;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.TBReport;
using Caresoft2._0.Areas.CareSoftReports.ViewModels;
using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.CrystalReports;
using Caresoft2._0.CrystalReports.ReportHeader;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using LabsDataAccess;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.CareSoftReports.Controllers
{
    [Auth]
    public class MISReportsController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();
        CaresoftHMISEntities db2 = new CaresoftHMISEntities();
        CareSoftLabsEntities db3 = new CareSoftLabsEntities();
        private object renal;

        // GET: CareSoftReports/MISReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientList()
        {
            PatientListDataModel patientListDataModel = new PatientListDataModel()
            {

                Employees = db2.Employees
                                .Where(p => p.Users.FirstOrDefault().UserRole.RoleName == "Consultant Doctor")
                                .OrderBy(p => p.FName)
                                .ToList(),
                Companies = db2.Companies
                                .Where(p => p.CompanyType.CompanyTypeName == "Insurance")
                                .ToList(),
                Departments = db2.Departments
                                .Where(p => p.IsMedical == "Yes").ToList()
            };
            return View(patientListDataModel);
        }

        public class PatientListViewModel
        {
            public string PatientsName { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public int OpdNo { get; set; }
            public DateTime EntryDate { get; set; }
            public string PatientType { get; set; }
            public string Scheme { get; set; }
            public string Department { get; set; }
            public string Clinician { get; set; }
            public double TotalBill { get; set; }
        }

        public ActionResult GeneratePatientListReport(DateTime FromDate, DateTime ToDate, string Clinician, string Department, string InsuranceCompany, string selectedDiv)
        {

            var dataSet = GetPatientListData(FromDate, ToDate, Clinician, Department, InsuranceCompany, selectedDiv);



            if (Session["UserId"] != null)
            {
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.HospitalDetails.AddHospitalDetailsRow(
                HospitalName, "", "");
            }
            else
            {
                dataSet.HospitalDetails.AddHospitalDetailsRow(
                "ABC FACILITY", "P.O.BOX 12345", "+254 71234567890");
            }



            ReportDocument Rd = new ReportDocument();

            if (selectedDiv == "Clinician")
            {
                Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientList/RptPatientLstClinician.rpt")));
            }
            else if (selectedDiv == "Department")
            {
                Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientList/RptPatientLstDepartment.rpt")));
            }
            else if (selectedDiv == "Insurance")
            {
                Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientList/RptPatientLstIInsuarance.rpt")));
            }
            //Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientList/RptPatientList.rpt")));
            //Rd.SetDataSource(dataSetHospitalDetails);
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Patient List " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        private DataSetPatientList GetPatientListData(DateTime FromDate, DateTime ToDate, string Clinician, string Department, string InsuranceCompany, string selectedOption)
        {
            DataSetPatientList dataSetPatientList = new DataSetPatientList();

            var opdRegisters = new List<OpdRegister>();

            if (selectedOption == "Clinician")
            {
                if (Clinician == "All")
                {
                    opdRegisters = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).ToList();
                }
                else
                {
                    opdRegisters = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate && p.ConsultantDoctor.Contains(Clinician)).ToList();
                }
            }
            else if (selectedOption == "Department")
            {
                if (Department == "All")
                {
                    opdRegisters = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).ToList();
                }
                else
                {
                    opdRegisters = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate && p.Department == Department).ToList();
                }
            }
            else if (selectedOption == "Insurance")
            {
                if (InsuranceCompany == "All")
                {
                    opdRegisters = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).ToList();
                }
                else
                {
                    opdRegisters = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate && p.Tariff.Company.CompanyName == InsuranceCompany).ToList();
                }
            }

            List<PatientListViewModel> listViewModels = new List<PatientListViewModel>();

            foreach (var item in opdRegisters)
            {
                if (item.Patient.DOB.HasValue)
                {
                    PatientListViewModel model = new PatientListViewModel()
                    {
                        PatientsName = item.Patient.FName + " " + item.Patient.LName,
                        Gender = item.Patient.Gender,
                        Age = (DateTime.Now.Year - item.Patient.DOB.Value.Year),
                        OpdNo = item.Id,
                        EntryDate = item.Date,
                        PatientType = item.Tariff.Company.CompanyType.CompanyTypeName,
                        Scheme = item.Tariff.Company.CompanyName,
                        Clinician = item.ConsultantDoctor,
                        Department = item.Department,
                        TotalBill = item.BillPayments.Sum(p => p.BillAmount)
                    };
                    listViewModels.Add(model);
                }

            }

            foreach (var item in listViewModels)
            {
                dataSetPatientList.PatientList.AddPatientListRow(
                    item.PatientsName,
                    item.Gender,
                    item.Age,
                    item.OpdNo.ToString(),
                    item.EntryDate,
                    item.PatientType,
                    item.Scheme,
                    item.Department,
                    Convert.ToInt32(item.TotalBill),
                    item.Clinician);
            }

            return dataSetPatientList;
        }

        public ActionResult PatientListByDepartment()
        {
            var Departments = db2.Departments
                                .Where(p => p.IsMedical == "Yes").ToList();
            return View(Departments);
        }

        public ActionResult GetPatientListByDepartmentReport(DateTime FromDate, DateTime ToDate, string Department)
        {
            var opdRegister = new List<OpdRegister>();

            if (FromDate != null && ToDate != null)
            {
                if (Department != "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Department == Department && p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p => p.PatientId).ToList();
                }
                else if (Department == "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p => p.PatientId).ToList();
                }
            }

            Reports.MISReports.PatientByDept.DataSetPatientsAttendantsByDepartment dataSet = new Reports.MISReports.PatientByDept.DataSetPatientsAttendantsByDepartment();

            foreach (var item in opdRegister)
            {
                if (item.Patient.DOB.HasValue)
                {
                    var pat = item.Patient;
                    var name = pat.FName + " " + pat.MName + " " + pat.LName;

                    var calcAge = FromDate.Year - pat.DOB.Value.Year;
                    string age = "";

                    if (calcAge > 0)
                    {
                        age = calcAge.ToString() + " Y";
                    }
                    else
                    {
                        calcAge = FromDate.Month - pat.DOB.Value.Month;
                        if (calcAge > 0)
                        {
                            age = calcAge.ToString() + " M";
                        }
                        else
                        {
                            calcAge = FromDate.Day - pat.DOB.Value.Day;
                            age = calcAge.ToString() + " D";
                        }
                    }

                    dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                       item.Patient.RegNumber,
                       name, age,
                       item.Patient.Gender,
                       item.Patient.HomeAddress,
                       item.Patient.RegDate.Value.ToString("dd-MMMM-yyyy")
                       );
                }
                else
                {
                    var pat = item.Patient;
                    var name = pat.FName + " " + pat.MName + " " + pat.LName;

                    string age = "Not Specified";

                    string regDate = "";

                    if (pat.RegDate.HasValue)
                        regDate = item.Patient.RegDate.Value.ToString("dd-MMMM-yyyy");
                    dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                       item.Patient.RegNumber,
                       name, age,
                       item.Patient.Gender,
                       item.Patient.HomeAddress,
                       regDate
                       );
                }

            }

            if (Session["UserId"] != null)
            {
                var Period = FromDate.ToString("dd-MM-yyyy") + " to " + ToDate.ToString("dd-MM-yyyy");
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.ReportMetaDataPatientsAttendance.AddReportMetaDataPatientsAttendanceRow(
                HospitalName,
                Department,
                Period);
            }

            ReportDocument Rd = new ReportDocument();


            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientByDept/RptPatientAttendanceByDept.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Patient List By Department " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult PatientListSheetNewRevisit()
        {
            var Departments = db2.Departments
                                 .Where(p => p.IsMedical == "Yes").ToList();
            return View(Departments);
        }

        public ActionResult GetPatientListSheetNewRevisitReport(DateTime FromDate, DateTime ToDate, string Department)
        {
            var opdRegister = new List<OpdRegister>();

            if (FromDate != null && ToDate != null)
            {
                if (Department != "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Department == Department && p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p => p.PatientId).ToList();
                }
                else if (Department == "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p => p.PatientId).ToList();
                }
            }

            Reports.MISReports.PatientListSheetNewRevisit.DataSetPatientsAttendantsByDepartment dataSet = new Reports.MISReports.PatientListSheetNewRevisit.DataSetPatientsAttendantsByDepartment();

            foreach (var item in opdRegister)
            {
                if (item.Patient.DOB.HasValue)
                {
                    var pat = item.Patient;
                    var name = pat.FName + " " + pat.MName + " " + pat.LName;

                    var calcAge = FromDate.Year - pat.DOB.Value.Year;
                    string age = "";

                    if (calcAge > 0)
                    {
                        age = calcAge.ToString() + " Y";
                    }
                    else
                    {
                        calcAge = FromDate.Month - pat.DOB.Value.Month;
                        if (calcAge > 0)
                        {
                            age = calcAge.ToString() + " M";
                        }
                        else
                        {
                            calcAge = FromDate.Day - pat.DOB.Value.Day;
                            age = calcAge.ToString() + " D";
                        }
                    }

                    string patientDiagnosis = "";

                    var theDiag = item.PatientDiagnosis.Where(p => p.OPDNo == item.Id).FirstOrDefault();

                    if (theDiag != null)
                    {
                        patientDiagnosis = theDiag.FinalDiagnosis;
                    }

                    dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                       item.Patient.RegNumber,
                       name.ToUpperInvariant(),
                       age,
                       item.Patient.Gender,
                       item.Patient.HomeAddress.ToUpper(),
                       item.Patient.RegDate.Value.ToString("dd-MMMM-yyyy"),
                       patientDiagnosis
                       );
                }
                else
                {
                    var pat = item.Patient;
                    var name = pat.FName + " " + pat.MName + " " + pat.LName;

                    string age = "Not Specified";

                    string regDate = "";

                    if (pat.RegDate.HasValue)
                        regDate = item.Patient.RegDate.Value.ToString("dd-MMMM-yyyy");


                    string patientDiagnosis = "";

                    var theDiag = item.PatientDiagnosis.Where(p => p.OPDNo == item.Id).FirstOrDefault();

                    if (theDiag != null)
                    {
                        patientDiagnosis = theDiag.FinalDiagnosis;
                    }

                    dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                       item.Patient.RegNumber,
                       name, age,
                       item.Patient.Gender,
                       item.Patient.HomeAddress,
                       regDate,
                       patientDiagnosis
                       );
                }

            }

            if (Session["UserId"] != null)
            {
                var Period = FromDate.ToString("dd-MM-yyyy") + " to " + ToDate.ToString("dd-MM-yyyy");
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.ReportMetaDataPatientsAttendance.AddReportMetaDataPatientsAttendanceRow(
                HospitalName,
                Department,
                Period);
            }

            ReportDocument Rd = new ReportDocument();


            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientListSheetNewRevisit/RptPatientListNewRevisit.rpt")));

            //Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientList/RptPatientList.rpt")));
            //Rd.SetDataSource(dataSetHospitalDetails);
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Patient List New and Revisit " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult PatientListSheetNew()
        {
            var Departments = db2.Departments
                                 .Where(p => p.IsMedical == "Yes").ToList();
            return View(Departments);
        }

        public ActionResult GetPatientListSheetNewReport(DateTime FromDate, DateTime ToDate, string Department)
        {
            var opdRegister = new List<OpdRegister>();

            if (FromDate != null && ToDate != null)
            {

                opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate && p.Patient.OpdRegisters.Count() == 1).DistinctBy(p => p.PatientId).ToList();

            }

            Reports.MISReports.PatientListSheetNewRevisit.DataSetPatientsAttendantsByDepartment dataSet = new Reports.MISReports.PatientListSheetNewRevisit.DataSetPatientsAttendantsByDepartment();

            foreach (var item in opdRegister)
            {
                if (item.Patient.DOB.HasValue)
                {
                    var pat = item.Patient;
                    var name = pat.FName + " " + pat.MName + " " + pat.LName;

                    var calcAge = FromDate.Year - pat.DOB.Value.Year;
                    string age = "";

                    if (calcAge > 0)
                    {
                        age = calcAge.ToString() + " Y";
                    }
                    else
                    {
                        calcAge = FromDate.Month - pat.DOB.Value.Month;
                        if (calcAge > 0)
                        {
                            age = calcAge.ToString() + " M";
                        }
                        else
                        {
                            calcAge = FromDate.Day - pat.DOB.Value.Day;
                            age = calcAge.ToString() + " D";
                        }
                    }

                    string patientDiagnosis = "";

                    var theDiag = item.PatientDiagnosis.Where(p => p.OPDNo == item.Id).FirstOrDefault();

                    if (theDiag != null)
                    {
                        patientDiagnosis = theDiag.FinalDiagnosis;
                    }

                    dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                       item.Patient.RegNumber,
                       name.ToUpperInvariant(),
                       age,
                       item.Patient.Gender,
                       item.Patient.HomeAddress.ToUpper(),
                       item.Patient.RegDate.Value.ToString("dd-MMMM-yyyy"),
                       patientDiagnosis
                       );
                }
                else
                {
                    var pat = item.Patient;
                    var name = pat.FName + " " + pat.MName + " " + pat.LName;

                    string age = "Not Specified";

                    string regDate = "";

                    if (pat.RegDate.HasValue)
                        regDate = item.Patient.RegDate.Value.ToString("dd-MMMM-yyyy");


                    string patientDiagnosis = "";

                    var theDiag = item.PatientDiagnosis.Where(p => p.OPDNo == item.Id).FirstOrDefault();

                    if (theDiag != null)
                    {
                        patientDiagnosis = theDiag.FinalDiagnosis;
                    }

                    dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                       item.Patient.RegNumber,
                       name, age,
                       item.Patient.Gender,
                       item.Patient.HomeAddress,
                       regDate,
                       patientDiagnosis
                       );
                }

            }

            if (Session["UserId"] != null)
            {
                var Period = FromDate.ToString("dd-MMM-yyyy") + " to " + ToDate.ToString("dd-MMM-yyyy");
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.ReportMetaDataPatientsAttendance.AddReportMetaDataPatientsAttendanceRow(
                HospitalName,
                Department,
                Period);
            }

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientListNew/RptPatientListNew.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Patient List New " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult DepartmentOutPatientAttendance()
        {
            return View();
        }

        public class DepartmentOutPatientAttendanceViewModel
        {
            public string DepartmentName { get; set; }
            public int MUnderOneYearN { get; set; }
            public int MUnderOneYearR { get; set; }
            public int MOneToFourYearsN { get; set; }
            public int MOneToFourYearsR { get; set; }
            public int MFiveToFourteenYearsN { get; set; }
            public int MFiveToFourteenYearsR { get; set; }
            public int MFifteenToFortyFourYearsN { get; set; }
            public int MFifteenToFourtyYearsR { get; set; }
            public int MFortyFiveToSixtyYearsN { get; set; }
            public int MFortyFiveToSixtyYearsR { get; set; }
            public int MOverSixtyYearsN { get; set; }
            public int MOverSixtyYearsR { get; set; }

            public int FUnderOneYearN { get; set; }
            public int FUnderOneYearR { get; set; }
            public int FOneToFourYearsN { get; set; }
            public int FOneToFourYearsR { get; set; }
            public int FFiveToFourteenYearsN { get; set; }
            public int FFiveToFourteenYearsR { get; set; }
            public int FFifteenToFortyFourYearsN { get; set; }
            public int FFifteenToFourtyYearsR { get; set; }
            public int FFortyFiveToSixtyYearsN { get; set; }
            public int FFortyFiveToSixtyYearsR { get; set; }
            public int FOverSixtyYearsN { get; set; }
            public int FOverSixtyYearsR { get; set; }
        }

        public ActionResult GetDepartmentOutPatientAttendanceReport(DateTime FromDate, DateTime ToDate, string Department)
        {

            List<DepartmentOutPatientAttendanceViewModel> viewModels = new List<DepartmentOutPatientAttendanceViewModel>();

            var Departments = db2.Departments
                                 .Where(p => p.IsMedical == "Yes").ToList();


            var opdRegister = new List<OpdRegister>();

            if (FromDate != null && ToDate != null)
            {
                opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate && p.IsIPD == false).ToList();
            }


            foreach (var item in Departments)
            {
                DepartmentOutPatientAttendanceViewModel model = new DepartmentOutPatientAttendanceViewModel();

                model.DepartmentName = item.DepartmentName;

                foreach (var opd in opdRegister)
                {
                    if (item.DepartmentName == "OUTPATIENT" || item.DepartmentName == null || item.DepartmentName == "")
                    {

                        if (opd.Patient.Gender == "M")
                        {
                            if (opd.Patient.OpdRegisters.Count() > 1)
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                    if (calcAge < 1)

                                    {
                                        model.MUnderOneYearR++;
                                    }
                                    else if (calcAge > 0 && calcAge < 5)
                                    {
                                        model.MOneToFourYearsR++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {

                                        {
                                            model.MUnderOneYearR++;
                                        }
                                    }
                                    else if (calcAge > 0 && calcAge < 5)
                                    {
                                        model.MOneToFourYearsR++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {

                                        model.MFiveToFourteenYearsR++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {
                                        model.MFifteenToFourtyYearsR++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {
                                        model.MFortyFiveToSixtyYearsR++;
                                    }
                                    else if (calcAge > 60)
                                    {
                                        model.MOverSixtyYearsR++;
                                    }
                                }
                            }
                            else
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                    if (calcAge < 1)
                                    {
                                        model.MUnderOneYearN++;
                                    }
                                    else if (calcAge > 0 && calcAge < 5)
                                    {
                                        model.MOneToFourYearsN++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)

                                    {
                                        model.MFiveToFourteenYearsN++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {

                                        {
                                            model.MFiveToFourteenYearsN++;
                                        }
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {


                                        model.MFifteenToFortyFourYearsN++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {
                                        model.MFortyFiveToSixtyYearsN++;
                                    }
                                    else if (calcAge > 60)
                                    {
                                        model.MOverSixtyYearsN++;
                                    }
                                }
                            }
                        }
                        else if (opd.Patient.Gender == "F")
                        {
                            if (opd.Patient.OpdRegisters.Count() > 1)
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                    if (calcAge < 1)
                                    {
                                        model.FUnderOneYearR++;
                                    }
                                    else if (calcAge > 0 && calcAge < 5)
                                    {
                                        model.FOneToFourYearsR++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.FFiveToFourteenYearsR++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)

                                    {
                                        model.FFifteenToFourtyYearsR++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {
                                        model.FFortyFiveToSixtyYearsR++;
                                    }
                                    else if (calcAge > 60)
                                    {
                                        {
                                            model.FFifteenToFourtyYearsR++;
                                        }
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {
                                        model.FFortyFiveToSixtyYearsR++;
                                    }
                                    else if (calcAge > 60)
                                    {

                                        model.FOverSixtyYearsR++;
                                    }
                                }
                            }
                            else
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                    if (calcAge < 1)

                                    {
                                        model.FUnderOneYearN++;
                                    }
                                    else if (calcAge > 0 && calcAge < 5)
                                    {
                                        model.FOneToFourYearsN++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.FFiveToFourteenYearsN++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {
                                        {
                                            model.FUnderOneYearN++;
                                        }
                                    }
                                    else if (calcAge > 0 && calcAge < 5)
                                    {
                                        model.FOneToFourYearsN++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.FFiveToFourteenYearsN++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {

                                        model.FFifteenToFortyFourYearsN++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {
                                        model.FFortyFiveToSixtyYearsN++;
                                    }
                                    else if (calcAge > 60)
                                    {
                                        model.FOverSixtyYearsN++;
                                    }
                                }
                            }
                        }
                    }
                    else if (item.DepartmentName == opd.Department)
                    {
                        if (opd.Patient.Gender == "M")
                        {
                            if (opd.Patient.OpdRegisters.Count() > 1)
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                    if (calcAge < 1)
                                    {
                                        model.MUnderOneYearR++;
                                    }
                                    else if (calcAge > 0 && calcAge < 5)

                                    {
                                        model.MOneToFourYearsR++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.MFiveToFourteenYearsR++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {
                                        model.MFifteenToFourtyYearsR++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {

                                        {
                                            model.MOneToFourYearsR++;
                                        }
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.MFiveToFourteenYearsR++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {
                                        model.MFifteenToFourtyYearsR++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {

                                        model.MFortyFiveToSixtyYearsR++;
                                    }
                                    else if (calcAge > 60)
                                    {
                                        model.MOverSixtyYearsR++;
                                    }
                                }
                            }
                            else
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                    if (calcAge < 1)
                                    {
                                        model.MUnderOneYearN++;
                                    }
                                    else if (calcAge > 0 && calcAge < 5)

                                    {
                                        model.MOneToFourYearsN++;
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.MFiveToFourteenYearsN++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {
                                        model.MFifteenToFortyFourYearsN++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {
                                        {
                                            model.MOneToFourYearsN++;
                                        }
                                    }
                                    else if (calcAge > 4 && calcAge < 15)
                                    {
                                        model.MFiveToFourteenYearsN++;
                                    }
                                    else if (calcAge > 14 && calcAge < 46)
                                    {
                                        model.MFifteenToFortyFourYearsN++;
                                    }
                                    else if (calcAge > 44 && calcAge < 61)
                                    {

                                        model.MFortyFiveToSixtyYearsN++;
                                    }
                                    else if (calcAge > 60)
                                    {
                                        model.MOverSixtyYearsN++;
                                    }
                                }
                            }
                        }
                        else if (opd.Patient.Gender == "F")
                        {
                            if (opd.Patient.OpdRegisters.Count() > 1)
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                                    if (calcAge < 1)

                                                    {
                                                        model.FUnderOneYearR++;
                                                    }
                                                    else if (calcAge > 0 && calcAge < 5)
                                                    {
                                                        model.FOneToFourYearsR++;
                                                    }
                                                    else if (calcAge > 4 && calcAge < 15)
                                                    {
                                                        model.FFiveToFourteenYearsR++;
                                                    }
                                                    else if (calcAge > 14 && calcAge < 46)
                                                    {
                                                        model.FFifteenToFourtyYearsR++;
                                                    }
                                                    else if (calcAge > 44 && calcAge < 61)
                                                    {
                                                        model.FFortyFiveToSixtyYearsR++;
                                                    }
                                                    else if (calcAge > 60)
                                                    {

                                                        {
                                                            model.FUnderOneYearR++;
                                                        }
                                                    }
                                                    else if (calcAge > 0 && calcAge < 5)
                                                    {
                                                        model.FOneToFourYearsR++;
                                                    }
                                                    else if (calcAge > 4 && calcAge < 15)
                                                    {
                                                        model.FFiveToFourteenYearsR++;
                                                    }
                                                    else if (calcAge > 14 && calcAge < 46)
                                                    {
                                                        model.FFifteenToFourtyYearsR++;
                                                    }
                                                    else if (calcAge > 44 && calcAge < 61)
                                                    {
                                                        model.FFortyFiveToSixtyYearsR++;
                                                    }
                                                    else if (calcAge > 60)
                                                    {

                                                        model.FOverSixtyYearsR++;
                                                    }
                                }
                            }
                            else
                            {
                                if (opd.Patient.DOB.HasValue)
                                {
                                    var pat = opd.Patient;

                                    var calcAge = FromDate.Year - pat.DOB.Value.Year;

                                                    if (calcAge < 1)

                                                    {
                                                        model.FUnderOneYearN++;
                                                    }
                                                    else if (calcAge > 0 && calcAge < 5)
                                                    {
                                                        model.FOneToFourYearsN++;
                                                    }
                                                    else if (calcAge > 4 && calcAge < 15)
                                                    {
                                                        model.FFiveToFourteenYearsN++;
                                                    }
                                                    else if (calcAge > 14 && calcAge < 46)
                                                    {
                                                        model.FFifteenToFortyFourYearsN++;
                                                    }
                                                    else if (calcAge > 44 && calcAge < 61)
                                                    {
                                                        model.FFortyFiveToSixtyYearsN++;
                                                    }
                                                    else if (calcAge > 60)
                                                    {

                                                        {
                                                            model.FUnderOneYearN++;
                                                        }
                                                    }
                                                    else if (calcAge > 0 && calcAge < 5)
                                                    {
                                                        model.FOneToFourYearsN++;
                                                    }
                                                    else if (calcAge > 4 && calcAge < 15)
                                                    {
                                                        model.FFiveToFourteenYearsN++;
                                                    }
                                                    else if (calcAge > 14 && calcAge < 46)
                                                    {
                                                        model.FFifteenToFortyFourYearsN++;
                                                    }
                                                    else if (calcAge > 44 && calcAge < 61)
                                                    {
                                                        model.FFortyFiveToSixtyYearsN++;
                                                    }
                                                    else if (calcAge > 60)
                                                    {

                                                        model.FOverSixtyYearsN++;
                                                    }
                                }
                            }
                        }
                    }
                }

                viewModels.Add(model);

            }

            DataSetDepartmentalOuptaptient dataSet = new DataSetDepartmentalOuptaptient();

            foreach (var item in viewModels)
            {
                dataSet.DepartmentalOutpatient.AddDepartmentalOutpatientRow(
                   item.DepartmentName,
                   item.MUnderOneYearN,
                   item.MUnderOneYearR,
                   item.MOneToFourYearsN,
                   item.MOneToFourYearsR,
                   item.MFiveToFourteenYearsN,
                   item.MFiveToFourteenYearsR,
                   item.MFifteenToFortyFourYearsN,
                   item.MFifteenToFourtyYearsR,
                   item.MFortyFiveToSixtyYearsN,
                   item.MFortyFiveToSixtyYearsR,
                   item.MOverSixtyYearsN,
                   item.MOverSixtyYearsR,

                   item.FUnderOneYearN,
                   item.FUnderOneYearR,
                   item.FOneToFourYearsN,
                   item.FOneToFourYearsR,
                   item.FFiveToFourteenYearsN,
                   item.FFiveToFourteenYearsR,
                   item.FFifteenToFortyFourYearsN,
                   item.FFifteenToFourtyYearsR,
                   item.FFortyFiveToSixtyYearsN,
                   item.FFortyFiveToSixtyYearsR,
                   item.FOverSixtyYearsN,
                   item.FOverSixtyYearsR);
            }



            if (Session["UserId"] != null)
            {
                var Period = FromDate.ToString("dd-MM-yyyy") + " to " + ToDate.ToString("dd-MM-yyyy");
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.ReportMetaDataPatientsAttendance.AddReportMetaDataPatientsAttendanceRow(
                HospitalName,
                ToDate.ToMonthName(),
                ToDate.Year.ToString()
                );
            }

            ReportDocument Rd = new ReportDocument();


            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/DepartmentalOutpatientAttendance/RptDepartmentalOutpatientAttendance.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Department Outpatient Attendance " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public ActionResult DiseasesMobidity()
        {
            var Departments = db2.Departments
                                .Where(p => p.IsMedical == "Yes").ToList();
            return View(Departments);
        }

        public class DiseasesSummaryViewModel
        {
            public string DiseaseName { get; set; }
            public int NumberOfCases { get; set; }
            public double Percentage { get; set; }
        }

        public ActionResult GetDiseasesMobilityReport(DateTime FromDate, DateTime ToDate, string Department)
        {
            DataSetDiseaseSummary dataSet = new DataSetDiseaseSummary();

            List<DiseasesSummaryViewModel> lstModel = new List<DiseasesSummaryViewModel>();

            var allDiseases = db2.Diseases.ToList();
            var AllDiagnosis = new List<PatientDiagnosi>();

            if (Department == null || Department == "" || Department == "All")
            {
                AllDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded > FromDate && p.TimeAdded <= ToDate).ToList();
            }
            else
            {
                AllDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded > FromDate && p.TimeAdded <= ToDate && p.OpdRegister.Department == Department).ToList();
            }

            foreach (var disease in allDiseases)
            {
                DiseasesSummaryViewModel model = new DiseasesSummaryViewModel()
                {
                    DiseaseName = disease.DiseaseName
                };

                foreach (var diagnosis in AllDiagnosis)
                {
                    if (diagnosis.FinalDiagnosis.Contains(disease.DiseaseName))
                    {
                        model.NumberOfCases++;
                    }
                }

                lstModel.Add(model);

            }

            var totalNumberOfCases = lstModel.Sum(p => p.NumberOfCases);

            lstModel = lstModel.OrderByDescending(p => p.NumberOfCases).ToList();

            foreach (var mod in lstModel)
            {
                if (mod.NumberOfCases > 0)
                {
                    var perc = Decimal.Divide(mod.NumberOfCases, totalNumberOfCases);
                    mod.Percentage = (double)(perc * 100);
                }
                else
                {
                    mod.Percentage = 0;
                }
            }

            foreach (var item in lstModel)
            {
                dataSet.DiseasesSummary.AddDiseasesSummaryRow(
                    item.DiseaseName,
                    item.NumberOfCases,
                    item.Percentage);
            }

            Department = Department ?? "All";

            if (Session["UserId"] != null)
            {
                var Period = FromDate.ToString("dd-MMM-yyyy") + " to " + ToDate.ToString("dd-MMM-yyyy");
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.ReportMetaDataPatientsAttendance.AddReportMetaDataPatientsAttendanceRow(
                HospitalName,
                Period,
                Department
                );
            }

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/DiagnosisSummary/RptDiagnosisSummary.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Diagnosis Mobidity " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);

        }

        public ActionResult PatientListDiagnosisCommentsRevisit()
        {

            return View();
        }

        public ActionResult GetPatientListDiagnosisCommentsReport(DateTime FromDate, DateTime ToDate, string Department)
        {
            var opdRegister = new List<OpdRegister>();

            if (FromDate != null && ToDate != null)
            {
                if (Department == "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Department == Department && p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p => p.PatientId).ToList();
                }
                else if (Department == "All" || Department == null)
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p => p.PatientId).ToList();
                }
            }

            var dataSet = new DataSetPatientDiagnosisWithDoctorsComments();
            var facilityDataSet = HeaderAndFooterForReports.GetAllReportHeader();

            foreach (var item in opdRegister)
            {

                var pat = item.Patient;
                var name = pat.FName + " " + pat.MName + " " + pat.LName;

                var chiefComplaints = item.Complaints.Where(p => p.OpdIpdNumber == item.Id).FirstOrDefault()?.ChiefComplaints;
                var patientDiagnosis = item.PatientDiagnosis.Where(p => p.OPDNo == item.Id).FirstOrDefault()?.FinalDiagnosis;

                dataSet.PatientsAttendance.AddPatientsAttendanceRow(
                    item.Patient.RegNumber,
                    name,
                    chiefComplaints,
                    item.TimeAdded.Value.ToString("dd-MMM-yyyy"),
                    patientDiagnosis
                    );
            }

            if (Session["UserId"] != null)
            {
                var Period = FromDate.ToString("dd-MM-yyyy") + " to " + ToDate.ToString("dd-MM-yyyy");
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                dataSet.ReportMetaDataPatientsAttendance.AddReportMetaDataPatientsAttendanceRow(
                HospitalName,
                Department,
                Period);
            }

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/PatientDiagnosisComments/RptPatientDiagnosisComments.rpt")));

            var path = Path.Combine(Server.MapPath("~/Content/icons/HospitalLogo.png"));

            //Rd.SetParameterValue("MainReportLogoUrl", path);
            //Rd.SetParameterValue("logoUrl", path, "RptReportHeader.rpt");
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());

            Rd.SetDataSource(dataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Patient Diagnosis with Complains" + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }


        private DataSetFacilityInformation GetAllReportHeader()
        {

            var facilityDataSet = new DataSetFacilityInformation();
            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            var facilityAddress = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityAddress").FirstOrDefault().Value;

            var facilityTelephone = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityContactNumber").FirstOrDefault().Value;

            var logoUrl = Path.Combine(Server.MapPath("~/Content/icons/HospitalLogo.png"));

            facilityDataSet.HospitalDetails.AddHospitalDetailsRow(
                HospitalName,
                facilityAddress,
                facilityTelephone,
                logoUrl
            );

            return facilityDataSet;
        }

        public ActionResult InsuranceReport()
        {
            ViewBag.Insurance = db2.Companies.ToList();
            var insuranceCompanies = db2.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim() == "insurance").ToList();

            return View(insuranceCompanies);
        }

        public class InsuranceReportsViewModel
        {
            public string InvoiceNo { get; set; }
            public DateTime Date { get; set; }
            public string RegNo { get; set; }
            public string MembershipNo { get; set; }
            public string PatientName { get; set; }
            public double Consultation { get; set; }
            public double Lab { get; set; }
            public double x_ray { get; set; }
            public double services { get; set; }
            public double drugs { get; set; }
        }

        public ActionResult GetInsuranceReport(DateTime FromDate, DateTime ToDate, string Department, string Format)
        {
            var dataSet = GetInsuranceReportData(FromDate, ToDate, Department);
            InsuranceReportsViewModel viewModel = new InsuranceReportsViewModel();

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/InsuranceReport/RptInsurance.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            if (Format != "PDF")
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "Insurance " + DateTime.Now.ToString("dd-MM-yyyy") + " .xlsx";

                return File(Stream, "application/xlsx", FileName);
            }
            else
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "Insurance" + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

                return File(Stream, "application/pdf", FileName);
            }
        }

        public class InsuranceReportVM
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

        public DataSetInsuranceReport GetInsuranceReportData(DateTime FromDate, DateTime ToDate, string Insurance)
        {
            // ToDate = ToDate.AddDays(1);
            var dataSet = new DataSetInsuranceReport();
           
            var data = new List<InsuranceReportVM>();
            var lServices = db2.BillServices.Where(e => ((e.Award > 0 && e.DateAdded >= FromDate && e.DateAdded < ToDate) ||
            (e.OpdRegister.Medications.Any(f => f.Award > 0 && e.DateAdded >= FromDate && e.DateAdded < ToDate)))).ToList();
            var meds = db2.Medications.Where(e => e.Award > 0 && e.TimeAdded >= FromDate && e.TimeAdded < ToDate).ToList();

            if (Insurance.ToLower() != "all")
            {
                lServices = lServices.Where(p => p.OpdRegister.Tariff.Company.CompanyName == Insurance).ToList();
                meds = meds.Where(p => p.OpdRegister.Tariff.Company.CompanyName == Insurance).ToList();
            }

            var services = lServices.GroupBy(e => e.OPDNo);

            foreach (var s in services)
            {
                if (s.FirstOrDefault(e => e.OpdRegister.InsuranceCards.Count() > 0) != null)
                {
                    var Pat = s.FirstOrDefault().OpdRegister.Patient;
                    var patientsName = Pat.FName + " " + Pat.LName;
                    var entry = new InsuranceReportVM();
                    entry.InvoiceNo = s.FirstOrDefault().OpdRegister.InsuranceCards.OrderByDescending(e => e.Id).FirstOrDefault().Id;
                    entry.OPDNo = s.FirstOrDefault().OPDNo;
                    entry.MembershipNo = s.FirstOrDefault().OpdRegister.InsuranceCards.OrderByDescending(e => e.Id).FirstOrDefault().MemberNo;
                    entry.InsuranceCompany = s.FirstOrDefault().OpdRegister.Tariff.Company.CompanyName;
                    entry.SchemeName = s.FirstOrDefault().OpdRegister.Tariff.TariffName;
                    entry.DateAdded = s.FirstOrDefault().DateAdded;
                    entry.RegNumber = s.FirstOrDefault().OpdRegister.Patient.RegNumber;
                    entry.PatientName = patientsName.ToUpper();
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

            foreach (var d in data)
            {
                var m = meds.Where(e => e.OPDNo == d.OPDNo);
                if (m.Count() > 0)
                {
                    d.Drugs = m.Sum(e => e.Award * e.QuantityIssued ?? 0);
                }
            }


            dataSet.InsuranceDetails.AddInsuranceDetailsRow(
                Insurance,
                FromDate.ToString("dd-MMM-yyyy"),
                ToDate.ToString("dd-MMM-yyyy"));


            foreach (var item in data)
            {
                dataSet.InsurancePatientsDetails.AddInsurancePatientsDetailsRow(
                  item.InvoiceNo.ToString(),
                  item.DateAdded,
                  item.RegNumber,
                  item.MembershipNo,
                  item.PatientName,
                  item.Consultation,
                  item.Lab,
                  item.XRAY,
                  item.Others,
                  item.Drugs
                  );
            }

            return dataSet;
        }

        public ActionResult TbReport()
        {
            var insuranceCompanies = db2.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim() == "insurance").ToList();
            return View(insuranceCompanies);
        }

        public ActionResult GetTBReport(DateTime FromDate, DateTime ToDate, string Department, string Format)
        {
            var dataSet = GetTBReportData(FromDate, ToDate, Department);

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/TBReport/RptTBReports.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            if (Format != "PDF")
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "TB Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .xlsx";

                return File(Stream, "application/xlsx", FileName);
            }
            else
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "TB Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

                return File(Stream, "application/pdf", FileName);
            }

        }

        public class TBDataViewModel
        {
            public string CategoryName { get; set; }
            public int ChildrenLessThanFourteenYearsFemale { get; set; }
            public int ChildrenLessThanFourteenYearsMale { get; set; }
            public int AdultGreaterThanFourteenYearsFemale { get; set; }
            public int AdultGreaterThanFourteenYearsMale { get; set; }
            public int TotalFemale { get; set; }
            public int TotalMale { get; set; }
        }
        public DataSetTbReport GetTBReportData(DateTime FromDate, DateTime ToDate, string Department)
        {
            var dataSet = new DataSetTbReport();
            var lstTBViewModel = new List<TBDataViewModel>();

            var data = db2.TBScreeningResponses
                 .Where(p => DbFunctions.TruncateTime(p.DateAdded) >= FromDate && DbFunctions.TruncateTime(p.DateAdded) <= ToDate)
                 .DistinctBy(x => x.OPDNo)
                 .ToList();


            TBDataViewModel tBCasesDetected = new TBDataViewModel()
            {
                CategoryName = "TB Cases Detected"
            };

            foreach (var item in data)
            {
                if (item.OpdRegister.Patient.Gender == "M")
                {
                    var age = DateTime.Now.Year - item.OpdRegister.Patient.DOB.Value.Year;
                    if (age <= 14)
                    {
                        tBCasesDetected.ChildrenLessThanFourteenYearsMale++;
                    }
                    else
                    {
                        tBCasesDetected.AdultGreaterThanFourteenYearsMale++;
                    }
                }
                else
                {
                    var age = DateTime.Now.Year - item.OpdRegister.Patient.DOB.Value.Year;
                    if (age <= 14)
                    {
                        tBCasesDetected.ChildrenLessThanFourteenYearsFemale++;
                    }
                    else
                    {
                        tBCasesDetected.AdultGreaterThanFourteenYearsFemale++;
                    }
                }


            }

            lstTBViewModel.Add(tBCasesDetected);

            TBDataViewModel smearPositiveTB = new TBDataViewModel()
            {
                CategoryName = "Smear Positive TB"
            };

            var smearPositiveData = db3.WorkOrders
                .Where(p => DbFunctions.TruncateTime(p.CreatedUtc.Value) > FromDate && DbFunctions.TruncateTime(p.CreatedUtc.Value) <= ToDate)
                .ToString();


            foreach (var item in lstTBViewModel)
            {
                dataSet.PatientsAnalysis.AddPatientsAnalysisRow(
                 item.CategoryName,
                 item.ChildrenLessThanFourteenYearsMale,
                 item.ChildrenLessThanFourteenYearsFemale,
                 item.AdultGreaterThanFourteenYearsMale,
                 item.AdultGreaterThanFourteenYearsFemale);
            }
            return dataSet;
        }

        public ActionResult ServicesAndPrices()
        {
            var model = new DepartmentsAndServiceGroups()
            {
                Departments = db2.Departments.Where(p => p.IsMedical == "Yes").ToList(),
                ServiceGroups = db2.ServiceGroups.ToList()
            };
            return View(model);
        }

        public ActionResult GetServicesAndPricesReport(int department, int serviceGroup)
        {
            var dataSet = GetServicesAndPricesData(department, serviceGroup);

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/ServicesAndPrices/RptServicePricing.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Services And Prices " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        class ServicesAndPricesViewModel
        {
            public string ServiceName { get; set; }
            public string ServiceGroup { get; set; }
            public string Department { get; set; }
            public double Price { get; set; }
        }
        public DataSetServicesAndPrices GetServicesAndPricesData(int department, int serviceGroup)
        {
            DataSetServicesAndPrices dataSet = new DataSetServicesAndPrices();
            List<ServicesAndPricesViewModel> lstServicesAndPrices = new List<ServicesAndPricesViewModel>();
            var ServicesData = new List<Service>();

            if (department == 0 && serviceGroup == 0)
            {
                ServicesData = db2.Services.OrderBy(p => p.DepartmentId).ToList();
            }
            else if (department > 0 && serviceGroup > 0)
            {
                ServicesData = db2.Services.Where(p => p.DepartmentId == department && p.ServiceGroupId == serviceGroup).OrderBy(p => p.DepartmentId).ToList();
            }
            else if (department > 0 && serviceGroup == 0)
            {
                ServicesData = db2.Services.Where(p => p.DepartmentId == department).OrderBy(p => p.DepartmentId).ToList();
            }
            else if (department == 0 && serviceGroup > 0)
            {
                ServicesData = db2.Services.Where(p => p.ServiceGroupId == serviceGroup).OrderBy(p => p.DepartmentId).ToList();
            }

            foreach (var service in ServicesData)
            {
                ServicesAndPricesViewModel viewModel = new ServicesAndPricesViewModel()
                {
                    ServiceName = service.ServiceName,
                    ServiceGroup = service.ServiceGroup.ServiceGroupName,
                    Department = service.Department.DepartmentName,
                    Price = service.CashPrice
                };

                lstServicesAndPrices.Add(viewModel);
            }


            foreach (var item in lstServicesAndPrices)
            {
                dataSet.ServiceAndPrices.AddServiceAndPricesRow(

                    item.ServiceGroup,
                    item.Department,
                    item.ServiceName,
                    item.Price);
            }

            return dataSet;
        }

        #region Renal Report
        public ActionResult RenalReport()
        {
            return View();
        }
        public class RenalViewModel
        {
            public string FirstVisit { get; set; }
            public string Revisit { get; set; }
            public string DialysisDoneCount { get; set; }

        }
        public ActionResult GenerateRenalReport(DateTime FromDate, DateTime ToDate)

        {
            DatasetRenal datasetRenal = new DatasetRenal();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/RenalReport/RReport.rpt")));

            rd.SetDataSource(Renal);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = " Renal Report" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }
        private object Renal
        {
            get
            {
                DatasetRenal datasetRenal = new DatasetRenal();

                var FirstVisit = db2.RenalPatientProfiles.Count(e => e.Id > 0);
                var Revisit = db2.OpdRegisters.Count(e => e.Department == "Renal Procedure" && e.Patient.OpdRegisters.Count > 1);
                var DialysisDoneCount = db2.RenalDialysisOrders.Count(e => e.Id > 0);

                //datasetRenal._DatasetRenal.(FirstVisit,
                //                                              Revisit,
                //                                              DialysisDoneCount
                //                                              );
                return datasetRenal;
            }

        }

        #endregion

        #region TB Screening Report

        public ActionResult TBScreening()
        {
            return View();
        }
        public class TBScreeningViewModel
        {
            public string TotalScreened { get; set; }
            public string PatientName { get; set; }
            public string OPDNo { get; set; }
            public string TestResults { get; set; }
            public string DateTested { get; set; }

        }
        public ActionResult GenerateTBScreeningReport(DateTime FromDate, DateTime ToDate)

        {
            TBS tBS = new TBS();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MISReports/TBScreeningReport/TBScreenReport.rpt")));

            rd.SetDataSource(TB);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "TB Screening Report" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }
        private object TB
        {
            get
            {
                TBS tBS = new TBS();

                var AllPatientsScreened = db2.TBScreeningResponses.Count(e => e.Id > 0);
                var ListPatientsScreened = db2.OutsideTests.Where(e => e.TestName == "SPITUM FOR GENE EXPERT").ToList();

                foreach (var pat in ListPatientsScreened)
                {

                    //    tBS.TBScreening.AddTBScreeningRow(AllPatientsScreened.ToString(),
                    //                                       pat.OpdRegister.Patient.FName + "" + pat.OpdRegister.Patient.LName,
                    //                                       pat.OPDId.ToString(),
                    //                                       pat.TestResults.ToString(),
                    //                                       pat.CreatedDate.ToString()
                    //                                       );
                }
                return tBS;
            }
        }

            #endregion
            #region HTC Service Summary Report
             public ActionResult HTC()
             {
                 return View();
            }

        public ActionResult GenerateHTCServiceSummaryReport(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            DatasetHT datasetHT = new DatasetHT();
            var One = 0;
            var Two = 0;
            var Three = 0;
            var Four = 0;
            var Five = 0;
            var Six = 0;
            var Seven = 0;
            var Eight = 0;
            var Nine = 0;
            var Ten = 0;
            var HundredThree = 0;
            var Eleven = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "Y" && (e.DateAdded.Year-e.OpdRegister.Patient.DOB.Value.Year)<15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Twelve = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Thirteen = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Fourteen = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Fifteen = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Sixteen = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Seventeen = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50);
            var Eighteen = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50);
            var Nineteen = Eleven + Thirteen + Fifteen + Seventeen;
            var Twenty = Twelve + Fourteen + Sixteen + Eighteen;
            var HundredFour = Nineteen + Twenty;
            var TwentyOne = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15);
            var TwentyTwo = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15);
            var TwentyThree = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TwentyFour = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TwentyFive = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TwentySix = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TwentySeven = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TwentyEight = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TwentyNine = TwentyOne + TwentyThree + TwentyFive + TwentySeven;
            var Thirty = TwentyTwo + TwentyFour + TwentySix + TwentyEight;
            var HundredFive = TwentyNine + Thirty;
            var ThirtyOne = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtyTwo = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtyThree = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtyFour = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtyFive = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtySix = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtySeven = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtyEight = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ThirtyNine = ThirtyOne + ThirtyThree + ThirtyFive + ThirtySeven;
            var Fourty = ThirtyTwo + ThirtyFour + ThirtySix + ThirtyEight;
            var HundredSix = ThirtyNine + Fourty;
            var FourtyOne = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtyTwo = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtyThree = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtyFour = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtyFive = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtySix = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtySeven = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtyEight = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResultGiven == "Y" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FourtyNine = FourtyOne + FourtyThree + FourtyFive + FourtySeven;
            var Fifty = FourtyTwo + FourtyFour + FourtySix + FourtyEight;
            var HundredSeven = FourtyNine + Fifty;
            var FiftyOne = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftyTwo = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftyThree = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftyFour = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftyFive = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftySix = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftySeven = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftyEight = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FiftyNine = FiftyOne + FiftyThree + FiftyFive + FiftySeven;
            var Sixty = FiftyTwo + FiftyFour + FiftySix + FiftyEight;
            var HundredEight = FiftyNine + Sixty;
            var SixtyOne = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtyTwo = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) < 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtyThree = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtyFour = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 24 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtyFive = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtySix = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 24 && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) <= 49 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtySeven = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "M" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtyEight = db2.HTCServiceSummaries.Count(e => e.OpdRegister.Patient.Gender == "F" && e.FinalResult == "P" && e.EverTested == "N" && (e.DateAdded.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var SixtyNine = SixtyOne + SixtyThree + SixtyFive + SixtySeven;
            var Seventy = SixtyTwo + SixtyFour + SixtySix + SixtyEight;
            var HundredNine = SixtyNine + Seventy;
            var SeventyOne = 0;
            var SeventyTwo = 0;
            var SeventyThree = 0;
            var SeventyFour = 0;
            var SeventyFive = 0;
            var SeventySix = 0;
            var SeventySeven = 0;
            var SeventyEight = 0;
            var SeventyNine = 0;
            var Eighty = 0;
            var HundredTen = 0;
            var EightyOne = 0;
            var EightyTwo = 0;
            var EightyThree = 0;
            var EightyFour = 0;
            var EightyFive = 0;
            var EightySix = 0;
            var EightySeven = 0;
            var EightyEight = 0;
            var EightyNine = 0;
            var Ninety = 0;
            var HundredEleven = 0;
            var NinetyOne = 0;
            var NinetyTwo = 0;
            var NinetyThree = 0;
            var NinetyFour = 0;
            var NinetyFive = 0;
            var NinetySix = 0;
            var NinetySeven = 0;
            var NinetyEight = 0;
            var NinetyNine = 0;
            var Hundred = 0;
            var HundredTwelve = 0;
            var HundredOne = 0;
            var HundredTwo = db2.HTCServiceSummaries.Count(e => e.CoupleDiscordant == "Y");
            var HundredThirteen = db2.HTCServiceSummaries.Count(e => e.HIVTest1 == "N");
            var HundredFourteen = db2.HTCServiceSummaries.Count(e => e.HIVTest2 == "N");
            var HundredFifteen = 0;
            var HundredSixteen = db2.HTCServiceSummaries.Count(e => e.HIVTest1 == "P");
            var HundredSeventeen = db2.HTCServiceSummaries.Count(e => e.HIVTest2 == "P");
            var HundredEighteen = 0;
            var HundredNineteen = db2.HTCServiceSummaries.Count(e => e.HIVTest1 == "I");
            var HundredTwenty = db2.HTCServiceSummaries.Count(e => e.HIVTest2 == "I");
            var Hundred21 = 0;
            var Hundred22 = 0;
            var Hundred23 = 0;
            var Hundred24 = 0;
            var Hundred25 = HundredThirteen + HundredSixteen + HundredNineteen + Hundred21;
            var Hundred26 = HundredFourteen + HundredSeventeen + HundredTwenty + Hundred23;
            var Hundred27 = 0;

            datasetHT.HTC.AddHTCRow(One, Two,
                Three, Four,
                Five, Six,
                Seven, Eight,
                Nine, Ten,
                Eleven,
                Twelve, Thirteen,
                Fourteen, Fifteen,
                Sixteen, Seventeen,
                Eighteen, 
                Twenty, TwentyOne,
                TwentyTwo, TwentyThree,
                TwentyFour, TwentyFive,
                TwentySix, TwentySeven,
                TwentyEight, TwentyNine,
                Thirty, ThirtyOne,
                ThirtyTwo, ThirtyThree,
                ThirtyFour, ThirtyFive,
                ThirtySix, ThirtySeven,
                ThirtyEight, ThirtyNine,
                Fourty, FourtyOne,
                FourtyTwo, FourtyThree,
                FourtyFour, FourtyFive,
                FourtySix, FourtySeven,
                FourtyEight, FourtyNine,
                Fifty, FiftyOne,
                FiftyTwo, FiftyThree,
                FiftyFour, FiftyFive,
                FiftySix, FiftySeven,
                FiftyEight, FiftyNine,
                Sixty, SixtyOne,
                SixtyTwo, SixtyThree,
                SixtyFour, SixtyFive,
                SixtySix, SixtySeven,
                SixtyEight, SixtyNine,
                Seventy, SeventyOne,
                SeventyTwo, SeventyThree,
                SeventyFour, SeventyFive,
                SeventySix, SeventySeven,
                SeventyEight, SeventyNine,
                Eighty, EightyOne,
                EightyTwo, EightyThree,
                EightyFour, EightyFive,
                EightySix, EightySeven,
                EightyEight, EightyNine,
                Ninety, NinetyOne,
                NinetyTwo, NinetyThree,
                NinetyFour, NinetyFive,
                NinetySix, NinetySeven,
                NinetyEight, NinetyNine,
                Hundred, HundredOne,
                HundredTwo, HundredThree,
                HundredFive, HundredSix,
                HundredSeven, HundredEight,
                HundredNine, HundredTen,
                HundredEleven, HundredTwelve,
                HundredThirteen, HundredFourteen,
                HundredFifteen, HundredSixteen,
                HundredSeventeen, HundredEighteen,
                HundredNineteen, HundredTwenty,
                Hundred21, Hundred22,
                Hundred23, Hundred24,
                Hundred25, Hundred26,
                Hundred27,Nineteen);
         
        ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/HTCServiceReport/HTCReport.rpt")));
            Rd.SetDataSource(datasetHT);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            
            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "HTC Service Summary " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }
  

        #endregion
    }
    


    }

