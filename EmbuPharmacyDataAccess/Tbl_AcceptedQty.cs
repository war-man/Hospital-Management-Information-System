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
    
    public partial class Tbl_AcceptedQty
    {
        public int AccID { get; set; }
        public Nullable<int> ReqID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> AccQty { get; set; }
        public string Status { get; set; }
        public string FinancialYearID { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string Username { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public Nullable<System.DateTime> TDate { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<int> NurseStationID { get; set; }
    }
}
