using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class DepartmentalRevenueData
    {
        public List<Department> Departments { get; set; }
        public List<BillService> BillServices { get; set; }
        public List<Medication> Medications { get; set; }
        public List<Walking> Walkins { get; set; }
        public List<BillPayment> Payments { get; set; }
    }
}