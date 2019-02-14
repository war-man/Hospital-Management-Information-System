using Caresoft2._0.Areas.MedicalStore.ViewModels;
using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;

namespace Caresoft2._0.Areas.MedicalStore.Controllers
{
    [Auth]
    public class DepartmentTransactionMStoreController : Controller
    {
        private ProcurementDbContext db = new ProcurementDbContext();
        private CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: MedicalStore/DepartmentTransactionMStore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IssueVoucher()
        {

            ViewBag.Departments = db2.Departments.OrderBy(p => p.DepartmentName)
                .Where(e => e.DepartmentType1.DepartmnetType.ToLower() == "Revenue").ToList();
            ViewBag.Items = db.ItemMaster.OrderBy(f => f.ItemName).
                Where(e => e.StoreName.Equals("MS"))
                .ToList();

            List<DepartmentVoucherItem> data = new List<DepartmentVoucherItem>();

            if(Session["DepartmentVoucherId"]!=null)
            {
                int id = (int)Session["DepartmentVoucherId"];


                data = db.DepartmentVoucherItem.Include(p => p.ItemMaster)
                                                   .Where(p => p.DepartmentVoucherId == id)
                                                   .ToList();
            }
           
            return View(data);
        }

        //search drug names
        public JsonResult SearchItemName(int id)
        {

            if (id > 0)
            {

                var name = db.ItemMaster.Find(id).ItemName;
                var data = db.ItemMaster.Where(e => e.ItemName == name && e.StoreName == "MS");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IssueVoucher(DepartmentIssueVoucherModel model)
        {

            if(model!=null)
            {
                if(Session["DepartmentVoucherId"]==null)
                {
                    DepartmentVoucher departmentVoucher = new DepartmentVoucher();
                    departmentVoucher.DepartmentId = model.DepartmentId;
                    db.DepartmentVoucher.Add(departmentVoucher);
                    db.SaveChanges();
                    Session["DepartmentVoucherId"] = departmentVoucher.VoucherNO;
                }

                int departmentVoucherId = (int)Session["DepartmentVoucherId"];
                DepartmentVoucherItem departmentVoucherItem = new DepartmentVoucherItem()
                {
                    DepartmentVoucherId = departmentVoucherId,
                    Amount = model.Amount,
                    ItemMasterId = model.ItemMasterId,
                    Units = model.Units
                };

                //check if the department voucher item exists
                var checkForDepartmentVoucherItem = db.DepartmentVoucherItem
                                                      .Where(p => p.DepartmentVoucherId == departmentVoucherId && p.ItemMasterId == model.ItemMasterId)
                                                      .Include(x=>x.ItemMaster)
                                                      .FirstOrDefault();

                if(checkForDepartmentVoucherItem!=null)
                {
                    var units = model.Units + checkForDepartmentVoucherItem.Units;
                    var Rate = Convert.ToInt32(checkForDepartmentVoucherItem.ItemMaster.SellingPriceUnit);

                    db.DepartmentVoucherItem.Attach(checkForDepartmentVoucherItem);
                    checkForDepartmentVoucherItem.Units = units;
                    checkForDepartmentVoucherItem.Amount = Rate * units;
                    db.SaveChanges();

                }
                else
                {
                    db.DepartmentVoucherItem.Add(departmentVoucherItem);
                    db.SaveChanges();
                }
                
            }

            int id = (int)Session["DepartmentVoucherId"];

            var data = db.DepartmentVoucherItem.Include(p => p.ItemMaster)
                                               .Where(p => p.DepartmentVoucherId == id)
                                               .ToList();


            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", data);
            
        }

        public ActionResult SaveIssueVoucher()
        {
            //used the attach method to update entities
            List<DepartmentVoucherItem> departmentVoucherItems = new List<DepartmentVoucherItem>();

            if (Session["DepartmentVoucherId"]!=null)
            {
                int id = (int)Session["DepartmentVoucherId"];
                var data = db.DepartmentVoucher.Include(p=>p.DepartmentVoucherItems)
                                               .Where(p=>p.VoucherNO == id) 
                                               .FirstOrDefault();
                //TODO: Does not load the department voucher items, load them to calculate totals 
                var totalsCalculation = db.DepartmentVoucherItem.Where(p => p.DepartmentVoucherId == id).Sum(x => x.Amount);

                db.DepartmentVoucher.Attach(data);
                data.IsItemIssued = true;
                data.IssueDate = DateTime.Now;
                data.Total = totalsCalculation;
                

                if (db.SaveChanges() == 1)
                {
                    var depVoucherItems = db.DepartmentVoucherItem.Include(k => k.ItemMaster).Include(o => o.DepartmentVoucher).Where(p => p.DepartmentVoucherId == id).ToList();

                    foreach (var dit in depVoucherItems)
                    {
                        DrugTransactions drugTransaction = new DrugTransactions()
                        {
                            DepartmentId = data.DepartmentId,
                            ItemMasterId = dit.ItemMasterId,
                            TransactionDate = DateTime.Now,
                            QuantityIssued = dit.Units,
                            User = (int)Session["UserId"],
                            Rate = Convert.ToInt32(dit.ItemMaster.CostPriceUnit)
                        };

                        db.DrugTransactions.Add(drugTransaction);
                        db.SaveChanges();

                        if (dit.DepartmentVoucher.DepartmentId == db2.Departments.FirstOrDefault(e => e.DepartmentName == "Pharmacy").Id)
                        {
                            var PItem = db.ItemMaster.FirstOrDefault(e => e.StoreName == "P" && e.ItemName == dit.ItemMaster.ItemName
                            && e.BatchNo == dit.ItemMaster.BatchNo);

                            if (PItem != null)
                            {
                                PItem.CurrentStock = PItem.CurrentStock + dit.Units;
                                db.SaveChanges();
                            }
                            else
                            {
                                var departmentToIssue = db2.Departments.Find(dit.DepartmentVoucher.DepartmentId);

                                using (ProcurementDbContext entity = new ProcurementDbContext())
                                {
                                    var newIM = dit.ItemMaster;

                                    newIM.CurrentStock = dit.Units;
                                    newIM.StoreName = "P";

                                    entity.ItemMaster.Add(newIM);
                                    entity.SaveChanges();
                                }
                             

                            }
                        }
                        else
                        {
                            var departmentToIssue = db2.Departments.Find(dit.DepartmentVoucher.DepartmentId);

                            var DepartmentItem = db.ItemMaster.FirstOrDefault(e => e.StoreName == departmentToIssue.DepartmentName
                            && e.ItemName == dit.ItemMaster.ItemName && e.BatchNo == dit.ItemMaster.BatchNo);

                            if (DepartmentItem != null)
                            {
                                DepartmentItem.CurrentStock = DepartmentItem.CurrentStock + dit.Units;
                                db.SaveChanges();
                            }
                            else
                            {
                                using(ProcurementDbContext entity = new ProcurementDbContext())
                                {
                                    var newIM = dit.ItemMaster;
                                    
                                    newIM.CurrentStock = dit.Units;
                                    newIM.StoreName = departmentToIssue.DepartmentName;

                                    entity.ItemMaster.Add(newIM);
                                    entity.SaveChanges();
                                }
                                

                            }
                        }
                        ProcurementDbContext custdb = new ProcurementDbContext();

                        var MSItem = custdb.ItemMaster.FirstOrDefault(e => e.StoreName == "MS" && e.ItemName == dit.ItemMaster.ItemName 
                        && e.BatchNo == dit.ItemMaster.BatchNo);

                        MSItem.CurrentStock = MSItem.CurrentStock - dit.Units;

                        int res = custdb.SaveChanges();
                        if (res > 0)
                        {
                            Session["DepartmentVoucherId"] = null;
                        }
                    }

                }


                Session["DepartmentVoucherId"] = null;

                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", new List<DepartmentVoucherItem>());

            }
            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", departmentVoucherItems);
        }

        public ActionResult SearchIssueVoucher()
        {
            var data = db.DepartmentVoucher.Where(p => p.IsItemIssued == true)
                                           .OrderByDescending(p=>p.VoucherNO)
                                           .ToList();

            return View(data);
        }

        public ActionResult DeptIndexList()
        {
            var data = db.DeptIndentList.ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult DeptIndexList(string Name)
        {
            if (Name != null)
            {
                var data = db.DeptIndentList.Where(p => p.processed == Name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIndex.cshtml", data);
            }
            return null;    
        }

        public ActionResult IsueForConsuption()
        {
            //TODO: add viewbag for items
            ViewBag.Items = db.ItemMaster.Select(p => p.ItemName).Distinct().ToList();
            return View();
        }

        [HttpPost]
        public ActionResult IsueForConsuption(IssueForConsumption issueForConsumption)
        {
            db.IssueForConsumption.Add(issueForConsumption);
            db.SaveChanges();

            var data = db.IssueForConsumption.Include(x=>x.ItemMaster).OrderByDescending(p => p.Id).ToList();

            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstIssueForConsumption.cshtml", data);
        }

        public ActionResult ViewPendingRequest()
        {
            List<PhamarcyRequests> data = new List<PhamarcyRequests>();

       
            var sTime = DateTime.Today;
            var eTime = DateTime.Today;
            eTime = eTime.AddDays(1);

            string name = "Issue";
            if (name!=null)
            {
                data = db.PhamarcyRequests.Where(p => p.PharmacyRequestedItems
                .Any(e => e.IsIssued == false || e.IsIssued == null))
                         .OrderByDescending(p=>p.Id)
                         .ToList();
            
            }

            
            return View(data.Where(e => e.RequestDate > sTime && e.RequestDate <= eTime));
        }

        public ActionResult IssueToMedical(int Id)
        {
            var lstPharmacyRequests = db.PharmacyRequestedItems.Where(p => p.PhamarcyRequestsId == Id && p.IsIssued == false).ToList();

            foreach (var item in lstPharmacyRequests)
            {
                var data = db.ItemMaster.Where(p => p.Id == item.ItemMasterId).FirstOrDefault();
                
            }

            return View(lstPharmacyRequests);
        }

        public ActionResult ViewPharmacyRequestedItems(int Id)
        {
            var data = db.PharmacyRequestedItems.Where(e => e.PhamarcyRequests.Id == Id);
            return View("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/PharmacyRequestedItems.cshtml", data.ToList());
        }

        public ActionResult IssueItemsToPharmacy(int Id)
        {
            var item = db.PharmacyRequestedItems
                                    .Find(Id);


            if (item != null)
            {
                {
                    item.IsIssued = true;
                    item.CurrentStock = item.CurrentStock - item.RequiredQuantity;
                    //db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();



                    var iMaster = db.ItemMaster.Find(item.ItemMasterId);
                    //declare this here to so that its not over ridden
                    var newPharmacyItemMaster = iMaster;

                    var CurrentStockMedical = iMaster.CurrentStock - item.RequiredQuantity;
                    //db.ItemMaster.Attach(ItemMaster);
                    iMaster.CurrentStock = CurrentStockMedical;


                    // lstItemMaster.Add(ItemMaster);
                    //db.Entry(ItemMaster).State = EntityState.Modified;
                    db.SaveChanges();

                    var itemExists = db.ItemMaster.Where(e => e.ItemName == item.ItemName && e.StoreName == "P").FirstOrDefault();

                    if (itemExists == null)
                    {
                        ItemMaster master = new ItemMaster()
                        {
                            AssetStatus = newPharmacyItemMaster.AssetStatus,
                            barCode = newPharmacyItemMaster.barCode,
                            BatchNo = newPharmacyItemMaster.BatchNo,
                            CasePackRate = newPharmacyItemMaster.CasePackRate,
                            Category = newPharmacyItemMaster.Category,
                            CellInRack = newPharmacyItemMaster.CellInRack,
                            CostPriceUnit = newPharmacyItemMaster.CostPriceUnit,
                            CurrentStock = item.RequiredQuantity,
                            DateCommisioned = newPharmacyItemMaster.DateCommisioned,
                            DateDisposed = newPharmacyItemMaster.DateDisposed,
                            DrugId = newPharmacyItemMaster.DrugId,
                            ExpiryDate = newPharmacyItemMaster.ExpiryDate,
                            ExpiryStatus = newPharmacyItemMaster.ExpiryStatus,
                            GenericDrugName = newPharmacyItemMaster.GenericDrugName,
                            ICDTenCode = newPharmacyItemMaster.ICDTenCode,
                            ItemName = newPharmacyItemMaster.ItemName,
                            MfgCoName = newPharmacyItemMaster.MfgCoName,
                            MfgDate = newPharmacyItemMaster.MfgDate,
                            PurchaseDate = newPharmacyItemMaster.PurchaseDate,
                            RackName = newPharmacyItemMaster.RackName,
                            ReorderLevel = newPharmacyItemMaster.ReorderLevel,
                            SellingPriceUnit = newPharmacyItemMaster.SellingPriceUnit,
                            StoreName = "P",
                            Supplier = newPharmacyItemMaster.Supplier,
                            UnitsPack = newPharmacyItemMaster.UnitsPack,
                            WarantyExpiryDate = newPharmacyItemMaster.WarantyExpiryDate

                        };

                        db.ItemMaster.Add(master);
                    }
                    else
                    {
                        itemExists.CurrentStock = itemExists.CurrentStock + item.RequiredQuantity;
                    }
                    var rez2 = db.SaveChanges();


                }

                return RedirectToAction("ViewPharmacyRequestedItems", new { id = Id});
            }

            return View(item.PhamarcyRequests.PharmacyRequestedItems);
        }

        public ActionResult IssueItemsToDepartment(int Id)
        {
            var PharmacyRequestedItem = db.PharmacyRequestedItems
                                    .Where(p => p.Id == Id)
                                    .Include(p => p.PhamarcyRequests)
                                    .FirstOrDefault();

            //if (db.ItemMaster.Find(PharmacyRequestedItem.ItemMasterId))
            //{

            //}
            if (PharmacyRequestedItem != null)
            {

                PharmacyRequestedItem.IsIssued = true;

                //Get The Item Master
                var ItemMaster = db.ItemMaster.Find(PharmacyRequestedItem.ItemMasterId);

                ItemMaster.CurrentStock = ItemMaster.CurrentStock - PharmacyRequestedItem.RequiredQuantity;
                //db.Entry(item).State = EntityState.Modified;
                if (db.SaveChanges() < 1) {

                    return RedirectToAction("ViewPharmacyRequestedItems", new { Id = PharmacyRequestedItem.PhamarcyRequestsId });
                }

                //declare this here to so that its not over ridden
                var newPharmacyItemMaster = ItemMaster;                            

                ItemMaster master = new ItemMaster()
                {
                    AssetStatus = newPharmacyItemMaster.AssetStatus,
                    barCode = newPharmacyItemMaster.barCode,
                    BatchNo = newPharmacyItemMaster.BatchNo,
                    CasePackRate = newPharmacyItemMaster.CasePackRate,
                    Category = newPharmacyItemMaster.Category,
                    CellInRack = newPharmacyItemMaster.CellInRack,
                    CostPriceUnit = newPharmacyItemMaster.CostPriceUnit,
                    CurrentStock = PharmacyRequestedItem.RequiredQuantity,
                    DateCommisioned = newPharmacyItemMaster.DateCommisioned,
                    DateDisposed = newPharmacyItemMaster.DateDisposed,
                    DrugId = newPharmacyItemMaster.DrugId,
                    ExpiryDate = newPharmacyItemMaster.ExpiryDate,
                    ExpiryStatus = newPharmacyItemMaster.ExpiryStatus,
                    GenericDrugName = newPharmacyItemMaster.GenericDrugName,
                    ICDTenCode = newPharmacyItemMaster.ICDTenCode,
                    ItemName = newPharmacyItemMaster.ItemName,
                    MfgCoName = newPharmacyItemMaster.MfgCoName,
                    MfgDate = newPharmacyItemMaster.MfgDate,
                    PurchaseDate = newPharmacyItemMaster.PurchaseDate,
                    RackName = newPharmacyItemMaster.RackName,
                    ReorderLevel = newPharmacyItemMaster.ReorderLevel,
                    SellingPriceUnit = newPharmacyItemMaster.SellingPriceUnit,
                    StoreName = "P",
                    Supplier = newPharmacyItemMaster.Supplier,
                    UnitsPack = newPharmacyItemMaster.UnitsPack,
                    WarantyExpiryDate = newPharmacyItemMaster.WarantyExpiryDate

                };

                var existingItem = db.ItemMaster.FirstOrDefault(e => e.BatchNo == ItemMaster.BatchNo && e.StoreName == 
                "P" && e.DrugId == ItemMaster.DrugId);
                if (existingItem == null)
                {
                    db.ItemMaster.Add(master);
                }
                else
                {
                    existingItem.CurrentStock = existingItem.CurrentStock + PharmacyRequestedItem.RequiredQuantity;
                }
                var rez2 = db.SaveChanges();

                return RedirectToAction("ViewPharmacyRequestedItems", new { Id= PharmacyRequestedItem.PhamarcyRequestsId });
            }

            return RedirectToAction("ViewPharmacyRequestedItems", new { Id = PharmacyRequestedItem.PhamarcyRequestsId });
        }

        public ActionResult ViewPendingRequestSearch(string name, DateTime? sTime, DateTime? eTime)
        {
            if (name == "" || name == null)
            {
                name = "Issue";
            }

            if (sTime == null || eTime == null)
            {
                sTime = DateTime.Today;
                eTime = DateTime.Today;
            }
            eTime = eTime.Value.AddDays(1);

            List<PhamarcyRequests> data = new List<PhamarcyRequests>();

            if (name != null)
            {
                if (name == "Partial") {

                    data = db.PhamarcyRequests.Include(t => t.PharmacyRequestedItems).Where(p => p.PharmacyRequestedItems
                    .Any(e => e.IsIssued == false || e.IsIssued == null) && p.PharmacyRequestedItems.Any(e => e.IsIssued == true))
                    .OrderByDescending(p => p.Id)
                    .ToList();

                    data.Where(p => p.PharmacyRequestedItems
                    .Any(e => e.IsIssued == true));
                    
                } else if (name == "Processed") {
                    data = db.PhamarcyRequests.Include(t => t.PharmacyRequestedItems).Where(p => p.PharmacyRequestedItems
                    .All(e => e.IsIssued == true))
                             .OrderByDescending(p => p.Id)
                             .ToList();

                } else if (name == "Issue") {
                    data = db.PhamarcyRequests.Include(t => t.PharmacyRequestedItems).Where(p => p.PharmacyRequestedItems
                    .All(e => e.IsIssued == false || e.IsIssued == null))
                             .OrderByDescending(p => p.Id)
                             .ToList();

                }
                
                //data = db.PhamarcyRequests.Where(p => p.Issue == name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstPharmacyRequest.cshtml", 
                    data.Where(e => e.RequestDate > sTime && e.RequestDate <= eTime));
            }


            return View(data);
        }

        public ActionResult IssueToCssd()
        {
            var data = db.RequestFromCSSD.ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult IssueToCssd(string Name)
        {
            if(Name!=null)
            {
                var data = db.RequestFromCSSD.Where(p => p.processed == Name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstRequestsCssd.cshtml", data);
            }
            return null;
        }

        public ActionResult UpdateItemCost(string newCost, string type, int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var item = db.ItemMaster.Find(id);

            ViewBag.AdjustedQuantity = newCost;

            if (type == "Surplus")
            {
         
                var nCost = Convert.ToInt32(item.CurrentStock)  + Convert.ToInt32(newCost);
                item.CurrentStock = nCost;

            }
            else
            {
                var nCost = Convert.ToInt32(item.CurrentStock) - Convert.ToInt32(newCost);
                item.CurrentStock = nCost;
            }

            db.Entry(item).State = EntityState.Modified;

            db.StockAdjusted.Add(new StockAdjusted()
            {
                ItemMasterId = id,
                DateAdjusted = DateTime.Now,
                UserId = Convert.ToInt32(Session["UserId"]),
                Quantity = Convert.ToInt32(newCost),
                Type = type,
                Department = "MS"
            });

            db.SaveChanges();

            return PartialView("~/Areas/Procurement/Views/ProcurementPurchase/_tblAdjustmentStock.cshtml", item);
        }

        public JsonResult SearchItemListItem(string name)
        {

           

            if (!string.IsNullOrEmpty(name))
            {
                var data = db.ItemMaster.Where(p => 
                p.ItemName.ToLower().Contains(name.ToLower()) && p.StoreName == "MS").
                    Select(p => new { p.ItemName, p.Id }).Take(20).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}