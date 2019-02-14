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
    
    public partial class RenalMachineCheck
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public System.TimeSpan BloodLeak { get; set; }
        public System.TimeSpan AirDetect { get; set; }
        public int Temperature { get; set; }
        public string Conductivity { get; set; }
        public string DialysisP { get; set; }
        public string TMP { get; set; }
        public string RejRAte { get; set; }
        public string Staff { get; set; }
        public System.TimeSpan TimeDue { get; set; }
        public string InitiatingNurse { get; set; }
        public int InitialClamps { get; set; }
        public System.TimeSpan TimeOff { get; set; }
        public string TerminatingNurse { get; set; }
        public int FinalClamps { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
