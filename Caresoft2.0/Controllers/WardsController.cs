using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using Caresoft2._0.CustomData;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class WardsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: Wards
        public ActionResult Index()
        {

            return PartialView("WardsList", db.HSWards.OrderByDescending(e=>e.Id).ToList());
        }

        public ActionResult WardsMaster()
        {
            WardsMasterData data = new WardsMasterData();
            data.Wards = db.HSWards.OrderByDescending(e => e.Id).ToList();
            data.Buildings = db.HSBuildings.ToList();
            data.Categories = db.HSWardCategories.ToList();
            return View(data);
        }

        public ActionResult Categories()
        {
            return View(db.HSWardCategories.ToList().OrderByDescending(e=>e.Id));
        }

        

        [HttpPost]
        public ActionResult Category(HSWardCategory category)
        {
            
            category.BranchId = (int)Session["UserBranchId"] ;
            category.UserId = (int)Session["UserId"];
            category.DateAdded = DateTime.Now;

            db.HSWardCategories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Categories");
        }

        public ActionResult Buildings()
        {
            return View(db.HSBuildings.ToList().OrderByDescending(e => e.Id));
        }

        [HttpPost]
        public ActionResult Building(HSBuilding building)
        {
            
            building.BranchId = (int)Session["UserBranchId"] ;
            building.UserId = (int)Session["UserId"];
            building.TimeAdded = DateTime.Now;

            db.HSBuildings.Add(building);
            db.SaveChanges();

            HSFloor floor = new HSFloor();
            floor.BuildingId = building.Id;
            floor.FloorName = "Ground Floor";
            floor.UserId = (int)Session["UserId"]; 
            floor.DateAdded = DateTime.Now;

            db.HSFloors.Add(floor);
            db.SaveChanges();

            for (var i = 1; i <=building.NumberOfFloors; i++)
            {
                floor.FloorName = OrdinalSuffixof(i) + " Floor";
                db.HSFloors.Add(floor);
                db.SaveChanges();
            }

            return RedirectToAction("Buildings");
        }


        public ActionResult Floors(int? id)
        {
            FloorMasterData data = new FloorMasterData();
            data.Floors = db.HSFloors.OrderByDescending(e => e.Id).ToList();
            data.Buildings = db.HSBuildings.ToList();

            if(data.Buildings.Count() < 1)
            {
                return RedirectToAction("Buildings");
            }
            if (id > 0)
            {
                ViewBag.SelectedBuilding = data.Buildings.FirstOrDefault(e=>e.Id == id);

            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Floor(HSFloor floor)
        {

            floor.UserId = (int)Session["UserId"];
            floor.DateAdded = DateTime.Now;

            db.HSFloors.Add(floor);
            db.SaveChanges();
            return RedirectToAction("Floors", new { id = floor.BuildingId});
        }



        public string OrdinalSuffixof(int i)
        {
            var j = i % 10;
            var   k = i % 100;

            if (j == 1 && k != 11)
            {
                return i + "st";
            }
            if (j == 2 && k != 12)
            {
                return i + "nd";
            }
            if (j == 3 && k != 13)
            {
                return i + "rd";
            }
            return i + "th";
        }

        public ActionResult BedsMaster()
        {
            var data = new WardsMasterData();
            data.Buildings = db.HSBuildings.Where(e=>e.HSFloors.Count() > 0).ToList();
            //if(data.Buildings ==null || data.Buildings.Count() == 0)
            //{
            //    return RedirectToAction("") //create buildings
            //}
            return View(data);
        }

        public ActionResult DischargeSummaryHeading()
        {
            return View();
        }

        public ActionResult IpdDischargeSummaryFormat()
        {
            return View();
        }


        public ActionResult GetFloors(int? id)
        {
            var floors = db.HSBuildings.Find(id).HSFloors.ToList();
            var results = "<option>Select</option>";
            foreach(var f in floors)
            {
                results += "<option value=" + f.Id + ">" + f.FloorName + "</option>";
            }

            return Content(results);
        }

        [HttpPost]
        public ActionResult CreateWard(HSWard ward, int? NumberOfBeds)
        {
            ward.DateAdded = DateTime.Now;
            ward.UserId = (int)Session["UserId"];
            db.HSWards.Add(ward);
            db.SaveChanges();
            if (NumberOfBeds > 0)
            {
                HSBed bed = new HSBed
                {
                    UserId = (int) Session["UserId"],
                    DateAdded = DateTime.Now
                };
                for (var b = 1; b<=NumberOfBeds; b++)
                {
                    bed.WardId = ward.Id;
                    bed.BedName = "Bed " + b;

                    db.HSBeds.Add(bed);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("index");
        }

        public ActionResult GetFWards(int id)
        {
            var wards = db.HSFloors.Find(id).HSWards.ToList();
            var results = "<option>Select</option>";
            foreach(var w in wards)
            {
                results += "<option value=" + w.Id + ">" + w.WardName+ "</option>";
            }

            return Content(results);
        }

        public ActionResult AddBed(HSBed bed)
        {
            bed.DateAdded = DateTime.Now;
            bed.UserId = (int)Session["UserId"];

            db.HSBeds.Add(bed);
            db.SaveChanges();

            return RedirectToAction("BedList", new {id = bed.WardId});
        }

        public ActionResult BedList(int? id)
        {
            return PartialView("BedsList", db.HSWards.Find(id).HSBeds.OrderByDescending(e => e.Id).ToList());
        }
    }
}