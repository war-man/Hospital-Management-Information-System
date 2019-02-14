using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Procurement.Controllers
{
    [Auth]
    public class ReportsController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();

        // GET: Procurement/SupplierSummary
        public ActionResult SupplierSummary(DateTime StartDate, DateTime EndDate, int? Supplier)
        {
            DateTime _EndDate = EndDate.AddDays(1);
            ViewBag.Suppliers = db.supplier;
            var invoices = db.Invoice.Where(e => e.InvoiceDate >= StartDate && e.InvoiceDate <= _EndDate).ToList();

            if(Supplier != null && Supplier > 0)
            invoices = invoices.Where(e => e.SupplierId == Supplier).ToList();

            return View(invoices);
        }

        public ActionResult SuppliersStatement()
        {
            return View(db.Invoice.ToList());
        }
    }
}