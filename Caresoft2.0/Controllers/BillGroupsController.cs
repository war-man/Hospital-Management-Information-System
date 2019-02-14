using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    //[Auth]
    //public class BillGroupsController : Controller
    //{
    //    private CaresoftHMISEntities db = new CaresoftHMISEntities();

    //    // GET: BillGroups
    //    public ActionResult Index()
    //    {
    //        return View(db.BillGroups.ToList());
    //    }

    //    // GET: BillGroups/Details/5
    //    public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        BillGroup billGroup = db.BillGroups.Find(id);
    //        if (billGroup == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(billGroup);
    //    }

    //    // GET: BillGroups/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: BillGroups/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //    //[HttpPost]
    //    //[ValidateAntiForgeryToken]
    //    //public ActionResult Create([Bind(Include = "Id,GroupName,GroupOrderNo,Dental,IsRegistration,HealthPackage,IsProcedure")] BillGroup billGroup)
    //    //{
    //    //    if (ModelState.IsValid)
    //    //    {
    //    //        db.BillGroups.Add(billGroup);
    //    //        db.SaveChanges();
    //    //        return RedirectToAction("Index");
    //    //    }

    //    //    return View(billGroup);
    //    //}

    //    // GET: BillGroups/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        BillGroup billGroup = db.BillGroups.Find(id);
    //        if (billGroup == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(billGroup);
    //    }

    //    // POST: BillGroups/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "Id,GroupName,GroupOrderNo,Dental,IsRegistration,HealthPackage,IsProcedure")] BillGroup billGroup)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(billGroup).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(billGroup);
    //    }

    //    // GET: BillGroups/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        BillGroup billGroup = db.BillGroups.Find(id);
    //        if (billGroup == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(billGroup);
    //    }

    //    // POST: BillGroups/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        BillGroup billGroup = db.BillGroups.Find(id);
    //        db.BillGroups.Remove(billGroup);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}
}
