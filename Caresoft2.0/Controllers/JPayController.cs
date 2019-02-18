using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using CaresoftHMISDataAccess;
namespace Caresoft2._0.Controllers.API
{
    public class JPayController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private HouseKeeping hs = new HouseKeeping();

        public class JPAYData
        {
            public String PatientName { get; set; }
            public String RegNumber { get; set; }
            public int OPDNo { get; set; }
            public String Status { get; set; }
            public string Message { get; set; }
            public double BillAmount { get; set; }

        }

        public double OutStandingBill(OpdRegister opd)
        {
            var bill = 0.0;
            double temp = 0;

            if (opd != null)
            {
                var myUnpaidBills = opd.BillServices.Where(b => !b.Paid).Sum(e => (e.Quatity * e.Price)
                - (e.WaivedAmount ?? temp) - (e.Award * e.Quatity) - (e.IPDBillPartialPayments.Sum(a => a.AllocatedAmount)));

                var myUnpaidMeds = (opd.Medications.Where(e => !e.Paid && e.Available==true).Sum(e => (e.QuantityIssued * e.UnitPrice)
                   - (e.Award * e.QuantityIssued) - (e.IPDBillPartialPayments.Sum(a => a.AllocatedAmount))- ((e.WaivedAmount)?? temp)));

                bill = myUnpaidBills + (myUnpaidMeds ?? temp);
            }

            return bill;
        }

