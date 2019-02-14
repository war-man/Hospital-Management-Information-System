using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class BillServiceData
    {
        public int OPDNo { get; set; }

        public int DepartmentId { get; set; }
        public int ServiceId { get; set; }
        public double PayableAmount { get; set; }
        public double AwardAmount { get; set; }
        public String View { get; set; }
        public int Quantity { get; set; }
        public int WorkOrderTestId { get; set; }

    }
}