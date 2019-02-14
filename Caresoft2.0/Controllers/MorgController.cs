using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class MorgController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        private HouseKeeping hs = new HouseKeeping();
        // GET: Morg
        public ActionResult Index()
        {
            return RedirectToAction("Admission");
        }

        public ActionResult Admission()
        {
            ViewBag.Relationships = db.Relationships.ToList();
            return View();
        }

        public ActionResult DeathRegister(int p=1)
        {
            var ma = db.MorgAdmissions.OrderByDescending(e=>e.Id).Where(e=>(e.Released ==null || !e.Released.Value)).ToList();
            int itemsperpage = 10;
            var offset = (itemsperpage * p) - itemsperpage;
            ViewBag.Page = p;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(ma.Count() / itemsperpage))) +1;
            return View(ma.Skip(offset).Take(itemsperpage));
        }

        public ActionResult DeathRegisterList(int p=1, string s = "")
        {
            var ma = db.MorgAdmissions.OrderByDescending(e => e.Id).Where(e => (e.Released == null || !e.Released.Value)).ToList();
            if (s.Trim() != "")
            {
                s = s.ToLower();
                ma = ma.Where(e => e.DeceasedName.ToLower().Contains(s) ||(e.RegNumber !=null && e.RegNumber.Contains(s))
                || e.Id.ToString() == s).ToList();
            }
            int itemsperpage = 10;
            var offset = (itemsperpage * p) - itemsperpage;
            ViewBag.Page = p;
            ViewBag.Pages = Convert.ToInt32(Math.Ceiling((double)(ma.Count() / itemsperpage))) +1;
            return PartialView(ma.Skip(offset).Take(itemsperpage));
        }

        public ActionResult DeathNotification(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("DeathRegister");
            }
            return View(db.MorgAdmissions.Find(id));
        }

       
        public ActionResult PermitCollector()
        {
            return View();
        }

        public ActionResult BodyCollection(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("DeathRegister");
            }

            ViewBag.Status = TempData["status"];
            ViewBag.Message = TempData["message"];


            var body = db.MorgAdmissions.Find(id);
            if (body.PatientId.HasValue)
            {
                ViewBag.OutStandingBill = hs.OutStandingBill(body.PatientId.Value);
            }
            return View(body);
        }

        public ActionResult Setup()
        {
            return View(db.MorgColdrooms.ToList());
        }

        public JsonResult PatientList(String search)
        {
            List<AutoCompleteData> patients = db.Patients.Where
                (e =>(e.FName.Contains(search) || e.LName.Contains(search) || e.RegNumber.Contains(search))).Take(15)
                .Select(x => new AutoCompleteData
                {
                    Id = x.Id,
                    RegNumber = x.RegNumber,
                    Name = x.RegNumber.Trim() + " " + x.Salutation.Trim() + " " + x.FName.Trim() 
                    + " " + x.MName.Trim() + " " 
                    + x.LName.Trim() + " ("+ (DateTime.Today.Year - x.DOB.Value.Year) + " Yrs)"
                }).Take(20).ToList();
            return new JsonResult { Data = patients, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class InternalBodyDetails
        {
            public String Address { get; set; }
            public int PatientId { get; set; }
            public String RegNumber { get; set; }
            public String Age { get; set; }
            public String CauseOfDeath { get; set; }
            public String CertifiedBy { get; set; }
            public String DateAdmitted { get; set; }
            public String DeceasedName { get; set; }
            public String Gender { get; set; }
            public String IdNo { get; set; }
            public String NameOfNextOfKin { get; set; }
            public String Relation { get; set; }
            public String Residence { get; set; }
            public String Telephone { get; set; }
            public String WardUnit { get; set; }

        }
       
        public ActionResult GetInternalPatient(int id)
        {
            var pat = db.Patients.Find(id);
            var ipd = pat.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault
                (e => e.IsIPD && !e.Discharged);
            var details = new InternalBodyDetails();
            if (ipd != null)
            {
                details = new InternalBodyDetails()
                {
                    PatientId = pat.Id,
                    RegNumber = pat.RegNumber,
                    Address = pat.HomeAddress,
                    Age = hs.GetAge(pat.DOB.Value),
                    DateAdmitted = ipd.AdmissionDate.Value.ToString("yyyy-MM-dd"),
                    DeceasedName = pat.Salutation + " " + pat.FName + " " + pat.MName
                + " " + pat.LName,
                    Gender = pat.Gender,
                    WardUnit = ipd.HSBed.BedName + " " + ipd.HSBed.HSWard.WardName,
                    Residence = pat.HomeAddress
                };
            }
            else
            {
                details = new InternalBodyDetails()
                {
                    PatientId = pat.Id,
                    RegNumber = pat.RegNumber,
                    Address = pat.HomeAddress,
                    Age = hs.GetAge(pat.DOB.Value),
                    DeceasedName = pat.Salutation + " " + pat.FName + " " + pat.MName
                + " " + pat.LName,
                    Gender = pat.Gender,
                    Residence = pat.HomeAddress
                };
            }

            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveMorgAdmission(MorgAdmission morgAdmit)
        {

            
            morgAdmit.BranchId = (int)Session["UserBranchId"] ;
            morgAdmit.UserId = (int)Session["UserId"];
            morgAdmit.TimeAdded = DateTime.Now;
            db.MorgAdmissions.Add(morgAdmit);
            db.SaveChanges();
            //if deceased was IPD, empty the bed
            if (morgAdmit.PatientId.HasValue)
            {
                var pat = db.Patients.Find(morgAdmit.PatientId);
                var lastVisit = pat.OpdRegisters.OrderByDescending(e => e.Id).FirstOrDefault();
                if (lastVisit != null)
                {
                    if (lastVisit.IsIPD)
                    {
                        lastVisit.Discharged = true;
                        lastVisit.DischargedAlive = false;
                        lastVisit.DischargeDate = morgAdmit.DateOfDeath;
                        db.SaveChanges();
                    }
                }
            }

            //create an opd entry for the 
            return Json(new { SerialNo = morgAdmit.Id.ToString().PadLeft(4, '0') });
        }

        [HttpPost]
        public ActionResult CreateColdRoom(MorgColdroom room, int AutoCreateCabs = 0)
        {
            db.MorgColdrooms.Add(room);
            db.SaveChanges();

            if (AutoCreateCabs == 1)
            {
                for(var x = 1; x<=room.AuthorizedCabinets; x++)
                {
                    var cab = new MorgCabinet();
                    cab.ColdRoomId = room.Id;
                    cab.CabinetName = "CAB" + x.ToString().PadLeft(3, '0');
                    db.MorgCabinets.Add(cab);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Setup");
        }

        [HttpPost]
        public ActionResult SaveBodyCollection(MorgBodyCollection collection)
        {
            var body = db.MorgAdmissions.Find(collection.AdmissionId);
            if (body.PatientId.HasValue)
            {
                if (hs.OutStandingBill(body.PatientId.Value)>0)
                {
                    TempData["status"] = "warning";
                    TempData["message"] = "Warning! Unable to release body. The bill is unsettled.";
                    return RedirectToAction("BodyCollection", new { id = collection.AdmissionId });
                }
            }

          
            body.Released = true;
            body.PersonCollectingBody = collection.BodyCollectorName;
            body.PersonCollectingBodyRelationship = collection.BodyCollectorRelationShip;
            body.PersonCollectingBodyIdNo = collection.BodyCollectorIdNo;
            body.PersonCollectingBodyTel = collection.BodyCollectorTelNo;
            body.ReleasedDate = collection.DateCollected;

            db.SaveChanges();

            TempData["status"] = "success";
            TempData["message"] = "Success! Body released successfully.";
            return RedirectToAction("BodyCollection", new { id = collection.AdmissionId });
        }

        public ActionResult SetMorgCabinet(int id)
        {
            ViewBag.ColdRooms = db.MorgColdrooms.ToList();
            return PartialView(db.MorgAdmissions.Find(id));
        }

        public ActionResult SetCabinetDetails(CabData data)
        {
            var body = db.MorgAdmissions.Find(data.Id);
            body.MorgCabinetId = data.CabinetId;
            db.SaveChanges();

            return Json(new
            {
                Message = "Storage Details Saved Successfully!",
                Status = "success",
                StorageDetails = body.MorgCabinet.CabinetName
            });
        }

        public ActionResult MorgBilling(int id)
        {
            var body = db.MorgAdmissions.Find(id);

            return Content(body.DeceasedName);
        }
    }

    public class CabData
    {
        public int Id { get; set; }
        public int CabinetId { get; set; }
    }

    public class MorgBodyCollection
    {
        public int AdmissionId { get; set; }
        public String BodyCollectorName { get; set; }
        public DateTime DateCollected { get; set; }
        public String BodyCollectorIdNo { get; set; }
        public String BodyCollectorRelationShip { get; set; }
        public String BodyCollectorTelNo { get; set; }
    }

}