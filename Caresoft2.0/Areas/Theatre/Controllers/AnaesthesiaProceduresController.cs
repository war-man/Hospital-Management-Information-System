using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Theatre.Controllers
{
    public class AnaesthesiaProceduresController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Theatre/AnaesthesiaProcedures
        public ActionResult Index(int id)
        {
            var pData = db.TheatrePatientBioDatas.Find(id);
            ViewBag.TheatreDesignations = db.TheatreDesignations.ToList();

            return View(pData);
        }
    }
}