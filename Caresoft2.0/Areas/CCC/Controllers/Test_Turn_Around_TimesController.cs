using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LabsDataAccess;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class Test_Turn_Around_TimesController : Controller
    {
        private CareSoftLabsEntities db = new CareSoftLabsEntities();

        // GET: HIV/Test_Turn_Around_Times
        public ActionResult Index()
        {
            return View(db.Test_Turn_Around_Times.ToList());
        }

        // GET: HIV/Test_Turn_Around_Times/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test_Turn_Around_Times test_Turn_Around_Times = db.Test_Turn_Around_Times.Find(id);
            if (test_Turn_Around_Times == null)
            {
                return HttpNotFound();
            }
            return View(test_Turn_Around_Times);
        }
        // GET: HIV/Test_Turn_Around_Times/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: HIV/Test_Turn_Around_Times/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Test,TTAT_Days,TTAT_Hours,RDD_Days,RDD_Hours,Vial_Type,srno,srno1,CreatedUtc,DepartmentRadPath,BranchId")] Test_Turn_Around_Times test_Turn_Around_Times)
        {
            if (ModelState.IsValid)
            {
                db.Test_Turn_Around_Times.Add(test_Turn_Around_Times);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(test_Turn_Around_Times);
        }

        // GET: HIV/Test_Turn_Around_Times/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test_Turn_Around_Times test_Turn_Around_Times = db.Test_Turn_Around_Times.Find(id);
            if (test_Turn_Around_Times == null)
            {
                return HttpNotFound();
            }
            return View(test_Turn_Around_Times);
        }

        // POST: HIV/Test_Turn_Around_Times/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Test,TTAT_Days,TTAT_Hours,RDD_Days,RDD_Hours,Vial_Type,srno,srno1,CreatedUtc,DepartmentRadPath,BranchId")] Test_Turn_Around_Times test_Turn_Around_Times)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test_Turn_Around_Times).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test_Turn_Around_Times);
        }

        // GET: HIV/Test_Turn_Around_Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test_Turn_Around_Times test_Turn_Around_Times = db.Test_Turn_Around_Times.Find(id);
            if (test_Turn_Around_Times == null)
            {
                return HttpNotFound();
            }
            return View(test_Turn_Around_Times);
        }

        // POST: HIV/Test_Turn_Around_Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Test_Turn_Around_Times test_Turn_Around_Times = db.Test_Turn_Around_Times.Find(id);
            db.Test_Turn_Around_Times.Remove(test_Turn_Around_Times);
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
