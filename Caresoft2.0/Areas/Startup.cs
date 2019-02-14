using CaresoftHMISDataAccess;
using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Caresoft2.Areas
{
   

    public partial class Startup
    {
        private CareSoftLabsEntities dblabs = new CareSoftLabsEntities();
        private CaresoftHMISEntities dbhms = new CaresoftHMISEntities();
        //int main_department_id = new CaresoftHMISEntities().Departments.FirstOrDefault(d => d.DepartmentName.Equals("PATHOLOGY")).Id;

        //Setting Department Names, 
        //Key_ = "PathologyDepartmentMarquee",
        //Value = "Pathology",
        public void setUpSettingsParameters()
        {

            //DPathology/Marquee
            var pathMarq = new PathKeyValuePair
            {
                Key_ = "PathologyDepartmentMarquee",
                Value = "Enter Pathology Marquee"

            };
            var pathDep = new PathKeyValuePair
            {
                Key_ = "PathologyDepartmentName",
                Value = "Pathology Department",

            };

            //radiology
            var radMarq = new PathKeyValuePair
            {
                Key_ = "RadiologyDepartmentMarquee",
                Value = "Enter Radiology Marquee"

            };
            var radDep = new PathKeyValuePair
            {
                Key_ = "RadiologyDepartmentName",
                Value = "Radiology Department",

            };

            //HistoCyto
            var histoCytoMarq = new PathKeyValuePair
            {
                Key_ = "HistoCytoDepartmentMarquee",
                Value = "HistoCyto Department",

            };
            var histoCytoDep = new PathKeyValuePair
            {
                Key_ = "HistoCytoDepartmentName",
                Value = "HistoCyto Department",

            };

            dblabs.PathKeyValuePairs.Add(pathMarq);
            dblabs.PathKeyValuePairs.Add(pathDep);

            dblabs.PathKeyValuePairs.Add(radMarq);
            dblabs.PathKeyValuePairs.Add(radDep);

            dblabs.PathKeyValuePairs.Add(histoCytoMarq);
            dblabs.PathKeyValuePairs.Add(histoCytoDep);

            //Add Ally types Of Users
            var userType1 = new UserType
            {
                UserType1 = "Admin",
            };
            var userType2 = new UserType
            {
                UserType1 = "Team Leader",
            };
            var userType3 = new UserType
            {
                UserType1 = "Lab Technichian",
            };
            dblabs.UserTypes.Add(userType1);
            dblabs.UserTypes.Add(userType2);
            dblabs.UserTypes.Add(userType3);


            //Add gender
            var genderFemale = new Gender { Type = "Female" };
            var genderMale = new Gender { Type = "Male" };
            var genderOther = new Gender { Type = "All" };
            dblabs.Genders.Add(genderFemale);
            dblabs.Genders.Add(genderMale);
            dblabs.Genders.Add(genderOther);


            //Add Period Types
            var Days = new Period { Days = "Days" };
            var Months = new Period { Days = "Months" };
            var Years = new Period { Days = "Years" };
            dblabs.Periods.Add(Days);
            dblabs.Periods.Add(Months);
            dblabs.Periods.Add(Years);


           //Add Status
            List<string> statuses = new List<string>();
            statuses.Add("Authorized");
            statuses.Add("Default");
            statuses.Add("SampleCollection");
            statuses.Add("TestResultEntry");
            statuses.Add("Rejected");

            statuses.Add("Tested");
            statuses.Add("Partial Tested");
            statuses.Add("Partial Authorized");
            statuses.Add("Printed");


            statuses.Add("Partially Printed");
            statuses.Add("Registered");
            


            foreach (string status in statuses)
            {
                var s = new Status
                {
                    StatusValue = status,

                };
                dblabs.Status.Add(s);

            }


            //Configure colors
            var LabsQueueColorSeen = new PathKeyValuePair
            {
                Key_ = "LabsQueueColorSeen",
                Value = "green",

            };


            var LabsColorBillPaid = new PathKeyValuePair
            {
                Key_ = "LabsColorBillPaid",
                Value = "#FF9800",

            };
            var LabsColorPartialBillPaid = new PathKeyValuePair
            {
                Key_ = "LabsColorPartialBillPaid",
                Value = "#0000FF",

            };
            var LabsColorPendingBill = new PathKeyValuePair
            {
                Key_ = "LabsColorPendingBill",
                Value = "#F74519",

            };
            var LabsColorSampleCollectionDone = new PathKeyValuePair
            {
                Key_ = "LabsColorSampleCollectionDone",
                Value = "#008000",

            };
            var LabsColorSampleCollectionPending = new PathKeyValuePair
            {
                Key_ = "LabsColorSampleCollectionPending",
                Value = "#F073F0",

            };

            dblabs.PathKeyValuePairs.Add(LabsQueueColorSeen);

            dblabs.PathKeyValuePairs.Add(LabsColorBillPaid);
            dblabs.PathKeyValuePairs.Add(LabsColorPartialBillPaid);
            dblabs.PathKeyValuePairs.Add(LabsColorPendingBill);
            dblabs.PathKeyValuePairs.Add(LabsColorSampleCollectionDone);
            dblabs.PathKeyValuePairs.Add(LabsColorSampleCollectionPending);

            var LabsColorRegistered = new PathKeyValuePair
            {
                Key_ = "LabsColorRegistered",
                Value = "#D4FFD5",

            };
            var LabsColorTested = new PathKeyValuePair
            {
                Key_ = "LabsColorTested",
                Value = "#FBFDC8",

            };
            var LabsColorAuthorized = new PathKeyValuePair
            {
                Key_ = "LabsColorAuthorized",
                Value = "#ABABAB",

            };
            var LabsColorPartialAuthorized = new PathKeyValuePair
            {
                Key_ = "LabsColorPartialAuthorized",
                Value = "#FCE8FC",

            };
            var LabsColorPartialTested = new PathKeyValuePair
            {
                Key_ = "LabsColorPartialTested",
                Value = "#FCD951",

            };
            var LabsColorPartiallyPrinted = new PathKeyValuePair
            {
                Key_ = "LabsColorPartiallyPrinted",
                Value = "#DBB8D1",

            };
            var LabsColorPrinted = new PathKeyValuePair
            {
                Key_ = "LabsColorPrinted",
                Value = "#B8F765",

            };


            dblabs.PathKeyValuePairs.Add(LabsColorRegistered);
            dblabs.PathKeyValuePairs.Add(LabsColorTested);
            dblabs.PathKeyValuePairs.Add(LabsColorAuthorized);
            dblabs.PathKeyValuePairs.Add(LabsColorPartialAuthorized);
            dblabs.PathKeyValuePairs.Add(LabsColorPartialTested);
            dblabs.PathKeyValuePairs.Add(LabsColorPartiallyPrinted);
            dblabs.PathKeyValuePairs.Add(LabsColorPrinted);

            var LabsColorAccessionDonePatient = new PathKeyValuePair
            {
                Key_ = "LabsColorAccessionDonePatient",
                Value = "#FFA6A3",

            };
            var LabsColorAccessionPendingPatient = new PathKeyValuePair
            {
                Key_ = "LabsColorAccessionPendingPatient",
                Value = "#2F7500",

            };

            dblabs.PathKeyValuePairs.Add(LabsColorAccessionDonePatient);
            dblabs.PathKeyValuePairs.Add(LabsColorAccessionPendingPatient);


            var ShowInQueueBillNotPaid = new PathKeyValuePair
            {
                Key_ = "ShowInQueueBillNotPaid",
                Value = "true",

            };
          
            dblabs.PathKeyValuePairs.Add(ShowInQueueBillNotPaid);

            dblabs.SaveChanges();
        }
    }
}
