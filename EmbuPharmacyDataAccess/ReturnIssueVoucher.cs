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
    
    public partial class ReturnIssueVoucher
    {
        public int ReturnId { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<int> Billno { get; set; }
        public Nullable<bool> Final_ReturnStatus { get; set; }
        public string StoreFlag { get; set; }
        public string IssuetoFlag { get; set; }
        public Nullable<int> companyid { get; set; }
        public Nullable<double> FinancialYearId { get; set; }
    }
}