using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.CrystalReports;
using Caresoft2._0.CrystalReports.OutsideTest;
using Caresoft2._0.CustomData;
using Caresoft2._0.UniversalHelpers;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class EMRController : Controller
    {
        private ProcurementDbContext procDB = new ProcurementDbContext();
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private CareSoftLabsEntities labDb = new CareSoftLabsEntities();
        public ActionResult Index()
        {
            return View();
        }
        // GET: OPD list// todays list by default
        public ActionResult OpdList(int page=1)
        {


            var opdReg = db.OpdRegisters.Where(e => e.Date >= DateTime.Today
            && !e.IsIPD && !e.Status.Trim().ToLower().Equals("draft")).OrderByDescending(e=>e.Id).ToList();

            @ViewBag.Unread = opdReg.Where(e => e.Complaints.Count() == 0).Count();

            int itemsperpage = 15;
            var offset = (itemsperpage * page) - itemsperpage;
            ViewBag.Page = page;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(opdReg.Count() / itemsperpage))); 

            ViewBag.MedicalDeps = db.Departments.Where(e => e.IsMedical != null && e.IsMedical.Trim().ToLower().Equals("yes")).ToList();

            return View(opdReg.Skip(offset).Take(itemsperpage));
        }

        //GET: search OPD mini list
        public ActionResult SearchOPDMiniList(String search)
        {
            var data = db.OpdRegisters.Where(e => (e.Date >= DateTime.Today &&
            !e.IsIPD && !e.Status.Trim().ToLower().Equals("draft")) && (
            e.Patient.RegNumber.Contains(search) || e.Patient.FName.Contains(search) ||
            e.Patient.MName.Contains(search) || e.Patient.LName.Contains(search))).Take(20).ToList();
            return PartialView("OPDMiniList", data);
        }
        // GET: OPD list// todays list by default
        public ActionResult FIlterOPDList(FilterOPD filterOPD, String IsNurse)
        {
            string dep = filterOPD.Department.Trim().ToLower();

            ViewBag.Mode = "AJAX";
            var data = db.OpdRegisters.Where(e => e.Date >= filterOPD.StartDate
            && DbFunctions.TruncateTime(e.Date) <= filterOPD.EndDate && !e.IsIPD
            && !e.Status.Trim().ToLower().Equals("draft")).OrderByDescending(e=>e.Id).ToList();

            if(filterOPD.SearchText == null)
            {
                int itemsperpage = 15;
                var offset = (itemsperpage * 1) - itemsperpage;
                ViewBag.Page = 1;
                ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(data.Count() / itemsperpage)));
                data = data.Skip(offset).Take(itemsperpage).ToList();
            }

            if (filterOPD.SearchText != null)
            {
                data = db.OpdRegisters.Where(e => (e.Patient.FName.Contains(filterOPD.SearchText) ||
                e.Patient.MName.Contains(filterOPD.SearchText) || e.Patient.LName.Contains(filterOPD.SearchText) ||
                e.Patient.RegNumber.Contains(filterOPD.SearchText) || e.Id.ToString().Equals(filterOPD.SearchText)) &&
                (e.Date >= filterOPD.StartDate && DbFunctions.TruncateTime(e.Date) <= filterOPD.EndDate) && !e.IsIPD
                && !e.Status.Trim().ToLower().Equals("draft")).ToList();
            }

            ViewBag.SearchMeta = "Search results for " + filterOPD.SearchText + " for dates within "
                                + filterOPD.StartDate.ToShortDateString() + " and " + filterOPD.EndDate.ToShortDateString();
            ViewBag.ResultsSize = data.Count();
            if (IsNurse != null && IsNurse.ToLower().Trim() == "value")
            {
                ViewBag.IsNurse = true;
            }
            if (dep != null && dep != "all")
            {
                data = data.Where(e => (e.Department != null && e.Department.Trim().ToLower() == dep && e.PatientReferals.Count() < 1)
                || (e.PatientReferals.Count() > 0 && e.PatientReferals.LastOrDefault().Department.DepartmentName.Trim().ToLower().Equals(dep))).ToList();
            }

            return PartialView("OpdList", data.Take(15));
        }

        //GET  Consultation form
        public ActionResult Consultation(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            new Methods().QueueTime(id, "Consultant");

            OpdRegister opdDetails = db.OpdRegisters.Find(id);


            //Update PatientTurnAroundTime. Set TimeOut
            var ptat = opdDetails.PatientTurnAroundTimes
                .FirstOrDefault(e => e.Department.Equals("emr"));

            if (ptat != null && ptat.FullfilmentTime == null)
            {
                ptat.FullfilmentTime = DateTime.Now;
                db.SaveChanges();
            }

            Patient patient = db.Patients.FirstOrDefault(e => e.RegNumber.Equals(opdDetails.Patient.RegNumber));
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            emr_opd_data.Questionnaires = db.Questionnaires.ToList();
            emr_opd_data.Complaints = db.Complaints.OrderByDescending(e => e.Id).Where(e =>
                e.PatientId == patient.Id).ToList();
            if (patient == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            ViewBag.OpdNo = id;
            ViewBag.Diagnosis = opdDetails.PatientDiagnosis.ToList();
            emr_opd_data.TodaysComplaint = opdDetails.Complaints.Where(e=>!e.ChiefComplaints.StartsWith("[DELETED]")).LastOrDefault();
            return View(emr_opd_data);
        }

        public ActionResult ComplaintsList(int id)
        {
            var myComplaints = db.Complaints.OrderByDescending(e => e.Id)
                    .Where(e => e.PatientId == id && !e.ChiefComplaints.StartsWith("[DELETED]")).Include(e => e.User).ToList();


            return PartialView("ComplaintsList", myComplaints);
        }

        // POST: EMR/PostComplaint
        public ActionResult PostComplaint(Complaint complaint)
        {
            try
            {
                complaint.Timestap = DateTime.Now;
                complaint.DoctorId = int.Parse(Session["UserId"].ToString());
                
                complaint.BranchId = (int)Session["UserBranchId"] ; ;
                db.Complaints.Add(complaint);

                db.SaveChanges();

                AddToRegularComplaints(complaint.ChiefComplaints);

                var myComplaints = db.Complaints.OrderByDescending(e => e.Id)
                    .Where(e => e.PatientId == complaint.PatientId && !e.ChiefComplaints.StartsWith("[DELETED]")
                    ).Include(e => e.User).Include(e => e.OpdRegister.PatientDiagnosis).ToList();

                return PartialView("ComplaintsList", myComplaints);
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

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                return Content(exceptionMessage);
            }
        }


        //GET Physical examination form
        public ActionResult PhysicalExamination(int? id)
        {
            //id is opd number
            if (id == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = opdDetails.Patient;
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            if (patient == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            if (opdDetails.IsIPD)
            {
                ViewBag.IsIPD = true;
            }
            ViewBag.OpdNo = id;
            return View(emr_opd_data);
        }



        //Add Physical examination form data
        [HttpPost]
        public ActionResult PostPhysicalExamination(PhysicalExamination physicalExamination, string RecommendDischarge, string Emergency)
        {
            if (physicalExamination == null)
            {
                return HttpNotFound();
            }

            physicalExamination.Date = DateTime.Now;
            physicalExamination.UserId = int.Parse(Session["UserId"].ToString());
            db.PhysicalExaminations.Add(physicalExamination);

            var patient = db.Patients.Find(physicalExamination.PatientId);

            var OpdIpd = patient.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault();
            if (RecommendDischarge != null)
            {
                if (RecommendDischarge == "1")
                {
                    //TODO:Add Discharge Recommendation

                    //OpdIpd.DischargeRecommendedBy = int.Parse(Session["UserId"].ToString());
                    //OpdIpd.RecommendDischarge=true;
                }
            }

            if (Emergency != null)
            {

                if (Emergency == "1")
                {
                    OpdIpd.Status = "Emergency";
                }
            }

            db.SaveChanges();

            return PartialView("PhysicalExaminationLog",
                db.PhysicalExaminations.OrderByDescending(e => e.Id).Where(e => e.PatientId.Equals(physicalExamination.PatientId)).ToList());
        }

        //render partial view with patients recent physical examinations
        public ActionResult GetPhysicalExamination(int? id)
        {

            return PartialView("PhysicalExaminationLog",
                //TO REMEMBER: How to order descending
                db.PhysicalExaminations.OrderByDescending(e => e.Id).Where(e => e.PatientId == id).ToList());
        }

        //render partial view with patients most recent physical examinations
        public ActionResult GetMostRecentPhysicalExam(int? id)
        {

            var results = db.PhysicalExaminations.OrderByDescending(e => e.Id).FirstOrDefault(e => e.PatientId == id);

            return PartialView("RecentInfo", results);
        }

        public ActionResult InvestigationForm(int? id, string idType = "")
        {

            ViewBag.OutsideTests = db.OutsideTests.Where(e => e.OPDId == id).ToList();

            InvestigationFormData data = new InvestigationFormData();

            var opdRegister = new OpdRegister();
            var patient = new Patient();
            new Methods().QueueTime(id, "Consultant");

            if (idType == "")
            {
                opdRegister = db.OpdRegisters.Find(id);
                patient = opdRegister.Patient;
            }
            else if (idType == "PatientId")
            {
                //direct lab request
                patient = db.Patients.Find(id);

                opdRegister = patient.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault();
                if(opdRegister == null || opdRegister.Date.Date != DateTime.Today)
                {
                    //open a new opd number
                    opdRegister = new OpdRegister();

                    opdRegister.Status = "draft";
                    opdRegister.PatientId = patient.Id;
                    opdRegister.TariffId = db.Tariffs.FirstOrDefault(e => e.TariffName.ToLower() == "cash").Id;
                    opdRegister.IsIPD = false;
                    opdRegister.Discharged = false;
                    opdRegister.TimeAdded = DateTime.Now;
                    opdRegister.Username = (int)Session["UserId"];
                    opdRegister.Date = DateTime.Today;
                    db.OpdRegisters.Add(opdRegister);
                    db.SaveChanges();
                }

            }
            


            data.TestBillServices = db.BillServices.Where(e => e.OPDNo == opdRegister.Id &&
            (e.Service.IsLAB || e.Service.IsXRAY)).ToList();
            data.OpdRegister = opdRegister;
            data.Patient = patient;
            return PartialView(data);
        }

        public ActionResult OutsideTest(OutsideTest test)
        {
            test.UserId = (int)Session["UserId"];
            test.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.OutsideTests.Add(test);
                db.SaveChanges();
            }

            return PartialView("OutsideTest", db.OutsideTests.Where(e => e.OPDId == test.OPDId).ToList());
        }

        public ActionResult DeleteOutsideTest(int id)
        {
            var test = db.OutsideTests.Find(id);
            db.OutsideTests.Remove(test);
            db.SaveChanges();
            return PartialView("OutsideTest", db.OutsideTests.Where(e => e.OPDId == test.OPDId).ToList());
        }

        public ActionResult ReferalForm(int id)
        {
            ViewBag.OPDNo = id;
            return PartialView(db.Departments.ToList());
        }

        public ActionResult DiseaseEntryForm(int? id)
        {
            DiseaseEntryFormData data = new DiseaseEntryFormData();
            data.Diseases = db.Diseases.ToList();
            data.OpdRegister = db.OpdRegisters.Find(id);
            return PartialView(data);
        }

        public ActionResult FilterDisease(string search)
        {
            var data = new List<Disease>();

            if (search.Trim() == "")
            {
               data = db.Diseases.ToList();
            }
            else
            {
                data = db.Diseases.Where(e => e.DiseaseName.ToLower().Trim().Contains(search.ToLower().Trim())).ToList();
            }

            return PartialView("DiseasesList", data);
        }

        public ActionResult TreatmentForm(int? id)
        {
            TreatmentFormData data = new TreatmentFormData();
            var opdEntry = data.OPDRegister = db.OpdRegisters.Find(id);
            if (opdEntry.PatientDiagnosis.Count() < 1)
            {
                return Json(new { status = "warning", message = "Please add a diagnosis for this patient, before proceeding to medication!" }, JsonRequestBehavior.AllowGet);
            }
            data.RoutingAdmins = db.RoutingAdmins.ToList();
            data.Frequencies = procDB.Dose.ToList();
            data.Complaint = db.Complaints.OrderByDescending(e => e.Id)
                .FirstOrDefault(i => i.OpdIpdNumber == id && !i.ChiefComplaints.StartsWith("[DELETED]"));

            return PartialView(data);
        }

        public JsonResult SearchDepartmentProcedures(String search, String departmentId)
        {
            var department = db.Departments.Find(int.Parse(departmentId));
            List<AutoCompleteData> autoData = department.Services.Where(e => e.ServiceName.Contains(search))
                .Select(x => new AutoCompleteData { Id = x.Id, Name = x.ServiceName }).ToList();
            return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
        public JsonResult SearchInvestigations(String search, String from, int id)
        {
            var isInsurace = false;
            var opd = db.OpdRegisters.Find(id);
            if (opd.Tariff.TariffName.ToLower().Trim() != "cash")
            {
                isInsurace = true;
            }

            List<int> availableLabTests = labDb.LabTests.Where(e => e.Available).Select(e => e.Id).ToList();

            if (from == "Lab")
            {
               List<AutoCompleteData> autoData = db.Services.Where
                (e => e.ServiceName.Contains(search) && e.IsLAB /*&& e.CashPrice>0*/).Select(x => new AutoCompleteData
               {
                   Id = x.Id,
                   Name = x.ServiceName,
                   Price = x.CashPrice,
                   AwardedAmount = x.ServicesPrices.FirstOrDefault(p => p.TariffId.Equals(opd.TariffId)).Award,
                   PayableAmount = x.CashPrice - 0,
                   Department = "Pathology",
                   TestCode = x.TestProfileCode,
                   Available = availableLabTests.Contains((int)x.LabId)
               }).ToList();
                return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else if (from == "Xray")
            {
               
                List<AutoCompleteData> autoData = db.Services.Where
                (e => e.ServiceName.Contains(search) && e.IsXRAY /*&& e.CashPrice>0*/).Select(x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.ServiceName,
                    Price = x.CashPrice,
                    AwardedAmount = 0.0,
                    PayableAmount = x.CashPrice - 0,
                    Department = "Radiology",
                    TestCode = x.TestProfileCode,
                    Available = availableLabTests.Contains((int)x.LabId)

                }).ToList();
                return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return null;
        }

        public JsonResult SearchDrugs(String search)
        {

            List<AutoCompleteData> autoData = procDB.Drug.Where(e => (e.IsActive == null || 
            (bool)e.IsActive) && e.Name.Contains(search)).Select(
                x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.Name /*+ " | "+x.GenericDrugName*/,
                    Price = 0,
                    InStock = 0,
                    Quantity = 1,
                    RoutingAdmin = x.RouteOfAdmin,
                    Frequency = ""

                }).Take(15).ToList();

            //get current stock for each drug
            var finalList = new List<AutoCompleteData>();

            var OnlyDrugsFromPharmacy = db.KeyValuePairs.FirstOrDefault(e =>
                e.Key_.Equals("OnlyDrugsFromPharmacy"));

            foreach (var item in autoData)
            {
                var myBatches = procDB.ItemMaster.Where(e => e.DrugId == item.Id);

                
                if (OnlyDrugsFromPharmacy == null)
                {
                    db.KeyValuePairs.Add(
                        new KeyValuePair() {
                            Key_ = "OnlyDrugsFromPharmacy",
                            Value = "No",
                            Owner = "Dev"
                        }
                    );

                    db.SaveChanges();
                }
                else if (OnlyDrugsFromPharmacy.Value.ToLower().Equals("yes"))
                {
                    myBatches = myBatches.Where(e => e.StoreName.Equals("P") && (e.ExpiryDate == null || (e.ExpiryDate < DateTime.Today)));
                }

                var drug = procDB.Drug.FirstOrDefault(e => e.Id == item.Id);
                if (drug.Dose != null)
                {
                    item.Quantity = drug.Dose.Quantity;
                    item.Frequency = drug.Dose.Name;
                }
                if (myBatches.Count() > 0)
                {
                    item.InStock = myBatches.Sum(e => e.CurrentStock);
                }

                finalList.Add(item);
            }

            if (OnlyDrugsFromPharmacy != null && OnlyDrugsFromPharmacy.Value.ToLower().Equals("yes"))
            {
                autoData.RemoveAll(e => e.InStock == 0);
            }

            return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public class MedicationFormData
        {
            public Medication Medication { get; set; }
            public String AllergiesInfo { get; set; }
        }
        public ActionResult EnterMedication(MedicationFormData data, string FormName)
        {
            var medication = data.Medication;
            medication.OpdRegister = db.OpdRegisters.Find(medication.OPDNo);
            //Get the drug tariff
            var DrugTariff = procDB.DrugTariffs.FirstOrDefault(d => d.TariffId
            == data.Medication.OpdRegister.TariffId && d.DrugId == medication.DrugId);

            medication.UnitPrice = (DrugTariff?.DrugUnitPrice)??0;

            medication.TimeAdded = DateTime.Now;
            medication.UserId = int.Parse(Session["UserId"].ToString());
            
            medication.BranchId = (int)Session["UserBranchId"] ; ;
            medication.Subtotal = medication.UnitPrice * medication.Quantity;
            medication.Paid = false;
            medication.Issued = false;
            medication.Award = 0;
            medication.Available = false;
            if (medication.DrugId != 0)
            {

                medication.QuantityIssued = medication.Quantity;
            }
            db.Medications.Add(medication);

            if (data.AllergiesInfo != null)
            {

                var MyAllergies = new PatientAllergy();
                MyAllergies.PatientId = db.OpdRegisters.Find(medication.OPDNo).Patient.Id;
                MyAllergies.AllergyDescription = data.AllergiesInfo;
                MyAllergies.BranchId = (int)Session["UserBranchId"] ;
                MyAllergies.UserId = medication.UserId;
                MyAllergies.DateAdded = DateTime.Now;
                var savedAllergyInfo = db.PatientAllergies.FirstOrDefault
                    (e => e.PatientId.Equals(MyAllergies.PatientId) && e.AllergyDescription == data.AllergiesInfo);
                if (savedAllergyInfo == null)
                {
                    db.PatientAllergies.Add(MyAllergies);
                }

            }

            db.SaveChanges();

            if (FormName != null && FormName == "NurseBilling")
            {
                return RedirectToAction("NurseBilling", "NurseModule", new { id = medication.OPDNo });
            }
            return RedirectToAction("TreatmentForm", new { id = medication.OPDNo });
        }

        public ActionResult ProcedureForm(int? id, int? selectedDepartment)
        {
            ProcedureFormData data = new ProcedureFormData();
            data.OPDRegister = db.OpdRegisters.Find(id);
            data.Departments = db.Departments.Where(e => e.DepartmentType1.DepartmnetType.ToLower().Trim().Equals("revenue")).ToList();
            if (selectedDepartment != 0)
            {
                ViewBag.SelectedDept = db.Departments.Find(selectedDepartment);
            }
            return PartialView(data);
        }

        public ActionResult AllergyForm(int? id)
        {

            return PartialView();
        }

        public ActionResult PatientEbook(int? id)
        {
            var entry = db.OpdRegisters.Find(id);
            ViewBag.CompanyName = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("FacilityName")).Value;
            return PartialView(entry);
        }

        public ActionResult DiagnosisForm(int? id)
        {
            DiagnosisFormData data = new DiagnosisFormData();
            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Diagnosis1s = db.Diagnosis1s.ToList();

            return PartialView(data);
        }

        public ActionResult GetLevelTwoClassifications(string level1code)
        {
            var level2s = db.Diagnosis2s.Where(e => e.icd_id == level1code).ToList();
            var results = "";
            foreach (var l2 in level2s)
            {
                results += "<input class='level2' id='diagno" + l2.Id + "-l2' type='checkbox' value='" + l2.Level2_id + "' data-diagnosis='" + l2.Level2_id + " " + l2.diagno_nameL2 + "'/> " +
                    "<label class='level2' data-value='" + l2.Level2_id + "'  data-slide-target='#" + l2.Id + "-children' >" + l2.Level2_id + " " + l2.diagno_nameL2 + " </label>" +
                    "<div id='" + l2.Id + "-children' class='collapse' style='border-left:1px solid var(--primary-color); margin-left:.5em; padding:.4em;'></div><br>";
            }

            return Content(results);

        }

        public ActionResult GetLevelThreeClassifications(string level2code)
        {
            var level3s = db.Diagnosis3s.Where(e => e.Level2_id == level2code).ToList();
            var results = "";
            foreach (var l3 in level3s)
            {
                results += "<input class='level3' id='diagno" + l3.Id + "-l3' type='checkbox' value='" + l3.Level3_id + "' data-diagnosis='" + l3.Level3_id + " " + l3.diagno_nameL3 + "' /> " +
                    "<label for='diagno" + l3.Id + "-l3'>" + l3.Level3_id + " " + l3.diagno_nameL3 + " </label><br>";
            }

            return Content(results);

        }

        public ActionResult SaveFinalDiagnosis(FinalDiagnosisData data, int? id)
        {
            //pass 1 to return list of this OPDIPD diagnosis
            foreach (var d in data.FinalDIagnosis)
            {
                PatientDiagnosi diagno = new PatientDiagnosi();
                
                diagno.BranchId = (int)Session["UserBranchId"] ;
                diagno.FinalDiagnosis = d;
                diagno.OPDNo = data.OPDNo;
                diagno.TimeAdded = DateTime.Now;
                diagno.UserId = int.Parse(Session["UserId"].ToString());
                diagno.OldNewCase = data.OldNewCase;
                db.PatientDiagnosis.Add(diagno);
                db.SaveChanges();
            }
            if (id != null && id == 1)
            {
                return RedirectToAction("DiseaseEntryForm", new { id = data.OPDNo });
            }
            return Content("Saved Successfully!");
        }

        public ActionResult Systemexamination(int? id)
        {
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = opdDetails.Patient;
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            emr_opd_data.SystemExamCategories = db.SystemExamCategories.ToList();
            ViewBag.SystemParticulars = db.SystemExamParticulars.ToList();
            return View(emr_opd_data);
        }

        public class SystemicResult
        {
            public int ParticularId { get; set; }
            public String ParticularQuiz { get; set; }
            public String NormalAbnormal { get; set; }
            public String Findings { get; set; }
        }

        public class BulkSystemicResult
        {
            public List<SystemicResult> SystemicResult { get; set; }
            public int OPDNo { get; set; }
        }



        [HttpPost]
        public ActionResult SaveSystemicExam(BulkSystemicResult data)
        {
            var loggedUser = int.Parse(Session["UserId"].ToString());
            foreach (var d in data.SystemicResult)
            {
                var entry = new SystemicExamResult();
                entry.BrachId = 1;
                entry.OPDNo = data.OPDNo;
                entry.DateAdded = DateTime.Now;
                entry.NormalAbnormal = d.NormalAbnormal;
                entry.ParticularId = d.ParticularId;
                entry.ParticularQuiz = d.ParticularQuiz;
                entry.Findings = d.Findings;
                entry.UserId = loggedUser;
                db.SystemicExamResults.Add(entry);
            }

            db.SaveChanges();
            return Json(new { Message = "Systemic Results Saved Successfully! See Ebook.", Status = "success" });
        }

        public ActionResult History(int? id)
        {
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = opdDetails.Patient;
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            emr_opd_data.MedHistoryQuestions = db.MedicalHistoryQuestions.ToList();
            emr_opd_data.FamilyMedHistQuestions = db.FamilyMedicalHistoryQuestions.ToList();
            return View(emr_opd_data);
        }

        public ActionResult BriefMedicationFollowUp(int? id)
        {
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = opdDetails.Patient;
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            emr_opd_data.SystemExamCategories = db.SystemExamCategories.ToList();
            return View(emr_opd_data);

        }

        public ActionResult BioMetricId()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult SearchRegularComplaints(string SearchText)
        {
            var lastBit = SearchText.Split(',').Last().Trim();
            var complaints = db.RegularComplaints.Where(e => e.ComplaintName.Trim().ToLower()
            .StartsWith(lastBit.ToLower())).Select(s => new
            {
                RComplaint = s.ComplaintName
            }).ToList();

            return Json(complaints, JsonRequestBehavior.AllowGet);
        }

        public void AddToRegularComplaints(string complaints)
        {
            var loggedUser = int.Parse(Session["UserId"].ToString());
            var splitComplints = complaints.Split(',');
            foreach( var c in splitComplints)
            {
                if (c.Trim().Length > 1)
                {
                    var regCom = db.RegularComplaints.FirstOrDefault(e => e.ComplaintName == c);
                    if(regCom == null)
                    {
                        regCom = new RegularComplaint();
                        regCom.ComplaintName = c;
                        regCom.DateAded = DateTime.Now;
                        
                        regCom.BranchId = (int)Session["UserBranchId"] ;
                        regCom.UserId = loggedUser;
                        db.RegularComplaints.Add(regCom);
                        db.SaveChanges();
                    }
                }
            }
        }

        public ActionResult RecommendDischargeForm(int? id)
        {
            OpdRegister opdDetails = db.OpdRegisters.Find(id);

            return PartialView(opdDetails);
        }

        [HttpPost]
        public ActionResult SaveDischargeRecommendation(DischargeRecommendation reco)
        {
            
            reco.BranchId = (int)Session["UserBranchId"] ;
            reco.UserId = int.Parse(Session["UserId"].ToString());
            reco.DateAdded = DateTime.Now;
            reco.Status = "Pending";

            db.DischargeRecommendations.Add(reco);
            db.SaveChanges();
            return Json(new { Status = "Success", Message = "Discharge recommended successfully!" });
        }

        public ActionResult MedicationAdministrationRecord(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = db.Patients.FirstOrDefault(e => e.RegNumber.Equals(opdDetails.Patient.RegNumber));
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;


            emr_opd_data.Complaints = db.Complaints.OrderByDescending(e => e.Id).Where(e =>
                e.PatientId == patient.Id).ToList();
            if (patient == null)
            {
                return RedirectToAction("IPDRegister", "IPD"); //opd waiting list
            }
            ViewBag.OpdNo = id;
            ViewBag.Diagnosis = opdDetails.PatientDiagnosis.ToList();
            emr_opd_data.TodaysComplaint = opdDetails.Complaints.LastOrDefault();
            return View(emr_opd_data);
        }


        public class TestRequest
        {
            public String Department { get; set; }
            public String OrderPriority { get; set; }
            public int ServiceId { get; set; }
        }

        public class WorkOrderRequest
        {
            public int OPDIPDNo { get; set; }
            public string DoctorNotes { get; set; }
            public List<TestRequest> Tests { get; set; }
        }
        [HttpPost]
        public ActionResult SendTestRequest(WorkOrderRequest wor)
        {
            if (wor.Tests.Count() < 1)
            {
                return Json(new { message = "Nothing to send!" });
            }
            var dep = db.Departments.FirstOrDefault(e => e.DepartmentName.ToLower().Trim().Equals("lab")
              || e.DepartmentName.ToLower().Trim().Equals("pathology")).Id;

            WorkOrder wo = new WorkOrder();
            wo.OPDNo = wor.OPDIPDNo;
            if (db.OpdRegisters.Find(wor.OPDIPDNo).IsIPD)
            {
                wo.OPDType = "IPD";
            }
            else
            {
                wo.OPDType = "OPD";
            }

            //Add Pathology PatientTurnAroundTime
            new Utils.TurnAroundTime().insert(
                        new PatientTurnAroundTime()
                        {
                            OPDId = wor.OPDIPDNo,
                            RequestTime = DateTime.Now,
                            FullfilmentTime = null,
                            Department = "pathology",
                            SearvedByUserId = (int)Session["UserId"],
                            FacilityId = 1,
                        }
                    );

            wo.DepartmentRadPath = dep; //move this to individual tests
            wo.BillPaid = 1;
            wo.Doctor = (int)Session["UserId"];
            wo.CreatedUtc = DateTime.Now;
            wo.DepartmentRadPath = dep;
            wo.Accession_Status = labDb.Status.FirstOrDefault(e => e.StatusValue.ToLower().Trim().Equals("default")).Id;
            wo.DoctorsNotes = wor.DoctorNotes;

            labDb.WorkOrders.Add(wo);

            labDb.SaveChanges();

            foreach (var t in wor.Tests)
            {
                WorkOrderTest test = new WorkOrderTest();

                test.WorkOrder = wo.Id;
                test.Test = (int)db.Services.Find(t.ServiceId).LabId;
                test.BillPaid = false;
                test.CreatedUtc = DateTime.Now;
                test.Status = labDb.Status.FirstOrDefault(e => e.StatusValue.ToLower().Trim().Equals("default")).Id;
                test.DepartmentRadPath = dep; //pick from incoming data

                labDb.WorkOrderTests.Add(test);
                labDb.SaveChanges();

                //add to bill
                BillServiceData bsData = new BillServiceData();

                bsData.ServiceId = t.ServiceId;
                bsData.Quantity = 1;
                bsData.DepartmentId = dep;
                bsData.AwardAmount = 0;
                bsData.PayableAmount = 0;
                bsData.OPDNo = wor.OPDIPDNo;
                bsData.WorkOrderTestId = test.Id;
                new RegistrationController().AddItemToBill(bsData, (int)Session["UserId"]);
            }

            return Json(new { message = "success" });
        }

        public ActionResult EMRQuestionnaire(int id)
        {
            var questionnaire = db.Questionnaires.Find(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView(questionnaire);
            }
            return View(questionnaire);
        }

        [HttpPost]
        public ActionResult DeleteDrug(int id)
        {
            var loggedUser = int.Parse(Session["UserId"].ToString());
            var targetDrug = db.Medications.Find(id);
            if (targetDrug == null)
            {
                return Json(new { status = "success" }); //the drug is already deleted and nolonger in the db
            }

            if (targetDrug.Issued || targetDrug.Paid)
            {
                return Json(new { status = "info", message = "Sorry! You cannot delete this drug because it has already been issued." });
            }

            if (targetDrug.UserId == loggedUser)
            {
                db.Medications.Remove(targetDrug);
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "danger", message = "Error! You cannot delete this drug because you did not prescribe it." });
            }
        }

        [HttpPost]
        public ActionResult DeleteBillService(int id)
        {
            var loggedUser = int.Parse(Session["UserId"].ToString());
            var targetService = db.BillServices.Find(id);
            if (targetService == null)
            {
                return Json(new { status = "success" }); //the service is already deleted and nolonger in the db
            }

            if (targetService.Offered)
            {
                return Json(new { status = "info", message = "Sorry! You cannot delete this item because it has already been offered." });
            }

            if (targetService.Paid)
            {
                return Json(new { status = "info", message = "Sorry! You cannot delete this item because it has already been paid for." });
            }

            if (targetService.UserId == loggedUser)
            {
                db.BillServices.Remove(targetService);
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            else
            {
                return Json(new { status = "danger", message = "Error! Unauthorized request." });
            }
        }

        public ActionResult DeleteTestRequest(int id)
        {
            var testService = db.BillServices.Find(id);
            var request = labDb.WorkOrderTests.Find(testService.WorkOrderTestId);
            var testServiceId = testService.Service.Id;

            if (request == null)
            {
                db.IPDBillPartialPayments.RemoveRange(testService.IPDBillPartialPayments);
                db.BillServices.Remove(testService);
                db.SaveChanges();

                return Json(new
                {
                    status = "success",
                    message = "The test request was canceled successfully!",
                    TestServiceId = testServiceId
                }, JsonRequestBehavior.AllowGet);
            }


            if (request.WorkOrder1.ShowInSpecimentCollection == null || !(bool)request.WorkOrder1.ShowInSpecimentCollection)
            {
                //delete
                labDb.WorkOrderTests.Remove(request);
                labDb.SaveChanges();
                if (request.BillPaid)
                {
                    //trigger refund
                }
                db.IPDBillPartialPayments.RemoveRange(testService.IPDBillPartialPayments);
                db.BillServices.Remove(testService);
                db.SaveChanges();

                return Json(new
                {
                    status = "success",
                    message = "The test request was canceled successfully!",
                    TestServiceId = testServiceId
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    status = "warning",
                    message = "Unable to delete the request! It has been commenced already."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveReferalDetails(PatientReferal data)
        {
            data.UserId = int.Parse(Session["UserId"].ToString());
            
            data.BranchId = (int)Session["UserBranchId"] ;
            data.DateAdded = DateTime.Now;

            db.PatientReferals.Add(data);
            db.SaveChanges();

            return Json(new { status = "success", message = "Referal request sent successfully!" });

        }


        public ActionResult CorneaClinic(int? id, string log="")
        {
            if (id == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = opdDetails.Patient;

            if (log == "true")
            {
                var data = db.CorneaClinicEntries.Where(e => e.OpdRegister.PatientId == patient.Id).ToList();
                return PartialView("CorneaClinicLog", data);
            }
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            if (patient == null)
            {
                return RedirectToAction("opdlist"); //opd waiting list
            }
            if (opdDetails.IsIPD)
            {
                ViewBag.IsIPD = true;
            }
            ViewBag.OpdNo = id;
            return View(emr_opd_data);
        }
        [HttpPost]
        public ActionResult SaveCorneaEntry(CorneaClinicEntry data)
        {
            data.UserId = (int)Session["UserId"];
            
            data.BranchId = (int)Session["UserBranchId"] ;
            data.DateAdded = DateTime.Now;
            
            db.CorneaClinicEntries.Add(data);
            db.SaveChanges();

            return Json(new { Status = "success", Message = "Saved Successfully!"});
        }

        public ActionResult DeleteComplaint(int id, string reason)
        {
            var chiefComplaint = db.Complaints.Find(id);
            var loggedInUser = db.Users.Find((int)Session["UserId"]);
            if(loggedInUser.Id != chiefComplaint.DoctorId)
            {
                return Json(new { Status = "danger", Message = "Sorry! You cannot delete this record. Only the person who recorded can do so." }, 
                    JsonRequestBehavior.AllowGet);
            }

            if (chiefComplaint.ComplaintDate != DateTime.Today)
            {
                return Json(new { Status = "danger", Message = "Sorry! This record cannot be deleted today" },
                    JsonRequestBehavior.AllowGet);
            }
            chiefComplaint.ChiefComplaints = "[DELETED]["+reason+"]["+DateTime.Now+"]["+loggedInUser.Username+"]"+ chiefComplaint.ChiefComplaints;

            db.SaveChanges();

            return Json(new { Status = "success", Message = "Record deleted successfully!" },
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDiagnosis(int id)
        {
            var diagnosis = db.PatientDiagnosis.Find(id);
            var loggedInUser = db.Users.Find((int)Session["UserId"]);
            if (loggedInUser.Id != diagnosis.UserId)
            {
                return Json(new { Status = "danger", Message = "Sorry! You cannot delete this record. Only the person who recorded can do so." },
                    JsonRequestBehavior.AllowGet);
            }

            if (diagnosis.TimeAdded.Date != DateTime.Today)
            {
                return Json(new { Status = "danger", Message = "Sorry! This record cannot be deleted today" },
                    JsonRequestBehavior.AllowGet);
            }
            db.PatientDiagnosis.Remove(diagnosis);

            db.SaveChanges();

            return Json(new { Status = "success", Message = "Record deleted successfully!" },
                    JsonRequestBehavior.AllowGet);
        }


        public ActionResult RecommendAdmissionForm(int id, string stage = "")
        {
            var data =  db.OpdRegisters.Find(id);
            ViewBag.Stage = stage;
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult SaveRecommendAdmission(AdmissionRecommendation data)
        {
            var pat = db.OpdRegisters.Find(data.OPDNo).Patient;
            var myRecomendation = db.AdmissionRecommendations.FirstOrDefault(e => 
            (e.OPDNo == data.OPDNo || 
            (e.OpdRegister.PatientId==pat.Id))
            && e.Status == "Pending");

            if (myRecomendation != null)
            {
                myRecomendation.AdmissionNotes = data.AdmissionNotes;
                myRecomendation.UserId = (int)Session["UserId"];
                myRecomendation.TimeEdited = DateTime.Now;
                db.SaveChanges();
                return Json(new { Status = "info", Message = "Record updated successfully!" },
                        JsonRequestBehavior.AllowGet);
            }

            data.UserId = (int)Session["UserId"];
            data.TimeAdded = DateTime.Now;
            data.Status = "Pending";
            
            data.BranchId = (int)Session["UserBranchId"] ;
            db.AdmissionRecommendations.Add(data);
            db.SaveChanges();
            return Json(new { Status = "success", Message = "Record saved successfully!" },
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult Immunization(int id)
        {
            OpdRegister opdDetails = db.OpdRegisters.Find(id);
            Patient patient = opdDetails.Patient;
            EMR_OPD_Data emr_opd_data = new EMR_OPD_Data();
            emr_opd_data.Patient = patient;
            emr_opd_data.OpdRegister = opdDetails;
            
            ViewBag.ImmunizationCategories = db.ImmunizationCategories.ToList();
            return View(emr_opd_data);
        }

        public ActionResult ImmunizationEntry(int id, int immId)
        {
            ViewBag.Immunization = db.ImmunizationMasters.Find(immId);
            return PartialView(db.OpdRegisters.Find(id));
        }

        [HttpPost]
        public ActionResult SaveImmunizationEntry(ImmunizationAdmin data)
        {
            
            data.BranchId = (int)Session["UserBranchId"] ;
            data.UserId = (int)Session["UserId"];
            db.ImmunizationAdmins.Add(data);
            db.SaveChanges();
            
            return Json(new { Status="success",
                Message = "Immunization entry saved successfully!",
                Entry = data, DateGiven = data.DateGiven.ToShortDateString(),
                NextVisit = data.DateOfNextVisit.Value.ToShortDateString()
            });
        }

        public ActionResult ApplyProcedure(int id)
        {
            var procedure = db.BillServices.Find(id);
            if (!procedure.Paid)
            {
                return Json(new { Status = "warnig", Message = "This procedure has not been paid for." }, JsonRequestBehavior.AllowGet);

            }
            procedure.Offered = true;
            procedure.OfferedBy = (int)Session["UserId"];
            db.SaveChanges();

            return Json(new { Status = "success", Message = "Procedure applied successfully!" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TBScreeningForm(int id)
        {
            var adultQuizes = new string[]{ "COUGH OF ANY DURATION?", "FEVER?", "ANY NOTICEABLE WEIGHT LOSS?",
       "IS THE PATIENT CURRENTLY ON TB TREATMENT?", "HAS THE PATIENT EVER BEEN ON TB TREATMENT?" };

            var childQuizes = new string[]{ "COUGH OF ANY DURATION?", "FEVER?", "FAILURE TO THRIVE OR POOR WEIGHT GAIN?",
        "ANY NOTICEABLE WEIGHT LOSS?", "LRTHARGY, LESS PLAYFUL THAN USUAL?", "HISTORY OF CONTACT WITH A KNOWN TB PATIENT OR CHRONIC COUGH?",
       "IS THE PATIENT CURRENTLY ON TB TREATMENT?", "HAS THE PATIENT EVER BEEN ON TB TREATMENT?" };

            var tbQuizez = db.TBScreeningQuestions.ToList();
            if (tbQuizez.Count()< (adultQuizes.Count() + childQuizes.Count())){
                //seed missing
                foreach(var aq in adultQuizes)
                {
                    if (tbQuizez.Count()==0 || tbQuizez.First(e => e.Question.ToUpper().Trim() ==aq.Trim().ToUpper() && e.IsAdult) == null)
                    {
                        //insert this
                        var quiz = new TBScreeningQuestion();
                        quiz.IsAdult = true;
                        quiz.DateAdded = DateTime.Now;
                        quiz.Question = aq;
                        db.TBScreeningQuestions.Add(quiz);
                    }
                }


                foreach (var cq in childQuizes)
                {
                    if (tbQuizez.Count() == 0 || tbQuizez.First(e => e.Question.ToUpper().Trim() == cq.Trim().ToUpper() && !e.IsAdult) == null)
                    {
                        //insert this
                        var quiz = new TBScreeningQuestion();
                        quiz.IsAdult = false;
                        quiz.DateAdded = DateTime.Now;
                        quiz.Question = cq;
                        db.TBScreeningQuestions.Add(quiz);
                    }
                }

                db.SaveChanges();
            }
            ViewBag.TBQuizez = db.TBScreeningQuestions.ToList();
            return PartialView(db.OpdRegisters.Find(id));
        }

        public class BulkTBScreeningData
        {
            public List<TBScreeningRespons> Responses { get; set; }
        }

        [HttpPost]
        public ActionResult SaveTBScreeningData(BulkTBScreeningData data)
        {
            foreach (var r in data.Responses)
            {
                r.DateAdded = DateTime.Now;
                
                r.BranchId = (int)Session["UserBranchId"] ;
                r.UserId = (int)Session["UserId"];

                db.TBScreeningResponses.Add(r);
            }

            db.SaveChanges();
            return Json(new { Status = "success", Message = "TB Screenin details saved successfully!"});
        }

        public ActionResult PrintOutsideTest(int Id, string type)
        {


            DSOutsideTest dataSet = new DSOutsideTest();

            var Opd = db.OpdRegisters.Find(Id);

            dataSet.Patient.AddPatientRow(
                Opd.Patient.Salutation + " " + Opd.Patient.FName + " " + Opd.Patient.LName,
                Opd.Tariff.Company.CompanyName,
                Opd.Tariff.TariffName,
                Opd.Patient.RegNumber,
                Opd.Id.ToString()
            );

            var id = 1;
            foreach (var item in Opd.OutsideTests.ToList())
            {
                dataSet.OutsideTest.AddOutsideTestRow(
                    id.ToString(),
                    item.UserId.ToString(),
                    item.User.Username,
                    item.Date.ToString(),
                    item.CreatedDate.ToString(),
                    item.TestName,
                    item.ClinicalComments
                );

                id = id + 1;
            }

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/OutsideTest/RPTOT.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportHeader());
            Rd.Subreports["RptReportFooter.rpt"].SetDataSource(HeaderAndFooterForReports.GetAllReportFooter());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);
            string FileName = Opd.Patient.FName + " " + Opd.Patient.LName + " Outside Test "  + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

            return File(Stream, "application/pdf", FileName);

        }
        
        public ActionResult HTCService(int? id)
        {
           
            var data = new EMR_OPD_Data();
            data.OpdRegister = db.OpdRegisters.Find(id);
            data.Patient = data.OpdRegister.Patient;
            ViewBag.OpdNo = id;
            return View(data);

        }
       
        [HttpPost]
        public ActionResult SaveHTCServiceSummaryData(HTCServiceSummary data)
        {
            
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = (int)Session["UserId"];
            db.HTCServiceSummaries.Add(data);

            db.SaveChanges();

            return RedirectToAction("HTCService");
            

        }
        
    }
}