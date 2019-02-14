using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class CompareProvisionalOrder
    {
        public IEnumerable<ProvisionalPOItemsDetail> provisionalPOItemsDetail  { get; set; }
        public string Supplier { get; set; }
    }
}