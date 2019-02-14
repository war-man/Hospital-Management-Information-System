using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaresoftHMISDataAccess;


namespace Caresoft2._0.Areas.CCC.Controllers
{
    public class VoluntaryTestingMasterController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: HIV/VoluntaryTestingMaster
        //public ActionResult Index()
        //{
        //    return View(db.VoluntaryTestingMasters.ToList());
        //}
        //[httppost]
        //public actionresult index(caresofthmisdataaccess.voluntarytestingmaster voluntarytestingmasters)
        //{
        //    if (modelstate.isvalid)
        //    {

        //        voluntarytestingmasters.opdipd = 2;
        //        voluntarytestingmasters.createddate = datetime.now;
        //        //db.voluntarytestingmasters.add(voluntarytestingmasters);
        //        db.savechanges();
                
        //        return view();
        //    }
        //    return redirecttoaction("index");

        //}

    }

    public class VoluntaryTestingMaster
    {
        public int OPDIPD { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
    }
}
