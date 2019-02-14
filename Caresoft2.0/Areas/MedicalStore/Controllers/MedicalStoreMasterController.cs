using Caresoft2._0.Areas.MedicalStore.Models;
using Caresoft2._0.Areas.MedicalStore.ViewModels;
using Caresoft2._0.Areas.Procurement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.MedicalStore.Controllers
{
    [Auth]
    public class MedicalStoreMasterController : Controller
    {
        DbContextMedicalStore db = new DbContextMedicalStore();
        ProcurementDbContext db2 = new ProcurementDbContext();

        // GET: MedicalStore/MedicalStoreMaster
        public ActionResult Index()
        {
            ExpiryNreorderItems expiryNreorderItems = new ExpiryNreorderItems()
            {

                ReorderItems = db.ItemMaster.Where(p=>p.ReorderLevel>=p.CurrentStock).Take(5).ToList(),
                ExpiryItems = db.ItemMaster.Where(p=>p.ExpiryDate < DateTime.Now).ToList()
            };


            return View(expiryNreorderItems);
        }

        public ActionResult ItemMasterList(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var data = db.ItemMaster.Where(e => e.StoreName == "MS").OrderByDescending(p => p.Id).ToList();

            var items = data.ToPagedList(pageIndex, pageSize);

            return View(items);
        }

        public ActionResult ItemMasterEdit(int Id)
        {
            var itemMaster = db.ItemMaster.Find(Id);
            return View(itemMaster);
        }

        public JsonResult SearchItemListItem(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.ItemMaster.Where(p => p.ItemName.ToLower().Contains(name.ToLower()) && p.StoreName == "MS").
                    Select(p => new { p.ItemName, p.Id }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ItemMasterEdit(ItemMaster item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ItemMasterList");
            }

            return View(item);
        }

        public ActionResult ItemMaster()
        {
            return View();
        }

        public ActionResult SupplierMaster()
        {
            var data = db.supplier.OrderByDescending(p => p.Supplier_Code).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult SupplierMaster(supplier supplier)
        {
            supplier.StoreName = "MedicalStore";
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

        public ActionResult DrugMaster(int? page, int? drugId)
        {
            ViewBag.Page = page??1;

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var dose = db.Dose.ToList();
            ViewBag.Dose = dose;

            var racks = db.RackMaster.Where(p=>p.department == "MS").ToList();
            ViewBag.Racks = racks;
            var data = db.Drug.OrderByDescending(p => p.Id).ToList();

            if(drugId != null )
            {
                data = data.Where(e => e.Id == drugId).ToList();
            }

            return View(data.ToPagedList(pageIndex,pageSize));
        }

        [HttpPost]
        public ActionResult DrugMaster(Drug drug)
        {   
            if(drug!=null)
            {
                db.Drug.Add(drug);
                db.SaveChanges();

                db.Configuration.LazyLoadingEnabled = false;
                var dat = db.Drug.OrderByDescending(p => p.Id).ToList();
                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_DrugMasterList.cshtml", dat);
            }
            var data = db.Drug.OrderByDescending(p => p.Id).ToList();
            return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_DrugMasterList.cshtml", data);
        }

        public ActionResult DoseMaster(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var data = db.Dose.OrderByDescending(p => p.Id).ToList();
            var doses = data.ToPagedList(pageIndex, pageSize);

            return View(doses);
        }

        [HttpPost]
        public ActionResult DoseMaster(Dose dose)
        {
            if(dose!=null)
            {
                db.Dose.Add(dose);
                db.SaveChanges();

                var data = db.Dose.OrderByDescending(p => p.Id).Take(10).ToList();

                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_DoseList.cshtml", data);
            }

            return Json(dose,JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDose(int id)
        {
            return PartialView(db.Dose.Find(id));
        }
        [HttpPost]
        public string EditDose(Dose dose)
        {
            if (ModelState.IsValid)
            {
                var db_dose = db2.Dose.Find(dose.Id);
                db_dose.Name = dose.Name;
                db_dose.Quantity = dose.Quantity;
                db_dose.StandardTime = dose.StandardTime;
                db_dose.Description = dose.Description;
                var res = db2.SaveChanges();

                return "Update was Successful";


            }
            return "Update Failed";
        }

        public ActionResult SetRateType()
        {
            var data = db.DrugTariffs.ToList();

            
            return View(data);
        }

        public ActionResult RateTypeForAwardAmount()
        {
            return View();
        }

        public ActionResult GenericDrugMaster()
        {
            ViewBag.SideEffects = db.GenericNameSideEffects.OrderBy(p => p.Description).ToList();
            ViewBag.Toxicities = db.GenericNameToxicites.OrderBy(p => p.Description).ToList();
            ViewBag.Contraindication = db.GenericNameContraindication.OrderBy(p => p.Description).ToList();
            ViewBag.Allergies = db.GenericNameAllergies.OrderBy(p => p.Description).ToList();

            GenericDrugName genericDrugName = new GenericDrugName();
            var data = db.GenericDrugName.OrderByDescending(p => p.Id).Take(10).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult GenericDrugMaster(GenericDrugViewModel genericDrugViewModel)
        {

            //placed the unreachables code on true condition so as to resuse it later 
            //TODO: Make sure that the new generic names added will also add the drug effect like side effects

            if (true)
            {
                db.GenericDrugName.Add(genericDrugViewModel.genericDrugName);
                db.SaveChanges();
            }
            else
            {
                if (genericDrugViewModel.SideEffects != null)
                {
                    //Loop though the entire array of Side effects 
                    foreach (var item in genericDrugViewModel.SideEffects)
                    {

                        //get the side effect from the db
                        var sideEffect = db.GenericNameSideEffects
                                            .Where(p => p.Id == item)
                                            .FirstOrDefault();
                        db.SaveChanges();

                        //db.Entry(sideEffect).Collection(p => p.GenericDrugName).Load();

                        ////add generic drug name to side effect
                        //sideEffect.GenericDrugName.Add(genericDrugViewModel.genericDrugName);
                    }
                }

                if (genericDrugViewModel.Allergies != null)
                {
                    //Loop though the entire array of Side effects 
                    foreach (var item in genericDrugViewModel.Allergies)
                    {
                        //get the side effect from the db
                        var sideEffect = db.GenericNameAllergies.Find(item);
                        //add generic drug name to side effect
                        sideEffect.GenericDrugName.Add(genericDrugViewModel.genericDrugName);
                    }
                }

                if (genericDrugViewModel.Contraindication != null)
                {
                    //Loop though the entire array of Side effects 
                    foreach (var item in genericDrugViewModel.Contraindication)
                    {
                        //get the side effect from the db
                        var sideEffect = db.GenericNameContraindication.Find(item);
                        //add generic drug name to side effect
                        sideEffect.GenericDrugName.Add(genericDrugViewModel.genericDrugName);
                    }
                }

                if (genericDrugViewModel.Toxicities != null)
                {
                    //Loop though the entire array of Side effects 
                    foreach (var item in genericDrugViewModel.Toxicities)
                    {
                        //get the side effect from the db
                        var sideEffect = db.GenericNameToxicites.Find(item);
                        //add generic drug name to side effect
                        sideEffect.GenericDrugName.Add(genericDrugViewModel.genericDrugName);
                    }
                }

                db.SaveChanges();

            }

            int Id = genericDrugViewModel.genericDrugName.Id;

            db.Configuration.LazyLoadingEnabled = false;
            var data = db.GenericDrugName.OrderByDescending(p => p.Id).Take(10).ToList();

            return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_GenericDrugNames.cshtml", data);
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
            if(category!=null)
            {
                db.Category.Add(category);
                db.SaveChanges();

                var data = db.Category.OrderByDescending(p => p.CategoryID).Take(10).ToList();
                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_CategoryList.cshtml", data);
            }

            return Json(category,JsonRequestBehavior.AllowGet);
        }

        public ActionResult RackMaster()
        {
            var data = db.RackMaster.Where(p => p.department == "MedicalStore").OrderByDescending(p => p.Id).ToList();
            
            return View(data);
        }

        [HttpPost]
        public ActionResult RackMaster(RackMaster rackMaster)
        {
            rackMaster.department = "MedicalStore";
            if (ModelState.IsValid)
            {
                db.RackMaster.Add(rackMaster);
                db.SaveChanges();

                var data = db.RackMaster.Where(p => p.department == "MedicalStore").OrderByDescending(p => p.Id).ToList();

                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_RackMasterList.cshtml", data); 
            }

            return Json("Error Occured",JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemLocationDetails()
        {
            var data = db.Drug.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        public JsonResult SearchDrugs(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var data = db.Drug.Where(p => p.Name.Contains(search)).Select(p => new { p.Name, p.Id }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivateDrug(int id, int? page)
        {
            var Drug = db.Drug.FirstOrDefault(e => e.Id == id);
            Drug.IsActive = !Drug.IsActive??false;
            db.SaveChanges();
            return RedirectToAction("DrugMaster", new { page = page});
        }
    }

}