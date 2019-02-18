using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class NurseModuleController : Controller
    {
        private ProcurementDbContext procDB = new ProcurementDbContext();
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: NurseModule
        public ActionResult Index()
        {
            ViewBag.ShowTopMenu = true;
            ViewBag.IsNurse = true;
            ViewBag.Title = "Nurse Module";
            return View("~/Views/EMR/Index.cshtml");
        }

        public ActionResult OPDList()
        {
            ViewBag.FromDepartment = "Nurse";

            ViewBag.ShowTopMenu = true;
            ViewBag.IsNurse = true;
            ViewBag.MedicalDeps = db.Departments.Where(e => e.IsMedical != null && e.IsMedical.Trim().ToLower().Equals("yes")).ToList();
            return View("~/Views/EMR/OpdList.cshtml", db.OpdRegisters.Where(e => e.Date >= DateTime.Today
            && !e.IsIPD && !e.Status.Trim().ToLower().Equals("draft")).ToList());
            
        }

        public ActionResult NurseStationMaster()
        {
            return View();
        }

        public ActionResult ChamberMaster()
        {
            return View();
        }



        public ActionResult AssignConsultantToChamber()
        {
            return View();
        }

        public ActionResult AssignRoomToNurseStation()
        {
            return View();
        }


        public ActionResult AssignNurseStationToNurse(string ajax)
        {
            if (ajax != null && ajax.Length > 0)
            {
                ViewBag.IsAjax = true;
            }

            return View();
        }

        public ActionResult IssueForConsuption()
        {
            return View();
        }

        public ActionResult IndentForDept()
        {
            return View();
        }

        public ActionResult RequestForNurseStationForm()
        {
            return PartialView();
        }

        public ActionResult IndentStatus()
        {
            return View();
        }

        public ActionResult StockAdjustments()
        {
            return View();
        }
        public ActionResult AdjustStockForm()
        {
            return PartialView();
        }
        public ActionResult IssueVoucher()
        {
            return View();
        }

        public ActionResult RackMaster()
        {
            return View();
        }

        public ActionResult SetItemLocation()
        {
            return View();
        }

        public ActionResult ColdChainEquipment()
        {
            return View();
        }

        public ActionResult CleanupRequest()
        {

            ViewBag.Buildings = db.HSBuildings.ToList();
            ViewBag.Floors = db.HSFloors.ToList();
            ViewBag.Wards = db.HSWards.ToList();
            ViewBag.Beds = db.HSBeds.ToList();
            return View();
        }

        public ActionResult PendingCleanupRequest()
        {
            return View();
        }

        public ActionResult BedCleanupRequest()
        {
            return View();
        }

        public ActionResult ReturntoHouseKeepingStore()
        {
            return View();
        }

        public ActionResult RequestForLinen()
        {
            return View();
        }
           
        public ActionResult OTSchedule_Set()
        {
            return View();
        }
        public ActionResult OTSchedular_Approval()
        {
            return View();
        }
        public ActionResult EditOTSchedular_Approval()
        {
            return View();
        }
        public ActionResult ViewOTRequest()
        {
            return View();
        }
        public ActionResult ViewOTSchedule()
        {
            return View();
        }

        public ActionResult OTPatientSelection(string type)
        {
            string[] acceptedTypes = { "Emergency", "Cataractsurgery", "OTSearch" };
            if (acceptedTypes.Contains(type))
            {
                return View("OTPatient" + type);
            }
            else
            {
                return Content(type + " is not a correct OT Patient type.");
            }
            
        }


            
        public ActionResult AcceptCssdRequest()
        {
            return View();
        }
        public ActionResult IssueItemToPatient()
        {
            return View();
        }
        public ActionResult ReturnToCSSD()
        {
            return View();
        }
        public ActionResult RequestToCSSD()
        {
            return View();
        }

        //ambulatory

        public ActionResult AmbulanceMaster()
        {
            return View();
        }

        public ActionResult AssignDriver()
        {
            return View();
        }

        //Nurse Billing Form
        public ActionResult NurseBilling(int id)
        {
            TreatmentFormData data = new TreatmentFormData();
            data.Users = db.Users.ToList();

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

        public ActionResult AmbulanceRequest(int? id)
        {
            //AmbulanceFormData data = new AmbulanceFormData();
            //data.TestBillServices = db.BillServices.ToList();

            //ViewBag.RelationshipsOptions = db.Relationships.ToList();




            return PartialView();
        }

        [HttpPost]
        public ActionResult SaveAmbulanceRequest(AmbulanceRequest data)
        {

            //var OpdIpd = data.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault();
            data.UserId = (int)Session["UserId"];
            var user = db.Users.Find(data.UserId);
            data.RequestingNurse = user.Employee.FName + " " + user.Employee.OtherName + " ( " + user.Username + " ) " ;
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Assigned = false;

            db.AmbulanceRequests.Add(data);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = data.Id });
        }

        [HttpPost]
        public ActionResult SaveTheatrRequest(NurseTheatreRequest data)
        {

            //var OpdIpd = data.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault();
            data.UserId = (int)Session["UserId"];
            var user = db.Users.Find(data.UserId);
            data.RequestingNurse = user.Employee.FName + " " + user.Employee.OtherName + " ( " + user.Username + " ) ";
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Assigned = false;

            db.NurseTheatreRequests.Add(data);
            db.SaveChanges();

            return RedirectToAction("ViewOTRequest", new { id = data.Id });
        }


        public ActionResult SearchPatient(string search)
        {
            //search = search.ToLower().Trim();
            List<PatientsAutoCompleteData> patients = db.OpdRegisters.OrderByDescending(f => f.Id).Where(e => e.Patient.RegNumber.Contains(search) ||
            e.Patient.FName.Contains(search) || e.Patient.MName.Contains(search) || e.Patient.LName.Contains(search)).Select(
                x => new PatientsAutoCompleteData
                {
                    PatientId = x.Patient.Id,
                    OpdId = x.Id,
                    RegNumber = x.Patient.RegNumber,
                    Name = x.Patient.Salutation + " " + x.Patient.FName + " " + x.Patient.LName,
                    OpdDate = x.TimeAdded.ToString(),

                }).Take(15).ToList();

            var jsonResult = Json(patients, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

          
        }

        public class DoctorsAutoCompleteData
        {
            public int DoctorId { get; set; }

            public string Name { get; set; }
        }

        public ActionResult SearchDoctors(string search)
        {
            //search = search.ToLower().Trim();
            List<DoctorsAutoCompleteData> doctors = db.Employees.Where(e => e.FName.Contains(search) ||
            e.OtherName.Contains(search) && e.Designation.DesignationName.ToLower() == "doctor").Select(
                x => new DoctorsAutoCompleteData
                {
                    DoctorId = x.Id,
                    Name = x.Salutation + " " + x.FName + " " + x.OtherName
                }).ToList();
            return new JsonResult { Data = doctors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SearchRoute(string search)
        {
            //search = search.ToLower().Trim();
            List<AutoCompleteData> autoData = db.Services.Where(e => e.ServiceName.Contains(search) 
             && e.Department.DepartmentName.ToLower() == "ambulatory").Select(
                x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.ServiceName,
                    Price = x.CashPrice,
                    PayableAmount = x.CashPrice - 0,
                    Department = "Ambulance"
                }).ToList();
            return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SearchTheatre(string search)
        {
            //var opd = db.OpdRegisters.Find(id);
            //search = search.ToLower().Trim();
            List<AutoCompleteData> autoData = db.Services.Where(e => e.ServiceName.Contains(search)
             && e.Department.DepartmentName.ToLower() == "theatre").Select(
                x => new AutoCompleteData
                {
                    Id = x.Id,
                    Name = x.ServiceName,
                    Price = x.CashPrice,
                   // AwardedAmount = x.ServicesPrices.FirstOrDefault(p => p.TariffId.Equals(opd.TariffId)).Award,
                    PayableAmount = x.CashPrice - 0,
                    Department = "theatre"
                }).ToList();
            return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SearchInvestigations(String search, String from, int id)
        {
            

            if (from == "Ambulatory")
            {
                List<AutoCompleteData> autoData = db.Services.Where
                 (e => e.ServiceName.Contains(search) /*&& e.IsLAB && e.CashPrice>0*/).Select(x => new AutoCompleteData
                 {
                     Id = x.Id,
                     Name = x.ServiceName,
                     Price = x.CashPrice,
                    // AwardedAmount = x.ServicesPrices.FirstOrDefault(p => p.TariffId.Equals(TariffId)).Award,
                     PayableAmount = x.CashPrice - 0,
                     Department = "Ambulatory",
                     //TestCode = x.TestProfileCode,
                    
                 }).ToList();
                return new JsonResult { Data = autoData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return null;
        }
        
        public ActionResult GetRequest()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.AssignmentDetails.Where(e => e.AddedOn >= DateTime.Today).Select(
                x => new
                {
                    x.Id,
                    x.OPDNo,
                    x.AmbulanceType,
                     x.VehicleNumber,
                     x.RequestedRoute,
                    x.AssignedDriver,
                    Paid = x.OpdRegister.BillServices1.Any(e => e.Service.Department.DepartmentName.ToLower().Equals("ambulatory")),
                  name =   x.OpdRegister.Patient.FName + " " + x.OpdRegister.Patient.LName

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetRequests()
        {
            db.Configuration.LazyLoadingEnabled = false;

            {
                List<AssignmentDetail> requestlist = db.AssignmentDetails.ToList<AssignmentDetail>();
                return Json(new { data = requestlist }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Approve(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            AssignmentDetail obj = db.AssignmentDetails.Find(id);
            if (obj != null)
            {

                obj.RequestStatus = false;
                UpdateModel(obj);
                // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Confirmed");
        }

        public ActionResult SaveCleanupRequest(CleanupRequest data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Approval = false;
            data.Status = true;

            db.CleanupRequests.Add(data);
            db.SaveChanges();

            return RedirectToAction("CleanupRequest", new { });
        }
        public ActionResult GetCleanupRequest()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.CleanupRequests.Select(
                x => new
                {
                    x.Id,
                    x.RequestBuilding,
                    x.RequestFloor,
                    x.RequestArea,
                    x.BedNo,
                    x.Remarks,
                    x.Status,
                    x.Approval
                    
                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult ApproveCleanup(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            CleanupRequest obj = db.CleanupRequests.Find(id);
            if (obj != null)
            {

                obj.Approval = true;
                UpdateModel(obj);
                // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }

        public ActionResult ApproveConfirmation(int? id)
        {
            // 
            CleanupRequest obj = db.CleanupRequests.Find(id);
            
            if (obj != null && obj.Approval == true)
            {

                obj.Status = false;
                UpdateModel(obj);
               
                db.SaveChanges();

            }

            else
            {
                return Content("Task Not Approved");
            }
            return Content("Task Completed");
        }

        public ActionResult SaveLinenRequest(LinenRequest data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Approval = false;
            data.Status = false;

            db.LinenRequests.Add(data);
            db.SaveChanges();

            return RedirectToAction("RequestForLinen", new { });
        }

        public ActionResult GetLinenRequest()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.LinenRequests.Select(
                x => new
                {
                    x.Id,
                    x.LinenName,
                    x.RequestQuantity,
                    x.LType,
                    
                    x.Status,
                    x.Approval

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult ApproveLinenRequest(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            LinenRequest obj = db.LinenRequests.Find(id);
            if (obj != null)
            {

                obj.Status = true;
                UpdateModel(obj);
                // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }
        [HttpPost]
        public ActionResult ConfirmDelivey(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            LinenRequest obj = db.LinenRequests.Find(id);
            if (obj != null)
            {

                obj.Approval = true;
                UpdateModel(obj);
                // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }

        public ActionResult SaveCSSDRequest(CSSDRequest data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Approval = false;
            data.Status = false;

            db.CSSDRequests.Add(data);
            db.SaveChanges();

            return RedirectToAction("RequestToCSSD", new { });
        }

        public ActionResult GetCSSDRequest()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.CSSDRequests.Select(
                x => new
                {
                    x.Id,
                    x.RequiredDate,
                    x.RequiredTime,
                    x.RequestType,
                    x.RequestItem,
                    x.Remarks,
                    x.Quantity,
                    x.Status,
                    x.Approval

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetTheatreRequest()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.NurseTheatreRequests.Select(
                x => new
                {
                    x.Id,
                    x.AddedOn,
                    x.Assigned,
                    x.OPDNo,
                    x.RequestingNurse,
                    x.ServiceName,
                    x.Price,
                    x.Department,
                   
                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult ConfirmCSSDDelivey(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            CSSDRequest obj = db.CSSDRequests.Find(id);
            if (obj != null)
            {

                obj.Status = true;
                UpdateModel(obj);
                // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }
        [HttpPost]
        public ActionResult ApproveCSSDDelivey(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            CSSDRequest obj = db.CSSDRequests.Find(id);
            if (obj != null)
            {

                obj.Approval = true;
                UpdateModel(obj);
                // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }

        public ActionResult SaveCSSDIssueToPatient(CSSDIssueToPatient data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            
            data.Status = false;

            db.CSSDIssueToPatients.Add(data);
            db.SaveChanges();

            return RedirectToAction("IssueItemToPatient", new { });
        }

        public ActionResult GetCSSDIssue()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.CSSDIssueToPatients.Select(
                x => new
                {
                    x.Id,
                    x.OPDNo,
                    x.RequestDate,
                    x.RequestItem,
                    x.IssueQuantity,
                    x.Status,
                    name = x.OpdRegister.Patient.FName + " " + x.OpdRegister.Patient.LName


                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [HttpPost]
        public ActionResult ConfirmCSSDReturn(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            CSSDIssueToPatient obj = db.CSSDIssueToPatients.Find(id);
            if (obj != null)
            {

                obj.Status = true;
                UpdateModel(obj);
               
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }

        [HttpPost]
        public ActionResult ConfirmTheatre(int? id, string QueueType = "OPD")
        {
            
            NurseTheatreRequest obj = db.NurseTheatreRequests.Find(id);
            if (obj != null)
            {

                obj.Assigned = true;
                UpdateModel(obj);

                if (db.SaveChanges() > 0)
                {
                    var dat = new BillPayment();
                  

                    var nursereq = db.Services.FirstOrDefault(e => e.DepartmentId == 40);
                    var billService = new BillService();

                    billService.OPDNo = obj.OPDNo;
                    billService.DepartmentId = nursereq.DepartmentId;
                    billService.SeviceId = nursereq.Id; billService.ServiceName = nursereq.ServiceName;
                    billService.Price = nursereq.CashPrice; billService.Quatity = 1;
                    billService.Award = 0; billService.DoctorFee = 0; billService.Paid = false;
                    billService.Offered = false; billService.DateAdded = DateTime.Now;
                    billService.UserId = int.Parse(Session["UserId"].ToString());
                    billService.BranchId = 1; billService.IsNurse = false;
                    db.BillServices.Add(billService);


                    db.SaveChanges();
                }


                ViewBag.QueueType = QueueType;

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }

        public ActionResult SaveMaintenceRequest(MaintenanceRequest data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Assigned = false;
            data.Status = false;

            db.MaintenanceRequests.Add(data);
            db.SaveChanges();

            return RedirectToAction("RequestToCSSD", new { });
        }

        public ActionResult GetMaintenanceAssn()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.MaintenanceAssignments.Select(
                x => new
                {
                    x.Id,
                    x.AllocatedTime,
                    x.AssignedWorker,
                    x.AllocatedDate,

                    x.Status,
                    x.RequsetedTask

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

    }
}