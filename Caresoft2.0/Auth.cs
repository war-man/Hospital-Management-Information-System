using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System.Data.Entity;
using Caresoft2._0.Controllers;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Diagnostics;

namespace Caresoft2._0
{
    public class Auth : ActionFilterAttribute
    {
        public CaresoftHMISEntities Db;


        public Auth()
        {
            Db = new CaresoftHMISEntities();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)

        {

            HttpSessionStateBase session = filterContext.HttpContext.Session;

            var LoggedInUser = session["UserId"];
            if (LoggedInUser == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Login", action = "Index", area = "" }));

              
            }
            else
            {
                
                //get the user id from the cookie
                var userId = (int)(LoggedInUser);


                //int userRole = Db.Users.Find(userId).UserRoleId;

                //get the controller names then filter the users allowed to a certain controller. 

                //var area_props = filterContext.HttpContext.Request.Path.Split('/');
                //Debug.WriteLine(filterContext.HttpContext.Request.Params.Get("Area"));

                //var Area = ("Caresoft2._0.Controllers");



                //if (area_props[1].Length > 0)
                //{
                //    string directory = "~/Areas/" + area_props[1];
                //    if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(directory)))
                //    {
                //        Area = ("Caresoft2._0.Areas." + area_props[1] + ".Controllers");
                //    }
                //}

                //Debug.WriteLine(Area);


                var Controller = filterContext.Controller.ToString();

                //var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";
                var actionName = filterContext.ActionDescriptor.ActionName;


                var user = Db.Users.FirstOrDefault(e=>e.Id == userId);

                if (session["ChangePasswordRequest"] !=null)
                {
                    var ChangePasswordRequest = (Boolean)session["ChangePasswordRequest"];

                    if (ChangePasswordRequest)
                    {
                        var dashboard = false;

                        if (actionName == "Index" && Controller == "Caresoft2._0.Controllers.HomeController")
                        {
                            dashboard = true;

                            if (user.UserRole.RoleName.ToLower() != "sa")
                            {
                                var default_page = user.UserRole.LandingPage;

                                if (default_page != null && default_page.Trim().Length != 0)
                                {
                                    //new RedirectResult("/Pathology");

                                    filterContext.Result = new RedirectResult(default_page.Trim());
                                }
                                
                            }
                        }

                        if (user.LockOutDate < DateTime.Now && !dashboard)
                        {
                            filterContext.Result = new RedirectToRouteResult(new
                 RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
                        }
                    }
                }

                if (actionName == "Index" && Controller == "Caresoft2._0.Controllers.HomeController")
                {
                    if (user.UserRole.RoleName.ToLower() != "sa")
                    {
                        var default_page = user.UserRole.LandingPage;

                        if (default_page != null && default_page.Trim().Length != 0)
                        {
                            //new RedirectResult("/Pathology");

                            filterContext.Result = new RedirectResult(default_page.Trim());
                            return;

                        }

                    }
                }

                //query db to get the action id
                var Action= Db.TblControllers.Any(e => e.Action == actionName && 
                (e.Area + "." + e.Name) == Controller);

                if(!Action)
                {
                    new LoginController().GetAllControllers();
                    
                }
                //var ActionId = Db.TblControllers.FirstOrDefault(p => p.Name == controllerName && p.Action == actionName).Id;

                //compare the actionid and the role id using a db query
                var _user = Db.Users.FirstOrDefault(e => e.Id == (int)LoggedInUser);
                var RoleRightsActions = Db.GroupRights.Any(p => p.RoleRight.RoleRightsActions
                .Any(e => e.TblController.Action == actionName &&
                (e.TblController.Area + "." + e.TblController.Name ) == Controller &&
                p.UserRoleId == _user.UserRoleId));


                string[] allowedRoles = new string[] { "dev", "sa" };

                if (allowedRoles.Contains(Db.Users.FirstOrDefault(e=> e.Id == (int)LoggedInUser).UserRole.RoleName.ToLower().Trim()))
                {
                    //allow access
                } else if (RoleRightsActions)
                {                   
                   
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Login", action = "UnauthorizedAccess", area = "" }));
                }
               
            }



            //if (session != null && session["UserId"] == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary {
            //        { "Controller", "Login" },
            //        { "Action", "Index" }
            //        });
            //}

            //if(session["UserId"] != null)
            //{
            //    var userId = (int)session["UserId"];
            //    int userRole = Db.Users.Find(userId).UserRoleId;

            //    //get the controller names
            //    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";
            //    var actionName = filterContext.ActionDescriptor.ActionName;

            //    //query db to get the action id
            //    int ActionId = Db.TblControllers.Where(p => p.Name == controllerName && p.Action == actionName).FirstOrDefault().Id;

            //    //compare the actionid and the role id using a db query
            //    var queryable = Db.GroupRights.Where(p => p.ActionId == ActionId && p.UserRoleId == userRole).FirstOrDefault();

            //    if (queryable == null)
            //    {
            //        filterContext.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary {
            //        { "Controller", "Login" },
            //        { "Action", "Error" }
            //            });

            //    }

            //}
            //else
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary {
            //        { "Controller", "Login" },
            //        { "Action", "Index" }
            //        });
            //}
        }
    }
}