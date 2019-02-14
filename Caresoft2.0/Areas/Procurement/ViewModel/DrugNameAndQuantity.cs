using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    //this view model is for retrieving drugs stored in a provisional purchase order
    public class DrugNameAndQuantity
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
}