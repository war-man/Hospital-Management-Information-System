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
    
    public partial class LeaveLetter
    {
        public int Id { get; set; }
        public int OPDno { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string CertNo { get; set; }
        public string DrName { get; set; }
        public string PatientName { get; set; }
        public string Illness { get; set; }
        public Nullable<System.DateTime> AbsenceStart { get; set; }
        public Nullable<System.DateTime> AbsenceEnd { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> EmployeeId { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
