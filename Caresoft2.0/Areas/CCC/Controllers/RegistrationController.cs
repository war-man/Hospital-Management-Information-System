using Caresoft2._0;
using Caresoft2._0.Controllers;
using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LabsDataAccess;
using Newtonsoft.Json;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    
    public class RegistrationController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private HouseKeeping hs = new HouseKeeping();

        // GET: Registration
        public ActionResult Index()
        {
            new Seeder().SeedPatientDemographics();
            new Seeder().SeedSalutations();
            if (db.CompanyTypes.ToList().Count < 1)
            {
                new Seeder().SeedCashAsATariff();
            }
            ViewBag.Title = "Patient Registration";
            RegistrationData registrationData = new RegistrationData();
            registrationData.BloodGroups = db.BloodGroups.ToList();
            registrationData.Categories = db.Companies.ToList();
            registrationData.IdentificationTypes = db.IdentificationTypes.ToList();
            registrationData.MainCategories = db.CompanyTypes.ToList();
            registrationData.MaritalStatus = db.MaritalStatus.ToList();
            registrationData.Relationships = db.Relationships.ToList();
            registrationData.Salutations = db.Salutations.ToList();
            registrationData.Religions = db.Religions.ToList();

            return View(registrationData);

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PatientsList", new { QueueType = "OPD" });
            }


            RegistrationData registrationData = new RegistrationData();
            registrationData.BloodGroups = db.BloodGroups.ToList();
            registrationData.Categories = db.Companies.ToList();
            registrationData.IdentificationTypes = db.IdentificationTypes.ToList();
            registrationData.MainCategories = db.CompanyTypes.ToList();
            registrationData.MaritalStatus = db.MaritalStatus.ToList();
            registrationData.Relationships = db.Relationships.ToList();
            registrationData.Salutations = db.Salutations.ToList();
            registrationData.Religions = db.Religions.ToList();

            if (db.Patients.Find(id) == null)
            {
                return RedirectToAction("PatientsList", new { QueueType = "OPD" });
            }
            registrationData.Patient = db.Patients.Find(id);

            return View("EditRegistration", registrationData);
        }
        public ActionResult PatientDetails(int? id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            Patient patient = new Patient();
            patient = db.Patients.Where(x => x.Id == id.Value).FirstOrDefault();

            var dt = JsonConvert.SerializeObject(patient);
            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        private ActionResult Json(Patient patient, object allowGet)
        {
            throw new NotImplementedException();
        }


        //GET: search patient
        public class SearchPatientListData
        {
            public String Searchtype { get; set; }
            public String Department { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string SearchText { get; set; }
            public string QueueType { get; set; }
        }

        [HttpPost]
        public ActionResult PatientsList(SearchPatientListData data)
        {
            var res = db.Patients.Take(20).ToList()
                /* && (e.RegDate>=data.StartDate && e.RegDate<=data.EndDate)*/.Take(20).ToList();

            if (data.Searchtype != null && data.Searchtype.ToLower().Equals("reg"))
            {
                res = db.Patients.Where(e => e.RegNumber.Contains(data.SearchText)).Take(20).ToList();
            }
            else
            {
                res = db.Patients.Where(e => (e.RegNumber.Contains(data.SearchText) || e.FName.Contains(data.SearchText)
                           || e.MName.Contains(data.SearchText) || e.LName.Contains(data.SearchText) || e.NationalId == data.SearchText ||
                           e.Mobile.Contains(data.SearchText))/* && (e.RegDate>=data.StartDate && e.RegDate<=data.EndDate)*/ ).Take(20).ToList();

            }

            if (data.SearchText == null || data.SearchText.Trim().Equals(""))
            {
                res = db.Patients.OrderByDescending(e => e.Id).Take(20).ToList();
            }

            ViewBag.QueuType = data.QueueType;
            ViewBag.IsSearch = true;
            return PartialView("PatientsListResult", res);
        }

        public ActionResult PatientsList(string QueueType = "OPD")
        {
            var data = db.Patients.OrderByDescending(e => e.Id).Take(20).ToList();
            ViewBag.QueuType = QueueType;
            ViewBag.MinimalFilterControlls = true;
            return View(data);
        }

        public ActionResult GetPatientDataForRepopulation(int? id)
        {
            var patient = db.Patients.Include(i => i.OpdRegisters).AsNoTracking().FirstOrDefault(e => e.Id == id);
            patient.OpdRegisters = null;
            return Json(patient, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OPDRegistration(int? id, string QueueType = "OPD", string mode = "New")
        {

            if (id != null)
            {
                var opdRegg = db.OpdRegisters.Find(id);

                //var opdRegg = db.Patients.Find(id).OpdRegisters.OrderByDescending(e => e.Id)
                // .FirstOrDefault();

                if (opdRegg != null)
                {
                    ViewBag.LastRegistationDate = opdRegg.TimeAdded;
                }
                else
                {
                    ViewBag.LastRegistationDate = DateTime.Now;
                }
            }
            else
            {
                ViewBag.LastRegistationDate = DateTime.Now;
            }

            var data = new OPDModels();

            if (mode == "New")
            {
                data.Patient = db.Patients.Find(id);

                if (data.Patient.OpdRegisters.FirstOrDefault(
                e => e.Date.Date == DateTime.Today && !(e.Status.ToLower().Trim().Equals("draft") || e.Status.ToLower().Trim().Equals("closed"))) != null)
                {
                    return Json(new { status = "info", message = "This patient is already in todays queue!" }, JsonRequestBehavior.AllowGet);
                }
                var opdDraft = data.Patient.OpdRegisters.FirstOrDefault(e => e.Status.ToLower().Trim().Equals("draft"));
                if (opdDraft != null)
                {
                    data.OPDEntry = opdDraft;
                }
                else
                {
                    var entry = new OpdRegister();
                    entry.BranchId = 1;
                    entry.Date = DateTime.Now;
                    entry.PatientId = data.Patient.Id;
                    entry.Status = "draft";
                    entry.TimeAdded = DateTime.Now;
                    var tariffCash = db.Tariffs.FirstOrDefault(e => e.TariffName.ToLower().Trim().Equals("cash"));
                    if (tariffCash == null)
                    {
                        entry.TariffId = new Seeder().SeedCashAsATariff();
                    }
                    else
                    {
                        entry.TariffId = tariffCash.Id;
                    }
                    //entry.Username = int.Parse(Session["UserId"].ToString());
                    entry.Username = 1;
                    db.OpdRegisters.Add(entry);
                    db.SaveChanges();
                    data.OPDEntry = entry;

                    //enter consultation fee by default
                    var consultationService = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "default_consultation_service_name").Value;
                    var consultation = db.Services.FirstOrDefault(e => e.ServiceName.Trim().ToLower().Equals(consultationService));
                    var billService = new BillService();

                    billService.OPDNo = entry.Id;
                    billService.DepartmentId = consultation.DepartmentId;
                    billService.SeviceId = consultation.Id; billService.ServiceName = consultation.ServiceName;
                    billService.Price = consultation.CashPrice; billService.Quatity = 1;
                    billService.Award = 0; billService.DoctorFee = 0; billService.Paid = false;
                    billService.Offered = false; billService.DateAdded = DateTime.Now;
                    //billService.UserId = int.Parse(Session["UserId"].ToString());
                    billService.UserId = 1;
                    billService.BranchId = 1; billService.IsNurse = false;
                    db.BillServices.Add(billService);
                    db.SaveChanges();

                    if (hs.IsUnderFive(entry.PatientId) && hs.ExemptUnderFive())
                    {
                        hs.AutoWaiver(billService.Id, "under 5");
                    }
                }
            }
            else if (mode == "Edit")
            {
                data.OPDEntry = db.OpdRegisters.Find(id);
                data.Patient = data.OPDEntry.Patient;

            }
            ViewBag.Mode = mode;

            data.Doctors = db.Employees.Where(e => e.Designation.DesignationName.Trim().ToLower().Equals("doctor")).ToList();
            data.RevenueDepartments = db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower().Trim().Equals("revenue")).ToList();
            //data.PatientCategories = db.Companies.ToList();
            data.MainCategories = db.CompanyTypes.ToList();
            data.Relationships = db.Relationships.ToList();
            var defaultConsultationServiceName = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Trim().Equals("default_consultation_service_name")).Value;
            data.Consultation = db.Services.FirstOrDefault(e => e.ServiceName.ToLower().Trim().Equals(defaultConsultationServiceName));
            ViewBag.QueueType = QueueType;
            ViewBag.CCCServices = db.Services.Where(p => p.Department.DepartmentName == "CCC").ToList();
            return PartialView(data);
        }


        public ActionResult AddBillService(BillService billService)
        {
            billService.DateAdded = DateTime.Now;
            db.BillServices.Add(billService);
            return Json(new { Status = "Success", BillService = billService });
        }

        public ActionResult PatientCategories(int? id)
        {
            var patientCategories = db.Companies.Where(e => e.CompanyTypeId == id).ToList();
            var options = "<option>Select</option>";
            if (patientCategories.Count > 0)
            {
                foreach (var cat in patientCategories)
                {
                    options += "<option data-pat-cat-id =" + cat.Id + " >" + cat.CompanyName + "</option>";
                }
            }

            return Content(options);

        }

        public ActionResult InuraceForm(int? id)
        {
            var myTariffs = db.Tariffs.Where(e => e.CompanyId == id).ToList();
            return PartialView("InuraceForm", myTariffs);
        }

        public ActionResult Tariffs(int? id)
        {
            if (db.Companies.Find(id) == null)
            {
                return Content("No company passed");
            }
            //List<Tariff> tariffs = db.Tariffs.Where(e => e.CompanyId.Equals(id)).ToList();
            var tariffs = db.Companies.Find(id).Tariffs.ToList();
            var options = "<option>Select</option>";
            if (tariffs.Count > 0)
            {
                foreach (var tar in tariffs)
                {
                    options += "<option data-tariff-id =" + tar.Id + " value=" + tar.Id + ">" + tar.TariffName + "</option>";
                }
            }

            return Content(options);
        }

        public ActionResult Services(int? id)
        {
            if (db.Departments.Find(id) == null)
            {
                return Content("Department not found");
            }

            var services = db.Departments.Find(id).Services.ToList();
            var options = "<option>Select</option>";
            if (services.Count > 0)
            {
                foreach (var ser in services)
                {
                    options += "<option data-service-id =" + ser.Id + " >" + ser.ServiceName + "</option>";
                }
            }

            return Content(options);
        }

        //Reception billing service
        //Get Service award amount depending on the selected service

        [HttpPost]
        public ActionResult ServiceAwardAmount(ServiceAwardAmountData data)
        {
            var OpdEntry = db.OpdRegisters.Find(data.OPDNo);
            var tariffId = OpdEntry.TariffId;
            var service = db.Services.Find(data.ServiceId);
            var price = service.CashPrice;
            var award = 0.0;
            if (OpdEntry.Tariff.TariffName.ToLower().Trim() != "cash")
            {
                var serviceTariff = db.ServicesPrices.FirstOrDefault(e => e.TariffId.Equals(tariffId) && e.ServiceId.Equals(data.ServiceId));
                if (serviceTariff != null)
                {
                    if (serviceTariff.AwardUnit.ToLower().Trim() == "percent")
                    {
                        award = (serviceTariff.Award * price) / 100;
                    }
                    else if (serviceTariff.AwardUnit.ToLower().Trim() == "amount")
                    {
                        award = serviceTariff.Award;
                    }
                }

            }


            data.Award = award;
            data.Price = price;

            return Json(data);
        }

        //[HttpPost]
        //public ActionResult SaveInsuranceDetails(InsuranceDetailsData data)
        //{
        //    var opdEntry = db.OpdRegisters.Find(data.OPDNo);
        //    opdEntry.TariffId = data.TariffId;
        //    db.SaveChanges();
        //    return Json(new { Status = "Success", Message = "Insurance Details Saved Successfully"});
        //   // return Json(opdEntry);

        //}


        [HttpPost]
        public ActionResult SaveInsuranceDetails(InsuranceCard card)
        {
            var opdENtry = db.OpdRegisters.Find(card.OPDNo);
            opdENtry.TariffId = card.SchemeId;
            db.SaveChanges();

            //Get the first Item with word Consultation
            var Consultation = opdENtry.BillServices.FirstOrDefault(e =>
            e.Service.ServiceName.ToLower().Contains("consultation"));

            if (Consultation != null)
            {
                //Get Id of the service from ServicePrices
                var ServicePrice = db.ServicesPrices.
                    FirstOrDefault(e => e.ServiceId == Consultation.Service.Id
                    && e.TariffId == card.SchemeId);

                if (ServicePrice != null)
                {
                    //Get the Company From Scheme
                    var Company = db.Tariffs.Find(card.SchemeId).Company;

                    //Get Insurance Prices
                    var IPrice = db.InsurancePrices.FirstOrDefault(e => e.CompanyId == Company.Id &&
                    e.ServicePriceId == ServicePrice.Id);

                    if (IPrice != null)
                    {
                        Consultation.Price = IPrice.Price;
                    }
                }

            }

            //re price all unpaid bill services
            foreach (var unpaid in opdENtry.BillServices.Where(e => !e.Paid))
            {
                hs.ApplyAward(unpaid.Id);
            }

            card.DateAdded = DateTime.Now;
            db.InsuranceCards.Add(card);
            db.SaveChanges();


            var billItems = db.BillServices.Where(e => e.OPDNo == card.OPDNo).ToList();


            return PartialView("~/Views/Registration/ProvisionalBillServices.cshtml", billItems);

        }

        public class CorporatePatientDetails
        {
            public int OPDNo { get; set; }
            public int CorporateTariff { get; set; }
            public String CorporateMemberNo { get; set; }
        }
        [HttpPost]
        public ActionResult SaveCorporateDetails(CorporatePatientDetails card)
        {
            var opdENtry = db.OpdRegisters.Find(card.OPDNo);
            opdENtry.TariffId = card.CorporateTariff;
            db.SaveChanges();
            //re price all unpaid bill services
            if (opdENtry.Tariff.Company.CompanyName.ToLower().Trim() == "exemption")
            {
                foreach (var unpaid in opdENtry.BillServices.Where(e => !e.Paid))
                {
                    hs.AttemptMarkPaid(unpaid.Id, 0);
                }
            }
            else
            {
                foreach (var unpaid in opdENtry.BillServices.Where(e => !e.Paid))
                {
                    hs.ApplyAward(unpaid.Id);
                }
            }


            var billItems = db.BillServices.Where(e => e.OPDNo == card.OPDNo).ToList();


            return PartialView("~/Views/Registration/ProvisionalBillServices.cshtml", billItems);

        }

        //private void updateUnpaidItems(int oPDNo, int schemeId)
        //{
        //    var opdEntry = db.OpdRegisters.Find(oPDNo);
        //    var tariff = db.Tariffs.Find(schemeId);

        //    foreach(var billEntry in opdEntry.BillServices.Where(e => !e.Paid))
        //    {
        //        var serviceInTariff = tariff.ServicesPrices.FirstOrDefault(e => e.ServiceId == billEntry.Service.Id);
        //        billEntry.TariffId = tariff.Id;
        //        if (serviceInTariff != null)
        //        {
        //            billEntry.Award = serviceInTariff.Award;
        //            if (serviceInTariff.AwardUnit.ToLower() == "percent")
        //            {
        //                billEntry.Award = serviceInTariff.Award / 100 * billEntry.Service.CashPrice;

        //            }
        //            else
        //            {
        //                billEntry.Award = billEntry.Award * billEntry.Quatity;
        //            }
        //        }
        //    }
        //}

        [HttpPost]
        public ActionResult SavePatient(Patient patient)
        {
            var filetype = "new";
            if (patient.RegNumber != null)
            {
                //Old file registration/ Do not produce a new reg number
                filetype = "old";
            }
            else if (patient.Id > 0)
            {
                filetype = "revisit";
            }

            if (filetype == "revisit")
            {
                //update and proceed to opd
                var file = db.Patients.Find(patient.Id);
                if (file != null)
                {
                    db.Entry(patient).State = EntityState.Modified;

                    db.SaveChanges();
                    return Json(new { Status = "Success", RegNumber = patient.RegNumber, PatientName = patient.Salutation + " " + patient.FName + " " + patient.MName + " " + patient.LName, PatientId = patient.Id, Username = patient.RegNumber, Password = patient.Password });
                }
            }

            //if (patient.Mobile !=null && db.Patients.Where(e => e.Mobile == patient.Mobile).ToList().Count>0)
            //{
            //    return Json(new { Status = "Error", Message = "The Mobile Number you provided is associated with another patient" });
            //}
            if (patient.NationalId != null && db.Patients.Where(e => e.NationalId == patient.NationalId).ToList().Count > 0)
            {
                return Json(new { Status = "Error", Message = "The National Id Number you provided is associated with another patient" });
            }
            if (patient.Email != null && db.Patients.Where(e => e.Email == patient.Email).ToList().Count > 0)
            {
                return Json(new { Status = "Error", Message = "The Email Address you provided is associated with another patient" });
            }
            patient.Timestamp = DateTime.Now;
            db.Patients.Add(patient);
            db.SaveChanges();

            if (filetype == "new")
            {
                var facilityinitial = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "facilityinittials").Value;
                var year = DateTime.Now.ToString("yy");
                var prefix = "00";
                if (patient.Id > 9)
                {
                    prefix = "0";
                }
                else if (patient.Id > 99)
                {
                    prefix = "";
                }
                var branchid = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "branchid").Value; //TODO pick from logged in user
                var regnumber = facilityinitial + "/" + branchid + "/" + prefix + patient.Id + "/" + year;
                patient.RegNumber = regnumber;
            }
            var password = "pat" + patient.Id.ToString();
            if (patient.FName != null && patient.MName != null)
            {
                char[] fnameArr = patient.FName.ToCharArray();
                char[] mnameArr = patient.MName.ToCharArray();
                password = fnameArr[0].ToString() + fnameArr[1].ToString() + mnameArr[0].ToString() + mnameArr[1].ToString() + DateTime.Now.ToString("dd");
            }



            patient.Password = password.ToLower();
            db.SaveChanges();

            return Json(new { Status = "Success", RegNumber = patient.RegNumber, PatientName = patient.Salutation + " " + patient.FName + " " + patient.MName + " " + patient.LName, PatientId = patient.Id, Username = patient.RegNumber, Password = patient.Password });
        }

        [HttpPost]
        public ActionResult SaveEditPatient(Patient epat)
        {

            var pat = db.Patients.Find((int)epat.Id);
            pat.Salutation = epat.Salutation;
            pat.FName = epat.FName;
            pat.MName = epat.MName;
            pat.LName = epat.LName;
            pat.Gender = epat.Gender;
            pat.DOB = epat.DOB;
            pat.Religion = epat.Religion;
            pat.BloodGroup = epat.BloodGroup;
            pat.Mobile = epat.Mobile;
            pat.Email = epat.Email;
            pat.IdentificationType = epat.IdentificationType;
            pat.NationalId = epat.NationalId;
            pat.HomeAddress = epat.HomeAddress;
            pat.KinInitial = epat.KinInitial;
            pat.KinFName = epat.KinFName;
            pat.KinOName = epat.KinOName;
            pat.KinRelationship = epat.KinRelationship;
            pat.KinMobile = epat.KinMobile;
            pat.MaritalStatus = epat.MaritalStatus;

            db.SaveChanges();

            return Json(new { status = "success", message = "Patient Details Updated Successfully!" });
        }

        [HttpPost]
        public ActionResult EnterBillService(BillServiceData data)
        {
            var OPDEntry = db.OpdRegisters.Find(data.OPDNo);
            var billserviceId = AddItemToBill(data, 1);
            if (billserviceId < 1)
            {
                return Json(new { message = "Unable to add bill item " + data.ServiceId });
            }

            if (data.View == "ProcedureForm")
            {

                return RedirectToAction("ProcedureForm", "emr", new { id = data.OPDNo, selectedDepartment = data.DepartmentId });
            }
            else

            if (data.View == "BillingForm")
            {
                var loggedInUser = db.Users.Find(1);
                if (loggedInUser.UserRole.RoleName.ToLower().Equals("billadjuster") ||
                loggedInUser.UserRole.RoleName.ToLower().Equals("sa"))
                {
                    ViewBag.CanAdjust = true;
                }


                if (!OPDEntry.IsIPD)
                {
                    var ser = db.Services.Find(data.ServiceId);
                    if (ser.IsLAB || ser.IsXRAY)
                    {
                        AddWorkOrderTest(OPDEntry, data.ServiceId, billserviceId);
                    }
                }

                OPDBillingFormData formData = new OPDBillingFormData();
                formData.OpdRegister = db.OpdRegisters.Find(data.OPDNo);
                formData.Patient = formData.OpdRegister.Patient;
                formData.ServiceGroups = db.ServiceGroups.ToList();
                formData.Drugs = formData.OpdRegister.Medications.ToList();
                formData.BillServices = db.BillServices.Where(e => e.OPDNo == data.OPDNo)
                    .OrderBy(e => e.Id)
                    .OrderByDescending(e => e.Service.DepartmentId).ToList();
                var myWallet = db.EWallets.Where(e => e.PatientId == formData.Patient.Id).ToList();

                ViewBag.EwalletBalance = (myWallet.Where(e => e.Direction == 1).Sum(e => e.AmountTransacted)
                    - myWallet.Where(e => e.Direction == 0).Sum(e => e.AmountTransacted));

                return PartialView("~/Views/Billing/BillingForm.cshtml", formData);
            }
            else if (data.View.ToLower() == "investigationform")
            {

                var investigationFormData = AddWorkOrderTest(OPDEntry, data.ServiceId, billserviceId);



                return PartialView("~/Views/EMR/InvestigationForm.cshtml", investigationFormData);
            }

            var myBillServices = db.BillServices.Where(e => e.OPDNo.Equals(data.OPDNo)).ToList();
            return PartialView(data.View, myBillServices);

        }

        public InvestigationFormData AddWorkOrderTest(OpdRegister OPDEntry, int serviceId, int billserviceId)
        {
            InvestigationFormData investigationFormData = new InvestigationFormData();
            investigationFormData.TestBillServices = db.BillServices.Where(e => e.OPDNo == OPDEntry.Id &&
        (e.Service.IsLAB || e.Service.IsXRAY)).ToList();
            investigationFormData.Patient = OPDEntry.Patient;
            investigationFormData.OpdRegister = OPDEntry;

            var dep = db.Departments.FirstOrDefault(e => e.DepartmentName.ToLower().Trim().Equals("lab")
            || e.DepartmentName.ToLower().Trim().Equals("pathology")).Id;

            CareSoftLabsEntities labDb = new CareSoftLabsEntities();
            var workOrder = labDb.WorkOrders.FirstOrDefault(e => e.OPDNo == OPDEntry.Id);
            if (workOrder == null)
            {
                //start new work order
                workOrder = new WorkOrder();
                workOrder.OPDNo = OPDEntry.Id;
                workOrder.OPDType = "OPD";
                if (OPDEntry.IsIPD)
                {
                    workOrder.OPDType = "IPD";

                }
                workOrder.BillPaid = 1;
                workOrder.Doctor = (int)Session["UserId"];
                workOrder.CreatedUtc = DateTime.Now;
                workOrder.DepartmentRadPath = dep;
                workOrder.Accession_Status = labDb.Status.FirstOrDefault(e => e.StatusValue.ToLower().Trim().Equals("default")).Id;
                labDb.WorkOrders.Add(workOrder);

                labDb.SaveChanges();



            }

            //add work order tests
            WorkOrderTest test = new WorkOrderTest();
            test.WorkOrder = workOrder.Id;
            test.Test = (int)db.Services.Find(serviceId).LabId;
            test.BillPaid = false;
            test.Status = labDb.Status.FirstOrDefault(e => e.StatusValue.ToLower().Trim().Equals("default")).Id;
            test.CreatedUtc = DateTime.Now;

            labDb.WorkOrderTests.Add(test);
            labDb.SaveChanges();

            var ser = db.BillServices.Find(billserviceId);
            ser.WorkOrderTestId = test.Id;
            db.SaveChanges();
            return investigationFormData;
        }

        public int AddItemToBill(BillServiceData data, int UserId)
        {
            BillService billService = new BillService();
            var service = db.Services.Find(data.ServiceId);
            var OPDEntry = db.OpdRegisters.Find(data.OPDNo);


            DateTime dob = ((DateTime)db.OpdRegisters.Find(data.OPDNo).Patient.DOB);

            DateTime today = DateTime.Today;

            int months = today.Month - dob.Month;
            int years = today.Year - dob.Year;


            if ((years <= 5 && service.IsUnder5))
            {
                billService.Paid = true;

                //Insert In waiver
                var waiver = new Waiver()
                {
                    OPDIPDNo = data.OPDNo,
                    AmountWaived = service.CashPrice,
                    ReasonForWaiver = "under 5 automatic waiver",
                    WaiverNote = "",
                    UserId = UserId,
                    BranchId = 1,
                    DateAdded = DateTime.Now
                };

                var _waiver = db.Waivers.Add(waiver);
                db.SaveChanges();


                billService.WaivedAmount = service.CashPrice * data.Quantity;
                billService.WaiverNo = waiver.Id;

            }

            billService.OPDNo = data.OPDNo;
            billService.DepartmentId = service.DepartmentId;
            billService.SeviceId = data.ServiceId;
            billService.ServiceName = service.ServiceName;
            billService.Price = service.CashPrice;

            var OPD = db.OpdRegisters.Find(data.OPDNo);

            if (OPD.Tariff.Company.CompanyName.ToLower() != "cash")
            {
                var Iprice = db.InsurancePrices.FirstOrDefault(e => e.CompanyId == OPD.Tariff.CompanyId &&
                e.ServicesPrice.ServiceId == data.ServiceId);

                if (Iprice != null)
                {
                    billService.Price = Iprice.Price;
                }
            }

            billService.TariffId = OPDEntry.TariffId;
            billService.Award = data.AwardAmount;
            billService.DoctorFee = 0.0;
            billService.Quatity = data.Quantity;
            billService.WorkOrderTestId = data.WorkOrderTestId;
            if (data.Quantity == 0)
            {
                billService.Quatity = 1;
            }


            billService.DateAdded = DateTime.Now;
            billService.UserId = UserId;
            billService.BranchId = 1; //TODO pick dynamically from logged in user

            if (data.View != null && data.View.ToLower() == "provisionalbillservices")
            {

                if (OPDEntry.Patient.OpdRegisters.Any(e => e.BillServices.Any(f => f.Service.
                Department.DepartmentName.ToLower().Contains("clinic"))))
                {
                    var ExemptConsultationForClinics = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ExemptConsultationForClinics"));

                    if (ExemptConsultationForClinics == null)
                    {
                        db.KeyValuePairs.Add(new KeyValuePair()
                        {
                            Key_ = "ExemptConsultationForClinics",
                            Value = "No",
                            Owner = "Dev"
                        });
                    }

                    var ExemptConsultationForClinics2 = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ExemptConsultationForClinics"));

                    if (ExemptConsultationForClinics2 != null && ExemptConsultationForClinics2.Value == "Yes")
                    {
                        var opd = db.OpdRegisters.Find(data.OPDNo);
                        var billtoExempt = opd.BillServices.FirstOrDefault(e => e.Service.ServiceName.Contains("Consultation"));

                        if (billtoExempt != null)
                        {
                            billtoExempt.Paid = true;
                            billtoExempt.Price = 0;
                        }

                    }

                }
            }

            db.BillServices.Add(billService);
            db.SaveChanges();
            if (billService.OpdRegister.Tariff.Company.CompanyName.ToLower().Trim() == "exemption")
            {
                hs.AttemptMarkPaid(billService.Id, 0);
            }
            if (OPDEntry.Tariff.TariffName.ToLower().Trim() != "cash")
            {
                hs.ApplyAward(billService.Id);

            }

            if (hs.ExemptUnderFive() && hs.IsUnderFive(billService.OpdRegister.PatientId))
            {
                hs.AutoWaiver(billService.Id, "under 5");
            }



            return billService.Id;
        }

        [HttpPost]
        public ActionResult FinalizeOpdRegistration(FinalizeOPDRegData data)
        {
            var opdEntry = db.OpdRegisters.Find(data.OPDNo);
            opdEntry.ConsultantDoctor = data.ConsultantDoctor;
            opdEntry.ReferringDoctor = data.ReferringDoctor;
            opdEntry.BroughtBy = data.BroughtBy;
            opdEntry.MLCNo = data.MLCNo;
            opdEntry.PoliceStationName = data.PoliceStationName;
            opdEntry.ReferralIn = data.ReferralIn;
            opdEntry.CounterReferral = data.Counterreferral;
            opdEntry.ReferringFacility = data.ReferringFacility;
            opdEntry.Status = data.Status;
            opdEntry.Department = data.Department;
            opdEntry.Date = DateTime.Today;
            opdEntry.TimeAdded = DateTime.Now;
            db.SaveChanges();
            return Json(new { Status = "Success", Message = "OPD Details Saved Successfully" });
        }

        public ActionResult PatDetails(int id)
        {
            var entry = db.OpdRegisters.Find(id);

            return Json(new
            {
                RegNumber = entry.Patient.RegNumber,
                PatName = entry.Patient.Salutation + " " + entry.Patient.FName + " " + entry.Patient.MName + " " + entry.Patient.LName,
                MainCat = entry.Tariff.Company.CompanyName,
                PatCat = entry.Tariff.TariffName
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DirectLab()
        {
            return RedirectToAction("PatientsList", new { QueueType = "Lab" });
        }

    }
}