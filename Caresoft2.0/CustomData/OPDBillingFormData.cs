using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
namespace Caresoft2._0.CustomData
{
    public class OPDBillingFormData
    {
        public OpdRegister OpdRegister { get; set; }
        public Patient Patient { get; set; }

        public List<BillService> BillServices { get; set; }
        public int Shift { get; set; }
        public List<Department> Departments { get; set; }
        public List<OpdRegister> OPDBillingQueue { get; set; }
        public List<PaymentMode> PaymentModes { get; set; }
        public List<ServiceGroup> ServiceGroups { get; set; }
        public List<Medication> Drugs { get; set; }
    }
}