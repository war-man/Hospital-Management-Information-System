using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class A4ReceiptData
    {
        public String FacilityName { get; set; }
        public String FacilityAddress { get; set; }
        public String FacilityContactNumber { get; set; }
        public Patient Patient { get; set; }
        public OpdRegister OpdRegister { get; set; }
        public List<BillService> BillServices{ get; set; }
    }
}