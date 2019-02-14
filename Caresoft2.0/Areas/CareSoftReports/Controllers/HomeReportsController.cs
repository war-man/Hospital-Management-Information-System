using Caresoft2._0.Areas.PharmacyModule.Reports.CurrentStock;
using Caresoft2._0.Areas.PharmacyModule.Reports.PharmacyReports;
using Caresoft2._0.Areas.PharmacyModule.ViewModel;
using Caresoft2._0.Areas.Procurement.Models;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH505;
using CaresoftHMISDataAccess;
using Microsoft.Ajax.Utilities;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH705A;
using LabsDataAccess;
using System.Globalization;
using Caresoft2._0.Areas.CareSoftReports.Reports;
using System.Drawing.Printing;
using Caresoft2._0.Areas.CareSoftReports.ViewModels;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH717;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH706;
using System.Data.Entity;
using Caresoft2._0.Areas.CareSoftReports.Reports.UserRolesAndRights;
using Caresoft2._0.CrystalReports.ReportHeader;
using Caresoft2._0.CrystalReports;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOHMonthlyCash;

namespace Caresoft2._0.Areas.CareSoftReports.Controllers
{
    [Auth]
    public class HomeReportsController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();
        CaresoftHMISEntities db2 = new CaresoftHMISEntities();
        CareSoftLabsEntities db3 = new CareSoftLabsEntities();


        // GET: CareSoftReports/HomeReports
        public ActionResult Index()
        {
            return View();
        }


        public class Moh705a
        {
            public string NameOfDisease { get; set; }
            public int[] DaysOfTheMonth { get; set; }
        }

        #region MOH 705 B 
        public ActionResult MOH705B()
        {
            return View();
        }
        public ActionResult GetMOH705BReport(int Year, int selectMonth)
        {

            var DiagnosisPatients = new List<PatientDiagnosi>();
            var diseases = db2.Diseases.Where(p => p.CodeMOH705_B > 0).OrderBy(p => p.CodeMOH705_B).ToList();
            var theReport = new List<Moh705a>();

            var NumberOfDays = DateTime.DaysInMonth(Year, selectMonth);

            var filteredPatDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded.Year == Year && p.TimeAdded.Month == selectMonth && p.OldNewCase == "New Case").ToList();

            foreach (var Pat in filteredPatDiagnosis)
            {
                var dateTaken = Pat.TimeAdded;
                var patientsDateOfBirth = Pat.OpdRegister.Patient?.DOB;
                var Age = dateTaken.Year - patientsDateOfBirth.Value.Year;

                if (Age > 5)
                {
                    DiagnosisPatients.Add(Pat);
                }
            }

            foreach (var item in diseases)
            {
                Moh705a moh705A = new Moh705a()
                {
                    NameOfDisease = item.DiseaseName,
                    DaysOfTheMonth = new int[31]
                };
                //get the total number of days for this month

                int[] AllDaysOfTheMonth = new int[31];

                for (int i = 1; i <= NumberOfDays; i++)
                {
                    var theDay = i;
                    var CountInDay = DiagnosisPatients.Where(p => p.FinalDiagnosis.Contains(item.DiseaseName) && p.TimeAdded.Day == theDay).Count();
                    AllDaysOfTheMonth[i - 1] = CountInDay;
                }

                if (AllDaysOfTheMonth.Count() < 31)
                {
                    var remainingDays = 31 - AllDaysOfTheMonth.Count();
                    for (int i = 1; i < remainingDays; i++)
                    {
                        //AllDaysOfTheMonth
                    }
                }
                else
                {
                    moh705A.DaysOfTheMonth = AllDaysOfTheMonth.ToArray();
                }

                theReport.Add(moh705A);
            }


            DataSetMOH705A dataSetMOH705A = new DataSetMOH705A();

