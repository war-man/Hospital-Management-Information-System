using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
using LabsDataAccess;

namespace Caresoft2._0
{
   
    public class HouseKeeping
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private CareSoftLabsEntities labDB = new CareSoftLabsEntities();

        public void MarkFullyAwardedBillItemAsPaid()
        {
            var unpaidServices = db.BillServices.Where(e => !e.Paid && e.Award > 0).ToList();
            foreach(var entry in unpaidServices)
            {

            }
        }

        public  bool IsUnderFive(int patientID)
        {
            var patient = db.Patients.Find(patientID);
            if (patient != null)
            {
                if(DateTime.Now.Year - patient.DOB.Value.Year <= 5)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ExemptUnderFive()
        {
            var key = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower() == "exempt_under_five");
            if (key == null)
            {
                AddKeyValue("exempt_under_five", "NO");
                return false;
            }
            if (key.Value.ToLower().Trim() == "yes")
            {
                return true;
            }
            return false;
        }

        public bool ExemptionsToFlow()
        {
            var key = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower() == "exemptions_to_flow");
            if (key == null)
            {
                AddKeyValue("exemptions_to_flow", "NO");
                return false;
            }
            if (key.Value.ToLower().Trim() == "yes")
            {
                return true;
            }
            return false;
        }

