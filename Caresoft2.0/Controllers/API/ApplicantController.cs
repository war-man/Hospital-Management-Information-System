using Caresoft2._0.CustomData;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Caresoft2._0.Controllers.API
{
    public class ApplicantController : ApiController
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        [Route("api/applicant/search/")]
        public HttpResponseMessage PostSearch(SearchBill searchBill)
        {
            var SearchBy = searchBill.SearchBy;

            if (SearchBy == "ApplicantName")
            {
                var items = db.HRApplicants.Where(e => e.FirstName.Contains(searchBill.SearchText)
               || e.OtherNames.Contains(searchBill.SearchText) || e.Surname.Contains(searchBill.SearchText));
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else if (SearchBy == "Applicant Id")
            {
                var items = db.HRApplicants.Where(e => e.Id.ToString().Contains(searchBill.SearchText));
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No data was found");
            }


        }

        // GET: api/Patients
        public IQueryable<HRApplicant> GetApplicants()
        {
            return db.HRApplicants;
        }

        // GET: api/Patients/5
        [ResponseType(typeof(HRApplicant))]
        public async Task<IHttpActionResult> GetApplicant(int id)
        {
            HRApplicant applicant = await db.HRApplicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }

            return Ok(applicant);
        }

        // PUT: api/Patients/5
        public HttpResponseMessage PutApplicant(int id, [FromBody]HRApplicant applicant)
        {
            if (applicant == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "no data sent");
            }
            applicant.Id = id;

            db.Entry(applicant).State = EntityState.Modified;

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, applicant);
        }

        // POST: api/Patients
        [ResponseType(typeof(HRApplicant))]
        public async Task<IHttpActionResult> PostApplicant(HRApplicant applicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            applicant.Timestamp = DateTime.Now;


            db.HRApplicants.Add(applicant);
            await db.SaveChangesAsync();

            //generate reg number and password if is a new registration
            if (applicant.ApplicantId.Equals(""))
            {
                var facilityInitial = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "FacilityInittials").Value;
                var year = DateTime.Now.ToString("yy");
                var prefix = "00";
                if (applicant.Id > 9)
                {
                    prefix = "0";
                }
                else if (applicant.Id > 99)
                {
                    prefix = "";
                }
                var branchId = db.KeyValuePairs.FirstOrDefault(e => e.Key_ == "BranchId").Value;
                var applicantid = facilityInitial + "/" + branchId + "/" + prefix + applicant.Id + "/" + year;
                applicant.ApplicantId = applicantid;
                applicant.Password = applicantid; //consider generating random password
            }
            else
            {
                applicant.Password = applicant.ApplicantId;
            }


            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = applicant.Id },applicant);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof(HRApplicant))]
        public async Task<IHttpActionResult> DeleteApplicant(int id)
        {
            HRApplicant applicant = await db.HRApplicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }

            db.HRApplicants.Remove(applicant);
            await db.SaveChangesAsync();

            return Ok(applicant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicantExists(int id)
        {
            return db.HRApplicants.Count(e => e.Id == id) > 0;
        }
    }
}
