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
    
    public partial class Tbl_ItemMasterLF
    {
        public int TbID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TDateTime { get; set; }
        public string IPAddresss { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public Nullable<int> Companyid { get; set; }
        public string Action { get; set; }
        public string Batchno { get; set; }
        public Nullable<int> CurrentStock { get; set; }
        public Nullable<double> CostPrice { get; set; }
        public Nullable<double> MRP { get; set; }
    }
}
