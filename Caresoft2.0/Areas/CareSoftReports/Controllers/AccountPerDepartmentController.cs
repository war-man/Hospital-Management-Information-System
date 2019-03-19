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

            ViewBag.Insurance = db.Companies.ToList();
            return View();
        }
        public class StatementOfAccountViewModel
        {
            public string NameOfDepartment { get; set; }
            public int[] DaysOfTheMonth { get; set; }
            public int? Month { get; set; }
            public int Year { get; set; }
        }
        

        public ActionResult StatementOfAccountReport()
        {
            var insuranceCompanies = db.Companies.Where(p => p.CompanyType.CompanyTypeName.ToLower().Trim() == "insurance").ToList();
            return View(insuranceCompanies);
        }


        public ActionResult GetAccountsPerDepartmentReport( int Year, int SelectMonth,string Department)
        {
          
            var amount = db.BillServices.Where(p => p.DateAdded.Year == Year && p.DateAdded.Month == SelectMonth).ToList();
            var allDepartments = db.Departments.ToList();
            var CrystalReportData = new List<StatementOfAccountViewModel>();

            // var NumberOfDays = DateTime.DaysInMonth(Month,Year);
            var NumberOfDays = DateTime.DaysInMonth(Year, SelectMonth);
            

            foreach (var item in allDepartments)
            {
                StatementOfAccountViewModel statementOfAccount = new StatementOfAccountViewModel()
                {
                    NameOfDepartment = item.DepartmentName,
                    DaysOfTheMonth = new int[31],
                    
                };
                int[] AllDaysOfTheMonth = new int[31];
                for (int i = 1; i <= NumberOfDays; i++)
                {
                    var theDay = i;

                    //var AmountInDay=amount.Where(p=>p.Award.Equals(amount)&& p.DateAdded.Day==theDay).Count();
                    var AmountInDay = amount.Where(p => p.DepartmentId == item.Id && p.DateAdded.Day == theDay && p.DateAdded.Month == SelectMonth
                     && p.DateAdded.Year == Year).Sum(p => (p.Price * p.Quatity));
                  
                    AllDaysOfTheMonth[i - 1] = Convert.ToInt32(AmountInDay);
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
                    item.DaysOfTheMonth[30],
                    item.Month.ToString(),
                    item.Year.ToString()
                    );

               
            }

            ReportDocument Rd = new ReportDocument();
            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/StatementPerScheme/StatementOfAccountPerDepartment/StatementOfAccount.rpt")));
            Rd.SetDataSource(newStatementOfAccount);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = "Statement of Account per Department Report" + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf";

            return File(Stream, "application/pdf", FileName);

        }
         

     }
         
       
    }

        

                    

            


        

    
   
