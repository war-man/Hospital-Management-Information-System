//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaresoftHMISDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class BillAdjustmentLog
    {
        public int Id { get; set; }
        public Nullable<int> BillServiceId { get; set; }
        public Nullable<int> MedicationId { get; set; }
        public int InitialQty { get; set; }
        public int FinalQty { get; set; }
        public double InitialPrice { get; set; }
        public double FinalPrice { get; set; }
        public double InitialAward { get; set; }
        public double FinalAward { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<int> WalkinMediCationId { get; set; }
    
        public virtual BillService BillService { get; set; }
        public virtual Medication Medication { get; set; }
        public virtual User User { get; set; }
    }
}
