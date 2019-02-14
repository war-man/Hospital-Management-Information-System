using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Theatre.Controllers
{
    public class ReportsController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        public ActionResult PatientHistory(int? id)
        {
            var history = db.OpdRegisters.Find(id).Patient;
            return PartialView(history);
        }
    }
}