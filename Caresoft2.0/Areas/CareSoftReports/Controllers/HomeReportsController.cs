using Caresoft2._0.Areas.CareSoftReports.Reports.MOH505;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH705A;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH706;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH710;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH711.B;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH711.B1;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH711.B2;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH711.J;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH717;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.AdditionalInfor;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.Anaemia;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.ARTAndCoexistingInfections;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.ClientsServed;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.InfantFeedingPractices;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.NutritionIntervention;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOH733B.SAMMAM;
using Caresoft2._0.Areas.CareSoftReports.Reports.MOHMonthlyCash;
using Caresoft2._0.Areas.CareSoftReports.Reports.UserRolesAndRights;
using Caresoft2._0.Areas.CareSoftReports.ViewModels;
using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.CrystalReports;
using Caresoft2._0.CrystalReports.MOH710.ChildImmunization;
using Caresoft2._0.CrystalReports.MOH711.A;
using Caresoft2._0.CrystalReports.MOH711.C;
using Caresoft2._0.CrystalReports.MOH711.D;
using Caresoft2._0.CrystalReports.MOH711.E;
using Caresoft2._0.CrystalReports.MOH711.F1;
using Caresoft2._0.CrystalReports.MOH711.H;
using Caresoft2._0.CrystalReports.MOH711.I;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i ;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i ;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i ;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i ;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i ;
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

            for (int i = 1; i < NumberOfDays; i++)
            {
                var dayOfMonth = i;
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

        #endregion bb

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
            public int Total { get; internal set; }
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

        #region MOH 733B Report
        public ActionResult MOH733B()
        {
            return View();
        }
        public ActionResult GenerateMOH733BReport(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            var NewClientsServed = GetNewClientsServedData(FromDate, ToDate);
            var NewSAM = GetNewSAMData(FromDate, ToDate);
            var NewART = GetNewARTData(FromDate, ToDate);
            var NewAnaemia = GetNewAnaemiaData(FromDate, ToDate);
            var NewInfant = GetNewInfantData(FromDate, ToDate);
            var NIntervention = GetNInterventionData(FromDate, ToDate);
            var NewAdditionalInfor = GetNewAdditionalInforData(FromDate, ToDate);


            Reports.MOH733B.NewHospitalDetails newHospitalDetails = new Reports.MOH733B.NewHospitalDetails();

            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            int session = Convert.ToInt32(Session["UserId"]);
            var userLoggedIn = db2.Users.Where(p => p.Id == session).FirstOrDefault();
            newHospitalDetails.NewHDetails.AddNewHDetailsRow("",
                                                             "",
                                                             HospitalName,
                                                             ToDate.ToShortMonthName(),
                                                             toDate.Year.ToString(),
                                                             "",
                                                             "");

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH733B/MainReport.rpt")));
            Rd.SetDataSource(newHospitalDetails);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Rd.Subreports["ClientsServed.rpt"].SetDataSource(NewClientsServed);
            Rd.Subreports["SAMMAM.rpt"].SetDataSource(NewSAM);
            Rd.Subreports["ARTAndCoexisting.rpt"].SetDataSource(NewART);
            Rd.Subreports["Anaemia.rpt"].SetDataSource(NewAnaemia);
            Rd.Subreports["InfantFeeding.rpt"].SetDataSource(NewInfant);
            Rd.Subreports["NutritionIntervention.rpt"].SetDataSource(NIntervention);
            Rd.Subreports["AdditionalInfor.rpt"].SetDataSource(NewAdditionalInfor);


            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "MOH 733B " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }


        private object GetNewClientsServedData(DateTime FromDate, DateTime ToDate)
        {
            NewClientsServed newClientsServed = new NewClientsServed();
            var A = db2.NutritionAdultRegisters.Where(p => DbFunctions.TruncateTime(p.date_time) >= FromDate.Date && DbFunctions.TruncateTime(p.date_time) <= ToDate.Date).Count();
            var C = db2.NutritionChildRegisters.Where(p => DbFunctions.TruncateTime(p.date_time) >= FromDate.Date && DbFunctions.TruncateTime(p.date_time) <= ToDate.Date).Count();
            var NMPAdult = db2.NutritionAdultRegisters.Count(e => e.gender == "M" && e.age > 15 && e.ServicePoint=="CCC" && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFPAdult = db2.NutritionAdultRegisters.Count(e => e.gender == "F" && e.age > 15 && e.ServicePoint == "CCC" && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMP15To17 = db2.NutritionAdultRegisters.Count(e => e.gender == "M" && e.age >=15-17 && e.ServicePoint == "CCC" && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFP15To17 = db2.NutritionAdultRegisters.Count(e => e.gender == "F" && e.age >= 15 - 17 && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMPPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.gender == "M" && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFPPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.gender == "F" && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMP0To59M = db2.NutritionChildRegisters.Count(e => e.gender == "M" && e.age <= 5 && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFP0To59M = db2.NutritionChildRegisters.Count(e => e.gender == "F" && e.age <= 5 && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMP5To15 = db2.NutritionChildRegisters.Count(e => e.gender == "M" && e.age >= 5 - 15 && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFP5To15 = db2.NutritionChildRegisters.Count(e => e.gender == "F" && e.age >= 5 - 15 && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NPReadmission = db2.NutritionAdultRegisters.Count(e =>e.SamMamClients == "Readmission" && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NPRelapse = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Relapse" && e.ServicePoint == "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NPLinkedOVC = db2.NutritionAdultRegisters.Count(e =>e.LinkedOVC=="Yes" && e.ServicePoint == "CCC" && e.VisitType == "New");
            var NMNAdult = db2.NutritionAdultRegisters.Count(e => e.gender == "M" && e.age > 15 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFNAdult = db2.NutritionAdultRegisters.Count(e => e.gender == "F" && e.age > 15 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMN15To17 = db2.NutritionAdultRegisters.Count(e => e.gender == "M" && e.age >= 15 - 17 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFN15To17 = db2.NutritionAdultRegisters.Count(e => e.gender == "F" && e.age >= 15 - 17 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMNPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.gender == "M" && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFNPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.gender == "F" && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMN0To59M = db2.NutritionChildRegisters.Count(e => e.gender == "M" && e.age <= 5 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFN0To59M = db2.NutritionChildRegisters.Count(e => e.gender == "F" && e.age <= 5 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMN5To15 = db2.NutritionChildRegisters.Count(e => e.gender == "M" && e.age >= 5 - 15 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NFN5To15 = db2.NutritionChildRegisters.Count(e => e.gender == "F" && e.age >= 5 - 15 && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NNReadmission = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Readmission" && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NNRelapse = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Relapse" && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NNLinkedOVC = db2.NutritionAdultRegisters.Count(e => e.LinkedOVC == "Yes" && e.ServicePoint != "CCC" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RPAdult = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RP15To17 = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RPPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RP0To59M = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RP5To15 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RPReadmission = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Readmission" && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RPRelapse = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Relapse" && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RPLinkedOVC = db2.NutritionAdultRegisters.Count(e => e.LinkedOVC == "Yes" && e.ServicePoint == "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RNAdult = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RN15To17 = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RNPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RN0To59M = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RN5To15 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RNReadmission = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Readmission" && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RNRelapse = db2.NutritionAdultRegisters.Count(e => e.SamMamClients == "Relapse" && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RNLinkedOVC = db2.NutritionAdultRegisters.Count(e => e.LinkedOVC == "Yes" && e.ServicePoint != "CCC" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);


            newClientsServed._NewClientsServed.AddNewClientsServedRow(
                NMPAdult,
                NFPAdult, 
                NMP15To17,
                NFP15To17, 
                NMPPost, 
                NFPPost, 
                NMP0To59M, 
                NFP0To59M, 
                NMP5To15, 
                NFP5To15, 
                NPReadmission, 
                NPRelapse, 
                NPLinkedOVC,
                NMNAdult,
                NFNAdult,
                NMN15To17,
                NFN15To17,
                NMNPost,
                NFNPost,
                NMN0To59M,
                NFN0To59M,
                NMN5To15,
                NFN5To15,
                NNReadmission,
                NNRelapse,
                NNLinkedOVC,
                RPAdult,
                RP15To17,
                RPPost,
                RP0To59M,
                RP5To15,
                RPReadmission,
                RPRelapse,
                RPLinkedOVC,
                RNAdult,
                RN15To17,
                RNPost,
                RN0To59M,
                RN5To15,
                RNReadmission,
                RNRelapse,
                RNLinkedOVC);
            return newClientsServed;
        }
        private object GetNewSAMData(DateTime FromDate, DateTime ToDate)
        {
            NewSAM newSAM = new NewSAM();

            var PSAMAdult = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ServicePoint == "CCC" && e.NutritionStatus == "SAM"  && e.date_time >= FromDate && e.date_time <= ToDate);
            var PMAMAdult = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ServicePoint == "CCC" && e.NutritionStatus == "MAM" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PSAM15To17 = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ServicePoint == "CCC" && e.NutritionStatus == "SAM" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PMAM15To17 = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ServicePoint == "CCC" && e.NutritionStatus == "SAM" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PSAMPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.NutritionStatus=="SAM" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PMAMPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.NutritionStatus == "MAM" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PSAM0To59M = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ServicePoint == "CCC" && e.NutritionStatus == "SAM" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PMAM0To59M = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ServicePoint == "CCC" && e.NutritionStatus == "MAM" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PSAM5To15 = db2.NutritionChildRegisters.Count(e => e.NutritionStatus == "SAM" && e.age >= 5 - 15 && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var PMAM5To15 = db2.NutritionChildRegisters.Count(e => e.NutritionStatus == "SAM" && e.age >= 5 - 15 && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NSAMAdult = db2.NutritionAdultRegisters.Count(e => e.NutritionStatus == "SAM" && e.age > 15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMAMAdult = db2.NutritionAdultRegisters.Count(e => e.NutritionStatus == "SAM" && e.age > 15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NSAM15To17 = db2.NutritionAdultRegisters.Count(e => e.NutritionStatus == "SAM" && e.age >= 15 - 17 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMAM15To17 = db2.NutritionAdultRegisters.Count(e => e.NutritionStatus == "MAM" && e.age >= 15 - 17 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NSAMPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.NutritionStatus == "SAM" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMAMPost = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.NutritionStatus == "MAM" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NSAM0To59 = db2.NutritionChildRegisters.Count(e => e.NutritionStatus == "SAM" && e.age <= 5 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMAM0To59 = db2.NutritionChildRegisters.Count(e => e.NutritionStatus == "MAM" && e.age <= 5 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NSAM5To15 = db2.NutritionAdultRegisters.Count(e => e.NutritionStatus == "SAM" && e.age >= 5 - 15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NMAM5To15 = db2.NutritionAdultRegisters.Count(e => e.NutritionStatus == "MAM" && e.age >= 5 - 15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            newSAM._NewSAM.AddNewSAMRow(
                PSAMAdult,
                PMAMAdult,
                PSAM15To17,
                PMAM15To17,
                PSAMPost,
                PMAMPost,
                PSAM0To59M,
                PMAM0To59M,
                PSAM5To15,
                PMAM5To15,
                NSAMAdult,
                NMAMAdult,
                NSAM15To17, 
                NMAM15To17, 
                NSAMPost,
                NMAMPost,
                NSAM0To59,
                NMAM0To59,
                NSAM5To15, 
                NMAM5To15);

            return newSAM;
        }
        private object GetNewARTData(DateTime FromDate, DateTime ToDate)
        {
            NewART newART = new NewART();

            var NTBAdult=db2.NutritionAdultRegisters.Count(e=>e.CoexistingConditions=="TB" && e.age>15 && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NTB15To17=db2.NutritionAdultRegisters.Count(e=>e.CoexistingConditions=="TB" && e.age >= 15 - 17 && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NTBPost = db2.NutritionAdultRegisters.Count(e=>e.CoexistingConditions=="TB" && e.maternalNutrition=="Post-natal" && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NTB0To59M = db2.NutritionChildRegisters.Count(e=>e.CoexistingConditions=="TB" && e.age <= 5 && e.VisitType=="New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NTB5To15 = db2.NutritionChildRegisters.Count(e=>e.CoexistingConditions=="TB" && e.age >= 5 - 15 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NDiarrhoeaAdult = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age > 15 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NDiarrhoea15To17 = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 15 - 17 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NDiarrhoeaPost = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.maternalNutrition == "Post-natal" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NDiarrhoea0To59M = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age <= 5 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NDiarrhoea5To15 = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 5 - 15 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NOtherOIsAdult = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age > 15 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NOtherOIs15To17 = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 15 - 17 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NOtherOIsPost = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.maternalNutrition == "Post-natal" && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NOtherOIs0To59M = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age <= 5 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NOtherOIs5To15 = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 5 - 15 && e.VisitType == "New" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RTBAdult = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "TB" && e.age > 15 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RTB15To17 = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "TB" && e.age >= 15 - 17 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RTBPost = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "TB" && e.maternalNutrition == "Post-natal" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RTB0To59M = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "TB" && e.age <= 5 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RTB5To15 = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "TB" && e.age >= 5 - 15 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RDiarrhoeaAdult = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age > 15 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RDiarrhoea15To17 = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 15 - 17 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RDiarrhoeaPost = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.maternalNutrition == "Post-natal" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RDiarrhoea0To59M = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age <= 5 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var RDiarrhoea5To15 = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 5 - 15 && e.VisitType == "Revist" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ROtherOIsAdult = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age > 15 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ROtherOIs15To17 = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 15 - 17 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ROtherOIsPost = db2.NutritionAdultRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.maternalNutrition == "Post-natal" && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ROtherOIs0To59 = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age <= 5 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ROtherOIs5To15 = db2.NutritionChildRegisters.Count(e => e.CoexistingConditions == "Diarrhoea" && e.age >= 5 - 15 && e.VisitType == "Revisit" && e.date_time >= FromDate && e.date_time <= ToDate);

            newART._NewART.AddNewARTRow(
                NTBAdult,
                NTB15To17,
                NTBPost,
                NTB0To59M,
                NTB5To15,
                NDiarrhoeaAdult,
                NDiarrhoea15To17,
                NDiarrhoeaPost,
                NDiarrhoea0To59M,
                NDiarrhoea5To15,
                NOtherOIsAdult,
                NOtherOIs15To17,
                NOtherOIsPost,
                NOtherOIs0To59M,
                NOtherOIs5To15,
                RTBAdult,
                RTB15To17,
                RTBPost,
                RTB0To59M,
                RTB5To15,
                RDiarrhoeaAdult,
                RDiarrhoea15To17,
                RDiarrhoeaPost,
                RDiarrhoea0To59M,
                RDiarrhoea5To15,
                ROtherOIsAdult,
                ROtherOIs15To17,
                ROtherOIsPost,
                ROtherOIs0To59,
                ROtherOIs5To15);
            return newART;
        }
        private object GetNewAnaemiaData(DateTime?  FromDate, DateTime? ToDate)
        {
            NewAnaemia newAnaemia = new NewAnaemia();

           var NSAdultP= db2.NutritionAdultRegisters.Count(e=>e.Anaemia=="Severe" && e.VisitType=="New" && e.ServicePoint=="CCC"&& e.age >15 && e.date_time >= FromDate && e.date_time <= ToDate); 
           var NSAdultN= db2.NutritionAdultRegisters.Count(e => e.Anaemia == "Severe" && e.VisitType == "New" && e.ServicePoint != "CCC" && e.age > 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NSAllChildren= db2.NutritionChildRegisters.Count(e => e.Anaemia == "Severe" && e.VisitType == "New"  && e.age <= 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NMAdultP= db2.NutritionAdultRegisters.Count(e => e.Anaemia == "Moderate" && e.VisitType == "New" && e.ServicePoint == "CCC" && e.age > 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NMAdultN = db2.NutritionAdultRegisters.Count(e => e.Anaemia == "Moderate" && e.VisitType == "New" && e.ServicePoint != "CCC" && e.age > 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NMAllChildren= db2.NutritionChildRegisters.Count(e => e.Anaemia == "Moderate" && e.VisitType == "New" && e.age <= 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NMiAdultP = db2.NutritionAdultRegisters.Count(e => e.Anaemia == "Mild" && e.VisitType == "New" && e.ServicePoint == "CCC" && e.age > 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NMiAdultN = db2.NutritionAdultRegisters.Count(e => e.Anaemia == "Mild" && e.VisitType == "New" && e.ServicePoint != "CCC" && e.age > 15 && e.date_time >= FromDate && e.date_time <= ToDate);
           var NMiAllChildren = db2.NutritionChildRegisters.Count(e => e.Anaemia == "Mild" && e.VisitType == "New" && e.age <= 15 && e.date_time >= FromDate && e.date_time <= ToDate);



            newAnaemia._NewAnaemia.AddNewAnaemiaRow(
                NSAdultP,
                NSAdultN,
                NSAllChildren,
                NMAdultP,
                NMAdultN,
                NMAllChildren,
                NMiAdultP,
                NMiAdultN,
                NMiAllChildren,
                NSAdultP+ NSAdultN+ NSAllChildren,
                NMAdultP+ NMAdultN+ NMAllChildren,
                NMiAdultP+ NMiAdultN+ NMiAllChildren);
            return newAnaemia;
        }
        private object GetNewInfantData(DateTime FromDate, DateTime ToDate)
        {
            NewInfant newInfant = new NewInfant();

            var EBF = db2.NutritionChildRegisters.Count(e=>e.First0to6Months=="Exclusive Breastfeeding" && e.ServicePoint=="CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ERF = db2.NutritionChildRegisters.Count(e => e.First0to6Months == "Exclusive replacement feeding" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var MF = db2.NutritionChildRegisters.Count(e => e.First0to6Months == "Mixed feeding" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var BF = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Breastfeeding" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NBF = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Not breastfeeding" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NK = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Not Known" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var BCF = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Began complimentary feeding" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EBF1 = db2.NutritionChildRegisters.Count(e => e.First0to6Months == "Exclusive Breastfeeding" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ERF1 = db2.NutritionChildRegisters.Count(e => e.First0to6Months == "Exclusive replacement feeding" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var MF1 = db2.NutritionChildRegisters.Count(e => e.First0to6Months == "Mixed feeding" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var BF1 = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Breastfeeding" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NBF1 = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Not breastfeeding" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NK1 = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Not Known" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var BCF1 = db2.NutritionChildRegisters.Count(e => e.Sixto12Months == "Began complimentary feeding" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);


            newInfant._NewInfant.AddNewInfantRow(
                EBF,
                ERF,
                MF,
                BF,
                NBF,
                NK,
                BCF,
                EBF1,
                ERF1,
                MF1,
                BF1,
                NBF1,
                NK1,
                BCF1);
            return newInfant; 
        }

        private object GetNInterventionData(DateTime FromDate, DateTime ToDate)
        {
            var One= db2.NutritionAdultRegisters.Count(e=>e.maternalNutrition=="Pre-natal" && e.age >= 15 - 17 && e.ServicePoint == "CCC" &&  e.date_time >= FromDate && e.date_time <= ToDate);
            var Two = 0;
            var Three = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Pre-natal" && e.age >15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Four = 0;
            var Five = 0;
            var Six = 0;
            var Seven = 0;
            var Eight = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.age >= 15 - 17 && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Nine = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal"  && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Ten = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.age > 15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Eleven = 0;
            var Twelve = 0;
            var Thirteen = 0;
            var Fourteen = 0;
            var Fifteen = db2.NutritionAdultRegisters.Count(e=>e.CriticalNutritionPractices != null && e.age >= 15 - 17 &&  e.ServicePoint=="CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Sixteen = db2.NutritionAdultRegisters.Count(e => e.CriticalNutritionPractices != null && e.maternalNutrition == "Post-natal" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Seventeen = db2.NutritionAdultRegisters.Count(e => e.CriticalNutritionPractices != null && e.age >15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Eighteen = db2.NutritionAdultRegisters.Count(e => e.CriticalNutritionPractices != null && e.age <=5 && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Nineteen = db2.NutritionAdultRegisters.Count(e => e.CriticalNutritionPractices != null && e.age >= 5-15 && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Twenty = db2.NutritionAdultRegisters.Count(e => e.CriticalNutritionPractices != null && e.age <= 5 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var TwentyOne= db2.NutritionAdultRegisters.Count(e => e.CriticalNutritionPractices != null && e.age >= 5 - 15 && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var TwentyTwo = 0;
            var TwentyThree = 0;
            var TwentyFour = 0;
            var TwentyFive = 0;
            var TwentySix = 0;
            var TwentySeven = 0;
            var TwentyEight = 0;
            var TwentyNine = db2.NutritionAdultRegisters.Count(e=>e.age >=15-17 && e.TherapeuticFoods=="F75||3=F100" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Thirty = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition=="Post-natal" && e.TherapeuticFoods == "F75||3=F100" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyOne = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.TherapeuticFoods == "F75||3=F100" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyTwo = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.TherapeuticFoods == "F75||3=F100" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyThree = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.TherapeuticFoods == "F75||3=F100" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyFour = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.TherapeuticFoods == "2=F75||3=F100" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyFive = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.TherapeuticFoods == "F75||3=F100" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtySix = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.TherapeuticFoods == "RUTF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtySeven = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.TherapeuticFoods == "RUTF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyEight = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.TherapeuticFoods == "RUTF" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var ThirtyNine = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.TherapeuticFoods == "RUTF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Fourty = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.TherapeuticFoods == "RUTF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyOne = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.TherapeuticFoods == "RUTF" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyTwo = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.TherapeuticFoods == "RUTF" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyThree = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.SupplementalFoods == "RUSF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyFour = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.SupplementalFoods == "RUSF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyFive = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.SupplementalFoods == "RUSF" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtySix = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.SupplementalFoods == "RUSF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtySeven = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.SupplementalFoods == "RUSF" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyEight = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.SupplementalFoods == "RUFT" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FourtyNine = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.SupplementalFoods == "RUSF" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Fifty = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyOne = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyTwo = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyThree = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyFour = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyFive = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftySix = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.SupplementalFoods == "FBF||CSB" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftySeven = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyEight = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var FiftyNine = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Sixty = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyOne = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyTwo = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyThree = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.SupplementalFoods == "Liquid Nutrition Supplements" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyFour = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.Micronutrients!=null && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyFive = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.Micronutrients != null && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtySix = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.Micronutrients != null && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtySeven = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.Micronutrients != null && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyEight = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.Micronutrients != null && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SixtyNine = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.Micronutrients != null && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Seventy = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.Micronutrients != null && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            
            var SeventyOne = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.TherapeuticFoods == "Others" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventyTwo = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.TherapeuticFoods == "Others" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventyThree = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.TherapeuticFoods == "Others" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventyFour = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.TherapeuticFoods == "Others" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventyFive= db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.TherapeuticFoods == "Others" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventySix = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.TherapeuticFoods == "Others" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventySeven = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.TherapeuticFoods == "Others" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventyEight = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.OutcomeClient=="Gaining Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var SeventyNine = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.OutcomeClient == "Gaining Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Eighty = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.OutcomeClient == "Gaining Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyOne = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Gaining Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyTwo = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Gaining Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyThree = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Gaining Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyFour = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Gaining Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyFive = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.OutcomeClient == "Loosing Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightySix = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.OutcomeClient == "Loosing Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightySeven = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.OutcomeClient == "Loosing Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyEight = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Loosing Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var EightyNine = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Loosing Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Ninety = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Loosing Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyOne = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Loosing Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyTwo = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.OutcomeClient == "Static Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyThree = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.OutcomeClient == "Static Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyFour = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.OutcomeClient == "Static Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyFive = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Static Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetySix = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Static Weight" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetySeven = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Static Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyEight = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Static Weight" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var NinetyNine = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.OutcomeClient == "Cured" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.OutcomeClient == "Cured" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredOne = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.OutcomeClient == "Cured" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredTwo = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Cured" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredThree= db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Cured" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredFour = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Cured" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredFive = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Cured" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            
            var HundredSix = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredSeven = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredEight = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.OutcomeClient == "Discharged" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredNine = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredTen = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredEleven = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Discharged" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredTwelve = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Discharged" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredThirteen = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredFourteen = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "2=Post-natal" && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredFifteen = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.OutcomeClient == "Discharged" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredSixteen = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredSeventeen = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Discharged" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredEighteen = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.OutcomeClient == "Discharged" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredNineteen = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.OutcomeClient == "Discharged" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var HundredTwenty = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ReferralsandTransfers=="Referral for in-patient care||Referral to other clinics" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred21 = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.ReferralsandTransfers == "Referral for in-patient care||Referral to other clinics" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred22 = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ReferralsandTransfers == "Referral for in-patient care||Referral to other clinics" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred23 = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ReferralsandTransfers == "Referral for in-patient care||Referral to other clinics" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred24 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ReferralsandTransfers == "Referral for in-patient care||Referral to other clinics" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred25= db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ReferralsandTransfers == "Referral for in-patient care||Referral to other clinics" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred26 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ReferralsandTransfers == "Referral for in-patient care||Referral to other clinics" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred27 = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred28 = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred29 = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred30 = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred31 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred32 = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred33 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ReferralsandTransfers == "Referral for livelihood support(food insecure)" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred34 = db2.NutritionAdultRegisters.Count(e => e.age >= 15 - 17 && e.ReferralsandTransfers == "Transferred" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred35 = db2.NutritionAdultRegisters.Count(e => e.maternalNutrition == "Post-natal" && e.ReferralsandTransfers == "Transferred" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred36 = db2.NutritionAdultRegisters.Count(e => e.age > 15 && e.ReferralsandTransfers == "Transferred" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred37 = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ReferralsandTransfers == "Transferred" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred38 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ReferralsandTransfers == "Transferred" && e.ServicePoint == "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred39 = db2.NutritionChildRegisters.Count(e => e.age <= 5 && e.ReferralsandTransfers == "Transferred" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            var Hundred40 = db2.NutritionChildRegisters.Count(e => e.age >= 5 - 15 && e.ReferralsandTransfers == "Transferred" && e.ServicePoint != "CCC" && e.date_time >= FromDate && e.date_time <= ToDate);
            
            NewNutrition newNutrition = new NewNutrition();
            newNutrition.NewIntervention.AddNewInterventionRow(
                One,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten,
                Eleven,Twelve,Thirteen,Fourteen,Fifteen,Sixteen,Seventeen,Eighteen,Nineteen,Twenty,
                TwentyOne,TwentyTwo,TwentyThree,TwentyFour,TwentyFive,TwentySix,TwentySeven,TwentyEight,TwentyNine,Thirty,
                ThirtyOne,ThirtyTwo,ThirtyThree,ThirtyFour,ThirtyFive,ThirtySix,ThirtySeven,ThirtyEight,ThirtyNine,Fourty,
                FourtyOne,FourtyTwo,FourtyThree,FourtyFour,FourtyFive,FourtySix,FourtySeven,FourtyEight,FourtyNine,Fifty,
                FiftyOne,FiftyTwo,FiftyThree,FiftyFour,FiftyFive,FiftySix,FiftySeven,FiftyEight,FiftyNine,Sixty,
                SixtyOne,SixtyTwo,SixtyThree,SixtyFour,SixtyFive,SixtySix,SixtySeven,SixtyEight,SixtyNine,Seventy,
                SeventyOne,SeventyTwo,SeventyThree,SeventyFour,SeventyFive,SeventySix,SeventySeven,SeventyEight,SeventyNine,Eighty,
                EightyOne,EightyTwo,EightyThree,EightyFour,EightyFive,EightySix,EightySeven,EightyEight,EightyNine,Ninety,
                NinetyOne,NinetyTwo,NinetyThree,NinetyFour,NinetyFive,NinetySix,NinetySeven,NinetyEight,NinetyNine,Hundred,
                HundredOne,HundredTwo,HundredThree,HundredFour,HundredFive,HundredSix,HundredSeven,HundredEight,HundredNine,HundredTen,
                HundredEleven,HundredTwelve,HundredThirteen,HundredFourteen,HundredFifteen,HundredSixteen,HundredSeventeen,HundredEighteen,HundredNineteen,HundredTwenty,
                Hundred21,Hundred22,Hundred23,Hundred24,Hundred25,Hundred26,Hundred27,Hundred28,Hundred29,Hundred30,
                Hundred31,Hundred32,Hundred33,Hundred34,Hundred35,Hundred36,Hundred37,Hundred38,Hundred39,Hundred40
                );
              
            return newNutrition;
        }


        private object GetNewAdditionalInforData(DateTime FromDate, DateTime ToDate)
        {
           NewAdditional  newAdditional = new NewAdditional();
            newAdditional._NewAdditional.AddNewAdditionalRow(
                0,
                0,
                0,
                0,
                0,
                0, 
                0, 
                0, 
                0, 
                0);
            return newAdditional;
        }

        #endregion

        #region MOH 710 REPORT
        public ActionResult MOH710()
        {
            return View();
        }

        public ActionResult Generate710Report(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            var DataSetChildImmunization = GetChildImmunizationData(FromDate, ToDate);
            var MeaslesVitaminA = GetMeaslesVitaminAData(FromDate, ToDate);
            var Tetanus = GetTetanusData(FromDate, ToDate);
            var AdverseNew = GetAdverseNewData(FromDate, ToDate);
            var VitaminA = GetVitaminAData(FromDate, ToDate);

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH710/Main.rpt")));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Rd.Subreports["ChildImmunization.rpt"].SetDataSource(DataSetChildImmunization);
            Rd.Subreports["VitaminAMeasles.rpt"].SetDataSource(MeaslesVitaminA);
            Rd.Subreports["TetanusToxoid.rpt"].SetDataSource(Tetanus);
            Rd.Subreports["AdverseEvents.rpt"].SetDataSource(AdverseNew);
            Rd.Subreports["VitaminReport.rpt"].SetDataSource(VitaminA);

            Rd.SetParameterValue("fromDate", FromDate);
            Rd.SetParameterValue("toDate", ToDate);


            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "MOH 710 " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);
        }



        public class ChildImmunization
        {

            public string Name { get; set; }
            public int CountGreaterThanOneYear { get; set; }
            public int CountLessThanOneYear { get; set; }

        }
        private DataSetChildImmunization GetChildImmunizationData(DateTime? fromDate, DateTime? toDate)
        {
            var allTypesOfImmunization = db2.ImmunizationMasters.ToList();

            var AllImmunizations = db2.ImmunizationAdmins
                                    .Where(p => p.DateGiven >= fromDate && p.DateGiven <= toDate)
                                    .ToList();

            foreach (var typeImmunization in AllImmunizations)
            {
                var PatientImmunizeds = AllImmunizations.Where(p => p.ImmunizationMasterId == typeImmunization.Id).ToList();


                ChildImmunization childImmunization = new ChildImmunization
                {

                };


                foreach (var pat in PatientImmunizeds)
                {

                    var dateOfBirth = pat.OpdRegister?.Patient.DOB;


                    if (dateOfBirth != null)
                    {

                        var age = pat.DateGiven.Year - dateOfBirth.Value.Year;

                        if (age > 1)
                        {
                            childImmunization.CountGreaterThanOneYear++;
                        }
                        else
                        {
                            childImmunization.CountLessThanOneYear++;
                        }

                    }
                }
            }

            DataSetChildImmunization dataSetChildImmunization = new DataSetChildImmunization();
            ChildImmunization IMBCGDosesAdministered = new ChildImmunization()
            {
                Name = "BCG doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.Trim().ToUpper().Contains("BCG ") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.Trim().ToUpper().Contains("BCG ") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1)
            };
            ChildImmunization IMOPVBirthDosesAdministered = new ChildImmunization()
            {
                Name = "OPV Birth doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("DOSE AT BIRTH-2 DROPS ORALLY") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Contains("DOSE AT BIRTH-2 DROPS ORALLY") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };

            ChildImmunization IMOPV1DosesAdministered = new ChildImmunization()
            {
                Name = "OPV1  doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMOPV2DosesAministered = new ChildImmunization()
            {
                Name = "OPV2  doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMOPV3DosesAdministered = new ChildImmunization()
            {
                Name = "OPV3  doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("THIRD DOSE AT 14 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("POLIO VACCINE:(Bivalent Oral Polio Vaccine(bOPV))") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("THIRD DOSE AT 14 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMIPVDosesAdministered = new ChildImmunization()
            {
                Name = "IPV doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("IPV(Inactivated Polio Vaccine)") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("IPV(Inactivated Polio Vaccine)") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMDPTHepHiB1DosesAdministered = new ChildImmunization()
            {
                Name = "DPT/Hep+HiB1 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("DIPTHERIA/PERTUSIS/TETANUS/HEPATITIS B/HAEMOPHILUS INFLUENZA TYPE B") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("DIPTHERIA/PERTUSIS/TETANUS/HEPATITIS B/HAEMOPHILUS INFLUENZA TYPE B") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMDPTHepHiB2DosesAdministered = new ChildImmunization()
            {
                Name = "DPT/Hep+HiB2 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("DIPTHERIA/PERTUSIS/TETANUS/HEPATITIS B/HAEMOPHILUS INFLUENZA TYPE B") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("DIPTHERIA/PERTUSIS/TETANUS/HEPATITIS B/HAEMOPHILUS INFLUENZA TYPE B") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMDPTHepHiB3DosesAdministered = new ChildImmunization()
            {
                Name = "DPT/Hep+HiB3 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("DIPTHERIA/PERTUSIS/TETANUS/HEPATITIS B/HAEMOPHILUS INFLUENZA TYPE B") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("THIRD DOSE AT 14 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("DIPTHERIA/PERTUSIS/TETANUS/HEPATITIS B/HAEMOPHILUS INFLUENZA TYPE B") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("THIRD DOSE AT 14 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMPneumococal1DosesAdministered = new ChildImmunization()
            {
                Name = "Pnemococal 1 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("PNEUMOCOCCAL VACCINE") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("PNEUMOCOCCAL VACCINE") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMPneumococal2DosesAdministered = new ChildImmunization()
            {
                Name = "Pnemococal 2 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("PNEUMOCOCCAL VACCINE") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("PNEUMOCOCCAL VACCINE") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMPneumococal3DosesAdministered = new ChildImmunization()
            {
                Name = "Pnemococal 3 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("PNEUMOCOCCAL VACCINE") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("THIRD DOSE AT 14 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("PNEUMOCOCCAL VACCINE") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("THIRD DOSE AT 14 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMRotavirus1 = new ChildImmunization()
            {
                Name = "Rotavirus 1 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("ROTA VIRUS(ROTARIX)") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("ROTA VIRUS(ROTARIX)") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("FIRST DOSE AT 6 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMRotavirus2 = new ChildImmunization()
            {
                Name = "Rotavirus 2 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("ROTA VIRUS(ROTARIX)") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("ROTA VIRUS(ROTARIX)") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("SECOND DOSE AT 10 WEEKS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMVitaminA = new ChildImmunization()
            {
                Name = "Vitamin A at 6 months(100,000 UI)",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("VITAMIN A CAPSULE FROM 6 MONTHS") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("1000001U") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("VITAMIN A CAPSULE FROM 6 MONTHS") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("1000001U") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMYellowFever = new ChildImmunization()
            {
                Name = "Yellow fever doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("YELLOW FEVER VACCINE AT NINE MONTHS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("YELLOW FEVER VACCINE AT NINE MONTHS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMMeaslesRubella = new ChildImmunization()
            {
                Name = "Measles-Rubella 1 doses Administered",
                CountGreaterThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("MEASLES RUBELLA VACCINE(MR) AT 9 MONTHS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 1),
                CountLessThanOneYear = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("MEASLES RUBELLA VACCINE(MR) AT 9 MONTHS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 1),
            };
            ChildImmunization IMFullyImmunized = new ChildImmunization()
            {
                Name = "Fully Immunized Children(FIC) under 1 year",
                CountGreaterThanOneYear = 0,
                CountLessThanOneYear = 0
            };

            List<ChildImmunization> LstChildImmunization = new List<ChildImmunization>();
            LstChildImmunization.Add(IMBCGDosesAdministered);
            LstChildImmunization.Add(IMOPVBirthDosesAdministered);
            LstChildImmunization.Add(IMOPV1DosesAdministered);
            LstChildImmunization.Add(IMOPV2DosesAministered);
            LstChildImmunization.Add(IMOPV3DosesAdministered);
            LstChildImmunization.Add(IMIPVDosesAdministered);
            LstChildImmunization.Add(IMDPTHepHiB1DosesAdministered);
            LstChildImmunization.Add(IMDPTHepHiB2DosesAdministered);
            LstChildImmunization.Add(IMDPTHepHiB3DosesAdministered);
            LstChildImmunization.Add(IMPneumococal1DosesAdministered);
            LstChildImmunization.Add(IMPneumococal2DosesAdministered);
            LstChildImmunization.Add(IMPneumococal3DosesAdministered);
            LstChildImmunization.Add(IMRotavirus1);
            LstChildImmunization.Add(IMRotavirus2);
            LstChildImmunization.Add(IMVitaminA);
            LstChildImmunization.Add(IMYellowFever);
            LstChildImmunization.Add(IMMeaslesRubella);
            LstChildImmunization.Add(IMFullyImmunized);


            foreach (var item in LstChildImmunization)
            {
                dataSetChildImmunization.ChildImmunization.AddChildImmunizationRow(
                    item.Name,
                    item.CountLessThanOneYear,
                    item.CountGreaterThanOneYear);

            }
            return dataSetChildImmunization;

        }
        public class Measles
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public int Age { get; set; }
        }
        private MeaslesVitaminA GetMeaslesVitaminAData(DateTime? fromDate, DateTime? toDate)
        {
            var AllImmunizations = db2.ImmunizationAdmins
                                    .Where(p => p.DateGiven >= fromDate && p.DateGiven <= toDate)
                                    .ToList();
            var allTypesOfImmunization = db2.ImmunizationMasters.ToList();

            foreach (var typeImmunization in AllImmunizations)
            {
                var PatientImmunizeds = AllImmunizations.Where(p => p.ImmunizationMasterId == typeImmunization.Id).ToList();
                Measles measles = new Measles()
                {

                };
                foreach (var pat in PatientImmunizeds)
                {

                    var dateOfBirth = pat.OpdRegister?.Patient.DOB;

                    if (dateOfBirth != null)
                    {
                        var Age = pat.DateGiven.Year - dateOfBirth.Value.Year;
                    }
                }
            }

            MeaslesVitaminA measlesVitaminA = new MeaslesVitaminA();

            Measles MVitaminAAt1 = new Measles()
            {

                Name = "Vitamin A at 1year (200,000IU)",
                Value = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("VITAMIN A CAPSULE FROM 6 MONTHS") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("2000001U") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) == 1),

            };
            Measles MVitaminAAt1AndAhalf = new Measles()
            {
                Name = "Vitamin a at 11/2 years(200,000IU)",
                Value = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("VITAMIN A CAPSULE FROM 6 MONTHS") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("2000001U") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) == 11 / 2),

            };
            Measles MMeaslesAt1AndAhalf = new Measles()
            {
                Name = "Measles-Rubella 2 Dose Adm (at 11/2-2 years)",
                Value = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("MEASLES RUBELLA VACCINE(MR) AT 18 MONTHS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) == 11 / 2 - 2),

            };
            Measles MMeaslesAtGreater2 = new Measles()
            {
                Name = "Measles-Rubella 2 Dose Administered >2yrs",
                Value = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName.ToUpper().Contains("MEASLES RUBELLA VACCINE(MR) AT 24 MONTHS") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 2),

            };

            List<Measles> LstMeasles = new List<Measles>();
            LstMeasles.Add(MVitaminAAt1);
            LstMeasles.Add(MVitaminAAt1AndAhalf);
            LstMeasles.Add(MMeaslesAt1AndAhalf);
            LstMeasles.Add(MMeaslesAtGreater2);


            foreach (var item in LstMeasles)
            {
                measlesVitaminA._MeaslesVitaminA.AddMeaslesVitaminARow(
                    item.Name,
                    item.Value,
                    item.Age);
            }
            return measlesVitaminA;
        }


        public class TetanusT
        {

            public string Name { get; set; }
            public int First { get; set; }
            public int Second { get; set; }
            public int Third { get; set; }
            public int Fourth { get; set; }
            public int Fifth { get; set; }

        }

        private Tetanus GetTetanusData(DateTime? fromDate, DateTime? toDate)
        {
            var AllImmunizations = db2.ImmunizationAdmins
                                     .Where(p => p.DateGiven >= fromDate && p.DateGiven <= toDate)
                                     .ToList();
            var allTypesOfImmunization = db2.MCHPreventativeServices.Where(p => p.DateGiven >= fromDate && p.DateGiven <= toDate).ToList();
            Tetanus tetanus = new Tetanus();

            TetanusT TDoses = new TetanusT()
            {

                First = allTypesOfImmunization.Count(e => e.Vaccine == "TetunusToxoid1"),
                Second = allTypesOfImmunization.Count(e => e.Vaccine == "TetunusToxoid2"),
                Third = allTypesOfImmunization.Count(e => e.Vaccine == "TetunusToxoid3"),
                Fourth = allTypesOfImmunization.Count(e => e.Vaccine == "TetunusToxoid4"),
                Fifth = allTypesOfImmunization.Count(e => e.Vaccine == "TetunusToxoid5"),
            };


            List<TetanusT> LstTetanusT = new List<TetanusT>();
            LstTetanusT.Add(TDoses);


            foreach (var item in LstTetanusT)
            {
                tetanus._Tetanus.AddTetanusRow(item.First, item.Second, item.Third, item.Fourth, item.Fifth);

            }
            return tetanus;
        }

        public class Adverse

        {
            public int CountGreaterThanOneYear { get; set; }
            public int CountLessThanOneYear { get; set; }
        }
        private AdverseNew GetAdverseNewData(DateTime fromDate, DateTime toDate)
        {
            var AllImmunizations = db2.ImmunizationAdmins
                                      .Where(p => p.DateGiven >= fromDate && p.DateGiven <= toDate)
                                      .ToList();
            var allTypesOfImmunization = db2.ImmunizationMasters.ToList();
            AdverseNew adverseNew = new AdverseNew();

            Adverse AAdverseEvents = new Adverse()
            {
                CountGreaterThanOneYear = 0,
                CountLessThanOneYear = 0

            };

            List<Adverse> LstAdverse = new List<Adverse>();
            // LstAdverse.Add(AAdverseEvents);

            foreach (var typeImmunization in allTypesOfImmunization)
            {
                var PatientImmunizeds = AllImmunizations.Where(p => p.ImmunizationMasterId == typeImmunization.Id).ToList();


                Adverse adverse = new Adverse()
                {

                };

                foreach (var pat in PatientImmunizeds)
                {

                    var dateOfBirth = pat.OpdRegister?.Patient.DOB;

                    if (dateOfBirth != null)
                    {
                        var age = pat.DateGiven.Year - dateOfBirth.Value.Year;

                        if (age > 1)
                        {
                            adverse.CountGreaterThanOneYear++;
                        }
                        else
                        {
                            adverse.CountLessThanOneYear++;
                        }
                    }
                }
                foreach (var item in LstAdverse)
                {
                    adverseNew.Adverse.AddAdverseRow(item.CountLessThanOneYear, item.CountGreaterThanOneYear);
                }

            }
            return adverseNew;
        }
        public class Vitamin
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public int Age { get; set; }
        }
        private VitaminA GetVitaminAData(DateTime fromDate, DateTime toDate)
        {
            var AllImmunizations = db2.ImmunizationAdmins
                                   .Where(p => p.DateGiven >= fromDate && p.DateGiven <= toDate)
                                   .ToList();
            var allTypesOfImmunization = db2.ImmunizationMasters.ToList();
            foreach (var typeImmunization in allTypesOfImmunization)
            {
                var PatientImmunizeds = AllImmunizations.Where(p => p.ImmunizationMasterId == typeImmunization.Id).ToList();
                Vitamin vitamin = new Vitamin()
                {

                };
                foreach (var pat in PatientImmunizeds)
                {

                    var dateOfBirth = pat.OpdRegister?.Patient.DOB;

                    if (dateOfBirth != null)
                    {
                        var Age = pat.DateGiven.Year - dateOfBirth.Value.Year;
                        //var Months = Age * 12;
                    }
                }
            }

            VitaminA vitaminA = new VitaminA();
            Vitamin VVitaminA2To5 = new Vitamin()
            {
                Name = "Vitamin A 2 years to 5 years(200,000 IU)",
                Value = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("VITAMIN A CAPSULE FROM 6 MONTHS") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("2000001U") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) == 2 - 5),

            };
            Vitamin VVitaminALactating = new Vitamin()
            {
                Name = "Vitamin A Supplemental Lactating Mothers (200,000IU)",
                Value = AllImmunizations.Count(e => e.ImmunizationMaster.ImmunizationCategory.ImmunizationCategoryName == ("VITAMIN A CAPSULE FROM 6 MONTHS") && e.ImmunizationMaster.ImmunizationName.Trim().ToUpper().Contains("2000001U") && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) > 2),
            };
            Vitamin VSquint = new Vitamin()
            {
                Name = "Squint/White Eye reflection under 1 year",
                Value = 0,

            };

            List<Vitamin> LstVitamin = new List<Vitamin>();

            LstVitamin.Add(VVitaminA2To5);
            LstVitamin.Add(VVitaminALactating);
            LstVitamin.Add(VSquint);

            foreach (var item in LstVitamin)
            {
                vitaminA._VitaminA.AddVitaminARow(item.Name, item.Value);

            }
            return vitaminA;

        }



        #endregion
        #region MOH 711
        public ActionResult MOH711()
        {
            return View();
        }


        public ActionResult GenerateMOH711Report(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            var ANC = GetAncSampleData(FromDate, ToDate);
            var NewMAndD = GetMaternityAndDSampleData(FromDate, ToDate);
            var Maternal = GetMaternalSampleData(FromDate, ToDate);
            var Referrals = GetReferralsSmpleData(FromDate, ToDate);
            var SGBVCCC = GetSGBVCCCSampleData(FromDate, ToDate);
            var FP = GetFPSampleData(FromDate, ToDate);
            var DatasetE = GetDatasetESampleData(FromDate, ToDate);
            var DatasetF = GetDatasetFSampleData(FromDate, ToDate);
            var CervicalCancerG = GetCervicalSampleData(FromDate, ToDate);
            var PNCH = GetPNCHSampleData(FromDate, ToDate);
            var RehabServicesI = GetRehabServicesISampleData(FromDate, ToDate);
            var MSocialWorkJ = GetMSocialWorkJSampleData(FromDate, ToDate);
            var PhysiotherapyService = GetPhysiotherapyServiceSampleData(FromDate, ToDate);

            Reports.MOH711.MainDataset mainDataset = new Reports.MOH711.MainDataset();
            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            int session = Convert.ToInt32(Session["UserId"]);
            var userLoggedIn = db2.Users.Where(p => p.Id == session).FirstOrDefault();
            mainDataset.HospitalD.AddHospitalDRow(HospitalName,
                                                   "",
                                                   "",
                                                   ToDate.ToShortMonthName(),
                                                   toDate.Year.ToString());

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/MOH711/Main.rpt")));
            rd.SetDataSource(mainDataset);


            rd.Subreports["MOH711.rpt"].SetDataSource(GetAncSampleData(FromDate, ToDate));
            rd.Subreports["MOH711B.rpt"].SetDataSource(GetMaternityAndDSampleData(FromDate, ToDate));
            rd.Subreports["MaternalComplications.rpt"].SetDataSource(GetMaternalSampleData(FromDate, ToDate));
            rd.Subreports["ReferralsReport.rpt"].SetDataSource(GetReferralsSmpleData(FromDate, ToDate));
            rd.Subreports["SGBV.rpt"].SetDataSource(GetSGBVCCCSampleData(FromDate, ToDate));
            rd.Subreports["FamilyPlanningD.rpt"].SetDataSource(GetFPSampleData(FromDate, ToDate));
            rd.Subreports["PACServicesE.rpt"].SetDataSource(GetDatasetESampleData(FromDate, ToDate));
            rd.Subreports["CHANISF.rpt"].SetDataSource(GetDatasetFSampleData(FromDate, ToDate));
            rd.Subreports["CervicalCancerG.rpt"].SetDataSource(GetCervicalSampleData(FromDate, ToDate));
            rd.Subreports["PostNatalCareH.rpt"].SetDataSource(GetPNCHSampleData(FromDate, ToDate));
            rd.Subreports["RehabI.rpt"].SetDataSource(GetRehabServicesISampleData(FromDate, ToDate));
            rd.Subreports["MSocialWorkJ.rpt"].SetDataSource(GetMSocialWorkJSampleData(FromDate, ToDate));
            rd.Subreports["PhysiotherapyService.rpt"].SetDataSource(GetPhysiotherapyServiceSampleData(FromDate, ToDate));


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "MOH 711" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }

        private ANC GetAncSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            
            ANC dataSetANC = new ANC();
            
            var NewClients = db2.MCHPreviousPregnancies.Count(P => P.NumberOfANCAttended == 1 && P.DateAdded >= FromDate && P.DateAdded <= ToDate);
            var Revisits = db2.MCHPreviousPregnancies.Count(p => p.NumberOfANCAttended > 1 && p.DateAdded >= FromDate && p.DateAdded <= ToDate);
            var TotalGivenIPTFirstDose = db2.MCHPreventativeServices.Count(e => e.Vaccine == "Malaria Prophylaxis IPT1" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TotalGivenIPTSecondDose = db2.MCHPreventativeServices.Count(e => e.Vaccine == "Tetunus Malaria Prophylaxis IPT2" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var TotalWithHb = db2.MCHPresentPregnancies.Count(e => e.HB != null && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ClientsCompleted4AntenatalVisits = db2.MCHPreventativeServices.Count(e => e.Vaccine == "Tetunus Toxoid 5" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var WomenDoneBreastExamination = db2.MCHCancerScreenings.Count(e => e.BreastTestResult != null && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var Adolescents10To14Pregnant = db2.MCHMaternalProfiles.Where(e => (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 14 && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 10 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var Adolescents15To19Pregnant = db2.MCHMaternalProfiles.Where(e => (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 19 && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();

            dataSetANC.A_ANC_PMCT1.AddA_ANC_PMCT1Row(
                                                     (NewClients + Revisits ),
                                                      TotalGivenIPTFirstDose,
                                                      TotalGivenIPTSecondDose,
                                                      TotalWithHb,
                                                      ClientsCompleted4AntenatalVisits,
                                                      0, 
                                                      0,
                                                      0, 
                                                      0,
                                                      0,
                                                      WomenDoneBreastExamination, 
                                                      0,
                                                      Adolescents10To14Pregnant, 
                                                      Adolescents15To19Pregnant,
                                                      0,
                                                      0, 
                                                      0);

            return dataSetANC;

        }
        public class MaternityServices1
        {
            public string NameOfService { get; set; }
            public int Total { get; set; }

        }
        private NewMAndD GetMaternityAndDSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            var deliveries = db2.BillServices.Where(e => DbFunctions.TruncateTime(e.DateAdded) >= FromDate &&
            DbFunctions.TruncateTime(e.DateAdded) <= ToDate && e.Service.Department.DepartmentName.ToUpper()
            .Contains("MATERNITY"));

            NewMAndD newMAndD = new NewMAndD();

            MaternityServices1 MSsNormalDelivery = new MaternityServices1()
            {
                NameOfService = "Normal Delivery",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("NORMAL DELIVERY"))
            };
            MaternityServices1 MSsCaesareanSection = new MaternityServices1()
            {
                NameOfService = "Caesarean Section",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("DELIVERY CS"))
            };
            MaternityServices1 MSsBreechDeliveries = new MaternityServices1()
            {
                NameOfService = "Breech Deliveries",
                Total = db2.MCHPresentPregnancies.Count(e => e.Presentation.Trim().ToLower().Contains("breech") && e.DateAdded >= FromDate && e.DateAdded <= ToDate)
        };
            MaternityServices1 MSsAssistedVaginalDelivery = new MaternityServices1()
            {
                NameOfService = "Assisted Vaginal Delivery ",
                Total = 0
            };
            MaternityServices1 MSsTotalDeliveries = new MaternityServices1()
            {
                NameOfService = "TOTAL DELIVERIES ",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("DELIVERY CS")) +
                        deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("NORMAL DELIVERY"))+
                        db2.MCHPresentPregnancies.Count(e => e.Presentation.Trim().ToLower().Contains("breech") && e.DateAdded >= FromDate && e.DateAdded <= ToDate )
                       
        };
            MaternityServices1 MSsLiveBirths = new MaternityServices1()
            {
                NameOfService = "Live Births",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("LIVE BIRTHS"))
            };
            MaternityServices1 MSsFreshStillBirth = new MaternityServices1()
            {
                NameOfService = "Fresh Still Births",
                Total = 0
            };
            MaternityServices1 MSsMaceratedStillBirths = new MaternityServices1()
            {
                NameOfService = "Macerated Still Births",
                Total = 0
            };
            MaternityServices1 MSsBirthWithDeformities = new MaternityServices1()
            {
                NameOfService = "Birth with Deformities",
                Total = 0
            };
            MaternityServices1 MSsLowAPGARScore = new MaternityServices1()
            {
                NameOfService = " No. of Low APGAR Score",
                Total = 0
            };
            MaternityServices1 MSsLowBirthWeight = new MaternityServices1()
            {
                NameOfService = "Low Birth Weight Babies(under 2500gms)",
                Total = 0
            };
            MaternityServices1 MSsBabiesGivenTetracyclineAtBirth = new MaternityServices1()
            {
                NameOfService = "No. of bAbies given tetracycline at birth",
                Total = 0
            };
            MaternityServices1 MSsPreTermBabies = new MaternityServices1()
            {
                NameOfService = "Pre-Term babies",
                Total = 0
            };
            MaternityServices1 MSsBabiesDischargedAlive = new MaternityServices1()
            {
                NameOfService = "No.of babies discharged alive",
                Total = 0
            };
            MaternityServices1 MSsInfantsInitiatedOnBreastFeeding = new MaternityServices1()
            {
                NameOfService = "No.of infants initiated on breestfeeding within 1 hour after birth",
                Total = 0
            };
            MaternityServices1 MSsTotalDeliveriesFromHIVWomen = new MaternityServices1()
            {
                NameOfService = "Total Deliveries from HIV +ve women",
                Total = 0
            };
            MaternityServices1 MSsNeotanalDeaths = new MaternityServices1()
            {
                NameOfService = "Neotanal Deaths",
                Total = 0
            };
            MaternityServices1 MSsNoOfAdolescentsMaternalDeaths = new MaternityServices1()
            {
                NameOfService = "No.of adolescents (10-19YRS) Maternal Deaths",
                Total = 0
            };
            MaternityServices1 MSsMaternalDeaths = new MaternityServices1()
            {
                NameOfService = "MaternalDeaths",
                Total = 0
            };
            MaternityServices1 MSsMaternalDeathsAudited = new MaternityServices1()
            {
                NameOfService = "Maternal Deaths Audited",
                Total = 0
            };

            List<MaternityServices1> LstMaternityServices1 = new List<MaternityServices1>();
            LstMaternityServices1.Add(MSsNormalDelivery);
            LstMaternityServices1.Add(MSsCaesareanSection);
            LstMaternityServices1.Add(MSsBreechDeliveries);
            LstMaternityServices1.Add(MSsAssistedVaginalDelivery);
            LstMaternityServices1.Add(MSsTotalDeliveries);
            LstMaternityServices1.Add(MSsLiveBirths);
            LstMaternityServices1.Add(MSsFreshStillBirth);
            LstMaternityServices1.Add(MSsMaceratedStillBirths);
            LstMaternityServices1.Add(MSsBirthWithDeformities);
            LstMaternityServices1.Add(MSsLowAPGARScore);
            LstMaternityServices1.Add(MSsLowBirthWeight);
            LstMaternityServices1.Add(MSsBabiesGivenTetracyclineAtBirth);
            LstMaternityServices1.Add(MSsPreTermBabies);
            LstMaternityServices1.Add(MSsBabiesDischargedAlive);
            LstMaternityServices1.Add(MSsInfantsInitiatedOnBreastFeeding);
            LstMaternityServices1.Add(MSsTotalDeliveriesFromHIVWomen);
            LstMaternityServices1.Add(MSsNeotanalDeaths);
            LstMaternityServices1.Add(MSsNoOfAdolescentsMaternalDeaths);
            LstMaternityServices1.Add(MSsMaternalDeaths);
            LstMaternityServices1.Add(MSsMaternalDeathsAudited);



            foreach (var item in LstMaternityServices1)
            {
                newMAndD._NewMAndD.AddNewMAndDRow(
                        item.NameOfService,
                        item.Total.ToString()
                    );
            }


            return newMAndD;
        }

        public class MaternalServices
        {
            public string Name { get; set; }
            public int Alive { get; set; }
            public int Deaths { get; set; }

        }
        private Maternal GetMaternalSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            

            Maternal maternal = new Maternal();
            MaternalServices MSAPH = new MaternalServices
            {
                Name = "A.P.H.(Ante Partum Haemorrhage",
                Alive = 0,
                Deaths = 0

            };
            MaternalServices MSPPH = new MaternalServices
            {
                Name = "P.P.H.(Post Partum Hemorrhage",
                Alive = 0,
                Deaths = 0
            };
            MaternalServices MSEclampsia = new MaternalServices
            {
                Name = "Eclampsia",
                Alive = db2.MCHDeliveries.Count(e => e.Eclampsia != null),
                Deaths = 0

            };
            MaternalServices MSRupturedUterus = new MaternalServices
            {
                Name = "Ruptured Uterus",
                Alive = 0,
                Deaths = 0

            };
            MaternalServices MSObstructedLabour = new MaternalServices
            {
                Name = "Obstructed Labour",
                Alive = db2.MCHDeliveries.Count(e=>e.ObstractedLabour != null),
                Deaths = 0

            };
            MaternalServices MSSepsis = new MaternalServices
            {
                Name = "Sepsis",
                Alive = 0,
                Deaths = 0
            };
            List<MaternalServices> LstMaternalServices = new List<MaternalServices>();
            LstMaternalServices.Add(MSAPH);
            LstMaternalServices.Add(MSPPH);
            LstMaternalServices.Add(MSEclampsia);
            LstMaternalServices.Add(MSRupturedUterus);
            LstMaternalServices.Add(MSObstructedLabour);
            LstMaternalServices.Add(MSSepsis);

            foreach (var item in LstMaternalServices)
            {
                maternal._Maternal.AddMaternalRow(
                    item.Name,
                    item.Alive,
                    item.Deaths);
            }

            return maternal;
        }
        private Referrals GetReferralsSmpleData(DateTime? FromDate, DateTime? ToDate)
        {

            Referrals referrals = new Referrals();
            referrals._Referrals.AddReferralsRow(
                0,
                0,
                0,
                0);

            return referrals;
        }

        private object GetPhysiotherapyServiceSampleData(DateTime? FromDate, DateTime? ToDate)
        {

            var dataSet = new Caresoft2._0.CrystalReports.MOH711.K.Physiotherapy_Service();

            dataSet.PysiotherapyService.AddPysiotherapyServiceRow(
                0, 
                0,
                0,
                0,
                0,
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0,
                0, 
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0,
                0);

            return dataSet;

        }

        private object GetMSocialWorkJSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            MSocialWorkJ mSocialWorkJ = new MSocialWorkJ();
            var dataset = new Caresoft2._0.CrystalReports.MOH711.J.MSocialworkJ();
            dataset.MSocialWorkJ.AddMSocialWorkJRow(
                0,
                0,
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0);

            return dataset;

        }

        private object GetRehabServicesISampleData(DateTime? FromDate, DateTime? ToDate)
        {
            RehabServicesI rehabServicesI = new RehabServicesI();
            rehabServicesI.RehabI.AddRehabIRow(0, 0, 0, 0, 0);
            return rehabServicesI;
        }

        private object GetPNCHSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            PNCH pNCH = new PNCH();

            var TotalBreastExam = db2.MCHPhysicalExaminations.Count(e => e.Breasts != null &&  e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var WomenCounselled = db2.MCHMotherPostNatalExams.Count(e=>e.FamilyPlanning !=null && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FromOtherHealthFacility = db2.MCHCancerScreenings.Count(e=>e.Referral=="From Other Health Facility" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var FromCommunityUnit= db2.MCHCancerScreenings.Count(e=>e.Referral=="From Community Unit" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ToOtherHealthFacility = db2.MCHCancerScreenings.Count (e=>e.Referral=="To Other Health Facility" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            var ToCommunityUnit = db2.MCHCancerScreenings.Count(e => e.Referral == "To Community Unit" && e.DateAdded >= FromDate && e.DateAdded <= ToDate);


            pNCH.PNC.AddPNCRow(
                TotalBreastExam,
                WomenCounselled,
                0,
                0,
                0, 
                0,
                0,
                0,
                0, 
                0,
                FromOtherHealthFacility,
                FromCommunityUnit,
                ToOtherHealthFacility,
                ToCommunityUnit);

            return pNCH;
        }
        public class Cervical
        {
            public int LessThan25Years { get; set; }    
            public int TwentyFiveTo49Years {get;set;}
            public int FiftyYearsPlus { get; set; }
    }

        private object GetCervicalSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            Caresoft2._0.CrystalReports.MOH711.G.DataSet1 dataSet = new CrystalReports.MOH711.G.DataSet1();

            var ClientsReceivingVIA25 = db2.MCHCancerScreenings.Where(e =>e.CervixTreatment != null && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year< 25 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ClientsReceivingVIA25To49 = db2.MCHCancerScreenings.Where(e => e.CervixTreatment != null && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year <= 49 && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year >= 25 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ClientsReceivingVIA50Plus = db2.MCHCancerScreenings.Where(e => e.CervixTreatment != null && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ScreenedForPapSmear25 = db2.MCHCancerScreenings.Where(e => e.PapSmearTestResult != null && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year < 25 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ScreenedForPapSmear25To49 = db2.MCHCancerScreenings.Where(e => e.PapSmearTestResult != null && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year <= 49 && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year >= 25 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ScreenedForPapSmear50Plus = db2.MCHCancerScreenings.Where(e => e.PapSmearTestResult != null && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ClientsWithPositiveVIAResult25 = db2.MCHCancerScreenings.Where(e => e.VIATestResult =="Positive" && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year < 25 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ClientsWithPositiveVIAResult25To49 = db2.MCHCancerScreenings.Where(e => e.VIATestResult =="Positive" && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year <= 49 && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year >= 25 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();
            var ClientsWithPositiveVIAResult50Plus = db2.MCHCancerScreenings.Where(e => e.VIATestResult== "Positive" && DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year >= 50 && e.DateAdded >= FromDate && e.DateAdded <= ToDate).Count();


            dataSet.CervicalCancer_G.AddCervicalCancer_GRow(
                ClientsReceivingVIA25,
                ClientsReceivingVIA25To49,
                ClientsReceivingVIA50Plus,
                ScreenedForPapSmear25,
                ScreenedForPapSmear25To49,
                ScreenedForPapSmear50Plus,
                ClientsWithPositiveVIAResult25,
                ClientsWithPositiveVIAResult25To49,
                ClientsWithPositiveVIAResult50Plus, 
                0, 
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0,
                0, 
                0,
                0, 
                0, 
                0, 
                0, 
                0);

            return dataSet;
        }

        public class CHANIS
        {
            public int Female { get; set; }
            public int Male { get; set; }
            public int Total { get; set; }
        }

        private object GetDatasetFSampleData(DateTime? FromDate, DateTime? ToDate)
        {

            DataSetF datasetF = new DataSetF();

            
            datasetF.F.AddFRow(0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0);
            return datasetF;
        }

        private object GetDatasetESampleData(DateTime? FromDate, DateTime? ToDate)
        {
            DatasetE datasetE = new DatasetE();
            datasetE._DatasetE.AddDatasetERow(0, 0);
            return datasetE;

        }

        private object GetFPSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            FP datasetFP = new FP();
           
           // var NumberOfClientsNew = db2.MCHFamilyPlanings.Count(e => e.OpdRegister.Patient.Equals(1) && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            //var NumberOfClientsRevisit = db2.MCHFamilyPlanings.Count(e => e.OpdRegister.Patient && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            //var Adolescents10To14New= db2.MCHFamilyPlanings.Count(e=>e.OpdRegister.Patient.Equals(1) && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 14 && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 10 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            //var Adolescents15To19New= db2.MCHFamilyPlanings.Count(e => e.OpdRegister.Patient.Equals(1) && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 19 && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 15 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);
            //var YouthClients20To24New= db2.MCHMaternalProfiles.Count(e =>e.OpdRegister.Patient.Equals(1)  && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) < 24 && (DateTime.Now.Year - e.OpdRegister.Patient.DOB.Value.Year) >= 20 && e.DateAdded >= FromDate && e.DateAdded <= ToDate);

            datasetFP.FamilyPlanningD.AddFamilyPlanningDRow(
                0,
                0,
                0,
                0,
                0, 
                0,
                0, 
                0,
                0, 
                0, 
                0,
                0, 
                0,
                0,
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0, 
                0,
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                0);
            return datasetFP;
        }

        private object GetSGBVCCCSampleData(DateTime? FromDate, DateTime? ToDate)
        {
            SGBVCCC datasetSGBVCCC = new SGBVCCC();
            datasetSGBVCCC.SGBV.AddSGBVRow(
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0, 
                0);
            return datasetSGBVCCC;
        }
        
        
        private ActionResult DataSet(DateTime? FromDate, DateTime? ToDate)
        {
            //throw new NotImplementedException();
            return null;
        }
        #endregion
        
    }

}
       
    



