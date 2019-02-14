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
    public class SystemExamCategoriesController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: SystemExamCategories
        public ActionResult Index()
        {
            return View(db.SystemExamCategories.ToList());
        }

        // GET: SystemExamCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemExamCategory systemExamCategory = db.SystemExamCategories.Find(id);
            if (systemExamCategory == null)
            {
                return HttpNotFound();
            }
            return View(systemExamCategory);
        }

        // GET: SystemExamCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemExamCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryName")] SystemExamCategory systemExamCategory)
        {
            if (ModelState.IsValid)
            {
                systemExamCategory.TimeAdded = DateTime.Now;
                systemExamCategory.UserId = int.Parse(Session["UserId"].ToString());
                
                systemExamCategory.BranchId = (int)Session["UserBranchId"] ;
                db.SystemExamCategories.Add(systemExamCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(systemExamCategory);
        }

        // GET: SystemExamCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemExamCategory systemExamCategory = db.SystemExamCategories.Find(id);
            if (systemExamCategory == null)
            {
                return HttpNotFound();
            }
            return View(systemExamCategory);
        }

        // POST: SystemExamCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryName,UserId,BranchId,TimeAdded")] SystemExamCategory systemExamCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemExamCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(systemExamCategory);
        }

        // GET: SystemExamCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemExamCategory systemExamCategory = db.SystemExamCategories.Find(id);
            if (systemExamCategory == null)
            {
                return HttpNotFound();
            }
            return View(systemExamCategory);
        }

        // POST: SystemExamCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemExamCategory systemExamCategory = db.SystemExamCategories.Find(id);
            db.SystemExamCategories.Remove(systemExamCategory);
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
