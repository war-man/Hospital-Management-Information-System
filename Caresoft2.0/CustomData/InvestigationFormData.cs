using System;
using System.Collections.Generic;
using System.Linq;
using CaresoftHMISDataAccess;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class InvestigationFormData
    {
        public Patient Patient { get; set; }
        public OpdRegister OpdRegister { get; set; }
        public List<BillService> TestBillServices { get; set; }
    }
}