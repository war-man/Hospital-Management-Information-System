using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class ViewInvestigationController : Controller
    {
        public CaresoftHMISEntities db = new CaresoftHMISEntities();
        public string PatientName { get; private set; }
        // GET: HIV/ViewInvestigation
        public ActionResult Index()
        {
            var ViewInvestigation = db.Patients.ToList();
            return View(ViewInvestigation.OrderByDescending(e => e.Id).Take(20).ToList());
           
        }
        [HttpPost]
        //public ActionResult ViewInvestigation(ViewInvestigation viewInvestigation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        viewInvestigation.Id = 1;
        //        viewInvestigation.OPDIPD = 2;
        //        viewInvestigation.CreatedDate = DateTime.Now;
        //        db.ViewInvestigations.Add(viewInvestigation);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");

        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}