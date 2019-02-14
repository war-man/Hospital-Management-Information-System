using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers.Temp
{
    [Auth]
    public class SystemExamParticularsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: SystemExamParticulars
        public ActionResult Index()
        {
            var systemExamParticulars = db.SystemExamParticulars.Include(s => s.SystemExamCategory);
            return View(systemExamParticulars.ToList());
        }

        // GET: SystemExamParticulars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemExamParticular systemExamParticular = db.SystemExamParticulars.Find(id);
            if (systemExamParticular == null)
            {
                return HttpNotFound();
            }
            return View(systemExamParticular);
        }

        // GET: SystemExamParticulars/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.SystemExamCategories, "Id", "CategoryName");
            return View();
        }

        // POST: SystemExamParticulars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,ParticularName")] SystemExamParticular systemExamParticular)
        {
            if (ModelState.IsValid)
            {
                
                systemExamParticular.BranchId = (int)Session["UserBranchId"] ;
                systemExamParticular.UserId = int.Parse(Session["UserId"].ToString());
                systemExamParticular.TimeAdded = DateTime.Now;
                db.SystemExamParticulars.Add(systemExamParticular);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.SystemExamCategories, "Id", "CategoryName", systemExamParticular.CategoryId);
            return View(systemExamParticular);
        }

        // GET: SystemExamParticulars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemExamParticular systemExamParticular = db.SystemExamParticulars.Find(id);
            if (systemExamParticular == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.SystemExamCategories, "Id", "CategoryName", systemExamParticular.CategoryId);
            return View(systemExamParticular);
        }

        // POST: SystemExamParticulars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,ParticularName,BranchId,UserId,TimeAdded")] SystemExamParticular systemExamParticular)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemExamParticular).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.SystemExamCategories, "Id", "CategoryName", systemExamParticular.CategoryId);
            return View(systemExamParticular);
        }

        // GET: SystemExamParticulars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemExamParticular systemExamParticular = db.SystemExamParticulars.Find(id);
            if (systemExamParticular == null)
            {
                return HttpNotFound();
            }
            return View(systemExamParticular);
        }

        // POST: SystemExamParticulars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemExamParticular systemExamParticular = db.SystemExamParticulars.Find(id);
            db.SystemExamParticulars.Remove(systemExamParticular);
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
