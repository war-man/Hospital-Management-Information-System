using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.ViewModel
{
    public class InvoiceAndInvoiceDetails
    {
        public Invoice Invoice { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}