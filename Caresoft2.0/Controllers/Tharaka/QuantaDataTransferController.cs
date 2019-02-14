using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TharakaDataAccess;
using CaresoftHMISDataAccess;
using System.Data.Entity.Validation;

namespace Caresoft2._0.Controllers.Tharaka
{
    public class QuantaDataTransferController : Controller
    {
        private TharakaEntities tDB = new TharakaEntities();
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: AddressBooks
        public ActionResult AddressBook()
        {
            //return Content("This action has been disabled by the dev for data safety");
            var start = DateTime.Now;

            var tharakaPatients = tDB.AddressBooks.ToList();
            var caresoftPatients = db.Patients.ToList();

            foreach (var tp in tharakaPatients)
            {
                if (caresoftPatients.FirstOrDefault(e=>e.RegNumber.Trim()==tp.PePatID.Trim())!=null)
                {
                    continue;
                }
            try
                {
                    var cp = new Patient();

                    cp.Id = tp.Srno;
                    cp.RegNumber = tp.PePatID;
                    if (tp.Initial != null)
                    {
                        tp.Initial = tp.Initial.Trim();
                    }
                    cp.Salutation = tp.Initial;
                    cp.FName = tp.FirstName.Trim();
                    cp.MName = tp.MiddleName.Trim();
                    cp.LName = tp.LastName.Trim();
                    cp.Gender = tp.sex.Substring(0, 1).ToUpper();
                    var age = (int)tp.Age;
                    var ageFlag = tp.ageflag;
                    //if (ageFlag == null)
                    //{
                    //    ageFlag = "Years";
                    //}

                    //if (ageFlag == "Years")
                    //{
                    //    cp.DOB = DateTime.Today.AddYears(-age);
                    //}
                    //else if (ageFlag == "Months")
                    //{
                    //    cp.DOB = DateTime.Today.AddMonths(-age);
                    //}
                    //else
                    //{
                    //    cp.DOB = DateTime.Today.AddDays(-age);
                    //}
                    cp.DOB = tp.DOB;
                    cp.Religion = tp.Religion;
                    cp.Mobile = tp.Mobile;
                    cp.HomeAddress = tp.Address;
                    cp.IdentificationType = tp.NationalIdType;
                    cp.NationalId = tp.NationalId;
                    cp.KinInitial = tp.Kin;
                    cp.Email = tp.Email;
                    cp.BloodGroup = tp.BloodGroup;
                    cp.MaritalStatus = tp.MaritalStatus;
                    cp.RegDate = DateTime.Today;
                    cp.RegTime = DateTime.Now.TimeOfDay;
                    cp.Password = tp.Ppassword;
                    cp.UserId = 1;

                    db.Patients.Add(cp);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {

                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);


                    var fullErrorMessage = string.Join("; ", errorMessages);

                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    var error = new KeyValuePair();
                    error.Key_ = "error " + tp.Srno;
                    error.Value = exceptionMessage;

                    db.KeyValuePairs.Add(error);
                    db.SaveChanges();

                }catch(Exception e)
                {
                    var error = new KeyValuePair();
                    error.Key_ = "error " + tp.Srno;
                    error.Value = e.ToString();

                    db.KeyValuePairs.Add(error);
                    db.SaveChanges();
                }

            }
            

            
            var stop = DateTime.Now;
            var duration = stop - start;

            return Content("Moved "+tharakaPatients.Count().ToString()+" in "+duration);
        }

        public ActionResult DeleteDuplicatePatients()
        {
            var pats = db.Patients.ToList();

            var original = new List<Patient>();

            var deleted = 0;

            foreach(var p in pats)
            {
                if (!original.Select(e=>e.RegNumber).Contains(p.RegNumber))
                {
                    original.Add(p);
                }
                else
                {
                    try
                    {
                        db.Patients.Remove(p);
                        db.SaveChanges();
                        deleted++;
                    }
                    catch(Exception e)
                    {
                        return Content(e.ToString());
                    }
                }
            }

            return Content("Deleted " + deleted);
        }

        public ActionResult ServiceChatter()
        {
            var quantaServices = tDB.view_servicechartter.Where(e => e.groupid != 151).ToList();
            var revenueDepId = db.DepartmentTypes.FirstOrDefault(e => e.DepartmnetType.Trim().ToLower() == "revenue").Id;
            var r = 0;
            var start = DateTime.Now;
            foreach (var s in quantaServices)
            {
                var caresoftdepartments = db.Departments.ToList();
                var caresoftServices = db.Services.ToList();
                var caresoftServiceGroups = db.ServiceGroups.ToList();

                try
                {
                    if(caresoftServices.FirstOrDefault(e=>e.ServiceName.ToLower().Trim() == s.Particular.ToLower().Trim()) == null)
                    {
                        var caresoftServ = new Service();
                        var sg = caresoftServiceGroups.FirstOrDefault(e => e.ServiceGroupName.Trim().ToLower() == "procedure");
                        var dep = caresoftdepartments.FirstOrDefault(e => e.DepartmentName.Trim().ToLower() == s.groups.ToLower().Trim());
                        if (dep == null)
                        {
                            dep = new Department();
                            dep.DepartmentName = s.groups.Trim();
                            dep.DepartmentType = revenueDepId;
                            dep.DateAdded = DateTime.Now;
                            db.Departments.Add(dep);
                            db.SaveChanges();
                        }
                        caresoftServ.ServiceGroupId = sg.Id;
                        caresoftServ.DepartmentId = dep.Id;
                        caresoftServ.ServiceName = s.Particular.Trim();
                        caresoftServ.CashPrice = (double)s.rate;
                        caresoftServ.UserId = 1;
                        
                        caresoftServ.BranchId = (int)Session["UserBranchId"] ;
                        caresoftServ.DateAdded = DateTime.Now;
                        db.Services.Add(caresoftServ);
                        db.SaveChanges();
                    }
                }catch(Exception e)
                {
                    var error = new KeyValuePair();
                    error.Key_ = "error " + s.Particular;
                    error.Value = e.ToString();

                    db.KeyValuePairs.Add(error); db.SaveChanges();
                }
                r++; 
            }

            var stop = DateTime.Now;
            var duration = stop - start;

            return Content("Moved " + r.ToString() + " in " + duration);
        }

        
    }
}


