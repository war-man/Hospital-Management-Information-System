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
    
    public partial class Tbl_Prescription
    {
        public int Id { get; set; }
        public Nullable<int> BillNo { get; set; }
        public byte[] Prescription { get; set; }
        public Nullable<decimal> CompanyId { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public Nullable<double> FinancialYearId { get; set; }
    }
}
