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
    
    public partial class PreventiveMiantenance
    {
        public int PMno { get; set; }
        public string PMtaskname { get; set; }
        public string Remark { get; set; }
        public Nullable<int> FileId { get; set; }
        public string FilePath { get; set; }
        public Nullable<int> Labourhour { get; set; }
        public Nullable<int> companyid { get; set; }
        public string Username { get; set; }
        public string FinancialyearId { get; set; }
        public Nullable<System.DateTime> PMDate { get; set; }
        public string Modelno { get; set; }
    }
}
