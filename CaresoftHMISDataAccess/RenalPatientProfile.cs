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
    
    public partial class RenalPatientProfile
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public System.DateTime ScreeningDate { get; set; }
        public int TreatmentNo { get; set; }
        public string Physician { get; set; }
        public string Access { get; set; }
        public string HIVHbs { get; set; }
        public string BloodGroup { get; set; }
        public int PreDialysisWeight { get; set; }
        public int TargetWeigh { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
