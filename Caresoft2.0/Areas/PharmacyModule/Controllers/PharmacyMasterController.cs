using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using Caresoft2._0.Areas.PharmacyModule.ViewModel;
using EmbuPharmacyDataAccess;

namespace Caresoft2._0.Areas.PharmacyModule.Controllers
{
    [Auth]
    public class PharmacyMasterController : Controller
    {
        ProcurementDbContext db = new ProcurementDbContext();
        CaresoftHMISEntities db2 = new CaresoftHMISEntities();
        //EmbuCounty_PharmacyEntities dbEmbu = new EmbuCounty_PharmacyEntities();


        // GET: PharmacyModule/PharmacyMaster
        public ActionResult Index()
        {
            //LoadDataFromEmbuDbToCareSoft();
            return View();
        }

        private void LoadDataFromEmbuDbToCareSoft()
        {
            ////function to migrate data to my database

            ////Add Categories 
            //var drugTypes = dbEmbu.DrugTypeMasters.ToList();
            //List<Category> categories = new List<Category>();

            //foreach (var item in drugTypes)
            //{
            //    Category category = new Category()
            //    {
            //        CategoryName = item.DrugTypeName,
            //        StoreName = item.StoreFlag
            //    };
            //    categories.Add(category);
            //}

            //db.Category.AddRange(categories);
            //db.SaveChanges();

            ////Add Generic drugs
            //var genericDrugs = dbEmbu.tb_GenericDrug.ToList();

            //List<GenericDrugName> genericDrugNames = new List<GenericDrugName>();

            //foreach (var item in genericDrugs)
            //{
            //    GenericDrugName genericDrugName = new GenericDrugName()
            //    {
            //        Name = item.G_DrugName
            //    };

            //    genericDrugNames.Add(genericDrugName);
            //}
            //db.GenericDrugName.AddRange(genericDrugNames);
            //db.SaveChanges();

            ////add Drugs 
            //var drugsEmbu = dbEmbu.DrugMasters.ToList();

            //List<Drug> drugs = new List<Drug>();

            //Random random = new Random();

            //foreach (var item in drugsEmbu)
            //{
            //    Drug drug = new Drug()
            //    {
            //        Name = item.DrugName,
            //        GenericDrugNameId = random.Next(1, 200),
            //        CategoryId = random.Next(1, 4),
            //        IcdTenCode = item.IDA_Code,
            //        IsStrip = item.IsStrip ?? false,
            //        IsVaccine = item.IsVaccine ?? false,
            //        IsVitamin = item.IsVitaminA ?? false,
            //        CellinRack = default,
            //        genericDrugName = default,
            //        UnitsPack = item.CasePack ?? default,
            //        DoseId = 1
            //    };

            //    drugs.Add(drug);
            //}

            //db.Drug.AddRange(drugs);
            //db.SaveChanges();

            //Add Item Master
            //var itemMasterFromEmbu = dbEmbu.ItemMaster_M.ToList();

            //List<Procurement.Models.ItemMaster> itemMasters = new List<Procurement.Models.ItemMaster>();

            //Random rand = new Random();
            //foreach (var item in itemMasterFromEmbu)
            //{
            //    Procurement.Models.ItemMaster itemMaster = new Procurement.Models.ItemMaster()
            //    {
                   
            //    };

            //    itemMaster.ItemName = item.Item_Description;
            //    itemMaster.DrugId = rand.Next(1,200);
            //    itemMaster.MfgCoName = item.CompName;
            //    itemMaster.BatchNo = item.BatchNo;
            //    //itemMaster.Supplier = item.SupplierMaster.Supplier_Name??"";
            //    itemMaster.CurrentStock = (int)item.CurrentStock.Value;
            //    itemMaster.MfgDate = item.Mfg_date.Value.ToString("dd-MM-yyyy");
            //    itemMaster.CasePackRate = (int)item.CasePackRate.Value;
            //    itemMaster.CostPriceUnit = item.CostPrice.HasValue?item.CostPrice.Value:0;
            //    itemMaster.SellingPriceUnit = item.sellingPrice.Value;
            //    itemMaster.ExpiryDate = item.Exp_date;
            //    itemMaster.PurchaseDate = item.Mfg_date.Value.ToString("dd-MM-yyyy");
            //    itemMaster.UnitsPack = item.CasePack.Value;
            //    itemMaster.StoreName = item.StoreFlag ?? "MS";

            //    itemMasters.Add(itemMaster);
            //}

            //db.ItemMaster.AddRange(itemMasters);
            //db.SaveChanges();

        }

        public ActionResult RequestAnItem()
        {
            return View();
        }

