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
    
    public partial class LocationMaster
    {
        public int LocationId { get; set; }
        public string LocName { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Wing { get; set; }
        public string username { get; set; }
        public string Financialyearid { get; set; }
        public Nullable<decimal> companyid { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
    }
}