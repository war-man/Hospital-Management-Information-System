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
    
    public partial class Tbl_IssueItem
    {
        public int IssID { get; set; }
        public Nullable<int> ReqItemId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> IssueQty { get; set; }
        public Nullable<int> RemainingIssueQty { get; set; }
        public string FinancialYearID { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
    }
}
