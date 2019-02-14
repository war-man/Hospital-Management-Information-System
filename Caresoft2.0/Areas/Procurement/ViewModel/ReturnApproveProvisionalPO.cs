using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class ReturnApproveProvisionalPO
    {
        public int ProvisionalPOId { get; set; }
        public string supplierName { get; set; }
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }

    }
}