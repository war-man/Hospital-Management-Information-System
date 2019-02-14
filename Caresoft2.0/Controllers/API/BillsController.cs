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
    public class BillsController : ApiController
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

      

        //[Route("api/bills/search")]
        //public HttpResponseMessage PostSearch(SearchBill searchBill)
        //{
        //    var SearchBy = searchBill.SearchBy;
           
        //    if (SearchBy  == "ReceiptNo")
        //    {
        //         var items = db.Chargeables.Where(e => e.BillNo.ToString().Contains(searchBill.SearchText));
        //        return Request.CreateResponse(HttpStatusCode.OK, items);
        //    }
        //    else if (SearchBy == "PatientNo")
        //    {
        //         var items = db.Chargeables.Where(e =>e.PatientId.ToString().Equals(searchBill.SearchText)
        //         && e.Status.ToLower() == "unpaid");
        //        return Request.CreateResponse(HttpStatusCode.OK, items);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, "No data was found");
        //    }
            

        //}

        //// GET: api/Bills/5
        //[ResponseType(typeof(Bill))]
        //public async Task<IHttpActionResult> GetBill(int id)
        //{
        //    Bill bill = await db.Bills.FindAsync(id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(bill);
        //}

        //// PUT: api/Bills/5
       
        //public HttpResponseMessage PutBill(int id, [FromBody]Bill bill)
        //{
        //    var entity = db.Bills.FirstOrDefault(e => e.Id.Equals(id));
        //    if(entity == null)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Bill not found");
        //    }

        //    entity.ShiftNo = bill.ShiftNo;
        //    entity.Received = bill.Received;
        //    entity.TotalChargeable = bill.TotalChargeable;
        //    entity.Waiver = bill.Waiver;
        //    entity.WaiverUnit = bill.WaiverUnit;
        //    entity.Payable = bill.Payable;
        //    entity.Change = bill.Change;
        //    entity.PaymentMode = bill.PaymentMode;
        //    entity.WaiverNumber = bill.WaiverNumber;
        //    entity.Exception = bill.Exception;
        //    entity.ReasonForException = bill.ReasonForException;
        //    entity.Currency = bill.Currency;
        //    entity.PaidBy = bill.PaidBy;
        //    entity.Status = bill.Status;
        //    entity.ClosureTime = DateTime.Now;

        //    db.SaveChanges();

        //    return Request.CreateResponse(HttpStatusCode.OK, "Billed Successfully");

        //}

        //// POST: api/Bills
        //[ResponseType(typeof(Bill))]
        //public async Task<IHttpActionResult> PostBill(Bill bill)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Bills.Add(bill);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = bill.Id }, bill);
        //}

        //// DELETE: api/Bills/5
        //[ResponseType(typeof(Bill))]
        //public async Task<IHttpActionResult> DeleteBill(int id)
        //{
        //    Bill bill = await db.Bills.FindAsync(id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Bills.Remove(bill);
        //    await db.SaveChangesAsync();

        //    return Ok(bill);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool BillExists(int id)
        //{
        //    return db.Bills.Count(e => e.Id == id) > 0;
        //}
    }
}