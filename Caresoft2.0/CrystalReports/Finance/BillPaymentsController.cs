using Caresoft2._0.CrystalReports.Finance;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using CaresoftHMISDataAccess;
using System.Data.Entity;
using Caresoft2._0.CrystalReports;

namespace Caresoft2._0.Controllers
{
    public class BillPaymentsController : Controller
    {
        private object dataSet;

        
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        public int Finance { get; private set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateReport()
              
        {
            CaresoftHMISEntities db = new CaresoftHMISEntities();
            

        
            ReportDocument rd = new ReportDocument();


            rd.Load(Path.Combine(Server.MapPath(@"~\Caresoft2.0\CrystalReports\Finance\BillPyments.rpt")));

            rd.SetDataSource(dataSet);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());




            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Report Sample - " + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }

        private object GetBillPymentsReport()
        {
            throw new NotImplementedException();
        }
        public class DepartmentReportViewModel
        {
            public string DepartmentName { get; set; }
            public double Amount { get; set; }
        }
        
        public ActionResult GetMonthlyDepartmentReport(DateTime FromDate,DateTime ToDate)
        {
           
           var data = db.BillServices.Where(p => p.Paid == true && p.PaidDate >= FromDate && p.PaidDate<=ToDate).ToList();

            
            var allDepartments = db.Departments.ToList();

            
            var CrystalReportData = new List<DepartmentReportViewModel>();

            
            foreach(var item in allDepartments)
            {
                var singleDept = new DepartmentReportViewModel();
                singleDept.DepartmentName = item.DepartmentName;
                singleDept.Amount = data.Where(p => p.DepartmentId == item.Id).Sum(x => x.Quatity * x.Price);

                CrystalReportData.Add(singleDept);
            }

            BillPaymentsDataSet billPayments = new BillPaymentsDataSet();

            foreach (var item in CrystalReportData)
            {
                billPayments.BillPayments.AddBillPaymentsRow(item.DepartmentName, item.Amount);
            }

            
          
            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/Finance/BillPyments.rpt")));

            rd.SetDataSource(billPayments);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetParameterValue("fromDate", FromDate);
            rd.SetParameterValue("toDate", ToDate);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = " - " + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);

        }
   
     



    }
}
    
