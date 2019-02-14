using CaresoftHMISDataAccess;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Theatre.Controllers
{
    public class RegistrationController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        int pageSize = 10;
        private int dep_Id = new CaresoftHMISEntities().Departments.FirstOrDefault(e => e.DepartmentName == "Theatre").Id;
        // GET: Theatre/Registration
        public ActionResult Index(int? page, string queuetype, DateTime? FromDate, DateTime? ToDate)
        {
            if (FromDate == null || ToDate == null)
            {
                FromDate = DateTime.Today;
                ToDate = DateTime.Today;

            }

            ViewBag.RegStatus = queuetype;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;


            ToDate = (DateTime)ToDate.Value.Date.AddDays(1);


            int pageNumber = (page ?? 1);

            List<int> exists = db.TheatrePatientBioDatas.Select(e => e.PatientOPDIPD).ToList();

            var opdReg = db.OpdRegisters.OrderBy(f => f.Id).Where(e => e.Date >= FromDate && e.Date < ToDate &&
            !e.TheatrePatientBioDatas.Any(f => f.PatientOPDIPD == e.Id && f.TheatreOperationPersonels.Any())
            && ((e.BillServices.Any(g => g.DepartmentId == dep_Id) || e.PatientReferals
            .Any(x => x.DateAdded >= FromDate && x.DateAdded < ToDate))) || e.Department.Contains("Theatre"));

            if (queuetype == "Registered")
            {
                var asdf = db.OpdRegisters.OrderBy(f => f.Id).Where(e =>
                e.TheatrePatientBioDatas.Any(f => f.PatientOPDIPD == e.Id && f.CreatedUTC >= FromDate && f.CreatedUTC < ToDate)
                           ).ToList();

                opdReg = db.OpdRegisters.OrderBy(f => f.Id).Where(e =>
                e.TheatrePatientBioDatas.Any(f => f.PatientOPDIPD == e.Id && f.CreatedUTC >= FromDate && f.CreatedUTC < ToDate)
                            );
            }
            return View(opdReg.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Register(int? id)
        {
            ViewBag.TheatreDesignations = db.TheatreDesignations.ToList();
            ViewBag.TheatreDepartment = db.TheatreDepartments.ToList();

            if (ViewBag.TheatreDesignations.Count == 0 )
            {
                db.TheatreDesignations.Add(
                    new TheatreDesignation()
                    {
                        DesignationName = "Surgeon",
                    });
                db.TheatreDesignations.Add(
                    new TheatreDesignation()
                    {
                        DesignationName = "Ass Surgeon",
                    });

                db.TheatreDesignations.Add(
                    new TheatreDesignation()
                    {
                        DesignationName = "Scrub Nurse",
                    });

                db.TheatreDesignations.Add(
                    new TheatreDesignation()
                    {
                        DesignationName = "Anaesthesist",
                    });
                db.SaveChanges();
            }

            if (ViewBag.TheatreDepartment.Count == 0)
            {
                db.TheatreDepartments.Add(
                    new TheatreDepartment()
                    {
                        DepartmentName = "General"
                    });
                db.SaveChanges();

            }

            if (db.TheatrePatientBioDatas.Any(e => e.PatientOPDIPD == id))
            {
                ViewBag.Opd = id;

                return PartialView(db.TheatrePatientBioDatas.FirstOrDefault(e => e.PatientOPDIPD == id));
            }

            var opd = db.OpdRegisters.Find(id);
            var docc = db.Employees.FirstOrDefault(e => (e.FName + " " + e.OtherName) == opd.ConsultantDoctor);
            var doc = 0;

            if (docc == null)
            {
                doc = 1;
            }
            else
            {
                doc = docc.Id;
            }
            var data = new TheatrePatientBioData()
            {
                PatientOPDIPD = opd.Id,
                PatientName = opd.Patient.FName + " " + opd.Patient.MName + " " + opd.Patient.LName,
                InternalRefferal = 0,
                HIV = "",
                AppointmentDate = DateTime.Now,
                DepartmentId = 0,
                CreatedUTC = DateTime.Now
            };
            ViewBag.Opd = id;

            var referals = opd.PatientReferals.OrderByDescending(e => e.Id);
            ViewBag.InternalRefferal = "N/A";

            if (opd.PatientReferals.Any(e=> e.ReferalType.Contains("Internal") ) && referals.FirstOrDefault().DepartmentId == dep_Id)
            {
                try{
                    if (opd.PatientReferals.Count() > 1)
                    {
                        data.InternalRefferal = (int)referals.Skip(1).FirstOrDefault().ReferedToId;
                        ViewBag.InternalRefferal = referals.Skip(1).FirstOrDefault().Department.DepartmentName;
                    }
                    else
                    {
                        data.InternalRefferal = db.Departments.First(e => e.DepartmentName.Contains("OutPatient")).Id;
                        ViewBag.InternalRefferal = db.Departments.First(e => e.DepartmentName.Contains("OutPatient")).DepartmentName;
                    }

                }catch(Exception e)
                {

                }
            }

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int Register(TheatrePatientBioData theatrePatientBio)
        {
            if (ModelState.IsValid)
            {
                theatrePatientBio.CreatedUTC = DateTime.Now;

                if (theatrePatientBio.Id != null && theatrePatientBio.Id > 0)
                {
                    var pat = db.TheatrePatientBioDatas.FirstOrDefault(e => e.PatientOPDIPD == theatrePatientBio.PatientOPDIPD);
                    pat.HIV = theatrePatientBio.HIV;
                    pat.DepartmentId = theatrePatientBio.DepartmentId;
                    pat.AppointmentDate = theatrePatientBio.AppointmentDate;
                    pat.RegStatus = theatrePatientBio.RegStatus;
                    pat.UserId = (int)Session["UserId"];

                    db.Entry(pat).State = EntityState.Modified;

                    db.SaveChanges();
                    return pat.Id;
                }
                else
                {
                    theatrePatientBio.UserId = (int)Session["UserId"];
                    db.TheatrePatientBioDatas.Add(theatrePatientBio);
                }

                db.SaveChanges();
                return theatrePatientBio.Id;

            }

            return 0;
        }

        public int getDepartmentPatientBooked(int id)
        {
            return db.TheatrePatientBioDatas.Where(e => e.DepartmentId == id).ToList().Count;
        }

        public ActionResult PatientPreparation()
        {
            ViewBag.TheatreDesignations = db.TheatreDesignations.ToList();
            return View();
        }

        public class TempObj
        {
            public string __RequestVerificationToken { get; set; }
            public List<TheatreOperationPersonel> theatreOperationPersonel { get; set; }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public int PatientPreparation(TempObj tempObj)
        {
            foreach (var data in tempObj.theatreOperationPersonel)
            {
                data.User = (int)Session["UserId"];
                db.TheatreOperationPersonels.Add(data);
            }

            return db.SaveChanges();
        }

        public ActionResult GetDoctors(string name)
        {
            var sanitizedname = Regex.Replace(name, @"\s+", "");

            return Json(db.Users.Where(e => e.Employee.DepartmentId == dep_Id &&
             (e.Employee.FName.Contains(name) || e.Employee.OtherName.Contains(name) || (sanitizedname.Contains(e.Employee.FName) ||
             sanitizedname.Contains(e.Employee.OtherName)))).Select(x => new {
                 x.Id,
                 Name = x.Employee.FName + "" + x.Employee.OtherName,
                 UserName = x.Username
             }).ToList(), JsonRequestBehavior.AllowGet);
        }

        
    }
}