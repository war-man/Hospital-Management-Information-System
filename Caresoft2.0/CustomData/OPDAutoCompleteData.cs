using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class AutoCompleteData
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String RegNumber { get; set; }
        public int InStock { get; set; }
        public double  Price{ get; set; }
        public double? AwardedAmount { get; set; }
        public double PayableAmount { get; set; }
        public String Department { get; set; }
        public String TestCode { get; set; }
        public bool Available { get; set; }
        public int Age { get; set; }
        public int Quantity { get; set; }
        public string Frequency { get; set; }
        public string RoutingAdmin { get; set; }


        public Patient PatientRow { get; set; }
    }
}