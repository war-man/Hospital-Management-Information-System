using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Caresoft2._0.Areas.GeneralStore.Models;

namespace Caresoft2._0.Areas.GeneralStore.Controllers
{
    [Auth]
    public class MasterGStoreController : Controller
    {
        private ProcurementDbContext db = new ProcurementDbContext();

        // GET: GeneralStore/MasterGStore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryMaster(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var data = db.Category.OrderByDescending(p => p.CategoryID).ToList();

            var categories = data.ToPagedList(pageIndex, pageSize);

            return View(categories);
        }

        [HttpPost]
        public ActionResult CategoryMaster(Category category)
        {
            category.StoreName = "GeneralStore";
            if (category != null)
            {
                db.Category.Add(category);
                db.SaveChanges();

                var data = db.Category.Where(p=>p.StoreName == "GeneralStore").OrderByDescending(p => p.CategoryID).Take(10).ToList();
                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_CategoryList.cshtml", data);
            }

            return Json(category, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierMaster()
        {
            var data = db.supplier.Where(p=>p.StoreName == "GeneralStore").OrderByDescending(p => p.Supplier_Code).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult SupplierMaster(supplier supplier)
        {
            supplier.StoreName = "GeneralStore";
            if (ModelState.IsValid)
            {
                db.supplier.Add(supplier);
                db.SaveChanges();
                var data = db.supplier.OrderByDescending(p => p.Supplier_Code).ToList();

                return PartialView("~/Areas/Procurement/Views/Shared/_SupplierList.cshtml", data);
            }
            return View();
        }

        public ActionResult ManufactureCompany()
        {
            var data = db.MfgCo.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult ManufacturingCompany(MfgCo mfgCo)
        {
            mfgCo.StoreName = "GeneralStore";
            if (ModelState.IsValid)
            {
                db.MfgCo.Add(mfgCo);
                db.SaveChanges();

                var data = db.MfgCo.Where(p=>p.StoreName== "GeneralStore").OrderByDescending(p => p.Id).ToList();
                return PartialView("~/Areas/Procurement/Views/Shared/_MfgCoList.cshtml", data);
            }
            return View();
        }

        public ActionResult ItemMaster()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ItemMaster(ItemMaster item)
        {
            if (ModelState.IsValid)
            {
                db.ItemMaster.Add(item);
                db.SaveChanges();
                return Json("");
            }
            return View();
        }
    }
}