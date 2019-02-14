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
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    public class ChargeablesController : ApiController
    {
        //private CaresoftHMISEntities db = new CaresoftHMISEntities();

        //// GET: api/Chargeables
        //public IQueryable<Chargeable> GetChargeables()
        //{
        //    return db.Chargeables;
        //}

        //// GET: api/Chargeables/5
        //[ResponseType(typeof(Chargeable))]
        //public async Task<IHttpActionResult> GetChargeable(int id)
        //{
        //    Chargeable chargeable = await db.Chargeables.FindAsync(id);
        //    if (chargeable == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(chargeable);
        //}

        //// PUT: api/Chargeables/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutChargeable(int id, Chargeable chargeable)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != chargeable.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(chargeable).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ChargeableExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Chargeables
        //public HttpResponseMessage PostChargeable(Chargeable chargeable)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, "No data was found");
        //    }
        //    if (chargeable.BillNo.Equals(0))
        //    {
        //        //create a new bill and return the bill number                   
        //        Bill bill = new Bill();
        //        bill.PatientId = chargeable.PatientId;
        //        bill.EntryTime = DateTime.Now;
        //        db.Bills.Add(bill);
        //        db.SaveChanges();

        //        var billId = bill.Id;

        //        chargeable.BillNo = billId;
        //    }
        //    chargeable.TimeAdded = DateTime.Now;
        //    db.Chargeables.Add(chargeable);
        //    db.SaveChanges();

        //    var obj = new {
        //                billNo = chargeable.BillNo, 
        //                entryId = chargeable.Id
        //        };

        //    return Request.CreateResponse(HttpStatusCode.OK,  obj);
        //}

        //// DELETE: api/Chargeables/5
        //[ResponseType(typeof(Chargeable))]
        //public async Task<IHttpActionResult> DeleteChargeable(int id)
        //{
        //    Chargeable chargeable = await db.Chargeables.FindAsync(id);
        //    if (chargeable == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Chargeables.Remove(chargeable);
        //    await db.SaveChangesAsync();

        //    return Ok(chargeable);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ChargeableExists(int id)
        //{
        //    return db.Chargeables.Count(e => e.Id == id) > 0;
        //}
    }
}