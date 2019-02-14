using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class SaveAcceptProvisionlPO
    {
        public int ProvisionalNumber { get; set; }
        public int POItemDetailsNumber { get; set; }
        public double CostPerUnit { get; set; }
        public double TotalCost { get; set; }     
    }
}