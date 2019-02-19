using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.Reflection;
using Caresoft2._0.CrystalReports.ReportHeader;
using Caresoft2._0.Areas.CareSoftReports.Reports.UserRolesAndRights;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using PagedList;
using Caresoft2._0.Areas.Procurement.Models;
using Caresoft2._0.Areas.Active.Reports.Datasets;

namespace Caresoft2._0.Controllers
{

   [Auth]
    public class UserManagementController : Controller
    {
        int pageSize = 10;

        private CaresoftHMISEntities db;
        private CaresoftHMISEntities db2;

        public UserManagementController()
        {
            db = new CaresoftHMISEntities();
            db2 = new CaresoftHMISEntities();
        }

        // GET: UserManagement
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult UserMaster(int? page, int? uid, int? dept)
        {
            ViewBag.dept = (dept ?? 0);

            int pageNumber = (page ?? 1);

            var data = db.Users.ToList();

            if (uid != null && uid != 0)
            {
                data = data.Where(e => e.Id == (int)uid).ToList();
            }

            if (dept != null && dept != 0)
            {
                data = data.Where(e => e.Employee.DepartmentId == (int)dept).ToList();
            }

            ViewBag.Departments = db.Departments.ToList();

            ViewBag.SingleDept = (dept ?? 0);
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RollMaster()
        {
            List<UserRole> userRoles = new List<UserRole>();

            userRoles = db.UserRoles.ToList();
            return View(userRoles);
        }

        public ActionResult AddRole()
        {
            ViewBag.Departments = db.Departments;
            return View();
        }

        public ActionResult EditRole(int id)
        {
            ViewBag.Departments = db.Departments;
            return View(db.UserRoles.Find(id));
        }

        [HttpPost]
        public ActionResult EditRole(UserRole userRole)
        {
            ViewBag.Departments = db.Departments;
            db.Entry(userRole).State = EntityState.Modified;
            db.SaveChanges();

            return View(db.UserRoles.Find(userRole.Id));
        }

        [HttpPost]
        public ActionResult AddRole(UserRole UserRole)
        {
            ViewBag.Departments = db.Departments;

            if (ModelState.IsValid)
            {
                db.UserRoles.Add(UserRole);
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult AddUser(AddUserUserManagementData data)
        {

            AddUserUserManagementData addUserUserManagementData = new AddUserUserManagementData()
            {
                lstDepartments = db.Departments.ToList()
            };
            return View(addUserUserManagementData);
        }

        public ActionResult EditUser(int id)
        {
            var user = db.Users.Find(id);

            AddUserUserManagementData addUserUserManagementData = new AddUserUserManagementData()
            {
                lstDepartments = db.Departments.Where(e => e.Id == user.Employee.DepartmentId).ToList(),
                lstEmployee = db.Employees.Where(e => e.Id == user.EmployeeId).ToList(),
                lstUserRole = db.UserRoles.ToList()
            };
            return View(addUserUserManagementData);
        }

        public ContentResult SearchEmployeesByDept(int? id)
        {
            if (id != null)
            {
                int deptId = Convert.ToInt32(id);

                List<Employee> lstEmployees = new List<Employee>();

                lstEmployees = db.Employees.Where(p => p.DepartmentId == deptId).ToList();

                var data = "<option>Select Employee</option>";

                foreach (var emp in lstEmployees)
                {
                    data += "<option value=" + emp.Id + ">" + emp.FName + " " + emp.OtherName + "</option>";
                }

                return Content(data);
            }

            return Content("<p> No Employees in that Dept </p>");

        }

        public ActionResult GetEmployeeInfoAndUserRole(int? id)
        {

            if (id == null)
            {
                return Content("<p>An error Occured Getting the Employee Details</p>");
            }

            var emp = db.Employees.Find(id);

            var mail = new MailAddress(emp.Email.ToString());
            var UserName = mail.User;

            List<allRoles> roles = new List<allRoles>();

            foreach (var item in db.UserRoles.ToList())
            {
                allRoles rol = new allRoles()
                {
                    Id = item.Id,
                    roleName = item.RoleName
                };

                roles.Add(rol);
            }



            EmployeeAndLstUserRoles data = new EmployeeAndLstUserRoles()
            {

                Email = emp.Email,
                Mobile = emp.Mobile,
                UserName = UserName,
                password = UserName + "123",
                userRoles = roles
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUserToRole(UserInfo userInfo, int? id)
        {
            try
            {
                if (userInfo != null)
                {
                    User user = new User();

                    user.Password = userInfo.password;
                    user.Username = userInfo.username;
                    user.AuthToken = "ASDfX77UpDlZXerSwqtP";
                    user.DateAdded = DateTime.Now;
                    user.EmployeeId = userInfo.employeeId;
                    user.UserRoleId = userInfo.userRoleId;
                    user.Status = "Active";
                    user.LastRefresh = DateTime.Now.Date;

                    if (id == null || id < 1)
                    {
                        var userExists = db.Users.Any(e => e.Username == user.Username);
                        if (!userExists)
                        {
                            db.Users.Add(user);
                        }
                        else
                        {
                            return Json("Username Already taken.");
                        }
                    }
                    else
                    {
                        var user_ = db.Users.Find(id);

                        user_.Password = userInfo.password;
                        user_.Username = userInfo.username;
                        user_.AuthToken = "ASDfX77UpDlZXerSwqtP";
                        user_.DateAdded = DateTime.Now;
                        user_.EmployeeId = userInfo.employeeId;
                        user_.UserRoleId = userInfo.userRoleId;
                        user_.Status = "Active";
                        user_.LastRefresh = DateTime.Now.Date;
                    }
                    db.SaveChanges();
                    string res = "successful";
                    return Json(res);

                    //AddUserUserManagementData addUserUserManagementData = new AddUserUserManagementData()
                    //{
                    //    lstDepartments = db.Departments.ToList()
                    //};
                    //return View("AddUser", addUserUserManagementData);


                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return View("AddUser");
        }

        public ActionResult UserRollRight()
        {
            AssignRoleObj assignRoleObj = new AssignRoleObj()
            {
                userRoles = db.UserRoles.ToList(),
                departments = db.Departments.ToList(),
                controllers = db.TblControllers.Select(p => p.Name).Distinct().ToList()
            };
            return View(assignRoleObj);
        }

        public ActionResult GetActionsInAController(GetActionsInControllerData data)
        {

            List<TblController> controller = db.TblControllers.Where(p => p.Name == data.ControllerName)
                                                              .ToList();

            ControllerModel controllerModel = new ControllerModel()
            {
                ActionsToBeAdded = new List<TblController>(),
                ActionsToBeRemoved = new List<TblController>()
            };

            foreach (var item in controller)
            {
                //check if the action exists in the group rights table
                var val = db.GroupRights.Where(p => p.ActionId == item.Id && p.UserRoleId == data.UserRoleId).FirstOrDefault();

                if (val != null)
                {
                    controllerModel.ActionsToBeRemoved.Add(item);
                }
                else
                {
                    controllerModel.ActionsToBeAdded.Add(item);
                }

            }

            return Json(controllerModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRoleRights(int id, int userRoleId)
        {
            var userRoles = db.UserRoles.Find(id);

            var groupRights = db.GroupRights.Where(e => e.UserRoleId == id).ToList();

            var role_rights = db.RoleRights.Where(e => e.DepartmentId == id)
                .Select(x => new
                {
                    Id = x.Id,
                    RoleRightName = x.RoleRightName,
                    DepartmentId = x.DepartmentId,
                    AlreadyAssigned = x.GroupRights.Any(e => e.Status == "Active" && e.RoleRightId == x.Id && e.UserRoleId == userRoleId)
                });
            return Json(role_rights, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveGroupUserRight(AddRolesRight addRolesRight)
        {
            addRolesRight.dateAdded = DateTime.Now.Date;
            if (!ModelState.IsValid)
            {
                string result = "<div class=\"alert alert-warning\">Not Allowed!</ div > ";
                return Json(result);
            }
            else
            {
                GroupRight groupRight = new GroupRight()
                {
                    ActionId = addRolesRight.ActionId,
                    UserRoleId = addRolesRight.UserRoleId,
                    TimeAdded = addRolesRight.dateAdded,
                    Status = addRolesRight.Status
                };
                GroupRight gr = db.GroupRights.Where(p => p.ActionId == groupRight.ActionId && p.UserRoleId == groupRight.UserRoleId).FirstOrDefault();

                if (gr == null)
                {
                    db.GroupRights.Add(groupRight);
                    db.SaveChanges();
                    string result = "<div class=\"alert alert-success\"> <b>Successful!!</b><br/> Role Assigned </ div > ";
                    return Json(result);
                }
                else
                {
                    string result = "<div class=\"alert alert-warning\">Role Already Assigned </ div > ";
                    return Json(result);
                }
            }
        }

        public ActionResult SaveGroupUserRightRefined(AddRolesRightRefined addRolesRightRefined)
        {

            if (!ModelState.IsValid)
            {
                string result = "<div class=\"alert alert-warning\">Not Allowed!</ div > ";
                return Json(result);
            }
            else
            {
                var roleExists = false;

                GroupRight groupRight = new GroupRight()
                {
                    ActionId = 0,
                    RoleRightId = addRolesRightRefined.RoleRightId,
                    UserRoleId = addRolesRightRefined.UserRoleId,
                    TimeAdded = DateTime.Now,
                    Status = addRolesRightRefined.Status
                };

                roleExists = db.GroupRights.Any(p => p.RoleRightId == addRolesRightRefined.RoleRightId);



                if (!roleExists)
                {
                    db.GroupRights.Add(groupRight);
                    db.SaveChanges();
                    string result = "<div class=\"alert alert-success\"> <b>Successful!!</b><br/> Role Assigned </ div > ";
                    return Json(result);
                }
                else
                {
                    string result = "<div class=\"alert alert-warning\">Role Already Assigned </ div > ";
                    return Json(result);
                }
            }
        }
        public JsonResult RemoveUserGroupRightRefined(AddRolesRightRefined addRolesRightRefined)
        {
            if (!ModelState.IsValid)
            {
                string result = "<div class=\"alert alert-warning\">Not Allowed!</ div > ";
                return Json(result);
            }
            else
            {
               
                var gr = db.GroupRights.Where(p => p.RoleRightId == addRolesRightRefined.RoleRightId);


                if (gr.Any())
                {

                    db.GroupRights.RemoveRange(gr);
                    db.SaveChanges();
                    string result = "<div class=\"alert alert-success\"> <b>Successful!!</b><br/> Role Removed Successfully</ div > ";
                    return Json(result);
                }
                else
                {
                    string result = "<div class=\"alert alert-warning\">Role Doesnt Exist</ div > ";
                    return Json(result);
                }
            }

        }

        public JsonResult rMoveGroupRight(AddRolesRight addRolesRight)
        {
            object obj = new object();

            obj = addRolesRight;
            return Json(obj);
        }

        public JsonResult RemoveUserGroupRight(AddRolesRight addRolesRight)
        {
            if (!ModelState.IsValid)
            {
                string result = "<div class=\"alert alert-warning\">Not Allowed!</ div > ";
                return Json(result);
            }
            else
            {
                GroupRight groupRight = new GroupRight()
                {
                    ActionId = addRolesRight.ActionId,
                    UserRoleId = addRolesRight.UserRoleId,
                    TimeAdded = addRolesRight.dateAdded,
                    Status = addRolesRight.Status
                };
                GroupRight gr = db.GroupRights.Where(p => p.ActionId == groupRight.ActionId && p.UserRoleId == groupRight.UserRoleId).FirstOrDefault();


                if (gr != null)
                {

                    db.GroupRights.Remove(gr);
                    db.SaveChanges();
                    string result = "<div class=\"alert alert-success\"> <b>Successful!!</b><br/> Role Removed Successfully</ div > ";
                    return Json(result);
                }
                else
                {
                    string result = "<div class=\"alert alert-warning\">Role Doesnt Exist</ div > ";
                    return Json(result);
                }
            }

        }

        public ActionResult AddRoleRight()
        {
            ViewBag.UserRoles = db.Departments;
            return View();
        }

        [HttpPost]
        public ActionResult AddRoleRight(RoleRight roleRight)
        {
            ViewBag.UserRoles = db.Departments;

            if (ModelState.IsValid)
            {
                db.RoleRights.Add(roleRight);
                db.SaveChanges();
                return View();

            }
            return View(roleRight);
        }

        public ActionResult AddEmployee()
        {
            EmployeeData2 employeesData = new EmployeeData2()
            {
                Employee = new Employee(),
                Employess = db.Employees.OrderByDescending(p => p.Id).ToList(),
                DesignationsList = db.Designations.ToList(),
                LstDepartment = db.Departments.ToList()
            };
            if (employeesData == null)
            {
                return Content("Error Occured");
            }

            ViewBag.LstDepartments = db.Departments.ToList();
            return View(employeesData);
        }

        [HttpPost]
        public ActionResult AddEmployee(CaresoftHMISDataAccess.Employee employee)
        {
            employee.DateAdded = DateTime.Now.Date;
            
            employee.BranchId = (int)Session["UserBranchId"] ;
            try
            {
                if (ModelState.IsValid || ModelState != null)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    //var emp = db.Employees.Include().OrderByDescending(p => p.Id).ToList();
                    var emp = db.Employees.Include(p => p.Department).Include(x => x.Designation).OrderByDescending(p => p.Id).ToList();
                    return PartialView("_Employees", emp);
                }
                return View(employee);

            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);


                var fullErrorMessage = string.Join("; ", errorMessages);


                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


                return Content(exceptionMessage);
            }


        }
        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult RoleRightActions(int? dep, int? userrole, int? roleright, string area, string controller)
        {
            if (dep == null)
            {
                dep = 0;
            }

            if (userrole == null)
            {
                userrole = 0;
            }

            if (roleright == null)
            {
                roleright = 0;
            }

            if (area == null || area.Length == 0)
            {
                area = "";
            }

            if (controller == null || controller.Length == 0)
            {
                controller = "";
            }

            ViewBag.Departments = db.Departments.ToList();
            ViewBag.RoleRights = db.RoleRights.Where(e => e.DepartmentId == dep).ToList();
            ViewBag.Areas = db.TblControllers.GroupBy(e => e.Area).ToList();
            ViewBag.Controllers = db.TblControllers.Where(e => e.Area == area).GroupBy(e => e.Name).ToList();

            ViewBag.dep = dep;
            ViewBag.roleright = roleright;
            ViewBag.userrole = userrole;
            ViewBag.area = area;
            ViewBag.controller = controller;



            return View();
        }

        public ActionResult getRoleRightActions(string area, string cont, int roleRightId)
        {
            //var data = db.RoleRightsActions
            //    .Where(e => e.TblController.Name == controller && e.TblController.Area == area).ToList();

            var data = db.TblControllers
               .Where(e => e.Name == cont && e.Area == area.Trim())
               .Select(x => new { x.Id, x.Name, x.Area, x.Action, available = x.RoleRightsActions.Any(e => e.RoleRightId == roleRightId) }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult activateRoleRightActions(RoleRightsAction rightsAction)
        {
            if (!db.RoleRightsActions.Any(e => e.ActionId == rightsAction.ActionId && 
            e.RoleRightId == rightsAction.RoleRightId))
            {
                db.RoleRightsActions.Add(rightsAction);
                if (db.SaveChanges() > 0)
                {
                    return Json("Rights Awarded Successfully", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Already awarded", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult deactivateRoleRightActions(RoleRightsAction rightsAction)
        {
            var rr = db.RoleRightsActions.FirstOrDefault(e => e.ActionId == rightsAction.ActionId &&
                        e.RoleRightId == rightsAction.RoleRightId);

            if (rr != null)
            {                
                db.RoleRightsActions.Remove(rr);
                if (db.SaveChanges() > 0)
                {
                    return Json("Rights Awarded Revoked", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Already revoked", JsonRequestBehavior.AllowGet);

            }
        }

        #region Users and their Roles 

        public ActionResult UserRolesAndRights()
        {
            return View();
        }

        public ActionResult GetUserRolesAndRightsReport()
        {
            string Format = "PDF";
            var dataSet = GetUserRolesAndRightsData();

            ReportDocument Rd = new ReportDocument();

            Rd.Load(Path.Combine(Server.MapPath("~/Areas/CareSoftReports/Reports/UserRolesAndRights/RptUserRolesAndRights.rpt")));
            Rd.SetDataSource(dataSet);
            Rd.Subreports["RptReportHeader.rpt"].SetDataSource(GetAllReportHeader());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            if (Format != "PDF")
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "User and Roles Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .xlsx";

                return File(Stream, "application/xlsx", FileName);
            }
            else
            {
                Stream Stream = Rd.ExportToStream(
                CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream.Seek(0, SeekOrigin.Begin);
                string FileName = "User and Roles Report " + DateTime.Now.ToString("dd-MM-yyyy") + " .pdf";

                return File(Stream, "application/pdf", FileName);
            }
        }

        private class UserModel
        {
            public string EmployeeNumber { get; set; }
            public string UserName { get; set; }
            public string Role { get; set; }
            public string roleRights { get; set; }
            public string FullNames { get; set; }
            public string Department { get; set; }

        }

        public DataSetUserRolesAndRights GetUserRolesAndRightsData()
        {
            DataSetUserRolesAndRights dataSet = new DataSetUserRolesAndRights();

            List<UserModel> lstUserModel = new List<UserModel>();

            var AllUsers = db2.Users.ToList();
            var AllGroupRights = db2.GroupRights.ToList();

            foreach (var item in AllUsers)
            {
                var employee = item.Employee;
                UserModel userModel = new UserModel()
                {
                    EmployeeNumber = item.EmployeeId.ToString(),
                    UserName = item.Username,
                    Role = item.UserRole.RoleName,
                    Department = item.Employee.Department.DepartmentName ?? "",
                    FullNames = employee.Salutation + " " + employee.FName + " " + employee.OtherName
                };

                if (item.UserRole.RoleName == "SA")
                {
                    userModel.roleRights = "Has Access to All Modules";
                }
                else
                {
                    var groupRight = AllGroupRights.Where(p => p.UserRoleId == item.UserRoleId).ToList();

                    string roleRightName = "";

                    foreach (var itm in groupRight)
                    {
                        roleRightName += itm.RoleRight.RoleRightName + ", ";
                    }

                    userModel.roleRights = roleRightName;
                }

                if (userModel.roleRights == "")
                {
                    userModel.roleRights = "No Access to any Module";
                }


                lstUserModel.Add(userModel);
            }




            foreach (var dat in lstUserModel)
            {
                dataSet.UsersAndRights.AddUsersAndRightsRow(
                    dat.EmployeeNumber,
                    dat.UserName,
                    dat.Role,
                    dat.roleRights.TrimEnd(new Char[] { ',' }),
                    dat.FullNames,
                    dat.Department);
            }


            return dataSet;
        }

        private DataSetFacilityInformation GetAllReportHeader()
        {

            var facilityDataSet = new DataSetFacilityInformation();
            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            var facilityAddress = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityAddress").FirstOrDefault().Value;

            var facilityTelephone = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityContactNumber").FirstOrDefault().Value;

            var logoUrl = Path.Combine(Server.MapPath("~/Content/icons/HospitalLogo.png"));

            facilityDataSet.HospitalDetails.AddHospitalDetailsRow(
                HospitalName,
                facilityAddress,
                facilityTelephone,
                logoUrl
            );

            return facilityDataSet;
        }

        public ActionResult Reactivate(int id)
        {
            var user = db.Users.Find(id);
            if (user != null && user.LoginFailureCount >= 5)
            {
                user.LoginFailureCount = 0;
                db.SaveChanges();

            }
            else
            {
                user.LoginFailureCount = 5;
                db.SaveChanges();

            }

            return RedirectToAction("UserMaster");
        }


        //From Barbara

        private CaresoftHMISEntities context = new CaresoftHMISEntities();
        private ProcurementDbContext pcontext = new ProcurementDbContext();
        // GET: ActiveUsers/ActiveUser
        public ActionResult ActiveUsers()
        {
            var ActiveUsers = context.Users;

            //var dataSource = new List<Caresoft2_0_Areas_ActiveUsers_Models_Users>();
            return View(ActiveUsers);
        }

        public ActionResult ExportUsers()
        {
            var allUser = context.Users.ToList();


            var ds = new ActiveUser();

            foreach (var us in allUser)
            {
                ds.DTActiveUser.AddDTActiveUserRow(us.Id.ToString(), 
                    us.Username, us.LastRefresh.Value.ToString(), us.Status, us.Employee.RollNo, "");

            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/UserManagement/ActiveUsers.rpt")));
            rd.SetDataSource(ds);


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "UserList.pdf");

        }

        #endregion

        //public ActionResult AllEmployeesReport()
        //{
        //    var empList = db.Employees.ToList();

        //    EmployeeDataSet employeeDataSet = new EmployeeDataSet();

        //    foreach (var item in empList)
        //    {
        //        employeeDataSet.Employee.AddEmployeeRow(item.Id,item.FName,item.OtherName,item.Gender,item.IdNo,item.DOB.ToString());
        //    }

        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/UserManagementReports/ReportEmployees.rpt")));
        //    rd.SetDataSource(employeeDataSet);

        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();

        //    Stream stream = rd.ExportToStream(
        //        CrystalDecisions.Shared.ExportFormatType.Excel);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    return File(stream, "application/pdf", "ListOfEmployees.xls");

        //}

        //public ActionResult EmployeeByDept()
        //{
        //    var empList = db.Employees.ToList();



        //    GroupEmployeesDept employeeDataSet = new GroupEmployeesDept();


        //    foreach (var item in empList)
        //    {
        //        employeeDataSet.Employee.AddEmployeeRow(
        //            item.Id,
        //            item.FName,
        //            item.OtherName,
        //            item.Gender,
        //            item.IdNo,
        //            item.DOB.ToString(),
        //            item.Department.Id.ToString(),
        //            item.Department.DepartmentName);
        //    }

        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/UserManagementReports/ReportGroupEmployees.rpt")));
        //    rd.SetDataSource(employeeDataSet);

        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();

        //    Stream stream = rd.ExportToStream(
        //        CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    return File(stream, "application/pdf", "ListOfEmployeesWithDept.pdf");

        //}


    }
}