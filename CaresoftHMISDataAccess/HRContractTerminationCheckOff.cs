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
    
    public partial class HRContractTerminationCheckOff
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<double> LoansBalances { get; set; }
        public Nullable<double> SaccoBalances { get; set; }
        public Nullable<double> CheckOffSystemBalance { get; set; }
        public Nullable<double> HRSBalance { get; set; }
        public Nullable<double> AdvanceBalance { get; set; }
        public Nullable<double> TrainingAllowanceBal { get; set; }
        public Nullable<double> Penalties { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<double> TelExpenses { get; set; }
        public string AdvancedItems { get; set; }
        public string Interests { get; set; }
        public string Comments { get; set; }
    }
}
