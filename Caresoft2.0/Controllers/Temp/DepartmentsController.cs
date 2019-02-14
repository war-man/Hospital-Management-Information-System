using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers.Temp
{
    [Auth]
    public class DepartmentsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Departments
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.DepartmentType1);
            return View(departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }


        public ActionResult GetThisDepartmentDoctors(int id)
        {
            var emps = db.Departments.Find(id).Employees.Where(e => e.Users.Count() > 0).ToList();
            List<Object> doctors = new List<object>();
            
            foreach(var emp in emps)
            {
                var entry = new
                {
                    Name = emp.Salutation + " " + emp.FName + " " + " " + emp.OtherName,
                    Usernam = emp.Users.FirstOrDefault().Username,
                    UserId = emp.Users.FirstOrDefault().Id,
                };

                doctors.Add(entry);
            }
           
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentType = new SelectList(db.DepartmentTypes, "Id", "DepartmnetType");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            department.DateAdded = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentType = new SelectList(db.DepartmentTypes, "Id", "DepartmnetType", department.DepartmentType);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentType = new SelectList(db.DepartmentTypes, "Id", "DepartmnetType", department.DepartmentType);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DepartmentName,DateAdded,DepartmentType")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentType = new SelectList(db.DepartmentTypes, "Id", "DepartmnetType", department.DepartmentType);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
