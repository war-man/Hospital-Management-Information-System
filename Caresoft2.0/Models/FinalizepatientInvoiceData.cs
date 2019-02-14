using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Models
{
    public class FinalizepatientInvoiceData
    {
        public FinalizepatientInvoice FinalizepatientInvoice { get; set; }
    }

    public class FinalizepatientInvoice
    {
        public List<AllocatedData> AllocationArray { get; set; }
        public int CompanyId { get; set; }
        public bool PreOuth { get; set; }
        public int OPDId { get; set; }
        public int NHIFRebate { get; set; }
        public int TotalAmount { get; set; }
        public int NHIFDiff { get; set; }
    }

    public class AllocatedData
    {
        public int BSID { get; set; }
        public int AllocatedAmount { get; set; }
        public string Type { get; set; }

    }
}