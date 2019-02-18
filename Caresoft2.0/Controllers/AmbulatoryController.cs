using Caresoft2._0.CustomData;
using Caresoft2._0.UniversalHelpers;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class AmbulatoryController : Controller
    {
        // GET: Ambulatory
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private HouseKeeping hs = new HouseKeeping();

        public ActionResult AmbulatorIndex()

        {
            List<AmbulanceMaster> vehiclelist = db.AmbulanceMasters.ToList<AmbulanceMaster>();


            return View(new { data = vehiclelist });
        }
        public ActionResult AmbulanceMaster()
        {
            return PartialView();
        }

        public ActionResult _SideNav()
        {
            return PartialView();
        }


        public ActionResult AssignDriver()
        {
            return View();
        }

        public ActionResult SaveAmbulanceMaster(AmbulanceMaster data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Engaged = false;
            

            db.AmbulanceMasters.Add(data);
            db.SaveChanges();

            return RedirectToAction("AmbulanceMaster", new { id = data.Id });
        }

        public ActionResult VehicleDetails()
        {

            return PartialView(db.AmbulanceMasters.OrderByDescending(e => e.Id).ToList());
        }


        //Fetch Vehicle from database

        public ActionResult GetData()
        {
            
            {
                List<AmbulanceMaster> vehiclelist = db.AmbulanceMasters.ToList<AmbulanceMaster>();
                return Json(new { data = vehiclelist }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetRequest()
        {

            {
                List<AmbulanceRequest> requestlist = db.AmbulanceRequests.Where(e => e.AddedOn >= DateTime.Today).ToList<AmbulanceRequest>();
                return Json(new { data = requestlist }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Releive(int? id)
        {
           // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            AmbulanceMaster obj = db.AmbulanceMasters.Find(id);
            if (obj != null)
            {

                obj.Engaged = false;
                UpdateModel(obj);
               // db.AmbulanceMasters.Add(obj);
                db.SaveChanges();

            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Ambulance assigned");
        }
        /*
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
                         (e => e.ServiceName.Contains(search) && e.IsLAB /*&& e.CashPrice>0).Select(x => new AutoCompleteData
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
                        (e => e.ServiceName.Contains(search) && e.IsXRAY /*&& e.CashPrice>0).Select(x => new AutoCompleteData
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
            */


        public ActionResult AmbulanceRequest(int? id )
        {

            
            //ViewBag.RelationshipsOptions = db.Relationships.ToList();

            
            
            
            return PartialView();
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
            List<DoctorsAutoCompleteData> doctors = db.Employees.Where(e => e.FName.Contains(search) ||
            e.OtherName.Contains(search) && e.Designation.DesignationName.ToLower() == "doctor").Select(
                x => new DoctorsAutoCompleteData
                {
                    DoctorId = x.Id,
                    Name = x.Salutation + " " + x.FName + " " + x.OtherName
                }).ToList();
            return new JsonResult { Data = doctors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Searchambulance(string search)
        {
            //search = search.ToLower().Trim();
            List<AmbulanceAutoCompleteData> amb = db.AmbulanceMasters.Where(e => e.VehicleName.Contains(search) && e.Engaged == false ).Select(
                x => new AmbulanceAutoCompleteData
                {
                    Id = x.Id,
                    VehicleName = x.VehicleName, 
                    VehicleNumber = x.VehicleNumber,
                    AssignedDriver = x.AssignedDriver
                }).ToList();
            return new JsonResult { Data = amb, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class AmbulanceAutoCompleteData
        {
            public int Id { get; set; }

            public string VehicleName { get; set; }
            public string VehicleNumber { get; set; }
            public string AssignedDriver { get; set; }
        }

        public class RequestAutoCompleteData
        {
            public int Id { get; set; }
            public int OPDNo { get; set; }

            public string ServiceName { get; set; }
        }

        public ActionResult SearchRequest(string search)
        {
            //search = search.ToLower().Trim();
            List<RequestAutoCompleteData> doctors = db.AmbulanceRequests.Where(e => e.ServiceName.Contains(search)
             && e.Assigned == false).Select(
                x => new RequestAutoCompleteData
                {
                    Id = x.Id,
                    ServiceName = x.ServiceName,
                    OPDNo = x.OPDNo
                }).ToList();
            return new JsonResult { Data = doctors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class DriversAutoCompleteData
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public ActionResult SearchDriver(string search)
        {
            //search = search.ToLower().Trim();
            List<DriversAutoCompleteData> drivers = db.Employees.Where(e => e.FName.Contains(search) ||
            e.OtherName.Contains(search) && e.DesignationId == 1).Select(
                x => new DriversAutoCompleteData
                {
                    Id = x.Id,
                    Name = x.Salutation + " " + x.FName + " " + x.OtherName
                }).ToList();
            return new JsonResult { Data = drivers, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
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
        public ActionResult SaveAssignmentDetails(AssignmentDetail data, string QueueType = "OPD")
        {

            //var OpdIpd = data.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault();
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.RequestStatus = true;

            db.AssignmentDetails.Add(data);
            if (db.SaveChanges() > 0)
            {
                var dat = new BillPayment();
                var pa = db.AmbulanceMasters.FirstOrDefault(e => e.VehicleNumber == data.VehicleNumber);
                
                var nursereq = db.AmbulanceRequests.FirstOrDefault(e => e.OPDNo == data.OPDNo);

                pa.Engaged = true;
                nursereq.Assigned = true;

                var ambulance = db.Services.FirstOrDefault(e => e.Department.DepartmentName.Trim().ToLower().Contains("ambula"));
                var billService = new BillService();

                billService.OPDNo = data.OPDNo;
                billService.DepartmentId = ambulance.DepartmentId;
                billService.SeviceId = ambulance.Id; billService.ServiceName = ambulance.ServiceName;
                billService.Price = ambulance.CashPrice; billService.Quatity = 1;
                billService.Award = 0; billService.DoctorFee = 0; billService.Paid = false;
                billService.Offered = false; billService.DateAdded = DateTime.Now;
                billService.UserId = int.Parse(Session["UserId"].ToString());
                billService.BranchId = 1; billService.IsNurse = false;
                db.BillServices.Add(billService);


                db.SaveChanges();
            }


            ViewBag.QueueType = QueueType;


            /*   if (hs.IsUnderFive(data.PatientId) && hs.ExemptUnderFive())
               {
                   hs.AutoWaiver(billService.Id, "under 5 automatic waiver");
               }*/


            return RedirectToAction("AmbulatorIndex", new { id = data.Id });
        }

        public ActionResult AddBillService(BillService billService)
        {
            billService.DateAdded = DateTime.Now;
            db.BillServices.Add(billService);
            return Json(new { Status = "Success", BillService = billService });
        }

        public ActionResult GetDrivers()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.AmbulanceMasters.Select(
                x => new
                {
                   
                  x.VehicleNumber,
                  x.VehicleName,
                  x.AssignedDriver,
                 x.Engaged
                    //DriverStatus = x.Designation.Employees.Where(e => e.DriverStatus == null && e.DesignationId == 1),
                   // name = x.Designation.Employees.FirstOrDefault(e=> e.Designation.DesignationName.ToLower().Equals("drivers")).Salutation + " " + x.Designation.Employees.FirstOrDefault(e=> e.Designation.DesignationName.ToLower().Equals("drivers")).OtherName + " " + x.Designation.Employees.FirstOrDefault(e => e.Designation.DesignationName.ToLower().Equals("drivers")).FName

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

    }

}






/*
 

        private IRepository _repository;
        private AppUserManagerProvider _appUserManagerProvider;
        public IRepository Repository
        {
            get { return _repository ?? (_repository = new Repository()); }
        }
        
        public AppUserManagerProvider AppUserManagerProvider
        {
            get { return _appUserManagerProvider ?? (_appUserManagerProvider = new AppUserManagerProvider()); }
        }
        

        // GET: System
        public ActionResult Index()
        {
            //redirect to login page unauthorized user
            return !Request.IsAuthenticated ? RedirectToAction("LogOn", "Account") : RedirectToAction(User.IsInRole("Manager") ? "Manager" : "Employee", "System");
        }
       
        public ActionResult Manager()
        {
            var scheduler = new DHXScheduler(this);
            
            #region check rights
            if (!RoleIs("Manager"))// checks the role
                return RedirectToAction("Index", "System");//in case the role is not manager, redirects to the login page
            
            #endregion

            #region configuration
            
            scheduler.Config.first_hour = 8;//sets the minimum value for the hour scale (Y-Axis)
            scheduler.Config.hour_size_px = 88;
            scheduler.Config.last_hour = 17;//sets the maximum value for the hour scale (Y-Axis)
            scheduler.Config.time_step = 30;//sets the scale interval for the time selector in the lightbox. 
            scheduler.Config.full_day = true;// blocks entry fields in the 'Time period' section of the lightbox and sets time period to a full day from 00.00 the current cell date until 00.00 next day. 
            
            scheduler.Skin = DHXScheduler.Skins.Flat;
            scheduler.Config.separate_short_events = true;
            
            scheduler.Extensions.Add(SchedulerExtensions.Extension.ActiveLinks);
            
            #endregion

            #region views configuration
            scheduler.Views.Clear();//removes all views from the scheduler
            scheduler.Views.Add(new WeekView());// adds a tab with the week view
            var units = new UnitsView("staff", "owner_id") { Label = "Staff" };// initializes the units view
            
                        var users = AppUserManagerProvider.Users;
            var staff = new List<object>();
            foreach (var user in users)
            {
                if (AppUserManagerProvider.GetUserRolesName(user.Id).Contains("Employee"))
                {
                    staff.Add(new { key = user.Id, label = user.UserName });
                }
            }
            
            units.AddOptions(staff);// sets X-Axis items to names of employees  
            scheduler.Views.Add(units);//adds a tab with the units view
            scheduler.Views.Add(new MonthView()); // adds a tab with the Month view
            scheduler.InitialView = units.Name;// makes the units view selected initially
            
            scheduler.Config.active_link_view = units.Name;
            #endregion
            
            #region lightbox configuration
            var text = new LightboxText("text", "Task") { Height = 20, Focus = true };// initializes a text input with the label 'Task'
            scheduler.Lightbox.Add(text);// adds the control to the lightbox
            var description = new LightboxText("details", "Details") { Height = 80 };// initializes a text input with the label 'Task'
            scheduler.Lightbox.Add(description);
            var status = new LightboxSelect("status_id", "Status");// initializes a dropdown list with the label 'Status'
            status.AddOptions(Repository.GetAll<Status>().Select(s => new
            {
                key = s.id,
                label = s.title
            }));// populates the list with values from the 'Statuses' table
            scheduler.Lightbox.Add(status);
            //add users list 
            var sUser = new LightboxSelect("owner_id", "Employee");
            sUser.AddOptions(staff);
            //--
            scheduler.Lightbox.Add(sUser);
            
            scheduler.Lightbox.Add(new LightboxTime("time"));// initializes and adds a control area for setting start and end times of a task
            #endregion
            
            #region data
            scheduler.EnableDataprocessor = true;// enables dataprocessor
            scheduler.LoadData = true;//'says' to send data request after scheduler initialization 
            scheduler.Data.DataProcessor.UpdateFieldsAfterSave = true;// Tracks after server responses for modified event fields 
            #endregion
            
                        Employees[] employees = users.Select(o => new Employees()
                        {
                            key = o.Id,
                            userName = o.UserName
                        }).ToArray();
            
            List<Status> statuses = Repository.GetAll<Status>().ToList();
            
                        var js = new DataContractJsonSerializer(typeof(Employees[]));
            var ms = new MemoryStream();
            js.WriteObject(ms, employees);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var json = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            var model = new SystemModel(scheduler, json, statuses);
            return View(model);
        }
        
        private bool RoleIs(string role)
        {
            return Request.IsAuthenticated && User.IsInRole(role);
        }
        
        public ActionResult Employee()
        {
            var scheduler = new DHXScheduler(this);
            
            #region check rights
            if (!RoleIs("Employee"))
            {
                return RedirectToAction("Index", "System");
            }
            #endregion
            
            #region configuration

            scheduler.Config.separate_short_events = true;
            scheduler.Config.hour_size_px = 88;
            
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Cookie);// activates the extension to provide cookie
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Tooltip);// activates the extension to provide tooltips
            var template = "<b>Task:</b> {text}<br/><b>Start date:</b>";
            template += "<%= scheduler.templates.tooltip_date_format(start) %><br/><b>End date:</b>";
            template += "<%= scheduler.templates.tooltip_date_format(end) %>";
            scheduler.Templates.tooltip_text = template; // sets template for the tooltip text
            
            scheduler.Skin = DHXScheduler.Skins.Flat;
            #endregion
            
            #region views
            scheduler.Views.Clear();//removes all views from the scheduler 
            scheduler.Views.Add(new WeekAgendaView());// adds a tab with the weekAgenda view
            scheduler.Views.Add(new MonthView()); // adds a tab with the Month view
            scheduler.InitialView = scheduler.Views[0].Name;// makes the weekAgenda view selected initially
            #endregion
            
            #region data
            scheduler.SetEditMode(EditModes.Forbid);// forbids editing of tasks  
            scheduler.LoadData = true;//'says' to send data request after scheduler initialization 
            scheduler.DataAction = "Tasks";//sets a controller action which will be called for data requests 
            scheduler.Data.Loader.PreventCache();// adds the current ticks value to url to prevent caching of the request 
            #endregion
            
            return View(scheduler);
        }
        
        
        public ActionResult Data()
        {
            //if the user is not authorized or not in the Manager Role, returns the empty dataset
            if (!RoleIs("Manager")) return new SchedulerAjaxData();
            
                        var tasks = Repository.GetAll<Task>()
                            .Join(Repository.GetAll<Status>(), task => task.status_id, status => status.id, (task, status) => new { Task = task, Status = status })
                            .Select(o => new
                            {
                                o.Status.color,
                                o.Task.id,
                                o.Task.owner_id,
                                o.Task.details,
                                o.Task.end_date,
                                o.Task.start_date,
                                o.Task.text,
                                o.Task.status_id
                            });
            
                        var resp = new SchedulerAjaxData(tasks);
            return resp;
            
        }
        public ActionResult Save(Task task)
        {
            // an action against particular task (updated/deleted/created) 
            var action = new DataAction(Request.Form);
            #region check rights
            if (!RoleIs("Manager"))
            {
                action.Type = DataActionTypes.Error;
                return new AjaxSaveResponse(action);
            }
            #endregion
            
            task.creator_id = Guid.Parse(AppUserManagerProvider.UserId);
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        Repository.Insert(task);
                        break;
                    case DataActionTypes.Delete:
                        Repository.Delete(task);
                        break;
                    case DataActionTypes.Update:
                        Repository.Update(task);
                        break;
                }
                action.TargetId = task.id;
            }
            catch (Exception)
            {
                action.Type = DataActionTypes.Error;
            }
            
                        var color = Repository.GetAll<Status>().SingleOrDefault(s => s.id == task.status_id);
            
                        var result = new AjaxSaveResponse(action);
            result.UpdateField("color", color.color);
            return result;
        }
        
        public ActionResult Tasks()
        {
            #region check rights
            if (!RoleIs("Employee"))
            {
                return new SchedulerAjaxData();//returns the empty dataset
            }
            #endregion
            
            var result = Repository.GetAll<Task>()
                .Join(Repository.GetAll<Status>(), task => task.status_id, status => status.id, (task, status) => new { Task = task, Status = status })
                .Select(o => new
                {
                    o.Status.color,
                    o.Task.id,
                    o.Task.owner_id,
                    o.Task.details,
                    o.Task.end_date,
                    o.Task.start_date,
                    o.Task.text,
                    o.Task.status_id
                });
            
                        var tasks = new List<object>();
            
            foreach (var r in result.ToList())
            {
                if (r.owner_id == Guid.Parse(AppUserManagerProvider.UserId))
                {
                    tasks.Add(new
                    {
                        r.color,
                        r.id,
                        r.owner_id,
                        r.details,
                        r.end_date,
                        r.start_date,
                        r.text,
                        r.status_id
                    });
                }
            }
            
                        var resp = new SchedulerAjaxData(tasks);
            return resp;
        }
        
        public ActionResult TaskDetails(int? id)
        {
            #region check rights
            
            if (!RoleIs("Employee"))
            {
                return RedirectToAction("Index", "System");
            }
            #endregion
            
                        var task = default(Task);
            if (id != null)
            {
                task = Repository.GetAll<Task>().FirstOrDefault(o => o.id == id);
                
                if (task.owner_id != Guid.Parse(AppUserManagerProvider.UserId))
                    task = default(Task);
            }
            
                        var statuses = Repository.GetAll<Status>().ToArray();
            
                        ViewData["status"] = task != default(Task) ? task.status_id : statuses[0].id;
            ViewData["user"] = User.Identity.Name;
            return View(new TaskDetails(task, statuses));
        }
        
        public ActionResult UpdateStatus(int? id)
        {
            if (!RoleIs("Employee") || this.Request.Form["result"] != "Update" || id == null)
                return RedirectToAction("Index", "System");
            
            
            var task = Repository.GetAll<Task>().SingleOrDefault(ev => ev.id == id);
            
            if (task == default(Task) && task.owner_id != Guid.Parse(AppUserManagerProvider.UserId))
                return RedirectToAction("Index", "System");
            
            task.status_id = int.Parse(this.Request.Form["status_id"]);
            UpdateModel(task);
            Repository.Update(task);
            
            return RedirectToAction("Index", "System");
        }
        
        public class SystemModel
        {
            public DHXScheduler Scheduler { get; set; }
            public string Users { get; set; }
            public List<Status> Statuses { get; set; }
            public SystemModel(DHXScheduler sched, string users, List<Status> statuses)
            {
                Scheduler = sched;
                Users = users;
                Statuses = statuses;
            }
        }
        

     
     */
