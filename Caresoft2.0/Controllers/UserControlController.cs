using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Caresoft2._0.Controllers
{
   [Auth]
    public class UserControlController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: UserControl
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Department()
        {
            DepartmentsData departmentsData = new DepartmentsData();
            departmentsData.DepartmentTypeList = db.DepartmentTypes.ToList();
            departmentsData.Departments = db.Departments.OrderBy(p=>p.Id).ToList();

            if(departmentsData.Departments.Count > 0 || departmentsData.DepartmentTypeList.Count > 0 )
            {
                return View(departmentsData);
            }

            return Content("Error");
        }

        public ActionResult CreateDepartment(Department department)
        {

            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Error" + Response.StatusCode);
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    db.Departments.Add(department);
                    db.SaveChanges();
                    return Content("Record Added Successfully");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Error" + ex.Message + Response.StatusCode);
                
            }
           
            return View();
        }

        public ActionResult Designation()
        {
            return View();
        }

        public ActionResult NewEmployee()
        {
            return View();
        }
    }
}