        //search item names when requesting an item
        public JsonResult SearchItemName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                //var dat = db.ItemMaster.Where(p => p.ItemName == name && p.StoreName == "MS").Select(x => new { x.CurrentStock, x.Id }).ToList();
                var data = db.ItemMaster.Where(p => p.ItemName == name &&p.StoreName=="MS")
                                        .Select(x=> new { x.CurrentStock, x.Id, x.BatchNo });

                //TODO:Get total of batches

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchRequestedItem(int? page)
        {
            if(Session["StartDate"]!=null|| Session["ToDate"]!=null)
            {
                var fDate = (DateTime)Session["StartDate"];
                var tDate = (DateTime)Session["ToDate"];

                fDate = fDate.Date;
                tDate = tDate.Date;

                int pagesize = 10;
                int pageindex = 1;

                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

                var Data = db.PhamarcyRequests.Include(p => p.PharmacyRequestedItems)
                                     .Where(x => x.RequestDate >= fDate && x.RequestDate <= tDate)
                                     .OrderByDescending(p => p.Id)
                                     .ToList()
                                     .ToPagedList(pageindex, pagesize);

                Session["StartDate"] = fDate;
                Session["ToDate"] = tDate;
                return View(Data);
            }

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var requestedItems = db.PhamarcyRequests.Include(p=>p.PharmacyRequestedItems).OrderByDescending(p => p.Id).ToList();
            return View(requestedItems.ToPagedList(pageIndex,pageSize));
        }

