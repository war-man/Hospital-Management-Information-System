using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.DepartmentalOutpatientAttendance;
using Caresoft2._0.Areas.CareSoftReports.Reports.MISReports.DiagnosisSummary;
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
                Departments  = db2.Departments
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

            var dataSet = GetPatientListData(FromDate,ToDate,Clinician, Department,InsuranceCompany,selectedDiv);



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

            if(selectedDiv == "Clinician")
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

        private DataSetPatientList GetPatientListData(DateTime FromDate, DateTime ToDate, string Clinician, string Department, string InsuranceCompany,string selectedOption)
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

            if (FromDate!=null && ToDate!=null)
            {
                if (Department != "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Department == Department && p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p=>p.PatientId).ToList();
                }
                else if(Department == "All")
                {
                    opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate).DistinctBy(p=>p.PatientId).ToList();
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
            
               opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate &&p.Patient.OpdRegisters.Count()==1).DistinctBy(p => p.PatientId).ToList();
                
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

            public int  FUnderOneYearN { get; set; }
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
                opdRegister = db2.OpdRegisters.Where(p => p.Date >= FromDate && p.Date <= ToDate && p.IsIPD==false).ToList();
            }


            foreach (var item in Departments)
            {
                DepartmentOutPatientAttendanceViewModel model = new DepartmentOutPatientAttendanceViewModel();

                    model.DepartmentName = item.DepartmentName;

                    foreach (var opd in opdRegister)
                    {
                        if (item.DepartmentName == "OUTPATIENT" || item.DepartmentName == null || item.DepartmentName=="")
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

            if (Department == null || Department == "" ||Department=="All")
            {
                AllDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded > FromDate && p.TimeAdded <= ToDate).ToList();
            }
            else
            {
                AllDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded > FromDate && p.TimeAdded <= ToDate && p.OpdRegister.Department==Department).ToList();
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
                else if (Department == "All" ||Department==null)
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

                    var chiefComplaints = item.Complaints.Where(p=>p.OpdIpdNumber == item.Id).FirstOrDefault()?.ChiefComplaints;
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

        public ActionResult GetInsuranceReport(DateTime FromDate, DateTime ToDate, string Department,string Format)
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
            ToDate = ToDate.AddDays(1);
            var dataSet = new DataSetInsuranceReport();

            var data = new List<InsuranceReportVM>();
            var lServices = db2.BillServices.Where(e => ((e.Award > 0 && e.DateAdded>=FromDate && e.DateAdded<ToDate) ||
            (e.OpdRegister.Medications.Any(f => f.Award > 0 && e.DateAdded >= FromDate && e.DateAdded < ToDate)) )).ToList();
            var meds = db2.Medications.Where(e => e.Award > 0 && e.TimeAdded>=FromDate && e.TimeAdded<ToDate).ToList();

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
                ToDate.AddDays(-1).ToString("dd-MMM-yyyy"));


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

        public class TBDataViewModel{
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
                .Where(p => DbFunctions.TruncateTime(p.DateAdded)>= FromDate && DbFunctions.TruncateTime(p.DateAdded) <= ToDate)
                .DistinctBy(x=>x.OPDNo)
                .ToList();


            TBDataViewModel tBCasesDetected = new TBDataViewModel()
            {
                CategoryName = "TB Cases Detected"
            };

            foreach (var item in data)
            {
                if(item.OpdRegister.Patient.Gender == "M")
                {
                    var age = DateTime.Now.Year - item.OpdRegister.Patient.DOB.Value.Year;
                    if(age <= 14)
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

            if(department==0 && serviceGroup==0)
            {
                ServicesData = db2.Services.OrderBy(p => p.DepartmentId).ToList();
            }
            else if(department > 0 && serviceGroup >0)
            {
                ServicesData = db2.Services.Where(p=>p.DepartmentId == department && p.ServiceGroupId ==serviceGroup).OrderBy(p => p.DepartmentId).ToList();
            }
            else if(department>0 && serviceGroup == 0)
            {
                ServicesData = db2.Services.Where(p => p.DepartmentId == department).OrderBy(p => p.DepartmentId).ToList();
            }
            else if(department==0 && serviceGroup > 0)
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

        #region Monthly Revenue Report

        public ActionResult MonthlyRevenue()
        {
            return View();
        }

        public class MonthlyReveneuModel
        {
            public string NameOfDepartment { get; set; } 
            public string Months { get; set; }
            public int Year { get; set; }
        }



        public ActionResult GetMonthlyRevenueReport(int Year, string Months)
        {


            var Jan = db2.BillServices.Where(p => p.DepartmentId == item.Id && p.DateAdded.Month == 1
                          && p.DateAdded.Year == Year && p.OpdRegister.Tariff.Company.CompanyName == Insurance).Sum(p => (p.Award * p.Quatity));
        }




        #endregion
    }
}