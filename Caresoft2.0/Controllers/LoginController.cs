using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
using Caresoft2._0.CustomData;
using System.Data.Entity.Validation;
using System.Reflection;
using System.Collections.Generic;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Caresoft2._0.Controllers
{

    
    public class LoginController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: Login
        [LS]
        public ActionResult Index()
        {
            Session["IsDebugMode"] = new LoginController().IsDebugMode;


            Session["Version"] = Assembly.GetExecutingAssembly().GetName().Version;

            GetAllControllers();

            //if user is logged in already, go to dashboard
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            // if no user exist in the db, seed super user
            if(db.Users.ToList().Count < 1)
            {
                var dep = new Department();
                var depType = new DepartmentType();

                if (db.Departments.ToList().Count() < 1)
                {
                    if (db.DepartmentTypes.ToList() .Count()< 1)
                    {
                       
                        depType.DepartmnetType = "Expense";
                        depType.DateAdded = DateTime.Now;
                        db.DepartmentTypes.Add(depType);
                        db.SaveChanges();

                        depType.DepartmnetType = "Revenue";
                        depType.DateAdded = DateTime.Now;
                        db.DepartmentTypes.Add(depType);
                        db.SaveChanges();
                    }
                    //seed ICT department
                   
                    dep.DepartmentName = "ICT";
                    dep.DepartmentType = db.DepartmentTypes.FirstOrDefault(e => e.DepartmnetType.Equals("Expense")).Id;
                    dep.DateAdded = DateTime.Now;
                    db.Departments.Add(dep);
                    db.SaveChanges();
                }
               
                Employee employee = new Employee
                {
                    RollNo = "000",
                    FName = "System",
                    OtherName = "Developer",
                    DepartmentId = db.Departments.FirstOrDefault(e => e.DepartmentName.Equals("ICT")).Id,
                    DateAdded = DateTime.Now
                };

                db.Employees.Add(employee);
                db.SaveChanges();

                if (db.UserRoles.ToList().Count() < 1)
                {
                    var ur = new UserRole();
                    ur.RoleName = "SA";
                    ur.DepartmentId = db.Departments.FirstOrDefault(e => e.DepartmentName.Equals("ICT")).Id;
                    ur.RoleDescription = "Super User";
                    db.UserRoles.Add(ur);
                    db.SaveChanges();
                }
                User superUser = new User
                {
                    Username = "Dev",
                    EmployeeId = employee.Id,
                    Password = "123456", //remember to encrypt
                    DateAdded = DateTime.Now,
                    AuthToken = "ASDfX77UpDlZXerSwqtP", //generate random
                    LastRefresh = DateTime.Now,
                    UserRoleId = db.UserRoles.FirstOrDefault(e=>e.RoleName.Equals("SA")).Id,
                    Status = "Active"
                };

                db.Users.Add(superUser);
                db.SaveChanges();
            }
            if(Session["RequestActivation"] !=null)
            {
                ViewBag.RequestActivation = (bool)Session["RequestActivation"];
            }

            return View();
        }

        public bool IsDebugMode
        {
            get
            {
            #if DEBUG
             return true;
            #else
                return false;
            #endif
            }
        }
        //post login details
        [HttpPost]
        public ActionResult Index(LoginData loginData)
        {
            var user = db.Users.FirstOrDefault(e => e.Password.Equals(loginData.Password) && e.Username.Equals(loginData.Username) && e.Status.Equals("Active"));


            if (!IsDebugMode)
            {
                if (!new LS().isActiveLicense())
                {
                    //return error message
                    ViewBag.ErrorMessage = "Please acitvate product to continue using the system";
                    ViewBag.RequestActivation = true;
                    return View();
                }
            }


            if (user != null)
            {
                if (user.LoginFailureCount >= 5)
                {
                    ViewBag.ErrorMessage = "Your Account Has been locked due to suspecius activities! Please Report this issue to the Systems admin" +
                        " " + (5 - user.LoginFailureCount) +
                   " Attemps Remain";
                    return View();

                }
                else
                {
                    user.LoginFailureCount = 0;
                    db.SaveChanges();
                }

                if (user.Password == "1234" || user.Password == user.Username + "123")
                {
                    if (user.LockOutDate == null)
                    {
                        user.LockOutDate = DateTime.Now.AddDays(3);
                        db.SaveChanges();

                    }


                    if (user.LockOutDate > DateTime.Now)
                    {
                        Session["ChangePasswordRequestMessage"] = "Please Change your password before " + user.LockOutDate;

                    }
                    else
                    {
                        Session["ChangePasswordRequestLock"] = true;

                        Session["ChangePasswordRequestMessage"] = "Your Account has been deactivated! Please Change your password! If " +
                            "you still get this message, contact system admin";
                    }
                    Session["ChangePasswordRequest"] = true;


                }
                //start session and proceed to dashboard
                Session["UserId"] = user.Id;
                Session["UserBranchId"] = user.Employee.BranchId;
                Session["UserRole"] = user.UserRole.RoleName;
                
                if (Request.UrlReferrer != null)
                {
                    return RedirectToRoute(Request.UrlReferrer);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var devUser = db.Users.FirstOrDefault(e => e.Username == loginData.Username);

                if (devUser == null)
                {
                    //return error message
                    ViewBag.ErrorMessage = "Invalid Username/Password combination! " +
                        " Attemps Remain";

                    return View();

                }
                if (devUser.LoginFailureCount < 5)
                {
                    devUser.LoginFailureCount = devUser.LoginFailureCount + 1;
                    db.SaveChanges();
                    //return error message
                    ViewBag.ErrorMessage = "Invalid Username/Password combination! " + (5 - devUser.LoginFailureCount) +
                        " Attemps Remain";
                }
                else
                {
                    ViewBag.ErrorMessage = "Your Account Has been locked due to suspecius activities! Please Report this issue to the Systems admin" +
                        " " + (5 - devUser.LoginFailureCount) +
                   " Attemps Remain";
                }

            
                return View();

            }
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,AuthToken,LastRefresh,Role,Status,DateAdded")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

         [HttpPost]
        public ActionResult ChangePassword(String CPassword, String NewPassword)
        {
            var currentUserId = Session["UserId"];
            if (currentUserId == null)
            {
                return Json(new { Status = "Unauthorized"});
            }
            var user = db.Users.Find(Int32.Parse(currentUserId.ToString()));
            if (user.Password == CPassword)
            {
                user.Password = NewPassword;
                db.SaveChanges();
                Session.Abandon();
                return Json(new { Status = "success", Message = "Password Cahnged Successfully!" });
            }
            else
            {
                return Json(new { Status = "warning", Message = "Incorrect Current Password! Try Again" });
            }
        }
  

        public class ActivationFormData
        {
            public String part1 { get; set; }
            public String part2 { get; set; }
            public String part3 { get; set; }
            public String part4 { get; set; }
            public String part5 { get; set; }
        }
        public ActionResult Activation(ActivationFormData data)
        {
            string code = data.part1 + data.part2 + data.part3 + data.part4 + data.part5;
            string mac = new LS().mac();

            using (WebClient client = new WebClient())
            {
                NameValueCollection nvc = new NameValueCollection()
                 {
                     { "activationData", "{\"mac_address\":\""+mac+"\",\"package\":\"caresoft.softcom.co.ke\",\"code\":\""+code+"\"}" }
                 };

                

                try
                {
                    byte[] responseBytes = client.UploadValues("https://softcom.co.ke/apps/activation.php", nvc);
                    string response = System.Text.Encoding.ASCII.GetString(responseBytes);

                    JObject json = JObject.Parse(response);
                    if(json.GetValue("active").ToString() == "Yes")
                    {
                        new LS().updateLicense("active");
                    }

                }catch (Exception e)
                {
                    return Content("There was an error attempting to activate your copy of Caresoft HMS. Please check your internet connection and try again. If problem persists contact vendor for assistance.");
                }

                return Redirect("Index");
            }
        }

        public ActionResult Error()
        {

            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }

        public ActionResult UnauthorizedAccess()
        {
            return View();
        }

        #region Register all Controllers to db
        public void GetAllControllers()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new {Area = x.DeclaringType.Namespace, Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

           // List<TblController> LstControllerClass = new List<TblController>();
            
            //TblController ControllerClass = new TblController();

            foreach (var item in controlleractionlist)
            {
                TblController controller = new TblController
                {
                    Action = item.Action,
                    Attributes = item.Attributes,
                    Name = item.Controller,
                    ReturnType = item.ReturnType,
                    Area = item.Area
                };

                AddControllersToDb(controller);
            }


        }

        private void AddControllersToDb(TblController controller)
        {
            //goes through the tblcontrollers table in db and tries to find if the controller exists
            TblController _Controller = db.TblControllers
                            .FirstOrDefault(p => p.Name == controller.Name
                            && p.Action == controller.Action
                            && p.Attributes == controller.Attributes
                            && p.ReturnType == controller.ReturnType && p.Area == controller.Area);

            // if there is no similar controller then we add the new controller
            if (_Controller == null)
            {
                db.TblControllers.Add(controller);
                db.SaveChanges();

            }
          
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
