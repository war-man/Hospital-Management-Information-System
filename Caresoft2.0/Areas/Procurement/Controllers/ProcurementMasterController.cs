using Caresoft2._0.Areas.Procurement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Procurement.Controllers
{
    [Auth]
    public class ProcurementMasterController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();

        // GET: Procurement/Master
        public ActionResult Index()
        {

            return View();
        }
        

        public ActionResult ItemMaster()
        {

            return View();
        }


        [HttpPost]
        public ActionResult ItemMaster(ItemMaster item)
        {
                db.ItemMaster.Add(item);
                db.SaveChanges();
                return Json("");
        }

        public ActionResult SupplierMaster(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var data = db.supplier.OrderByDescending(p => p.Supplier_Code).ToList().ToPagedList(pageIndex, pageSize); ;
            return View(data);
        }

        [HttpPost]
        public ActionResult SupplierMaster(supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.supplier.Add(supplier);
                db.SaveChanges();
                var data = db.supplier.OrderByDescending(p => p.Supplier_Code).ToList();

                return PartialView("~/Areas/Procurement/Views/Shared/_SupplierList.cshtml", data);
            }
            return View();
        }

        public ActionResult EditSupplier(int id)
        {          
            return View(db.supplier.Find(id));
        }

        [HttpPost]
        public int EditSupplier(supplier supplier)
        {
            if (ModelState.IsValid)
            {

                db.Entry(supplier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
              
                return 1;
               
            }
            return 0;
        }

        public ActionResult ManufacturingCompany(int? page)
        {
            int pageSize = 5;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var data = db.MfgCo.OrderByDescending(p => p.Id).ToList().ToPagedList(pageIndex, pageSize); 
            return View(data);
        }

        [HttpPost]
        public ActionResult ManufacturingCompany(MfgCo mfgCo)
        {
            if (ModelState.IsValid)
            {
                db.MfgCo.Add(mfgCo);
                db.SaveChanges();

                var data = db.MfgCo.OrderByDescending(p => p.Id).Take(5).ToList();
                return PartialView("~/Areas/Procurement/Views/Shared/_MfgCoList.cshtml", data);
            }

            return View();
        }

        public ActionResult AddProvisionalPo()
        {

            return View();
        }

        public ActionResult AcceptProvisionalPo()
        {

            return View();
        }

        [HttpPost]
        public JsonResult AddNewGenericDrug(string name)
        {

            GenericDrugName drugName = new GenericDrugName()
            {
                Name = name
            };

            if (ModelState.IsValid)
            {
                db.GenericDrugName.Add(drugName);
                db.SaveChanges();
                return Json("");
            }
            return Json("");

        }

        [HttpPost]
        public JsonResult AddNewCompany(string name)
        {

            MfgCo mfgCo = new MfgCo()
            {
                Name = name
            };

            if (ModelState.IsValid)
            {
                db.MfgCo.Add(mfgCo);
                db.SaveChanges();
                return Json("");
            }
            return Json("");

        }

        [HttpPost]
        public JsonResult AddNewCategory(string name)
        {

            Category category = new Category()
            {
                CategoryName = name
            };

            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return Json("");
            }
            return Json("");

        }

        public ActionResult DeleteSupplier(int supplierCode)
        {
            var data = db.supplier.AsNoTracking().FirstOrDefault(p => p.Supplier_Code == supplierCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSupplierConfirmed(int supplierCode)
        {
            var data = db.supplier.AsNoTracking().FirstOrDefault(p => p.Supplier_Code == supplierCode);

            db.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            var model = db.supplier.OrderByDescending(p => p.Supplier_Code).ToList();

            return PartialView("~/Areas/Procurement/Views/Shared/_SupplierList.cshtml", model);
        }

        public ActionResult SupplierMasterEdit(supplier supplier)
        {
            db.Entry(supplier).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var data = db.supplier.OrderByDescending(p => p.Supplier_Code).ToList();

            return PartialView("~/Areas/Procurement/Views/Shared/_SupplierList.cshtml", data);

        }

        public ActionResult GetMfgCo(int mfgco)
        {
            var data = db.MfgCo.FirstOrDefault(p => p.Id == mfgco);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMfgCoConfirmed(int MfgCoNumber)
        {
            var data = db.MfgCo.FirstOrDefault(p => p.Id == MfgCoNumber);

            db.Entry(data).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

             var model = db.MfgCo.OrderByDescending(p => p.Id).ToList();

             return PartialView("~/Areas/Procurement/Views/Shared/_MfgCoList.cshtml", model);
            
        }

        public ActionResult EditManufacturingCo(MfgCo mfgco)
        {
            db.Entry(mfgco).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var data = db.MfgCo.OrderByDescending(p => p.Id).ToList();

            return PartialView("~/Areas/Procurement/Views/Shared/_MfgCoList.cshtml", data);
        }


    }
}
