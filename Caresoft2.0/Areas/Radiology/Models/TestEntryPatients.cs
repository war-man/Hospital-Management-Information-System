using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Radiology.Models
{
    public class TestEntryPatients
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PathNo { get; set; }
        public int LabNo { get; set; }
        public string PatientId { get; set; }
        public string BarCode { get; set; }
        public string PatientType { get; set; }
        public string FinancialYear { get; set; }
        public string AccessionStatusRegistered { get; set; } = "";
        public string AccessionStatusTested { get; set; } = "";
        public string AccessionStatusAuthorized { get; set; } = "";
        public string RefCustomer { get; set; }

        public string VialID { get; set; }
        public string TestCode { get; set; }
        public string DepartmentCode { get; set; }
        public string PSCNo { get; set; }
    }
}