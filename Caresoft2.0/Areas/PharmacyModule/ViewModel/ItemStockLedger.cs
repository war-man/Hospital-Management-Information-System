using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.PharmacyModule.ViewModel
{
    public class ItemStockLedger
    {
        public int Id { get; set; }
        public int OpeningStock { get; set; }
        public int ClosingStock { get; set; }
        public string ItemName { get; set; }
        public string BatchNo { get; set; }
        public DateTime Date { get; set; }
        public int PurchaseQuantity { get; set; }
        public int IssueQuantity { get; set; }
        
    }
}