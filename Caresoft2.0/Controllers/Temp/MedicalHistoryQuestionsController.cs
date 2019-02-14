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
    public class MedicalHistoryQuestionsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: MedicalHistoryQuestions
        public ActionResult Index()
        {
            return View(db.MedicalHistoryQuestions.ToList());
        }

        // GET: MedicalHistoryQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistoryQuestion medicalHistoryQuestion = db.MedicalHistoryQuestions.Find(id);
            if (medicalHistoryQuestion == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistoryQuestion);
        }

        // GET: MedicalHistoryQuestions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalHistoryQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MedicalHistoryQuestionName")] MedicalHistoryQuestion medicalHistoryQuestion)
        {
            if (ModelState.IsValid)
            {
                
                medicalHistoryQuestion.BranchId = (int)Session["UserBranchId"] ;
                medicalHistoryQuestion.UserId = int.Parse(Session["UserId"].ToString());
                medicalHistoryQuestion.TimeAdded = DateTime.Now;
                db.MedicalHistoryQuestions.Add(medicalHistoryQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicalHistoryQuestion);
        }

        // GET: MedicalHistoryQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistoryQuestion medicalHistoryQuestion = db.MedicalHistoryQuestions.Find(id);
            if (medicalHistoryQuestion == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistoryQuestion);
        }

        // POST: MedicalHistoryQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MedicalHistoryQuestionName,BranchId,UserId,TimeAdded")] MedicalHistoryQuestion medicalHistoryQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalHistoryQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicalHistoryQuestion);
        }

        // GET: MedicalHistoryQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistoryQuestion medicalHistoryQuestion = db.MedicalHistoryQuestions.Find(id);
            if (medicalHistoryQuestion == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistoryQuestion);
        }

        // POST: MedicalHistoryQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalHistoryQuestion medicalHistoryQuestion = db.MedicalHistoryQuestions.Find(id);
            db.MedicalHistoryQuestions.Remove(medicalHistoryQuestion);
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
