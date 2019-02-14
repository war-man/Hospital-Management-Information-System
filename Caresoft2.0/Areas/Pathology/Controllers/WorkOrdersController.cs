using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LabsDataAccess;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Areas.Pathology.Controllers
{
    [Auth]
    public class WorkOrdersController : Controller
    {
        private CareSoftLabsEntities db = new CareSoftLabsEntities();
        private CaresoftHMISEntities db_main = new CaresoftHMISEntities();
        int main_department_id = new CaresoftHMISEntities().Departments.FirstOrDefault(d => d.DepartmentName.Equals("PATHOLOGY")).Id;

        // GET: Pathology/WorkOrders
        public ActionResult Index(int id)
        {
            ViewBag.main_department_id = main_department_id;

            var workOrder = db.WorkOrders.Find(id);
            ViewBag.OPD = db_main.OpdRegisters.Find(workOrder.OPDNo);
            return View(workOrder);
        }

        public ActionResult LabResultsAll()
        {
            var workOrders = db.WorkOrders.Include(w => w.Status);
            return View(workOrders.ToList());
        }

        public ActionResult LabResults(int id)
        {
            ViewBag.main_department_id = main_department_id;

            var workOrder = db.WorkOrders.Find(id);
            ViewBag.OPD = db_main.OpdRegisters.Find(workOrder.OPDNo);
            ViewBag.Users = db_main.Users;

            return PartialView(workOrder);
        }

        public ActionResult OpdLabResults(int id)
        {
            var workOrder = db.WorkOrders.Where(e => e.OPDNo == id).ToList();
            ViewBag.OPD = db_main.OpdRegisters.Find(id);

            return PartialView(workOrder);
        }

        // GET: Pathology/WorkOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = db.WorkOrders.Find(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }

        // GET: Pathology/WorkOrders/Create
        public ActionResult Create()
        {
            ViewBag.Accession_Status = new SelectList(db.Status, "Id", "StatusValue");
            return View();
        }

        // POST: Pathology/WorkOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OPDNo,OPDType,BillPaid,Doctor,LabNo,PathNo,ShowInSpecimentResultEnty,ShowInSpecimentCollection,Accession_Status,Financial_Year,CreatedUtc,DepartmentRadPath")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                db.WorkOrders.Add(workOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Accession_Status = new SelectList(db.Status, "Id", "StatusValue", workOrder.Accession_Status);
            return View(workOrder);
        }

        // GET: Pathology/WorkOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = db.WorkOrders.Find(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.Accession_Status = new SelectList(db.Status, "Id", "StatusValue", workOrder.Accession_Status);
            return View(workOrder);
        }

        // POST: Pathology/WorkOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OPDNo,OPDType,BillPaid,Doctor,LabNo,PathNo,ShowInSpecimentResultEnty,ShowInSpecimentCollection,Accession_Status,Financial_Year,CreatedUtc,DepartmentRadPath")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Accession_Status = new SelectList(db.Status, "Id", "StatusValue", workOrder.Accession_Status);
            return View(workOrder);
        }

        // GET: Pathology/WorkOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = db.WorkOrders.Find(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }

        // POST: Pathology/WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkOrder workOrder = db.WorkOrders.Find(id);
            db.WorkOrders.Remove(workOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