            foreach (var item in theReport)
            {
                dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                     item.NameOfDisease,
                    item.DaysOfTheMonth[0],
                    item.DaysOfTheMonth[1],
                    item.DaysOfTheMonth[2],
                    item.DaysOfTheMonth[3],
                    item.DaysOfTheMonth[4],
                    item.DaysOfTheMonth[5],
                    item.DaysOfTheMonth[6],
                    item.DaysOfTheMonth[7],
                    item.DaysOfTheMonth[8],
                    item.DaysOfTheMonth[9],
                    item.DaysOfTheMonth[11],
                    item.DaysOfTheMonth[12],
                    item.DaysOfTheMonth[13],
                    item.DaysOfTheMonth[14],
                    item.DaysOfTheMonth[15],
                    item.DaysOfTheMonth[16],
                    item.DaysOfTheMonth[17],
                    item.DaysOfTheMonth[18],
                    item.DaysOfTheMonth[19],
                    item.DaysOfTheMonth[20],
                    item.DaysOfTheMonth[21],
                    item.DaysOfTheMonth[22],
                    item.DaysOfTheMonth[23],
                    item.DaysOfTheMonth[24],
                    item.DaysOfTheMonth[25],
                    item.DaysOfTheMonth[26],
                    item.DaysOfTheMonth[27],
                    item.DaysOfTheMonth[28],
                    item.DaysOfTheMonth[29],
                    item.DaysOfTheMonth[30],
                    item.DaysOfTheMonth[10]
                    );
            }



            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);


            #region All other diseases

            var AllOtherDiseases = new List<PatientDiagnosi>();
            var DiseasesWithout705ACode = db2.Diseases.Where(p => p.CodeMOH705_A == 0 || p.CodeMOH705_A == null).ToList();
            var theReportAllOtherDiseases = new List<Moh705a>();

            foreach (var item in DiseasesWithout705ACode)
            {
                Moh705a moh705A = new Moh705a()
                {
                    NameOfDisease = item.DiseaseName,
                    DaysOfTheMonth = new int[31]
                };
                //get the total number of days for this month

                int[] AllDaysOfTheMonth = new int[31];

                for (int i = 1; i <= NumberOfDays; i++)
                {
                    var theDay = i;
                    var CountInDay = DiagnosisPatients.Where(p => p.FinalDiagnosis.Contains(item.DiseaseName) && p.TimeAdded.Day == theDay).Count();
                    AllDaysOfTheMonth[i - 1] = CountInDay;
                }

                if (AllDaysOfTheMonth.Count() < 31)
                {
                    var remainingDays = 31 - AllDaysOfTheMonth.Count();
                    for (int i = 1; i < remainingDays; i++)
                    {
                        //AllDaysOfTheMonth
                    }
                }
                else
                {
                    moh705A.DaysOfTheMonth = AllDaysOfTheMonth.ToArray();
                }

                theReportAllOtherDiseases.Add(moh705A);
            }

            var a = theReportAllOtherDiseases;
            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
               "ALL OTHER DISEASES",
               a.Sum(p => p.DaysOfTheMonth[0]),
               a.Sum(p => p.DaysOfTheMonth[1]),
               a.Sum(p => p.DaysOfTheMonth[2]),
               a.Sum(p => p.DaysOfTheMonth[3]),
               a.Sum(p => p.DaysOfTheMonth[4]),
               a.Sum(p => p.DaysOfTheMonth[5]),
               a.Sum(p => p.DaysOfTheMonth[6]),
               a.Sum(p => p.DaysOfTheMonth[7]),
               a.Sum(p => p.DaysOfTheMonth[8]),
               a.Sum(p => p.DaysOfTheMonth[9]),
               a.Sum(p => p.DaysOfTheMonth[10]),
               a.Sum(p => p.DaysOfTheMonth[11]),
               a.Sum(p => p.DaysOfTheMonth[12]),
               a.Sum(p => p.DaysOfTheMonth[13]),
               a.Sum(p => p.DaysOfTheMonth[14]),
               a.Sum(p => p.DaysOfTheMonth[15]),
               a.Sum(p => p.DaysOfTheMonth[16]),
               a.Sum(p => p.DaysOfTheMonth[17]),
               a.Sum(p => p.DaysOfTheMonth[18]),
               a.Sum(p => p.DaysOfTheMonth[19]),
               a.Sum(p => p.DaysOfTheMonth[20]),
               a.Sum(p => p.DaysOfTheMonth[21]),
               a.Sum(p => p.DaysOfTheMonth[22]),
               a.Sum(p => p.DaysOfTheMonth[23]),
               a.Sum(p => p.DaysOfTheMonth[24]),
               a.Sum(p => p.DaysOfTheMonth[25]),
               a.Sum(p => p.DaysOfTheMonth[26]),
               a.Sum(p => p.DaysOfTheMonth[27]),
               a.Sum(p => p.DaysOfTheMonth[28]),
               a.Sum(p => p.DaysOfTheMonth[29]),
               a.Sum(p => p.DaysOfTheMonth[30])
               );

            #endregion



            #region Number of First Attendants
            int[] FirstAttendants = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                FirstAttendants[i] = DiagnosisPatients.Where(p => p.OpdRegister.Patient.OpdRegisters.Count() == 1 && p.TimeAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "NO. OF FIRST ATTENDANTS",
                FirstAttendants[0],
                FirstAttendants[1],
                FirstAttendants[2],
                FirstAttendants[3],
                FirstAttendants[4],
                FirstAttendants[5],
                FirstAttendants[6],
                FirstAttendants[7],
                FirstAttendants[8],
                FirstAttendants[9],
                FirstAttendants[10],
                FirstAttendants[11],
                FirstAttendants[12],
                FirstAttendants[13],
                FirstAttendants[14],
                FirstAttendants[15],
                FirstAttendants[16],
                FirstAttendants[17],
                FirstAttendants[18],
                FirstAttendants[19],
                FirstAttendants[20],
                FirstAttendants[21],
                FirstAttendants[22],
                FirstAttendants[23],
                FirstAttendants[24],
                FirstAttendants[25],
                FirstAttendants[26],
                FirstAttendants[27],
                FirstAttendants[28],
                FirstAttendants[29],
                FirstAttendants[30]

                );

            #endregion


            #region Number of ReAttendants
            int[] ReAttendants = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                ReAttendants[i] = DiagnosisPatients.Where(p => p.OpdRegister.Patient.OpdRegisters.Count() > 1 && p.TimeAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "NO. OF RE-ATTENDANCES",
                ReAttendants[0],
                ReAttendants[1],
                ReAttendants[2],
                ReAttendants[3],
                ReAttendants[4],
                ReAttendants[5],
                ReAttendants[6],
                ReAttendants[7],
                ReAttendants[8],
                ReAttendants[9],
                ReAttendants[10],
                ReAttendants[11],
                ReAttendants[12],
                ReAttendants[13],
                ReAttendants[14],
                ReAttendants[15],
                ReAttendants[16],
                ReAttendants[17],
                ReAttendants[18],
                ReAttendants[19],
                ReAttendants[20],
                ReAttendants[21],
                ReAttendants[22],
                ReAttendants[23],
                ReAttendants[24],
                ReAttendants[25],
                ReAttendants[26],
                ReAttendants[27],
                ReAttendants[28],
                ReAttendants[29],
                ReAttendants[30]

                );

            #endregion

            #region Refferals from other Health Facility

            var ReferralsFromOtherHealthFacility = db2.PatientReferals
                                                    .Where(p => p.DateAdded.Year == Year && 
                                                    p.DateAdded.Month == selectMonth && 
                                                    p.ReferalType.ToLower() != "internal")
                                                    .ToList();

            int[] InsideSideRefferals = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                InsideSideRefferals[i] = ReferralsFromOtherHealthFacility.Where(p => p.DateAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Refferals from other health facility",
                InsideSideRefferals[0],
                InsideSideRefferals[1],
                InsideSideRefferals[2],
                InsideSideRefferals[3],
                InsideSideRefferals[4],
                InsideSideRefferals[5],
                InsideSideRefferals[6],
                InsideSideRefferals[7],
                InsideSideRefferals[8],
                InsideSideRefferals[9],
                InsideSideRefferals[10],
                InsideSideRefferals[11],
                InsideSideRefferals[12],
                InsideSideRefferals[13],
                InsideSideRefferals[14],
                InsideSideRefferals[15],
                InsideSideRefferals[16],
                InsideSideRefferals[17],
                InsideSideRefferals[18],
                InsideSideRefferals[19],
                InsideSideRefferals[20],
                InsideSideRefferals[21],
                InsideSideRefferals[22],
                InsideSideRefferals[23],
                InsideSideRefferals[24],
                InsideSideRefferals[25],
                InsideSideRefferals[26],
                InsideSideRefferals[27],
                InsideSideRefferals[28],
                InsideSideRefferals[29],
                InsideSideRefferals[30]

                );

            #endregion

            #region Referral to other Facilities
            var ReferralsToOtherHealthFacility = db2.PatientReferals
                                                    .Where(p => p.DateAdded.Year == Year && p.DateAdded.Month == selectMonth && p.ReferalType.ToLower() == "external")
                                                    .ToList();

            int[] OutSideRefferals = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                OutSideRefferals[i] = ReferralsToOtherHealthFacility.Where(p => p.DateAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Refferals to other health facility",
                OutSideRefferals[0],
                OutSideRefferals[1],
                OutSideRefferals[2],
                OutSideRefferals[3],
                OutSideRefferals[4],
                OutSideRefferals[5],
                OutSideRefferals[6],
                OutSideRefferals[7],
                OutSideRefferals[8],
                OutSideRefferals[9],
                OutSideRefferals[10],
                OutSideRefferals[11],
                OutSideRefferals[12],
                OutSideRefferals[13],
                OutSideRefferals[14],
                OutSideRefferals[15],
                OutSideRefferals[16],
                OutSideRefferals[17],
                OutSideRefferals[18],
                OutSideRefferals[19],
                OutSideRefferals[20],
                OutSideRefferals[21],
                OutSideRefferals[22],
                OutSideRefferals[23],
                OutSideRefferals[24],
                OutSideRefferals[25],
                OutSideRefferals[26],
                OutSideRefferals[27],
                OutSideRefferals[28],
                OutSideRefferals[29],
                OutSideRefferals[30]

                );
            #endregion

            #region Refferals from Community Unit
            var refferalsFromCommunity = db2.OpdRegisters.Where(p => p.ReferralByCHW == true && p.TimeAdded.Value.Year == Year && p.TimeAdded.Value.Month == selectMonth).ToList();


            int[] RefferalsFromCommunity = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                RefferalsFromCommunity[i] = refferalsFromCommunity.Where(p => p.TimeAdded.Value.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Referrals from community",
                RefferalsFromCommunity[0],
                RefferalsFromCommunity[1],
                RefferalsFromCommunity[2],
                RefferalsFromCommunity[3],
                RefferalsFromCommunity[4],
                RefferalsFromCommunity[5],
                RefferalsFromCommunity[6],
                RefferalsFromCommunity[7],
                RefferalsFromCommunity[8],
                RefferalsFromCommunity[9],
                RefferalsFromCommunity[10],
                RefferalsFromCommunity[11],
                RefferalsFromCommunity[12],
                RefferalsFromCommunity[13],
                RefferalsFromCommunity[14],
                RefferalsFromCommunity[15],
                RefferalsFromCommunity[16],
                RefferalsFromCommunity[17],
                RefferalsFromCommunity[18],
                RefferalsFromCommunity[19],
                RefferalsFromCommunity[20],
                RefferalsFromCommunity[21],
                RefferalsFromCommunity[22],
                RefferalsFromCommunity[23],
                RefferalsFromCommunity[24],
                RefferalsFromCommunity[25],
                RefferalsFromCommunity[26],
                RefferalsFromCommunity[27],
                RefferalsFromCommunity[28],
                RefferalsFromCommunity[29],
                RefferalsFromCommunity[30]

                );

            #endregion
            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Refferals to comunity", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                );


            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
            int session = Convert.ToInt32(Session["UserId"]);
            var userLoggedIn = db2.Users.Where(p => p.Id == session).FirstOrDefault();

            var date = new DateTime(Year, selectMonth, 1);

            dataSetMOH705A.HospitalInfo.AddHospitalInfoRow(
                HospitalName,
                "",
                "",
                date.ToString("dd-MMM-yyyy"),
                date.ToShortMonthName(),
                date.Year.ToString(),
                "1",
                userLoggedIn.Username,
                ""
                );


            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH705B/RptMOH705B.rpt")));
            Rd.SetDataSource(dataSetMOH705A);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "MOH707B" + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf";

            return File(Stream, "application/pdf", FileName);
        }
        #endregion

        #region MOH 705 A Report
        public ActionResult MOH705A()
        {
            return View();
        }

        // GET: /CareSoftReports/HomeReports/MOH705A
        public ActionResult GetMOH705AReport(int Year, int selectMonth)
        {

            var DiagnosisPatients = new List<PatientDiagnosi>();
            var diseases = db2.Diseases.Where(p => p.CodeMOH705_A > 0).OrderBy(p => p.CodeMOH705_A).ToList();
            var theReport = new List<Moh705a>();

            var filteredPatDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded.Year == Year && p.TimeAdded.Month == selectMonth && p.OldNewCase == "New Case").ToList();

            foreach (var Pat in filteredPatDiagnosis)
            {
                var dateTaken = Pat.TimeAdded;
                var patientsDateOfBirth = Pat.OpdRegister.Patient?.DOB;
                var Age = dateTaken.Year - patientsDateOfBirth.Value.Year;

                if (Age <= 5)
                {
                    DiagnosisPatients.Add(Pat);
                }
            }

            #region Diseases in the 705 A Report
            var NumberOfDays = DateTime.DaysInMonth(Year, selectMonth);

            foreach (var item in diseases)
            {
                Moh705a moh705A = new Moh705a()
                {
                    NameOfDisease = item.DiseaseName,
                    DaysOfTheMonth = new int[31]
                };
                //get the total number of days for this month

                int[] AllDaysOfTheMonth = new int[31];

                for (int i = 1; i <= NumberOfDays; i++)
                {
                    var theDay = i;

                    var CountInDay = DiagnosisPatients.Where(p => p.FinalDiagnosis.Contains(item.DiseaseName) && p.TimeAdded.Day == theDay).Count();
                    AllDaysOfTheMonth[i - 1] = CountInDay;
                }

                if (AllDaysOfTheMonth.Count() < 31)
                {
                    var remainingDays = 31 - AllDaysOfTheMonth.Count();
                    for (int i = 1; i < remainingDays; i++)
                    {
                        //AllDaysOfTheMonth
                    }
                }
                else
                {
                    moh705A.DaysOfTheMonth = AllDaysOfTheMonth.ToArray();
                }

                theReport.Add(moh705A);
            }


            Reports.MOH705A.DataSetMOH705A dataSetMOH705A = new Reports.MOH705A.DataSetMOH705A();

            foreach (var item in theReport)
            {
                dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                    item.NameOfDisease,
                    item.DaysOfTheMonth[0],
                    item.DaysOfTheMonth[1],
                    item.DaysOfTheMonth[2],
                    item.DaysOfTheMonth[3],
                    item.DaysOfTheMonth[4],
                    item.DaysOfTheMonth[5],
                    item.DaysOfTheMonth[6],
                    item.DaysOfTheMonth[7],
                    item.DaysOfTheMonth[8],
                    item.DaysOfTheMonth[9],
                    item.DaysOfTheMonth[11],
                    item.DaysOfTheMonth[12],
                    item.DaysOfTheMonth[13],
                    item.DaysOfTheMonth[14],
                    item.DaysOfTheMonth[15],
                    item.DaysOfTheMonth[16],
                    item.DaysOfTheMonth[17],
                    item.DaysOfTheMonth[18],
                    item.DaysOfTheMonth[19],
                    item.DaysOfTheMonth[20],
                    item.DaysOfTheMonth[21],
                    item.DaysOfTheMonth[22],
                    item.DaysOfTheMonth[23],
                    item.DaysOfTheMonth[24],
                    item.DaysOfTheMonth[25],
                    item.DaysOfTheMonth[26],
                    item.DaysOfTheMonth[27],
                    item.DaysOfTheMonth[28],
                    item.DaysOfTheMonth[29],
                    item.DaysOfTheMonth[30],
                    item.DaysOfTheMonth[10]
                    );
            }

            #endregion

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
               "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            #region All other diseases

            var AllOtherDiseases = new List<PatientDiagnosi>();
            var DiseasesWithout705ACode = db2.Diseases.Where(p => p.CodeMOH705_A == 0 || p.CodeMOH705_A == null).ToList();
            var theReportAllOtherDiseases = new List<Moh705a>();

            foreach (var item in DiseasesWithout705ACode)
            {
                Moh705a moh705A = new Moh705a()
                {
                    NameOfDisease = item.DiseaseName,
                    DaysOfTheMonth = new int[31]
                };
                //get the total number of days for this month

                int[] AllDaysOfTheMonth = new int[31];

                for (int i = 1; i <= NumberOfDays; i++)
                {
                    var theDay = i;
                    var CountInDay = DiagnosisPatients.Where(p => p.FinalDiagnosis.Contains(item.DiseaseName) && p.TimeAdded.Day == theDay).Count();
                    AllDaysOfTheMonth[i - 1] = CountInDay;
                }

                if (AllDaysOfTheMonth.Count() < 31)
                {
                    var remainingDays = 31 - AllDaysOfTheMonth.Count();
                    for (int i = 1; i < remainingDays; i++)
                    {
                        //AllDaysOfTheMonth
                    }
                }
                else
                {
                    moh705A.DaysOfTheMonth = AllDaysOfTheMonth.ToArray();
                }

                theReportAllOtherDiseases.Add(moh705A);
            }

            var a = theReportAllOtherDiseases;
            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
               "ALL OTHER DISEASES",
               a.Sum(p => p.DaysOfTheMonth[0]),
               a.Sum(p => p.DaysOfTheMonth[1]),
               a.Sum(p => p.DaysOfTheMonth[2]),
               a.Sum(p => p.DaysOfTheMonth[3]),
               a.Sum(p => p.DaysOfTheMonth[4]),
               a.Sum(p => p.DaysOfTheMonth[5]),
               a.Sum(p => p.DaysOfTheMonth[6]),
               a.Sum(p => p.DaysOfTheMonth[7]),
               a.Sum(p => p.DaysOfTheMonth[8]),
               a.Sum(p => p.DaysOfTheMonth[9]),
               a.Sum(p => p.DaysOfTheMonth[10]),
               a.Sum(p => p.DaysOfTheMonth[11]),
               a.Sum(p => p.DaysOfTheMonth[12]),
               a.Sum(p => p.DaysOfTheMonth[13]),
               a.Sum(p => p.DaysOfTheMonth[14]),
               a.Sum(p => p.DaysOfTheMonth[15]),
               a.Sum(p => p.DaysOfTheMonth[16]),
               a.Sum(p => p.DaysOfTheMonth[17]),
               a.Sum(p => p.DaysOfTheMonth[18]),
               a.Sum(p => p.DaysOfTheMonth[19]),
               a.Sum(p => p.DaysOfTheMonth[20]),
               a.Sum(p => p.DaysOfTheMonth[21]),
               a.Sum(p => p.DaysOfTheMonth[22]),
               a.Sum(p => p.DaysOfTheMonth[23]),
               a.Sum(p => p.DaysOfTheMonth[24]),
               a.Sum(p => p.DaysOfTheMonth[25]),
               a.Sum(p => p.DaysOfTheMonth[26]),
               a.Sum(p => p.DaysOfTheMonth[27]),
               a.Sum(p => p.DaysOfTheMonth[28]),
               a.Sum(p => p.DaysOfTheMonth[29]),
               a.Sum(p => p.DaysOfTheMonth[30])
               );

            #endregion



            #region Number of First Attendants
            int[] FirstAttendants = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                FirstAttendants[i] = DiagnosisPatients.Where(p => p.OpdRegister.Patient.OpdRegisters.Count() == 1 && p.TimeAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "NO. OF FIRST ATTENDANTS",
                FirstAttendants[0],
                FirstAttendants[1],
                FirstAttendants[2],
                FirstAttendants[3],
                FirstAttendants[4],
                FirstAttendants[5],
                FirstAttendants[6],
                FirstAttendants[7],
                FirstAttendants[8],
                FirstAttendants[9],
                FirstAttendants[10],
                FirstAttendants[11],
                FirstAttendants[12],
                FirstAttendants[13],
                FirstAttendants[14],
                FirstAttendants[15],
                FirstAttendants[16],
                FirstAttendants[17],
                FirstAttendants[18],
                FirstAttendants[19],
                FirstAttendants[20],
                FirstAttendants[21],
                FirstAttendants[22],
                FirstAttendants[23],
                FirstAttendants[24],
                FirstAttendants[25],
                FirstAttendants[26],
                FirstAttendants[27],
                FirstAttendants[28],
                FirstAttendants[29],
                FirstAttendants[30]

                );

            #endregion


            #region Number of ReAttendants
            int[] ReAttendants = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                ReAttendants[i] = DiagnosisPatients.Where(p => p.OpdRegister.Patient.OpdRegisters.Count() > 1 && p.TimeAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "NO. OF RE-ATTENDANCES",
                ReAttendants[0],
                ReAttendants[1],
                ReAttendants[2],
                ReAttendants[3],
                ReAttendants[4],
                ReAttendants[5],
                ReAttendants[6],
                ReAttendants[7],
                ReAttendants[8],
                ReAttendants[9],
                ReAttendants[10],
                ReAttendants[11],
                ReAttendants[12],
                ReAttendants[13],
                ReAttendants[14],
                ReAttendants[15],
                ReAttendants[16],
                ReAttendants[17],
                ReAttendants[18],
                ReAttendants[19],
                ReAttendants[20],
                ReAttendants[21],
                ReAttendants[22],
                ReAttendants[23],
                ReAttendants[24],
                ReAttendants[25],
                ReAttendants[26],
                ReAttendants[27],
                ReAttendants[28],
                ReAttendants[29],
                ReAttendants[30]

                );

            #endregion

            #region Refferals from other Health Facility

            var ReferralsFromOtherHealthFacility = db2.PatientReferals
                                                    .Where(p => p.DateAdded.Year == Year && p.DateAdded.Month == selectMonth && p.ReferalType.ToLower() == "internal")
                                                    .ToList();

            int[] InsideSideRefferals = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                InsideSideRefferals[i] = ReferralsFromOtherHealthFacility.Where(p => p.DateAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Refferals from other health facility",
                InsideSideRefferals[0],
                InsideSideRefferals[1],
                InsideSideRefferals[2],
                InsideSideRefferals[3],
                InsideSideRefferals[4],
                InsideSideRefferals[5],
                InsideSideRefferals[6],
                InsideSideRefferals[7],
                InsideSideRefferals[8],
                InsideSideRefferals[9],
                InsideSideRefferals[10],
                InsideSideRefferals[11],
                InsideSideRefferals[12],
                InsideSideRefferals[13],
                InsideSideRefferals[14],
                InsideSideRefferals[15],
                InsideSideRefferals[16],
                InsideSideRefferals[17],
                InsideSideRefferals[18],
                InsideSideRefferals[19],
                InsideSideRefferals[20],
                InsideSideRefferals[21],
                InsideSideRefferals[22],
                InsideSideRefferals[23],
                InsideSideRefferals[24],
                InsideSideRefferals[25],
                InsideSideRefferals[26],
                InsideSideRefferals[27],
                InsideSideRefferals[28],
                InsideSideRefferals[29],
                InsideSideRefferals[30]

                );

            #endregion

            #region Referral to other Facilities
            var ReferralsToOtherHealthFacility = db2.PatientReferals
                                                    .Where(p => p.DateAdded.Year == Year && p.DateAdded.Month == selectMonth && p.ReferalType.ToLower() == "external")
                                                    .ToList();

            int[] OutSideRefferals = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                OutSideRefferals[i] = ReferralsToOtherHealthFacility.Where(p => p.DateAdded.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Refferals to other health facility",
                OutSideRefferals[0],
                OutSideRefferals[1],
                OutSideRefferals[2],
                OutSideRefferals[3],
                OutSideRefferals[4],
                OutSideRefferals[5],
                OutSideRefferals[6],
                OutSideRefferals[7],
                OutSideRefferals[8],
                OutSideRefferals[9],
                OutSideRefferals[10],
                OutSideRefferals[11],
                OutSideRefferals[12],
                OutSideRefferals[13],
                OutSideRefferals[14],
                OutSideRefferals[15],
                OutSideRefferals[16],
                OutSideRefferals[17],
                OutSideRefferals[18],
                OutSideRefferals[19],
                OutSideRefferals[20],
                OutSideRefferals[21],
                OutSideRefferals[22],
                OutSideRefferals[23],
                OutSideRefferals[24],
                OutSideRefferals[25],
                OutSideRefferals[26],
                OutSideRefferals[27],
                OutSideRefferals[28],
                OutSideRefferals[29],
                OutSideRefferals[30]

                );
            #endregion

            #region Refferals from Community Unit
            var refferalsFromCommunity = db2.OpdRegisters.Where(p => p.ReferralByCHW == true && p.TimeAdded.Value.Year == Year && p.TimeAdded.Value.Month == selectMonth).ToList();


            int[] RefferalsFromCommunity = new int[31];

            for (int i = 0; i < NumberOfDays; i++)
            {
                var dayOfMonth = i + 1;
                RefferalsFromCommunity[i] = refferalsFromCommunity.Where(p => p.TimeAdded.Value.Day == dayOfMonth).Count();

            }

            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Referrals from community",
                RefferalsFromCommunity[0],
                RefferalsFromCommunity[1],
                RefferalsFromCommunity[2],
                RefferalsFromCommunity[3],
                RefferalsFromCommunity[4],
                RefferalsFromCommunity[5],
                RefferalsFromCommunity[6],
                RefferalsFromCommunity[7],
                RefferalsFromCommunity[8],
                RefferalsFromCommunity[9],
                RefferalsFromCommunity[10],
                RefferalsFromCommunity[11],
                RefferalsFromCommunity[12],
                RefferalsFromCommunity[13],
                RefferalsFromCommunity[14],
                RefferalsFromCommunity[15],
                RefferalsFromCommunity[16],
                RefferalsFromCommunity[17],
                RefferalsFromCommunity[18],
                RefferalsFromCommunity[19],
                RefferalsFromCommunity[20],
                RefferalsFromCommunity[21],
                RefferalsFromCommunity[22],
                RefferalsFromCommunity[23],
                RefferalsFromCommunity[24],
                RefferalsFromCommunity[25],
                RefferalsFromCommunity[26],
                RefferalsFromCommunity[27],
                RefferalsFromCommunity[28],
                RefferalsFromCommunity[29],
                RefferalsFromCommunity[30]

                );

            #endregion


            dataSetMOH705A.DiseasesWithDates.AddDiseasesWithDatesRow(
                "Refferals to comunity", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                );


            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
            int session = Convert.ToInt32(Session["UserId"]);
            var userLoggedIn = db2.Users.Where(p => p.Id == session).FirstOrDefault();

            var date = new DateTime(Year, selectMonth, 1);

            dataSetMOH705A.HospitalInfo.AddHospitalInfoRow(
                HospitalName,
                "",
                "",
                date.ToString("dd-MMM-yyyy"),
                date.ToShortMonthName(),
                date.Year.ToString(),
                "1",
                userLoggedIn.Username,
                ""
                );


            ReportDocument report = new ReportDocument();
            report.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH705A/RptMOH705A.rpt")));

            report.SetDataSource(dataSetMOH705A);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream str = report.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);

            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string name = "Rpt MOH 705 A" + Timestamp + ".pdf";

            //report.GetEditor()
            return File(str, "application/pdf", name);


        }

        #endregion

        #region MOH 505 Report
        public class LabaratoryDs
        {
            public string TestName { get; set; }
            public int UnderFiveTests { get; set; }
            public int UnderFivePositive { get; set; }
            public int OverFiveTests { get; set; }
            public int OverFivePositive { get; set; }
        }

        public class Diseases_Conditions_Events
        {
            public string Name { get; set; }
            public int UnderFiveCases { get; set; }
            public int UnderFiveDeaths { get; set; }
            public int OverFiveCases { get; set; }
            public int OverFiveDeaths { get; set; }

        }


        //CareSoftReports/HomeReports/MOH505
        public ActionResult MOH505()
        {
            return View();
        }

        //CareSoftReports/HomeReports/GetMOH505Report
        public ActionResult GetMOH505Report(DateTime StartDate, DateTime EndDate)
        {
            var TempEndDate = EndDate;

            EndDate = EndDate.AddDays(1);

            if (StartDate == null && EndDate == null && StartDate == EndDate)
            {
                ViewBag.DatesNotSet = true;
                return RedirectToAction("MOH505");
            }
            else
            {
                //add some sample data to test on the sub report
                DtSetMOH505 dtSetMOH505 = new DtSetMOH505();
                DtSetMOH505 dtSetMOH505b = new DtSetMOH505();
                var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;
                int session = Convert.ToInt32(Session["UserId"]);
                var userLoggedIn = db2.Users.Where(p => p.Id == session).FirstOrDefault();
                dtSetMOH505.HospitalInfo.AddHospitalInfoRow(
                    HospitalName,
                    "",
                    "",
                    EndDate.AddDays(-1).ToString("dd-MMM-yyyy"),
                    EndDate.AddDays(-1).ToShortMonthName(),
                    EndDate.AddDays(-1).Year.ToString(),
                    "1",
                    userLoggedIn.Username,
                    ""
                    );

                List<Diseases_Conditions_Events> diseases_Conditions_Events_a
                    = new List<Diseases_Conditions_Events>();

                List<Diseases_Conditions_Events> diseases_Conditions_Events_b
                    = new List<Diseases_Conditions_Events>();




                //here i get the total count in the list
                int sizeOfList = GetDiseases(StartDate, EndDate).Count();
                int ModulusOfList = sizeOfList % 2;

                if (ModulusOfList != 0)
                {
                    sizeOfList++;
                }

                int HalfTheList = sizeOfList / 2;

                var allDiseases = GetDiseases(StartDate, EndDate);
                diseases_Conditions_Events_a = allDiseases.Take(HalfTheList).ToList();
                diseases_Conditions_Events_b = allDiseases.Skip(HalfTheList).ToList();

                //diseases_Conditions_Events_a = GetDiseases(StartDate, EndDate).Take(HalfTheList).ToList();
                //diseases_Conditions_Events_b = GetDiseases(StartDate, EndDate).Skip(HalfTheList).ToList();

                var theFirstItem = diseases_Conditions_Events_a.ToArray()[2];
                diseases_Conditions_Events_a.RemoveAt(2);
                diseases_Conditions_Events_a.Insert(0, theFirstItem);



                foreach (var item in diseases_Conditions_Events_a)
                {
                    dtSetMOH505.Conditions.AddConditionsRow(
                      item.Name,
                      item.UnderFiveCases,
                      item.OverFiveCases,
                      item.UnderFiveCases + item.OverFiveCases,
                      item.UnderFiveDeaths,
                      item.OverFiveDeaths,
                      item.OverFiveDeaths + item.UnderFiveDeaths
                      );
                }

                foreach (var item in diseases_Conditions_Events_b)
                {
                    dtSetMOH505b.Conditions.AddConditionsRow(
                      item.Name,
                      item.UnderFiveCases,
                      item.OverFiveCases,
                      item.UnderFiveCases + item.OverFiveCases,
                      item.UnderFiveDeaths,
                      item.OverFiveDeaths,
                      item.OverFiveDeaths + item.UnderFiveDeaths
                      );
                }

                //Add the Last column in second List
                dtSetMOH505b.Conditions.AddConditionsRow(
                        "Others(Specify)****",
                        default,
                        default,
                        default,
                        default,
                        default,
                        default
                        );



                List<LabaratoryDs> LstlabaratoryDs = new List<LabaratoryDs>();

                List<string> LstlabTests = new List<string>()
                    {
                         "Malaria",
                         "Shegella Dysentery",
                         "Tuberculosis (MDR/XDR)",
                         "Typhoid"
                    };

                //var Today = DateTime.Today;

                var labaratoryTests = db3.LabTestForMOH505Report.Where(p => p.CreatedUtc.Value >= StartDate
                && p.CreatedUtc.Value <= EndDate).ToList();

                //creates two lists one for under five and the other for over five
                var testsForUnderFive = new List<LabTestForMOH505Report>();
                var testsForOverFive = new List<LabTestForMOH505Report>();

                foreach (var item in labaratoryTests)
                {
                    var age = item.CreatedUtc.Value.Year - item.DOB.Value.Year;

                    if (age >= 5)
                    {
                        testsForOverFive.Add(item);
                    }
                    else
                    {
                        testsForUnderFive.Add(item);
                    }
                }

                foreach (var item in LstlabTests)
                {

                    if (item.Contains("Malaria"))
                    {
                        var dataUnderFive = testsForUnderFive.Where(p => p.LabTest.ToUpper().Contains("BS FOR MPS UNDER 5")).ToList();
                        var dataOverFive = testsForOverFive.Where(p => p.LabTest.ToUpper().Contains("BS FOR MPS")).ToList();

                        var countDataUnderFive = 0;
                        var countDataOverFive = 0;

                        if (dataUnderFive.Count() > 0)
                        {
                            var labtestId = dataUnderFive.FirstOrDefault().LabTestId;
                            var labTest = db3.LabTests.Find(labtestId);

                            if (labTest?.Parameters.Count() > 0)
                            {

                                foreach (var underFive in dataUnderFive)
                                {
                                    countDataUnderFive += db3.WorkOrderTestParameters
                                        .Where(p => p.WorkOrderTest == underFive.WorkOrderTestId)
                                        .Where(x => x.Results.ToLower().Contains("positive")
                                        || x.Results.ToLower().Contains("positive++")
                                        || x.Results.Trim().ToLower().Equals("mps seen")
                                        || x.Results.ToLower().Contains("pos")
                                        || x.Results.ToLower().Contains("+")).GroupBy(x => x.WorkOrderTest).Count();
                                }

                            }
                            else
                            {
                                var dtUnderFive = dataUnderFive.Where(p => p.LabTestResults != null).ToList();

                                countDataUnderFive += dtUnderFive
                                        .Where(p => p.LabTestResults.Contains("positive")
                                        || p.LabTestResults.Contains("positive++")
                                        || p.LabTestResults.Trim().ToLower().Equals("mps seen")
                                        || p.LabTestResults.Contains("+") || 
                                        p.LabTestResults.ToLower().Contains("pos")).Count();

                            }

                        }

                        if (dataOverFive.Count() > 0)
                        {
                            var labtestId = dataOverFive.FirstOrDefault().LabTestId;
                            var labTest = db3.LabTests.Find(labtestId);

                            if (labTest.Parameters.Count() > 0)
                            {

                                foreach (var overFive in dataOverFive)
                                {
                                    countDataOverFive += db3.WorkOrderTestParameters
                                        .Where(x => x.WorkOrderTest == overFive.WorkOrderTestId 
                                        && (x.Results.Contains("positive") ||
                                        x.Results.Contains("pos") ||
                                        x.Results.Trim().Equals("mps seen") ||
                                        x.Results.Contains("+"))).GroupBy(x => x.WorkOrderTest)
                                        .Count();
                                }

                            }
                            else
                            {
                                var dtOverFive = dataOverFive.Where(p => p.LabTestResults != null).ToList();

                                countDataOverFive += dtOverFive
                                        .Where(p => p.LabTestResults.Contains("positive") 
                                        || p.LabTestResults.Contains("positive++") ||
                                        p.LabTestResults.Trim().Equals("mps seen") ||
                                        p.LabTestResults.Contains("+") ||
                                        p.LabTestResults.Contains("pos")).Count();

                            }

                        }

                        LabaratoryDs ds = new LabaratoryDs()
                        {
                            TestName = item,
                            UnderFiveTests = dataUnderFive.Count(),
                            UnderFivePositive = countDataUnderFive,
                            OverFiveTests = dataOverFive.Count(),
                            OverFivePositive = countDataOverFive
                        };

                        LstlabaratoryDs.Add(ds);
                    }
                    else if (item == "Shegella Dysentery")
                    {
                        var dataUnderFive = testsForUnderFive.Where(p => p.LabTest.Contains("STOOL MICROSCOPY")).ToList();
                        var dataOverFive = testsForOverFive.Where(p => p.LabTest.Contains("STOOL MICROSCOPY")).ToList();


                        LabaratoryDs ds = new LabaratoryDs()
                        {
                            TestName = item,
                            UnderFiveTests = dataUnderFive.Count(),
                            UnderFivePositive = 0,
                            OverFiveTests = dataOverFive.Count(),
                            OverFivePositive = 0
                        };

                        #region Depends on configuration Over 5

                        var labtestId = dataOverFive.FirstOrDefault()?.LabTestId ?? dataUnderFive.FirstOrDefault()?.LabTestId;
                        var labTest = db3.LabTests.Find(labtestId);

                        if (labTest?.Parameters.Count() > 0)
                        {

                            foreach (var overFive in dataOverFive)
                            {
                                ds.OverFivePositive += db3.WorkOrderTestParameters
                                    .Where(x => x.WorkOrderTest == overFive.WorkOrderTestId && (x.Results.Contains("growth")))
                                    .Count();
                            }

                        }
                        else
                        {
                            var dtOverFive = dataOverFive.Where(p => p.LabTestResults != null).ToList();

                            ds.OverFivePositive += dtOverFive
                                    .Where(p => p.LabTestResults.Contains("growth")).Count();

                        }
                        #endregion
                        #region Depends on configuration Under 5

                        if (labTest?.Parameters.Count() > 0)
                        {

                            foreach (var underFive in dataUnderFive)
                            {
                                ds.UnderFivePositive += db3.WorkOrderTestParameters
                                    .Where(p => p.WorkOrderTest == underFive.WorkOrderTestId)
                                    .Where(x => x.Results.Contains("growth")).Count();
                            }

                        }
                        else
                        {
                            var dtUnderFive = dataUnderFive.Where(p => p.LabTestResults != null).ToList();

                            ds.UnderFivePositive += dtUnderFive
                                    .Where(p => p.LabTestResults.Contains("growth")).Count();

                        }


                        #endregion

                        LstlabaratoryDs.Add(ds);
                    }
                    else if (item.Contains("Tuberculosis"))
                    {
                        var dataUnderFive = testsForUnderFive.Where(p => p.LabTest.Contains("SPITUM FOR GENE EXPERT")).ToList();
                        var dataOverFive = testsForOverFive.Where(p => p.LabTest.Contains("SPITUM FOR GENE EXPERT")).ToList();

                        LabaratoryDs ds = new LabaratoryDs()
                        {
                            TestName = item,
                            UnderFiveTests = dataUnderFive.Count(),
                            UnderFivePositive = 0,
                            OverFiveTests = dataOverFive.Count(),
                            OverFivePositive = 0
                        };

                        #region Depends on configuration Over 5

                        var labtestId = dataOverFive.FirstOrDefault()?.LabTestId ?? dataUnderFive.FirstOrDefault()?.LabTestId;
                        var labTest = db3.LabTests.Find(labtestId);
                        if (labTest != null)
                        {
                            if (labTest.Parameters.Count() > 0)
                            {

                                foreach (var overFive in dataOverFive)
                                {
                                    ds.OverFivePositive += db3.WorkOrderTestParameters
                                        .Where(x => x.WorkOrderTest == overFive.WorkOrderTestId && (x.Results.Contains("positive")))
                                        .Count();
                                }

                            }
                            else
                            {
                                var dtOverFive = dataOverFive.Where(p => p.LabTestResults != null).ToList();

                                ds.OverFivePositive += dtOverFive
                                        .Where(p => p.LabTestResults.Contains("positive")).Count();

                            }
                        }
                        #endregion
                        #region Depends on configuration Under 5

                        if (labTest != null)
                        {
                            if (labTest.Parameters.Count() > 0)
                            {

                                foreach (var underFive in dataUnderFive)
                                {
                                    ds.UnderFivePositive += db3.WorkOrderTestParameters
                                        .Where(p => p.WorkOrderTest == underFive.WorkOrderTestId)
                                        .Where(x => x.Results.Contains("positive")).Count();
                                }

                            }
                            else
                            {
                                var dtUnderFive = dataUnderFive.Where(p => p.LabTestResults != null).ToList();

                                ds.UnderFivePositive += dtUnderFive
                                        .Where(p => p.LabTestResults.Contains("positive")).Count();

                            }
                        }

                        #endregion


                        LstlabaratoryDs.Add(ds);
                    }
                    else if (item.Contains("Typhoid"))
                    {
                        var dataUnderFive = testsForUnderFive.Where(p => p.LabTest.Contains("SALMONELLA ANTIGEN TEST")).ToList();
                        var dataOverFive = testsForOverFive.Where(p => p.LabTest.Contains("SALMONELLA ANTIGEN TEST")).ToList();

                        LabaratoryDs ds = new LabaratoryDs()
                        {
                            TestName = item,
                            UnderFiveTests = dataUnderFive.Count(),
                            UnderFivePositive = 0,
                            OverFiveTests = dataOverFive.Count(),
                            OverFivePositive = 0

                        };

                        #region Depends on configuration Over 5

                        var labtestId = dataOverFive.FirstOrDefault()?.LabTestId ?? dataUnderFive.FirstOrDefault()?.LabTestId;
                        var labTest = db3.LabTests.Find(labtestId);

                        if (labTest?.Parameters?.Count() > 0)
                        {

                            foreach (var overFive in dataOverFive)
                            {
                                ds.OverFivePositive += db3.WorkOrderTestParameters
                                    .Where(x => x.WorkOrderTest == overFive.WorkOrderTestId && (x.Results.Contains("positive")))
                                    .Count();
                            }

                        }
                        else
                        {
                            var dtOverFive = dataOverFive.Where(p => p.LabTestResults != null).ToList();

                            ds.OverFivePositive += dtOverFive
                                    .Where(p => p.LabTestResults.Contains("positive")).Count();

                        }
                        #endregion
                        #region Depends on configuration Under 5

                        if (labTest?.Parameters?.Count() > 0)
                        {

                            foreach (var underFive in dataUnderFive)
                            {
                                ds.UnderFivePositive += db3.WorkOrderTestParameters
                                    .Where(p => p.WorkOrderTest == underFive.WorkOrderTestId)
                                    .Where(x => x.Results.Contains("positive")).Count();
                            }

                        }
                        else
                        {
                            var dtUnderFive = dataUnderFive.Where(p => p.LabTestResults != null).ToList();

                            ds.UnderFivePositive += dtUnderFive
                                    .Where(p => p.LabTestResults.Contains("positive")).Count();

                        }


                        #endregion

                        LstlabaratoryDs.Add(ds);
                    }

                }

                foreach (var item in LstlabaratoryDs)
                {
                    var totalTested = item.UnderFiveTests + item.OverFiveTests;
                    var totalPositive = item.UnderFivePositive + item.OverFivePositive;
                    dtSetMOH505.Labaratory.AddLabaratoryRow(
                        item.TestName,
                        item.UnderFiveTests,
                        item.UnderFivePositive,
                        item.OverFiveTests,
                        item.OverFivePositive,
                        totalTested,
                        totalPositive
                        );
                }

                ReportDocument report = new ReportDocument();
                report.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH505/RptMOH505.rpt")));
                report.SetDataSource(dtSetMOH505);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                report.Subreports[0].SetDataSource(dtSetMOH505);
                report.Subreports[1].SetDataSource(dtSetMOH505b);
                report.Subreports[2].SetDataSource(dtSetMOH505);
                //report.Subreports["RptCurrentStock.rpt"].SetDataSource(dataSet);
                Stream str = report.ExportToStream(
                    CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                str.Seek(0, SeekOrigin.Begin);

                str.Seek(0, SeekOrigin.Begin);
                //PrinterSettings printerSettings = new PrinterSettings();
                //report.PrintOptions.PrinterName = printerSettings.PrinterName;
                //report.PrintToPrinter(1, false, 0, 0);

                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                string name = "MOH 505" + Timestamp + ".pdf";


                return File(str, "application/pdf", name);
            }
        }

        List<PatientDiagnosi> patientDiagnosis = new List<PatientDiagnosi>();
        private List<Diseases_Conditions_Events> GetDiseases(DateTime StartDate, DateTime EndDate)
        {
            var diseases = db2.Diseases
                .Where(p => p.EpidemicFlag == true && p.DiseaseName != "Poliomyelitis (AFP)" && p.DiseaseName != "Tuberculosis")
                .ToList();
            patientDiagnosis = db2.PatientDiagnosis.Where(p => p.TimeAdded >= StartDate && p.TimeAdded <= EndDate).ToList();

            List<Diseases_Conditions_Events> theListofDiseases
            = new List<Diseases_Conditions_Events>();

            foreach (var item in diseases)
            {
                var selectedPatients = SeparatePatients(item.DiseaseName, patientDiagnosis);

                Diseases_Conditions_Events dis = new Diseases_Conditions_Events()
                {
                    Name = item.DiseaseName,
                    UnderFiveCases = selectedPatients.UnderFiveOpdNumbers.Count(),
                    OverFiveCases = selectedPatients.FiveAndOverOpdNumbers.Count(),
                    UnderFiveDeaths = default,
                    OverFiveDeaths = default
                };

                theListofDiseases.Add(dis);
            }


            return theListofDiseases.OrderBy(p => p.Name).ToList();

        }

        private int NumberOfCases(string name, List<PatientDiagnosi> pDiagnosis)
        {
            int numberOfCases = patientDiagnosis.Where(p => p.FinalDiagnosis == name).Count();
            return numberOfCases;
        }

        public class casesOpdNumbers
        {
            public List<int> UnderFiveOpdNumbers { get; set; }
            public List<int> FiveAndOverOpdNumbers { get; set; }
        }
        private casesOpdNumbers SeparatePatients(string name, List<PatientDiagnosi> pDiagnosis)
        {
            var patients = pDiagnosis.Where(p => p.FinalDiagnosis.Contains(name)).ToList();
            var selectedDistinctOpdNumbers = patients.DistinctBy(p => p.OPDNo).Select(p => p.OPDNo).ToList();

            casesOpdNumbers casesOpdNumbers = new casesOpdNumbers()
            {
                UnderFiveOpdNumbers = new List<int>(),
                FiveAndOverOpdNumbers = new List<int>()
            };

            foreach (var item in selectedDistinctOpdNumbers)
            {
                //go to opdRegister and get patient details 
                var Patient = db2.OpdRegisters.Where(p => p.Id == item).FirstOrDefault().Patient;

                DateTime dateOfBirth;

                if (Patient.DOB.HasValue)
                {
                    dateOfBirth = new DateTime();
                    dateOfBirth = Patient.DOB.Value;

                    var timeAdded = patients.Where(p => p.OPDNo == item).FirstOrDefault().TimeAdded;

                    var years = timeAdded.Year - dateOfBirth.Year;

                    if (years >= 5)
                    {
                        casesOpdNumbers.FiveAndOverOpdNumbers.Add(item);
                    }
                    else
                    {
                        casesOpdNumbers.UnderFiveOpdNumbers.Add(item);
                    }
                }

            }



            return casesOpdNumbers;
        }
        #endregion

        #region MOH 717 Report
        public ActionResult MOH717()
        {
            return View();
        }

        DataSetMoh717 dataSetMoh717 = new DataSetMoh717();

        public ActionResult GenerateMOH717Report(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            var dataSetMoh717OutPatient = GetAllOutPatientData(FromDate, ToDate);
            var dataSetMohCasualty = GetCasualtyData(FromDate, ToDate);
            var dataSetMohSpecialClinics = GetSpecialClinicsData(FromDate, ToDate);
            var dataSetMohMch = GetMchPatientsData(FromDate, ToDate);
            var dataSetMohDental = GetDentalClinicsData(FromDate, ToDate);
            var dataSetInPatient = GetInPatientsData(FromDate, ToDate);
            var dataSetInPatientAdmissions = GetAdmisionsData(FromDate, ToDate);
            var dataSetMaternityServices = GetMaternityServices(FromDate, ToDate);
            var dataSetOperation = GetOperationsData(FromDate, ToDate);
            var dataSetPharmacy = GetPharmacyDrugs(FromDate, ToDate);
            var dataSetMortuary = GetMortuaryData(FromDate, ToDate);
            var dataSetMedicalRecords = GetMedicalRecordsIssuedData(FromDate, ToDate);
            var dataSetFinance = GetFinanceReportData(FromDate, ToDate);
            var dataSetA6ToA12 = GetA6ToA12Data(FromDate, ToDate);
            var dataSetSpecialServices = GetSpecialServices(FromDate, ToDate);

            var SumOutPatientNewAttendances = dataSetMoh717OutPatient.OutPatientService.Sum(p => p.NewAttendances);
            var SumOutPatientReAttendances = dataSetMoh717OutPatient.OutPatientService.Sum(p => p.ReAttendances);
            var SumSpecialClinicsNewAttendances = dataSetMohSpecialClinics.OutPatientService.Sum(p => p.NewAttendances);
            var SumSpecialClinicsReAttendances = dataSetMohSpecialClinics.OutPatientService.Sum(p => p.ReAttendances);
            var SumDentalNewAttendances = dataSetMohDental.OutPatientService.Sum(p => p.NewAttendances);
            var SumDentalReAttendances = dataSetMohDental.OutPatientService.Sum(p => p.ReAttendances);
            var SumMchNewAttendances = dataSetMohMch.OutPatientService.Sum(p => p.NewAttendances);
            var SumMchReAttendances = dataSetMohMch.OutPatientService.Sum(p => p.ReAttendances);

            var a6toa12 = dataSetA6ToA12.A6ToA12;

            DataSetA6ToA12 FromA6ToA12 = new DataSetA6ToA12();

            var totalNewAttendances = SumOutPatientNewAttendances + SumSpecialClinicsNewAttendances + SumDentalNewAttendances + SumMchNewAttendances;
            var totalReattendances = SumOutPatientReAttendances + SumSpecialClinicsReAttendances + SumDentalReAttendances + SumMchReAttendances;

            FromA6ToA12.A6ToA12.AddA6ToA12Row(
                totalNewAttendances,
                totalReattendances,
                dataSetA6ToA12.A6ToA12.Select(p => p.MedicalExaminations).FirstOrDefault(),
                dataSetA6ToA12.A6ToA12.Select(p => p.Dressings).FirstOrDefault(),
                dataSetA6ToA12.A6ToA12.Select(p => p.RemovalOfStitches).FirstOrDefault(),
                dataSetA6ToA12.A6ToA12.Select(p => p.Injections).FirstOrDefault(),
                dataSetA6ToA12.A6ToA12.Select(p => p.Stitching).FirstOrDefault(),
                dataSetA6ToA12.A6ToA12.Select(p => p.PlasterOfParis).FirstOrDefault(),
                dataSetA6ToA12.A6ToA12.Select(p => p.MedicalReports).FirstOrDefault()
                );

            Reports.MOH717.DataSetHospitalDetails dataSetHospitalDetails = new Reports.MOH717.DataSetHospitalDetails();

            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            int session = Convert.ToInt32(Session["UserId"]);
            var userLoggedIn = db2.Users.Where(p => p.Id == session).FirstOrDefault();

            dataSetHospitalDetails.HospitalDetails.AddHospitalDetailsRow(

                "",
                "",
                HospitalName,
                ToDate.ToShortMonthName(),
                toDate.Year.ToString(),
                ""
                );

            //var count = dataSetMoh717.OutPatientService.Count;

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH717/RptMOH717.rpt")));
            Rd.SetDataSource(dataSetHospitalDetails);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Rd.Subreports["OutPatientServices.rpt"].SetDataSource(dataSetMoh717OutPatient);
            Rd.Subreports["RptCasualty.rpt"].SetDataSource(dataSetMohCasualty);
            Rd.Subreports["RptSpecialClinics.rpt"].SetDataSource(dataSetMohSpecialClinics);
            Rd.Subreports["RptMCH.rpt"].SetDataSource(dataSetMohMch);
            Rd.Subreports["RptDental.rpt"].SetDataSource(dataSetMohDental);
            Rd.Subreports["RptInpatients.rpt"].SetDataSource(dataSetInPatient);
            Rd.Subreports["RptAdmissions.rpt"].SetDataSource(dataSetInPatientAdmissions);
            Rd.Subreports["RptMaternityServices.rpt"].SetDataSource(dataSetMaternityServices);
            Rd.Subreports["RptOperations.rpt"].SetDataSource(dataSetOperation);
            Rd.Subreports["RptPharmacy.rpt"].SetDataSource(dataSetPharmacy);
            Rd.Subreports["RptMortuary.rpt"].SetDataSource(dataSetMortuary);
            Rd.Subreports["RptMedicalRecordsIssued.rpt"].SetDataSource(dataSetMedicalRecords);
            Rd.Subreports["RptSpecialServices.rpt"].SetDataSource(dataSetSpecialServices);
            Rd.Subreports["RptFinance.rpt"].SetDataSource(dataSetFinance);
            Rd.Subreports["RptFromA6toA12.rpt"].SetDataSource(FromA6ToA12);

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "MOH 717 " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        #region Private Methods for MOH 717
        private DataSetMoh717 GetAllOutPatientData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717 dataSet = new DataSetMoh717();
            var LastMonth = FromDate.AddMonths(-1);
            var today = FromDate;


            var opdRegisters = db2.OpdRegisters.Where(p => DbFunctions.TruncateTime(p.Date) >= FromDate.Date && DbFunctions.TruncateTime(p.Date) <= ToDate.Date).ToList();
            //var Outpatient = opdRegisters
            //    .Where(p => p.Department == null || p.Department.Contains("OUTPATIENT") && p.Patient.RegDate.Value.Month > LastMonth.Month)
            //    .Where(x => x.Status != "draft")
            //    .ToList();
            var Outpatient = opdRegisters
                .Where(p => (p.Department == null || p.Department.Contains("OUTPATIENT") || p.Department.Contains("OPD")
                || p.Department.Contains("OUTPATIENT")) && !p.IsIPD && p.Patient.RegDate.Value.Month > LastMonth.Month)
                .Where(x => x.Status != "draft" && x.IsIPD == false)
                .ToList();

            var selectedPatients = Outpatient.Select(p => p.Patient).ToList();

            //var NewPatientsRegistered = db2.Patients.Where(p => p.RegDate.Value.Month > LastMonth.Month).ToList();
            // var NewPatientsRegistered = selectedPatients.DistinctBy(p => p.RegNumber).ToList();
            //List<Patient> MalePatients = new List<Patient>();
            //List<Patient> FeMalePatients = new List<Patient>();



            List<OpdRegister> MalePatients = new List<OpdRegister>();
            List<OpdRegister> FeMalePatients = new List<OpdRegister>();

            foreach (var item in opdRegisters)
            {
                if (item.Patient.Gender == "M")
                {
                    MalePatients.Add(item);
                }
                else
                {
                    FeMalePatients.Add(item);
                }
            }

            int NewMalePatientsOverFive = 0;
            int NewMalePatientsUnderFive = 0;
            int RevMalePatientsOverFive = 0;
            int RevMalePatientsUnderFive = 0;

            foreach (var item in MalePatients)
            {
                if (item.Patient.DOB.HasValue)
                {
                    int Years = today.Year - item.Patient.DOB.Value.Year;

                    if (Years >= 5)
                    {
                        if (item.Patient.OpdRegisters.Count() == 1 && item.Patient.RegDate.Value.Month == today.Month)
                        {
                            NewMalePatientsOverFive++;
                        }
                        else
                        {
                            RevMalePatientsOverFive++;
                        }

                    }
                    else
                    {
                        if (item.Patient.OpdRegisters.Count() == 1 && item.Patient.RegDate.Value.Month == today.Month)
                        {
                            NewMalePatientsUnderFive++;
                        }
                        else
                        {
                            RevMalePatientsUnderFive++;
                        }

                    }
                }

            }

            int NewFeMalePatientsOverFive = 0;
            int NewFeMalePatientsUnderFive = 0;
            int RevFeMalePatientsOverFive = 0;
            int RevFeMalePatientsUnderFive = 0;

            foreach (var item in FeMalePatients)
            {
                if (item.Patient.DOB.HasValue)
                {
                    int Years = today.Year - item.Patient.DOB.Value.Year;

                    if (Years >= 5)
                    {
                        if (item.Patient.OpdRegisters.Count() == 1 && item.Patient.RegDate.Value.Month == today.Month)
                        {
                            NewFeMalePatientsOverFive++;
                        }
                        else
                        {
                            RevFeMalePatientsOverFive++;
                        }

                    }
                    else
                    {
                        if (item.Patient.OpdRegisters.Count() == 1 && item.Patient.RegDate.Value.Month == today.Month)
                        {
                            NewFeMalePatientsUnderFive++;
                        }
                        else
                        {
                            RevFeMalePatientsUnderFive++;
                        }

                    }
                }
            }


            List<Moh717ViewModel> ListMoh717Data = new List<Moh717ViewModel>()
            {
                new Moh717ViewModel(){ Name="Over 5 - Male", NewAttendances=NewMalePatientsOverFive,ReAttendances=RevMalePatientsOverFive,Total= NewMalePatientsOverFive + RevMalePatientsOverFive },
                new Moh717ViewModel(){ Name="Over 5 - Female", NewAttendances=NewFeMalePatientsOverFive,ReAttendances=RevFeMalePatientsOverFive,Total= NewFeMalePatientsOverFive + RevFeMalePatientsOverFive},
                new Moh717ViewModel(){ Name="Children Under 5 - Male", NewAttendances=NewMalePatientsUnderFive,ReAttendances=RevMalePatientsUnderFive,Total=NewMalePatientsUnderFive +RevMalePatientsUnderFive},
                new Moh717ViewModel(){ Name="Children Under 5 - Female", NewAttendances=NewFeMalePatientsUnderFive,ReAttendances=RevFeMalePatientsUnderFive,Total= NewFeMalePatientsUnderFive + RevFeMalePatientsUnderFive }
            };

            foreach (var item in ListMoh717Data)
            {
                dataSet.OutPatientService.AddOutPatientServiceRow(
                  item.Name, item.NewAttendances, item.ReAttendances, item.Total
                  );
            }
            return dataSet;
        }

        private DataSetMoh717 GetCasualtyData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717 dataSet = new DataSetMoh717();
            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = FromDate;

            var opdRegisters = db2.OpdRegisters.Where(p => DbFunctions.TruncateTime(p.TimeAdded.Value) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded.Value) <= ToDate && p.Status.Contains("Emergency")).ToList();

            var NewEmergencyPatients = opdRegisters
                .Where(p => p.Patient.OpdRegisters.Count() == 1)
                .ToList();

            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);
            var OldEmergencyPatients = opdRegisters.Where(p => p.Patient.OpdRegisters.Count() > 1)
                .ToList();

            dataSet.OutPatientService.AddOutPatientServiceRow(
                "",
                NewEmergencyPatients.Count(),
                OldEmergencyPatients.Count(),
                NewEmergencyPatients.Count() + OldEmergencyPatients.Count()
                );


            return dataSet;
        }

        private DataSetMoh717 GetSpecialClinicsData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717 dataSet = new DataSetMoh717();
            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = FromDate;
            FromDate.AddDays(1);

            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);

            var opdRegisters = db2.OpdRegisters
                                .Where(p => DbFunctions.TruncateTime(p.Date) >= FromDate && DbFunctions.TruncateTime(p.Date) <= ToDate && p.IsIPD == false)
                                .ToList();

            Moh717ViewModel ENTClinicModel = new Moh717ViewModel()
            {
                Name = "E.N.T Clinic"
            };
            Moh717ViewModel NewEyeClinicModel = new Moh717ViewModel()
            {
                Name = "Eye Clinic"
            };

            //var tbResponses = db2.TBScreeningResponses
            //    .Where(p => p.DateAdded > DbFunctions.TruncateTime(FromDate) && DbFunctions.TruncateTime(p.DateAdded) <= ToDate).ToList();


            //Moh717ViewModel TBLeprosyModel = new Moh717ViewModel()
            //{
            //    Name = "TB & Leprosy",
            //    NewAttendances = tbResponses.DistinctBy(p => p.OPDNo).Count(),
            //    ReAttendances = 0,
            //    Total = tbResponses.DistinctBy(p => p.OPDNo).Count()
            //};

            var tbResponses = db2.OpdRegisters
               .Where(p => (p.Department.Contains("TB & Leprosy") || p.Department.Contains("TB") ||
               p.Department.Contains("Leprosy")) && p.TimeAdded > DbFunctions.TruncateTime(FromDate)
               && DbFunctions.TruncateTime(p.Date) <= ToDate).ToList();

            var alltbResponses = db2.OpdRegisters
               .Where(p => (p.Department.Contains("TB & Leprosy") || p.Department.Contains("TB") &&
               p.Department.Contains("Leprosy"))).ToList();

            Moh717ViewModel TBLeprosyModel = new Moh717ViewModel()
            {
                Name = "TB & Leprosy",
                NewAttendances = tbResponses.Where(e => e.Patient.OpdRegisters.Where(p => (p.Department?.Contains("TB & Leprosy") ?? false
                || (p.Department?.Contains("TB") ?? false) ||
                (p.Department?.Contains("Leprosy") ?? false))).Count() == 1).Count(),

                ReAttendances = alltbResponses.Where(e => e.Patient.OpdRegisters.Where(p => (p.Department?.Contains("TB & Leprosy") ?? false
                || (p.Department?.Contains("TB") ?? false) ||
                (p.Department?.Contains("Leprosy") ?? false))).Count() > 1).Count(),

                Total = tbResponses.Count()
            };

            var allDiagnosis = db2.PatientDiagnosis
                .Where(p => p.FinalDiagnosis.Contains("sexually transmitted infections") && DbFunctions.TruncateTime(p.TimeAdded) > FromDate && DbFunctions.TruncateTime(p.TimeAdded) <= ToDate)
                .ToList();
            var newAttendancesSexually = allDiagnosis.Where(p => p.OpdRegister.Patient.OpdRegisters.Count() == 1)
                                             .Count();
            var revisitCasesSexally = allDiagnosis.Where(p => p.OpdRegister.Patient.OpdRegisters.Count() > 1)
                                             .Count();


            Moh717ViewModel SexuallyTransmittedInfectionsModel = new Moh717ViewModel()
            {
                Name = "Sexually Transmitted Infections",
                NewAttendances = newAttendancesSexually,
                ReAttendances = revisitCasesSexally,
                Total = newAttendancesSexually + revisitCasesSexally

            };
            Moh717ViewModel NewPsychiatryModel = new Moh717ViewModel()
            {
                Name = "Psychiatry"
            };
            Moh717ViewModel NewOrthopedicModel = new Moh717ViewModel()
            {
                Name = "Orthopaedic Clinic"
            };
            Moh717ViewModel NewOccupationalTherapyModel = new Moh717ViewModel()
            {
                Name = "Occupational Therapy"
            };
            Moh717ViewModel NewPhysiotherapyModel = new Moh717ViewModel()
            {
                Name = "Physiotherapy"
            };
            Moh717ViewModel MedicalClinicsModel = new Moh717ViewModel()
            {
                Name = "Medical Clinic",

            };
            Moh717ViewModel SurgicalClinicsModel = new Moh717ViewModel()
            {
                Name = "Surgical Clinics"
            };
            Moh717ViewModel PaedeatricsModel = new Moh717ViewModel()
            {
                Name = "Paedeatrics"
            };
            Moh717ViewModel ObstetricsGynaecologyModel = new Moh717ViewModel()
            {
                Name = "Obstetrics/Gynaecology"
            };

            foreach (var item in opdRegisters)
            {
                if (item.Patient.RegDate.HasValue)
                {
                    if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.Department == "ENT")
                    {
                        ENTClinicModel.NewAttendances++;
                    }
                    else if (item.Department == "ENT")
                    {
                        ENTClinicModel.ReAttendances++;
                    }
                    else if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.Department == "EYE UNIT")
                    {
                        NewEyeClinicModel.NewAttendances++;
                    }
                    else if (item.Department == "EYE UNIT")
                    {
                        NewEyeClinicModel.ReAttendances++;
                    }
                    else if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.Department == "PSYCHIATRIC")
                    {
                        NewPsychiatryModel.NewAttendances++;
                    }
                    else if (item.Department == "PSYCHIATRIC")
                    {
                        NewPsychiatryModel.ReAttendances++;
                    }
                    else if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.Department == "ORTHOPAEDIC")
                    {
                        NewOrthopedicModel.NewAttendances++;
                    }
                    else if (item.Department == "ORTHOPAEDIC")
                    {
                        NewOrthopedicModel.ReAttendances++;
                    }
                    else if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.Department == "OCCUPATIONAL THERAPY")
                    {
                        NewOccupationalTherapyModel.NewAttendances++;
                    }
                    else if (item.Department == "OCCUPATIONAL THERAPY")
                    {
                        NewOccupationalTherapyModel.ReAttendances++;
                    }
                    else if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.Department == "PHYSIOTHERAPY")
                    {
                        NewPhysiotherapyModel.NewAttendances++;
                    }
                    else if (item.Department == "PHYSIOTHERAPY")
                    {
                        NewPhysiotherapyModel.ReAttendances++;
                    }
                }
            }

            foreach (var item in opdRegisters)
            {
                if (item.Patient.RegDate.HasValue)
                {
                    if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("MOPC")))
                    {
                        MedicalClinicsModel.NewAttendances++;
                    }
                    else if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("MOPC")))
                    {
                        MedicalClinicsModel.ReAttendances++;
                    }
                }
            }

            foreach (var item in opdRegisters)
            {
                if (item.Patient.RegDate.HasValue)
                {
                    if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("SOPC")))
                    {
                        SurgicalClinicsModel.NewAttendances++;
                    }
                    else if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("SOPC")))
                    {
                        SurgicalClinicsModel.ReAttendances++;
                    }

                }
            }

            foreach (var item in opdRegisters)
            {
                if (item.Patient.RegDate.HasValue)
                {
                    if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("POPC")))
                    {
                        PaedeatricsModel.NewAttendances++;
                    }
                    else if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("POPC")))
                    {
                        PaedeatricsModel.ReAttendances++;
                    }
                }
            }

            foreach (var item in opdRegisters)
            {
                if (item.Patient.RegDate.HasValue)
                {
                    if (item.Patient.RegDate.Value.Month == today.Month && item.Patient.OpdRegisters.Count() == 1 && item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("GOPC")))
                    {
                        ObstetricsGynaecologyModel.NewAttendances++;
                    }
                    else if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("GOPC")))
                    {
                        ObstetricsGynaecologyModel.ReAttendances++;
                    }
                }
            }


            List<Moh717ViewModel> moh717ViewModels = new List<Moh717ViewModel>();

            moh717ViewModels.Add(ENTClinicModel);
            moh717ViewModels.Add(NewEyeClinicModel);
            moh717ViewModels.Add(TBLeprosyModel);
            moh717ViewModels.Add(SexuallyTransmittedInfectionsModel);
            moh717ViewModels.Add(NewPsychiatryModel);
            moh717ViewModels.Add(NewOrthopedicModel);
            moh717ViewModels.Add(NewOccupationalTherapyModel);
            moh717ViewModels.Add(NewPhysiotherapyModel);
            moh717ViewModels.Add(MedicalClinicsModel);
            moh717ViewModels.Add(SurgicalClinicsModel);
            moh717ViewModels.Add(PaedeatricsModel);
            moh717ViewModels.Add(ObstetricsGynaecologyModel);

            foreach (var item in moh717ViewModels)
            {
                dataSet.OutPatientService.AddOutPatientServiceRow(
                  item.Name, item.NewAttendances, item.ReAttendances, item.NewAttendances + item.ReAttendances
                  );
            }

            return dataSet;
        }

        private DataSetMoh717 GetMchPatientsData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717 dataSet = new DataSetMoh717();

            dataSet.OutPatientService.AddOutPatientServiceRow(
                "CWC Attendances", 0, 0, 0
                 );
            dataSet.OutPatientService.AddOutPatientServiceRow(
                "ANC Attendances", 0, 0, 0
                 );
            dataSet.OutPatientService.AddOutPatientServiceRow(
                "PNC Attendances", 0, 0, 0
                 );
            dataSet.OutPatientService.AddOutPatientServiceRow(
                "FP Attendances", 0, 0, 0
                 );

            return dataSet;
        }

        private DataSetMoh717 GetDentalClinicsData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717 dataSet = new DataSetMoh717();
            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = FromDate;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);

            var opdRegisters = db2.OpdRegisters
                                .Where(p => DbFunctions.TruncateTime(p.Date) >= FromDate && DbFunctions.TruncateTime(p.Date) <= ToDate && p.Patient.RegDate.HasValue == true)
                                .ToList();
            var NewDentalPatients = opdRegisters
            .Where(p => p.Department != null && p.Department.Trim().ToLower().Contains("dental") && p.Patient.OpdRegisters.Count() == 1)
            .ToList();

            var NewAttendances = NewDentalPatients.Select(p => p.BillServices).ToList();


            var AttendancesBillServices = new List<BillService>();
            var FillingBillServices = new List<BillService>();
            var ExtractionsBillServices = new List<BillService>();
            foreach (var bills in NewAttendances)
            {
                foreach (var item in bills)
                {
                    if (item.ServiceName.ToLower().Trim().Contains("filling") || item.ServiceName.ToLower().Trim().Contains("extraction"))
                    {
                        if (item.ServiceName.ToLower().Trim().Contains("filling"))
                        {
                            FillingBillServices.Add(item);
                        }
                        else if (item.ServiceName.ToLower().Trim().Contains("extraction"))
                        {
                            ExtractionsBillServices.Add(item);
                        }
                    }
                    else
                    {
                        AttendancesBillServices.Add(item);
                    }
                }
            }

            var NewPatientsAttendances = new List<Patient>();
            var RePatientsAttendances = new List<Patient>();
            var NewPatientsFillings = new List<Patient>();
            var OldPatientsFillings = new List<Patient>();
            var NewPatientsExtractions = new List<Patient>();
            var OldPatientsExtractions = new List<Patient>();

            foreach (var item in AttendancesBillServices.DistinctBy(p => p.OPDNo))
            {
                if (item.OpdRegister.Patient.RegDate.Value.Month == FromDate.Month && item.OpdRegister.Patient.OpdRegisters.Count() == 1)
                {

                    NewPatientsAttendances.Add(item.OpdRegister.Patient);

                }
                else
                {
                    RePatientsAttendances.Add(item.OpdRegister.Patient);
                }
            }

            foreach (var item in FillingBillServices.DistinctBy(p => p.OPDNo))
            {
                if (item.OpdRegister.Patient.RegDate.Value.Month == FromDate.Month && item.OpdRegister.Patient.OpdRegisters.Count() == 1)
                {
                    NewPatientsFillings.Add(item.OpdRegister.Patient);


                }
                else
                {
                    OldPatientsFillings.Add(item.OpdRegister.Patient);
                }
            }

            foreach (var item in ExtractionsBillServices.DistinctBy(p => p.OPDNo))
            {
                if (item.OpdRegister.Patient.RegDate.Value.Month == FromDate.Month && item.OpdRegister.Patient.OpdRegisters.Count() == 1)
                {

                    NewPatientsExtractions.Add(item.OpdRegister.Patient);

                }
                else
                {
                    OldPatientsExtractions.Add(item.OpdRegister.Patient);
                }
            }

            List<Moh717ViewModel> dentalData = new List<Moh717ViewModel>()
            {
                new Moh717ViewModel(){
                    Name ="Attendances(Excluding fillings and extractions)",
                    NewAttendances = NewPatientsAttendances.DistinctBy(p=>p.RegNumber).Count(),
                    ReAttendances = RePatientsAttendances.DistinctBy(p=>p.RegNumber).Count(),
                    Total =NewPatientsAttendances.DistinctBy(p=>p.RegNumber).Count() + RePatientsAttendances.DistinctBy(p=>p.RegNumber).Count()
                },
                 new Moh717ViewModel(){
                    Name ="Fillings",
                    NewAttendances = NewPatientsFillings.DistinctBy(p=>p.RegNumber).Count(),
                    ReAttendances = OldPatientsFillings.DistinctBy(p=>p.RegNumber).Count(),
                    Total =NewPatientsFillings.DistinctBy(p=>p.RegNumber).Count() + OldPatientsFillings.DistinctBy(p=>p.RegNumber).Count()
                },
                  new Moh717ViewModel(){
                    Name ="Extractions",
                    NewAttendances = NewPatientsExtractions.DistinctBy(p=>p.RegNumber).Count(),
                    ReAttendances = OldPatientsExtractions.DistinctBy(p=>p.RegNumber).Count(),
                    Total = NewPatientsExtractions.DistinctBy(p=>p.RegNumber).Count() + OldPatientsExtractions.DistinctBy(p=>p.RegNumber).Count()
                }
            };

            foreach (var item in dentalData)
            {
                dataSet.OutPatientService.AddOutPatientServiceRow(
                  item.Name, item.NewAttendances, item.ReAttendances, item.Total
                  );
            }

            return dataSet;
        }

        public class Moh717InpatientsViewModel
        {
            public string NameOfService { get; set; }
            public int Medical { get; set; }
            public int Surgical { get; set; }
            public int ObstGyn { get; set; }
            public int Paediatrics { get; set; }
            public int Maternity { get; set; }
            public int Eye { get; set; }
            public int NurseryNewBorn { get; set; }
            public int Orthopaedic { get; set; }
            public int Isolation { get; set; }
            public int Amenity { get; set; }
            public int Psychiatry { get; set; }
            public int Other { get; set; }
        }
        private DataSetMoh717Inpatients GetInPatientsData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717Inpatients dataSet = new DataSetMoh717Inpatients();
            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = FromDate;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);

            var opdRegisters = db2.OpdRegisters
                                .Where(p => p.Discharged == true && DbFunctions.TruncateTime(p.DischargeDate.Value) >= FromDate && DbFunctions.TruncateTime(p.DischargeDate.Value) <= ToDate)
                                .ToList();

            var AllDischarges = opdRegisters.Where(p => p.Discharged == true && p.DischargedAlive == null).ToList();
            var AllDeaths = opdRegisters.Where(p => p.Discharged == true && p.DischargedAlive == false).ToList();


            Moh717InpatientsViewModel Discharges = new Moh717InpatientsViewModel()
            {
                NameOfService = "Discharges"
            };
            Moh717InpatientsViewModel Deaths = new Moh717InpatientsViewModel()
            {
                NameOfService = "Deaths"
            };
            Moh717InpatientsViewModel Abscondees = new Moh717InpatientsViewModel()
            {
                NameOfService = "Abscondees"
            };


            foreach (var item in AllDischarges)
            {

                if (item.HSBed.HSWard.WardName == "SURGICAL WARD")
                {
                    Discharges.Surgical++;
                }
                else if (item.HSBed.HSWard.WardName == "PAEDIATRIC WARD")
                {
                    Discharges.Paediatrics++;
                }
                else if (item.HSBed.HSWard.WardName == "MATERNITY")
                {
                    Discharges.Maternity++;
                }
                else if (item.HSBed.HSWard.WardName == "AMENITY WARD")
                {
                    Discharges.Amenity++;
                }
                else
                {
                    Discharges.Other++;
                }
            }

            foreach (var item in AllDeaths)
            {

                if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {
                    Deaths.Surgical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    Deaths.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    Deaths.Maternity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    Deaths.Amenity++;
                }
                else
                {
                    Deaths.Other++;
                }
            }

            List<Moh717InpatientsViewModel> moh717Inpatients = new List<Moh717InpatientsViewModel>();

            moh717Inpatients.Add(Discharges);
            moh717Inpatients.Add(Deaths);
            moh717Inpatients.Add(Abscondees);


            foreach (var item in moh717Inpatients)
            {
                dataSet.InPatients.AddInPatientsRow(
                    item.Medical,
                    item.Surgical,
                    item.ObstGyn,
                    item.Paediatrics,
                    item.Maternity,
                    item.Eye,
                    item.NurseryNewBorn,
                    item.Orthopaedic,
                    item.Other,
                    item.NameOfService,
                    item.Isolation,
                    item.Amenity,
                    item.Psychiatry

                    );
            }

            return dataSet;
        }

        private DataSetMoh717Inpatients GetAdmisionsData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717Inpatients dataSet = new DataSetMoh717Inpatients();

            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = FromDate;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);
            var opdRegisters = db2.OpdRegisters
                                .Where(p => DbFunctions.TruncateTime(p.TimeAdded.Value) >= FromDate && DbFunctions.TruncateTime(p.TimeAdded.Value) <= ToDate && p.IsIPD == true)
                                .ToList();

            var AlltheAdmissions = opdRegisters
                                    .Select(p => p.Patient)
                                    .ToList();
            List<OpdRegister> AdmissionUnderFive = new List<OpdRegister>();
            List<OpdRegister> AdmissionOverFive = new List<OpdRegister>();

            var todaysDate = DateTime.Now;

            foreach (var item in opdRegisters)
            {
                var Years = todaysDate.Year - item.Patient.DOB.Value.Year;

                if (Years > 5)
                {
                    AdmissionOverFive.Add(item);
                }
                else
                {
                    AdmissionUnderFive.Add(item);
                }

            }

            Moh717InpatientsViewModel underFiveAdmission = new Moh717InpatientsViewModel()
            {
                NameOfService = "Admission Under Five"
            };

            Moh717InpatientsViewModel OverFiveAdmission = new Moh717InpatientsViewModel()
            {
                NameOfService = "Admission Over Five"
            };

            Moh717InpatientsViewModel Paroles = new Moh717InpatientsViewModel()
            {
                NameOfService = "Paroles"
            };
            Moh717InpatientsViewModel OccupiedBedDaysNHIF = new Moh717InpatientsViewModel()
            {
                NameOfService = "Occupied Bed Days NHIF"
            };
            Moh717InpatientsViewModel OccupiedBedDaysCash = new Moh717InpatientsViewModel()
            {
                NameOfService = "Occupied Bed Days Cash"
            };
            Moh717InpatientsViewModel WellPersonDays = new Moh717InpatientsViewModel()
            {
                NameOfService = "Well Person Days"
            };
            Moh717InpatientsViewModel BedsAuthorized = new Moh717InpatientsViewModel()
            {
                NameOfService = "Beds - Authorized"
            };
            Moh717InpatientsViewModel BedsActualPhysical = new Moh717InpatientsViewModel()
            {
                NameOfService = "Beds - Actual Physical"
            };
            Moh717InpatientsViewModel CotsAuthorized = new Moh717InpatientsViewModel()
            {
                NameOfService = "Cots - Authorized"
            };
            Moh717InpatientsViewModel CotsActualPhysical = new Moh717InpatientsViewModel()
            {
                NameOfService = "Cots Actual Physical"
            };


            foreach (var item in AdmissionUnderFive)
            {

                if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {
                    underFiveAdmission.Surgical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    underFiveAdmission.Medical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("OBS/GYN"))
                {
                    underFiveAdmission.ObstGyn++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    underFiveAdmission.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    underFiveAdmission.Maternity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    underFiveAdmission.Eye++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY") ||
                    item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NEWBORNS"))
                {
                    underFiveAdmission.NurseryNewBorn++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    underFiveAdmission.Orthopaedic++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    underFiveAdmission.Isolation++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    underFiveAdmission.Amenity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    underFiveAdmission.Psychiatry++;
                }
                else
                {
                    underFiveAdmission.Other++;
                }
            }

            foreach (var item in AdmissionOverFive)
            {
                if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {
                    OverFiveAdmission.Surgical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    OverFiveAdmission.Medical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("OBS/GYN"))
                {
                    OverFiveAdmission.ObstGyn++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    OverFiveAdmission.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    OverFiveAdmission.Maternity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    OverFiveAdmission.Eye++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY") ||
                    item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NEWBORNS"))
                {
                    OverFiveAdmission.NurseryNewBorn++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    OverFiveAdmission.Orthopaedic++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    OverFiveAdmission.Isolation++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    OverFiveAdmission.Amenity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    OverFiveAdmission.Psychiatry++;
                }
                else
                {
                    OverFiveAdmission.Other++;
                }

            }

            var opdNhifMembers = opdRegisters.Where(p => p.Tariff.Company.CompanyName.Trim().ToLower().Contains("nhif")).ToList();
            foreach (var item in opdNhifMembers)
            {
                if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {

                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Surgical += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Surgical += numberOfDays;
                    }
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Medical += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Medical += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("OBST/GYN"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.ObstGyn += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.ObstGyn += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Paediatrics += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Paediatrics += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Maternity += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Maternity += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Maternity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Eye += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Eye += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY/NEWBORN"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.NurseryNewBorn += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.NurseryNewBorn += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Orthopaedic += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Orthopaedic += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Isolation += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Isolation += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Amenity += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Amenity += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Amenity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Psychiatry += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Psychiatry += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Other += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysNHIF.Other += numberOfDays;
                    }
                    //OccupiedBedDaysNHIF.Other++;
                }



            }


            var opdCashMembers = opdRegisters.Where(p => p.Tariff.Company.CompanyName.Trim().ToLower().Contains("cash")).ToList();
            foreach (var item in opdCashMembers)
            {
                if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {

                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Surgical += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Surgical += numberOfDays;
                    }
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Medical += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Medical += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("OBST/GYN"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.ObstGyn += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.ObstGyn += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Paediatrics += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Paediatrics += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Eye += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Eye += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Maternity += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Maternity += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Maternity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY/NEWBORN"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.NurseryNewBorn += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.NurseryNewBorn += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Orthopaedic += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Orthopaedic += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Isolation += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Isolation += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Amenity += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Amenity += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Amenity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Psychiatry += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Psychiatry += numberOfDays;
                    }

                    //OccupiedBedDaysNHIF.Paediatrics++;
                }
                else
                {
                    if (item.Discharged == true)
                    {
                        var numberOfDays = (item.DischargeDate.Value - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Other += numberOfDays;
                    }
                    else
                    {
                        var numberOfDays = (DateTime.Today - item.TimeAdded.Value).Days;
                        if (numberOfDays == 0)
                        {
                            numberOfDays = 1;
                        }
                        OccupiedBedDaysCash.Other += numberOfDays;
                    }
                    //OccupiedBedDaysNHIF.Other++;
                }

            }

            var allWards = db2.HSWards.ToList();
            foreach (var item in allWards)
            {
                if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {
                    BedsActualPhysical.Surgical = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    BedsActualPhysical.Medical = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    BedsActualPhysical.Paediatrics = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("OBST/GYN"))
                {
                    BedsActualPhysical.ObstGyn = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    BedsActualPhysical.Maternity = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    BedsActualPhysical.Eye = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY/NEWBORN") || item.HSWardCategory.WardCategoryName.ToUpper().Contains("NBU") || item.HSWardCategory.WardCategoryName.ToUpper().Contains("NEW BORN"))
                {
                    BedsActualPhysical.NurseryNewBorn = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    BedsActualPhysical.Orthopaedic = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    BedsActualPhysical.Isolation = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    BedsActualPhysical.Psychiatry = item.HSBeds.Count();
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    BedsActualPhysical.Amenity = item.HSBeds.Count();
                }
                else
                {
                    BedsActualPhysical.Other = item.HSBeds.Count();
                }

            }

            foreach (var item in allWards)
            {

                if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {
                    BedsAuthorized.Surgical = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    BedsAuthorized.Medical = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    BedsAuthorized.Paediatrics = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("OBST/GYN"))
                {
                    BedsAuthorized.ObstGyn = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    BedsAuthorized.Maternity = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    BedsAuthorized.Eye = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY/NEWBORN") || item.HSWardCategory.WardCategoryName.ToUpper().Contains("NBU") || item.HSWardCategory.WardCategoryName.ToUpper().Contains("NEW BORN"))
                {
                    BedsAuthorized.NurseryNewBorn = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    BedsAuthorized.Orthopaedic = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    BedsAuthorized.Isolation = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    BedsAuthorized.Psychiatry = item.AuthorizedBeds ?? 0;
                }
                else if (item.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    BedsAuthorized.Amenity = item.AuthorizedBeds ?? 0;
                }
                else
                {
                    BedsAuthorized.Other = item.AuthorizedBeds ?? 0;
                }



            }


            var ParolesViewModel = opdRegisters.Where(p => p.Patient.OpdRegisters.Count(x => x.Discharged == true) > 1).ToList();

            var parolesList = new List<OpdRegister>();

            parolesList = ParolesViewModel.DistinctBy(p => p.PatientId).ToList();

            foreach (var item in parolesList)
            {

                if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                {
                    Paroles.Surgical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MEDICAL"))
                {
                    Paroles.Medical++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                {
                    Paroles.Paediatrics++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("OBST/GYN"))
                {
                    Paroles.ObstGyn++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                {
                    Paroles.Amenity++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("EYE"))
                {
                    Paroles.Eye++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NURSERY/NEWBORN") || item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NBU") || item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("NEW BORN"))
                {
                    Paroles.NurseryNewBorn++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ORTHOPEDIC"))
                {
                    Paroles.Orthopaedic++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("ISOLATION"))
                {
                    Paroles.Isolation++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PSYCHIATRY"))
                {
                    Paroles.Psychiatry++;
                }
                else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                {
                    Paroles.Amenity++;
                }
                else
                {
                    Paroles.Other++;
                }




                //if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("SURGICAL"))
                //{
                //    Paroles.Surgical++;
                //}
                //else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("PAEDIATRIC"))
                //{
                //    Paroles.Paediatrics++;
                //}
                //else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("MATERNITY"))
                //{
                //    Paroles.Maternity++;
                //}
                //else if (item.HSBed.HSWard.HSWardCategory.WardCategoryName.ToUpper().Contains("AMENITY"))
                //{
                //    Paroles.Amenity++;
                //}
                //else
                //{
                //    Paroles.Other++;
                //}


            }


            List<Moh717InpatientsViewModel> moh717Inpatients = new List<Moh717InpatientsViewModel>
            {
                underFiveAdmission,
                OverFiveAdmission,
                Paroles,
                OccupiedBedDaysNHIF,
                OccupiedBedDaysCash,
                WellPersonDays,
                BedsAuthorized,
                BedsActualPhysical,
                CotsAuthorized,
                CotsActualPhysical
            };


            foreach (var item in moh717Inpatients)
            {
                dataSet.InPatients.AddInPatientsRow(
                    item.Medical,
                    item.Surgical,
                    item.ObstGyn,
                    item.Paediatrics,
                    item.Maternity,
                    item.Eye,
                    item.NurseryNewBorn,
                    item.Orthopaedic,
                    item.Other,
                    item.NameOfService,
                    item.Isolation,
                    item.Amenity,
                    item.Psychiatry

                    );
            }

            return dataSet;
        }

        public class MaternityServices
        {
            public string NameOfService { get; set; }
            public int Number { get; set; }
        }
        private DataSetMoh717MaternityOperations GetMaternityServices(DateTime FromDate, DateTime ToDate)
        {
            var deliveries = db2.BillServices.Where(e => DbFunctions.TruncateTime(e.DateAdded) >= FromDate &&
            DbFunctions.TruncateTime(e.DateAdded) <= ToDate && e.Service.Department.DepartmentName.ToUpper()
            .Contains("MATERNITY"));

            DataSetMoh717MaternityOperations dataSet = new DataSetMoh717MaternityOperations();
            
            MaternityServices MSNormalDelivery = new MaternityServices()
            {
                NameOfService = "NormalDelivery",
                Number = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("NORMAL DELIVERY"))
            };
            MaternityServices MSCaesareanSection = new MaternityServices()
            {
                NameOfService = "Caesarean Section",
                Number = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("DELIVERY CS"))
            };
            MaternityServices MSBreechDeliveries = new MaternityServices()
            {
                NameOfService = "Breech Deliveries",
                Number = 0
            };
            MaternityServices MSAssistedVaginalDelivery = new MaternityServices()
            {
                NameOfService = "Assisted Vaginal Delivery ",
                Number = 0
            };
            MaternityServices MSBBA = new MaternityServices()
            {
                NameOfService = "BBA(Born Before Arrival)",
                Number = 0
            };
            MaternityServices MSMaternalDeaths = new MaternityServices()
            {
                NameOfService = "Maternal Deaths",
                Number = 0
            };
            MaternityServices MSMaternalDeathsAudited = new MaternityServices()
            {
                NameOfService = "Maternal Deaths Audited",
                Number = 0
            };
            MaternityServices MSLiveBirths = new MaternityServices()
            {
                NameOfService = "Live Births",
                Number = 0
            };
            MaternityServices MSStillBirths = new MaternityServices()
            {
                NameOfService = "Still Births",
                Number = 0
            };
            MaternityServices MSNeonatalDeaths = new MaternityServices()
            {
                NameOfService = "Neonatal Deaths",
                Number = 0
            };
            MaternityServices MSLowBirthWeight = new MaternityServices()
            {
                NameOfService = "Low Birth Weight Babies(under 2500gms)",
                Number = 0
            };
            MaternityServices MSTotalDischarges = new MaternityServices()
            {
                NameOfService = "Total Discharges: new born",
                Number = 0
            };

            List<MaternityServices> LstMaternityServices = new List<MaternityServices>();
            LstMaternityServices.Add(MSNormalDelivery);
            LstMaternityServices.Add(MSCaesareanSection);
            LstMaternityServices.Add(MSBreechDeliveries);
            LstMaternityServices.Add(MSAssistedVaginalDelivery);
            LstMaternityServices.Add(MSBBA);
            LstMaternityServices.Add(MSMaternalDeaths);
            LstMaternityServices.Add(MSMaternalDeathsAudited);
            LstMaternityServices.Add(MSLiveBirths);
            LstMaternityServices.Add(MSStillBirths);
            LstMaternityServices.Add(MSNeonatalDeaths);
            LstMaternityServices.Add(MSLowBirthWeight);
            LstMaternityServices.Add(MSTotalDischarges);

            foreach (var item in LstMaternityServices)
            {
                dataSet.Maternity.AddMaternityRow(
                        item.NameOfService,
                        item.Number
                    );
            }


            return dataSet;
        }

        public class OperationViewModel
        {
            public string NameOfService { get; set; }
            public int NumberOfBooked { get; set; }
            public int NumberOfOperated { get; set; }
        }
        private DataSetMoh717MaternityOperations GetOperationsData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717MaternityOperations dataSet = new DataSetMoh717MaternityOperations();

            OperationViewModel modelMinorSurgeries = new OperationViewModel()
            {
                NameOfService = "Minor Surgeries(Excluding circumcision)"
            };
            OperationViewModel modelEmergencies = new OperationViewModel()
            {
                NameOfService = "Emergencies"
            };
            OperationViewModel modelColdCases = new OperationViewModel()
            {
                NameOfService = "Cold Cases"
            };
            OperationViewModel modelCircumcision = new OperationViewModel()
            {
                NameOfService = "Circumcision"
            };
            OperationViewModel modelMajorSurgeries = new OperationViewModel()
            {
                NameOfService = "MajorSurgeries"
            };

            List<OperationViewModel> lstOperations = new List<OperationViewModel>();

            lstOperations.Add(modelMinorSurgeries);
            lstOperations.Add(modelEmergencies);
            lstOperations.Add(modelColdCases);
            lstOperations.Add(modelCircumcision);
            lstOperations.Add(modelMajorSurgeries);

            foreach (var item in lstOperations)
            {
                dataSet.Operation.AddOperationRow(
                    item.NameOfService,
                    item.NumberOfBooked,
                    item.NumberOfOperated
                    );
            }
            return dataSet;
        }

        public class ModelPharmacyMortuaryMedicalViewModel
        {
            public string ServiceName { get; set; }
            public int Number { get; set; }
        }
        private DataSetPharmacyMortuaryMedicalRecord GetPharmacyDrugs(DateTime FromDate, DateTime ToDate)
        {
            DataSetPharmacyMortuaryMedicalRecord dataSet = new DataSetPharmacyMortuaryMedicalRecord();

            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = DateTime.Today;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);
            var medicationsIssued = db2.Medications.Where(p => p.TimeAdded > FirstDayOfTheMonth).ToList();



            ModelPharmacyMortuaryMedicalViewModel CommonDrugs = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Common Drugs"

            };
            ModelPharmacyMortuaryMedicalViewModel Antibiotics = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Anti Biotics"

            };

            ModelPharmacyMortuaryMedicalViewModel SpecialDrugs = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Special Drugs"

            };

            ModelPharmacyMortuaryMedicalViewModel DrugsForChildren = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Drugs For Children"

            };

            List<ModelPharmacyMortuaryMedicalViewModel> lstPharmacy = new List<ModelPharmacyMortuaryMedicalViewModel>();


            var drugTransactions = db.DrugTransactions
                                     .Where(p => DbFunctions.TruncateTime(p.TransactionDate) >= FromDate && DbFunctions.TruncateTime(p.TransactionDate) <= ToDate)

                                     .ToList();

            var allDrugs = db.Drug.ToList();
            var itemMasters = db.ItemMaster.ToList();



            foreach (var item in drugTransactions)
            {
                var theDrug = itemMasters.Where(p => p.Id == item.ItemMasterId).FirstOrDefault();

                if (theDrug != null)
                {
                    if (theDrug?.Drug?.Category != null)
                    {
                        if (theDrug.Drug.Category.CategoryName.Trim().ToLower().Contains("common"))
                        {
                            CommonDrugs.Number++;
                        }
                        else if (theDrug.Drug.Category.CategoryName.Trim().ToLower().Contains("antibiotics") || theDrug.Drug.Category.CategoryName.Trim().ToLower().Contains("antibiotics") || theDrug.Drug.Category.CategoryName.Trim().ToLower().Contains("anti-biotics"))
                        {
                            Antibiotics.Number++;
                        }
                        else if (theDrug.Drug.Category.CategoryName.Trim().ToLower().Contains("special"))
                        {
                            SpecialDrugs.Number++;
                        }
                        else if (theDrug.Drug.Category.CategoryName.Trim().ToLower().Contains("children"))
                        {
                            DrugsForChildren.Number++;
                        }
                    }
                    else
                    {
                        CommonDrugs.Number++;
                    }

                }
            }


            lstPharmacy.Add(CommonDrugs);
            lstPharmacy.Add(Antibiotics);
            lstPharmacy.Add(SpecialDrugs);
            lstPharmacy.Add(DrugsForChildren);

            foreach (var item in lstPharmacy)
            {
                dataSet.Pharmacy.AddPharmacyRow(
                    item.ServiceName,
                    item.Number);
            }


            return dataSet;
        }

        private DataSetPharmacyMortuaryMedicalRecord GetMortuaryData(DateTime FromDate, DateTime ToDate)
        {
            DataSetPharmacyMortuaryMedicalRecord dataSet = new DataSetPharmacyMortuaryMedicalRecord();

            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = DateTime.Today;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);


            ModelPharmacyMortuaryMedicalViewModel BodyDays = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Body Days"

            };
            ModelPharmacyMortuaryMedicalViewModel Embalment = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Embalment"

            };

            ModelPharmacyMortuaryMedicalViewModel PostMoterm = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Post-morterm"

            };

            ModelPharmacyMortuaryMedicalViewModel UnclaimedBodies = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Unclaimed Bodies"

            };





            List<ModelPharmacyMortuaryMedicalViewModel> lstMortuary = new List<ModelPharmacyMortuaryMedicalViewModel>();

            lstMortuary.Add(BodyDays);
            lstMortuary.Add(Embalment);
            lstMortuary.Add(PostMoterm);
            lstMortuary.Add(UnclaimedBodies);

            foreach (var item in lstMortuary)
            {
                dataSet.Mortuary.AddMortuaryRow(
                    item.ServiceName,
                    item.Number);
            }


            return dataSet;
        }

        private DataSetPharmacyMortuaryMedicalRecord GetMedicalRecordsIssuedData(DateTime FromDate, DateTime ToDate)
        {
            DataSetPharmacyMortuaryMedicalRecord dataSet = new DataSetPharmacyMortuaryMedicalRecord();

            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = DateTime.Today;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);
            var medicationsIssued = db2.Medications.Where(p => DbFunctions.TruncateTime(p.TimeAdded) > FirstDayOfTheMonth).ToList();

            ModelPharmacyMortuaryMedicalViewModel NewFiles = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "New Files/ Folders"

            };
            ModelPharmacyMortuaryMedicalViewModel OutpatientCards = new ModelPharmacyMortuaryMedicalViewModel()
            {
                ServiceName = "Out patient cards"

            };
            List<ModelPharmacyMortuaryMedicalViewModel> lstMedicalRecords = new List<ModelPharmacyMortuaryMedicalViewModel>();

            var allPatients = db2.Patients.Where(p => p.RegDate.Value >= FromDate && p.RegDate.Value <= FromDate)
                .ToList();

            var opdReg = db2.OpdRegisters.Where(p => DbFunctions.TruncateTime(p.TimeAdded.Value) >= FromDate && 
            DbFunctions.TruncateTime(p.TimeAdded.Value) <= ToDate).ToList();

            NewFiles.Number = allPatients.Count(p => p.OpdRegisters.Any(f => f.IsIPD));
            OutpatientCards.Number = opdReg.Count(p => p.Patient.OpdRegisters.Count() == 1);

            lstMedicalRecords.Add(NewFiles);
            lstMedicalRecords.Add(OutpatientCards);

            foreach (var item in lstMedicalRecords)
            {
                dataSet.MedicalRecordIssued.AddMedicalRecordIssuedRow(
                    item.ServiceName,
                    item.Number);
            }

            return dataSet;
        }

        private class FinanceViewModel
        {
            public double AvailableFinancing { get; set; }
            public int ClientsWaivered { get; set; }
            public double TotalWaivered { get; set; }
            public int ClientsExempted { get; set; }
            public double TotalExempted { get; set; }
        }
        private DataSetMoh717Finance GetFinanceReportData(DateTime FromDate, DateTime ToDate)
        {
            DataSetMoh717Finance dataSet = new DataSetMoh717Finance();
            //var today = FromDate;
            //var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);
            //var waivers2 = db2.Waivers.ToList();

            var fDate = FromDate.Date;
            var tDate = ToDate.Date;

            var waivers = db2.Waivers.Where(p => p.DateAdded >= fDate && p.DateAdded <= tDate)
                             .ToList();

            var exempted = db2.OpdRegisters
                              .Where(p => p.Tariff.Company.CompanyType.CompanyTypeName.Trim().ToLower().Contains("EXEMPTION"))
                              .ToList();

            var billServices = exempted.Select(p => p.BillServices).ToList();



            FinanceViewModel viewModel = new FinanceViewModel()
            {
                ClientsWaivered = waivers.DistinctBy(p => p.OPDIPDNo).Count(),
                TotalWaivered = waivers.Sum(p => p.AmountWaived),
                ClientsExempted = exempted.DistinctBy(p => p.PatientId).Count(),
                TotalExempted = exempted.Sum(p => p.BillServices.Sum(x => x.Price * x.Quatity))
            };

            foreach (var item in billServices)
            {
                viewModel.TotalExempted += item.Sum(p => p.Price * p.Quatity);
            }


            dataSet.Finance.AddFinanceRow(
                0, viewModel.ClientsWaivered, viewModel.TotalWaivered, viewModel.ClientsExempted, viewModel.TotalExempted
                );

            return dataSet;
        }

        private class SpecialServicesViewModel
        {
            public int NoXrayPlainWithEnhancement { get; set; }
            public int PhysiotherapyPrivate { get; set; }
            public int OccupationalTherapyPrivate { get; set; }
            public int OrthopaedicTechnologyPrivatePnI { get; set; }
            public int OrthopaedicTechnologyPrivateI { get; set; }
            public int NoXrayEnhancementWithContrastMedia { get; set; }
            public int NoSpecialMagneticProcessUltraSound { get; set; }
            public int NoSpecialWithMagneticMRICTscan { get; set; }
            public int NoTotalRadiologicalExaminations { get; set; }
            public int PhyisiotherapyNonPrivate { get; set; }
            public int OccupationalTherapyNonPrivate { get; set; }
            public int OrthopaedicTechnologyNonPrivatePnS { get; set; }
            public int OrthopaedicTechnologyNonPrivateP { get; set; }
        }
        private DataSetSpecialServices GetSpecialServices(DateTime FromDate, DateTime ToDate)
        {
            DataSetSpecialServices dataSet = new DataSetSpecialServices();

            SpecialServicesViewModel ViewModel = new SpecialServicesViewModel();

            var labaratoryTests = db3.LabTestForMOH505Report.Where(p => p.DepartmentName.Equals("radiology") && p.CreatedUtc.Value >= FromDate && p.CreatedUtc.Value <= ToDate).ToList();

            var opdRegisters = db2.OpdRegisters
                                .Where(p => p.Date >= FromDate && p.Date <= ToDate)
                                .ToList();

            ViewModel.NoXrayPlainWithEnhancement = labaratoryTests.Where(p => p.LabTest.Trim().ToLower().Contains("x ray")).Count();
            ViewModel.NoSpecialMagneticProcessUltraSound = labaratoryTests.Where(p => p.LabTest.Trim().ToLower().Contains("ultra sound")).Count();
            ViewModel.NoSpecialWithMagneticMRICTscan = labaratoryTests.Where(p => p.LabTest.Trim()
            .ToLower().Contains("mri") || p.LabTest.Trim()
            .ToLower().Contains("ct scan")).Count();

            foreach (var item in opdRegisters)
            {
                if (item.Department == "OCCUPATIONAL THERAPY")
                {
                    ViewModel.OccupationalTherapyNonPrivate++;
                }

                if (item.Department == "PHYSIOTHERAPY")
                {
                    ViewModel.PhyisiotherapyNonPrivate++;
                }

                if (item.Department == "ORTHOPAEDIC TECHNOLOGY")
                {
                    ViewModel.OrthopaedicTechnologyNonPrivateP = item.BillServices.Where(p => p.ServiceName == "Issued").Count();
                    ViewModel.OrthopaedicTechnologyNonPrivatePnS = item.BillServices.Where(p => p.ServiceName == "Prepared and Issued").Count();
                }
            }

            dataSet.SpecialServices.AddSpecialServicesRow(
                ViewModel.NoXrayPlainWithEnhancement,
                ViewModel.PhysiotherapyPrivate,
                ViewModel.OccupationalTherapyPrivate,
                ViewModel.OrthopaedicTechnologyPrivatePnI,
                ViewModel.OrthopaedicTechnologyPrivateI,
                ViewModel.NoXrayEnhancementWithContrastMedia,
                ViewModel.NoSpecialMagneticProcessUltraSound,
                ViewModel.NoSpecialWithMagneticMRICTscan,
                ViewModel.NoTotalRadiologicalExaminations,
                ViewModel.PhyisiotherapyNonPrivate,
                ViewModel.OccupationalTherapyNonPrivate,
                ViewModel.OrthopaedicTechnologyNonPrivatePnS,
                ViewModel.OrthopaedicTechnologyNonPrivateP
                );

            return dataSet;
        }

        private class A6ToA12ViewModel
        {
            public int TotalOutPatientServicesNew { get; set; }
            public int TotalOutPatientServicesRe { get; set; }
            public int TotalOutPatientServicesAll { get; set; }
            public int MedicalExaminations { get; set; }
            public int MedicalReports { get; set; }
            public int Dressings { get; set; }
            public int RemovalOfStiches { get; set; }
            public int Injections { get; set; }
            public int Stitching { get; set; }
            public int PlasterOfParis { get; set; }
        }
        private DataSetA6ToA12 GetA6ToA12Data(DateTime FromDate, DateTime ToDate)
        {
            DataSetA6ToA12 dataSet = new DataSetA6ToA12();

            A6ToA12ViewModel a6ToA12ViewModel = new A6ToA12ViewModel();

            var LastMonth = DateTime.Now.AddMonths(-1);
            var today = FromDate;
            var FirstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);

            var opdRegisters = db2.OpdRegisters
                                .Where(p => DbFunctions.TruncateTime(p.Date) >= FromDate && DbFunctions.TruncateTime(p.Date) <= ToDate && p.IsIPD == false)
                                .ToList();



            foreach (var item in opdRegisters)
            {
                if (item.Patient.RegDate.HasValue)
                {
                    //if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("INJECTION")))
                    //{
                    //    a6ToA12ViewModel.Injections++;
                    //}

                    foreach (var meds in item.Medications)
                    {

                        if (meds.RoutingAdmin.ToLower().Equals("iv"))
                        {
                            a6ToA12ViewModel.Injections = a6ToA12ViewModel.Injections + meds.QuantityIssued ?? 0;
                        }
                    }
                    //if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("INJECTION")))
                    //{
                    //    a6ToA12ViewModel.Injections++;
                    //}

                    if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("STITCHING")))
                    {
                        a6ToA12ViewModel.Stitching++;
                    }
                    if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("PLASTER")))
                    {
                        a6ToA12ViewModel.PlasterOfParis++;
                    }
                    if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("MEDICAL EXAMINATION")))
                    {
                        a6ToA12ViewModel.MedicalExaminations++;
                    }
                    if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("MEDICAL REPORTS")) || item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("P3")) || item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("MEDICAL EXAMS")))
                    {
                        a6ToA12ViewModel.MedicalReports++;
                    }
                    if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("DRESSING")))
                    {
                        a6ToA12ViewModel.Dressings++;
                    }
                    if (item.BillServices.Any(p => p.ServiceName.Trim().ToUpper().Contains("REMOVAL OF STITCHES")))
                    {
                        a6ToA12ViewModel.RemovalOfStiches++;
                    }
                }
            }

            dataSet.A6ToA12.AddA6ToA12Row(
                 a6ToA12ViewModel.TotalOutPatientServicesNew,
                 a6ToA12ViewModel.TotalOutPatientServicesRe,
                 a6ToA12ViewModel.MedicalExaminations,
                 a6ToA12ViewModel.Dressings,
                 a6ToA12ViewModel.RemovalOfStiches,
                 a6ToA12ViewModel.Injections,
                 a6ToA12ViewModel.Stitching,
                 a6ToA12ViewModel.PlasterOfParis,
                 a6ToA12ViewModel.MedicalReports
                  );

            return dataSet;
        }
        #endregion
        #endregion

        #region MOH Monthly Cash Analysis
        public ActionResult MOHMonthlyAnalysis()
        {
            return View();
        }

        public ActionResult GetMOHMonthlyAnalysisReport(int Year, int Month)
        {
            ReportDocument report = new ReportDocument();
            report.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOHMonthlyCash/RptMonthlyCashAnalysis.rpt")));
            var dataSet = GetMOHMonthlyAnalysisData(Year, Month);
            report.SetDataSource(dataSet);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            report.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            report.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());

            //report.Subreports["RptCurrentStock.rpt"].SetDataSource(dataSet);
            Stream str = report.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);

            str.Seek(0, SeekOrigin.Begin);

            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string name = "MOH Monthly Cash Analyis" + Timestamp + ".pdf";


            return File(str, "application/pdf", name);
        }

        public DataSetMohMonthlyCashAnalysis GetMOHMonthlyAnalysisData(int Year, int Month)
        {
            DataSetMohMonthlyCashAnalysis dataSet = new DataSetMohMonthlyCashAnalysis();

            var lstViewModel = new List<MonthlyCashAnalysisViewModel>();

            //get the number of days in the month
            var numberOfDaysInTheMonth = DateTime.DaysInMonth(Year, Month);
            var firstDayOfTheMonth = new DateTime(Year, Month, 1);
            var LastDayOfTheMonth = new DateTime(Year, Month, numberOfDaysInTheMonth);
            var selectedBillServices = db2.BillServices.Where(p => DbFunctions.TruncateTime(p.DateAdded) >= firstDayOfTheMonth && DbFunctions.TruncateTime(p.DateAdded) <= LastDayOfTheMonth).ToList();
            var allDepartments = db2.Departments.ToList();

            var cashCollectionBillServices = selectedBillServices.Where(p => p.OpdRegister.Tariff.Company.CompanyName.Contains("cash")).ToList();

            for (int i = 1; i <= numberOfDaysInTheMonth; i++)
            {
                MonthlyCashAnalysisViewModel viewModel = new MonthlyCashAnalysisViewModel()
                {
                    breakDownByDepartement = new BreakDownByDepartement(),
                    cashReceipts = new CashReceipts(),
                    revenueNotCollected = new RevenueNotCollected(),
                    banking = new Banking(),
                    category = new CategoryMonthlyCashAnalysis()
                };

                foreach (var dept in allDepartments)
                {
                    if (dept.DepartmentName.ToLower().Contains("lab"))
                    {
                        viewModel.breakDownByDepartement.Lab = selectedBillServices.Where(p => p.DepartmentId == dept.Id && p.Paid == true && p.DateAdded.Day == i).Sum(x => (x.Quatity * x.Price));
                    }
                }

                lstViewModel.Add(viewModel);

            }

            int theIdValue = 1;
            foreach (var item in lstViewModel)
            {
                dataSet.BreakDownByDepartment.AddBreakDownByDepartmentRow(
                    theIdValue,
                    0,
                    0,
                    0,
                    item.breakDownByDepartement.Lab,
                    0,
                    0,
                    0,
                    0,
                    0);

                dataSet.CashReciepts.AddCashRecieptsRow(
                    theIdValue, 0, 0, 0);
                dataSet.RevenueNotCollected.AddRevenueNotCollectedRow(theIdValue, 0, 0, 0, 0, 0);
                dataSet.Banking.AddBankingRow(theIdValue, 0, 0);
                dataSet.MonthlyBreakDownOfOtherIncome.AddMonthlyBreakDownOfOtherIncomeRow(theIdValue, "Nothing", 0);
            }

            return dataSet;
        }

        #endregion 

        #region MOH 706

        public ActionResult MOH706()
        {
            return View();
        }

        public ActionResult GenerateMOH706Report(DateTime FromDate, DateTime ToDate)
        {

            var dataSetUrineAnalysis = GetUrineAnalysisData(FromDate, ToDate);

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH706/RptMOH706.rpt")));
            //Rd.SetDataSource(dataSetMOH705A);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Rd.Subreports["RptUrineAnalysis.rpt"].SetDataSource(dataSetUrineAnalysis);


            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "MOH 706 " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }

        public class UrineAnalysisViewModel
        {
            public int GlucoseTotal { get; set; }
            public int KetonesTotal { get; set; }
            public int ProteinsTotal { get; set; }
            public int PusCellsTotal { get; set; }
            public int SHaemotobiumTotal { get; set; }
            public int TVaginalisTotal { get; set; }
            public int YeastCellsTotal { get; set; }
            public int BacteriaTotal { get; set; }

            public int GlucosePositive { get; set; }
            public int KetonesPositive { get; set; }
            public int ProteinsPositive { get; set; }
            public int PusCellsPositive { get; set; }
            public int SHaemotobiumPositive { get; set; }
            public int TVaginalisPositive { get; set; }
            public int YeastCellsPositive { get; set; }
            public int BacteriaPositive { get; set; }
        }
        private DataSetUrineAnalysis GetUrineAnalysisData(DateTime FromDate, DateTime ToDate)
        {
            DataSetUrineAnalysis dataSet = new DataSetUrineAnalysis();

            UrineAnalysisViewModel viewModel = new UrineAnalysisViewModel();

            var selectedLabTests = db3.WorkOrderTests.Where(p => p.CreatedUtc.Value > FromDate && p.CreatedUtc.Value <= ToDate && p.LabTest.Test.Trim().ToUpper() == "URINALYSIS").ToList();

            foreach (var patientTest in selectedLabTests)
            {
                var patientResults = patientTest.WorkOrderTestParameters.ToList();

                foreach (var item in patientResults)
                {
                    var glucoseTested = item.Parameter1.Parameter_Name.ToLower().Contains("glucose");
                    if (glucoseTested)
                    {
                        viewModel.GlucoseTotal++;

                        var glucosePositive = item.Results;

                        if (glucosePositive.Contains("+") || glucosePositive.Contains("++") || glucosePositive.Contains("pos"))
                        {
                            viewModel.GlucosePositive++;
                        }
                    }

                    var KetonTested = item.Parameter1.Parameter_Name.ToLower().Contains("ketones");
                    if (KetonTested)
                    {
                        viewModel.KetonesTotal++;

                        var KetonesPositive = item.Results;

                        if (KetonesPositive.Contains("+") || KetonesPositive.Contains("++") || KetonesPositive.Contains("pos"))
                        {
                            viewModel.KetonesPositive++;
                        }
                    }

                    var ProteinTested = item.Parameter1.Parameter_Name.ToLower().Contains("protein");
                    if (ProteinTested)
                    {
                        viewModel.ProteinsTotal++;

                        var ProteinPositive = item.Results;

                        if (ProteinPositive.Contains("+") || ProteinPositive.Contains("++") || ProteinPositive.Contains("pos"))
                        {
                            viewModel.ProteinsPositive++;
                        }
                    }

                    var PusCellsTested = item.Parameter1.Parameter_Name.ToLower().Contains("pus cells");
                    if (PusCellsTested)
                    {
                        viewModel.PusCellsTotal++;

                        var PusCellsPositive = item.Results;

                        if (PusCellsPositive.Contains("+") || PusCellsPositive.Contains("++") || PusCellsPositive.Contains("pos"))
                        {
                            viewModel.PusCellsPositive++;
                        }
                    }

                    var SHaemoglobinTested = item.Parameter1.Parameter_Name.ToLower().Contains("haemoglobin");

                    if (SHaemoglobinTested)
                    {
                        viewModel.SHaemotobiumTotal++;

                        var SHaemotobiumPositive = item.Results;

                        if (SHaemotobiumPositive.Contains("+") || SHaemotobiumPositive.Contains("++") || SHaemotobiumPositive.Contains("pos"))
                        {
                            viewModel.SHaemotobiumPositive++;
                        }
                    }

                    var TVaginalisTested = item.Parameter1.Parameter_Name.ToLower().Contains("tricomonas vaginalis");

                    if (TVaginalisTested)
                    {
                        viewModel.TVaginalisTotal++;

                        var TVaginalisPositive = item.Results;

                        if (TVaginalisPositive.Contains("+") || TVaginalisPositive.Contains("++") || TVaginalisPositive.Contains("pos"))
                        {
                            viewModel.TVaginalisPositive++;
                        }
                    }

                    var YeastCellsTested = item.Parameter1.Parameter_Name.ToLower().Contains("yeast cells");

                    if (YeastCellsTested)
                    {
                        viewModel.YeastCellsTotal++;

                        var YeastCellsTestedPositive = item.Results;

                        if (YeastCellsTestedPositive.Contains("+") || YeastCellsTestedPositive.Contains("++") || YeastCellsTestedPositive.Contains("pos"))
                        {
                            viewModel.YeastCellsPositive++;
                        }
                    }


                    var BacteriaTested = item.Parameter1.Parameter_Name.ToLower().Contains("bacteria");

                    if (BacteriaTested)
                    {
                        viewModel.BacteriaTotal++;

                        var BacteriaPositive = item.Results;

                        if (BacteriaPositive.Contains("+") || BacteriaPositive.Contains("++") || BacteriaPositive.Contains("pos"))
                        {
                            viewModel.BacteriaPositive++;
                        }
                    }
                }
            }

            dataSet.UrineAnalysis.AddUrineAnalysisRow(
               viewModel.GlucoseTotal,
               viewModel.KetonesTotal,
               viewModel.ProteinsTotal,
               viewModel.PusCellsTotal,
               viewModel.SHaemotobiumTotal,
               viewModel.TVaginalisTotal,
               viewModel.YeastCellsTotal,
               viewModel.BacteriaTotal,
               viewModel.GlucosePositive,
               viewModel.KetonesPositive,
               viewModel.ProteinsPositive,
               viewModel.PusCellsPositive,
               viewModel.SHaemotobiumPositive,
               viewModel.TVaginalisPositive,
               viewModel.BacteriaPositive,
               viewModel.YeastCellsPositive
               );

            return dataSet;
        }
        public class BloodChemistryViewModel
        {
            public int BloodSugar { get; set; }
            public int Ogtt { get; set; }
            public int Creatinine { get; set; }
            public int Urea { get; set; }
            public int Sodium { get; set; }
            public int Potassium { get; set; }
            public int Chlorides { get; set; }
            public int DirectBilirubin { get; set; }
            public int TotalBilirubin { get; set; }
            public int Asat_sgot { get; set; }
            public int Alat_sgpt { get; set; }
            public int Serum_protein { get; set; }
            public int Albumin { get; set; }
            public int Alkaline_Phosphate { get; set; }
            public int TotalCholestral { get; set; }
            public int Triglycerides { get; set; }
            public int Ldl { get; set; }
            public int T3 { get; set; }
            public int T4 { get; set; }
            public int Tsh { get; set; }
            public int Psa { get; set; }
            public int Cea { get; set; }
            public int C15_3 { get; set; }
            public int Proteins { get; set; }
            public int Glucose { get; set; }

        }

        public DataSetBloodChemistry GetBloodChemistryData(DateTime FromDate, DateTime ToDate)
        {
            DataSetBloodChemistry dataSet = new DataSetBloodChemistry();
            var LowViewModel = new BloodChemistryViewModel();
            var HighViewModel = new BloodChemistryViewModel();
            var TotalViewModel = new BloodChemistryViewModel();



            return dataSet;
        }

        #endregion

        #region Users and their Roles 

        public ActionResult UserRolesAndRights()
        {
            return View();
        }

        public ActionResult GetUserRolesAndRightsReport()
        {
            string Format = "PDF";
            var dataSet = GetUserRolesAndRightsData();

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/UserRolesAndRights/RptUserRolesAndRights.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            if (Format != "PDF")
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "User and Roles Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .xlsx";

                return File(Stream, "application/xlsx", FileName);
            }
            else
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "User and Roles Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

                return File(Stream, "application/pdf", FileName);
            }
        }

        private class UserModel
        {
            public string EmployeeNumber { get; set; }
            public string UserName { get; set; }
            public string Role { get; set; }
            public string roleRights { get; set; }
            public string FullNames { get; set; }
            public string Department { get; set; }

        }

        public DataSetUserRolesAndRights GetUserRolesAndRightsData()
        {
            DataSetUserRolesAndRights dataSet = new DataSetUserRolesAndRights();

            List<UserModel> lstUserModel = new List<UserModel>();

            var AllUsers = db2.Users.ToList();
            var AllGroupRights = db2.GroupRights.ToList();

            foreach (var item in AllUsers)
            {
                var employee = item.Employee;
                UserModel userModel = new UserModel()
                {
                    EmployeeNumber = item.EmployeeId.ToString(),
                    UserName = item.Username,
                    Role = item.UserRole.RoleName,
                    Department = item.Employee.Department.DepartmentName ?? "",
                    FullNames = employee.Salutation + " " + employee.FName + " " + employee.OtherName
                };

                if (item.UserRole.RoleName == "SA")
                {
                    userModel.roleRights = "Has Access to All Modules";
                }
                else
                {
                    var groupRight = AllGroupRights.Where(p => p.UserRoleId == item.UserRoleId).ToList();

                    string roleRightName = "";

                    foreach (var itm in groupRight)
                    {
                        roleRightName += itm.RoleRight.RoleRightName + ", ";
                    }

                    userModel.roleRights = roleRightName;
                }

                if (userModel.roleRights == "")
                {
                    userModel.roleRights = "No Access to any Module";
                }


                lstUserModel.Add(userModel);
            }




            foreach (var dat in lstUserModel)
            {
                dataSet.UsersAndRights.AddUsersAndRightsRow(
                    dat.EmployeeNumber,
                    dat.UserName,
                    dat.Role,
                    dat.roleRights.TrimEnd(new Char[] { ',' }),
                    dat.FullNames,
                    dat.Department);
            }


            return dataSet;
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
        #endregion



    }
}