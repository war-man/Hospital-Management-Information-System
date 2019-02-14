using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using PagedList;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class PatientClaimsController : Controller
    {
        int pageSize = 10;

        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: PatientClaims
        public ActionResult Index(SearchData data, int? page)
        {
            if (data.Company != null && data.Company > 0)
            {
                ViewBag.SelectedCompanyType = db.Companies.Find(data.Company).CompanyType.CompanyTypeName;
            }
            ViewBag.SelectedCompany = data.Company;
            ViewBag.Companies = db.Companies.Where(e => e.CompanyType.CompanyTypeName.Equals("Insurance")
            || e.CompanyType.CompanyTypeName.Equals("Corporate")).ToList();
            ViewBag.SearchData = data;

            if (data.FromDate == null || data.ToDate == null)
            {
                data.FromDate = DateTime.Now;
                data.ToDate = DateTime.Now;

                data.RegNo = "";
                data.Company = 0;

            }

            var ToDate = data.ToDate?.AddDays(1);


            var opdRegisters = db.OpdRegisters.Where(e =>
               DbFunctions.TruncateTime(e.TimeAdded) >= data.FromDate && DbFunctions.TruncateTime(e.TimeAdded) <= ToDate
             && (!e.Tariff.Company.CompanyName.ToLower().Equals("Cash")));

            if (data.RegNo != null && data.RegNo.Count() == 0)
            {
                opdRegisters = opdRegisters.Where(e => e.Patient.RegNumber == data.RegNo);
            }

            if (data.BarCode != null && data.BarCode.Count() == 0)
            {
            }

            if (data.Company != null && data.Company > 0)
            {
                opdRegisters = opdRegisters.Where(e => e.Tariff.CompanyId == data.Company);
            }

            int pageNumber = (page ?? 1);
            return View(opdRegisters.OrderByDescending(e => e.Id)
                .ToPagedList(pageNumber, pageSize));
        }
       
        public class SearchData
        {
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public string RegNo { get; set; }
            public string Name { get; set; }

            public string BarCode { get; set; }
            public int Company { get; set; }
        }

        public int UpdateClaimNo(int id, string ClaimNumber)
        {
            var ClaimNo = db.PatientClaims.FirstOrDefault(e => e.OPDId == id);

            if (ClaimNo != null)
            {
                ClaimNo.ClaimNumber = ClaimNumber;
                ClaimNo.UserId = int.Parse(Session["UserId"].ToString());
            }
            else
            {
                db.PatientClaims.Add(new PatientClaim()
                {
                    OPDId = id,
                    ClaimNumber = ClaimNumber,
                    UserId = int.Parse(Session["UserId"].ToString()),
                    BrunchId = db.Users.FirstOrDefault(e => e.Id.Equals((int)Session["UserId"])).Employee.Branch.Id,
                });
            }

            int res = db.SaveChanges();

            if (res > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult SanctionInsurance(SearchData data, int? page)
        {
            if (data.Company != null && data.Company > 0)
            {
                ViewBag.SelectedCompanyType = db.Companies.Find(data.Company).CompanyType.CompanyTypeName;
            }
            ViewBag.SelectedCompany = data.Company;
            ViewBag.Companies = db.Companies.Where(e => e.CompanyType.CompanyTypeName.Equals("Insurance")
            || e.CompanyType.CompanyTypeName.Equals("Corporate")).ToList();
            ViewBag.SearchData = data;

            if (data.FromDate == null || data.ToDate == null)
            {
                data.FromDate = DateTime.Now;
                data.ToDate = DateTime.Now;

                data.RegNo = "";
                data.Company = 0;

            }

            var ToDate = data.ToDate?.AddDays(1);

            var opdRegisters = db.OpdRegisters.Where(e =>
               DbFunctions.TruncateTime(e.TimeAdded) >= data.FromDate && DbFunctions.TruncateTime(e.TimeAdded) < ToDate
             && (!e.Tariff.Company.CompanyName.Equals("Cash")) && e.PatientClaims.Any());

            if (data.RegNo != null && data.RegNo.Count() == 0)
            {
                opdRegisters = opdRegisters.Where(e => e.Patient.RegNumber == data.RegNo);
            }

            if (data.BarCode != null && data.BarCode.Count() == 0)
            {
            }

            if (data.Company != null && data.Company > 0)
            {
                opdRegisters = opdRegisters.Where(e => e.Tariff.CompanyId == data.Company);
            }

            int pageNumber = (page ?? 1);
            return View(opdRegisters.OrderByDescending(e => e.Id)
                .ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult SanctionInsurance(int opd, int tarrif, string status, double Amount)
        {
            var SanctionedAmounts = db.SanctionedAmounts.FirstOrDefault(e => e.OPDNo == opd
            && e.TarrifId == tarrif);

            if (SanctionedAmounts == null)
            {
                
                var BranchId = (int)Session["UserBranchId"] ;
                //var temp = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("BranchId"));
                //if (temp != null)
                //{
                //    BranchId = Int32.Parse(temp.Value);
                //}
                db.SanctionedAmounts.Add(new SanctionedAmount()
                {
                    OPDNo = opd,
                    TarrifId = tarrif,
                    Status = status,
                    UserId = (int)Session["UserId"],
                    BranchId = BranchId
                });
            }
            else
            {
                SanctionedAmounts.SanctionAmount = Amount;
                SanctionedAmounts.Status = status;
            }

            return View();
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
