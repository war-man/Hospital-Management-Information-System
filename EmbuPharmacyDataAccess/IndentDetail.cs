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
    
    public partial class IndentDetail
    {
        public int IndentDetailId { get; set; }
        public string PONO { get; set; }
        public Nullable<double> QtyRequired { get; set; }
        public double FinancialYearID { get; set; }
        public Nullable<bool> Itemwise_InvoiceFlag { get; set; }
        public Nullable<int> DrugId { get; set; }
        public Nullable<int> Supplier_Code { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<int> Per { get; set; }
        public Nullable<decimal> ReceivedQty { get; set; }
        public string mfgcompany { get; set; }
        public string Username { get; set; }
        public string IndentDetail1 { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public Nullable<int> companyid { get; set; }
        public Nullable<int> FreeQty { get; set; }
        public Nullable<double> MRPUnit { get; set; }
        public Nullable<double> MRP { get; set; }
        public Nullable<double> Disc { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<double> TotalAmt { get; set; }
        public Nullable<bool> IsApprove { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> Approvedate { get; set; }
    }
}
