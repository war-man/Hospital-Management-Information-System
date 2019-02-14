using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{

    [Auth]
    public class HomeController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private HouseKeeping hs = new HouseKeeping();

        public ActionResult GetNotificationContacts()
        {
            return Json(new NotificationComponent().GetContacts(DateTime.Now), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {

            ViewBag.Title = db.KeyValuePairs.FirstOrDefault(e => e.Key_.Equals("FacilityName")).Value + " :: Dashboard";
            // ViewBag.Icons = new { icons = "[{\"text\":\"Facility\", \"action\":\"Facility\",\"icon\":\"HMS\"}]"};

            return View(db.DashboardIcons.ToList());
        }

        public ActionResult ExemptEarlierRegisteredUnder5()
        {
            if (hs.ExemptUnderFive())
            {
                var underFiveServices = db.BillServices.Where(e => !e.Paid);
                foreach (var u5s in underFiveServices)
                {
                    if (hs.IsUnderFive(u5s.OpdRegister.PatientId)){
                        hs.AutoWaiver(u5s.Id, "under 5 automatic waiver");
                    }
                }

                return Content("Cleared " + underFiveServices.Count());
            }

            return Content("Under five exemption is disabled. Check Keyvalue pairs.");

        }

        public ActionResult Award100PercentToNHIF()
        {
            var x = 0;
            var nhif = db.Companies.FirstOrDefault(e => e.CompanyName.ToLower().Trim() == "nhif");
            var services = db.Services.ToList();
            if (nhif != null)
            {
                foreach(var t in nhif.Tariffs)
                {
                    db.ServicesPrices.RemoveRange(t.ServicesPrices);
                    db.SaveChanges();

                    foreach(var s in services)
                    {
                        var sp = new ServicesPrice();
                        sp.TariffId = t.Id;
                        sp.ServiceId = s.Id;
                        sp.Award = 100;
                        sp.AwardUnit = "Percent";
                        sp.DoctorFee = 0;
                        sp.DoctorFeeUnit = "Amount";
                        sp.DateAdded = DateTime.Now;
                        
                        sp.BranchId = (int)Session["UserBranchId"] ;
                        sp.UserId = (int)Session["UserId"];

                        db.ServicesPrices.Add(sp);
                        x++;
                    }
                    db.SaveChanges();
                }
            }

            return Content("Updated " + x + " records");
        }


 

        //public ActionResult ProcessKiambuFormerPatients()
        //{
        //    var start = DateTime.Now;
        //    var fpatients = db.KiambuPatients.ToList();
        //    var inserted = 0;
        //    foreach(var p in fpatients)
        //    {
        //        if(p.OutpatientNumber != null && p.Age!=null && p.Age !="0")
        //        {
        //            //try
        //            //{
        //                var cp = new Patient();

        //                cp.RegNumber = "ST/1/" + p.OutpatientNumber.ToString().PadLeft(4, '0') + "/" + p.Date.Value.ToString("yy");

        //                if (p.Gender.ToLower() == "m")
        //                {
        //                    cp.Salutation = "Mr";
        //                }
        //                else
        //                {
        //                    cp.Salutation = "Miss";
        //                }

        //                var names = p.Name.Split(' ');
        //                cp.FName = names[0];
        //                if (names.Count() > 1)
        //                {
        //                    for (var x = 1; x <= names.Count()-1; x++)
        //                    {
        //                        cp.LName += names[x] + " ";
        //                    }

        //                }
        //                else
        //                {
        //                    cp.LName = names[0];
        //                }
        //                cp.Gender = p.Gender.ToUpper().Trim();

        //                if (p.Gender.Length > 1)
        //                {
        //                cp.Gender = "M";
        //                }
                        
        //                int n;
        //                bool isNumericAge = int.TryParse(p.Age, out n);

        //                if (isNumericAge)
        //                {
        //                var age = Convert.ToInt32(p.Age);
        //                if (Convert.ToInt32(p.Age) > 100)
        //                {
        //                    age = 100;
        //                }
        //                    cp.DOB = DateTime.Today.AddYears(-age);
        //                }
        //                else
        //                {
        //                    cp.DOB = p.Date;
        //                }

        //                cp.Mobile = p.Contact.PadRight(10, '0');

        //                if (p.Address != null && p.Address.Trim()!="")
        //                {
        //                    cp.HomeAddress = p.Address;
        //                }
        //                else
        //                {
        //                    cp.HomeAddress = "Kiambu";
        //                }

        //                cp.RegDate = p.Date;

        //                db.Patients.Add(cp);
        //                db.SaveChanges();

        //                inserted++;
        //            //}
        //            //catch (Exception e)
        //            //{
        //            //    WriteLog(e.ToString());
        //            //}
        //        }

        //    }

        //    var tot = DateTime.Now - start;
        //    return Content("Inserted " + inserted + " in " + tot);
        //}

        public static void WriteLog(string strLog)
        {
            string logFilePath = @"C:\CaresoftLogs\Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            FileInfo logFileInfo = new FileInfo(logFilePath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(strLog);
                }
            }
        }
    }


    
}
