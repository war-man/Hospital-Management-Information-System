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
    
    public partial class Indent
    {
        public int PONO { get; set; }
        public Nullable<int> AcceptedPOId { get; set; }
        public Nullable<System.DateTime> IndentDate { get; set; }
        public string RefNo { get; set; }
        public Nullable<int> Supplier_Code { get; set; }
        public double FinancialYearId { get; set; }
        public string Mfg_Co_Name { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<bool> Process { get; set; }
        public string Username { get; set; }
        public Nullable<System.DateTime> TDateTime { get; set; }
        public string CompName { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public Nullable<int> companyid { get; set; }
        public string TermsCondition { get; set; }
        public Nullable<double> OtherDiscount { get; set; }
        public Nullable<double> CreditAmt { get; set; }
        public Nullable<bool> creditflag { get; set; }
        public Nullable<bool> Approvedflag { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<bool> FinalApproved { get; set; }
        public int IndentId { get; set; }
    
        public virtual AcceptedProvisionalPurchaseOrder AcceptedProvisionalPurchaseOrder { get; set; }
    }
}
