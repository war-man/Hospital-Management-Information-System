using Caresoft2._0.Areas.CareSoftReports.Reports.StatementPerScheme.StatementOfAccountPerDepartment;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.CareSoftReports.Controllers
{
    [Auth]
    public class AccountPerDepartmentController : Controller
    {
        // GET: CareSoftReports/AccountPerDepartment
        CaresoftHMISEntities db = new CaresoftHMISEntities();
        private object statementOfAccount;

        public ActionResult Index()
        {
            return View();
        }
        public class StatementOfAccount
        {
            public string NameOfDepartment { get; set; }
            public int[] DaysOfTheMonth { get; set; }
        }
        //public ActionResult AccountPerDepartmentReport(DateTime FromDate, DateTime ToDate, string Department, string Format, int Year, int selectMonth)
        //{
        //    var newStatementOfAccount = GetAccountsPerDepartmentReport(FromDate, ToDate, Department, Year, selectMonth);

        //    var insuranceCompanies = db.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim() == "insurance").ToList();
        //    return View(insuranceCompanies);
        //}

        //private object GetAccountsPerDepartmentReport(DateTime fromDate, DateTime toDate, string department, int year, int selectMonth)
        //{
        //    throw new NotImplementedException();
        //}

        public ActionResult GetAccountsPerDepartmentReport(DateTime? FromDate, DateTime ?ToDate, string Department, string Format, int Year, int selectMonth)
        {
            var insuranceCompanies = db.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim() == "insurance").ToList();
            var amount = db.BillServices.Where(p => p.DateAdded.Year == Year && p.DateAdded.Month == selectMonth).ToList();
            var allDepartments = db.Departments.ToList();
            var CrystalReportData = new List<StatementOfAccount>();

            var NumberOfDays = DateTime.DaysInMonth(Year, selectMonth);

            foreach (var item in allDepartments)
            {
                StatementOfAccount statementOfAccount = new StatementOfAccount()
                {
                    NameOfDepartment = item.DepartmentName,
                    DaysOfTheMonth = new int[31]
                };
                int[] AllDaysOfTheMonth = new int[31];
                for (int i = 1; i <= NumberOfDays; i++)
                {
                    var theDay = i;
                    var AmountInDay = allDepartments.Where(p => p.DepartmentName.Contains(item.DepartmentName) && p.DateAdded.Day == theDay).Count();
                    AllDaysOfTheMonth[i - 1] = AmountInDay;
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
                    statementOfAccount.DaysOfTheMonth = AllDaysOfTheMonth.ToArray();
                }
                CrystalReportData.Add(statementOfAccount);

            }
            NewStatementOfAccount newStatementOfAccount = new NewStatementOfAccount();
            foreach (var item in CrystalReportData)
            {
                newStatementOfAccount.NewAccountPerDept.AddNewAccountPerDeptRow(
                    item.NameOfDepartment,
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
                    item.DaysOfTheMonth[10],
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
                    item.DaysOfTheMonth[30]
                     );
                newStatementOfAccount.NewAccountPerDept.AddNewAccountPerDeptRow("", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
            return View(insuranceCompanies);

        }
        
       
    }
}
        

                    

            


        

    
   
