using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LabsDataAccess;
using Caresoft2._0.Areas.Radiology.Models;
using CaresoftHMISDataAccess;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;

namespace Caresoft2._0.Areas.Radiology.Controllers
{
    [Auth]

    public class AccessionController : Controller
    {
        private CareSoftLabsEntities db = new CareSoftLabsEntities();
        private CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        int main_department_id = new CaresoftHMISEntities().Departments.FirstOrDefault(d => d.DepartmentName.Equals("Radiology")).Id;

        // GET: Radiology/Assession
        public async Task<ActionResult> SampleCollectionList(WorkOrderFilter filter)
        {
            ViewBag.main_department_id = main_department_id;
            ViewBag.ShowInQueueBillNotPaid = new LabsDataAccess.CareSoftLabsEntities().PathKeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ShowInQueueBillNotPaid")).Value;

            ViewBag.Accession_Status = new SelectList(db.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");


            var Todate = filter.ToDate.Date.AddDays(1);
            var workOrders = db.WorkOrders.Where(e => e.ShowInSpecimentCollection == true && (e.CreatedUtc >= filter.FromDate && e.CreatedUtc <= Todate)
            && e.WorkOrderTests.Any(w => w.LabTest.DepartmentRadPath.Equals(main_department_id)));

            if (filter.PatientId > 0)
            {
                var patientopd = db2.OpdRegisters.FirstOrDefault(e => e.Id == filter.PatientId);
                if (patientopd != null)
                {
                    var patientOPDs = (patientopd.Patient.OpdRegisters.Select(g => g.Id)).ToList();

                    workOrders = db.WorkOrders.Where(e => e.WorkOrderTests.Any(w =>
                    w.LabTest.DepartmentRadPath.Equals(main_department_id))
                    && patientOPDs.Contains((int)e.OPDNo));
                }
                else
                {
                    workOrders.Where(e => e.Id == 0);
                }
            }
            else if (filter.BarCode != null && filter.BarCode.Length > 0)
            {
                //workOrders = db.WorkOrders.Where(e => e.WorkOrderTests.Equals(1));
            }


            if (filter.AccessionStatus.Equals("Done"))
            {
                workOrders = workOrders.Where(e => e.ShowInSpecimentResultEnty == true);
            }
            else if (filter.AccessionStatus.Equals("Pending"))
            {

                workOrders = workOrders.Where(e => (e.ShowInSpecimentResultEnty == null || e.ShowInSpecimentResultEnty == false) && e.ShowInSpecimentCollection == true);
            }
            else
            {
            }

            if (filter.PatientType.Trim().Equals("All"))
            {
                return PartialView(await workOrders.Where(e => e.WorkOrderTests.Any(w => 
                w.LabTest.DepartmentRadPath.Equals(main_department_id))).OrderByDescending(e => e.Id).OrderByDescending(e => e.Id).ToListAsync());
            }
            else
            {
                return PartialView(await workOrders.Where(e => e.OPDType.Equals(filter.PatientType)).OrderByDescending(e => e.Id).
                    OrderByDescending(e => e.Id).ToListAsync());

            }
        }

        public async Task<ActionResult> LabTestsList(int? id)
        {

            ViewBag.main_department_id = main_department_id;
            ViewBag.ShowInQueueBillNotPaid = new LabsDataAccess.CareSoftLabsEntities().PathKeyValuePairs.FirstOrDefault(e => e.Key_.Equals("ShowInQueueBillNotPaid")).Value;

            var workOrdersTests = db.WorkOrderTests.Where(e => e.WorkOrder1.Id == id && e.LabTest.DepartmentRadPath.Equals(main_department_id));
            return PartialView(await workOrdersTests.ToListAsync());
        }

        public JsonResult searchPatient(string search)
        {
            var search2 = Regex.Replace(search, @"\s+", "");

            var patientOpd = db2.Patients.Where(e => e.OpdRegisters.Any() && (e.RegNumber.Contains(search) || e.FName.Contains(search) || e.MName.Contains(search) ||
            e.LName.Contains(search) || (search2.Contains(e.FName) && search2.Contains(e.LName))
            || (search2.Contains(e.FName) && search2.Contains(e.MName)) || (search2.Contains(e.MName)
            && search2.Contains(e.LName)))).Select(d => new
            {
                Name = d.FName + " " + d.MName + " " + " " + d.LName + " ",
                OPD = d.OpdRegisters.FirstOrDefault().Id

            }).Take(20).ToList();
            return Json(patientOpd, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateSampleCollectionToTestResEntry(int? id)
        {
            var wo = db.WorkOrders.Find(id);
            if (wo.ShowInSpecimentCollection == true)
            {
                wo.ShowInSpecimentResultEnty = true;
            }

            // wo.ForEach(a => a.Accession_Status = db.Status.FirstOrDefault(e => e.StatusValue == "Registered").Id);

            await db.SaveChangesAsync();
            return RedirectToAction("LabTestsList", new { id = id });
        }

        // GET: Radiology/Assession/Create
        public ActionResult SampleCollection()
        {
            ViewBag.Accession_Status = new SelectList(db.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department_Code");
            return View();
        }

        // POST: Radiology/Assession/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SampleCollection([Bind(Include = "Id,Patient,Doctor,BarCode,Accession_Status,Financial_Year,CreatedUtc")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                db.WorkOrders.Add(workOrder);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Accession_Status = new SelectList(db.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department_Code");
            return View(workOrder);
        }


        public async Task<ActionResult> SampleCondition(int? id)
        {
            var workOrdersTests = db.WorkOrderTests.Where(e => e.DepartmentRadPath.Equals(main_department_id) && e.WorkOrder == id);
            return PartialView(await workOrdersTests.ToListAsync());
        }
        // POST: Radiology/Assession/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> SampleCondition(FormCollection sampleCondition)
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

            ViewBag.Accession_Status = new SelectList(db.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department_Code");
            return 0;
        }

        public ActionResult RejectedSamples()
        {
            ViewBag.FinancialYear = new SelectList(db.Profiles.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Group_Code");
            return View();
        }

        public async Task<ActionResult> RejectedSamplesList(WorkOrderFilter filter)
        {
            var Todate = filter.ToDate.Date.AddDays(1);
            var workOrdersTests = db.WorkOrderTests.Where(e => e.DepartmentRadPath.Equals(main_department_id) && (e.CreatedUtc >= filter.FromDate && e.CreatedUtc <= Todate) && e.WorkOrder1.Accession_Status == db.Status.FirstOrDefault(f => f.StatusValue.Equals("Rejected")).Id);

            if (filter.PatientId != null && filter.PatientId > 0)
            {
                workOrdersTests = workOrdersTests.Where(e => e.WorkOrder1.OPDNo == filter.PatientId);
            }
            if (filter.LabNo != null && filter.LabNo.Length > 0)
            {
                workOrdersTests = workOrdersTests.Where(e => e.WorkOrder1.LabNo == filter.LabNo);
            }
            if (filter.FinancialYear != null && filter.FinancialYear.Length > 0)
            {
                workOrdersTests = workOrdersTests.Where(e => e.WorkOrder1.Financial_Year == filter.FinancialYear);
            }


            return PartialView(await workOrdersTests.ToListAsync());
        }


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
