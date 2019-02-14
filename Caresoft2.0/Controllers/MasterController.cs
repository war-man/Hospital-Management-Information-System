using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabsDataAccess;
using System.Data.Entity;

namespace Caresoft2._0.Controllers
{
    [Auth]
    public class MasterController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        
        // GET: Isuarance company Master
        public ActionResult InsuranceCompany()
        {
            InsuranceCompanyData insuranceCompanyData = new InsuranceCompanyData();
            insuranceCompanyData.InsuranceCompany = null;
            insuranceCompanyData.InsuranceCompanies = db.Companies.ToList();
            return View(insuranceCompanyData);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInsuranceCompany([Bind(Include = "Id,CompanyName,CompanyCode,Address,Country,Email,Mobile,ContactPersonName,ContactPersonMobile")] CaresoftHMISDataAccess.Company insuranceCompany)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(insuranceCompany);
                db.SaveChanges();
                ViewBag.Message = "success~Company Addedd Successfully";
                return RedirectToAction("InsuranceCompany");
            }
            else
            {
                ViewBag.Message = "warning~Fill in all required fields";
                InsuranceCompanyData insuranceCompanyData = new InsuranceCompanyData();
                insuranceCompanyData.InsuranceCompany = insuranceCompany;
                insuranceCompanyData.InsuranceCompanies = db.Companies.ToList();
                return View("InsuranceCompany", insuranceCompanyData);
            }


        }

        

        // GET: Departments
        public ActionResult Department()
        {
            DepartmentsData departmentsData = new DepartmentsData();
            departmentsData.Departments = db.Departments.OrderByDescending(e=>e.Id).ToList();
            departmentsData.DepartmentTypeList = db.DepartmentTypes.ToList();
            return View(departmentsData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Department(CaresoftHMISDataAccess.Department department)
        {

            department.DateAdded = DateTime.Now;

            try
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return PartialView("Departments", db.Departments);
            } catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);


                var fullErrorMessage = string.Join("; ", errorMessages);


                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


                return Content(exceptionMessage);
            }


        }


        // GET: Services
        public ActionResult Service()
        {

            ServicesData serviceData = new ServicesData();
            serviceData.Services = db.Services.ToList();
            serviceData.DepartmentsList = db.Departments.ToList();
            return View(serviceData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Service([Bind(Include = "Id,ServiceName,DepartmentId,CashPrice")] CaresoftHMISDataAccess.Service service)
        {
            service.DateAdded = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Service");
            }

            ServicesData serviceData = new ServicesData();
            serviceData.Services = db.Services.ToList();
            serviceData.Service = service;
            serviceData.DepartmentsList = db.Departments.ToList();
            return View(serviceData);

        }

        //services added from the modal at the tariff pricing window
        [HttpPost]
        public ActionResult ServiceOnTheFly([Bind(Include = "Id,ServiceName,DepartmentId,CashPrice")] CaresoftHMISDataAccess.Service service)
        {
            service.DateAdded = DateTime.Now;
            try
            {
              
                db.Services.Add(service);
                db.SaveChanges();
                return Json(new {
                    Message = "<b>"+service.ServiceName+"</b> Added Successfully!",
                    Status = true,
                    ServiceId = service.Id,
                    ServiceName = service.ServiceName,
                    CashPrice = service.CashPrice
                });
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

        // GET: Tariff/
        public ActionResult Tariff()
        {

            TariffData tariffData = new TariffData();
            tariffData.CompaniesList = db.Companies.ToList();
            tariffData.Tariffs = db.Tariffs.ToList();
            return View(tariffData);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tariff([Bind(Include = "Id,TariffName,CompanyId,DateAdded")] CaresoftHMISDataAccess.Tariff tariff)
        {
            tariff.DateAdded = DateTime.Now;
            try
            {
                db.Tariffs.Add(tariff);
                db.SaveChanges();
                return RedirectToAction("Tariff");
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

        public ActionResult FilterTariffs(int? id)
        {
            var tariffs = db.Tariffs.Where(e => e.CompanyId == id);
            var options = "";
            foreach(var t in tariffs)
            {
                options += "<option value=" + t.Id + ">"+t.TariffName+"</option>";
            }
            return Content(options);
        }

        public class PriceListFilterData
        {
            public int DepartmentId { get; set; }
            public int TariffId { get; set; }
        }

        public ActionResult ReloadPriceList(PriceListFilterData priceListFilterData)
        {
            if(priceListFilterData.DepartmentId <1)
            {
                return Content("Missing Parameters");
            }
            var dept = db.Departments.Find(priceListFilterData.DepartmentId);

            

            TariffData tariffData = new TariffData();
            tariffData.Tariff = db.Tariffs.Find(priceListFilterData.TariffId);
            tariffData.Services = db.Services.Where(e=>e.DepartmentId == priceListFilterData.DepartmentId).ToList();
            tariffData.ServicePrices = db.ServicesPrices.Where(e=>e.TariffId == priceListFilterData.TariffId).ToList();

            return PartialView("_PriceList", tariffData);
        }


        public bool InsertTestsFromLab(LabTest test, int id) 
        {
         var path = db.Departments.Any(e => e.Id == test.DepartmentRadPath &&
                e.DepartmentName.Trim().ToLower().Equals("pathology"));

         var rad = db.Departments.Any(e => e.Id == test.DepartmentRadPath &&
          e.DepartmentName.Trim().ToLower().Equals("radiology"));


         var serviceGroupId = db.ServiceGroups.FirstOrDefault(
                        e => e.ServiceGroupName.ToLower().Trim().Equals("labs") ||
                        e.ServiceGroupName.ToLower().Trim().Equals("lab") ||
                        e.ServiceGroupName.ToLower().Trim().Equals("pathology")).Id;

                if (rad)
                {
                    serviceGroupId = db.ServiceGroups.FirstOrDefault(
                        e => e.ServiceGroupName.ToLower().Trim().Equals("radiology") ||
                        e.ServiceGroupName.ToLower().Trim().Equals("xray") ||
                        e.ServiceGroupName.ToLower().Trim().Equals("x-ray")).Id;
                }

         Service service = new Service();

            service.ServiceGroupId = serviceGroupId;

            service.DepartmentId = test.DepartmentRadPath;
            service.ServiceName = test.Test;
            service.CashPrice = 0;
            service.IsLAB = path;
            service.IsXRAY = rad;
            service.LabId = test.Id;
            service.UserId = id;
            
            service.BranchId = (int)Session["UserBranchId"] ;
            service.DateAdded = DateTime.Now;
            service.TestProfileCode = test.Test_Code;

            db.Services.Add(service);
            db.SaveChanges();

            return true;
        }


        public ActionResult Employee()
        {
            //new EmployeesData { Employee = null, Employess = db.Employees.OrderByDescending(e => e.Id).ToList(), DesignationsList = db.Designations.ToList() });


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
        public ActionResult Employee(CaresoftHMISDataAccess.Employee employee)
        {
            employee.DateAdded = DateTime.Now.Date;

            try
            {
                if (ModelState.IsValid || ModelState != null)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    //var emp = db.Employees.Include().OrderByDescending(p => p.Id).ToList();
                    var emp = db.Employees.Include(p => p.Department)
                        .Include(x => x.Designation).OrderByDescending(p => p.Id).ToList();
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


        public ActionResult FreeForm()
        {
            return View(db.FreeForms.ToList());
        }

        [HttpPost]
        public ActionResult CreateFreeForm(string FreeFormName)
        {
            FreeForm freeForm = new FreeForm();
            freeForm.UserId = int.Parse(Session["UserId"].ToString());
            
            freeForm.BranchId = (int)Session["UserBranchId"] ;
            freeForm.TimeAdded = DateTime.Now;
            freeForm.FreeFormName = FreeFormName;
            db.FreeForms.Add(freeForm);
            db.SaveChanges();


            var freeformNames = "--Select Form";
            var freeForms = db.FreeForms.ToList();
            foreach(var form in freeForms)
            {
                freeformNames += "<option value=" + form.Id + ">" + form.FreeFormName + "</option>";
            }
            return Content(freeformNames);

        }

        public ActionResult DiseaseMaster()
        {
            ViewBag.Diseases = db.Diseases.OrderByDescending(e=>e.Id).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateDisease(Disease data)
        {
            data.UserId = int.Parse(Session["UserId"].ToString());
            
            data.BranchId = (int)Session["UserBranchId"] ;
            data.TimeAdded = DateTime.Now;

            db.Diseases.Add(data);
            db.SaveChanges();

            return RedirectToAction("DiseaseMaster");
        }


        public ActionResult PatientMainCategory()
        {
            return View(db.CompanyTypes.ToList());
        }


        [HttpPost]
        public ActionResult PatientMainCategory(CompanyType type)
        {
            if (db.CompanyTypes.FirstOrDefault(e => e.CompanyTypeName.ToLower().Trim().Equals(type.CompanyTypeName.ToLower().Trim())) == null)
            {
                type.DateAdded = DateTime.Now;
                db.CompanyTypes.Add(type);
                db.SaveChanges();
            }
            
            return RedirectToAction("PatientMainCategory");
        }

        public ActionResult PatientCategory()
        {
            if (db.CompanyTypes.ToList().Count() < 1)
            {
                return RedirectToAction("PatientMainCategory");
            }

            var general = db.CompanyTypes.FirstOrDefault(e => e.CompanyTypeName.ToLower().Trim().Equals("general"));

            if (general == null)
            {
                return RedirectToAction("PatientMainCategory");
            }
            return View(general);
        }

        [HttpPost]
        public ActionResult PatientCategory(Company type)
        {
            if (db.CompanyTypes.FirstOrDefault(e => e.CompanyTypeName.ToLower().Trim().Equals(type.CompanyName.ToLower().Trim())) == null)
            {
                type.DateAdded = DateTime.Now;
               
                db.Companies.Add(type);
                db.SaveChanges();
            }

            return RedirectToAction("PatientCategory");
        }
        public ActionResult Initial()
        {
            return View(db.Salutations.ToList());
        }

        [HttpPost]
        public ActionResult Initial(FormCollection collection)
        {
            
            var salutation = new Salutation();

            if (!string.IsNullOrEmpty(collection["Patient"]))
            {
                string checkpat = collection["Patient"];
                
                salutation.Patient = true;
            }
            if (!string.IsNullOrEmpty(collection["Employee"]))
            {
                string checkEmp = collection["checkResp"];
                
                salutation.Employee = true;
            }

            salutation.SalutationName = collection["SalutationName"];
            salutation.Gender = collection["Gender"];

            if (db.Salutations.FirstOrDefault(e => e.SalutationName.ToLower().Trim().Equals(salutation.SalutationName.Trim().ToLower())) == null)
            {
                db.Salutations.Add(salutation);
                db.SaveChanges();
            }
            if (salutation.Patient)
            {
                ViewBag.Patient = true;
            }
            if (salutation.Patient)
            {
                ViewBag.EMployee = true;
            }

            ViewBag.SelectedGender = salutation.Gender;
            
            return View(db.Salutations.ToList());
        }


        public ActionResult DeletePatientMainCategory(int id)
        {
            CompanyType c = db.CompanyTypes.Find(id);
            db.CompanyTypes.Remove(c);
            db.SaveChanges();
            return RedirectToAction("PatientMainCategory");
        }

        public ActionResult DeletePatientCategory(int id)
        {
            Company c = db.Companies.Find(id);
            db.Companies.Remove(c);
            db.SaveChanges();
            return RedirectToAction("PatientCategory");
        }

        public ActionResult Immunizations()
        {
            //***Seed immunizatioType table***\\
            string[] immunizationTypeDropDown = new string[] { "Child", "Adult"};
            if(db.ImmunizationTypes.ToList().Count()< immunizationTypeDropDown.Count())
            {
                foreach(var it in immunizationTypeDropDown)
                {
                    if(db.ImmunizationTypes.FirstOrDefault(e=>e.ImmunizationTypeName == it) == null)
                    {
                        var imtype = new ImmunizationType();
                        imtype.ImmunizationTypeName = it;
                        db.ImmunizationTypes.Add(imtype);
                    }

                    db.SaveChanges();
                }
                
                
            }
            //*** End Seed immunizatioType table***\\


            ViewBag.ImmunizationCategories = db.ImmunizationCategories.ToList();


            return View(db.ImmunizationMasters.ToList());
        }

        public ActionResult ImmunizationCategories()
        {
            return View(db.ImmunizationCategories.OrderByDescending(e => e.Id).ToList());
        }


        [HttpPost]
        public ActionResult ImmunizationCategories(string ImmunizationCategoryName)
        {
            if (db.ImmunizationCategories.FirstOrDefault(e => e.ImmunizationCategoryName.Trim().ToLower() == ImmunizationCategoryName.Trim().ToLower()) != null)
            {
                ViewBag.Status = "warning";
                ViewBag.Message = "Warning! There exists another category with exact name!";
                return View(db.ImmunizationCategories.OrderByDescending(e => e.Id).ToList());
            }
            var immCat = new ImmunizationCategory
            {
                ImmunizationCategoryName = ImmunizationCategoryName,
                UserId = (int)Session["UserId"],
                BrachId = 1,
                DateAdded = DateTime.Now
            };
            db.ImmunizationCategories.Add(immCat);
            db.SaveChanges();
            ViewBag.Status = "success";
            ViewBag.Message = "Success! Category Addedd Successfully!";
            return View(db.ImmunizationCategories.OrderByDescending(e=>e.Id).ToList());
        }

        [HttpPost]
        public ActionResult SaveImmunizationMaster(ImmunizationMaster data)
        {
            data.UserId = int.Parse(Session["UserId"].ToString());
            
            data.BranchId = (int)Session["UserBranchId"] ;
            data.DateAdded = DateTime.Now;
            db.ImmunizationMasters.Add(data);
            db.SaveChanges();
            return PartialView("ImmunizationMasterList", db.ImmunizationMasters.ToList());
        }

        public ActionResult DeleteImmunization(int id)
        {
            var entry = db.ImmunizationMasters.Find(id);
            if (db.ImmunizationAdmins.Where(e => e.ImmunizationMasterId == id).Count() > 0)
            {
                return Json(new {
                    Message = "Unable to delete " + entry.ImmunizationName + ". It has already been administered to some patients.",
                    Status = "warning"}, JsonRequestBehavior.AllowGet);

            }
            db.ImmunizationMasters.Remove(entry);
            db.SaveChanges();
            return Json(new
            {
                Message = "Entry deleted successfully!",
                Status = "success"
            },
                JsonRequestBehavior.AllowGet);
        }
    }
}