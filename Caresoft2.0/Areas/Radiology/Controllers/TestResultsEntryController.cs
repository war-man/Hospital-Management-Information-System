using Caresoft2._0.Areas.Radiology.Models;
using CaresoftHMISDataAccess;
using LabsDataAccess;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Radiology.Controllers
{
    [Auth]

    public class TestResultsEntryController : Controller
    {
        private CareSoftLabsEntities db = new CareSoftLabsEntities();
        private CaresoftHMISEntities db2 = new CaresoftHMISEntities();
        int main_department_id = new CaresoftHMISEntities().Departments.FirstOrDefault(d => d.DepartmentName.Equals("Radiology")).Id;

        // GET: Radiology/TestResultsEntry
        public ActionResult Index()
        {
            ViewBag.FinancialYear = new SelectList(db.Profiles.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Group_Name");
            ViewBag.Departments = db.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            return View();
        }

        public ActionResult GetTests(int id)
        {
            var tests = db.Departments.Find(id).LabTests.ToList();
            var testString = "";
            foreach (var test in tests)
            {
                testString += "<li class=' " + test.Department + " TestsCheckbox" + "'><input type='checkbox' value='" + test.Id  + "' department='" + test.Department + "' id='" + test.Id + "'> " + test.Test + "</li>";
            }
            return Content(testString);
        }

        public async Task<ActionResult> AccesionList(TestEntryPatients filter, int? page)
        {
            ViewBag.main_department_id = main_department_id;
            ViewBag.ShowInQueueBillNotPaid = new LabsDataAccess.CareSoftLabsEntities().PathKeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ShowInQueueBillNotPaid")).Value;

            var Todate = filter.ToDate.Date.AddDays(1);

            var workOrders = db.WorkOrders.Where(e => (e.CreatedUtc >= filter.FromDate && e.CreatedUtc <= Todate) && e.ShowInSpecimentResultEnty == true && e.WorkOrderTests.Any(f => f.LabTest.DepartmentRadPath == main_department_id));

            if (filter.PatientId != null && filter.PatientId.Length > 0)
            {
                var OPDReg = db2.OpdRegisters.Where(o => o.Patient.RegNumber == filter.PatientId).ToList();
                List<int> opdIDs = new List<int>();
                foreach (var opd in OPDReg)
                {
                    opdIDs.Add(opd.Id);
                }
                workOrders = workOrders.Where(e => (opdIDs.Contains((int)e.OPDNo)));
            }

            ViewBag.PatientId = filter.PatientId;

            if (filter.BarCode != null && filter.BarCode.Length > 0)
            {
                workOrders = workOrders.Where(e => (e.WorkOrderTests.Any(f => f.BarCode == filter.BarCode)));
            }

            if (filter.LabNo != null && filter.LabNo > 0)
            {
                workOrders = workOrders.Where(e => e.Id == (filter.LabNo));

            }

            if (filter.VialID != null && filter.VialID.Length > 0)
            {
                workOrders = workOrders;

            }

            if (filter.PSCNo != null && filter.PSCNo.Length > 0)
            {
                workOrders = workOrders;

            }

            if (filter.PathNo != null && filter.PathNo.Length > 0)
            {
                workOrders = workOrders.Where(e => e.PathNo.Equals(filter.PathNo));

            }


             var departmentcodes = new List<string>();

            if (filter.DepartmentCode != null && filter.DepartmentCode.Length > 0)
            {
                departmentcodes = filter.DepartmentCode.Split(',').ToList();
            }
            if (departmentcodes.Count > 0)
            {
                workOrders = workOrders.Where(e => e.WorkOrderTests.Any(f => departmentcodes.Contains(f.LabTest.Department.ToString())) == true);
            }

            var testcodes = new List<string>();

            if (filter.TestCode != null && filter.TestCode.Length > 0)
            {
                testcodes = filter.TestCode.Split(',').ToList();
            }

            if (testcodes.Count > 0)
            {
                workOrders = workOrders.Where(e => e.WorkOrderTests.Any(f => testcodes.Contains(f.LabTest.Id.ToString())) == true);
            }


            ViewBag.Dep = departmentcodes.Count;
            ViewBag.testcodes = testcodes.Count;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (filter.PatientType.Equals("All"))
            {
                return PartialView(workOrders.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return PartialView(workOrders.Where(e => e.OPDType.Equals(filter.PatientType)).OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize));
            }

        }

        public JsonResult searchPatientName(string search)
        {
            var search2 = Regex.Replace(search, @"\s+", "");
            var patientOpd = db2.Patients.Where(e => e.RegNumber.Contains(search) || e.RegNumber.Contains(search) || e.FName.Contains(search) || e.MName.Contains(search) ||
            e.LName.Contains(search) || (search2.Contains(e.FName) && search2.Contains(e.LName))
            || (search2.Contains(e.FName) && search2.Contains(e.MName)) || (search2.Contains(e.MName)
            && search2.Contains(e.LName))).Select(d => new
            {
                Name = d.FName + " " + d.MName + " " + " " + d.LName + " ",
                RegNumber = d.RegNumber

            }).Take(20).ToList();
            return Json(patientOpd, JsonRequestBehavior.AllowGet);
        }
        
        public async Task<ActionResult> LabTestsList(int? id)
        {
            ViewBag.main_department_id = main_department_id;
            ViewBag.ShowInQueueBillNotPaid = new LabsDataAccess.CareSoftLabsEntities().PathKeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ShowInQueueBillNotPaid")).Value;

            var workOrdersTests = db.WorkOrderTests.Where(e => e.WorkOrder == id && e.LabTest.DepartmentRadPath == main_department_id).OrderBy(e => e.WorkOrder);
            return PartialView(await workOrdersTests.ToListAsync());
        }

        public ActionResult ResultsEntry(int? id)
        {
            ViewBag.main_department_id = main_department_id;
            ViewBag.ShowInQueueBillNotPaid = new LabsDataAccess.CareSoftLabsEntities().PathKeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ShowInQueueBillNotPaid")).Value;

            ViewBag.LabNumber = id;
            var workOrder = db.WorkOrders.Find(id);
            ViewBag.workOrder = workOrder;
            ViewBag.OPD = workOrder.OPDNo;

            foreach (var wot in workOrder.WorkOrderTests)
            {
                if (wot.LabTest.Parameter)
                {
                    foreach (var ltp in wot.LabTest.Parameters)
                    {
                        var wtp = db.WorkOrderTestParameters.FirstOrDefault(e => e.WorkOrderTest1.WorkOrder == wot.WorkOrder1.Id &&
                    e.Parameter == ltp.Id);

                        if (wtp == null)
                        {
                            var wotp = new WorkOrderTestParameter();

                            wotp.WorkOrderTest = wot.Id;
                            wotp.Parameter = ltp.Id;
                            wotp.Results = "";
                            wotp.CreatedUtc = DateTime.Now;

                            db.WorkOrderTestParameters.Add(wotp);
                        }
                    }
                    db.SaveChanges();


                }
            }
            return PartialView();
        }

        [HttpPost]
        public int UploadImage(int wot, int wotp, HttpPostedFileBase file)
        {
            if (file != null && wot > 0)
            {
                var name = wot + "-" + wotp + Path.GetExtension(file.FileName);
                if (wotp > 0)
                {
                    var wtp = db.WorkOrderTestParameters.Find(wotp);
                    wtp.Image = name;
                }
                else
                {
                    var wt = db.WorkOrderTests.Find(wot);
                    wt.Image = name;
                }
                db.SaveChanges();

                return UploadSignature(file, name);

                
            }

            return 0;
        }

        private int UploadSignature(HttpPostedFileBase Image, string filename)
        {
            if (Image != null)
            {
                string path = null;
                string FileName = null;

                string directory = "~/Areas/Radiology/Images/Results";
                bool exists = System.IO.Directory.Exists(Server.MapPath("directory"));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(directory));

                FileName = filename;
                path = Path.Combine(Server.MapPath(directory), FileName);
                Image.SaveAs(path);
                return 1;


            }
            return 0;

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public int ResultsEntry(FormCollection testEntry)
        {
            if (ModelState.IsValid)
            {
                var model = new Dictionary<string, string>();

                var DoctorsList = new Dictionary<string, int>();

                var wo = db.WorkOrders.Find(Convert.ToInt32(testEntry.Get("WorkOrder")));

                //if (wo.Accession_Status.Equals(db.Status.FirstOrDefault(e => e.StatusValue.Equals("Authorized")).Id))
                //{
                //    return 2;
                //}

                for (int i = 0; i < testEntry.Count; i++)
                {

                    if (testEntry.GetKey(i).StartsWith("Doctor"))
                    {
                        string departmentName = testEntry.GetKey(i).Replace("Doctor", "");
                        int doctorId = Convert.ToInt32(testEntry.Get(i).Trim());

                        DoctorsList.Add(departmentName, doctorId);

                    }
                  

                    if (testEntry.GetKey(i).StartsWith("Test"))
                    {
                        int id = Convert.ToInt32(testEntry.GetKey(i).Replace("Test", ""));
                        var wot = db.WorkOrderTests.Find(id);
                       
                        //if (wot.Status != db.Status.FirstOrDefault(e => e.StatusValue.Equals("Authorized")).Id)
                          {
                            var res = testEntry.Get(i).Trim();

                            if (wot.Results != null && (wot.Results.Trim() != "" || !wot.Results.Trim().Equals(res)))
                            {
                                wot.Doctor = DoctorsList.SingleOrDefault(e => e.Key == wot.LabTest.Department1.Department1).Value;

                            }

                            wot.Results = testEntry.Get(i);
                            db.Entry(wot).State = EntityState.Modified;

                           }

                    }
                    else if(testEntry.GetKey(i).StartsWith("Par"))
                    {
                        var tp = Convert.ToInt32(testEntry.GetKey(i).Replace("Par", ""));

                        var wotp = db.WorkOrderTestParameters.Find(tp);
                        //if (wotp != null)
                        //{
                        
                        //if (wotp.WorkOrderTest1.Status != db.Status.FirstOrDefault(e => e.StatusValue.Equals("Authorized")).Id)
                        {

                            if (wotp.Results.Trim() != null && (wotp.Results.Trim() != "" ||
                                !wotp.Results.Trim().Equals(testEntry.Get(i).Trim())))
                            {
                                wotp.WorkOrderTest1.Doctor = DoctorsList.SingleOrDefault(e => e.Key ==
                                wotp.WorkOrderTest1.LabTest.Department1.Department1).Value;

                            }

                            wotp.Results = testEntry.Get(i);
                            db.Entry(wotp).State = EntityState.Modified;

                        }



                        //}
                        //else
                        //{
                        //    wotp = new WorkOrderTestParameter() {
                        //        WorkOrderTest = 3,
                        //        Parameter = tp,
                        //        Results = testEntry.Get(i),
                        //        CreatedUtc = DateTime.Now
                        //    };

                        //    db.WorkOrderTestParameters.Add(wotp);
                        //}

                    }

                    if (testEntry.GetKey(i).StartsWith("Auth"))
                    {
                        string testIdtext = testEntry.GetKey(i).Replace("Auth", "");
                        int testId = Convert.ToInt32(testIdtext);

                        int res = AutorizeTestResults(testId);

                    }

                }
                db.SaveChanges();
                updateTestStatus(wo);
                return 1;

            }

            ViewBag.Accession_Status = new SelectList(db.Departments, "Id", "Department_Code");
            return 0;
        }

        private void updateTestStatus(WorkOrder workOrder)
        {
            var alltested = true;
            var tested = 0;
            foreach (var wo in workOrder.WorkOrderTests)
            {
             

                if (wo.LabTest.Parameter)
                {
                    foreach (var wop in wo.WorkOrderTestParameters)
                    {
                        if (wop.Results != null && wop.Results.Trim().Length > 0)
                        {
                            tested = +1;
                        }
                        else
                        {
                            alltested = false;
                        }
                    }
                }
                else
                {
                    if (wo.Results != null && wo.Results.Trim().Length > 0)
                    {
                        tested = +1;
                    }
                    else
                    {
                        alltested = false;
                    }
                }
            }
            var finalWo = db.WorkOrders.Find(workOrder.Id);

            if (alltested)
            {
                finalWo.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue.Equals("Tested")).Id;
            }
            else if(tested > 0)
            {
                finalWo.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue.Equals("Partial Tested")).Id;

            }
        }
        private void updateTestStatus2(WorkOrder workOrder)
        {
            var allAuthorized = true;
            var Authorized = 0;
            foreach (var wo in workOrder.WorkOrderTests)
            {

                if (wo.Status != db.Status.FirstOrDefault(e => e.StatusValue.Equals("Authorized")).Id)
                {
                    allAuthorized = false;
                }
                else
                {
                    Authorized = +1;
                }

                var finalWo = db.WorkOrders.Find(workOrder.Id);

                if (allAuthorized)
                {
                    finalWo.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue.Equals("Authorized")).Id;
                }
                else if (Authorized > 0)
                {
                    finalWo.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue.Equals("Partial Authorized")).Id;

                }
            }
        }

        public ActionResult AbnormalResults()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> AbnormalResults(FormCollection sampleCondition)
        {
            if (ModelState.IsValid)
            {

                var model = new Dictionary<string, string>();

                for (int i = 0; i < sampleCondition.Count; i++)
                {

                    if (int.TryParse((string)sampleCondition.GetKey(i), out int key))
                    {
                        var wot = db.WorkOrderTests.Find(key);
                        wot.Condition = sampleCondition.Get(i);
                        db.Entry(wot).State = EntityState.Modified;
                    }


                }
                await db.SaveChangesAsync();
                return 1;

            }

            ViewBag.Accession_Status = new SelectList(db.Departments, "Id", "Department_Code");
            return 0;
        }

        public ActionResult PanicResults()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> PanicResults(FormCollection sampleCondition)
        {
            if (ModelState.IsValid)
            {

                var model = new Dictionary<string, string>();

                for (int i = 0; i < sampleCondition.Count; i++)
                {

                    if (int.TryParse((string)sampleCondition.GetKey(i), out int key))
                    {
                        var wot = db.WorkOrderTests.Find(key);
                        wot.Condition = sampleCondition.Get(i);
                        db.Entry(wot).State = EntityState.Modified;
                    }


                }
                await db.SaveChangesAsync();
                return 1;

            }

            ViewBag.Accession_Status = new SelectList(db.Departments, "Id", "Department_Code");
            return 0;
        }
       
        public int AutorizeTestResults(int id)
        {
            if (ModelState.IsValid)
            {

                var test = db.WorkOrderTests.Find(id);

                test.TimeAuthorized = DateTime.Now;
                test.Status = db.Status.FirstOrDefault(e => e.StatusValue == "Authorized").Id;



                var WorkOrder = test.WorkOrder;
                var wots = db.WorkOrderTests.Where(e => e.WorkOrder == WorkOrder);

                var allauthorzed = true;

                foreach (var wot in wots)
                {
                    if (!wot.Status.Equals(db.Status.FirstOrDefault(e => e.StatusValue == "Authorized").Id))
                    {
                        allauthorzed = false;
                    }
                }

                if (allauthorzed)
                {
                    var wo = db.WorkOrders.Find(test.WorkOrder);
                    wo.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue == "Authorized").Id;
                }
                var res = db.SaveChanges();
                updateTestStatus2(test.WorkOrder1);

                return res; 

            }
            return -1;
        }
        public async Task<int> AutorizeWorkOrderResults(int id)
        {
            try
            {
                var workOrder = db.WorkOrders.Find(id);
                workOrder.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue == "Authorized").Id;
                return await db.SaveChangesAsync();
            }
            catch
            {
                return 2;
            }
        }
    }
}