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
    
    public partial class HRClaimManagement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> ClaimNo { get; set; }
        public Nullable<int> RefNo { get; set; }
        public string TypeOfClaim { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<double> AmountOfClaim { get; set; }
        public string IdemnityIssuer { get; set; }
        public Nullable<System.DateTime> ExpectedDateOfReimbursement { get; set; }
        public string Comments { get; set; }
    }
}
