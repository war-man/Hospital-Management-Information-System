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
    
    public partial class HRStaffRegistration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Surname { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DateOfEmployment { get; set; }
        public string Salutation { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
    }
}
