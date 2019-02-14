using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using Caresoft2._0.CustomData;
using System.Data.Entity.Validation;

namespace Caresoft2._0.Controllers.Temp
{
    [Auth]
    public class ServicesPricesController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: ServicesPrices
        public ActionResult Index()
        {
            var servicesPrices = db.ServicesPrices.Include(s => s.Service).Include(s => s.Tariff);
            return View(servicesPrices.ToList());
        }

        // GET: ServicesPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesPrice servicesPrice = db.ServicesPrices.Find(id);
            if (servicesPrice == null)
            {
                return HttpNotFound();
            }
            return View(servicesPrice);
        }

        // GET: ServicesPrices/Create
        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "ServiceName");
            ViewBag.TariffId = new SelectList(db.Tariffs, "Id", "TariffName");
            TariffData tariffData = new TariffData();
            tariffData.Services = db.Services.ToList();
            tariffData.Tariffs = db.Tariffs.ToList();
            tariffData.CompaniesList = db.Companies.ToList();
            tariffData.DepartmentsList = db.Departments.Where(e=>
                e.DepartmentType1.DepartmnetType.ToLower().Trim().Equals("revenue")).ToList();
            tariffData.ServicePrices = db.ServicesPrices.ToList();
            return View(tariffData);
        }

        // POST: ServicesPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ServicesPricesData servicesPricesData)
        {
            var rows = 0;
            List<ServicesPrice> prices = servicesPricesData.ServicesPrices;
            foreach (var price in prices)
            {
                price.DateAdded = DateTime.Now;
                var entry = db.ServicesPrices.FirstOrDefault(e => e.ServiceId == price.ServiceId && e.TariffId == price.TariffId);
                if (entry != null)
                {
                    //this is an existing entry, update
                    try
                    {
                        entry.Award = price.Award;
                        entry.AwardUnit = price.AwardUnit;
                        entry.DoctorFee = price.DoctorFee;
                        entry.DoctorFeeUnit = price.DoctorFeeUnit;
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        //TO REMEMBER: How to return  specific validation error in EF
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                        return Json(price);
                        
                        //return Content(exceptionMessage);
                    }

                }
                else
                {
                    //insert as a new record
                    db.ServicesPrices.Add(price);

                }
                db.SaveChanges();
                rows += 1;
            }

            //Get the Tarrif
            var tariff = db.Tariffs.Find(servicesPricesData.TariffId);

            foreach (var altered in servicesPricesData.AlteredPrices)
            {
                if (tariff != null && tariff.Company.CompanyName != "Cash")
                {
                    var entry = db.InsurancePrices.FirstOrDefault(e => 
                    e.ServicesPrice.ServiceId == altered.ServiceId && e.CompanyId == tariff.CompanyId);

                    if (entry != null)
                    {
                        entry.Price = altered.CashPrice;
                    }
                    else
                    {
                        var ServicePriceId = db.ServicesPrices.FirstOrDefault
                            (e => e.ServiceId == altered.ServiceId && e.TariffId == tariff.Id);
                        var inPrice = new InsurancePrice()
                        {
                            ServicePriceId = ServicePriceId.Id,
                            CompanyId = tariff.CompanyId,
                            Price = altered.CashPrice

                        };
                        var res = db.InsurancePrices.Add(inPrice);
                    }

                }
                else
                {
                    var service = db.Services.Find(altered.ServiceId);
                    service.CashPrice = altered.CashPrice;
                }
            }

            db.SaveChanges();

            return Content(rows.ToString() + " changes saved successfully");


        }

        // GET: ServicesPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesPrice servicesPrice = db.ServicesPrices.Find(id);
            if (servicesPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "ServiceName", servicesPrice.ServiceId);
            ViewBag.TariffId = new SelectList(db.Tariffs, "Id", "TariffName", servicesPrice.TariffId);
            return View(servicesPrice);
        }

        // POST: ServicesPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TariffId,ServiceId,Award,AwardUnit,DoctorFee,DoctorFeeUnit")] ServicesPrice servicesPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicesPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "ServiceName", servicesPrice.ServiceId);
            ViewBag.TariffId = new SelectList(db.Tariffs, "Id", "TariffName", servicesPrice.TariffId);
            return View(servicesPrice);
        }

        // GET: ServicesPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesPrice servicesPrice = db.ServicesPrices.Find(id);
            if (servicesPrice == null)
            {
                return HttpNotFound();
            }
            return View(servicesPrice);
        }

        // POST: ServicesPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicesPrice servicesPrice = db.ServicesPrices.Find(id);
            db.ServicesPrices.Remove(servicesPrice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult WardTypePricing()
        {
            var data = new WardTypeChargesData();
            data.WardCategories = db.HSWardCategories.ToList();
            data.IPDServices = db.Services.Where(e => e.IsIPD).ToList();
            return View(data);
        }


        public ActionResult WardTypePriceList(int? id)
        {
            ViewBag.WardCatId = id;
            return PartialView("IPDPriceList", db.Services.Where(e => e.IsIPD).ToList());
        }

        
        
        public class PriceListData
        {
            public List<WardTypeCharge> Items { get; set; }
        }
        [HttpPost]
        public ActionResult UpdateWardTypePrices(PriceListData data)
        {
           
            
            var newp = new WardTypeCharge();
            newp.DateAdded = DateTime.Now;
            newp.UserId = (int)Session["UserId"];
            
            newp.BranchId = (int)Session["UserBranchId"] ;

            foreach (var entry in data.Items)
            {
                var p = db.WardTypeCharges.FirstOrDefault(e => e.ServiceId == entry.ServiceId && e.WardCategoryId == entry.WardCategoryId);
                if (p == null)
                {
                    newp.ServiceId = entry.ServiceId;
                    newp.WardCategoryId = entry.WardCategoryId;
                    newp.Price = entry.Price;
                    db.WardTypeCharges.Add(newp);
                }
                else
                {
                    p.Price = entry.Price;
                }

                db.SaveChanges();
            }

            return Content("Prices Update Successuffly!");
        }

    }

   
}
