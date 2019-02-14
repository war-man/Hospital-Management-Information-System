using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Caresoft2._0.Areas.MedicalStore.ViewModels;

namespace Caresoft2._0.Areas.FixedAssets.Controllers
{
    [Auth]
    public class TransactionFixedAssetsController : Controller
    {
        private ProcurementDbContext db = new ProcurementDbContext();
        private CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        // GET: FixedAssets/TransactionFixedAssets
        public ActionResult Index()
        {
            return View();
        }

     
        public ActionResult IssueVoucher()
        {

            ViewBag.Departments = db2.Departments.OrderBy(p => p.DepartmentName).ToList();
            ViewBag.Items = db.ItemMaster.Select(p => p.ItemName).Distinct().ToList();

            List<DepartmentVoucherItem> data = new List<DepartmentVoucherItem>();

            if (Session["DepartmentVoucherId"] != null)
            {
                int id = (int)Session["DepartmentVoucherId"];


                data = db.DepartmentVoucherItem.Include(p => p.ItemMaster)
                                                   .Where(p => p.DepartmentVoucherId == id)
                                                   .ToList();
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult IssueVoucher(DepartmentIssueVoucherModel model)
        {

            if (model != null)
            {
                if (Session["DepartmentVoucherId"] == null)
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
                                                      .Include(x => x.ItemMaster)
                                                      .FirstOrDefault();

                if (checkForDepartmentVoucherItem != null)
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

            if (Session["DepartmentVoucherId"] != null)
            {
                int id = (int)Session["DepartmentVoucherId"];
                var data = db.DepartmentVoucher.Include(p => p.DepartmentVoucherItems)
                                               .Where(p => p.VoucherNO == id)
                                               .FirstOrDefault();
                //TODO: Does not load the department voucher items, load them to calculate totals 
                var totalsCalculation = db.DepartmentVoucherItem.Where(p => p.DepartmentVoucherId == id).Sum(x => x.Amount);

                db.DepartmentVoucher.Attach(data);
                data.IsItemIssued = true;
                data.IssueDate = DateTime.Now;
                data.Total = totalsCalculation;
                db.SaveChanges();

                Session["DepartmentVoucherId"] = null;

                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", departmentVoucherItems);

            }
            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstDeptIssueVoucher.cshtml", departmentVoucherItems);
        }

        public ActionResult SearchIssueVoucher()
        {
            var data = db.DepartmentVoucher.Where(p => p.IsItemIssued == true).ToList();

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

            var data = db.IssueForConsumption.Include(x => x.ItemMaster).OrderByDescending(p => p.Id).ToList();

            return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstIssueForConsumption.cshtml", data);
        }

        public ActionResult ViewPendingRequest()
        {
            List<PhamarcyRequests> data = new List<PhamarcyRequests>();

            string name = "Issue";
            if (name != null)
            {
                data = db.PhamarcyRequests.Where(p => p.Issue == name).ToList();

            }


            return View(data);
        }


        public ActionResult ViewPendingRequestSearch(string name)
        {
            List<PhamarcyRequests> data = new List<PhamarcyRequests>();

            if (name != null)
            {
                data = db.PhamarcyRequests.Where(p => p.Issue == name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstPharmacyRequest.cshtml", data);
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
            if (Name != null)
            {
                var data = db.RequestFromCSSD.Where(p => p.processed == Name).ToList();
                return PartialView("~/Areas/MedicalStore/Views/DepartmentTransactionMStore/_lstRequestsCssd.cshtml", data);
            }
            return null;
        }
    }
}