        public ActionResult RackMaster()
        {
            var data = db.RackMaster.Where(p=>p.department == "Pharmacy").OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult RackMaster(RackMaster rackMaster)
        {
            rackMaster.department = "Pharmacy";
            if (ModelState.IsValid)
            {
                db.RackMaster.Add(rackMaster);
                db.SaveChanges();

                var data = db.RackMaster.Where(p => p.department == "Pharmacy").OrderByDescending(p => p.Id).ToList();

                return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_RackMasterList.cshtml", data);
            }

            return Json("Error Occured", JsonRequestBehavior.AllowGet);
        }


        public JsonResult SearchDrugList(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.ItemMaster.Where(p => p.StoreName== "MS" && p.ItemName.Contains(name)).Select(p => new { p.ItemName, p.Id }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetItemLocation(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;


            ViewBag.RackNames = db.RackMaster.Where(p => p.department == "Pharmacy").OrderByDescending(p => p.Id).ToList();
            var items = db.ItemMaster.Where(p => p.StoreName == "MS").ToList();

            return View(items.ToPagedList(pageIndex,pageSize));
        }

        [HttpPost]
        public ActionResult SetItemLocation(ItemLocationModel itemLocationModel)
        {

            var Item = db.ItemMaster.Where(p => p.Id == itemLocationModel.ItemId).FirstOrDefault();

            Item.RackName = itemLocationModel.RackName;
            Item.CellInRack = itemLocationModel.CellInRack;

            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();


            var items = db.ItemMaster.Where(p => p.StoreName == "MS").ToList();

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyMaster/_SetItemLocation.cshtml", items);
        }

        public ActionResult GetItemsInStore(string storeName)
        {
            var stores = db.ItemMaster.Where(p => p.StoreName == storeName)
                                      .OrderByDescending(x => x.ItemName)
                                      .Select(c=>c.ItemName)
                                      .Distinct()
                                      .ToList();
            return Json(stores,JsonRequestBehavior.AllowGet);
        }

      
        public struct PharmacyReq
        {
            public int RequiredQuantity { get; set; }
            public int ItemMasterId { get; set; }
        }

        public ActionResult RequestPharmacy(PharmacyReq model)
        {
            List<PharmacyRequestedItems> lstPharmacyRequests = new List<PharmacyRequestedItems>();
            var userName = db2.Users.Find(Convert.ToInt32(Session["UserId"])).Username;
            //var userName = "Dev";

            if (Session["lstPharmacyRequestesItems"] == null)
            {
                var theItemMaster = db.ItemMaster.Find(model.ItemMasterId);
                PharmacyRequestedItems pharmacyRequest = new PharmacyRequestedItems()
                {
                    ItemMasterId = model.ItemMasterId,
                    RequiredQuantity = model.RequiredQuantity,
                    StoreName = theItemMaster.StoreName,
                    CurrentStock =theItemMaster.CurrentStock,
                    ItemName = theItemMaster.ItemName

                };

                lstPharmacyRequests.Add(pharmacyRequest);

                Session["lstPharmacyRequestesItems"] = lstPharmacyRequests;
            }
            else
            {
                lstPharmacyRequests = (List<PharmacyRequestedItems>)Session["lstPharmacyRequestesItems"];

                //here check if any of the requested items already exists in the new request
                PharmacyRequestedItems existingRequest = new PharmacyRequestedItems();

                if (lstPharmacyRequests.Any(e => e.ItemMasterId == model.ItemMasterId))
                {
                    lstPharmacyRequests.FirstOrDefault(e => e.ItemMasterId == model.ItemMasterId).RequiredQuantity
                        += model.RequiredQuantity;
                }
                else
                {
                    var theItemMaster = db.ItemMaster.Find(model.ItemMasterId);
                    PharmacyRequestedItems pharmacyRequestedItems = new PharmacyRequestedItems()
                    {
                        ItemMasterId = model.ItemMasterId,
                        RequiredQuantity = model.RequiredQuantity,
                        StoreName = theItemMaster.StoreName,
                        CurrentStock = theItemMaster.CurrentStock,
                        ItemName = theItemMaster.ItemName

                    };

                    lstPharmacyRequests.Add(pharmacyRequestedItems);
                    Session["lstPharmacyRequestesItems"] = lstPharmacyRequests;
                }


                //foreach (var item in lstPharmacyRequests)
                //{
                //    var theItemMaster = db.ItemMaster.Find(model.ItemMasterId);
                //    if (item.ItemMasterId == model.ItemMasterId)
                //    {
                //        item.RequiredQuantity = item.RequiredQuantity + model.RequiredQuantity;
                //        existingRequest = item;
                //        item.StoreName = theItemMaster.StoreName;
                //        item.ItemName = theItemMaster.ItemName;
                        
                //    }
                //}

                //if (existingRequest == null)
                //{
                //    var theItemMaster = db.ItemMaster.Find(model.ItemMasterId);
                //    PharmacyRequestedItems pharmacyRequestedItems = new PharmacyRequestedItems()
                //    {
                //        ItemMasterId = model.ItemMasterId,
                //        RequiredQuantity = model.RequiredQuantity,
                //        StoreName = theItemMaster.StoreName,
                //        CurrentStock = theItemMaster.CurrentStock,
                //        ItemName = theItemMaster.ItemName
                        
                //    };

                //    lstPharmacyRequests.Add(pharmacyRequestedItems);
                //    Session["lstPharmacyRequestesItems"] = lstPharmacyRequests;
                //}
            }

            return PartialView("~/Areas/PharmacyModule/Views/PharmacyMaster/_RequestedItemsList.cshtml", lstPharmacyRequests);
        }

        public ActionResult SaveRequestedItems()
        {
            if (Session["lstPharmacyRequestesItems"]!=null)
            {
                var data = (List<PharmacyRequestedItems>)Session["lstPharmacyRequestesItems"];

                int UserLoggedIn = new int();
                string UserName = "";

                if(Session["UserId"] != null)
                {
                    UserLoggedIn = (int)Session["UserId"];
                    UserName = db2.Users.Where(p => p.Id == UserLoggedIn).FirstOrDefault().Username;
                }
                else
                {
                    UserName = "Admin";
                }

                PhamarcyRequests phamarcyRequests = new PhamarcyRequests()
                {
                    RequestDate = DateTime.Now,
                    Issue = "false",
                    RequestBy = UserName,
                    WardNo = 1,
                    RequestFrom = "Pharmacy"
                };

                db.PhamarcyRequests.Add(phamarcyRequests);
                db.SaveChanges();

                foreach (var item in data)
                {
                    item.PhamarcyRequestsId = phamarcyRequests.Id;
                    db.PharmacyRequestedItems.Add(item);
                }

                db.SaveChanges();

                Session["lstPharmacyRequestesItems"] = null;

                var Data = new List<PharmacyRequestedItems>();

                return PartialView("~/Areas/PharmacyModule/Views/PharmacyMaster/_RequestedItemsList.cshtml", Data);
            }
           
            return null;
        }

        public struct Dates
        {
            public string fromDate { get; set; }
            public string toDate { get; set; }
        }
        //search invoices using dates
        public ActionResult SearchByDatesRequestedItems(Dates dates)
        {
            
            var fDate = DateTime.Parse(dates.fromDate);
            var tDate = DateTime.Parse(dates.toDate);

            int? page = 1;
            int pagesize = 10;
            int pageindex = 1;

            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var Data = db.PhamarcyRequests.Include(p=>p.PharmacyRequestedItems)
                                 .Where(x => x.RequestDate >= fDate && x.RequestDate <= tDate)
                                 .OrderByDescending(p => p.Id)
                                 .ToList()
                                 .ToPagedList(pageindex, pagesize);

            Session["StartDate"] = fDate;
            Session["ToDate"] = tDate;
            return PartialView("~/Areas/PharmacyModule/Views/PharmacyMaster/_SearchRequestedItem.cshtml", Data);
        }


    }
}