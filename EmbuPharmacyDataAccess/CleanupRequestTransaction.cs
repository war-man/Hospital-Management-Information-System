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
    
    public partial class CleanupRequestTransaction
    {
        public int TransId { get; set; }
        public Nullable<int> RequestId { get; set; }
        public string pepatid { get; set; }
        public Nullable<int> PNo { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public string Building { get; set; }
        public string FloorNo { get; set; }
        public string RoomNo { get; set; }
        public Nullable<int> BedNo { get; set; }
        public Nullable<int> companyid { get; set; }
        public Nullable<decimal> FinancialYearId { get; set; }
        public string UserName { get; set; }
    
        public virtual CleanupRequest CleanupRequest { get; set; }
    }
}
