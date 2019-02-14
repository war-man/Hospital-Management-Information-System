using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class DCBIReportData
    {
        public String DepartmentName { get; set; }
        public String ServiceName { get; set; }
        public double ActualAmount { get; set; }
        public double Waiver { get; set; }
        public int WaiverNo { get; set; }
        public double Award { get; set; }
        public DateTime TimeAdded { get; set; }
        public int Receipt { get; set; }
        public DateTime TimePaid { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public int OPDNo { get; set; }
        public String PatType { get; set; }

    }
}