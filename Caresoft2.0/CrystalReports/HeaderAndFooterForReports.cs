using Caresoft2._0.CrystalReports.ReportFooter;
using Caresoft2._0.CrystalReports.ReportHeader;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CrystalReports
{
    public static class HeaderAndFooterForReports
    {
        static CaresoftHMISEntities db2 = new CaresoftHMISEntities();

        public static DataSetFacilityInformation GetAllReportHeader()
        {

            var facilityDataSet = new DataSetFacilityInformation();
            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            var facilityAddress = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityAddress").FirstOrDefault().Value;

            var facilityTelephone = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityContactNumber").FirstOrDefault().Value;

            var logoUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/icons/HospitalLogo.png"));

            facilityDataSet.HospitalDetails.AddHospitalDetailsRow(
                HospitalName,
                facilityAddress,
                facilityTelephone,
                logoUrl
            );

            return facilityDataSet;
        }

        public static DataSetFooter GetAllReportFooter()
        {

            var facilityDataSet = new DataSetFooter();
            var HospitalName = db2.KeyValuePairs.Where(p => p.Key_ == "FacilityName").FirstOrDefault().Value;

            
            var facilityMission = db2.KeyValuePairs.Where(p => p.Key_.Trim().ToLower().Contains("mission")).FirstOrDefault()?.Value ??
                                  db2.KeyValuePairs.Where(p => p.Key_.Trim().ToLower().Contains("vision")).FirstOrDefault()?.Value ??
                                  db2.KeyValuePairs.Where(p => p.Key_.Trim().ToLower().Contains("slogan")).FirstOrDefault()?.Value?? "Providing Health Care";

            var todaysYear = DateTime.Now.Year.ToString();
            HospitalName = HospitalName + " - " + facilityMission +" "+ "@"+ todaysYear; 

            facilityDataSet.HospitalDetails.AddHospitalDetailsRow(
                HospitalName,"","","","",
                ""
            );

            return facilityDataSet;
        }
    }

}