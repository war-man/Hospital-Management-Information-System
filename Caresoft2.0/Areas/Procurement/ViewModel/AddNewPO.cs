using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class AddNewPO
    {
       public ProvisionalPurchaseOrder provisionalPurchaseOrder { get; set; }
       public int[] suppliers { get; set; }
       public ProvisionalPOItemsDetail provisionalPOItemsDetail { get; set; }
    }
}