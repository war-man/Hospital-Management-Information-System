using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class MaintenanceController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: Maintenance
        public ActionResult MaintenanceIndex()
        {

            ViewBag.Worktrade = db.MaintenceWorkTrades.ToList();
            ViewBag.worktypes = db.MaintenaceWorkTypes.ToList();

            return View();
        }

        public ActionResult SaveAMCMaster(AMCMaster data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;

            

            db.AMCMasters.Add(data);
            db.SaveChanges();

            return RedirectToAction("MaintenanceIndex", new { });
        }

        public ActionResult GetMaintenanceReq()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.MaintenanceRequests.Select(
                x => new
                {
                    x.Id,
                    x.ComplainName,
                    x.Problem,
                    x.Priority,
                    x.RequestArea,
                    x.RequestBuilding,
                    x.RequestFloor,
                    x.Status,
                    x.Assigned

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult SaveMaintenanceAssn(MaintenanceAssignment data)
        {
            
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;
            data.Status = false;


            db.MaintenanceAssignments.Add(data);
            if (db.SaveChanges() > 0)
            {

                var pa = db.MaintenanceRequests.FirstOrDefault(e=> e.ComplainName.Equals(data.RequsetedTask) );

                if(pa == null)
                {
                    return RedirectToAction("MaintenanceIndex", new { });
                }

                pa.Assigned = true;


                
                db.SaveChanges();
            }


            return RedirectToAction("MaintenanceIndex", new { });
        }

        public class requestmaintence
        {
            public int Id { get; set; }

            public string ComplainName { get; set; }
            public string Problem { get; set; }
            public string RequestBuilding { get; set; }
            public string RequestFloor { get; set; }
            public string RequestArea { get; set; }
        }


        public ActionResult SearchRequest(string search)
        {
            //search = search.ToLower().Trim();
            List<requestmaintence> amb = db.MaintenanceRequests.Where(e => e.ComplainName.Contains(search) || e.Problem.Contains(search) || e.RequestBuilding.Contains(search) && e.Assigned == false).Select(
                x => new requestmaintence
                {
                    Id = x.Id,
                    ComplainName = x.ComplainName,
                    Problem = x.Problem,
                    RequestBuilding = x.RequestBuilding,
                    RequestFloor = x.RequestFloor,
                    RequestArea = x.RequestArea
                    
                }).ToList();
            return new JsonResult { Data = amb, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class DoctorsAutoCompleteData
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public ActionResult SearchWorker(string search)
        {
            //search = search.ToLower().Trim();
            List<DoctorsAutoCompleteData> doctors = db.Employees.Where(e => e.FName.Contains(search) ||
            e.OtherName.Contains(search) && e.Designation.DesignationName.ToLower() == "maintenance").Select(
                x => new DoctorsAutoCompleteData
                {
                   Id = x.Id,
                    Name = x.Salutation + " " + x.FName + " " + x.OtherName
                }).ToList();
            return new JsonResult { Data = doctors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult ConfirmMaintenace(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            MaintenanceAssignment obj = db.MaintenanceAssignments.Find(id);
            if (obj != null)
            {

                obj.Status = true;
                UpdateModel(obj);

                if (db.SaveChanges() > 0)
                {

                    var pa = db.MaintenanceRequests.FirstOrDefault(e => e.ComplainName.Equals(obj.RequsetedTask));

                    if (pa == null)
                    {
                        return RedirectToAction("MaintenanceIndex", new { });
                    }

                    pa.Status = true;



                    db.SaveChanges();
                }


            }

            else
            {
                return Content("Not such an object");
            }
            return Content("Request Approved");
        }

        public ActionResult SavePMMAster(MaintenancePMMaster data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;



            db.MaintenancePMMasters.Add(data);
            db.SaveChanges();

            return RedirectToAction("MaintenanceIndex", new { });
        }
        public ActionResult SaveWorkTrade(MaintenceWorkTrade data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;



            db.MaintenceWorkTrades.Add(data);
            db.SaveChanges();

            return RedirectToAction("MaintenanceIndex", new { });
        }

        public ActionResult SaveWorkType(MaintenaceWorkType data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;



            db.MaintenaceWorkTypes.Add(data);
            db.SaveChanges();

            return RedirectToAction("MaintenanceIndex", new { });
        }

        public ActionResult SaveSchedule(MaintenanceScheduling data)
        {
            data.UserId = (int)Session["UserId"];
            data.BranchId = 1;
            data.AddedOn = DateTime.Now;



            db.MaintenanceSchedulings.Add(data);
            db.SaveChanges();

            return RedirectToAction("MaintenanceIndex", new { });
        }

        public ActionResult GetScheduleActivity()
        {

            {
                db.Configuration.LazyLoadingEnabled = false;

                var requestlist = db.MaintenanceSchedulings.Select(
                x => new
                {
                    x.Id,
                    x.FromDate,
                    x.FromTime,
                    x.PMName,
                    x.PMNo,
                    x.WorkTrade,
                    x.WorkType,
                    x.Status,
                    x.Schedule,
                    x.DueOn

                }).ToList();
                return new JsonResult { Data = requestlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult SearchPM(string search)
        {
            //search = search.ToLower().Trim();
            List<PMMaSter> amb = db.MaintenancePMMasters.Where(e => e.PMNo.Contains(search) || e.PMTaskName.Contains(search)).Select(
                x => new PMMaSter
                {
                    Id = x.Id,
                    PMNo = x.PMNo,
                    PMTaskName = x.PMTaskName,
                   

                }).ToList();
            return new JsonResult { Data = amb, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class PMMaSter
        {
            public int Id { get; set; }

            public string PMNo { get; set; }
            public string PMTaskName { get; set; }
        }

        [HttpPost]
        public ActionResult ConfirmPM(int? id)
        {
            // id = db.AmbulanceMasters.Where(e => e.Engaged == false);
            MaintenanceScheduling obj = db.MaintenanceSchedulings.Find(id);
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

        public ActionResult SaveSafetyInstruction(SafetyInstructionMaster data)
        {

            data.AddedOn = DateTime.Now;
            data.BranchId = 1;
            data.UserId = (int)Session["UserId"];

            db.SafetyInstructionMasters.Add(data);
            db.SaveChanges();

            return View("MaintenanceIndex", new { });
        }
    }
}