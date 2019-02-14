using Caresoft2._0.Areas.Radiology.Models;
using CaresoftHMISDataAccess;
using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Radiology.Controllers
{
    [Auth]
    public class ReportsController : Controller
    {
        private CareSoftLabsEntities db = new CareSoftLabsEntities();
        private CaresoftHMISEntities db_main = new CaresoftHMISEntities();
        int main_department_id = new CaresoftHMISEntities().Departments.FirstOrDefault(d => d.DepartmentName.Equals("Radiology")).Id;


        // GET: Radiology/Reports
        public ActionResult TestTurnAroundTime(DateTime? StatDate, DateTime? EndDate)
        {
            if (StatDate == null || EndDate == null)
            {
                StatDate = DateTime.Now.Date; EndDate = DateTime.Now.Date;
            }
            var ttat = db.WorkOrderTests.Where(e => (e.CreatedUtc >= StatDate && e.CreatedUtc <= EndDate) && 
            e.TimeAuthorized != null && e.DepartmentRadPath == main_department_id);
            ViewBag.OPDReg = db_main.OpdRegisters.Where(o => o.Id > 0).ToList();
                                          
            ttat.Any(f => f.WorkOrder1.OPDNo > 0);

            ViewBag.StartDate = StatDate;
            ViewBag.EndDate = EndDate;

            return View(ttat.OrderBy(e => e.CreatedUtc));
        }

        public ActionResult NormalRangeReport()
        {
          
            ViewBag.Department = db.Departments.Where(e => e.DepartmentRadPath == main_department_id);
            ViewBag.Tests = db.LabTests.Where(e => e.DepartmentRadPath == main_department_id && e.Department == 0);

            ViewBag.ShowTopMenu = true;
            return View();
        }

        public ActionResult NormalRangeTestList(int? Dep)
        {
            var list = db.LabTests.Where(e => e.DepartmentRadPath == main_department_id && e.Department == Dep)
                .Select( x =>new {
                    Id = x.Id,
                    TestName = x.Test
                });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NormalRangeReportList(int Department, int Test)
        {
            ViewBag.IsPartial = true;
            ViewBag.Department = db.Departments.Find(Department).Department1;
            var normal_range = db.NormalValues.Where(e => e.DepartmentRadPath == main_department_id && e.LabTest.Department == Department);

            if (Test >= 0)
            {
                normal_range = normal_range.Where( e => e.Test == Test);
            }
            else
            {
            }
            return PartialView(normal_range);

        }

        public ActionResult TestDoneReport()
        {
            ViewBag.Department = db.Departments.Where(e => e.DepartmentRadPath == main_department_id);
            ViewBag.Tests = db.LabTests.Where(e => e.DepartmentRadPath == main_department_id && e.Department == 0);
            return View();
        }

        public ActionResult TestDoneReportList(DateTime StatDate, DateTime EndDate, int Department, int Test)
        {
            EndDate.Date.AddDays(1);
            var test = db.WorkOrderTests.Where(e => e.DepartmentRadPath == main_department_id && e.LabTest.Department == Department && (e.CreatedUtc >= StatDate && e.CreatedUtc <= EndDate.Date));

            if (Test >= 0)
            {
                test = test.Where(e => e.Test == Test);
            }
            else
            {
            }

            return PartialView(test);
        }

        public ActionResult TestDoneReportPrintOut(DateTime StatDate, DateTime EndDate, int Department, int Test)
        {
            EndDate.Date.AddDays(1);
            var test = db.WorkOrderTests.Where(e => e.DepartmentRadPath == main_department_id && e.LabTest.Department == Department && (e.CreatedUtc >= StatDate && e.CreatedUtc <= EndDate.Date));

            if (Test >= 0)
            {
                test = test.Where(e => e.Test == Test);
            }
            else
            {
            }

            return PartialView(test);
        }


        public ActionResult AuthorisedPatient()
        {
            return View();
        }

        public ActionResult AuthorisedPatientList(DateTime StatDate, DateTime EndDate)
        {
            EndDate = EndDate.Date.AddDays(1);
            var workOrders = db.WorkOrders.Where(e => e.DepartmentRadPath == main_department_id && e.WorkOrderTests.All(f => f.Status1.StatusValue == "Authorized")
            && (e.CreatedUtc >= StatDate && e.CreatedUtc <= EndDate.Date));

            return PartialView(workOrders);
        }

        public ActionResult AuthorisedPatientPrintOut(DateTime StatDate, DateTime EndDate)
        {
            EndDate.Date.AddDays(1);
            var workOrders = db.WorkOrders.Where(e => e.DepartmentRadPath == main_department_id && e.WorkOrderTests.All(f => f.Status1.StatusValue == "Authorized")
            && (e.CreatedUtc >= StatDate && e.CreatedUtc <= EndDate.Date));

            return PartialView(workOrders);
        }

        public ActionResult SearchPatient()
        {
            ViewBag.Department = db.Departments.Where(e => e.DepartmentRadPath == main_department_id).ToList();
            ViewBag.PatientCategory = db_main.CompanyTypes.ToList();
            ViewBag.Company = db_main.Companies.ToList();
            ViewBag.Tarrif = db_main.Tariffs.ToList();

            return View();
        }

        //Todo : Filter Data
        public ActionResult SearchPatientList(SearchPatientModel searchPatient)
        {
            searchPatient.ToDate = searchPatient.ToDate.AddDays(1);

            var patients = db.SearchPatientViews.Where( e => e.DepartmentRadPath == main_department_id && 
            (e.CreatedUtc >= searchPatient.FromDate && e.CreatedUtc <= searchPatient.ToDate));

            if (searchPatient.PatientCategory != null && searchPatient.PatientCategory > 0)
            {
                patients = patients.Where(e => e.CompanyTypeId == searchPatient.PatientCategory);
            }
          
            if (searchPatient.Company != null && searchPatient.Company > 0)
            {
                patients = patients.Where(e => e.CompanyID == searchPatient.Company);
            }

            if (searchPatient.Tarrif != null && searchPatient.Tarrif > 0)
            {
                patients = patients.Where(e => e.TarrifId == searchPatient.Tarrif);
            }


            if (searchPatient.patient_type == "All")
            {
            }
            else if (searchPatient.patient_type == "Direct")
            {
                patients = patients.Where(e => e.OPDNo == null || e.OPDType == "");
            }
            else
            {
                patients = patients.Where(e => e.OPDType.Trim() == searchPatient.patient_type.Trim());
            }

            if (searchPatient.Department != null && searchPatient.Department > 0)
            {
                patients = patients.Where(e => e.TestDepartmentId == searchPatient.Department);
            }

            if (searchPatient.Title != null && searchPatient.Title > 0)
            {
                patients = patients.Where(e => e.LabTestId == searchPatient.Title);
            }

            if (searchPatient.DoctorName != null && searchPatient.DoctorName.Length > 0)
            {
                patients = patients.Where(e => e.DoctorUsername.Contains(searchPatient.DoctorName));
            }

            if (searchPatient.PatientRegNo != null && searchPatient.PatientRegNo.Length > 0 && searchPatient.PatientName != null && searchPatient.PatientName.Length > 0)
            {
                patients = patients.Where(e => e.PatientRegNumber == (searchPatient.PatientRegNo));
            }

            return PartialView(patients);
        }

        public ActionResult getAllTestInDepartments(int? id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var labtests = db.LabTests.ToList();

            if (id != null || id > 0)
            {
                labtests = labtests.Where(e => e.Department == id).ToList();
            }
            return Json(labtests, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchPatientName(string search)
        {
            var search2 = Regex.Replace(search, @"\s+", "");
            var patientOpd = db_main.Patients.Where(e => e.RegNumber.Contains(search) || e.FName.Contains(search) || e.MName.Contains(search) ||
            e.LName.Contains(search) || (search2.Contains(e.FName) && search2.Contains(e.LName))
            || (search2.Contains(e.FName) && search2.Contains(e.MName)) || (search2.Contains(e.MName)
            && search2.Contains(e.LName))).Select(d => new
            {
                Name = d.FName + " " + d.MName + " " + " " + d.LName + " ",
                RegNumber = d.RegNumber

            }).Take(20).ToList();
            return Json(patientOpd, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchDoctorName(string search)
        {
            var patientOpd = db_main.Users.Where(o => o.Username.Contains(search)).Select(d => new
            {
                UserName = d.Username
            }).ToList();
            return Json(patientOpd, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllTarrifs(int search)
        {
            var patientOpd = db_main.Tariffs.Select(d => new
            {
                Id = d.Id,
                TariffName = d.TariffName
            }).ToList();

            if (search > 0)
            {
                patientOpd = db_main.Tariffs.Where(o => o.CompanyId == (search)).Select(d => new
                {
                    Id = d.Id,
                    TariffName = d.TariffName
                }).ToList();
            }

            return Json(patientOpd, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllCompanies(int search)
        {
            var patientOpd = db_main.Companies.Select(d => new
            {
                Id = d.Id,
                CompanyName = d.CompanyName
            }).ToList();

            if (search > 0)
            {
                patientOpd = db_main.Companies.Where(o => o.CompanyTypeId == (search)).Select(d => new
                {
                    Id = d.Id,
                    CompanyName = d.CompanyName
                }).ToList();
            }
            return Json(patientOpd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PatientHistory(int? id)
        {
            var labtests = db_main.Patients.FirstOrDefault(e => e.OpdRegisters.Any(f => f.Id == id));
         
            return PartialView(labtests);
        }


    }
}