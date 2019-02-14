using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using Caresoft2._0.CustomData;
using PagedList;

namespace Caresoft2._0.Controllers.Temp
{
    //[Auth]
    public class ServicesController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Services
        public ActionResult Index()
        {
            
            return PartialView(db.Services.OrderByDescending(e => e.Id).ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create(int? searchId, int? page = 1)
        {
            new Seeder().SeedServicesMetaData();
            
            var data = new ServicesMasterData();
            data.ServiceGroups = db.ServiceGroups.ToList();
            data.Services = db.Services.OrderBy(e => e.ServiceName).ToPagedList((int)page, 10);

            if (searchId != null)
            {
                data.Services = db.Services.OrderBy(e => e.ServiceName).Where(e => e.Id == searchId).ToPagedList((int)page, 10);
            }
            data.Departments = db.Departments.Where(e=>e.DepartmentType1.DepartmnetType.ToLower() == "revenue").ToList();
            return View(data);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ServiceName,DepartmentId,CashPrice, ServiceGroupId, IsIPD, IsOPD")] Service service)
        {  service.DateAdded = DateTime.Now;
            db.Services.Add(service);
            db.SaveChanges();
            return Redirect("Index");
            
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower() == "revenue"), "Id", "DepartmentName", service.DepartmentId);
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups, "Id", "ServiceGroupName", service.ServiceGroupId);
            return View(service);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ServiceName,DepartmentId,CashPrice,DateAdded,ServiceGroupId")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower() == "revenue"), "Id", "DepartmentName", service.DepartmentId);
            ViewBag.ServiceGroupId = new SelectList(db.ServiceGroups, "Id", "ServiceGroupName", service.ServiceGroupId);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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

        public ActionResult Procedures(int id)
        {
            var department = db.Departments.Find(id);
            var procedures = department.Services.Where
                (e => e.ServiceGroup.ServiceGroupName.Trim().ToLower().Equals("procedure"))
                .Select(x=>new { ProcedureId = x.Id, ProcedureName = x.ServiceName})
                .ToList();

            return Json(procedures, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ActivateService(int id, int page)
        {
            var service = db.Services.Find(id);
            service.IsActive = !service.IsActive;
            db.SaveChanges();
            return RedirectToAction("Create", new { page = page });
        }

        public ActionResult ActivateUnder5(int id, int page)
        {
            var service = db.Services.Find(id);
            service.IsUnder5 = !service.IsUnder5;
            db.SaveChanges();

            return RedirectToAction("Create", new { page = page });
        }

        public ActionResult SearchServices(string search)
        {
            var services = db.Services.Where(e => e.ServiceName.ToLower().Contains(search))
                .Select(x => new
                {
                    x.ServiceName,
                    x.Id
                }).ToList();
           
            return Json(services, JsonRequestBehavior.AllowGet);
        }
    }
}
