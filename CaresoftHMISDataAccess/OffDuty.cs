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
    
    public partial class OffDuty
    {
        public int Id { get; set; }
        public int OPDno { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string DrName { get; set; }
        public int Days { get; set; }
        public Nullable<System.DateTime> From { get; set; }
        public Nullable<System.DateTime> To { get; set; }
        public string Reason1 { get; set; }
        public string Reason2 { get; set; }
        public string Reason3 { get; set; }
        public string Reason4 { get; set; }
        public string DaysApproved { get; set; }
        public string ProgrammeCoordinator { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
