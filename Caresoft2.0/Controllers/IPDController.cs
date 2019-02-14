using System.Linq;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using Caresoft2._0.CustomData;
using System.Collections.Generic;
using System;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class IPDController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET
        public ActionResult Index()
        {
            var data = db.HSWardCategories.ToList();
            if(data==null || data.Count < 1)
            {
                return RedirectToAction("Categories", "Wards");
            }
            return View(data);
        }

        public ActionResult BedsGrid(int? id)
        {
            var wards = db.HSWardCategories.Find(id).HSWards.ToList();

            return PartialView(wards);
        }

        public ActionResult IPDAdmissionForm(int? id)
        {

            ViewBag.CompanyTypes = db.CompanyTypes.ToList();
            ViewBag.RelationshipsOptions = db.Relationships.ToList();
            ViewBag.Companies = db.Companies.ToList();
            var data = new AdmissionFormData();
            data.Bed = db.HSBeds.Find(id);
            data.PatientMainCategories = db.CompanyTypes.ToList();
            return PartialView(data);
        }

        public ActionResult SearchPatient(string search)
        {
            search = search.ToLower().Trim();
            List<PatientsAutoCompleteData> patients = db.Patients.Where(e => e.RegNumber.Contains(search) ||
            e.FName.Contains(search) || e.MName.Contains(search) || e.LName.Contains(search)).Select(
                x => new PatientsAutoCompleteData
                {
                    PatientId = x.Id,
                    RegNumber = x.RegNumber,
                    FName = x.FName.Trim(),
                    MName = x.MName.Trim(),
                    LName = x.LName.Trim(),
                    PhoneNumber = x.Mobile.Trim(),
                    
                    Name = x.Salutation.Trim() + " " + x.FName.Trim() + " " + x.MName.Trim() + " " + x.LName.Trim()
                }).Take(15).ToList();
            return new JsonResult { Data = patients, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class DoctorsAutoCompleteData
        {
            public int DoctorId { get; set; }
           
            public string Name { get; set; }
        }

        public ActionResult SearchDoctors(string search)
        {
            //search = search.ToLower().Trim();
            List<DoctorsAutoCompleteData> doctors = db.Employees.Where(e =>  e.FName.Contains(search) ||
            e.OtherName.Contains(search) && e.Designation.DesignationName.ToLower() == "doctor").Select(
                x => new DoctorsAutoCompleteData
                {
                    DoctorId = x.Id,
                    Name = x.Salutation + " " + x.FName + " " + x.OtherName 
                }).ToList();
            return new JsonResult { Data = doctors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult AdmitPatient (OpdRegister register, string autoBedAndAdmission = "false", string type = "Cash")
        {
            var myActiveAdmissions = db.OpdRegisters.Where(e => e.PatientId == register.PatientId && e.IsIPD && !e.Discharged).ToList();

            var IPDNo = 0;

            if (myActiveAdmissions.Count()<1)
            {
                register.IsIPD = true;
                
                register.BranchId = (int)Session["UserBranchId"] ;
                register.TimeAdded = DateTime.Now;
                register.Status = "Active";
                register.Date = (DateTime)register.AdmissionDate;
                //TODO changeUesrname to int
                register.Username = int.Parse(Session["UserId"].ToString());
                db.OpdRegisters.Add(register);
                db.SaveChanges();
                IPDNo = register.Id;
                if (autoBedAndAdmission == "true")
                {
                    var billItem = new BillService();
                    var ipdDepartment = db.Departments.FirstOrDefault(e => e.DepartmentName.Trim().ToLower().Equals("ipd"));
                    if (ipdDepartment == null)
                    {
                        ipdDepartment = new Department();
                        ipdDepartment.DepartmentName = "IPD";
                        ipdDepartment.DepartmentType = 2;
                        ipdDepartment.IsMedical = "No";
                        ipdDepartment.DateAdded = DateTime.Now;
                        db.Departments.Add(ipdDepartment);
                        db.SaveChanges();
                    }


                    billItem.DepartmentId = ipdDepartment.Id;
                    var admissionService = db.Services.FirstOrDefault(e => e.ServiceName.Trim().ToLower().Contains("admission"));
                    if (admissionService == null)
                    {
                        admissionService = new Service();
                        admissionService.ServiceName = "Admission";
                        admissionService.BranchId = (int)Session["UserBranchId"] ;
                        admissionService.CashPrice = 800;
                        admissionService.IsIPD = true;
                        admissionService.DepartmentId = billItem.DepartmentId;
                        admissionService.DateAdded = DateTime.Now;
                        admissionService.ServiceGroupId = db.ServiceGroups.FirstOrDefault(e => e.ServiceGroupName.ToLower().Trim() == "procedure").Id;
                        admissionService.UserId = int.Parse(Session["UserId"].ToString());

                        db.Services.Add(admissionService);
                        db.SaveChanges();
                    }
                    billItem.SeviceId = admissionService.Id;
                    billItem.ServiceName = admissionService.ServiceName;

                    billItem.OPDNo = register.Id;
                    var bed = db.HSBeds.Find(register.BedId);
                    var pricing = db.WardTypeCharges.FirstOrDefault(e => e.ServiceId.Equals(admissionService.Id)
                    && e.WardCategoryId.Equals(bed.HSWard.CategoryId));
                    if (pricing != null)
                    {
                        billItem.Price = pricing.Price;
                    }
                    else
                    {
                        billItem.Price = admissionService.CashPrice;
                    }

                    billItem.Quatity = 1;
                    var PayableAmount = billItem.Quatity * billItem.Price;

                    billItem.TariffId = register.TariffId;

                    var awardentry = db.ServicesPrices.FirstOrDefault(e => e.TariffId.Equals(register.TariffId)
                    && e.ServiceId.Equals(admissionService.Id));
                    billItem.Award = 0;
                    billItem.DoctorFee = 0;
                    if (awardentry != null)
                    {
                        billItem.Award = awardentry.Award * billItem.Quatity;
                        if (awardentry.AwardUnit.ToLower().Trim().Equals("percent"))
                        {
                            billItem.Award = PayableAmount * (1 + awardentry.Award / 100);
                        }
                    }
                    PayableAmount = PayableAmount - billItem.Award;
                    billItem.Paid = false;
                    billItem.Offered = true;
                    billItem.UserId = (int)Session["UserId"];
                    billItem.BranchId = (int)Session["UserBranchId"] ;
                    billItem.IsNurse = false;
                    billItem.DateAdded = DateTime.Now;
                    db.BillServices.Add(billItem);
                    db.SaveChanges();

                    AddBedCharges(register.Id);

                }
            }

            var res = new CustRes()
            {
                Type = "Cash",
                IPDNo = IPDNo
            };

            if (type == "Insurance")
            {
                res = new CustRes()
                {
                    Type = "Insurance",
                    IPDNo = IPDNo
                };
            }
            else if (type == "Corporate")
            {
                res = new CustRes()
                {
                    Type = "Corporate",
                    IPDNo = IPDNo
                };
            }
            else
            {
            }
            return Json(res);

            //return RedirectToAction("BedsGrid", new { id = db.HSBeds.Find(register.BedId).HSWard.HSWardCategory.Id });
        }

        class CustRes
        {
            public string Type { get; set; }
            public int IPDNo { get; set; }
        }
        [HttpPost]
        public int IPDSaveInsuranceDetails(InsuranceCard card)
        {
            var opdENtry = db.OpdRegisters.Find(card.OPDNo);
            opdENtry.TariffId = card.SchemeId;
            db.SaveChanges();

            card.DateAdded = DateTime.Now;
            db.InsuranceCards.Add(card);
            db.SaveChanges();
            return 1;


            //var billItems = db.BillServices.Where(e => e.OPDNo == card.OPDNo).ToList();


            //return PartialView("~/Views/Registration/ProvisionalBillServices.cshtml", billItems);

        }

        public int AddBedCharges(int? id)
        {
            var pat = db.OpdRegisters.Find(id);
            if (!pat.IsIPD)
            {
                return -1;  //patient is not an IDP
            }
            if (pat.Discharged)
            {
                return -2; //patient already discharged
            }

            //check if this patient has an existing 
            var billItem = new BillService();

            var bedService = db.Services.FirstOrDefault(e => e.ServiceName.Trim().ToLower().Equals("bed charges"));

            if (bedService == null)
            {
                bedService = new Service();
                bedService.ServiceName = "Bed Charges";
                bedService.ServiceGroupId = db.Services.FirstOrDefault().ServiceGroupId; //just pick any for now, it will be edited later
                bedService.DepartmentId = db.Departments.FirstOrDefault(e => e.DepartmentName.ToLower().Trim() == "ipd").Id;
                bedService.CashPrice = 1500;
                bedService.IsIPD = true;
                bedService.IsOPD = false;
                bedService.DateAdded = DateTime.Now;

                db.Services.Add(bedService);
                db.SaveChanges();
            }

            billItem.SeviceId = bedService.Id;
            billItem.ServiceName = bedService.ServiceName + "-"+ pat.HSBed.HSWard.HSWardCategory.WardCategoryName;
            billItem.DepartmentId = db.Departments.FirstOrDefault(e => e.DepartmentName.Trim().ToLower().Equals("ipd")).Id;
            billItem.OPDNo = pat.Id;

            billItem.Price =  bedService.CashPrice;
            //ward variation price
            var bed = pat.HSBed;
            var wardCategory = bed.HSWard.CategoryId;
            var variation = db.WardTypeCharges.FirstOrDefault
                (e => e.ServiceId.Equals(bedService.Id) && e.WardCategoryId.Equals(wardCategory));
            if (variation != null)
            {
                billItem.Price = variation.Price;
            }

            //billItem.Quatity = (DateTime.Now.Date - pat.AdmissionDate.Value.Date).TotalDays;
            billItem.Quatity = 1;

            var PayableAmount = billItem.Quatity * billItem.Price;

            billItem.TariffId = pat.TariffId;

            var awardentry = db.ServicesPrices.FirstOrDefault(e => e.TariffId.Equals(pat.TariffId)
            && e.ServiceId.Equals(bedService.Id));
            billItem.Award = 0;
            billItem.DoctorFee = 0;
            if (awardentry != null)
            {
                billItem.Award = awardentry.Award * billItem.Quatity;
                if (awardentry.AwardUnit.ToLower().Trim().Equals("percent"))
                {
                    billItem.Award = PayableAmount * (1 + awardentry.Award / 100);
                }
            }
            PayableAmount = PayableAmount - billItem.Award;
            billItem.Paid = false;
            billItem.Offered = true;
            billItem.UserId = (int)Session["UserId"];
            
            billItem.BranchId = (int)Session["UserBranchId"] ;
            billItem.IsNurse = false;
            billItem.DateAdded = DateTime.Now;
            db.BillServices.Add(billItem);
            db.SaveChanges();

            return 1;

        }

        public ActionResult IPDRegister()
        {
            var role = db.Users.Find((int)Session["UserId"]).UserRole.RoleName;
            if (role.ToLower().Trim().Equals("nurse"))
            {
                ViewBag.IsNurse = true;
            }
            return View(db.OpdRegisters.Where(e=>e.IsIPD && 
            e.Status.ToLower().Trim().Equals("active") &&
            !e.Discharged).ToList());
        }

        public ActionResult DischargeRecommended()
        {
            return View(db.OpdRegisters.Where(e => e.IsIPD && 
            e.Status.ToLower().Trim().Equals("active")
            && !e.Discharged
            && (e.DischargeRecommendations.FirstOrDefault
            (d=>d.Status.Trim().ToLower().Equals("pending")) != null)).ToList());
        }


        [HttpPost]
        public ActionResult ShiftIPD(string AdmissionId, string NewBed)
        {
            var admission = db.OpdRegisters.Find(int.Parse(AdmissionId));
            admission.BedId = int.Parse(NewBed);
            db.SaveChanges();
            return Content("Patient Shifted Successfully!");
        }

        public ActionResult AdministerMedication(int id)
        {
            var admin = new MedicalAdministrationEntry();
            admin.Applied = true;
            
            admin.BranchId = (int)Session["UserBranchId"] ;
            admin.DateApplied = DateTime.Now;
            admin.MedicationId = id;
            admin.UserId = (int)Session["UserId"];
            db.MedicalAdministrationEntries.Add(admin);
            db.SaveChanges();
            return Json(new { status = "success", lastAdmin = admin.DateApplied.ToString() }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddDoctorNotes(DoctorNote note)
        {
            
            note.BranchId = (int)Session["UserBranchId"] ;
            note.UserId = (int)Session["UserId"];
            note.DateAdded = DateTime.Now;
            db.DoctorNotes.Add(note);
            db.SaveChanges();

            return RedirectToAction("DoctorNotesList", new { id = note.IpdOpdNo });
        }

        public ActionResult DoctorNotesList(int id)
        {
            return PartialView(db.DoctorNotes.Where(e => e.IpdOpdNo.Equals(id)).OrderByDescending(e=>e.Id).ToList());
        }

        public ActionResult AdmitRecommended()
        {
            return View(db.AdmissionRecommendations.ToList());
        }

        public ActionResult GetWardCategoriesJson()
        {
            var data = db.HSWardCategories.Select(x => new { Id = x.Id, Name = x.WardCategoryName }).ToList();
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetWardsJson(int id)
        {
            var wards = db.HSWards.Where(e=>e.CategoryId == id).Select(x => new { Id = x.Id, Name = x.WardName }).ToList();
            return new JsonResult { Data = wards, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult GetBedsJson(int id)
        {
            var beds = db.HSBeds.Where(e => e.WardId == id).Select(x => new { Id = x.Id, Name = x.BedName }).ToList();
            return new JsonResult { Data = beds, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult FinalizeDischarge(int id)
        {
            var admission = db.OpdRegisters.Find(id);
            if(admission == null || admission.Status.ToLower() == "discharged")
            {
                return Json(new { Status = "success", Message = "Patient Discharged Successfully!" }, JsonRequestBehavior.AllowGet);
            }

            //if (admission.DischargeRecommendations.ToList().Count() < 1)
            //{
            //    return Json(new { Status = "warning", Message = "This patient has not been recommended for discharge. Please recommend discharge and try again." }, JsonRequestBehavior.AllowGet);
            //}

            if (admission.BillServices.Where(e=>!e.Paid).ToList().Count()>0)
            {
                return Json(new { Status = "warning", Message = "Unable to discharge patient due to unpaid bill." }, JsonRequestBehavior.AllowGet);
            }

            admission.Status = "Discharged";
            admission.DischargeDate = DateTime.Now;
            admission.Discharged = true;
            admission.DischargedBy = int.Parse(Session["UserId"].ToString());
            db.SaveChanges();


            return Json(new { Status = "success", Message = "Patient Discharged Successfully!" }, JsonRequestBehavior.AllowGet);
        }

     
    }
}