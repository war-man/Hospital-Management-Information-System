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
    //public class BillParticularsController : Controller
    //{
    //    private CaresoftHMISEntities db = new CaresoftHMISEntities();

    //    // GET: BillParticulars
    //    public ActionResult Index()
    //    {
    //        return View(db.BillParticulars.ToList());
    //    }

    //    // GET: BillParticulars/Details/5
    //    public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        BillParticular billParticular = db.BillParticulars.Find(id);
    //        if (billParticular == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(billParticular);
    //    }

    //    // GET: BillParticulars/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: BillParticulars/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create([Bind(Include = "Id,Groups,Status,Particular,BillParticularOrderNumber,HCPCS,OPD,IPD,ICD10,CPTCode,DoctorWise,ConsultantWise,Daily")] BillParticular billParticular)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.BillParticulars.Add(billParticular);
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        return View(billParticular);
    //    }

    //    // GET: BillParticulars/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        BillParticular billParticular = db.BillParticulars.Find(id);
    //        if (billParticular == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(billParticular);
    //    }

    //    // POST: BillParticulars/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "Id,Groups,Status,Particular,BillParticularOrderNumber,HCPCS,OPD,IPD,ICD10,CPTCode,DoctorWise,ConsultantWise,Daily")] BillParticular billParticular)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(billParticular).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(billParticular);
    //    }

    //    // GET: BillParticulars/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        BillParticular billParticular = db.BillParticulars.Find(id);
    //        if (billParticular == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(billParticular);
    //    }

    //    // POST: BillParticulars/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        BillParticular billParticular = db.BillParticulars.Find(id);
    //        db.BillParticulars.Remove(billParticular);
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
