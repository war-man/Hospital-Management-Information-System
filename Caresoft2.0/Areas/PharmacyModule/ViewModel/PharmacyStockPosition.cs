using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.PharmacyModule.ViewModel
{
    public class PharmacyStockPosition
    {
        public int Id { get; set; }
        public int OpeningStock { get; set; }
        public int ClosingStock { get; set; }
        public string ItemName { get; set; }
        public int TotalPurchaseQuantity { get; set; }
        public int TotalIssueQuantity { get; set; }
        public int CurrentStock { get; set; }
        public decimal CostPrice { get; set; }
        public int ReOrderLevel { get; set; }
        public double TotalAmountSold { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? Date { get; set; }
    }
}