        public void AddKeyValue(String key, string value, string owner = null)
        {
            var k = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Trim() == key.ToLower().Trim()
            && e.Owner == owner);
            if (k == null)
            {
                var keyvalue = new KeyValuePair();
                keyvalue.Key_ = key.Trim();
                keyvalue.Value = value.Trim();
                keyvalue.Owner = owner;
                db.KeyValuePairs.Add(keyvalue);
                db.SaveChanges();
            }
            
        }

        public void AutoWaiver(int billServiceId, string reason)
        {
            var billService = db.BillServices.Find(billServiceId);

            //get waiverNumber
            var waiverNumber = 0;
            if(billService != null)
            {
                var owner = billService.OpdRegister;
                if (owner.Waivers.Count() >= 1)
                {
                    waiverNumber = owner.Waivers.Last().Id;
                    var waiver = db.Waivers.Find(waiverNumber);
                    waiver.AmountWaived = waiver.AmountWaived + (billService.Price * billService.Quatity) - (billService.Award * billService.Quatity);
                    db.SaveChanges();
                }
                else
                {
                    var waiver = new Waiver();
                    waiver.OPDIPDNo = owner.Id;
                    waiver.AmountWaived = (billService.Price * billService.Quatity) - (billService.Award * billService.Quatity);
                    waiver.ReasonForWaiver = reason;
                    waiver.WaiverNote = "Auto Waiver";
                    waiver.UserId = 1; waiver.BranchId = 1;
                    waiver.DateAdded = DateTime.Now;
                    db.Waivers.Add(waiver);
                    db.SaveChanges();

                    waiverNumber = waiver.Id;
                }

                billService.WaivedAmount = (billService.Price * billService.Quatity) - (billService.Award * billService.Quatity);
                billService.WaiverNo = waiverNumber;
                billService.Paid = true;
                db.SaveChanges();

                AttemptMarkPaid(billService.Id, 0);
            }
        }

        public double OutStandingBill(int patientId)
        {
            var bill = 0.0;
            var pat = db.Patients.Find(patientId);
            var myVisits = pat.OpdRegisters.OrderByDescending(e => e.Id);
            if (myVisits.Count() > 0)
            {
                var myLastVisit = myVisits.FirstOrDefault();
                var myUnpaidBills = myLastVisit.BillServices.Where(b => !b.Paid).Sum(e => (e.Quatity * e.Price)
                - (e.WaivedAmount ?? 0) - (e.Award * e.Quatity) - (e.IPDBillPartialPayments.Sum(a => a.AllocatedAmount)));

                var myUnpaidMeds = myLastVisit.Medications.Where(e => !e.Paid).Sum(e => (e.QuantityIssued * e.UnitPrice)
                  - e.WaivedAmount??0 - (e.Award * e.QuantityIssued) - (e.IPDBillPartialPayments.Sum(a => a.AllocatedAmount)));

                bill = myUnpaidBills + (myUnpaidMeds ?? 0);
            }

            return bill;
        }

        public void ExemptEarlierRegisteredUnder5()
        {
            if (ExemptUnderFive())
            {
                var underFiveServices = db.BillServices.Where(e => !e.Paid && IsUnderFive(e.OpdRegister.PatientId));
                foreach (var u5s in underFiveServices)
                {
                    AutoWaiver(u5s.Id, "under 5 automatic waiver");
                }
            }
            
        }


        public void ApplyAward(int billserviceId)
        {
            var billService = db.BillServices.Find(billserviceId);
            if (billService != null)
            {
                var owner = billService.OpdRegister;
                var tariff = owner.Tariff;

                //If IPD Go though invoicing
                if (tariff != null && tariff.TariffName.ToLower().Trim()!="cash" && !owner.IsIPD)
                {
                    var awardEntry = tariff.ServicesPrices.FirstOrDefault(e => e.ServiceId == billService.Service.Id);
                    if(awardEntry != null)
                    {
                        var amt = awardEntry.Award;
                        if(awardEntry.AwardUnit.ToLower().Trim() == "percent")
                        {
                            var InsurancePrices = db.InsurancePrices.FirstOrDefault(w => w.ServicesPrice.ServiceId
                            == billService.Service.Id && w.CompanyId == tariff.CompanyId);

                            if (InsurancePrices != null)
                            {
                                amt = amt / 100 * InsurancePrices.Price;

                                billService.Award = amt;
                                db.SaveChanges();
                                AttemptMarkPaid(billService.Id, 0);
                            }
                            else
                            {
                                amt = amt / 100 * billService.Service.CashPrice;

                                billService.Award = amt;
                                db.SaveChanges();
                                AttemptMarkPaid(billService.Id, 0);
                            }

                        }
                        else
                        {
                            billService.Award = amt;
                            db.SaveChanges();
                            AttemptMarkPaid(billService.Id, 0);
                        }
                       
                    }
                }
            }
        }

        public void AttemptMarkPaid(int billSeviceId, int? wotid)
        {
            var billService = db.BillServices.Find(billSeviceId);

            if (wotid != 0)
            {
                billService.WorkOrderTestId = wotid;
                db.SaveChanges();
            }

            var bal = ((billService.Price * billService.Quatity) -
                (billService.Award * billService.Quatity) 
                - (billService.WaivedAmount ?? 0) - (billService.Discount??0));
            if ( bal<= 0 || billService.OpdRegister.Tariff.Company.CompanyName.ToLower().Trim()=="exemption")
            {
                billService.Paid = true;
                db.SaveChanges();

                if (billService.Service.IsLAB || billService.Service.IsXRAY )
                {
                    if(billService.WorkOrderTestId !=null && billService.WorkOrderTestId != 0)
                    {
                        var workOrderTest = labDB.WorkOrderTests.Find(billService.WorkOrderTestId);
                        workOrderTest.BillPaid = true;
                        labDB.SaveChanges();
                    }
                } 
            }
        }

        public String GetAge(DateTime dob)
        {
            var age = DateTime.Today.Year - dob.Year;
            var ageUnit = "Yr";
            if (age < 1)
            {
                age = DateTime.Today.Month - dob.Month;
                ageUnit = "Mnt";
            }
            if (age < 1)
            {
                age = DateTime.Today.Day - dob.Day;
                ageUnit = "Day";
            }
            if (age > 1)
            {
                ageUnit += "s";
            }

            return age + " " + ageUnit;
        }

        public String PatientName(int id)
        {
            var name = "";
            var pat = db.Patients.Find(id);
            if (pat != null)
            {
                name =  pat.Salutation.Trim() + " " + pat.FName + " " + pat.MName + " " + pat.LName;
            }

           return name;
        }


    }
}