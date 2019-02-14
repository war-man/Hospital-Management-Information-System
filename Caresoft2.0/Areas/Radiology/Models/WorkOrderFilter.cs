using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Radiology.Models
{
    public class WorkOrderFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PathNo { get; set; }
        public string LabNo { get; set; }
        public int PatientId { get; set; }
        public string BarCode { get; set; }
        public string PatientType { get; set; }
        public string FinancialYear { get; set; }
        public string AccessionStatus { get; set; }
        public string RefCustomer { get; set; }
    }
}