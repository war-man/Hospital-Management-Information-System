using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caresoft2._0.Areas.Procurement.Models;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class ProvisionalPurchaseOrderViewModel
    {
        public IEnumerable<supplier> Suppliers { get; set; }
        public IEnumerable<ProvisionalPOItemsDetail> ProvisionalPOItemsDetail { get; set; }
    }
}