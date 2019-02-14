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
    
    public partial class MCHAntenatalProfile
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public string Hb { get; set; }
        public string BloodGroup { get; set; }
        public string Rhesus { get; set; }
        public string Serology { get; set; }
        public string TDScreening { get; set; }
        public Nullable<System.DateTime> IPTIsoniazidDateGiven { get; set; }
        public Nullable<System.DateTime> NextVisit { get; set; }
        public string HIV { get; set; }
        public string Urinalysis { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
        public virtual User User { get; set; }
    }
}
