using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using Caresoft2._0.CustomData;

namespace Caresoft2._0.Controllers.Misc
{
    public class FieldMetaController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        /*DB Crawler */

        public ActionResult ListTables()
        {
            var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;
            var tables = metadata.GetItemCollection(DataSpace.SSpace)
        .GetItems<EntityContainer>()
        .Single()
        .BaseEntitySets
        .OfType<EntitySet>()
        .Where(s => !s.MetadataProperties.Contains("Type")
        || s.MetadataProperties["Type"].ToString() == "Tables");

            TablesModel tableModel = new TablesModel();
            List<TablesModel> tableModels = new List<TablesModel>();

            foreach(var t in tables)
            {
                tableModel.TableName = t.Name;
                tableModels.Add(tableModel);
            }
            return View("TablesList", tableModels);
        }

        /*End DB Crawler*/



        // GET: FieldMeta
        public ActionResult Index()
        {
            return View(db.FieldMetas.ToList());
        }

        // GET: FieldMeta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldMeta fieldMeta = db.FieldMetas.Find(id);
            if (fieldMeta == null)
            {
                return HttpNotFound();
            }
            return View(fieldMeta);
        }

        // GET: FieldMeta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FieldMeta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TableName,Field,Required")] FieldMeta fieldMeta)
        {
            if (ModelState.IsValid)
            {
                db.FieldMetas.Add(fieldMeta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fieldMeta);
        }

        // GET: FieldMeta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldMeta fieldMeta = db.FieldMetas.Find(id);
            if (fieldMeta == null)
            {
                return HttpNotFound();
            }
            return View(fieldMeta);
        }

        // POST: FieldMeta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TableName,Field,Required")] FieldMeta fieldMeta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fieldMeta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fieldMeta);
        }

        // GET: FieldMeta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldMeta fieldMeta = db.FieldMetas.Find(id);
            if (fieldMeta == null)
            {
                return HttpNotFound();
            }
            return View(fieldMeta);
        }

        // POST: FieldMeta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FieldMeta fieldMeta = db.FieldMetas.Find(id);
            db.FieldMetas.Remove(fieldMeta);
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