        //Get patient with bill amount
        public ActionResult Get(int id)
        {
            var OPD = db.OpdRegisters.FirstOrDefault(e => e.Id.Equals(id));
            
            try
            {
                UnconfirmedPayment(OPD.Id);
            }
            catch (Exception e)
            {
                return Content("failed to check unconfirmed payments : " + e.Message);
            }
            //id is opd/ipd number
            var data = new JPAYData();

            var Patient = OPD.Patient;

            if (Patient != null)
            {
                var opd = OPD;

                if (opd == null)
                {
                    data.Status = "Error";
                    data.Message = "Patient not found";

                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                var pat = opd.Patient;

                data.PatientName = hs.PatientName(pat.Id);
                data.RegNumber = pat.RegNumber;

                data.OPDNo = opd.Id;
                data.Status = "Success";

                data.BillAmount = OutStandingBill(OPD);

                var MyUnusedJPays = opd.JPayments.Where(e => e.Status.ToLower() == "unused").ToList();

                //foreach (var jpayment in MyUnusedJPays)
                //{
                //    var send_extra_to_ewallet = db.EWallets.Add(
                //        new EWallet()
                //        {
                //            PatientId = OPD.Patient.Id,
                //            AmountTransacted = jpayment.Amount,
                //            Direction = 1,
                //            Description = "Jpay",
                //            BranchId = (int)Session["UserBranchId"],
                //            UserId = (int)Session["UserId"],
                //            TimeAdded = DateTime.Now
                //        });

                //    //Add to e wallet
                //    db.EWallets.Add(send_extra_to_ewallet);

                //    //chek if successfully added 
                //    if (db.SaveChanges() > 0)
                //    {
                //        //Mark as used
                //        jpayment.Status = "Used";
                //        db.SaveChanges();
                //    }

                //}

                //if (MyUnusedJPays != null && MyUnusedJPays.Count() > 0)
                //{
                //    data.BillAmount = data.BillAmount - MyUnusedJPays.Sum(e => e.Amount);

                //    if (data.BillAmount <= 0)
                //    {
                //        if (data.BillAmount < 0)
                //        {
                //            var send_extra_to_ewallet = db.EWallets.Add(
                //           new EWallet()
                //           {
                //               PatientId = OPD.Patient.Id,
                //               AmountTransacted = System.Math.Abs(data.BillAmount),
                //               Direction = 1,
                //               Description = "Jpay",
                //               BranchId = (int)(Session["UserBranchId"]??1),
                //               UserId = (int)(Session["UserId"]??1),
                //               TimeAdded = DateTime.Now
                //           });


                //            db.EWallets.Add(send_extra_to_ewallet);
                //            db.SaveChanges();
                //        }


                //        var services = opd.BillServices.Where(e => !e.Paid);
                //        var meds = opd.Medications.Where(e => !e.Paid);

                //        var jPayAuto = new JPayAutoPayment();
                //        jPayAuto.OPDNo = opd.Id;


                //        if (services.Count() > 0)
                //        {
                //            var billedServices = "";
                //            foreach (var s in services)
                //            {
                //                s.Paid = true;
                //                billedServices += s.Id + ",";
                //            }

                //            jPayAuto.BilledServices = billedServices;
                //            jPayAuto.JPayId = MyUnusedJPays.FirstOrDefault().Id;
                //        }

                //        if (meds.Count() > 0)
                //        {
                //            var billedMeds = "";
                //            foreach (var m in meds)
                //            {
                //                m.Paid = true;
                //                billedMeds += m.Id + ",";
                //            }
                //            jPayAuto.BilledMedication = billedMeds;
                //            jPayAuto.JPayId = MyUnusedJPays.FirstOrDefault().Id;

                //        }

                //        jPayAuto.DateAdded = DateTime.Now;
                //        db.JPayAutoPayments.Add(jPayAuto);

                //        foreach (var p in MyUnusedJPays)
                //        {
                //            p.Status = "Used";
                //        }

                //        db.SaveChanges();
                //    }
                //    else
                //    {
                //        //Send to e-Wallet

                //        foreach (var jpayment in MyUnusedJPays)
                //        {
                //            var send_extra_to_ewallet = db.EWallets.Add(
                //                new EWallet()
                //                {
                //                    PatientId = OPD.Patient.Id,
                //                    AmountTransacted = System.Math.Abs(data.BillAmount),
                //                    Direction = 1,
                //                    Description = "Jpay",
                //                    BranchId = db.Users.FirstOrDefault(e => e.Id.Equals((int)Session["UserId"])).Employee.Branch.Id,
                //                    UserId = 1,
                //                    TimeAdded = DateTime.Now
                //                });

                //            //Add to e wallet
                //            db.EWallets.Add(send_extra_to_ewallet);

                //            //chek if successfully added 
                //            if (db.SaveChanges() > 0)
                //            {
                //                //Mack as used
                //                jpayment.Status = "Used";
                //            }

                //            //save db changes
                //            db.SaveChanges();

                //        }

                //        var dep = OPD.Patient.EWallets.Where(f => f.Direction.Equals(1))
                //            .Sum(e => e.AmountTransacted);

                //        var with = OPD.Patient.EWallets.Where(f => f.Direction.Equals(2))
                //            .Sum(e => e.AmountTransacted);

                //        var accBalance = dep - with;

                //        data.BillAmount = data.BillAmount - accBalance;


                //    }
                //}
                //else
                //{
                //    var dep = OPD.Patient.EWallets.Where(f => f.Direction.Equals(1))
                //            .Sum(e => e.AmountTransacted);

                //    var with = OPD.Patient.EWallets.Where(f => f.Direction.Equals(2))
                //        .Sum(e => e.AmountTransacted);

                //    var accBalance = dep - with;

                //    data.BillAmount = data.BillAmount - accBalance;
                //}

                if (TempData["Message"] != null)
                {
                    data.Message = TempData["Message"].ToString();
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }

            data.Status = "Error";
            data.Message = "Patient not found";

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ReceiveJPay(JPayment payment)
        {

           
            try
            {
                payment.Status = "Unused";
                payment.DateAdded = DateTime.Now;
                db.JPayments.Add(payment);
                db.SaveChanges();

                TempData["Message"] = "Payment recorded successfully";

                //attempt mark bill items as paid
                var opd = db.OpdRegisters.Find(payment.OPDNo);
                if (opd != null)
                {
                    var jpayWallet = 0.0;

                    var MyUnusedJPays = opd.JPayments.Where(e => e.Status.ToLower() == "unused")
                        .ToList();
                    if (MyUnusedJPays.Count() > 0)
                    {
                        jpayWallet = MyUnusedJPays.Sum(e => e.Amount);
                    }

                    var billAmount = hs.OutStandingBill(opd.PatientId);
                    if ((jpayWallet == billAmount))
                    {
                        var services = opd.BillServices.Where(e => !e.Paid);
                        var meds = opd.Medications.Where(e => !e.Paid);

                        var jPayAuto = new JPayAutoPayment();
                        jPayAuto.OPDNo = opd.Id;
                        jPayAuto.JPayId = payment.Id;


                        if (services.Count() > 0)
                        {
                            var billedServices = "";
                            foreach (var s in services)
                            {
                                s.Paid = true;
                                billedServices += s.Id + ",";
                            }

                            jPayAuto.BilledServices = billedServices;
                        }

                        if (meds.Count() > 0)
                        {
                            var billedMeds = "";
                            foreach (var m in meds)
                            {
                                m.Paid = true;
                                billedMeds += m.Id + ",";
                            }
                            jPayAuto.BilledMedication = billedMeds;
                        }

                        jPayAuto.DateAdded = DateTime.Now;
                        db.JPayAutoPayments.Add(jPayAuto);

                        foreach (var p in MyUnusedJPays)
                        {
                            p.Status = "Used";
                        }

                        db.SaveChanges();

                    }
                }
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);


                var fullErrorMessage = string.Join("; ", errorMessages);


                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", 
                    fullErrorMessage);


                return Json(new { Message = exceptionMessage });
            }

  
            return RedirectToAction("Get", new {id =payment.OPDNo});
        }

        public class JPAYDataFromSoftcomServer
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public int facility_id { get; set; }
            public string patient_number { get; set; }
            public string bill_number { get; set; }
            public String payer_name { get; set; }
            public double amount { get; set; }
            public String remote_ip { get; set; }
            public String transaction_id { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public String status { get; set; }

            //public int id { get; set; }
            //public string user_id { get; set; }
            //public string facility_id { get; set; }
            //public string patient_number { get; set; }
            //public string bill_number { get; set; }
            //public string payer_name { get; set; }
            //public string amount { get; set; }
            //public string remote_ip { get; set; }
            //public string transaction_id { get; set; }
            //public string status { get; set; }
            //public string created_at { get; set; }
            //public string updated_at { get; set; }

        }

        public dynamic UnconfirmedPayment(int id)

        {
            //id is opd/ipd number

            using (WebClient wc = new WebClient())
            {
                var data = wc.DownloadString("https://softcom.co.ke/extras/api/jpay/" + id);

                if (new LoginController().IsDebugMode)
                {
                    data = wc.DownloadString("https://softcom.co.ke/extras/api/jpay/" + id);
                }
                var paymentsList = new List<JPAYDataFromSoftcomServer>();
                paymentsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JPAYDataFromSoftcomServer>>(data);
                var inserted = 0;

                if (paymentsList.Count() > 0)
                {
                    var unused = paymentsList.Where(e => e.status.ToLower().Trim() == "unused"
                    && e.bill_number.Equals(id.ToString())).ToList();
                    if (unused.Count() > 0)
                    {

                        try
                        {
                            var mypayments = db.JPayments.Where(e => e.OPDNo == id).ToList();
                            foreach (var p in unused)
                            {
                                if (mypayments.Where(e => e.TransactionId == p.transaction_id).Count() == 0)
                                {
                                    var jpa = new JPayment();
                                    jpa.Amount = p.amount;
                                    jpa.OPDNo = int.Parse(p.bill_number);
                                    jpa.Payer = p.payer_name;
                                    jpa.RemoteIP = p.remote_ip;
                                    jpa.TransactionId = p.transaction_id;
                                    jpa.JPayUser = p.user_id;
                                    jpa.Status = p.status;
                                    jpa.DateAdded = DateTime.Now;

                                    db.JPayments.Add(jpa);
                                    db.SaveChanges();
                                    inserted++;
                                }
                                
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {

                            var errorMessages = ex.EntityValidationErrors
                                    .SelectMany(x => x.ValidationErrors)
                                    .Select(x => x.ErrorMessage);


                            var fullErrorMessage = string.Join("; ", errorMessages);


                            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


                            return Json(new { Message = exceptionMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                return inserted;
            }

            
        }
        
    }
}
