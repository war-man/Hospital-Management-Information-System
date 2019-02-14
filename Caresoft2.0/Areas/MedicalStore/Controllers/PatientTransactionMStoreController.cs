using Caresoft2._0.Areas.MedicalStore.ViewModels;
using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.MedicalStore.Controllers
{
    [Auth]
    public class PatientTransactionMStoreController : Controller
    {
        private ProcurementDbContext db = new ProcurementDbContext();
        private CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: MedicalStore/PatientTransactionMStore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchPatientIssueVoucher()
        {
            var data = db.SimulationPatientIssueVoucher.ToList();

            return View(data);
        }

        public ActionResult EditPatientIssueVoucher()
        {
            var data = db.SimulationPatientIssueVoucher.Include(p=>p.SimulationTreatment).Where(p=>p.ItemIssued==true).ToList();
            return View(data);
        }

        public ActionResult SearchCanceledIssueVoucher()
        {
            var data = db.SimulationPatientIssueVoucher.Include(p => p.SimulationTreatment).Where(p => p.ItemIssued == true).ToList();

            return View(data);
            
        }

        public ActionResult CancelIssuedVoucher(int Id)
        {
            var data = db.SimulationPatientIssueVoucher.Include(p => p.SimulationTreatment).Where(p => p.Id == Id).FirstOrDefault();

            //use explicit loading to get the Items from Item Master
            foreach (var item in data.SimulationTreatment)
            {
                db.Entry(item).Reference(p => p.ItemMaster).Load();
            }
            return View(data);
        }

        public ActionResult SimulationTreatmentToBeCancelled(int Id)
        {
            var data = db.SimulationTreatment.Where(p => p.Id == Id).FirstOrDefault();

            db.Entry(data).Reference(p => p.ItemMaster).Load();

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_LstCancelledSimulatedIssuedVoucher.cshtml", data);
        }

        public struct RefundData
        {
            //This struct Holds data for refund
            public int Id { get; set; }
            public int ItemRefund { get; set; }
            public int totalRefund { get; set; }
      
        }

        public ActionResult SaveCancelledTreatment(RefundData refundData)
        {
            var data = db.SimulationTreatment.Where(p => p.Id == refundData.Id).FirstOrDefault();

            data.units = data.units-refundData.ItemRefund;
            data.ItemRefund += refundData.ItemRefund;
            data.Amount = data.Amount - refundData.totalRefund;

            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();


            db.Entry(data).Reference(p => p.ItemMaster).Load();

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_LstCancelledSimulatedIssuedVoucher.cshtml", data);
        }

        public ActionResult CancelVoucherReqFromNurse()
        {
            return View();
        }

        public ActionResult ItemRefund()
        {
            var data = db.SimulationTreatment.Include(p => p.ItemMaster)
                                             .Include(p=>p.SimulationPatientIssueVoucher)
                                             .Where(p => p.ItemRefund > 0).ToList();

            return View(data);
        }

        public ActionResult StockAdjustment(DateTime? StartTime, DateTime? EndTime)
        {

            if (StartTime == null || EndTime == null)
            {
                StartTime = DateTime.Today;
                EndTime = DateTime.Today;

            }
            ViewBag.StartTime = StartTime;
            ViewBag.EndTime = EndTime;

            var stocksAdjusted = db.StockAdjusted.Where(p => p.ItemMaster.StoreName == "MS" &&
           DbFunctions.TruncateTime(p.DateAdjusted) >= DbFunctions.TruncateTime(StartTime) &&
           DbFunctions.TruncateTime(p.DateAdjusted) <= DbFunctions.TruncateTime(EndTime)).
           Include(p => p.ItemMaster).ToArray();

            //var stocksAdjusted = db.StockAdjusted.Where(p => p.ItemMaster.StoreName == "")
            //    .Include(p => p.ItemMaster).ToArray();

            return View(stocksAdjusted);
        }

        public ActionResult AdjustmentStock()
        {
            return View();
        }

        //method for getting the items in Adjustment View
        public JsonResult SearchItemListItem(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.ItemMaster.Where(p => p.ItemName.Contains(name) && p.StoreName == "MS").Select(p => new { p.ItemName, Id = p.DrugId }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchItems(string name)
        {
            var data = db.ItemMaster.Where(p => p.ItemName.Contains(name) && p.StoreName == "MS").ToList();
            return PartialView("~/Areas/MedicalStore/Views/MedicalStoreMaster/_ItemMasterList.cshtml",data);
        }

        public ActionResult DirectPatientIssueVoucher(int? Id)
        {
            
            ViewBag.Dose = db.Dose.OrderBy(p => p.Name).ToList();

            if(Id!=null)
            {
                SimulationData simulationData = new SimulationData();
                int id = (int)Id;
                simulationData.SimulationPatientIssueVoucher = db.SimulationPatientIssueVoucher.Find(id);
                simulationData.LstSimulationTreatment = db.SimulationTreatment
                                                                .Include(p=>p.ItemMaster)
                                                                .Where(p => p.SimulationPatientIssueVoucherId == id).ToList();
                return View(simulationData);
            }
            SimulationData data = new SimulationData()
            {
                SimulationPatientIssueVoucher = new SimulationPatientIssueVoucher(),
                LstSimulationTreatment = new List<SimulationTreatment>()
            };
            

            return View(data);
        }

        public ActionResult SaveTreatmentData(SimulationTreatment simulationTreatment)
        {
            if(simulationTreatment!=null)
            {
                db.SimulationTreatment.Add(simulationTreatment);
                db.SaveChanges();
                
                int Id = simulationTreatment.SimulationPatientIssueVoucherId;
                
                var data = db.SimulationTreatment.Include(p => p.ItemMaster)
                                                 .Where(p=>p.SimulationPatientIssueVoucherId == Id)
                                                 .ToList();

                return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", data);
            }

            SimulationTreatment dat = null;

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_DirectPatientsIssueVoucherLst.cshtml", dat);

        }

        public ActionResult IssueItems(int Id)
        {
            var simulationPatientIssueVoucher = db.SimulationPatientIssueVoucher.Include(p => p.SimulationTreatment).Where(x => x.Id == Id).FirstOrDefault();

            if(simulationPatientIssueVoucher.SimulationTreatment.Count > 0)
            {
                simulationPatientIssueVoucher.ItemIssued = true;
                simulationPatientIssueVoucher.totalAmount = simulationPatientIssueVoucher.SimulationTreatment.Sum(p => p.Amount);
                simulationPatientIssueVoucher.DateIssued = DateTime.Now;

                //TODO: Inquire how the voucher number is generated 
                Random random = new Random();
                simulationPatientIssueVoucher.VoucherNumber = random.Next(9999,99999).ToString();
               
                db.Entry(simulationPatientIssueVoucher).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("DirectPatientIssueVoucher",new { Id });
        }

        //used for loading drugs in the set rate type form in master
        public JsonResult SearchDrugbyName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var data = db.Drug.Where(p => p.Name.Contains(name)).Select(p => new { p.Name, p.Id}).Take(20).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchBatchNumbers(string search)
        {
            var data = db.ItemMaster.Where(p => p.ItemName == (search) && p.StoreName == "MS")
                .DistinctBy(f => f.BatchNo).Select(p => new { p.BatchNo }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TextboxesForTarrifs(int DrugId)
        {
            ViewBag.DrugId = DrugId;
            var tarrifs = db2.Tariffs.ToList();
            ViewBag.DrugTariffs = db.DrugTariffs.ToList();

            return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_lstTarrifs.cshtml",tarrifs);
        }

        public struct LstDrugTarrifs
        {
            public List<DrugTariffs> drugTariffs { get; set; }
        }
        public ActionResult AddDrugsPriceToDrugs(LstDrugTarrifs lstDrugTarrifs)
        {
           if(lstDrugTarrifs.drugTariffs!=null)
            {
                

                foreach (var item in lstDrugTarrifs.drugTariffs)
                {
                    var tarriffSet = db.DrugTariffs.Where(p => p.DrugId == item.DrugId && p.TariffId == item.TariffId).FirstOrDefault();
                    //var data = db.DrugTariffs.ToList();
                    if (tarriffSet != null)
                    {
                        //here we cant add entity state cause it throws an exception
                        //db.Entry(item).State = EntityState.Modified;
                        tarriffSet.DrugUnitPrice = item.DrugUnitPrice;
                        
                    }
                    else
                    {
                        db.DrugTariffs.Add(item);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    //db.DrugTariffs.Add(item);
                    //db.SaveChanges();
                }
                
                
                var theDrugId = lstDrugTarrifs.drugTariffs.FirstOrDefault().DrugId;
                var drugTarrifsData = db.DrugTariffs.Include(p=>p.Drug).Where(p=>p.DrugId==theDrugId).ToList();

                return PartialView("~/Areas/MedicalStore/Views/PatientTransactionMStore/_lstDrugTarrifPrices.cshtml", drugTarrifsData);
                //return Json("Saved", JsonRequestBehavior.AllowGet);
            }

            return Json("null",JsonRequestBehavior.AllowGet);
        }
    }
}