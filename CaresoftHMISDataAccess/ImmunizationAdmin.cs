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
    
    public partial class ImmunizationAdmin
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public int ImmunizationMasterId { get; set; }
        public System.DateTime DateGiven { get; set; }
        public Nullable<System.DateTime> DateOfNextVisit { get; set; }
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<int> AgeCategory { get; set; }
    
        public virtual ImmunizationMaster ImmunizationMaster { get; set; }
        public virtual OpdRegister OpdRegister { get; set; }
        public virtual User User { get; set; }
    }
}
