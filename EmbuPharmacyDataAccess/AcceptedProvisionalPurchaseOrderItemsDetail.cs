//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmbuPharmacyDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class AcceptedProvisionalPurchaseOrderItemsDetail
    {
        public int AcceptedProvisionalPOItemsDetailId { get; set; }
        public int AcceptedProvisionalPOId { get; set; }
        public int DrugId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> CostPerUnit { get; set; }
        public Nullable<double> TotalCost { get; set; }
        public double FinancialYearId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public string Username { get; set; }
        public Nullable<bool> AcceptFlag { get; set; }
        public Nullable<int> NoofPack { get; set; }
        public Nullable<int> CostPerCasePack { get; set; }
    
        public virtual AcceptedProvisionalPurchaseOrder AcceptedProvisionalPurchaseOrder { get; set; }
    }
}