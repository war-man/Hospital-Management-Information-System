using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class SaveInvoiceMain
    {
        public double Amount { get; set; }
        public double vatAmount { get; set; }
        public double discount { get; set; }
        public double other { get; set; }
        public double totalAmount { get; set; }
        public double amountPage { get; set; }
        public double GrandTotal { get; set; }

    }
}