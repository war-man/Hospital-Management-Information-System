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
    
    public partial class HRUnionRegistration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> UnionId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public string StaffName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> UnionRegId { get; set; }
        public Nullable<double> SubscriptionFee { get; set; }
        public Nullable<double> RegistrationFee { get; set; }
        public string Remarks { get; set; }
        public string SubscriptionPeriodicity { get; set; }
    }
}