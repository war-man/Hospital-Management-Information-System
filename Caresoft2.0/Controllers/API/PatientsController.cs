using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    public class PatientsController : ApiController
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        [Route("api/patients/search/")]
        public HttpResponseMessage PostSearch(SearchBill searchBill)
        {
            var SearchBy = searchBill.SearchBy;

            if (SearchBy == "PatientName")
            {
                var items = db.Patients.Where(e => e.FName.Contains(searchBill.SearchText)
               || e.MName.Contains(searchBill.SearchText) || e.LName.Contains(searchBill.SearchText));
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else if (SearchBy == "PatientNo")
            {
                var items = db.Patients.Where(e => e.Id.ToString().Contains(searchBill.SearchText));
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No data was found");
            }


        }

        // GET: api/Patients
        public IQueryable<Patient> GetPatients()
        {
            return db.Patients;
        }

        // GET: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> GetPatient(int id)
        {
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // PUT: api/Patients/5
        public HttpResponseMessage PutPatient(int id, [FromBody]Patient patient)
        {
            if (patient == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "no data sent");
            }
           patient.Id = id;

            db.Entry(patient).State = EntityState.Modified;

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, patient);
        }

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            patient.Timestamp = DateTime.Now;
           

            db.Patients.Add(patient);
            await db.SaveChangesAsync();

            //generate reg number and password if is a new registration
            if (patient.RegNumber.Equals(""))
            {
                var facilityInitial = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "FacilityInittials").Value;
                var year = DateTime.Now.ToString("yy");
                var prefix = "00";
                if (patient.Id > 9)
                {
                    prefix = "0";
                }
                else if (patient.Id > 99)
                {
                    prefix = "";
                }
                var branchId = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "BranchId").Value;
                var regNumber = facilityInitial + "/" + branchId + "/" + prefix + patient.Id + "/" + year;
                patient.RegNumber = regNumber;
                patient.Password = regNumber; //consider generating random password
            }
            else
            {
                patient.Password = patient.RegNumber;
            }
           
           
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.Id }, patient);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> DeletePatient(int id)
        {
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            await db.SaveChangesAsync();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.Id == id) > 0;
        }
    }
}