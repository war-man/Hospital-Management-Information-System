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
    public class FamilyMedicalHistoryQuestionsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: FamilyMedicalHistoryQuestions
        public ActionResult Index()
        {
            return View(db.FamilyMedicalHistoryQuestions.ToList());
        }

        // GET: FamilyMedicalHistoryQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMedicalHistoryQuestion familyMedicalHistoryQuestion = db.FamilyMedicalHistoryQuestions.Find(id);
            if (familyMedicalHistoryQuestion == null)
            {
                return HttpNotFound();
            }
            return View(familyMedicalHistoryQuestion);
        }

        // GET: FamilyMedicalHistoryQuestions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FamilyMedicalHistoryQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FamilyMedicalHistoryQuestionName")] FamilyMedicalHistoryQuestion familyMedicalHistoryQuestion)
        {
            if (ModelState.IsValid)
            {
                
                familyMedicalHistoryQuestion.BranchId = (int)Session["UserBranchId"] ;
                familyMedicalHistoryQuestion.UserId = int.Parse(Session["UserId"].ToString());

                familyMedicalHistoryQuestion.TimeAdded = DateTime.Now;
                db.FamilyMedicalHistoryQuestions.Add(familyMedicalHistoryQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(familyMedicalHistoryQuestion);
        }

        // GET: FamilyMedicalHistoryQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMedicalHistoryQuestion familyMedicalHistoryQuestion = db.FamilyMedicalHistoryQuestions.Find(id);
            if (familyMedicalHistoryQuestion == null)
            {
                return HttpNotFound();
            }
            return View(familyMedicalHistoryQuestion);
        }

        // POST: FamilyMedicalHistoryQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FamilyMedicalHistoryQuestionName,BranchId,UserId,TimeAdded")] FamilyMedicalHistoryQuestion familyMedicalHistoryQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(familyMedicalHistoryQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(familyMedicalHistoryQuestion);
        }

        // GET: FamilyMedicalHistoryQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMedicalHistoryQuestion familyMedicalHistoryQuestion = db.FamilyMedicalHistoryQuestions.Find(id);
            if (familyMedicalHistoryQuestion == null)
            {
                return HttpNotFound();
            }
            return View(familyMedicalHistoryQuestion);
        }

        // POST: FamilyMedicalHistoryQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FamilyMedicalHistoryQuestion familyMedicalHistoryQuestion = db.FamilyMedicalHistoryQuestions.Find(id);
            db.FamilyMedicalHistoryQuestions.Remove(familyMedicalHistoryQuestion);
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
