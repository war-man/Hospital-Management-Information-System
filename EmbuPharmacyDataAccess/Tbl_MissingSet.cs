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
    
    public partial class Tbl_MissingSet
    {
        public int MissingID { get; set; }
        public Nullable<int> PackageID { get; set; }
        public Nullable<int> SubPackID { get; set; }
        public Nullable<bool> MissingFlag { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> TDate { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public string FinancialYearID { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string Username { get; set; }
    }
}
