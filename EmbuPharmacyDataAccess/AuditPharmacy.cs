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
    
    public partial class AuditPharmacy
    {
        public int regno { get; set; }
        public string Record_UniqueNo { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string Description { get; set; }
        public string Formname { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> DateOfRecord { get; set; }
        public string UserName { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
