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
    
    public partial class NurseReturnLinenToHk
    {
        public int RetrunId { get; set; }
        public string DeptId { get; set; }
        public string StoreFlag { get; set; }
        public string IssuetoFlag { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string UserName { get; set; }
        public string NurseName { get; set; }
        public Nullable<System.DateTime> TDatetime { get; set; }
        public Nullable<int> DrugId { get; set; }
        public Nullable<int> ReturnQty { get; set; }
    }
}