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
    
    public partial class VatMaster
    {
        public int VatID { get; set; }
        public Nullable<int> Supplier_Code { get; set; }
        public string Supplier_name { get; set; }
        public Nullable<int> DrugID { get; set; }
        public string ItemName { get; set; }
        public string VatType { get; set; }
        public Nullable<double> VatAmt { get; set; }
        public Nullable<int> CompID { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public Nullable<bool> IsCST { get; set; }
        public Nullable<double> CSTamt { get; set; }
    }
}
