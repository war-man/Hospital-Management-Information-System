using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.PharmacyModule.Reports.DrugWiseReport
{
    public class DrugWiseViewModel
    {
        public string DrugName { get; set; }
        public string BatchNo { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string IssueTo { get; set; }
        public int Quantity { get; set; }
        public int CurrentStock { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public double ProfitAmount { get; set; }
        public double Profit { get; set; }

    }
}