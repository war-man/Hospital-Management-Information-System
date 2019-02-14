using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    public class OpdRegisterController : ApiController
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: api/OpdRegister
        public IQueryable<OpdRegister> GetOpdRegisters()
        {
            return db.OpdRegisters;
        }

        // GET: api/OpdRegister/5
        [ResponseType(typeof(OpdRegister))]
        public IHttpActionResult GetOpdRegister(int id)
        {
            OpdRegister opdRegister = db.OpdRegisters.Find(id);
            if (opdRegister == null)
            {
                return NotFound();
            }

            return Ok(opdRegister);
        }

        // PUT: api/OpdRegister/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOpdRegister(int id, OpdRegister opdRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != opdRegister.Id)
            {
                return BadRequest();
            }

            db.Entry(opdRegister).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpdRegisterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE: api/OpdRegister/5
        [ResponseType(typeof(OpdRegister))]
        public IHttpActionResult DeleteOpdRegister(int id)
        {
            OpdRegister opdRegister = db.OpdRegisters.Find(id);
            if (opdRegister == null)
            {
                return NotFound();
            }

            db.OpdRegisters.Remove(opdRegister);
            db.SaveChanges();

            return Ok(opdRegister);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OpdRegisterExists(int id)
        {
            return db.OpdRegisters.Count(e => e.Id == id) > 0;
        }
    }
}