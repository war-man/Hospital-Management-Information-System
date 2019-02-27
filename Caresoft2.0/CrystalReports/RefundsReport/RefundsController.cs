using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.RefundsReport
{
    [Auth]
    public class RefundsController : Controller
    {
        // GET: Refunds
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private DateTime fromDate;
        private DateTime toDate;

        public ActionResult Index()
        {
            return View();
        }
        public class RefundsViewModel
        {
            public string PatientName { get; set; }
            public string DateOfRefund { get; set; }
            public string ReceiptNo { get; set; }
            public double RefundedAmount { get; set; }
            public string RefundedItemsCount { get; set; }
            public string ReasonForRefund { get; set; }
        }
        public ActionResult GenerateReport(DateTime FromDate, DateTime ToDate)

        {
            CaresoftHMISEntities db = new CaresoftHMISEntities();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/RefundsReport/Refunds.rpt")));


            rd.SetDataSource(NewRefunds);
            rd.Subreports["RptReportHeader.rpt"].SetDataSource(CrystalReports.HeaderAndFooterForReports.GetAllReportHeader());
            rd.SetParameterValue("fromDate", FromDate);
            rd.SetParameterValue("toDate", ToDate);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = " RefundsReport" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }
        private object NewRefunds
        {
            get
            {
                var AllRefunds = db.Refunds.Where(p => p.DateAdded >= fromDate && p.DateAdded <= toDate).ToList();
                var TotalRefunds = new Caresoft2._0.CrystalReports.RefundsReport.NewRefunds();
                var lstAllRefunds = db.Refunds.Where(p => p.ReceiptNo !=0).ToList();
                
                foreach (var item in lstAllRefunds)
                {
                    var pt = db.Patients.FirstOrDefault(e => e.Id !=0);

                    TotalRefunds._NewRefunds.AddNewRefundsRow(pt.FName + "" + pt.LName, item.DateAdded.ToString(), item.ReceiptNo.ToString(), item.RefundedAmount.ToString(),
                                                              item.RefundedItemsCount.ToString(), item.ReasonForRefund.ToString());
                }
                return TotalRefunds;
            }

        }

 
    }
}
