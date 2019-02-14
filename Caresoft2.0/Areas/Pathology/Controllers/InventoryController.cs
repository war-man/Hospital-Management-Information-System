using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Pathology.Controllers
{
    [Auth]

    public class InventoryController : Controller
    {
        // GET: Pathology/Inventory
        public ActionResult Index()
        {
            return View();
        }
    }
}