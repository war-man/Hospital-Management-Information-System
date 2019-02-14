using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class PatientsAutoCompleteData
    {
        public int PatientId { get; set; }
        public string RegNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }
        public double EWalletBalance { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string PhoneNumber { get; set; }

        public int OpdId { get; set; }
        public string OpdDate { get; set; }

        public string Age { get; set; }
    }
}