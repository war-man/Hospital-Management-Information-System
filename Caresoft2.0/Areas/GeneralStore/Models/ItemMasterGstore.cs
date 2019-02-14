using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.GeneralStore.Models
{
    public class ItemMasterGstore
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string MfgCoName { get; set; }
        public string GenericDrugName { get; set; }
        public string BatchNo { get; set; }
        public string Supplier { get; set; }
        public string Category { get; set; }
        public int CurrentStock { get; set; }
        public string MfgDate { get; set; }
        public int ReorderLevel { get; set; }
        public string ICDTenCode { get; set; }
        public string barCode { get; set; }
        public string CasePackRate { get; set; }
        public string CostPriceUnit { get; set; }
        public string SellingPriceUnit { get; set; }
        public string ExpiryStatus { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string PurchaseDate { get; set; }
        public string AssetStatus { get; set; }
        public string WarantyExpiryDate { get; set; }
        public string DateDisposed { get; set; }
        public string DateCommisioned { get; set; }
        public int UnitsPack { get; set; }
    }
}