using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using System.Data.Entity.Validation;
using Caresoft2._0.CustomData;

namespace Caresoft2._0.Controllers.Temp
{
    [Auth]
    public class TariffsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Tariffs
        public ActionResult Index()
        {
            var tariffs = db.Tariffs.Include(t => t.Company);
            return View(tariffs.ToList());
        }

        // GET: Tariffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tariff tariff = db.Tariffs.Find(id);
            if (tariff == null)
            {
                return HttpNotFound();
            }
            return View(tariff);
        }

        // GET: Tariffs/Create
        public ActionResult Create()
        {
            //ViewBag.CompanyId = new SelectList(db.Companies, "Id", "CompanyName");
            TariffData tariffData = new TariffData();
            tariffData.Tariffs = db.Tariffs.ToList();
            tariffData.CompaniesList = db.Companies.ToList();
            return View(tariffData);
        }

        // POST: Tariffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TariffName,CompanyId,DateAdded")] Tariff tariff)
            
        {
            tariff.DateAdded = DateTime.Now;
        
            //if (ModelState.IsValid)
            //{
                
            //}

            try
            {
                db.Tariffs.Add(tariff);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);


                var fullErrorMessage = string.Join("; ", errorMessages);


                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


                return Content(exceptionMessage);
            }

            //ViewBag.CompanyId = new SelectList(db.Companies, "Id", "CompanyName", tariff.CompanyId);
            //return Json(tariff);
        }

        // GET: Tariffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tariff tariff = db.Tariffs.Find(id);
            if (tariff == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "CompanyName", tariff.CompanyId);
            return View(tariff);
        }

        // POST: Tariffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TariffName,CompanyId,DateAdded")] Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tariff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "CompanyName", tariff.CompanyId);
            return View(tariff);
        }

        // GET: Tariffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tariff tariff = db.Tariffs.Find(id);
            if (tariff == null)
            {
                return HttpNotFound();
            }
            return View(tariff);
        }

        // POST: Tariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tariff tariff = db.Tariffs.Find(id);
            db.Tariffs.Remove(tariff);
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
