using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.Areas.Procurement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Procurement.Controllers
{
    //this controller returns data/ saves data using json in the item Master
    [Auth]
    public class MinorMasterProcurementController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewGenericDrug(string name)
        {
            //first trial of the repository pattern 
            RepoProcurement<GenericDrugName> repoProcurement = new RepoProcurement<GenericDrugName>();

            GenericDrugName drugName = new GenericDrugName() { Name = name };
          
            if (ModelState.IsValid)
            {
                repoProcurement.Add(drugName);
                return Json("");
            }
            else
            {
                return View();
            }
            

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
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);

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
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }

        public JsonResult SearchGenericDrug(string name)
    {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.GenericDrugName.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchManufactureName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.MfgCo.Where(p => p.Name.Contains(name)).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCategory(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Category.Where(p => p.CategoryName.ToLower().Contains(name.ToLower())).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchSupplier(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.supplier.Where(p => p.Supplier_Name.Contains(name)).ToList();
                return Json(data);
            }
            return Json("");
        }

        public JsonResult SearchItemList(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => p.Name.Contains(name)).Select(p => p.Name).ToList();
                return Json(data,JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchDrugList(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => p.Name.Contains(name)).Select(p => new { p.Name,p.Id }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchDrugById(int? Id)
        {
            if (Id!=null)
            {
                var ID = (int)Id;
                var data = db.Drug.Where(p => p.Id == ID).Select(p=> new {p.genericDrugName.Name, p.Category.CategoryName, p.ReorderLevel,p.UnitsPack }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("UnSuccessful", JsonRequestBehavior.AllowGet);
        }
    }
}