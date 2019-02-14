using Caresoft2._0.Areas.Procurement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.FixedAssets.Controllers
{
    [Auth]
    public class MasterFixedAssetsController : Controller
    {
        private ProcurementDbContext db = new ProcurementDbContext();

        // GET: FixedAssets/MasterFixedAssets
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryMaster(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var data = db.Category.Where(p=>p.StoreName=="FixedAssets").OrderByDescending(p => p.CategoryID).ToList();

            var categories = data.ToPagedList(pageIndex, pageSize);

            return View(categories);
        }

        [HttpPost]
        public ActionResult CategoryMaster(Category category)
        {
            category.StoreName = "FixedAssets";
            if (category != null)
            {
                db.Category.Add(category);
                db.SaveChanges();

                var data = db.Category.Where(p => p.StoreName == "FixedAssets").OrderByDescending(p => p.CategoryID).Take(10).ToList();
                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_CategoryList.cshtml", data);
            }

            return Json(category, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierMaster()
        {
            var data = db.supplier.Where(p => p.StoreName == "FixedAssets").OrderByDescending(p => p.Supplier_Code).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult SupplierMaster(supplier supplier)
        {
            supplier.StoreName = "FixedAssets";
            if (ModelState.IsValid)
            {
                db.supplier.Add(supplier);
                db.SaveChanges();
                var data = db.supplier.Where(p=>p.StoreName == "FixedAssets").OrderByDescending(p => p.Supplier_Code).ToList();

                return PartialView("~/Areas/Procurement/Views/Shared/_SupplierList.cshtml", data);
            }
            return View();
        }

        public ActionResult ManufactureCompany()
        {
            var data = db.MfgCo.Where(p=>p.StoreName == "FixedAssets").OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult ManufacturingCompany(MfgCo mfgCo)
        {
            mfgCo.StoreName = "FixedAssets";
            if (ModelState.IsValid)
            {
                db.MfgCo.Add(mfgCo);
                db.SaveChanges();

                var data = db.MfgCo.Where(p => p.StoreName == "FixedAssets").OrderByDescending(p => p.Id).ToList();
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


        public ActionResult RackMaster()
        {
            var data = db.RackMaster.OrderByDescending(p => p.Id).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult RackMaster(RackMaster rackMaster)
        {
            if (ModelState.IsValid)
            {
                db.RackMaster.Add(rackMaster);
                db.SaveChanges();

                var data = db.RackMaster.OrderByDescending(p => p.Id).ToList();

                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_RackMasterList.cshtml", data);
            }

            return Json("Something", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemLocationDetails()
        {
            var data = db.Drug.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

    }
}